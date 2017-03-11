using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Filters
{
    class WavesFilter : Filters // Эффект "волны"
    {
        protected override Color calculateNewPixelColor(Bitmap sourseImage, int x, int y)
        {
            Color sourseColor = sourseImage.GetPixel(x, y);
            int newX = Clamp((int)(x+20*Math.Sin(2*Math.PI*x/60)), 0, sourseImage.Width - 1);
            int newY = y;
            return sourseImage.GetPixel(newX, newY); ;
        }
    }
}
