using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Filters
{
    class PointFilters : Filters // Точечные фильтры
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            return sourceImage.GetPixel(x, y);
        }
    }

    class InvertFilter : Filters // Инверсия (негатив)
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color sourseColor = sourceImage.GetPixel(x, y);
            Color resultColor = Color.FromArgb(255 - sourseColor.R, 255 - sourseColor.G, 255 - sourseColor.B);
            return resultColor;
        }
    }

    class GrayScaleFilter : Filters // Черно-белое
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color sourceColor = sourceImage.GetPixel(x, y);
            int Intensity = (int)(0.213 * sourceColor.R + 0.715 * sourceColor.G + 0.072 * sourceColor.B);
            Color resultColor = Color.FromArgb(Intensity, Intensity, Intensity);
            return resultColor;
        }
    }

    class SepiaFilter : Filters // Сепия
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            int koef = 20;
            Color sourceColor = sourceImage.GetPixel(x, y);
            int Intensity = (int)(0.213 * sourceColor.R + 0.715 * sourceColor.G + 0.072 * sourceColor.B);
            int R = Clamp(Intensity + 2 * koef, 0, 255);
            int G = Clamp(Intensity + koef / 2, 0, 255);
            int B = Clamp(Intensity - koef, 0, 255);
            Color resultColor = Color.FromArgb(R, G, B);
            return resultColor;
        }
    }

    class BrightnessFilter : Filters // Увеличение яркости
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            int koef = 50;
            Color sourceColor = sourceImage.GetPixel(x, y);
            int R = Clamp(sourceColor.R + koef, 0, 255);
            int G = Clamp(sourceColor.G + koef, 0, 255);
            int B = Clamp(sourceColor.B + koef, 0, 255);
            Color resultColor = Color.FromArgb(R, G, B);
            return resultColor;
        }
    }

    class WavesFilter : Filters // Эффект "волны"
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            int newX = Clamp((int)(x + 20 * Math.Sin(2 * Math.PI * x / 60)), 0, sourceImage.Width - 1);
            int newY = y;
            return sourceImage.GetPixel(newX, newY); ;
        }
    }

    class GlassEffect : Filters // Эффект "стекла"
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Random Rand = new Random();
            int newX = Clamp((int)(x + (Rand.NextDouble() - 0.5) * 10.0), 0, sourceImage.Width - 1);
            int newY = Clamp((int)(y + (Rand.NextDouble() - 0.5) * 10.0), 0, sourceImage.Height - 1);
            return sourceImage.GetPixel(newX, newY); ;
        }
    }

    class Shift : Filters // Перенос (сдвиг)
    {
        double x0, y0; // координаты сдвига
        public Shift(double _x0, double _y0)
        {
            x0 = _x0; y0 = _y0;
        }
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color sourseColor = sourceImage.GetPixel(x, y);
            int newX = Clamp((int)(x + x0), 0, sourceImage.Width - 1);
            int newY = Clamp((int)(y + y0), 0, sourceImage.Height - 1); ;
            return sourceImage.GetPixel(newX, newY); ;
        }
    }

    class Rotation : Filters // Поворот
    {
        double x0, y0; // координаты центра поворота
        double alfa; // угол поворота
        public Rotation(double _x0, double _y0, double _alfa)
        {
            x0 = _x0; y0 = _y0; alfa = _alfa;
        }
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            double p1 = Math.Cos(alfa); double p2 = Math.Sin(alfa);
            int newX = Clamp((int)((x - x0) * p1 - (y - y0) * p2 + x0), 0, sourceImage.Width - 1);
            int newY = Clamp((int)((x - x0) * p2 + (y - y0) * p1 + y0), 0, sourceImage.Height - 1);
            return sourceImage.GetPixel(newX, newY);
        }
    }

        class BinaryFilter : Filters // В бинарное изображение
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color s = sourceImage.GetPixel(x, y);
            Color resultColor;
            if (s.R < 127 && s.G < 127 && s.B < 127)
                resultColor = Color.FromArgb(0, 0, 0);
            else
                resultColor = Color.FromArgb(255, 255, 255);
            return resultColor;
        }
    }
}
