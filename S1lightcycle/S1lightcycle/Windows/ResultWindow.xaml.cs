﻿using System;
using System.Windows;

namespace S1lightcycle.Windows
{
    /// <summary>
    /// Interaktionslogik für Result.xaml
    /// </summary>
    public partial class ResultWindow : Window
    {
        public ResultWindow()
        {
            InitializeComponent();
            
        }

        private void Revanche_Button_Click(object sender, RoutedEventArgs e)
        {
            Controller.Instance.PlaceRobots();
            Hide();
        }

        private void Menu_Button_Click(object sender, RoutedEventArgs e)
        {
            Controller.Instance.ResetPlayerPoints();
            Application.Current.MainWindow.Show();
            Hide();
        }

        private void Quit_Button_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }

        public void UpdateResults() {
            Player1ResultLabel.Content = Controller.Instance.Player1Points;
            Player2ResultLabel.Content = Controller.Instance.Player2Points;
        }
        
    }
}
