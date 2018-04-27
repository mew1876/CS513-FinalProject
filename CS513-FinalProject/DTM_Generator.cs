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
        static void Main(string[] args)
        {
            PointCloud pointCloud = LoadPointCloud("../final_project_point_cloud.fuse");
            pointCloud.RemoveHighElevationOutliers();
            pointCloud.Sort((point1, point2) => -1 * point1.elevation.CompareTo(point2.elevation));
            Bitmap heightMap = pointCloud.GetHeightMap(1000, 1000);
            heightMap.Save("Height Map.png");
            //Console.ReadLine(); //To keep console open till keypress
        }

        static PointCloud LoadPointCloud(string path)
        {
            PointCloud pointCloud = new PointCloud();
            foreach(string line in File.ReadLines(path))
            {
                string[] elements = line.Split(' ');
                Point point = new Point(double.Parse(elements[0]), double.Parse(elements[1]), double.Parse(elements[2]), int.Parse(elements[3]));
                pointCloud.Add(point);
            }
            return pointCloud;
        }
    }
}
