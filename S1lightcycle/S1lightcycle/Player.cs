﻿
namespace S1lightcycle {
    public class Player {
        public Direction CurDirection;
        public Grid CurPos;
        public WallColor Color;

        public Player() { }

        public Player(Direction dir, Grid curPos, WallColor color)
        {
            CurDirection = dir;
            CurPos = curPos;
            Color = color;
        }
    }
}
