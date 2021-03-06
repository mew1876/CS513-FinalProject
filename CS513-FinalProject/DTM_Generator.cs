﻿using System;
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
            Console.WriteLine("Point cloud loaded.");
            pointCloud.RemoveHighElevationOutliers();
            pointCloud.Sort((point1, point2) => -1 * point1.elevation.CompareTo(point2.elevation));
            Console.WriteLine("Point cloud trimmed and sorted");
            Bitmap heightMap = pointCloud.GetHeightMap();
            Console.WriteLine("Raw height map generated");
            //ImageProcessor.SmoothGridSquares(heightMap, 15);
            Bitmap smoothMap = ImageProcessor.Convolve(heightMap, 10, (neighbors) => neighbors.Max());
            smoothMap = ImageProcessor.Convolve(smoothMap, 30, SpecialAverage);
            heightMap.Save("Height Map.png");
            smoothMap.Save("Smooth Map.png");
            //Console.ReadLine(); //To keep console open till keypress
        }

        static double SpecialAverage(double[] neighbors)
        {
            double meanNoBlack = 0;
            int nNoBlack = 0;
            foreach(double neighbor in neighbors)
            {
                if(neighbor > 0)
                {
                    nNoBlack++;
                    meanNoBlack += neighbor;
                }
            }
            if(nNoBlack == 0)
            {
                return 0;
            }
            meanNoBlack /= nNoBlack;
            return meanNoBlack;
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
