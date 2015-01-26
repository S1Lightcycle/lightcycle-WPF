using System;

namespace S1lightcycle.UART
{
    class ProtocolTest
    {
        static void Main(string[] args)
        {
            LcProtocolStruct package = new LcProtocolStruct();
            package.address = LcProtocol.ADDRESS_BROADCAST;
            package.command = LcProtocol.CMD_STOP;
            package.parameter = 0;
            Communicator.Instance.SendPackage(package);

            Console.Read();


        }
    }
}
