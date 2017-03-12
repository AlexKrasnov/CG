using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel;

namespace Filters
{
    class Opening : Filters // Морфологическое открытие
    {
        protected override Color calculateNewPixelColor(Bitmap SourseImage, int x, int y)
        {
            return SourseImage.GetPixel(x, y);
        }
        public override Bitmap processImage(Bitmap sourseImage, BackgroundWorker worker)
        {
            Bitmap resultImage = new Bitmap(sourseImage.Width, sourseImage.Height);
            Filters filter1 = new Erosion();
            Bitmap result = filter1.processImage(sourseImage, worker);
            Filters filter2 = new Dilation();
            result = filter2.processImage(result, worker);
            return result;
        }
    }
}
