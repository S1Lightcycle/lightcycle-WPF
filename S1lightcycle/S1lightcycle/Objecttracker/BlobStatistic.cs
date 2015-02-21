using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp.Blob;

namespace S1lightcycle.Objecttracker
{
    class BlobStatistic
    {
        private List<int> _blobSizeList; 
        public BlobStatistic(int blobMinSize, int blobMaxSize)
        {
            MinBlobSize = blobMinSize;
            MaxBlobSize = blobMaxSize;
            _blobSizeList = new List<int>();
        }

        public int MinBlobSize { get; set; }

        public int MaxBlobSize { get; set; }

        public void AddBlobs(CvBlob[] blobs)
        {
            foreach (CvBlob blob in blobs)
            {
                _blobSizeList.Add(blob.Area);
            }
        }

        private ulong CalculateMean()
        {
            ulong mean = 0;
            foreach (int blobSize in _blobSizeList)
            {
                mean += (ulong) blobSize;
            }
            if (_blobSizeList.Count > 0)
            {
                return mean/(ulong) _blobSizeList.Count();
            }
            return 0;
        }

        //Not saved yet
        private ulong CalculateConstrainedMean()
        {
            ulong mean = 0;
            ulong count = 0;
            foreach (int blobSize in _blobSizeList)
            {
                if (blobSize <= MaxBlobSize && blobSize >= MinBlobSize)
                {
                    mean += (ulong) blobSize;
                    count++;
                }
            }
            if (count > 0)
            {
                return mean/count;
            }
            return 0;
        }

        /// <summary>
        /// Creates a histogram from the blobArea data
        /// Key     = one histogram slice
        /// Value   = amount of blobs with areas that fall into that slice 
        /// Example: Key: 0, Value: 12 = 12 blobs with areas between 0 and the passed histogramInterval
        /// </summary>
        /// <param name="histogramInterval">Size of one histogram slice</param>
        /// <returns></returns>
        private Dictionary<int, int> CreateHistogram(int histogramInterval)
        {
            var histogram = new Dictionary<int, int>();

            foreach (int blobArea in _blobSizeList)
            {
                int blobInterval = (blobArea/histogramInterval)*histogramInterval;
                if (histogram.ContainsKey(blobInterval))
                {
                    histogram[blobInterval] += 1;
                }
                else
                {
                    histogram.Add(blobInterval, 1);
                }
            }
            return histogram;
        }

        public void Stop()
        {
            Properties.Settings.Default.BlobMean = CalculateMean();
            var asdf = CreateHistogram(1000);
            Properties.Settings.Default.Save();

            _blobSizeList = new List<int>();
        }

    }
}
