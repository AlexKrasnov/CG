using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Filters
{
    class LinearCorrection : Filters
    {
        private int F(int y, int yMax, int yMin)
        {
            return Clamp(((y - yMin) * (255 / (yMax - yMin))), 0, 255);
        }
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            return sourceImage.GetPixel(x, y);
        }
        public override Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
        {
            Bitmap result = new Bitmap(sourceImage);
            // беру значения цвета верхнего правого и левого нижнего угла с отступом
            int XminR = sourceImage.GetPixel(1, 1).R;
            int XminG = sourceImage.GetPixel(1, 1).G;
            int XminB = sourceImage.GetPixel(1, 1).B;
            int XmaxR = sourceImage.GetPixel(sourceImage.Width - 1, sourceImage.Height - 1).R;
            int XmaxG = sourceImage.GetPixel(sourceImage.Width - 1, sourceImage.Height - 1).G;
            int XmaxB = sourceImage.GetPixel(sourceImage.Width - 1, sourceImage.Height - 1).B;
            for (int i = 0; i < sourceImage.Width; i++)
            {
                worker.ReportProgress((int)((float)i / sourceImage.Width * 100));
                if (worker.CancellationPending)
                    return null;
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    int R = sourceImage.GetPixel(i, j).R;
                    int G = sourceImage.GetPixel(i, j).G;
                    int B = sourceImage.GetPixel(i, j).B;
                    result.SetPixel(i, j, Color.FromArgb(F(R, XmaxR, XminR), F(G, XmaxG, XminG), F(B, XmaxB, XminB)));
                }
            }
            return result;
        }
    }
}
