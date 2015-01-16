using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S1lightcycle.UART
{
    class ProtocolTest
    {
        static void Main(string[] args)
        {
            LcProtocolStruct package = new LcProtocolStruct();
            package.address = LcProtocol.ADDRESS_BROADCAST;
            package.command = LcProtocol.CMD_FORWARD;
            package.parameter = 0;
            Communicator.Instance.SendPackage(package);

            Console.Read();


        }
    }
}
