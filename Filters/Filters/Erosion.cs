using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel;

namespace Filters
{
    class Erosion : Filters // Морфологическое сужение
    {
        bool[,] mask = null;
        public Erosion()
        {
            mask = new bool[,] { { true, true, true, true, true },
                { true, true, true, true, true },
                { true, true, true, true, true },
                { true, true, true, true, true },
                { true, true, true, true, true } };
        }
        protected override Color calculateNewPixelColor(Bitmap SourseImage, int x, int y)
        {
            return SourseImage.GetPixel(x, y);
        }
        public override Bitmap processImage(Bitmap sourseImage, BackgroundWorker worker)
        {
            int MW = 4; int MH = 4;
            Bitmap resultImage = new Bitmap(sourseImage.Width, sourseImage.Height);
            for (int i = MW / 2; i < sourseImage.Width - MW / 2; i++)
            {
                worker.ReportProgress((int)((float)i / resultImage.Width * 100));
                if (worker.CancellationPending)
                    return null;
                for (int j = MH / 2; j < sourseImage.Height - MH / 2; j++)
                {
                    int minR = 255; int minG = 255; int minB = 255;
                    for (int i1 = -MW / 2; i1 <= MW / 2; i1++)
                        for (int j1 = -MH / 2; j1 <= MH / 2; j1++)
                        {
                            if ((mask[i1 + MW / 2, j1 + MH / 2]) && (sourseImage.GetPixel(i + i1, j + j1).R < minR))
                                minR = sourseImage.GetPixel(i + i1, j + j1).R;
                            if ((mask[i1 + MW / 2, j1 + MH / 2]) && (sourseImage.GetPixel(i + i1, j + j1).G < minG))
                                minG = sourseImage.GetPixel(i + i1, j + j1).G;
                            if ((mask[i1 + MW / 2, j1 + MH / 2]) && (sourseImage.GetPixel(i + i1, j + j1).B < minB))
                                minB = sourseImage.GetPixel(i + i1, j + j1).B;
                        }
                    resultImage.SetPixel(i, j, Color.FromArgb(minR, minG, minB));
                }
            }
            return resultImage;
        }
    }
}
