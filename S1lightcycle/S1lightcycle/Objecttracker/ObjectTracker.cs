using OpenCvSharp.CPlusPlus;
using OpenCvSharp;
using OpenCvSharp.Blob;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System;

namespace S1lightcycle.Objecttracker
{
    public class ObjectTracker : AbstractObjectTracker
    {
        private readonly VideoCapture _capture;
        private readonly CvWindow _blobWindow;
        private readonly BackgroundSubtractor _subtractor;
        private Mat _frame;
        private CvBlobs _blobs;
        private CvPoint _oldFirstCar;
        private CvPoint _oldSecondCar;
        private bool _isTracking;
        private const int CaptureWidthProperty = 3;
        private const int CaptureHeightProperty = 4;
        public int CamResolutionWidth = 1280;
        public int CamResolutionHeight = 720;
        private readonly static object Lock = new object();

        private bool _arePlayersInitialized;

        private Thread _trackingThread;

        public ObjectTracker() {
            //webcam
            _capture = new VideoCapture(0);

            //setting _capture resolution
            _capture.Set(CaptureWidthProperty, CamResolutionWidth);
            _capture.Set(CaptureHeightProperty, CamResolutionHeight);

            _blobWindow = new CvWindow("_blobs");

            //Background _subtractor, alternatives: MOG, GMG
            _subtractor = new BackgroundSubtractorMOG2();

            _oldFirstCar = CvPoint.Empty;
            FirstCar = new Robot(-1, -1);
            SecondCar = new Robot(-1, -1);

            _arePlayersInitialized = false;
        }

        public override void StartTracking() {
            _oldFirstCar = CvPoint.Empty;
            _oldSecondCar = CvPoint.Empty;
            _trackingThread = new Thread(Track);
            _isTracking = true;
            _trackingThread.Priority = ThreadPriority.Highest;
            _trackingThread.Start();
        }

        public override void StopTracking() {
            _isTracking = false;
            _trackingThread.Abort();
        }

        public override void Track()
        {
            CvWindow roiWindow = new CvWindow("roi");
            int roiHeight = Properties.Settings.Default.RoiHeight;
            int roiWidth = Properties.Settings.Default.RoiWidth;
            while (_isTracking) {
                _frame = new Mat();

                //get new _frame from camera
                _capture.Read(_frame);

                //_frame height == 0 => camera hasn't been initialized properly and provides garbage data
                while (_frame.Height == 0)
                {
                    _capture.Read(_frame);
                }
                
                Mat sub = new Mat();

                //camera calibration - ROI
                Coordinate roiBase = new Coordinate(Properties.Settings.Default.CalibrationPointX, Properties.Settings.Default.CalibrationPointY);
                CvRect roiRect = new CvRect(roiBase.XCoord, roiBase.YCoord, roiWidth, roiHeight);
                Mat srcRoi = _frame.Clone(roiRect);

                IplImage tmpImg = srcRoi.ToIplImage().Clone();
                roiWindow.ShowImage(tmpImg);

                //perform background subtraction with selected _subtractor.
                _subtractor.Run(srcRoi, sub, LearningRate);
                IplImage src = (IplImage)sub;

                //binarize image
                Cv.Threshold(src, src, 250, 255, ThresholdType.Binary);

                IplConvKernel element = Cv.CreateStructuringElementEx(4, 4, 0, 0, ElementShape.Rect, null);
                Cv.Erode(src, src, element, 1);
                Cv.Dilate(src, src, element, 1);
                _blobs = new CvBlobs();
                _blobs.Label(src);

                ComputeMean();

                //PrintBlobs(blobList);
                _blobs.FilterByArea(BlobMinSize, BlobMaxSize);
                var blobList = SortBlobsBySize(_blobs);

                CvBlob largest = null;
                CvBlob secondLargest = null;

                CvBlobs blobs = _blobs.Clone();

                if (blobList.Count >= 1)
                {
                    largest = blobList[0];
                    blobs.FilterByLabel(largest.Label);
                }

                if (blobList.Count >= 2)
                {
                    secondLargest = blobList[1];
                    _blobs.FilterByLabel(secondLargest.Label);
                }

                IplImage render = new IplImage(src.Size, BitDepth.U8, 3);
                
                _blobs.RenderBlobs(src, render);
                blobs.RenderBlobs(render, render);
                //_blobWindow.ShowImage(render);

                Cv2.WaitKey(1);
                if ((largest != null) && (secondLargest != null))
                {
                    LinearPrediction(largest, secondLargest);
                } else if ((largest != null) && (secondLargest == null))
                {
                    //LinearPrediction(largest);
                }
            }
        }

        private void ComputeMean()
        {
            if (_blobs.Count > 0)
            {
                ulong mean = 0;
                foreach (CvBlob blob in _blobs.Values)
                {
                    mean = mean + (ulong) blob.Area;
                }
                if (Properties.Settings.Default.BlobMean > 0)
                {
                    //add the previous mean as one blob to the current mean
                    mean += Properties.Settings.Default.BlobMean;
                    Properties.Settings.Default.BlobMean = mean/(ulong) _blobs.Count + 1;
                }
                else
                {
                    Properties.Settings.Default.BlobMean = mean / (ulong)_blobs.Count;
                }
                Properties.Settings.Default.Save();
            }
        }

        private void PrintBlobs(List<CvBlob> blobs) {
            Console.WriteLine("Blob list...");
            foreach (CvBlob blob in blobs) {
                
                Console.WriteLine("Blob size: " + Math.Abs(blob.MaxX - blob.MinX) * Math.Abs(blob.MaxY - blob.MinY));
            }
        }

        /// <summary>
        /// Compares the distance between the largest blob of the last cycle and the current largest and second largest blob.
        /// If the distance between the last largest and current largest is shorter than between the last largest and second largest 
        /// it returns the current largest as first element, otherwise it returns the second largest as second element
        /// </summary>
        /// <param name="largest">Largest detected blob</param>
        /// <param name="secondLargest">Second largest detected blob</param>
        private void LinearPrediction(CvBlob largest, CvBlob secondLargest)
        {


            if (largest != null)
            {
                CvPoint largestCenter = largest.CalcCentroid();
                CvPoint secondCenter = secondLargest.CalcCentroid();

                if (!_arePlayersInitialized)
                {
                    int roiWidth = Properties.Settings.Default.RoiWidth;
                    if (largest.MaxX < roiWidth/2)
                    {
                        EnqueuePlayers(new Coordinate(largestCenter), new Coordinate(secondCenter));
                    }
                    else
                    {
                        EnqueuePlayers(new Coordinate(secondCenter), new Coordinate(largestCenter));
                    }
                    _arePlayersInitialized = true;
                    return;
                }
                double distanceRobots = largestCenter.DistanceTo(secondCenter);
                
                if (distanceRobots <= 80) {
                    if (_oldFirstCar.DistanceTo(largestCenter) < _oldSecondCar.DistanceTo(largestCenter)){
                        EnqueuePlayers(new Coordinate(largestCenter), null);
                    } else {
                        EnqueuePlayers(null, new Coordinate(largestCenter));
                    }
                    System.Console.WriteLine("distance between robots: " + distanceRobots);    
                    return;
                }
                /*Console.WriteLine("distance oldfirstcar to largest center: " +  _oldFirstCar.DistanceTo(largestCenter));
                Console.WriteLine("distance oldfirstcar to small center: " + _oldFirstCar.DistanceTo(secondCenter));

                Console.WriteLine("distance oldsecondcar to largest center: " + _oldSecondCar.DistanceTo(largestCenter));
                Console.WriteLine("distance oldsecondcar to small center: " + _oldSecondCar.DistanceTo(secondCenter));*/

                if ((_oldFirstCar == CvPoint.Empty) || 
                    ((_oldFirstCar.DistanceTo(largestCenter) < _oldFirstCar.DistanceTo(secondCenter)) && 
                    _oldSecondCar.DistanceTo(largestCenter) > _oldSecondCar.DistanceTo(secondCenter)))
                {
                    
                    
                    FirstCar.Width = CalculateDiameter(largest.MaxX, largest.MinX);
                    FirstCar.Height = CalculateDiameter(largest.MaxY, largest.MinY);

                    SecondCar.Width = CalculateDiameter(secondLargest.MaxX, secondLargest.MinX);
                    SecondCar.Height = CalculateDiameter(secondLargest.MaxY, secondLargest.MinY);

                    EnqueuePlayers(new Coordinate(largestCenter), new Coordinate(secondCenter));
                }
                else
                {

                    SecondCar.Width = CalculateDiameter(largest.MaxX, largest.MinX);
                    SecondCar.Height = CalculateDiameter(largest.MaxY, largest.MinY);

                    FirstCar.Width = CalculateDiameter(secondLargest.MaxX, secondLargest.MinX);
                    FirstCar.Height = CalculateDiameter(secondLargest.MaxY, secondLargest.MinY);

                    EnqueuePlayers(new Coordinate(secondCenter), new Coordinate(largestCenter));
                }
            }
        }

        private void LinearPrediction(CvBlob blob)
        {
            CvPoint center = blob.CalcCentroid();

            if (_oldFirstCar.DistanceTo(center) < _oldSecondCar.DistanceTo(center))
            {
                EnqueuePlayers(new Coordinate(center), null);
            }
            else
            {
                EnqueuePlayers(null, new Coordinate(center));
            }
        }

        private void EnqueuePlayers(Coordinate firstPlayer, Coordinate secondPlayer)
        {
            lock(Lock)
            {
                if (firstPlayer != null)
                {
                    FirstCar.Coord.Enqueue(firstPlayer);
                    _oldFirstCar = new CvPoint(firstPlayer.XCoord, firstPlayer.YCoord);
                }

                if (secondPlayer != null)
                {
                    SecondCar.Coord.Enqueue(secondPlayer);
                    _oldSecondCar = new CvPoint(secondPlayer.XCoord, secondPlayer.YCoord);
                }
            }
        }

        private int CalculateDiameter(int max, int min)
        {
            return max - min;
        }

        private List<CvBlob> SortBlobsBySize(CvBlobs blobs)
        {
            List<CvBlob> blobList = new List<CvBlob>();

            foreach(CvBlob blob in blobs.Values)
            {
                blobList.Add(blob);
            }

            return blobList.OrderByDescending(x => x.Area).ToList();
        }
    }
}
