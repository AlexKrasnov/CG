using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Filters
{
    class BinaryFilter : Filters // В бинарное изображение
    {
        protected override Color calculateNewPixelColor(Bitmap sourseImage, int x, int y)
        {
            Color sourseColor = sourseImage.GetPixel(x, y);
            Color resultColor;
            if (sourseColor.R < 127 && sourseColor.G < 127 && sourseColor.B < 127)
                resultColor = Color.FromArgb(0, 0, 0);
            else
                resultColor = Color.FromArgb(255, 255, 255);
            return resultColor;
        }
    }
}
