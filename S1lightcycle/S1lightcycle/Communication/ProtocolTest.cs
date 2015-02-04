using System;

namespace S1lightcycle.Communication
{
    class ProtocolTest
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Available com ports...");

            foreach (String port in Communicator.Instance.GetSerialPorts())
            {
                Console.WriteLine(port);
            }
            Console.Write("Set com port: ");
            Communicator.Instance.PortName = Console.ReadLine();

            while (true)
            {
                Console.Write("lcPackage> ");
                String input = Console.ReadLine();
                String[] split = input.Split(' ');
                LcProtocol package = new LcProtocol(Byte.Parse(split[0]), Byte.Parse(split[1]), 0);
                Communicator.Instance.SendPackage(package);
            }
            Console.Read();
        }
    }
}
