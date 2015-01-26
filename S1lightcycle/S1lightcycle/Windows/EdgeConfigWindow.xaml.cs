using System.Windows;
using System.Windows.Input;
using S1LightcycleNET;

namespace S1lightcycle.Windows {
    /// <summary>
    /// Interaction logic for EdgeConfigWindow.xaml
    /// </summary>
    public partial class EdgeConfigWindow : Window {
        public EdgeConfigWindow() {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e) {
            CalibrateCamera.GetInstance().ShowFrame();
            this.Hide();
        }
    }
}
