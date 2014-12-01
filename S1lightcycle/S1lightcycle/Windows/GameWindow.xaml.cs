using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using S1LightcycleNET;

namespace S1lightcycle.Windows {
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window {
         
        public GameWindow() {
            InitializeComponent();
            this.KeyDown += new KeyEventHandler(Grid_KeyDown);
        }

        public void DrawGrid(int gridSize) {
            //vertical grid
            for (int i = 0; (i * gridSize) <= this.Width; i++) {
                DrawGridLine(i * gridSize, i * gridSize, 0, this.Height);
            }
            //horizontal grid
            for (int j = 0; (j * gridSize) <= this.Height; j++) {
                DrawGridLine(0, this.Width, j * gridSize, j * gridSize);
            }
        }

        public void DrawGridLine(double x1, double x2, double y1, double y2) {
            Line gridLine = new Line();
            gridLine.Stroke = System.Windows.Media.Brushes.Black;
            gridLine.StrokeThickness = 3;

            gridLine.X1 = x1;
            gridLine.X2 = x2;
            gridLine.Y1 = y1;
            gridLine.Y2 = y2;

            GameFieldCanvas.Children.Add(gridLine);
        }

        public void DrawWall(Coordinate coordinates, WallColor color) {
            Rectangle newWall = new Rectangle();

            switch (color)
            {
                case WallColor.Black:
                    newWall.Fill = System.Windows.Media.Brushes.Black;
                    break;
                case WallColor.Red:
                    newWall.Fill = System.Windows.Media.Brushes.Red;
                    break;
                case WallColor.Blue:
                    newWall.Fill = System.Windows.Media.Brushes.Blue;
                    break;
            }
            newWall.Width = 30;
            newWall.Height = 30;

            Canvas.SetTop(newWall, coordinates.XCoord);
            Canvas.SetLeft(newWall, coordinates.YCoord);

            GameFieldCanvas.Children.Add(newWall);
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e) {
            switch (e.Key) {
                case Key.Down:
                    Console.WriteLine("player1: " + e.Key.ToString());
                    //if (player1.CurDirection == Direction.direction.up) return;
                    //player1.CurDirection = Direction.direction.down;
                    break;
                case Key.Up:
                    Console.WriteLine("player1: " + e.Key.ToString());
                    //if (player1.CurDirection == Direction.direction.down) return;
                    //player1.CurDirection = Direction.direction.up;
                    break;
                case Key.Right:
                    Console.WriteLine("player1: " + e.Key.ToString());
                    //if (player1.CurDirection == Direction.direction.left) return;
                    //player1.CurDirection = Direction.direction.right;
                    break;
                case Key.Left:
                    Console.WriteLine("player1: " + e.Key.ToString());
                    //if (player1.CurDirection == Direction.direction.right) return;
                    //player1.CurDirection = Direction.direction.left;
                    break;
                case Key.D:
                    //if (player2.CurDirection == Direction.direction.left) return;
                    //player2.CurDirection = Direction.direction.right;
                    break;
                case Key.A:
                    //if (player2.CurDirection == Direction.direction.right) return;
                    //player2.CurDirection = Direction.direction.left;
                    break;
                case Key.S:
                    //if (player2.CurDirection == Direction.direction.up) return;
                    //player2.CurDirection = Direction.direction.down;
                    break;
                case Key.W:
                    //if (player2.CurDirection == Direction.direction.down) return;
                    //player2.CurDirection = Direction.direction.up;
                    break;
                default:
                    Console.WriteLine("Not yet supported.");
                    break;
            } 
        }
    }
}
