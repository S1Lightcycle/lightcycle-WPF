using S1lightcycle.Objecttracker;
using System.Collections.Generic;
using System.Diagnostics;

namespace S1lightcycle 
{
    public class Player 
    {
        public WallColor Color;
        public Grid CurPos;
        public Robot Robot;
        public Queue<Grid> OldPositions;

        public Player(Grid curPos, WallColor color)
        {
            CurPos = curPos;
            Color = color;
            OldPositions = new Queue<Grid>(5);
        }

        public bool IsOldPosition()
        {
            foreach (Grid g in OldPositions)
            {
                Trace.WriteLine("old-Position: column: " + g.Column + " row: " + g.Row + "new-Position: column: " + CurPos.Column + " row: " + CurPos.Row);
                //Trace.WriteLine("new-Position: column: " + CurPos.Column + " row: " + CurPos.Row);
                if (CurPos.Equals(g))
                {
                    Trace.WriteLine("is old position");
                    return true;
                }
            }
            return false;
        }
    }
}
