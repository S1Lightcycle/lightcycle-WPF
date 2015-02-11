using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S1lightcycle.Objecttracker
{
    public abstract class AbstractObjectTracker
    {
        public Robot FirstCar { get; set; }
        public Robot SecondCar { get; set; }
        public int BlobMinSize { get; set; }
        public int BlobMaxSize { get; set; }

        //determines how fast stationary objects are incorporated into the background mask ( higher = faster)
        public double LearningRate { get; set; }

        public abstract void StartTracking();

        public abstract void StopTracking();

        public abstract void Track();
    }
}
