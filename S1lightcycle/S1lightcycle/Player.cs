using S1lightcycle.Objecttracker;

namespace S1lightcycle {
    public class Player {
        public Direction CurDirection;
        public WallColor Color;
        public Grid CurPos;
        public Robot Robot;

        public Player() { }

        public Player(Direction dir, Grid curPos, WallColor color)
        {
            CurDirection = dir;
            CurPos = curPos;
            Color = color;
        }
    }
}
