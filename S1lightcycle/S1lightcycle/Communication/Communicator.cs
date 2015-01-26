using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
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

        private string _portName = SerialPort.GetPortNames().FirstOrDefault();
        public string PortName
        {
            get { return _portName; }
            set { _portName = value; InitializeSerialPort(); }
        }

        private Communicator()
        {
            Trace.TraceInformation("Connected to " + _portName);
            InitializeSerialPort();

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



        private void InitializeSerialPort()
        {
            if (_serialPort.IsOpen) {
                _serialPort.Close();
            }

            _serialPort.BaudRate = 115200;
            _serialPort.DataBits = 8;
            _serialPort.Handshake = Handshake.None;
            _serialPort.Parity = Parity.None;
            _serialPort.StopBits = StopBits.One;
            _serialPort.DataReceived += new SerialDataReceivedEventHandler(_serialPort_DataReceived);
            _serialPort.DtrEnable = false;

            if (_portName != null)
            {
                _serialPort.PortName = _portName;
                _serialPort.Open();
            }
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

            if (bytesRead == 2)
            {
                LcProtocolStruct msg = LcProtocol.getProtocolStruct(buffer[LcProtocol.HI], buffer[LcProtocol.LO]);
                Trace.TraceInformation("received command: address = {0}, command = {1}, parameter = {2}", msg.address, msg.command, msg.parameter);
                OnPackageReceived(msg);
            }
        }

        public String[] GetSerialPorts()
        {
            return SerialPort.GetPortNames();
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
                //throw new ApplicationException("Protocol send error " + e.Message);
            }   
        }

        ~Communicator()
        {
            _serialPort.Close();
        }
    }
}
