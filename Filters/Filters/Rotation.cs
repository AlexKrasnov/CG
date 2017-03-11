using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Filters
{
    class Rotation : Filters // Поворот
    {
        double x0, y0; // координаты центра поворота
        double alfa; // угол поворота
        public Rotation(double _x0, double _y0, double _alfa)
        {
            x0 = _x0; y0 = _y0; alfa = _alfa;
        }
        protected override Color calculateNewPixelColor(Bitmap sourseImage, int x, int y)
        {
            Color sourseColor = sourseImage.GetPixel(x, y);
            double p1 = Math.Cos(alfa); double p2 = Math.Sin(alfa);
            int newX = Clamp((int)((x - x0) * p1 - (y - y0) * p2 + x0), 0, sourseImage.Width - 1);
            int newY = Clamp((int)((x - x0) * p2 + (y - y0) * p1 + y0), 0, sourseImage.Height - 1);
            return sourseImage.GetPixel(newX, newY);
        }
    }
}
