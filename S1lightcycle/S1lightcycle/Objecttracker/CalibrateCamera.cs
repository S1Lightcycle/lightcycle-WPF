using OpenCvSharp;
using OpenCvSharp.CPlusPlus;
using System;
using System.Threading;

namespace S1lightcycle.Objecttracker {
    public class CalibrateCamera {
        private VideoCapture _capture;
        private Mat _frame;
        private const int CaptureWidthProperty = 3;
        private const int CaptureHeightProperty = 4;
        public int CamResolutionWidth = 1280;
        public int CamResolutionHeight = 720;
        private int _countClicks = 0;
        private CvWindow _cvFrame;
        private IplImage _srcImg;
        private static CalibrateCamera _instance;

        public int RoiWidth
        {
            get
            {
                if (Properties.Settings.Default.x2 > Properties.Settings.Default.x1)
                {
                    return Properties.Settings.Default.x2 - Properties.Settings.Default.x1;
                }
                return Properties.Settings.Default.x1 - Properties.Settings.Default.x2;
            }
        }

        public int RoiHeight
        {
            get
            {
                if (Properties.Settings.Default.y2 > Properties.Settings.Default.y1)
                {
                    return Properties.Settings.Default.y2 - Properties.Settings.Default.y1;
                }
                return Properties.Settings.Default.y1 - Properties.Settings.Default.y2;
            }
        }

        public VideoCapture GetVideoCapture() {
            return _capture;
        }

        public CvPoint[] CalibrationPoints
        {
            get
            {
                CvPoint point1 = new CvPoint(Properties.Settings.Default.x1, Properties.Settings.Default.y1);
                CvPoint point2 = new CvPoint(Properties.Settings.Default.x2, Properties.Settings.Default.y2);

                if (point1.X < point2.X)
                {
                    return new[] {point1, point2};
                }
                return new[] {point2, point1};
            }
        }

        public static CalibrateCamera GetInstance() {
            if (_instance == null) _instance = new CalibrateCamera();
            return _instance;
        }

        private CalibrateCamera() {
            _capture = new VideoCapture(0);
            _capture.Set(CaptureWidthProperty, CamResolutionWidth);
            _capture.Set(CaptureHeightProperty, CamResolutionHeight);
        }

        public void ShowFrame() {
            
            _frame = new Mat();

            //get new _frame from camera
            _capture.Read(_frame);
            
            //_frame height == 0 => camera hasn't been initialized properly and provides garbage data
            while (_frame.Height == 0) {
                _capture.Read(_frame);
            }
            for (int i = 0; i < 5; i++) {
                _capture.Read(_frame);
                Thread.Sleep(500);
            }
            
            _srcImg = _frame.ToIplImage();
            _cvFrame = new CvWindow("edge calibration editor", WindowMode.Fullscreen, _srcImg);
            _cvFrame.OnMouseCallback += new CvMouseCallback(OnMouseDown);
            
        }

        public void OnMouseDown(MouseEvent me, int x, int y, MouseEvent me2) {
            if (me == MouseEvent.LButtonDown) {
                if (_countClicks > 1) _countClicks = 0;
                
                Console.WriteLine((_countClicks + 1) + ". Coordinate:");
                Console.Write("x-coord: " + x + ", ");
                Console.WriteLine("y-coord: " + y);

                CvPoint point = new CvPoint(x, y);
                Cv.Circle(_srcImg, point, 10, new CvColor(255, 0, 0), 5);
                if (_countClicks == 0) {
                    Properties.Settings.Default.x1 = x;
                    Properties.Settings.Default.y1 = y;
                } else if (_countClicks == 1) {
                    Properties.Settings.Default.x2 = x;
                    Properties.Settings.Default.y2 = y;
                }
                _countClicks++;
                Properties.Settings.Default.Save();
                _cvFrame.Image = _srcImg;

                if (_countClicks > 1) {
                    _cvFrame.Close();
                    
                }
            }
        }
    }
}
