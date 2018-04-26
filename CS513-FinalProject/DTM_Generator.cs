using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace CS513_FinalProject
{
    class DTM_Generator
    {
        static List<Point> pointCloud = new List<Point>();
            
        static void Main(string[] args)
        {
            loadPointCloud("../final_project_point_cloud.fuse");
            pointCloud = pointCloud.OrderBy(point => point.elevation).ToList();
            foreach (Point point in pointCloud)
            {
                Console.WriteLine(point.elevation);
            }
            Console.ReadLine(); //To keep console open till keypress
        }

        static Bitmap generateHeightMap(int imageWidth, int imageHeight)
        {
            //draw points to bitmap
        }

        static void loadPointCloud(string path)
        {
            foreach(string line in File.ReadLines(path))
            {
                string[] elements = line.Split(' ');
                Point point = new Point(float.Parse(elements[0]), float.Parse(elements[1]), float.Parse(elements[2]), int.Parse(elements[3]));
                pointCloud.Add(point);
            }
        }
    }
}
