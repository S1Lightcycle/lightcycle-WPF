using System;
using System.Windows;

namespace S1lightcycle.Windows {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            lbl_connecting.Visibility = Visibility.Collapsed;
            btnStartGame.IsEnabled = true;
            // enable events
            Controller.Instance.Connected += new S1lightcycle.Controller.ConnectedEventHandler(Connected);
        }

        void Connected(object sender)
        {
            lbl_connecting.Dispatcher.BeginInvoke((Action)(() => lbl_connecting.Visibility = Visibility.Collapsed));
            btnStartGame.Dispatcher.BeginInvoke((Action)(() => btnStartGame.IsEnabled = true));
        }

        private void btnExit_Click(object sender, RoutedEventArgs e) {
            Application.Current.Shutdown();
        }

        private void btnStartGame_Click(object sender, RoutedEventArgs e) {
            Controller.Instance.InitGame();
            this.Hide();
        }

        private void btnOptions_Click(object sender, RoutedEventArgs e) {
            new OptionWindow().Show();
            
        }
    }
}
