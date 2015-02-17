using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace S1lightcycle.Windows
{
    /// <summary>
    /// Interaktionslogik für ConfigurationWindow.xaml
    /// </summary>
    public partial class ConfigurationWindow : Window
    {

        private DispatcherTimer _timer;
        private const int SecondsToWait = 3;
        private DateTime _startTime;
        private bool _countdownStarted = false;

        public ConfigurationWindow()
        {
            InitializeComponent();
            _timer = new DispatcherTimer();
            _timer.Tick += timer_Tick;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            lblRobot.Visibility = Visibility.Hidden;

            int elapsedSeconds = (int)(DateTime.Now - _startTime).TotalSeconds;
            int remainingSeconds = SecondsToWait - elapsedSeconds;

            if (remainingSeconds <= 0)
            {
                _timer.Stop();
                Hide();
                Controller.Instance.InitGame();
            }
            else
            {
                lblCountdown.Content = "" + remainingSeconds;
                lblCountdown.FontSize = 150 + 30 * (4 - remainingSeconds);
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (!_countdownStarted)
            {
                _startTime = DateTime.Now;
                _timer.Start();
                _countdownStarted = true;
            }               
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!_countdownStarted)
            {
                _startTime = DateTime.Now;
                _timer.Start();
                _countdownStarted = true;
            }
        }
    }
}
