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
        private int _minBlobSize;
        private int _maxBlobSize;

        private List<int> _blobSizeList; 
        public BlobStatistic(int blobMinSize, int blobMaxSize)
        {
            _minBlobSize = blobMinSize;
            _maxBlobSize = blobMaxSize;
            _blobSizeList = new List<int>();
        }

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
                if (blobSize <= _maxBlobSize && blobSize >= _minBlobSize)
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

        public void Stop()
        {
            Properties.Settings.Default.BlobMean = CalculateMean();
            Properties.Settings.Default.Save();

            _blobSizeList = new List<int>();
        }

    }
}
