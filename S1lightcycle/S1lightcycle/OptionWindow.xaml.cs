using System;
using System.ComponentModel;
using System.Windows;

namespace S1lightcycle {
    /// <summary>
    /// Interaction logic for OptionWindow.xaml
    /// </summary>
    public partial class OptionWindow : Window {
        public OptionWindow() {
            InitializeComponent();
            this.LearningRate.Text = Properties.Settings.Default.LearningRate.ToString();
            this.MinBlobSize.Text = Properties.Settings.Default.MinBlobSize.ToString();
            this.MaxBlobsize.Text = Properties.Settings.Default.MaxBlobSize.ToString();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            double learningRate = 0;
            if ((LearningRate.Text != "") && (Double.TryParse(LearningRate.Text, out learningRate)))
            {
                Properties.Settings.Default.LearningRate = learningRate;
            }

            int minBlobSize = 0;
            if ((MinBlobSize.Text != "") && (Int32.TryParse(MinBlobSize.Text, out minBlobSize)))
            {
                Properties.Settings.Default.MinBlobSize = minBlobSize;
            }

            int maxBlobSize = 0;
            if ((MaxBlobsize.Text != "") && (Int32.TryParse(MaxBlobsize.Text, out maxBlobSize)))
            {
                Properties.Settings.Default.MaxBlobSize = maxBlobSize;
            }
            Properties.Settings.Default.Save();
            base.OnClosing(e);
        }
    }
}
