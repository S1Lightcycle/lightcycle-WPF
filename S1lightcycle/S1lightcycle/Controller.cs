using System;
using System.Collections.Generic;
using S1LightcycleNET;
using System.Windows.Threading;
using System.Diagnostics;

namespace S1lightcycle {
    public class Controller {
        private Player player1;
        private Player player2;
        private DispatcherTimer timer;
        private ObjectTracker objTracker;
        private GameWindow gameWindow;
        private HashSet<Grid> walls;
        private Stopwatch stopWatch;
        private int countTicks = 0;
        private int timerIntervall = 1;    // in ms  berechnen 
        private int robotSize = 30;        //test value; robotsize = gridsize

        public int GameHeight = 480;
        public int GameWidth = 640;

        private static Controller instance;
        private Controller()
        {
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
            walls = new HashSet<Grid>();

            //init window
            gameWindow = new GameWindow();
            gameWindow.Height = GameHeight;
            gameWindow.Width = GameWidth;
            gameWindow.Show();

            gameWindow.DrawGrid(robotSize);
            
            //init players
            player1 = new Player(Direction.Right, new Grid(1, 1), WallColor.Blue);
            //GenerateWall(player1, player1.CurPos);
            player2 = new Player(Direction.Left, new Grid(2, 2), WallColor.Red);
            //GenerateWall(player2, player2.CurPos);

            //set timer -> Update method
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(Update);
            timer.Interval = new TimeSpan(0, 0, 0, 0, timerIntervall); //TimeSpan days/hours/minutes/seconds/milliseconds
            timer.Start();

            stopWatch = new Stopwatch();

            //Start object tracking
            InitTracking();
        }

        private void InitTracking()
        {
            objTracker = new ObjectTracker()
            {
                LEARNING_RATE = Properties.Settings.Default.LearningRate,
                BLOB_MAX_SIZE = Properties.Settings.Default.MaxBlobSize,
                BLOB_MIN_SIZE = Properties.Settings.Default.MinBlobSize
            };

            //start objTracker (threaded)
        }

        /* Thread priority definieren */
        private void Update(object sender, EventArgs e) {
            stopWatch.Start();
            objTracker.track();
            Coordinate firstCarPos = DoPositionCompensation(objTracker.FirstCar.Coord);
            Coordinate secondCarPos = DoPositionCompensation(objTracker.SecondCar.Coord);

            if (IsValidPosition(player1, firstCarPos)) {
                CheckCollision(player1);
                GenerateWall(player1, firstCarPos);
                //determine player position on grid
                player1.CurPos.column = firstCarPos.XCoord / robotSize;
                player1.CurPos.row = firstCarPos.YCoord / robotSize;
            }

            if (IsValidPosition(player2, secondCarPos)) {
                CheckCollision(player2);
                GenerateWall(player2, secondCarPos);
                //determine player position on grid
                player2.CurPos.column = secondCarPos.XCoord / robotSize;
                player2.CurPos.row = secondCarPos.YCoord / robotSize;
            }
            countTicks += 1;
            stopWatch.Stop();
            
            Console.WriteLine("ellapsed time in ms: " + stopWatch.ElapsedMilliseconds);
            stopWatch.Reset();
        }

        private Coordinate DoPositionCompensation(Coordinate coordinates) {
            int x = coordinates.XCoord;
            int y = coordinates.YCoord;
            int center = robotSize / 2;

            if (x == -1 || y == -1) return null;

            x = (x / robotSize) * robotSize;
            y = (y / robotSize) * robotSize;

            return new Coordinate(x, y);
        }

        private bool IsValidPosition(Player player, Coordinate coordinates) {
            if (coordinates == null) return false;
            //coordinates are outside the canvas (gamefield)
            if (coordinates.XCoord < 0 || coordinates.XCoord > GameWidth) return false;
            if (coordinates.YCoord < 0 || coordinates.YCoord > GameHeight) return false;

            //old position equals new position -> no redrawing needed
            if (player.CurPos.column == (coordinates.XCoord/robotSize) && player.CurPos.row == (coordinates.YCoord/robotSize)) {
                return false;
            }
            return true;
        }

        private void GenerateWall(Player player, Coordinate coordinates) {
            if (coordinates == null) return;

#if DEBUG
            //Console.WriteLine("tick-counter: " + countTicks);
            //PrintCoordinates(coordinates);
#endif
            
            gameWindow.DrawWall(coordinates, player.Color);
            
            walls.Add(new Grid (coordinates.XCoord / robotSize, coordinates.YCoord / robotSize));
        }

        private void CheckCollision(Player player) {
            if (walls.Contains(player.CurPos)) {
                //collision detected -> Game Over
                //gameWindow.Close();
                Console.WriteLine("Collision detected");
            }
        }

        private void PrintCoordinates(Coordinate coordinates) {
            Console.WriteLine("-----------");
            Console.Write("x: " + coordinates.XCoord + " | ");
            Console.Write("y: " + coordinates.YCoord);
            Console.WriteLine("-----------");
        }
        
        private void SetRobotDirection(Player player, Direction dir) {
            //TODO: send new direction to dfrobot
        }
    }
}
