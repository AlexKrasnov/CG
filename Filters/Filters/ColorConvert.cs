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
        public ushort h; // hue - тон // 0-359
        public byte s; // saturation - насыщенность // 0-100
        public byte v; // value - яркость // 0-100
        public HSVcolor(ushort _h, byte _s, byte _v)
        { h = _h; s = _s; v = _v; }
    }
    class ColorConvert
    {
        public Bitmap rgbImage;
        public HSVcolor[,] hsvImage;

        byte clamp(byte value, byte min, byte max)
        {
            return Math.Min(Math.Max(value, min), max);
        }
        ushort clamp(ushort value, ushort min, ushort max)
        {
            return Math.Min(Math.Max(value, min), max);
        }
        public HSVcolor RGBtoHSV(Color sourse)
        {
            float eps = 0.00001f; ushort hue = 0; byte saturation = 0, value = 0;
            float R_ = sourse.R / 255f; float G_ = sourse.G / 255f; float B_ = sourse.B / 255f;
            float CMax = Math.Max(R_, Math.Max(G_, B_));
            float CMin = Math.Min(R_, Math.Min(G_, B_));
            float delta = CMax - CMin;
            value = (byte)(100 * CMax);

            if (CMax < eps) saturation = 0;
            else saturation = (byte)(100 * delta / CMax);
            if (delta < eps) hue = 0;
            else if (CMax == R_)
                hue = (ushort)(60f * (((G_ - B_) / delta) % 6f));
            else if (CMax == G_)
                hue = (ushort)(60f * (((B_ - R_) / delta) + 2f));
            else if (CMax == B_)
                hue = (ushort)(60f * (((R_ - G_) / delta) + 4f));
            return new HSVcolor(clamp(hue, 0, 359), saturation, value);
        }
        public Color HSVtoRGB(HSVcolor sourse)
        {
            float R_ = 0f, G_ = 0f, B_ = 0f;
            float C = sourse.s * sourse.v / 100f / 100f;
            float X = C * (1 - Math.Abs((sourse.h / 60f) % 2f - 1));
            float m = sourse.v / 100f - C;

            if (sourse.h < 60)
            { R_ = C; G_ = X; B_ = 0f; }
            else if (sourse.h < 120)
            { R_ = X; G_ = C; B_ = 0f; }
            else if (sourse.h < 180)
            { R_ = 0f; G_ = C; B_ = X; }
            else if (sourse.h < 240)
            { R_ = 0f; G_ = X; B_ = C; }
            else if (sourse.h < 300)
            { R_ = X; G_ = 0; B_ = C; }
            else if (sourse.h < 360)
            { R_ = C; G_ = 0; B_ = X; }

            byte R = (byte)(255f * (R_ + m));
            byte G = (byte)(255f * (G_ + m));
            byte B = (byte)(255f * (B_ + m));

            return Color.FromArgb(clamp(R, 0, 255), clamp(G, 0, 255), clamp(B, 0, 255));
        }
        public void ConvertRGBimagetoHSV()
        {
            int width = rgbImage.Width;
            int height = rgbImage.Height;
            hsvImage = new HSVcolor[width, height];
            for (int i = 0; i < width; ++i)
                for (int j = 0; j < height; ++j)
                    hsvImage[i, j] = RGBtoHSV(rgbImage.GetPixel(i, j));
        }
        public void ConvertHSVimagetoRGB()
        {
            int width = hsvImage.GetLength(0);
            int height = hsvImage.GetLength(1);
            rgbImage = new Bitmap(width, height);
            for (int i = 0; i < width; ++i)
                for (int j = 0; j < height; ++j)
                    rgbImage.SetPixel(i, j, HSVtoRGB(hsvImage[i, j]));
        }
    }
}
