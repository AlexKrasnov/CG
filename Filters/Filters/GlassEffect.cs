using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel;

namespace Filters
{
    class GlassEffect : Filters // Эффект "стекла"
    {
        protected override Color calculateNewPixelColor(Bitmap sourseImage, int x, int y)
        {
            Color sourseColor = sourseImage.GetPixel(x, y);
            Random Rand = new Random();
            int newX = Clamp((int)(x + (Rand.NextDouble() - 0.5) * 10.0), 0, sourseImage.Width - 1);
            int newY = Clamp((int)(y + (Rand.NextDouble() - 0.5) * 10.0), 0, sourseImage.Height - 1);
            return sourseImage.GetPixel(newX, newY); ;
        }
    }
}
