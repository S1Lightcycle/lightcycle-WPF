using System;
using System.Linq;
using S1lightcycle.Windows;
using S1lightcycle.Objecttracker;
using System.Diagnostics;
using System.Windows.Input;
using S1lightcycle.Communication;

namespace S1lightcycle 
{
    public partial class Controller 
    {

        public delegate void ConnectedEventHandler(object sender);
        public event ConnectedEventHandler Connected;

        protected virtual void OnConnected()
        {
            if (Connected != null)
                Connected(this);
        }

        private Player _player1;
        private Player _player2;
        private AbstractObjectTracker _objTracker;
        private GameWindow _gameWindow;
        private ResultWindow _resultWindow;
        private WallColor[][] _walls;
        private Stopwatch _stopWatch;
        private const int TimerIntervall = 50;    // in ms  berechnen 
        public const int RobotSize = 120;        //test value; robotsize = gridsize
        private int _roiHeight;
        private int _roiWidth;
        private readonly Communicator _communicator;

        public double GameHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
        public double GameWidth = System.Windows.SystemParameters.PrimaryScreenWidth;

        public uint Player1Points { get; private set; }
        public uint Player2Points { get; private set; }

        private static Controller _instance;
        private Controller()
        {
            Player1Points = 0;
            Player2Points = 0;

            _communicator = new Communicator();
            // register for robot events
            _communicator.PackageReceived += PackageReceived;
        }

        public static Controller Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Controller();
                }
                return _instance;
            }
        }

        void PackageReceived(object sender, LcProtocol package)
        {
            if (package.Address == LcProtocol.ADDRESS_SERVER)
            {
                switch (package.Command)
                {
                    case LcProtocol.CMD_ROBOTS_CONNECTED:
                        Trace.TraceInformation("Robots are connected");
                        OnConnected();
                        break;
                }
            }
        }

        public String[] GetSerialPorts()
        {
            return _communicator.GetSerialPorts();
        }

        public void SetSerialPort(String name)
        {
            if (GetSerialPorts().Contains(name))
            {
                _communicator.PortName = name;
            }
        }

        public void PlaceRobots()
        {
            new ConfigurationWindow().Show();
        }

        public void InitGame() 
        {
            InitGameWindow();

            //init wall - collision
            InitWalls();

            //init players
            InitPlayers();

            //init Timers
            InitGameTimer();
            InitCountdownTimer();

            _stopWatch = new Stopwatch();

            _roiWidth = Properties.Settings.Default.RoiWidth;
            _roiHeight = Properties.Settings.Default.RoiHeight;

            /*_communicator.SendPackage(new LcProtocol(LcProtocol.ADDRESS_BROADCAST, LcProtocol.CMD_REVERSE, 0));
            Thread.Sleep(500);
            _communicator.SendPackage(new LcProtocol(LcProtocol.ADDRESS_BROADCAST, LcProtocol.CMD_FORWARD, 0));
            Thread.Sleep(500);
            _communicator.SendPackage(new LcProtocol(LcProtocol.ADDRESS_BROADCAST, LcProtocol.CMD_STOP, 0));
            */

            //Start object tracking
            InitTracking();
            _player1.Robot = _objTracker.FirstCar;
            _player2.Robot = _objTracker.SecondCar;
            _timer.Start();
            _communicator.SendPackage(new LcProtocol(LcProtocol.ADDRESS_BROADCAST, LcProtocol.CMD_FORWARD, 0));
        }

        private void InitPlayers()
        {
            _player1 = new Player(new Grid(1, 1), WallColor.Red);
            _player2 = new Player(new Grid(2, 2), WallColor.Blue);
        }

        private void InitGameWindow()
        {
            //init window
            _gameWindow = new GameWindow();
            _gameWindow.Height = GameHeight;
            _gameWindow.Width = GameWidth;
            _gameWindow.Show();

            _gameWindow.DrawGrid(RobotSize);
        }

        private void InitWalls()
        {
            _walls = new WallColor[_gameWindow.GridWidth + 1][];
            for (int i = 0; i < _walls.Length; i++)
            {
                _walls[i] = new WallColor[_gameWindow.GridHeight + 1];
                for (int j = 0; j < _walls[i].Length; j++)
                {
                    if ((i == 0) || (j == 0) || (i == _walls.Length - 1) || (j == _walls[i].Length - 1))
                    {
                        _walls[i][j] = WallColor.Black; //TODO change
                    }
                    else
                    {
                        _walls[i][j] = WallColor.White;
                    }
                }
            }
        }

        private void InitTracking()
        {
            if (_objTracker == null)
            {
                _objTracker = new ObjectTracker();
            }
            _objTracker.LearningRate = Properties.Settings.Default.LearningRate;
            _objTracker.BlobMaxSize = Properties.Settings.Default.MaxBlobSize;
            _objTracker.BlobMinSize = Properties.Settings.Default.MinBlobSize;
            _objTracker.StartTracking();
        }

        private void UpdatePlayerPosition(Player player)
        {
            if (player.Robot.Coord.Count > 0)
            {
                Coordinate tempPos = DoPositionCompensation(player.Robot.Coord.Dequeue());
                player.CurPos.Column = tempPos.XCoord / RobotSize;
                player.CurPos.Row = tempPos.YCoord / RobotSize;

                //if (IsValidPosition(player, tempPos))
                //{
                    if (IsCollision(player)) {
                        if (player == _player1) {
                            Player2Points++;
                        } else {
                            Player1Points++;
                        }
                        GoToResults();
                        return;
                    }

                    GenerateWall(player);
                    //determine player position on grid
                    

                    //Console.WriteLine("X: " + carPos.XCoord + " | Y: " + carPos.YCoord);
                    //Console.WriteLine("column: " + player.CurPos.Column + " | row: " + player.CurPos.Row);
                //}
            }
        }

        private void GoToResults()
        {
            _communicator.SendPackage(new LcProtocol(LcProtocol.ADDRESS_BROADCAST, LcProtocol.CMD_STOP, 0));
            _communicator.SendPackage(new LcProtocol(LcProtocol.ADDRESS_BROADCAST, LcProtocol.CMD_STOP, 0));
            _timer.Stop();
            _objTracker.StopTracking();
            _resultWindow = new ResultWindow();
            _gameWindow.Close();
            _resultWindow.Show();
        }

        private Coordinate DoPositionCompensation(Coordinate coordinates) 
        {
            int x = Convert.ToInt32(Convert.ToDouble(coordinates.XCoord) / Convert.ToDouble(_roiWidth) * Convert.ToDouble(GameWidth));
            int y = Convert.ToInt32(Convert.ToDouble(coordinates.YCoord) / Convert.ToDouble(_roiHeight) * Convert.ToDouble(GameHeight));
            //Console.WriteLine("x: " + x + " y: " + y);
            //Console.WriteLine("xcoord: " + coordinates.XCoord + " ycoord: " + coordinates.YCoord);
            if (x == -1 || y == -1) return null;

            x = (x / RobotSize) * RobotSize;
            y = (y / RobotSize) * RobotSize;

            return new Coordinate(x, y);
        }

        private bool IsValidPosition(Player player, Coordinate coordinates) 
        {
            if (coordinates == null) return false;
            //coordinates are outside the canvas (gamefield)
            if (coordinates.XCoord < 0 || coordinates.XCoord > GameWidth) return false;
            if (coordinates.YCoord < 0 || coordinates.YCoord > GameHeight) return false;

            //old position equals new position -> no redrawing needed
            if (player.CurPos.Column == (coordinates.XCoord/RobotSize) && player.CurPos.Row == (coordinates.YCoord/RobotSize)) 
            {
                return false;
            }
            return true;
        }

        private void GenerateWall(Player player) 
        {
            if (player.CurPos == null) return;
            _gameWindow.DrawWall(new Coordinate(player.CurPos.Column, player.CurPos.Row), player.Color);
            
            _walls[player.CurPos.Column][player.CurPos.Row] = player.Color;
        }

        private bool IsCollision(Player player)
        {
            Console.WriteLine(player.Color + " " + _walls[player.CurPos.Column][player.CurPos.Row]);
            if ((_walls[player.CurPos.Column][player.CurPos.Row] == player.Color) || (_walls[player.CurPos.Column][player.CurPos.Row] == WallColor.White))
            {
                return false;
            }
            Trace.WriteLine("Collision detected");
            return true;
        }

        public void ResetPlayerPoints()
        {
            Player1Points = 0;
            Player2Points = 0;
        }

        public void ConfigureEdges()
        {
            CameraCalibrator cameraCalibrator = new CameraCalibrator();
            cameraCalibrator.ShowFrame();
        }

        public void Move(Key key)
        {
            switch (key)
            {
                case Key.Escape:
                    GoToResults();
                    break;
                case Key.Down:                   
                    Trace.TraceInformation("player1: " + key.ToString() + " (invalid direction)");
                    break;
                case Key.Up:
                    Trace.TraceInformation("player1: " + key.ToString() + " (invalid direction)");
                    break;
                case Key.Right:                   
                    _communicator.SendPackage(new LcProtocol(LcProtocol.ADDRESS_ROBOT_1, LcProtocol.CMD_TURN_RIGHT_STATIC, 90));     
                    break;
                case Key.Left:
                    _communicator.SendPackage(new LcProtocol(LcProtocol.ADDRESS_ROBOT_1, LcProtocol.CMD_TURN_LEFT_STATIC, 90));
                    break;
                // player 2
                case Key.S: // backward                   
                    Trace.TraceInformation("player2: " + key.ToString() + " (invalid direction)");
                    break;
                case Key.W: // forward                   
                    Trace.TraceInformation("player2: " + key.ToString() + " (invalid direction)");
                    break;
                case Key.D: // right                   
                     _communicator.SendPackage(new LcProtocol(LcProtocol.ADDRESS_ROBOT_2, LcProtocol.CMD_TURN_RIGHT_STATIC, 90));                   
                    break;
                case Key.A: // left                   
                    _communicator.SendPackage(new LcProtocol(LcProtocol.ADDRESS_ROBOT_2, LcProtocol.CMD_TURN_LEFT_STATIC, 90));                  
                    break;
                default:
                    Trace.TraceInformation("invalid key");
                    break;
            }
        }
    }
}
