using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel;

namespace Filters
{
    class Grad : Filters // Морфология : Grad
    {
        protected override Color calculateNewPixelColor(Bitmap SourseImage, int x, int y)
        {
            return SourseImage.GetPixel(x, y);
        }
        public override Bitmap processImage(Bitmap sourseImage, BackgroundWorker worker)
        {
            Bitmap resultImage = new Bitmap(sourseImage.Width, sourseImage.Height);
            Filters filter1 = new Dilation();
            Bitmap result1 = filter1.processImage(sourseImage, worker);
            Filters filter2 = new Erosion();
            Bitmap result2 = filter2.processImage(sourseImage, worker);
            Filters filter3 = new Erosion();
            Bitmap result = filter3.processImage(result1, worker);
            return result;
        }
    }
}
