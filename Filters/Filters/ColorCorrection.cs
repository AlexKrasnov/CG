﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel;

namespace Filters
{
    class ColorCorrection : Filters // Цветовая коррекция "autolevels"
    {
        private int F(int y, int yMax, int yMin)
        {
            return Clamp(((255 * (y - yMin)) / (yMax - yMin)), 0, 255);
        }
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            return sourceImage.GetPixel(x, y);
        }
        public override Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
        {
            Bitmap result = new Bitmap(sourceImage.Width, sourceImage.Height);
            int XminR = 0, XmaxR = 0, XmaxG = 0, XminG = 0, XmaxB = 0, XminB = 0;
            for (int i = 0; i < sourceImage.Width; i++)
            {
                worker.ReportProgress((int)((float)i / sourceImage.Width * 100));
                if (worker.CancellationPending)
                    return null;
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    Color pixColor = sourceImage.GetPixel(i, j);
                    if (XminR > pixColor.R) XminR = pixColor.R;
                    if (XmaxR < pixColor.R) XmaxR = pixColor.R;
                    if (XminG > pixColor.G) XminG = pixColor.G;
                    if (XmaxG < pixColor.G) XmaxG = pixColor.G;
                    if (XminB > pixColor.B) XminB = pixColor.B;
                    if (XmaxB < pixColor.B) XmaxB = pixColor.B;
                }
            }
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
