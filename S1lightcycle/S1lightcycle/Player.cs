using S1lightcycle.Objecttracker;

namespace S1lightcycle 
{
    public class Player 
    {
        public WallColor Color;
        public Grid CurPos;
        public Robot Robot;

        public Player(Grid curPos, WallColor color)
        {
            CurPos = curPos;
            Color = color;
        }
    }
}
