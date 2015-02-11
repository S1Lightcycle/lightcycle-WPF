using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using S1lightcycle.Objecttracker;
using System.Windows.Media;

namespace S1lightcycle.Windows {
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window {

        public int GridWidth { get; private set; }
        public int GridHeight { get; private set; }
         
        public GameWindow() {
            InitializeComponent();
        }

        public void DrawGrid(int gridSize) {
            Console.WriteLine("width: " + Width + " height: " + Height);

            int maxVert = (int)Width / gridSize;
            int remainderVert = (int)Width - (maxVert * gridSize);
            int maxHor = (int)Height / gridSize;
            int remainderHor = (int)Height - (maxHor * gridSize);

            //vertical grid
            for (int i = 0; (i * gridSize) <= Width; i++) {
                DrawGridLine(i * gridSize, i * gridSize, 0, Height);
                GridWidth = i;
            }

            //horizontal grid
            for (int j = 0; (j * gridSize) <= Height; j++) {
                DrawGridLine(0, Width, j * gridSize, j * gridSize);
                GridHeight = j;
            }

            Rectangle rect1 = InitRect((int)Width, gridSize);
            Canvas.SetLeft(rect1, 0);
            Canvas.SetTop(rect1, 0);

            Rectangle rect2 = InitRect((int)Width, gridSize);
            Canvas.SetLeft(rect2, 0);
            Canvas.SetTop(rect1, Height - remainderHor);

            Rectangle rect3 = InitRect(gridSize, (int)Height);
            Canvas.SetLeft(rect3, 0);
            Canvas.SetTop(rect3, 0);

            Rectangle rect4 = InitRect(gridSize, (int)Height);
            Canvas.SetLeft(rect4, Width - remainderVert);
            Canvas.SetTop(rect4, 0);

            GameFieldCanvas.Children.Add(rect1);
            GameFieldCanvas.Children.Add(rect2);
            GameFieldCanvas.Children.Add(rect3);
            GameFieldCanvas.Children.Add(rect4);
        }

        private Rectangle InitRect(int width, int height)
        {
            Rectangle rect = new Rectangle();
            rect.Stroke = new SolidColorBrush(Colors.Black);
            rect.Fill = new SolidColorBrush(Colors.Black);
            rect.Width = width;
            rect.Height = height;
            return rect;
        }

        public void DrawGridLine(double x1, double x2, double y1, double y2) {
            Line gridLine = new Line();
            gridLine.Stroke = Brushes.Black;
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
                    newWall.Fill = Brushes.Black;
                    break;
                case WallColor.Red:
                    newWall.Fill = Brushes.Red;
                    break;
                case WallColor.Blue:
                    newWall.Fill = Brushes.Blue;
                    break;
                case WallColor.White:
                    newWall.Fill = Brushes.White;
                    break;
            }
            newWall.Width = Controller.RobotSize;
            newWall.Height = Controller.RobotSize;

            Canvas.SetLeft(newWall, coordinates.XCoord*Controller.RobotSize);
            Canvas.SetTop(newWall, coordinates.YCoord*Controller.RobotSize);

            GameFieldCanvas.Children.Add(newWall);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            Controller.Instance.Move(e.Key);
        }
     }
}
