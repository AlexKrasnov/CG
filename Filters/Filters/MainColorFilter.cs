using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel;

namespace Filters
{
    class MainColorFilter : Filters // Коррекция с опорным цветом
    {
        Color c;
        public MainColorFilter(Color _c)
        {
            c = _c;
        }
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            return sourceImage.GetPixel(x, y);
        }
        public override Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
        {
            double Rsrc = 0, Gsrc = 0, Bsrc = 0, Rdst = c.R, Gdst = c.G, Bdst = c.B;
            for (int i = 0; i < sourceImage.Width; i++)
            {
                worker.ReportProgress((int)((float)i / sourceImage.Width * 100));
                if (worker.CancellationPending)
                    return null;
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    if (sourceImage.GetPixel(i, j).R > Rsrc) Rsrc = sourceImage.GetPixel(i, j).R;
                    if (sourceImage.GetPixel(i, j).G > Gsrc) Gsrc = sourceImage.GetPixel(i, j).G;
                    if (sourceImage.GetPixel(i, j).B > Bsrc) Bsrc = sourceImage.GetPixel(i, j).B;
                }
            }
            Bitmap result = new Bitmap(sourceImage.Width, sourceImage.Height);
            for (int i = 0; i < sourceImage.Width; i++)
            {
                worker.ReportProgress((int)((float)i / sourceImage.Width * 100));
                if (worker.CancellationPending)
                    return null;
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    int newR = Clamp((int)(calculateNewPixelColor(sourceImage, i, j).R * Rdst / Rsrc), 0, 255);
                    int newG = Clamp((int)(calculateNewPixelColor(sourceImage, i, j).G * Gdst / Gsrc), 0, 255);
                    int newB = Clamp((int)(calculateNewPixelColor(sourceImage, i, j).B * Bdst / Bsrc), 0, 255);
                    result.SetPixel(i, j, Color.FromArgb(newR, newG, newB));
                }
            }
            return result;
        }
    }
}
