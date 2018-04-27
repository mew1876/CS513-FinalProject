using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace CS513_FinalProject
{
    class ImageProcessor
    {
        public static Bitmap Convolve(Bitmap original, int windowSize, Func<double[], double> f)
        {
            Bitmap result = new Bitmap(original.Width, original.Height, PixelFormat.Format24bppRgb);
            //Console.WriteLine("h=" + original.Height);
            //Console.WriteLine("w=" + original.Width);
            for (int y = 0; y < original.Height; y++)
            {
                for(int x = 0; x < original.Width; x++)
                {
                    //int windowOriginX = x - (int)Math.Floor((double)windowSize / 2);
                    //int windowOriginY = y - (int)Math.Floor((double)windowSize / 2);
                    int windowOriginX = - (int)Math.Floor((double)windowSize / 2);
                    int windowOriginY = - (int)Math.Floor((double)windowSize / 2);

                    double[] neighbors = new double[windowSize * windowSize];

                    for(int windowY = windowOriginY; windowY < (int)Math.Ceiling((double)windowSize / 2); windowY++)
                    {
                        for (int windowX = windowOriginX; windowX < (int)Math.Ceiling((double)windowSize / 2); windowX++)
                        {
                            int indexX = x + windowX;
                            int indexY = y + windowY;
                            if(indexX >= original.Width || indexX < 0 || indexY >= original.Height || indexY < 0)
                            {
                                neighbors[(windowY - windowOriginY) * windowSize + (windowX - windowOriginX)] = 0;
                            } 
                            else
                            {
                                ColorToHSV(original.GetPixel(indexX, indexY), out double hue, out double saturation, out double value);
                                neighbors[(windowY - windowOriginY) * windowSize + (windowX - windowOriginX)] = hue * value;
                            }
                        }

                    }
                    double newHue = f(neighbors);
                    result.SetPixel(x, y, ColorFromHSV(newHue, 1, newHue > 0 ? 1 : 0));
                }
            }
            return result;
        }

        public static Color[] GetPixelRectangle(Bitmap image, int originX, int originY, int width, int height)
        {
            Color[] colors = new Color[width * height];
            for(int y = originY; y < originY + height; y++)
            {
                for(int x = originX; x < originX + width; x++)
                {
                    if(x > image.Width || y > image.Height)
                    {
                        colors[y * width + x] = Color.Black;
                    }
                    else
                    {
                        colors[(y - originY) * width + (x - originX)] = image.GetPixel(x, y);
                    }
                }
            }
            return colors;
        }

        //Credit to Greg on stack overflow for RGB HSV conversions
        public static void ColorToHSV(Color color, out double hue, out double saturation, out double value)
        {
            int max = Math.Max(color.R, Math.Max(color.G, color.B));
            int min = Math.Min(color.R, Math.Min(color.G, color.B));

            hue = color.GetHue();
            saturation = (max == 0) ? 0 : 1d - (1d * min / max);
            value = max / 255d;
        }

        public static Color ColorFromHSV(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            if (hi == 0)
                return Color.FromArgb(255, v, t, p);
            else if (hi == 1)
                return Color.FromArgb(255, q, v, p);
            else if (hi == 2)
                return Color.FromArgb(255, p, v, t);
            else if (hi == 3)
                return Color.FromArgb(255, p, q, v);
            else if (hi == 4)
                return Color.FromArgb(255, t, p, v);
            else
                return Color.FromArgb(255, v, p, q);
        }
    }
}
