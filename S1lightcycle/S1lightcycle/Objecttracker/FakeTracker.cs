using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S1lightcycle.Objecttracker
{
    public class FakeTracker : AbstractObjectTracker
    {

        public FakeTracker()
        {
            FirstCar = new Robot(-1, -1);
            SecondCar = new Robot(-1, -1);

            FirstCar.Coord.Enqueue(new Coordinate(75, 75));
            FirstCar.Coord.Enqueue(new Coordinate(225, 75));
            FirstCar.Coord.Enqueue(new Coordinate(375, 75));

            SecondCar.Coord.Enqueue(new Coordinate(75, 225));
            SecondCar.Coord.Enqueue(new Coordinate(225, 225));
            SecondCar.Coord.Enqueue(new Coordinate(375, 225));

        }

        public override void StartTracking()
        {
        }

        public override void StopTracking()
        {
        }

        public override void Track()
        {
        }
    }
}
