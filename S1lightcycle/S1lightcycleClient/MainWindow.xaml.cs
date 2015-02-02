using S1lightcycle.Communication;
using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace S1lightcycleClient {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e) {
            switch (e.Key) {
                case Key.Right:
                    Communicator.Instance.SendPackage(new LcProtocol(LcProtocol.ADDRESS_ROBOT_1, LcProtocol.CMD_TURN_RIGHT_STATIC, 90));
                    break;
                case Key.Left:
                    Communicator.Instance.SendPackage(new LcProtocol(LcProtocol.ADDRESS_ROBOT_1, LcProtocol.CMD_TURN_LEFT_STATIC, 90));
                    break;
                case Key.Down:
                    Communicator.Instance.SendPackage(new LcProtocol(LcProtocol.ADDRESS_ROBOT_1, LcProtocol.CMD_REVERSE, 90));
                    break;
                case Key.Up:
                    Communicator.Instance.SendPackage(new LcProtocol(LcProtocol.ADDRESS_ROBOT_1, LcProtocol.CMD_FORWARD, 90));
                    break;
                case Key.A:
                    Communicator.Instance.SendPackage(new LcProtocol(LcProtocol.ADDRESS_ROBOT_2, LcProtocol.CMD_TURN_LEFT_STATIC, 90));
                    break;
                case Key.S:
                    Communicator.Instance.SendPackage(new LcProtocol(LcProtocol.ADDRESS_ROBOT_2, LcProtocol.CMD_REVERSE, 90));
                    break;
                case Key.D:
                    Communicator.Instance.SendPackage(new LcProtocol(LcProtocol.ADDRESS_ROBOT_2, LcProtocol.CMD_TURN_RIGHT_STATIC, 90));
                    break;
                case Key.W:
                    Communicator.Instance.SendPackage(new LcProtocol(LcProtocol.ADDRESS_ROBOT_2, LcProtocol.CMD_FORWARD, 90));
                    break;
                default:
                    break;
            }
        }
    }
}
