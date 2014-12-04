using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Timers;
using S1lightcycle.Windows;
using S1LightcycleNET;
using System.Windows.Threading;
using System.Diagnostics;

namespace S1lightcycle {
    public partial class Controller {
        private Player _player1;
        private Player _player2;
        private ObjectTracker _objTracker;
        private Windows.GameWindow _gameWindow;
        private Windows.ResultWindow _resultWindow;
        private bool[][] _walls;
        private Stopwatch _stopWatch;
        private int _countTicks = 0;
        private const int TimerIntervall = 10;    // in ms  berechnen 
        private const int RobotSize = 30;        //test value; robotsize = gridsize

        public int GameHeight = 480;
        public int GameWidth = 640;

        public uint Player1Points { get; private set; }
        public uint Player2Points { get; private set; }

        private static Controller _instance;
        private Controller()
        {
            Player1Points = 0;
            Player2Points = 0;
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

        public void InitGame() {
            InitGameWindow();

            //init wall - collision
            InitWalls();

            //init players
            InitPlayers();

            //init Timers
            InitGameTimer();
            InitCountdownTimer();

            _stopWatch = new Stopwatch();

            //Start object tracking
            if (_objTracker == null)
            {
                InitTracking();
            }
            _player1.Robot = _objTracker.FirstCar;
            _player2.Robot = _objTracker.SecondCar;
            _timer.Start();
        }

        private void InitPlayers()
        {
            _player1 = new Player(Direction.Right, new Grid(1, 1), WallColor.Blue);
            _player2 = new Player(Direction.Left, new Grid(2, 2), WallColor.Red);
        }

        private void InitGameWindow()
        {
            //init window
            _gameWindow = new Windows.GameWindow();
            _gameWindow.Height = GameHeight;
            _gameWindow.Width = GameWidth;
            _gameWindow.Show();

            _gameWindow.DrawGrid(RobotSize);
        }

        private void InitWalls()
        {
            _walls = new bool[_gameWindow.GridWidth + 1][];
            for (int i = 0; i < _walls.Length; i++)
            {
                _walls[i] = new bool[_gameWindow.GridHeight + 1];
                for (int j = 0; j < _walls[i].Length; j++)
                {
                    _walls[i][j] = false;
                }
            }
        }

        private void InitTracking()
        {
            _objTracker = new ObjectTracker()
            {
                LearningRate = Properties.Settings.Default.LearningRate,
                BlobMaxSize = Properties.Settings.Default.MaxBlobSize,
                BlobMinSize = Properties.Settings.Default.MinBlobSize
            };

            _objTracker.StartTracking();

        }

        private void UpdatePlayerPosition(Player player)
        {
            if (player.Robot.Coord.Count > 0)
            {
                Coordinate carPos = DoPositionCompensation(player.Robot.Coord.Dequeue());
                if (IsValidPosition(player, carPos))
                {
                    if (IsCollision(player))
                    {
                        if (player == _player1)
                        {
                            Player2Points++;
                        }
                        else
                        {
                            Player1Points++;
                        }
                        GoToResults();
                        return;
                    }
                    GenerateWall(player, carPos);
                    //determine player position on grid
                    player.CurPos.Column = carPos.XCoord / RobotSize;
                    player.CurPos.Row = carPos.YCoord / RobotSize;
                }
            }
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
            if (player.CurPos.Column == (coordinates.XCoord/RobotSize) && player.CurPos.Row == (coordinates.YCoord/RobotSize)) {
                return false;
            }
            return true;
        }

        private void GenerateWall(Player player, Coordinate coordinates) {
            if (coordinates == null) return;
            _gameWindow.DrawWall(coordinates, player.Color);
            
            _walls[player.CurPos.Column][player.CurPos.Row] = true;
        }

        private bool IsCollision(Player player)
        {
            return _walls[player.CurPos.Column][player.CurPos.Row];
        }

        public void ResetPlayerPoints()
        {
            Player1Points = 0;
            Player2Points = 0;
        }
    }
}
