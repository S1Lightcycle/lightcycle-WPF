using S1lightcycle.UART;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtocolTest {
    class Program {
        static void Main(string[] args) {
            LcProtocol package = new LcProtocol(LcProtocol.ADDRESS_BROADCAST, LcProtocol.CMD_STOP, 0);
            Communicator.Instance.SendPackage(package);
            bool isRunning = true;
            while (isRunning) {
                LcProtocol prot;
                string asdf = Console.ReadLine();
                switch (asdf) {
                    case "forward":
                        prot = new LcProtocol(LcProtocol.ADDRESS_BROADCAST, LcProtocol.CMD_FORWARD, 0);
                        break;
                    case "stop":
                        prot = new LcProtocol(LcProtocol.ADDRESS_BROADCAST, LcProtocol.CMD_STOP, 0);
                        break;
                    case "reverse":
                        prot = new LcProtocol(LcProtocol.ADDRESS_BROADCAST, LcProtocol.CMD_REVERSE, 0);
                        break;
                    case "right":
                        prot = new LcProtocol(LcProtocol.ADDRESS_BROADCAST, LcProtocol.CMD_TURN_RIGHT_STATIC, 0);
                        break;
                    case "left":
                        prot = new LcProtocol(LcProtocol.ADDRESS_BROADCAST, LcProtocol.CMD_TURN_LEFT_STATIC, 0);
                        break;
                    default:
                        Console.WriteLine("not yet supported");
                        prot = new LcProtocol(LcProtocol.ADDRESS_BROADCAST, LcProtocol.CMD_STOP, 0);
                        break;
                }
                Communicator.Instance.SendPackage(prot);
            }
            
        }
    }
}
