using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Filters
{
    class SepiaFilter : Filters // Сепия
    {
        protected override Color calculateNewPixelColor(Bitmap sourseImage, int x, int y)
        {
            int koef = 20;
            Color sourseColor = sourseImage.GetPixel(x, y);
            int Intensity = (int)(0.213 * sourseColor.R + 0.715 * sourseColor.G + 0.072 * sourseColor.B);
            int R = Clamp (Intensity + 2 * koef, 0, 255);
            int G = Clamp(Intensity + koef / 2, 0, 255);
            int B = Clamp(Intensity - koef, 0, 255);
            Color resultColor = Color.FromArgb(R, G, B);
            return resultColor;
        }
    }
}
