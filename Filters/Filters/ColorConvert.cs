using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Filters
{
    public struct HSVcolor
    {
        public int h; // hue - тон // 0-359
        public int s; // saturation - насыщенность // 0-100
        public int v; // value - яркость // 0-100
        public HSVcolor(int _h, int _s, int _v)
        { h = _h; s = _s; v = _v; }
    }
    class ColorConvert
    {
        public Bitmap rgbImage;
        public HSVcolor[,] hsvImage;

        int Clamp(int value, int min, int max)
        {
            return Math.Min(Math.Max(value, min), max);
        }
        public HSVcolor RGBtoHSV(Color sourse)
        {
            double eps = 0.00001;
            int hue = 0, saturation = 0, value = 0;
            double R_ = sourse.R / 255, G_ = sourse.G / 255, B_ = sourse.B / 255;
            double CMax = Math.Max(R_, Math.Max(G_, B_));
            double CMin = Math.Min(R_, Math.Min(G_, B_));
            double delta = CMax - CMin;

            value = (int)(100 * CMax);
            if (CMax < eps) saturation = 0;
            else saturation = (int)(100 * delta / CMax);
            if (delta < eps) hue = 0;
            else if (CMax == R_)
                hue = (int)(60 * (((G_ - B_) / delta) % 6));
            else if (CMax == G_)
                hue = (int)(60 * (((B_ - R_) / delta) + 2));
            else if (CMax == B_)
                hue = (int)(60 * (((R_ - G_) / delta) + 4));
            return new HSVcolor(Clamp(hue, 0, 359), Clamp(saturation, 0, 100), Clamp(value, 0, 100));
        }
        public Color HSVtoRGB(HSVcolor sourse)
        {
            double R_ = 0, G_ = 0, B_ = 0;
            double C = sourse.s * sourse.v / 100 / 100;
            double X = C * (1 - Math.Abs((sourse.h / 60) % 2 - 1));
            double m = sourse.v / 100 - C;

            if (sourse.h < 60)
            { R_ = C; G_ = X; B_ = 0; }
            else if (sourse.h < 120)
            { R_ = X; G_ = C; B_ = 0; }
            else if (sourse.h < 180)
            { R_ = 0; G_ = C; B_ = X; }
            else if (sourse.h < 240)
            { R_ = 0; G_ = X; B_ = C; }
            else if (sourse.h < 300)
            { R_ = X; G_ = 0; B_ = C; }
            else if (sourse.h < 360)
            { R_ = C; G_ = 0; B_ = X; }

            byte R = (byte)(255 * (R_ + m));
            byte G = (byte)(255 * (G_ + m));
            byte B = (byte)(255 * (B_ + m));

            return Color.FromArgb(Clamp(R, 0, 255), Clamp(G, 0, 255), Clamp(B, 0, 255));
        }
        public void ConvertRGBimagetoHSV()
        {
            int width = rgbImage.Width;
            int height = rgbImage.Height;
            hsvImage = new HSVcolor[width, height];
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    hsvImage[i, j] = RGBtoHSV(rgbImage.GetPixel(i, j));
        }
        public void ConvertHSVimagetoRGB()
        {
            int width = hsvImage.GetLength(0);
            int height = hsvImage.GetLength(1);
            rgbImage = new Bitmap(width, height);
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    rgbImage.SetPixel(i, j, HSVtoRGB(hsvImage[i, j]));
        }
    }
}
