using System;
using System.ComponentModel;
using System.Windows;

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
    }
}
