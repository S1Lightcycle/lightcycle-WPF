﻿using System.Windows;

namespace S1lightcycle.Windows {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e) {
            Application.Current.Shutdown();
        }

        private void btnStartGame_Click(object sender, RoutedEventArgs e) {
            Controller.Instance.InitGame();
            this.Close();
        }

        private void btnOptions_Click(object sender, RoutedEventArgs e) {
            new OptionWindow().Show();
            
        }
    }
}