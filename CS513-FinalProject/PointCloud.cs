using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace CS513_FinalProject
{
    class PointCloud : List<Point>
    {
        private static double minLatitude = double.MaxValue;
        private static double minLongitude = double.MaxValue;
        private static double minElevation = double.MaxValue;
        private static double maxLatitude = double.MinValue;
        private static double maxLongitude = double.MinValue;
        private static double maxElevation = double.MinValue;

        public Bitmap GetHeightMap(int imageWidth, int imageHeight)
        {
            double latRange = maxLatitude - minLatitude;
            double longRange = maxLongitude - minLongitude;

            imageHeight = 1080;
            imageWidth = (int)(imageHeight / longRange / Math.Sin(Math.PI / 180 * minLatitude) * latRange);

            Bitmap heightMap = new Bitmap(imageWidth, imageHeight, PixelFormat.Format24bppRgb);
            using (Graphics g = Graphics.FromImage(heightMap))
            {
                g.FillRectangle(Brushes.Black, 0, 0, imageWidth, imageHeight);
                foreach (Point point in this.AsEnumerable())
                {
                    HsvToRgb(180 * (1 - GetNormalizedElevation(point)), 1, 1, out int red, out int green, out int blue);
                    Color pointColor = Color.FromArgb(red, green, blue);
                    int drawX = (int)(GetNormalizedX(point, imageWidth, imageHeight));
                    int drawY = (int)(GetNormalizedY(point, imageWidth, imageHeight));
                    int size = 1;/* + (int)(9 * (1 - GetNormalizedElevation(point)));*/
                    g.FillRectangle(new SolidBrush(pointColor), drawX, imageHeight - drawY, size, size);
                }
            }
            return heightMap;
        }

        public double ConvertToRad(double inputLongitude)
        {
            return inputLongitude * Math.PI / 180;
        }

        public double ConvertToMercN(double latitudeInRadians)
        {
            return Math.Log(Math.Tan((Math.PI / 4) + (latitudeInRadians / 2)));
        }

        public double ConvertMercNToX(double mercN, int imageWidth, int imageHeight)
        {
            return (imageHeight / 2) - (imageWidth * mercN / (2 * Math.PI));
        }

        public double GetNormalizedY(Point point, int imageWidth, int imageHeight)
        {
            double x = (point.latitude + 180) * (imageWidth / 360);
            double xmin = (minLatitude + 180) * (imageWidth / 360);
            double xmax = (maxLatitude + 180) * (imageWidth / 360);

            double returnValue = (x - xmin) / (xmax - xmin);
            //Console.WriteLine("y=" + returnValue);
            return returnValue * imageWidth;
        }

        public double GetNormalizedX(Point point, int imageWidth, int imageHeight)
        {
            // convert from degrees to radians
            double latRad = ConvertToRad(point.longitude);
            double minLatRad = ConvertToRad(minLongitude);
            double maxLatRad = ConvertToRad(maxLongitude);

            // get y value
            double mercN = ConvertToMercN(latRad);
            double minMercN = ConvertToMercN(minLatRad);
            double maxMercN = ConvertToMercN(maxLatRad);

            double y = ConvertMercNToX(mercN, imageWidth, imageHeight);
            double ymin = ConvertMercNToX(minMercN, imageWidth, imageHeight);
            double ymax = ConvertMercNToX(maxMercN, imageWidth, imageHeight);

            double returnValue = (y - ymin) / (ymax - ymin);
            //Console.WriteLine("y=" + returnValue);
            return returnValue * imageHeight;
        }

        public double GetNormalizedElevation(Point point)
        {
            return (point.elevation - minElevation) / (maxElevation - minElevation);
        }

        public void RemoveHighElevationOutliers()
        {
            double meanElevation = GetMeanElevation();
            double stdevElevation = GetStandardDeviationElevation(meanElevation);

            for (int i = this.Count - 1; i >= 0; i--)
            {
                if (this[i].elevation - meanElevation > 1 * stdevElevation)
                {
                    this.RemoveAt(i);
                }
            }
            UpdateMinMax();
        }

        double GetMeanElevation()
        {
            double summation = 0;
            double count = 0;
            foreach (Point point in this.AsEnumerable())
            {
                summation += point.elevation;
                count += 1;
            }
            return summation / count;
        }

        double GetStandardDeviationElevation(double mean)
        {
            double summation = 0;
            double count = 0;
            foreach (Point point in this.AsEnumerable())
            {
                summation += Math.Pow(point.elevation - mean, 2);
                count += 1;
            }
            return Math.Sqrt((1 / count) * summation);
        }

        new void Add(Point point)
        {
            if (point.latitude < minLatitude)
            {
                minLatitude = point.latitude;
            }
            if (point.longitude < minLongitude)
            {
                minLongitude = point.longitude;
            }
            if (point.elevation < minElevation)
            {
                minElevation = point.elevation;
            }
            if (point.latitude > maxLatitude)
            {
                maxLatitude = point.latitude;
            }
            if (point.longitude > maxLongitude)
            {
                maxLongitude = point.longitude;
            }
            if (point.elevation > maxElevation)
            {
                maxElevation = point.elevation;
            }
            base.Add(point);
        }

        void UpdateMinMax()
        {
            minLatitude = double.MaxValue;
            minLongitude = double.MaxValue;
            minElevation = double.MaxValue;
            maxLatitude = double.MinValue;
            maxLongitude = double.MinValue;
            maxElevation = double.MinValue;

            foreach (Point point in this.AsEnumerable())
            {
                if (point.latitude < minLatitude)
                {
                    minLatitude = point.latitude;
                }
                if (point.longitude < minLongitude)
                {
                    minLongitude = point.longitude;
                }
                if (point.elevation < minElevation)
                {
                    minElevation = point.elevation;
                }
                if (point.latitude > maxLatitude)
                {
                    maxLatitude = point.latitude;
                }
                if (point.longitude > maxLongitude)
                {
                    maxLongitude = point.longitude;
                }
                if (point.elevation > maxElevation)
                {
                    maxElevation = point.elevation;
                }
            }
        }
        void HsvToRgb(double h, double S, double V, out int r, out int g, out int b)
        {
            // ######################################################################
            // T. Nathan Mundhenk
            // mundhenk@usc.edu
            // C/C++ Macro HSV to RGB

            double H = h;
            while (H < 0) { H += 360; };
            while (H >= 360) { H -= 360; };
            double R, G, B;
            if (V <= 0)
            { R = G = B = 0; }
            else if (S <= 0)
            {
                R = G = B = V;
            }
            else
            {
                double hf = H / 60.0;
                int i = (int)Math.Floor(hf);
                double f = hf - i;
                double pv = V * (1 - S);
                double qv = V * (1 - S * f);
                double tv = V * (1 - S * (1 - f));
                switch (i)
                {

                    // Red is the dominant color

                    case 0:
                        R = V;
                        G = tv;
                        B = pv;
                        break;

                    // Green is the dominant color

                    case 1:
                        R = qv;
                        G = V;
                        B = pv;
                        break;
                    case 2:
                        R = pv;
                        G = V;
                        B = tv;
                        break;

                    // Blue is the dominant color

                    case 3:
                        R = pv;
                        G = qv;
                        B = V;
                        break;
                    case 4:
                        R = tv;
                        G = pv;
                        B = V;
                        break;

                    // Red is the dominant color

                    case 5:
                        R = V;
                        G = pv;
                        B = qv;
                        break;

                    // Just in case we overshoot on our math by a little, we put these here. Since its a switch it won't slow us down at all to put these here.

                    case 6:
                        R = V;
                        G = tv;
                        B = pv;
                        break;
                    case -1:
                        R = V;
                        G = pv;
                        B = qv;
                        break;

                    // The color is not defined, we should throw an error.

                    default:
                        //LFATAL("i Value error in Pixel conversion, Value is %d", i);
                        R = G = B = V; // Just pretend its black/white
                        break;
                }
            }
            r = Clamp((int)(R * 255.0));
            g = Clamp((int)(G * 255.0));
            b = Clamp((int)(B * 255.0));
        }

        /// <summary>
        /// Clamp a value to 0-255
        /// </summary>
        int Clamp(int i)
        {
            if (i < 0) return 0;
            if (i > 255) return 255;
            return i;
        }
    }
}
