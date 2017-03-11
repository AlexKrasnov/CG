using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Filters
{
    class Shift : Filters // Перенос (сдвиг)
    {
        double x0, y0; // координаты сдвига
        public Shift(double _x0, double _y0)
        {
            x0 = _x0; y0 = _y0;
        }
        protected override Color calculateNewPixelColor(Bitmap sourseImage, int x, int y)
        {
            Color sourseColor = sourseImage.GetPixel(x, y);
            int newX = Clamp((int)(x + x0), 0, sourseImage.Width - 1);
            int newY = Clamp((int)(y + y0), 0, sourseImage.Height - 1); ;
            return sourseImage.GetPixel(newX, newY); ;
        }
    }
}
