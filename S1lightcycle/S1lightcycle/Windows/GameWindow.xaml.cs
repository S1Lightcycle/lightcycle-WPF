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

        public int GridWidth { get; private set; }
        public int GridHeight { get; private set; }
         
        public GameWindow(Controller controller) {
            InitializeComponent();
        }

        

        public void DrawGrid(int gridSize) {
            //vertical grid
            for (int i = 0; (i * gridSize) <= this.Width; i++) {
                DrawGridLine(i * gridSize, i * gridSize, 0, this.Height);
                GridWidth = i;
            }

            //horizontal grid
            for (int j = 0; (j * gridSize) <= this.Height; j++) {
                DrawGridLine(0, this.Width, j * gridSize, j * gridSize);
                GridHeight = j;
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
                case WallColor.White:
                    newWall.Fill = System.Windows.Media.Brushes.White;
                    break;
            }
            newWall.Width = Controller.RobotSize;
            newWall.Height = Controller.RobotSize;

            Canvas.SetTop(newWall, coordinates.XCoord);
            Canvas.SetLeft(newWall, coordinates.YCoord);

            GameFieldCanvas.Children.Add(newWall);
        }

        
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            Controller.Instance.move(e.Key);
        }
     }
}
