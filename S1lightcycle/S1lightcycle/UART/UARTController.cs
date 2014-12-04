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
            _comPort = new SerialPort();
        }

    }
}
