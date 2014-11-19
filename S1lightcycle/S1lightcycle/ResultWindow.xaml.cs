﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace S1lightcycle
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
                return Int32.Parse(this.Player1ResultBox.Text);
            }
            set
            {
                this.Player1ResultBox.Text = value.ToString();
            }
        }

        public int Player2Result {
            get
            {
                return Int32.Parse(this.Player2ResultBox.Text);
            }
            set
            {
                this.Player2ResultBox.Text = value.ToString();
            }
        }

        private void Revanche_Button_Click(object sender, RoutedEventArgs e)
        {
            GameWindow game = new GameWindow();
            this.Close();
            game.Show();
        }

        private void NewGame_Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            this.Close();
            main.Show();
        }

        private void Quit_Button_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }
    }
}
