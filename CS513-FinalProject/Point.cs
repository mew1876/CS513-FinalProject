using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS513_FinalProject
{
    class Point
    {
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
        }

        public override string ToString()
        {
            return "Latitude: " + latitude + "\tLongitude: " + longitude + "\tElevation: " + elevation + "\tIntensity: " + intensity;
        }
    }
}
