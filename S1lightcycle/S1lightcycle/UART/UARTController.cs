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
        private SerialPort _comPort;

        private const String DefaultPortName = "COM4";
        private const int DefaultBaudRate = 9600;
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

    }
}
