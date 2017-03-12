using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel;

namespace Filters
{
    class IdealFilter : Filters // "Идеальный отражатель"
    {
        protected override Color calculateNewPixelColor(Bitmap SourseImage, int x, int y)
        {
            return SourseImage.GetPixel(x, y);
        }
        public override Bitmap processImage(Bitmap sourseImage, BackgroundWorker worker)
        {
            double Rmax = 0, Gmax = 0, Bmax = 0;
            for (int i = 0; i < sourseImage.Width; i++)
            {
                worker.ReportProgress((int)((float)i / sourseImage.Width * 100));
                if (worker.CancellationPending)
                    return null;
                for (int j = 0; j < sourseImage.Height; j++)
                {
                    if (sourseImage.GetPixel(i, j).R > Rmax) Rmax = sourseImage.GetPixel(i, j).R;
                    if (sourseImage.GetPixel(i, j).G > Gmax) Gmax = sourseImage.GetPixel(i, j).G;
                    if (sourseImage.GetPixel(i, j).B > Bmax) Bmax = sourseImage.GetPixel(i, j).B;
                }
            }
            Bitmap result = new Bitmap(sourseImage.Width, sourseImage.Height);
            for (int i = 0; i < sourseImage.Width; i++)
            {
                worker.ReportProgress((int)((float)i / sourseImage.Width * 100));
                if (worker.CancellationPending)
                    return null;
                for (int j = 0; j < sourseImage.Height; j++)
                {
                    int newR = Clamp((int)(calculateNewPixelColor(sourseImage, i, j).R * 255 / Rmax), 0, 255);
                    int newG = Clamp((int)(calculateNewPixelColor(sourseImage, i, j).G * 255 / Gmax), 0, 255);
                    int newB = Clamp((int)(calculateNewPixelColor(sourseImage, i, j).B * 255 / Bmax), 0, 255);
                    result.SetPixel(i, j, Color.FromArgb(newR, newG, newB));
                }
            }
            return result;
        }
    }
}
