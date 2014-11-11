using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
