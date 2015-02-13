using System.Windows;
using System.Windows.Input;
using S1lightcycle.Objecttracker;
using System.Timers;
using System;
using System.Threading.Tasks;

namespace S1lightcycle.Windows {

    

    /// <summary>
    /// Interaction logic for EdgeConfigWindow.xaml
    /// </summary>
    public partial class EdgeConfigWindow : Window {

        private Timer timer = new Timer();

        public EdgeConfigWindow() {
            InitializeComponent();
            timer.Interval = 100;
            timer.Elapsed += timer_Elapsed;
            timer.Start();
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            timer.Stop();                        
            Dispatcher.BeginInvoke((Action)(() => { 
                Controller.Instance.ConfigureEdges();
                Hide();
            }));
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e) {
           
        }
    }
}
