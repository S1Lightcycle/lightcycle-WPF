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

namespace S1lightcycle.Windows {
    /// <summary>
    /// Interaction logic for EdgeConfigWindow.xaml
    /// </summary>
    public partial class EdgeConfigWindow : Window {
        public EdgeConfigWindow() {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e) {
            new CalibrateCamera().ShowFrame();
            this.Hide();
        }
    }
}
