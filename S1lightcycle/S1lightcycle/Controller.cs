using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using S1LightcycleNET;
using System.Windows.Threading;

namespace S1lightcycle {
    public class Controller {
        private Player player1;
        private Player player2;
        private DispatcherTimer timer;
        private ObjectTracker objTracker;
        private GameWindow gameWindow;
        public int gameHeight = 480;
        public int gameWidth = 640;
        private int countTicks = 0;
        private int timerIntervall = 1;    // in ms  berechnen 
        private int robotSize = 30;        //test value; robotsize = gridsize
        private HashSet<Grid> walls;
        System.Diagnostics.Stopwatch stopWatch;

        public void InitGame() {
            //init wall - collision
            walls = new HashSet<Grid>();

            //init window
            gameWindow = new GameWindow();
            gameWindow.Height = gameHeight;
            gameWindow.Width = gameWidth;
            gameWindow.Show();

            gameWindow.DrawGrid(robotSize);
            
            //init players
            player1 = new Player();
            player1.curDirection = Direction.direction.right;
            player1.curPos = new Grid(1,1);
            player1.color = WallColor.Blue;
            //GenerateWall(player1, player1.curPos);

            player2 = new Player();
            player2.curDirection = Direction.direction.left;
            player2.curPos = new Grid(2, 2);
            player2.color = WallColor.Red;
            //GenerateWall(player2, player2.curPos);

            //set timer -> Update method
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(Update);
            timer.Interval = new TimeSpan(0, 0, 0, 0, timerIntervall); //TimeSpan days/hours/minutes/seconds/milliseconds
            timer.Start();

            stopWatch = new System.Diagnostics.Stopwatch();

            //Start object tracking
            InitTracking();
        }

        private void InitTracking() {
            objTracker = new ObjectTracker();
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
                player1.curPos.column = firstCarPos.XCoord / robotSize;
                player1.curPos.row = firstCarPos.YCoord / robotSize;
            }

            if (IsValidPosition(player2, secondCarPos)) {
                CheckCollision(player2);
                GenerateWall(player2, secondCarPos);
                //determine player position on grid
                player2.curPos.column = secondCarPos.XCoord / robotSize;
                player2.curPos.row = secondCarPos.YCoord / robotSize;
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
            if (coordinates.XCoord < 0 || coordinates.XCoord > gameWidth) return false;
            if (coordinates.YCoord < 0 || coordinates.YCoord > gameHeight) return false;

            //old position equals new position -> no redrawing needed
            if (player.curPos.column == (coordinates.XCoord/robotSize) && player.curPos.row == (coordinates.YCoord/robotSize)) {
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
            
            gameWindow.DrawWall(coordinates, player.color);
            
            walls.Add(new Grid (coordinates.XCoord / robotSize, coordinates.YCoord / robotSize));
        }

        private void CheckCollision(Player player) {
            if (walls.Contains(player.curPos)) {
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
        
        private void SetRobotDirection(Player player, Direction.direction dir) {
            //TODO: send new direction to dfrobot
        }
    }
}
