using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Timers;
using System.IO;
namespace S1lightcycle.Communication
{
    public class Communicator
    {
        public delegate void PackageReceivedEventHandler(object sender, LcProtocol package);
        public event PackageReceivedEventHandler PackageReceived;

        protected virtual void OnPackageReceived(LcProtocol package)
        {
            if (PackageReceived != null)
                PackageReceived(this, package);
        }
        private Timer _heartbeatTimer;
        private SerialPort _serialPort = new SerialPort();

        private string _portName;
        public string PortName
        {
            get { return _portName; }
            set 
            { 
                if (_serialPort.IsOpen)
                {
                    try
                    {
                        _serialPort.Close();
                    }
                    catch (IOException e)
                    {
                        Trace.WriteLine(e.Message);
                    }
                }
                _portName = value; 
                InitializeSerialPort(); 
            }
        }

        public Communicator()
        {
            _heartbeatTimer = new Timer();
            _heartbeatTimer.Elapsed +=
                (sender, e) => SendPackage(new LcProtocol(LcProtocol.ADDRESS_BROADCAST, LcProtocol.CMD_HEARTBEAT, 0));
            _heartbeatTimer.Interval = 2000;
            PortName = SerialPort.GetPortNames().FirstOrDefault();
        }

        private void InitializeSerialPort()
        {
            if (_serialPort.IsOpen)
            {
                _serialPort.Close();
            }

            _serialPort.BaudRate = 115200;
            _serialPort.DataBits = 8;
            _serialPort.Handshake = Handshake.None;
            _serialPort.Parity = Parity.None;
            _serialPort.StopBits = StopBits.One;
            _serialPort.DataReceived += _serialPort_DataReceived;
            _serialPort.DtrEnable = false;

            if (_portName != null)
            {
                _serialPort.PortName = _portName;
                try
                {
                    _serialPort.Open();
                }
                catch (IOException e)
                {
                    Trace.WriteLine(e.Message);
                }
                
                if (_serialPort.IsOpen)
                {
                    Trace.WriteLine("Connected to serial port " + _serialPort.PortName);
                    _heartbeatTimer.Enabled = true;
                    _heartbeatTimer.Start();
                    Trace.WriteLine("Heartbeat started");
                }
            }
        }

        private void _serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try {
                //Console.WriteLine(_serialPort.ReadExisting()); 

                //Initialize a buffer to hold the received data 
                byte[] buffer = new byte[_serialPort.ReadBufferSize];

                //There is no accurate method for checking how many bytes are read 
                //unless you check the return from the Read method 
                int bytesRead = _serialPort.Read(buffer, 0, buffer.Length);

                if (bytesRead == 2) {
                    LcProtocol msg = new LcProtocol(buffer[LcProtocol.HI], buffer[LcProtocol.LO]);
                    Trace.TraceInformation("received command: address = {0}, command = {1}, parameter = {2}", msg.Address, msg.Command, msg.Parameter);
                    OnPackageReceived(msg);
                }
            } catch (IOException ex) {
                Trace.WriteLine(ex.StackTrace);
            }

        }

        public String[] GetSerialPorts()
        {
            return SerialPort.GetPortNames();
        }

        public void SendPackage(LcProtocol package)
        {

            byte[] data = package.BuildProtocolData();
            try
            {
                _serialPort.Write(data, 0, 2);
                Trace.TraceInformation("send package: address = {0}, command = {1}, parameter = {2}; raw: bin {3} {4}, hex {5} {6}, dec {7} {8}", package.Address, package.Command, package.Parameter, Convert.ToString(data[LcProtocol.HI], 2).PadLeft(8, '0'), Convert.ToString(data[LcProtocol.LO], 2).PadLeft(8, '0'), Convert.ToString(data[LcProtocol.HI], 16).PadLeft(2, '0'), Convert.ToString(data[LcProtocol.LO], 16).PadLeft(2, '0'), data[LcProtocol.HI], data[LcProtocol.LO]);
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
