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
                minLatitude = latitude;
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
                maxLatitude = latitude;
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

        public double ConvertToRad(double inputLongitude)
        {
            return inputLongitude * Math.PI / 180;
        }

        public double ConvertToMercN(double latitudeInRadians)
        {
            return Math.Log(Math.Tan((Math.PI / 4) + (latitudeInRadians / 2)));
        }

        public double ConvertMercNToY(double mercN, int imageWidth, int imageHeight)
        {
            return (imageHeight / 2) - (imageWidth * mercN / (2 * Math.PI));
        }

        //get transformed points between 0 and 1
        public double GetNormalizedX(int imageWidth, int imageHeight)
        {
            double x = (latitude + 180) * (imageWidth / 360);
            double xmin = (minLatitude + 180) * (imageWidth / 360);
            double xmax = (maxLatitude + 180) * (imageWidth / 360);

            double returnValue = (x - xmin) / (xmax - xmin);
            //Console.WriteLine("y=" + returnValue);
            return returnValue * imageWidth;
        }

        public double GetNormalizedY(int imageWidth, int imageHeight)
        {
            // convert from degrees to radians
            double latRad = ConvertToRad(longitude);
            double minLatRad = ConvertToRad(minLongitude);
            double maxLatRad = ConvertToRad(maxLongitude);

            // get y value
            double mercN = ConvertToMercN(latRad);
            double minMercN = ConvertToMercN(minLatRad);
            double maxMercN = ConvertToMercN(maxLatRad);

            double y = ConvertMercNToY(mercN, imageWidth, imageHeight);
            double ymin = ConvertMercNToY(minMercN, imageWidth, imageHeight);
            double ymax = ConvertMercNToY(maxMercN, imageWidth, imageHeight);

            double returnValue = (y - ymin) / (ymax - ymin);
            //Console.WriteLine("y=" + returnValue);
            return returnValue * imageHeight;
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
