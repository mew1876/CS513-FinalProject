using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS513_FinalProject
{
    class Point
    {
        private static float minLatitude = float.MaxValue;
        private static float minLongitude = float.MaxValue;
        private static float maxLatitude = float.MinValue;
        private static float maxLongitude = float.MinValue;

        public float latitude;
        public float longitude;
        public float elevation;
        public int intensity;

        public Point(float latitude, float longitude, float elevation, int intensity)
        {
            this.latitude = latitude;
            this.longitude = longitude;
            this.elevation = elevation;
            this.intensity = intensity;

            if (latitude < minLatitude)
            {
                maxLatitude = latitude;
            }
            if (latitude > maxLatitude)
            {
                minLatitude = latitude;
            }
            if (longitude < minLongitude)
            {
                minLongitude = longitude;
            }
            if (longitude > maxLongitude)
            {
                maxLongitude = longitude;
            }
        }

        //get transformed points between 0 and 1
        public float getImageX()
        {

        }

        public float getImageY()
        {

        }

        public override string ToString()
        {
            return "Latitude: " + latitude + "\tLongitude: " + longitude + "\tElevation: " + elevation + "\tIntensity: " + intensity;
        }
    }
}
