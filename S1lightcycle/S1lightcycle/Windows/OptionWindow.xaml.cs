using System;
using System.ComponentModel;
using System.Windows;

namespace S1lightcycle.Windows {
    /// <summary>
    /// Interaction logic for OptionWindow.xaml
    /// </summary>
    public partial class OptionWindow : Window 
    {
        public OptionWindow() {
            InitializeComponent();
            LearningRateBox.Text = Properties.Settings.Default.LearningRate.ToString();
            MinBlobSizeBox.Text = Properties.Settings.Default.MinBlobSize.ToString();
            MaxBlobSizeBox.Text = Properties.Settings.Default.MaxBlobSize.ToString();
            lblBlobMean.Content = Properties.Settings.Default.BlobMean.ToString();
            foreach (String name in Controller.Instance.GetSerialPorts())
            {
                cmbSerialPort.Items.Add(name);
                if (name.Equals(Properties.Settings.Default.ComPort))
                {
                    cmbSerialPort.SelectedItem = name;
                }
            }
            Controller.Instance.SetSerialPort((String)cmbSerialPort.SelectedItem);  
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

            Properties.Settings.Default.ComPort = cmbSerialPort.Text;

            Properties.Settings.Default.Save();
            base.OnClosing(e);
        }


        private void Calibration_Button_Click(object sender, RoutedEventArgs e) 
        {
            new EdgeConfigWindow().Show();
            Hide();
        }
        private void OK_Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void cmbSerialPort_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Controller.Instance.SetSerialPort((String)cmbSerialPort.SelectedItem);
        }

        private void BlobMeanResetButton_OnClick(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.BlobMean = 0;
            Properties.Settings.Default.Save();
            lblBlobMean.Content = 0;
        }
    }
}
