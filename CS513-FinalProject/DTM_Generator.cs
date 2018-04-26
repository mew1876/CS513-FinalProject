using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS513_FinalProject
{
    class DTM_Generator
    {
        static List<Point> pointCloud = new List<Point>();

        static void Main(string[] args)
        {
            loadPointCloud("../final_project_point_cloud.fuse");
            foreach(Point point in pointCloud)
            {
                Console.WriteLine(point);
            }
            Console.ReadLine(); //To keep console open till keypress
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
