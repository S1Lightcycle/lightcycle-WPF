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
        }

        public int Player1Result { 
            get 
            {
                return Int32.Parse(this.Player1ResultBox.Content.ToString());
            }
            set
            {
                this.Player1ResultBox.Content = value.ToString();
            }
        }

        public int Player2Result {
            get
            {
                return Int32.Parse(this.Player2ResultBox.Content.ToString());
            }
            set
            {
                this.Player2ResultBox.Content = value.ToString();
            }
        }

        private void Revanche_Button_Click(object sender, RoutedEventArgs e)
        {
            Controller.Instance.InitGame();
            this.Close();
        }

        private void NewGame_Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Show();
			this.Close();
        }

        private void Quit_Button_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }
    }
}
