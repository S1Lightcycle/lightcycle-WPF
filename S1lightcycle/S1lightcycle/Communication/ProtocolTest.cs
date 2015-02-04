using System;

namespace S1lightcycle.Communication
{
    class ProtocolTest
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Available com ports...");
            for (int i = 0; i < Communicator.Instance.GetSerialPorts().Length; i++)
            {
                Console.WriteLine(Communicator.Instance.GetSerialPorts()[i]);
            }
            Console.Write("Set com port: ");
            String comPort = Console.ReadLine();

            Communicator.Instance.PortName = comPort;

            String input;
            while (true)
            {
                Console.Write("lcPackage> ");
                input = Console.ReadLine();
                String[] split = input.Split(' ');
                LcProtocol package = new LcProtocol(Byte.Parse(split[0]), Byte.Parse(split[1]), 0);
                Communicator.Instance.SendPackage(package);
            }

           

            Console.Read();
        }
    }
}
