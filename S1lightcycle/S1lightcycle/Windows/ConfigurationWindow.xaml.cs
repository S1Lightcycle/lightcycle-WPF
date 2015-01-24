using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace S1lightcycle.Windows
{
    /// <summary>
    /// Interaktionslogik für ConfigurationWindow.xaml
    /// </summary>
    public partial class ConfigurationWindow : Window
    {

        private DispatcherTimer _timer;
        private const int _secondsToWait = 3;
        private DateTime startTime;
        public ConfigurationWindow()
        {
            InitializeComponent();
            _timer = new DispatcherTimer();
            _timer.Tick += timer_Tick;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            int elapsedSeconds = (int)(DateTime.Now - startTime).TotalSeconds;
            int remainingSeconds = _secondsToWait - elapsedSeconds;

            if (remainingSeconds <= 0)
            {
                _timer.Stop();
                this.Hide();
                Controller.Instance.InitGame();
            }
            else
            {
                lblRobot.Content = remainingSeconds;
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {

                startTime = DateTime.Now;
                _timer.Start();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {

                startTime = DateTime.Now;
                _timer.Start();
        }
    }
}
