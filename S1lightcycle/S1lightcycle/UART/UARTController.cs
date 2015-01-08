using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace S1lightcycle.UART
{
    class UARTController
    {
        enum Command{
            Heartbeat, Forward, Reverse, RightStatic, LeftStatic, Right, Left, SetSpeed
        }
        enum Receiver
        {
            Server = 0, Client1 = 1, Client2 = 2, Broadcast = 7
        }


        private SerialPort _comPort;

        private const String DefaultPortName = "COM4";
        private const int DefaultBaudRate = 115200;
        private const int DefaultDataBits = 8;
        private const Parity DefaultParity = Parity.None;
        private const StopBits DefaultStopBits = StopBits.One;
        private const Handshake DefaultHandshake = Handshake.None;

        private static UARTController _instance;
        public UARTController Instance 
        { 
            get
            {
                if (_instance == null)
                {
                    _instance = new UARTController();
                }
                return _instance;
            } 
        }

        private UARTController()
        {
            _comPort = new SerialPort()
            {
                PortName = DefaultPortName,
                BaudRate = DefaultBaudRate,
                DataBits = DefaultDataBits,
                Parity = DefaultParity,
                StopBits = DefaultStopBits,
                Handshake = DefaultHandshake
            };
        }


        public void Write(Receiver receiver, Command command, int parameter)
        {
            try
            {
                _comPort.Open();
                _comPort.Write("" + receiver + command + parameter);
            }
            finally
            {
                _comPort.Close();
            }
        }

        public void Write(String message)
        {
            try
            {
                _comPort.Open();
                _comPort.Write(message);
            }
            finally
            {
                _comPort.Close();
            }
            
        }
    }
}
