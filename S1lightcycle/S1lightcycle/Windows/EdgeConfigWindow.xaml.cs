using System.Windows;
using System.Windows.Input;
using S1lightcycle.Objecttracker;

namespace S1lightcycle.Windows {
    /// <summary>
    /// Interaction logic for EdgeConfigWindow.xaml
    /// </summary>
    public partial class EdgeConfigWindow : Window {
        public EdgeConfigWindow() {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e) {
            CameraCalibrator cameraCalibrator = new CameraCalibrator();
            cameraCalibrator.ShowFrame();
            Hide();
            System.Windows.MessageBox.Show("Click on the red points in the screenshot", "S1Lightcycle");
        }
    }
}
