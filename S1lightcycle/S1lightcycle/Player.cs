
using S1LightcycleNET;

namespace S1lightcycle {
    public class Player {
        public Direction CurDirection;
        public WallColor Color;
        public Grid CurPos;
        public Robot Robot;

        public Player() { }

        public Player(Direction dir, Grid curPos, WallColor color, Robot robot)
        {
            CurDirection = dir;
            CurPos = curPos;
            Color = color;
            Robot = robot;
        }
    }
}
