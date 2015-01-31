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
        public ConfigurationWindow()
        {
            InitializeComponent();
            _timer = new DispatcherTimer();
            _timer.Tick += timer_Tick;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
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
                TextBlock tb = new TextBlock();
                tb.Text = ""+remainingSeconds;
                tb.FontSize = 20.0 * (5-remainingSeconds);
                lblRobot.Content = tb;
 
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {

                _startTime = DateTime.Now;
                _timer.Start();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {

                _startTime = DateTime.Now;
                _timer.Start();
        }
    }
}
