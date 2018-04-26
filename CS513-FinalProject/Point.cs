using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS513_FinalProject
{
    class Point
    {
        private static double minLatitude = double.MaxValue;
        private static double minLongitude = double.MaxValue;
        private static double minElevation = double.MaxValue;
        private static double maxLatitude = double.MinValue;
        private static double maxLongitude = double.MinValue;
        private static double maxElevation = double.MinValue;

        public double latitude;
        public double longitude;
        public double elevation;
        public int intensity;

        public Point(double latitude, double longitude, double elevation, int intensity)
        {
            this.latitude = latitude;
            this.longitude = longitude;
            this.elevation = elevation;
            this.intensity = intensity;

            if (latitude < minLatitude)
            {
                maxLatitude = latitude;
            }
            if (longitude < minLongitude)
            {
                minLongitude = longitude;
            }
            if (elevation < minElevation)
            {
                minElevation = elevation;
            }
            if (latitude > maxLatitude)
            {
                minLatitude = latitude;
            }
            if (longitude > maxLongitude)
            {
                maxLongitude = longitude;
            }
            if (elevation > maxElevation)
            {
                maxElevation = elevation;
            }
        }

        //get transformed points between 0 and 1
        public double GetNormalizedX()
        {
            return (latitude - minLatitude) / (maxLatitude - minLatitude);
        }

        public double GetNormalizedY()
        {
            return (longitude - minLongitude) * Math.Cos(Math.PI / 180 * latitude) / (maxLongitude - minLongitude);
        }

        public double GetNormalizedElevation()
        {
            return 255 * (elevation - minElevation) / (maxElevation - minElevation);
        }

        public override string ToString()
        {
            return "Latitude: " + latitude + "\tLongitude: " + longitude + "\tElevation: " + elevation + "\tIntensity: " + intensity;
        }
    }
}
