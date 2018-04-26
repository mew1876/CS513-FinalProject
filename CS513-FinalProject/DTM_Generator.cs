using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace CS513_FinalProject
{
    class DTM_Generator
    {
        static List<Point> pointCloud = new List<Point>();
            
        static void Main(string[] args)
        {
            LoadPointCloud("../final_project_point_cloud.fuse");
            pointCloud = pointCloud.OrderBy(point => point.elevation).ToList();
            Bitmap heightMap = GenerateHeightMap(1000, 1000);
            heightMap.Save("Height Map.png");
            //Console.ReadLine(); //To keep console open till keypress
        }

        static Bitmap GenerateHeightMap(int imageWidth, int imageHeight)
        {
            Bitmap heightMap = new Bitmap(imageWidth, imageHeight, PixelFormat.Format24bppRgb);
            using (Graphics g = Graphics.FromImage(heightMap))
            {
                g.FillRectangle(Brushes.Black, 0, 0, imageWidth, imageHeight);
                foreach(Point point in pointCloud)
                {
                    Color pointColor = Color.FromArgb((int)point.GetNormalizedElevation(), 0, 255 - (int)point.GetNormalizedElevation());
                    g.FillRectangle(new SolidBrush(pointColor), (int)(point.GetNormalizedX() * imageWidth), (int)(point.GetNormalizedY() * imageHeight), 5, 5);
                }
            }
            return heightMap;
        }

        static void LoadPointCloud(string path)
        {
            foreach(string line in File.ReadLines(path))
            {
                string[] elements = line.Split(' ');
                Point point = new Point(double.Parse(elements[0]), double.Parse(elements[1]), double.Parse(elements[2]), int.Parse(elements[3]));
                pointCloud.Add(point);
            }
        }
    }
}
