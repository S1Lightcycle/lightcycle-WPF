using System;
using S1lightcycle.Communication;

namespace ProtocolTest2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Available com ports...");

            Communicator communicator = new Communicator();

            foreach (String port in communicator.GetSerialPorts())
            {
                Console.WriteLine(port);
            }
            Console.Write("Set com port: ");
            communicator.PortName = Console.ReadLine();

            while (true)
            {
                Console.Write("lcPackage> ");
                String input = Console.ReadLine();
                String[] split = input.Split(' ');
                LcProtocol package = new LcProtocol(Byte.Parse(split[0]), Byte.Parse(split[1]), 0);
                communicator.SendPackage(package);
            }
            Console.Read();
        }
    }
}
