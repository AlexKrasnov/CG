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
        protected override Color calculateNewPixelColor(Bitmap SourseImage, int x, int y)
        {
            return SourseImage.GetPixel(x, y);
        }
        public override Bitmap processImage(Bitmap sourseImage, BackgroundWorker worker)
        {
            double Rsrc = 0, Gsrc = 0, Bsrc = 0, Rdst = c.R, Gdst = c.G, Bdst = c.B;
            for (int j = 0; j < sourseImage.Height; j++)
                for (int i = 0; i < sourseImage.Height; i++)
                {
                    if (sourseImage.GetPixel(i, j).R > Rsrc) Rsrc = sourseImage.GetPixel(i, j).R;
                    if (sourseImage.GetPixel(i, j).G > Gsrc) Gsrc = sourseImage.GetPixel(i, j).G;
                    if (sourseImage.GetPixel(i, j).B > Bsrc) Bsrc = sourseImage.GetPixel(i, j).B;
                }
            Bitmap result = new Bitmap(sourseImage.Width, sourseImage.Height);
            for (int i = 0; i < sourseImage.Width; i++)
            {
                worker.ReportProgress((int)((float)i / sourseImage.Width * 100));
                if (worker.CancellationPending)
                    return null;
                for (int j = 0; j < sourseImage.Height; j++)
                {
                    int newR = Clamp((int)(calculateNewPixelColor(sourseImage, i, j).R * Rdst / Rsrc), 0, 255);
                    int newG = Clamp((int)(calculateNewPixelColor(sourseImage, i, j).G * Gdst / Gsrc), 0, 255);
                    int newB = Clamp((int)(calculateNewPixelColor(sourseImage, i, j).B * Bdst / Bsrc), 0, 255);
                    result.SetPixel(i, j, Color.FromArgb(newR, newG, newB));
                }
            }
            return result;
        }
    }
}
