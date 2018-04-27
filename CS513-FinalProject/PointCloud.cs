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

        public Bitmap GetHeightMap()
        {
            double latRange = maxLatitude - minLatitude;
            double longRange = maxLongitude - minLongitude;

            double hwRatio = this.GetNormalizedHWRatio();
            int imageHeight = 1080 ;
            int imageWidth = (int)(1080 / (hwRatio));

            Bitmap heightMap = new Bitmap(imageWidth, imageHeight, PixelFormat.Format24bppRgb);
            using (Graphics g = Graphics.FromImage(heightMap))
            {
                g.FillRectangle(Brushes.Black, 0, 0, imageWidth, imageHeight);
                foreach (Point point in this.AsEnumerable())
                {
                    Color pointColor = ImageProcessor.ColorFromHSV(180 * (1 - GetNormalizedElevation(point)), 1, 1);
                    int drawX = (int)(GetNormalizedX(point, imageWidth, imageHeight));
                    int drawY = (int)(GetNormalizedY(point, imageWidth, imageHeight));
                    int size = 1;/* + (int)(9 * (1 - GetNormalizedElevation(point)));*/
                    g.FillRectangle(new SolidBrush(pointColor), drawX, imageHeight - drawY, size, size);
                }
            }
            return heightMap;
        }

        public double GetNormalizedY(Point point, int imageWidth, int imageHeight)
        {
            double y = 0.5 - Math.Log((1 + Math.Sin(point.latitude * Math.PI / 180)) / (1 - Math.Sin(point.latitude * Math.PI / 180))) / (4 * Math.PI);
            double ymin = 0.5 - Math.Log((1 + Math.Sin(minLatitude * Math.PI / 180)) / (1 - Math.Sin(minLatitude * Math.PI / 180))) / (4 * Math.PI);
            double ymax = 0.5 - Math.Log((1 + Math.Sin(maxLatitude * Math.PI / 180)) / (1 - Math.Sin(maxLatitude * Math.PI / 180))) / (4 * Math.PI);

            double returnValue = (y - ymin) / (ymax - ymin);

            return returnValue * imageWidth;
        }

        public double GetNormalizedHWRatio()
        {
            double xmin = (minLongitude + 180) / 360;
            double ymin = 0.5 - Math.Log((1 + Math.Sin(minLatitude * Math.PI / 180)) / (1 - Math.Sin(minLatitude * Math.PI / 180))) / (4 * Math.PI);
            double xmax = (maxLongitude + 180) / 360;
            double ymax = 0.5 - Math.Log((1 + Math.Sin(maxLatitude * Math.PI / 180)) / (1 - Math.Sin(maxLatitude * Math.PI / 180))) / (4 * Math.PI);

            double returnValue = Math.Abs(ymax - ymin) / Math.Abs(xmax - xmin);

            return Math.Abs(ymax - ymin) / Math.Abs(xmax - xmin); // height difference / width difference = h-w ratio
        }

        public double GetNormalizedX(Point point, int imageWidth, int imageHeight)
        {
            double x = (point.longitude + 180) / 360;
            double xmin = (minLongitude + 180) / 360;
            double xmax = (maxLongitude + 180) / 360;


            double returnValue = (x - xmin) / (xmax - xmin);            
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
    }
}
