using System;
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
            Player1ResultLabel.Content = Controller.Instance.Player1Points;
            Player2ResultLabel.Content = Controller.Instance.Player2Points;
        }

        private void Revanche_Button_Click(object sender, RoutedEventArgs e)
        {
            Controller.Instance.PlaceRobots();
            Close();
        }

        private void Menu_Button_Click(object sender, RoutedEventArgs e)
        {
            Controller.Instance.ResetPlayerPoints();
            Application.Current.MainWindow.Show();
			Close();
        }

        private void Quit_Button_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }
    }
}
