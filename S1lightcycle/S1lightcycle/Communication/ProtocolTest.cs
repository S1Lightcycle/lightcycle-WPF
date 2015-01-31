using System;

namespace S1lightcycle.Communication
{
    class ProtocolTest
    {
        static void Main(string[] args)
        {
            LcProtocol package = new LcProtocol(LcProtocol.ADDRESS_BROADCAST, LcProtocol.CMD_STOP, 0);
            Communicator.Instance.SendPackage(package);

            Console.Read();
        }
    }
}
