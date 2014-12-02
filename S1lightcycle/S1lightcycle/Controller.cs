using System;
using System.Collections.Generic;
using System.Linq;
using S1LightcycleNET;
using System.Windows.Threading;
using System.Diagnostics;

namespace S1lightcycle {
    public class Controller {
        private Player _player1;
        private Player _player2;
        private DispatcherTimer _timer;
        private ObjectTracker _objTracker;
        private Windows.GameWindow _gameWindow;
        private Windows.ResultWindow _resultWindow;
        private HashSet<Grid> _walls;
        private Stopwatch _stopWatch;
        private int _countTicks = 0;
        private const int TimerIntervall = 10;    // in ms  berechnen 
        private const int RobotSize = 30;        //test value; robotsize = gridsize

        public int GameHeight = 480;
        public int GameWidth = 640;

        public uint Player1Points { get; private set; }
        public uint Player2Points { get; private set; }

        private static Controller instance;
        private Controller()
        {
            Player1Points = 0;
            Player2Points = 0;
        }

        public static Controller Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Controller();
                }
                return instance;
            }
        }

        public void InitGame() {
            //init wall - collision
            _walls = new HashSet<Grid>();

            //init window
            _gameWindow = new Windows.GameWindow();
            _gameWindow.Height = GameHeight;
            _gameWindow.Width = GameWidth;
            _gameWindow.Show();

            _gameWindow.DrawGrid(RobotSize);
            
            //init players
            _player1 = new Player(Direction.Right, new Grid(1, 1), WallColor.Blue);
            //GenerateWall(_player1, _player1.CurPos);
            _player2 = new Player(Direction.Left, new Grid(2, 2), WallColor.Red);
            //GenerateWall(_player2, _player2.CurPos);

            //set _timer -> Update method
            _timer = new DispatcherTimer();
            _timer.Tick += new EventHandler(Update);
            _timer.Interval = new TimeSpan(0, 0, 0, 0, TimerIntervall); //TimeSpan days/hours/minutes/seconds/milliseconds
            _timer.Start();

            _stopWatch = new Stopwatch();

            //Start object tracking
            InitTracking();
        }

        private void InitTracking()
        {
            _objTracker = new ObjectTracker()
            {
                LEARNING_RATE = Properties.Settings.Default.LearningRate,
                BLOB_MAX_SIZE = Properties.Settings.Default.MaxBlobSize,
                BLOB_MIN_SIZE = Properties.Settings.Default.MinBlobSize
            };

            _objTracker.StartTracking();
        }

        /* Thread priority definieren */
        private void Update(object sender, EventArgs e) {
            _stopWatch.Start();

            if (_objTracker.FirstCar.Coord.Count > 0) {
                Coordinate firstCarPos = DoPositionCompensation(_objTracker.FirstCar.Coord.Dequeue());
                if (IsValidPosition(_player1, firstCarPos)) {
                    if (DidCollide(_player1))
                    {
                        Player2Points++;
                        GoToResults();
                        return;
                    }
                    GenerateWall(_player1, firstCarPos);
                    //determine player position on grid
                    _player1.CurPos.column = firstCarPos.XCoord / RobotSize;
                    _player1.CurPos.row = firstCarPos.YCoord / RobotSize;
                }
            }

            if (_objTracker.SecondCar.Coord.Count > 0) {
                Coordinate secondCarPos = DoPositionCompensation(_objTracker.SecondCar.Coord.Dequeue());
                if (IsValidPosition(_player2, secondCarPos)) {
                    if (DidCollide(_player2))
                    {
                        Player1Points++;
                        GoToResults();
                        return;
                    }
                    GenerateWall(_player2, secondCarPos);
                    //determine player position on grid
                    _player2.CurPos.column = secondCarPos.XCoord / RobotSize;
                    _player2.CurPos.row = secondCarPos.YCoord / RobotSize;
                }
            }
            _countTicks += 1;
            _stopWatch.Stop();
            
            Console.WriteLine("ellapsed time in ms: " + _stopWatch.ElapsedMilliseconds);
            _stopWatch.Reset();
        }

        private void GoToResults()
        {
            _timer.Stop();
            _resultWindow = new Windows.ResultWindow();
            _gameWindow.Close();
            _resultWindow.Show();
        }

        private Coordinate DoPositionCompensation(Coordinate coordinates) {
            int x = coordinates.XCoord;
            int y = coordinates.YCoord;
            int center = RobotSize / 2;

            if (x == -1 || y == -1) return null;

            x = (x / RobotSize) * RobotSize;
            y = (y / RobotSize) * RobotSize;

            return new Coordinate(y, x);
        }

        private bool IsValidPosition(Player player, Coordinate coordinates) {
            if (coordinates == null) return false;
            //coordinates are outside the canvas (gamefield)
            if (coordinates.XCoord < 0 || coordinates.XCoord > GameWidth) return false;
            if (coordinates.YCoord < 0 || coordinates.YCoord > GameHeight) return false;

            //old position equals new position -> no redrawing needed
            if (player.CurPos.column == (coordinates.XCoord/RobotSize) && player.CurPos.row == (coordinates.YCoord/RobotSize)) {
                return false;
            }
            return true;
        }

        private void GenerateWall(Player player, Coordinate coordinates) {
            if (coordinates == null) return;
            _gameWindow.DrawWall(coordinates, player.Color);
            
            _walls.Add(new Grid (coordinates.XCoord / RobotSize, coordinates.YCoord / RobotSize));
        }

        private bool DidCollide(Player player)
        {
            foreach (Grid wall in _walls)
            {
                if ((wall.column.Equals(player.CurPos.column)) && (wall.row.Equals((player.CurPos.column))))
                {
                    return true;
                }
            }

            return false;
        }

        private void PrintCoordinates(Coordinate coordinates) {
            Console.WriteLine("-----------");
            Console.Write("x: " + coordinates.XCoord + " | ");
            Console.Write("y: " + coordinates.YCoord);
            Console.WriteLine("-----------");
        }

        public void ResetPlayerPoints()
        {
            Player1Points = 0;
            Player2Points = 0;
        }
    }
}
