using S1lightcycle.Communication;
using System.Windows;
using System.Windows.Input;


namespace S1lightcycleClient {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Communicator _communicator;
        public MainWindow() {
            InitializeComponent();
            _communicator = new Communicator();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e) {
            switch (e.Key) {
                case Key.Right:
                    _communicator.SendPackage(new LcProtocol(LcProtocol.ADDRESS_ROBOT_1, LcProtocol.CMD_TURN_RIGHT_STATIC, 90));
                    break;
                case Key.Left:
                    _communicator.SendPackage(new LcProtocol(LcProtocol.ADDRESS_ROBOT_1, LcProtocol.CMD_TURN_LEFT_STATIC, 90));
                    break;
                case Key.Down:
                    _communicator.SendPackage(new LcProtocol(LcProtocol.ADDRESS_ROBOT_1, LcProtocol.CMD_REVERSE, 90));
                    break;
                case Key.Up:
                    _communicator.SendPackage(new LcProtocol(LcProtocol.ADDRESS_ROBOT_1, LcProtocol.CMD_FORWARD, 90));
                    break;
                case Key.A:
                    _communicator.SendPackage(new LcProtocol(LcProtocol.ADDRESS_ROBOT_2, LcProtocol.CMD_TURN_LEFT_STATIC, 90));
                    break;
                case Key.S:
                    _communicator.SendPackage(new LcProtocol(LcProtocol.ADDRESS_ROBOT_2, LcProtocol.CMD_REVERSE, 90));
                    break;
                case Key.D:
                    _communicator.SendPackage(new LcProtocol(LcProtocol.ADDRESS_ROBOT_2, LcProtocol.CMD_TURN_RIGHT_STATIC, 90));
                    break;
                case Key.W:
                    _communicator.SendPackage(new LcProtocol(LcProtocol.ADDRESS_ROBOT_2, LcProtocol.CMD_FORWARD, 90));
                    break;
                default:
                    break;
            }
        }
    }
}
