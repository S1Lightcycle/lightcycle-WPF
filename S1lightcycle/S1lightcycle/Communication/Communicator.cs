using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace S1lightcycle.UART
{
    public class Communicator
    {
        public delegate void PackageReceivedEventHandler(object sender, LcProtocolStruct package);
        public event PackageReceivedEventHandler PackageReceived;

        protected virtual void OnPackageReceived(LcProtocolStruct package)
        {
            if (PackageReceived != null)
                PackageReceived(this, package);
        }

        private static Communicator instance;

        private Timer timer = new Timer();

        private LcProtocolStruct heartbeatPackage = new LcProtocolStruct();

        private int HEARTBEAT_INTERVALL = 1000;

        private SerialPort _serialPort = new SerialPort();
        private int _baudRate = 115200;
        private int _dataBits = 8;
        private Handshake _handshake = Handshake.None;
        private Parity _parity = Parity.None;
        private string _portName = "COM13";
        private StopBits _stopBits = StopBits.One;

        /// <summary> 
        /// Holds data received until we get a terminator. 
        /// </summary> 
        private string tString = string.Empty;
        /// <summary> 
        /// End of transmition byte in this case EOT (ASCII 4). 
        /// </summary> 
        private byte _terminator = 0x4;
        

        public int BaudRate { get { return _baudRate; } set { _baudRate = value; } }
        public int DataBits { get { return _dataBits; } set { _dataBits = value; } }
        public Handshake Handshake { get { return _handshake; } set { _handshake = value; } }
        public Parity Parity { get { return _parity; } set { _parity = value; } }
        public string PortName { get { return _portName; } set { _portName = value; } }

        private Communicator()
        {
            int portNr = 0;
            while (!Open() && portNr < 30)
            {
                _portName = "COM" + ++portNr;
            }
            Trace.TraceInformation("Connected to " + _portName);

            // enable heartbeat
            heartbeatPackage.address = LcProtocol.ADDRESS_BROADCAST;
            heartbeatPackage.command = LcProtocol.CMD_HEARTBEAT;

            //timer.Elapsed += new ElapsedEventHandler(heartbeat_tick);
            timer.Interval = HEARTBEAT_INTERVALL;
            timer.Enabled = true;
            timer.Start();
        }

        public static Communicator Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Communicator();
                }
                return instance;
            }
        }

        

        private bool Open()
        {
            try
            {
                _serialPort.BaudRate = _baudRate;
                _serialPort.DataBits = _dataBits;
                _serialPort.Handshake = _handshake;
                _serialPort.Parity = _parity;
                _serialPort.PortName = _portName;
                _serialPort.StopBits = _stopBits;
                _serialPort.DataReceived += new SerialDataReceivedEventHandler(_serialPort_DataReceived);
                _serialPort.DtrEnable = false;
                _serialPort.Open();
            }
            catch
            {
                return false;
            }
            return true;
        }

        void heartbeat_tick(object sender, EventArgs e)
        {
            Communicator.Instance.SendPackage(heartbeatPackage);
        }

        private void _serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //Console.WriteLine(_serialPort.ReadExisting()); 
            
            //Initialize a buffer to hold the received data 
            byte[] buffer = new byte[_serialPort.ReadBufferSize];

            //There is no accurate method for checking how many bytes are read 
            //unless you check the return from the Read method 
            int bytesRead = _serialPort.Read(buffer, 0, buffer.Length);

            if(bytesRead == 2) {
                LcProtocolStruct msg = LcProtocol.getProtocolStruct(buffer[LcProtocol.HI], buffer[LcProtocol.LO]);
                Trace.TraceInformation("received command: address = {0}, command = {1}, parameter = {2}", msg.address, msg.command, msg.parameter);
                OnPackageReceived(msg);
            }
            

            
             
        }



        public void SendPackage(LcProtocolStruct package)
        {
            byte[] data = LcProtocol.buildProtocolData(package);
            try
            {
                _serialPort.Write(data, 0, 2);
                Trace.TraceInformation("send package: address = {0}, command = {1}, parameter = {2}; raw: bin {3} {4}, hex {5} {6}, dec {7} {8}", package.address, package.command, package.parameter, Convert.ToString(data[LcProtocol.HI], 2).PadLeft(8, '0'), Convert.ToString(data[LcProtocol.LO], 2).PadLeft(8, '0'), Convert.ToString(data[LcProtocol.HI], 16).PadLeft(2, '0'), Convert.ToString(data[LcProtocol.LO], 16).PadLeft(2, '0'), data[LcProtocol.HI], data[LcProtocol.LO]);
            }
            catch (Exception e)
            {
                throw new ApplicationException("Protocol send error " + e.Message);
            }

        }

        ~Communicator()
        {
            _serialPort.Close();
        }
    }
}
