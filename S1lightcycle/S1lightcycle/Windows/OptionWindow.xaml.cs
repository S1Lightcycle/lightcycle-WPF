using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using S1LightcycleNET;

namespace S1lightcycle.Windows {
    /// <summary>
    /// Interaction logic for OptionWindow.xaml
    /// </summary>
    public partial class OptionWindow : Window {
        public OptionWindow() {
            InitializeComponent();
            this.LearningRateBox.Text = Properties.Settings.Default.LearningRate.ToString();
            this.MinBlobSizeBox.Text = Properties.Settings.Default.MinBlobSize.ToString();
            this.MaxBlobSizeBox.Text = Properties.Settings.Default.MaxBlobSize.ToString();
            foreach (String name in Controller.Instance.GetSerialPorts())
            {
                cmbSerialPort.Items.Add(name);
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            double learningRate = 0;
            if ((LearningRateBox.Text != "") && (Double.TryParse(LearningRateBox.Text, out learningRate)))
            {
                Properties.Settings.Default.LearningRate = learningRate;
            }

            int minBlobSize = 0;
            if ((MinBlobSizeBox.Text != "") && (Int32.TryParse(MinBlobSizeBox.Text, out minBlobSize)))
            {
                Properties.Settings.Default.MinBlobSize = minBlobSize;
            }

            int maxBlobSize = 0;
            if ((MaxBlobSizeBox.Text != "") && (Int32.TryParse(MaxBlobSizeBox.Text, out maxBlobSize)))
            {
                Properties.Settings.Default.MaxBlobSize = maxBlobSize;
            }
            Properties.Settings.Default.Save();
            base.OnClosing(e);
        }

        private void OK_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void cmbSerialPort_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Controller.Instance.SetSerialPort((String)cmbSerialPort.SelectedItem);
        }

        private void btn_calibrate_Click(object sender, RoutedEventArgs e)
        {
            /*
            Window calibrationWindow = new Window();
            calibrationWindow.Background = Brushes.Red;
            calibrationWindow.WindowState = WindowState.Maximized;
            calibrationWindow.WindowStyle = WindowStyle.None;
            calibrationWindow.Show();
            calibrationWindow.KeyDown += calibrationWindow_KeyDown;
            calibrationWindow.MouseDown += calibrationWindow_MouseDown;
            */

            CalibrateCamera cal = new CalibrateCamera();
            

            //Calibrater cal = new Calibrater();
            //cal.Calibrate(); 
        }

        void calibrationWindow_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            CloseCalibrationWindow((Window)sender);
        }

        void calibrationWindow_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            CloseCalibrationWindow((Window)sender);
        }

       

        private void CloseCalibrationWindow(Window calibrationWindow)
        {            
            calibrationWindow.Close();
        }
    }
}
