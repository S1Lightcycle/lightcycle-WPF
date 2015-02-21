using System.Windows;
using System.Windows.Input;
using System.Timers;
using System;
using System.Threading.Tasks;

namespace S1lightcycle.Windows 
{
    /// <summary>
    /// Interaction logic for EdgeConfigWindow.xaml
    /// </summary>
    public partial class EdgeConfigWindow : Window {

        private Timer _timer;

        public EdgeConfigWindow() {
            InitializeComponent();
            _timer = new Timer();
            _timer.Interval = 100;
            _timer.Elapsed += timer_Elapsed;
            _timer.Start();
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _timer.Stop();
            Task.Factory.StartNew(() => ConfigureEdges());
        }

        private void ConfigureEdges()
        {            
            Dispatcher.BeginInvoke((Action)(() => {
                Controller.Instance.ConfigureEdges();
                Hide();
                }));
        }
    }
}
