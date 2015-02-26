using System.Diagnostics;
namespace S1lightcycle 
{
    public class Grid {
        public int Row;
        public int Column;

        public Grid(int x, int y) 
        {
            Row = x;
            Column = y;
        }
        

        public override bool Equals(object obj) 
        {
            Grid newGrid = obj as Grid;
            if (newGrid == null)
            {
                return false;
            }
            Trace.WriteLine(newGrid.Row + " " + newGrid.Column);
            if (newGrid.Row != this.Row || newGrid.Column != this.Column) 
            {
                return false;
            }

            return true;
        }
    }
}
