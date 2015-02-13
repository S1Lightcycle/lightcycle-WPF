using OpenCvSharp;
using OpenCvSharp.CPlusPlus;
using System;
using System.Threading;

namespace S1lightcycle.Objecttracker
{
    public class CameraCalibrator
    {
        private VideoCapture _capture;
        private Mat _frame;
        private const int CaptureWidthProperty = 3;
        private const int CaptureHeightProperty = 4;
        public int CamResolutionWidth = 1280;
        public int CamResolutionHeight = 720;
        private int _countClicks = 0;
        private CvWindow _cvFrame;
        private IplImage _srcImg;
        private int x1;
        private int x2;
        private int y1;
        private int y2;

        public VideoCapture GetVideoCapture()
        {
            return _capture;
        }

        public CameraCalibrator()
        {
            _capture = new VideoCapture(0);
            _capture.Set(CaptureWidthProperty, CamResolutionWidth);
            _capture.Set(CaptureHeightProperty, CamResolutionHeight);
        }

        public void ShowFrame()
        {

            _frame = new Mat();

            //get new _frame from camera
            _capture.Read(_frame);

            //_frame height == 0 => camera hasn't been initialized properly and provides garbage data
            while (_frame.Height == 0 || _frame.Width == 0)
            {
                _capture.Read(_frame);
            }
            for (int i = 0; i < 5; i++)
            {
                _capture.Read(_frame);
                Thread.Sleep(500);
            }
           
            _srcImg = _frame.ToIplImage();
            _cvFrame = new CvWindow("edge calibration editor", WindowMode.Fullscreen, _srcImg);
            
            _cvFrame.OnMouseCallback += OnMouseDown;

        }

        public void OnMouseDown(MouseEvent me, int x, int y, MouseEvent me2)
        {
            if (me == MouseEvent.LButtonDown)
            {
                if (_countClicks > 1) _countClicks = 0;

                Console.WriteLine((_countClicks + 1) + ". Coordinate:");
                Console.Write("x-coord: " + x + ", ");
                Console.WriteLine("y-coord: " + y);

                CvPoint point = new CvPoint(x, y);
                Cv.Circle(_srcImg, point, 10, new CvColor(255, 0, 0), 5);
                if (_countClicks == 0)
                {
                    x1 = x;
                    y1 = y;
                }
                else if (_countClicks == 1)
                {
                    x2 = x;
                    y2 = y;

                    SaveRoi();

                }
                _countClicks++;
                _cvFrame.Image = _srcImg;

                if (_countClicks > 1)
                {
                    _cvFrame.Close();
                }
            }
        }

        private void SaveRoi()
        {
            //Set RoiWidth
            if (x2 > x1)
            {
                Properties.Settings.Default.RoiWidth = x2 - x1;
            }
            else
            {
                Properties.Settings.Default.RoiWidth = x1 - x2;
            }

            //Set RoiHeight
            if (y2 > y1)
            {
                Properties.Settings.Default.RoiHeight = y2 - y1;
            }
            else
            {
                Properties.Settings.Default.RoiHeight = y1 - y2;
            }

            //Set CalibrationPoint
            if (x1 < x2)
            {
                Properties.Settings.Default.CalibrationPointX = x1;
                Properties.Settings.Default.CalibrationPointY = y1;
            }
            else
            {
                Properties.Settings.Default.CalibrationPointX = x2;
                Properties.Settings.Default.CalibrationPointY = y2;
            }

            Properties.Settings.Default.Save();
        }
    }
}
