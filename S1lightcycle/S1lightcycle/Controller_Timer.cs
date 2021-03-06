﻿using System;
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
            _countDownTimer = new Timer(1000);
            _countDownTimer.Elapsed += CountDownOver;
            _countDownTimer.Enabled = true;
            _isCountDownOver = false;
        }

        private void InitGameTimer()
        {
            //set _timer -> Update method
            _timer = new DispatcherTimer();
            _timer.Tick += Update;
            _timer.Interval = new TimeSpan(0, 0, 0, 0, TimerIntervall); //TimeSpan days/hours/minutes/seconds/milliseconds
        }

        private void CountDownOver(object sender, ElapsedEventArgs e)
        {
            _isCountDownOver = true;
            _countDownTimer.Enabled = false;
            if (_objTracker != null)
            {
                _objTracker.FirstCar.Coord.Clear();
                _objTracker.SecondCar.Coord.Clear();
            }
        }

        /* Thread priority definieren */
        private void Update(object sender, EventArgs e)
        {
            if (_isCountDownOver)
            {
                _stopWatch.Start();

                UpdatePlayerPosition(_player1);
                UpdatePlayerPosition(_player2);

                _stopWatch.Stop();
                _stopWatch.Reset();
            }
        }
    }
}
