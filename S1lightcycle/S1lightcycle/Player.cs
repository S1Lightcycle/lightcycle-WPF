using S1lightcycle.Objecttracker;
using System.Collections.Generic;

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
            OldPositions = new Queue<Grid>(3);
        }

        public bool IsOldPosition()
        {
            foreach (Grid g in OldPositions) 
            {
                if (CurPos.Equals(g)) 
                {
                    return true;
                }
            }
            return false;
        }
    }
}
