using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Filters
{
    class BrightnessFilter : Filters // Увеличение яркости
    {
        protected override Color calculateNewPixelColor(Bitmap sourseImage, int x, int y)
        {
            int koef = 50;
            Color sourseColor = sourseImage.GetPixel(x, y);
            int R = Clamp(sourseColor.R + koef, 0, 255);
            int G = Clamp(sourseColor.G + koef, 0, 255);
            int B = Clamp(sourseColor.B + koef, 0, 255);
            Color resultColor = Color.FromArgb(R, G, B);
            return resultColor;
        }
    }
}
