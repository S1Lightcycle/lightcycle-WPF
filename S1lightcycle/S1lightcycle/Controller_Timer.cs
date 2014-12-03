using System;
using System.Timers;
using System.Windows.Threading;

namespace S1lightcycle
{
    partial class Controller
    {
        private DispatcherTimer _timer;
        private Timer _countDownTimer;
        private bool _isCountDownOver;

        private void InitCountdownTimer()
        {
            _countDownTimer = new Timer(2000);
            _countDownTimer.Elapsed += new ElapsedEventHandler(CountDownOver);
            _countDownTimer.Enabled = true;
            _isCountDownOver = false;
        }

        private void InitGameTimer()
        {
            //set _timer -> Update method
            _timer = new DispatcherTimer();
            _timer.Tick += new EventHandler(Update);
            _timer.Interval = new TimeSpan(0, 0, 0, 0, TimerIntervall); //TimeSpan days/hours/minutes/seconds/milliseconds
            _timer.Start();
        }

        private void CountDownOver(object sender, ElapsedEventArgs e)
        {
            _isCountDownOver = true;
            _countDownTimer.Enabled = false;
            this._objTracker.FirstCar.Coord.Clear();
            this._objTracker.SecondCar.Coord.Clear();
        }
    }
}
