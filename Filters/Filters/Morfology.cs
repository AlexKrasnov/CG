using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel;

namespace Filters
{
    class Morfology : Filters // Операции морфологии 
    {
        protected bool isDilation;
        protected bool[,] kernel = null;
        protected Morfology() { }
        public Morfology(bool[,] kernel)
        {
            this.kernel = kernel;
        }
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            int max = 0;
            int min = int.MaxValue;
            Color clr = Color.Black;
            int radiusX = kernel.GetLength(0) / 2;
            int radiusY = kernel.GetLength(1) / 2;
            for (int k = -radiusX; k <= radiusX; k++)
                for (int l = -radiusY; l <= radiusY; l++)
                {
                    int idX = Clamp(x + k, 0, sourceImage.Width - 1);
                    int idY = Clamp(y + l, 0, sourceImage.Height - 1);
                    Color sourceColor = sourceImage.GetPixel(idX, idY);
                    int intensity = (int)(0.36 * sourceColor.R + 0.53 * sourceColor.G + 0.11 * sourceColor.B);
                    if (isDilation)
                    {
                        if ((kernel[k + radiusX, l + radiusY]) && (intensity > max))
                        {
                            max = intensity;
                            clr = sourceColor;
                        }
                    }
                    else
                    {
                        if (intensity < min)
                        {
                            min = intensity;
                            clr = sourceColor;
                        }
                    }
                }

            return clr;
        }
    }

    class Dilation : Morfology // Морфологическое расширение
    {
        public Dilation()
        {
            isDilation = true;
            kernel = new bool[,] { { false, true, false }, { true, true, true }, { false, true, false } };
        }
        public Dilation(bool[,] kernel)
        {
            this.kernel = kernel;
        }
        //bool[,] mask = null;
        //public Dilation()
        //{
        //    mask = new bool[,] { { true, true, true, true, true },
        //        { true, true, true, true, true },
        //        { true, true, true, true, true },
        //        { true, true, true, true, true },
        //        { true, true, true, true, true } };
        //}
        //protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        //{
        //    return sourceImage.GetPixel(x, y);
        //}
        //public override Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
        //{
        //    int MW = 4; int MH = 4;
        //    Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);
        //    for (int i = MW / 2; i < sourceImage.Width - MW / 2; i++)
        //    {
        //        worker.ReportProgress((int)((float)i / resultImage.Width * 100));
        //        if (worker.CancellationPending)
        //            return null;
        //        for (int j = MH / 2; j < sourceImage.Height - MH / 2; j++)
        //        {
        //            int maxR = 0; int maxG = 0; int maxB = 0;
        //            for (int i1 = -MW / 2; i1 <= MW / 2; i1++)
        //                for (int j1 = -MH / 2; j1 <= MH / 2; j1++)
        //                {
        //                    if ((mask[i1 + MW / 2, j1 + MH / 2]) && (sourceImage.GetPixel(i + i1, j + j1).R > maxR))
        //                        maxR = sourceImage.GetPixel(i + i1, j + j1).R;
        //                    if ((mask[i1 + MW / 2, j1 + MH / 2]) && (sourceImage.GetPixel(i + i1, j + j1).G > maxG))
        //                        maxG = sourceImage.GetPixel(i + i1, j + j1).G;
        //                    if ((mask[i1 + MW / 2, j1 + MH / 2]) && (sourceImage.GetPixel(i + i1, j + j1).B > maxB))
        //                        maxB = sourceImage.GetPixel(i + i1, j + j1).B;
        //                }
        //            resultImage.SetPixel(i, j, Color.FromArgb(maxR, maxG, maxB));
        //        }
        //    }
        //    return resultImage;
        //}
    }

    class Erosion : Morfology // Морфологическое сужение
    {
        public Erosion()
        {
            isDilation = false;
            kernel = new bool[,] { { false, true, false }, { true, true, true }, { false, true, false } };
        }
        public Erosion(bool[,] kernel)
        {
            this.kernel = kernel;
        }
        //bool[,] mask = null;
        //public Erosion()
        //{
        //    mask = new bool[,] { { true, true, true, true, true },
        //        { true, true, true, true, true },
        //        { true, true, true, true, true },
        //        { true, true, true, true, true },
        //        { true, true, true, true, true } };
        //}
        //protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        //{
        //    return sourceImage.GetPixel(x, y);
        //}
        //public override Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
        //{
        //    int MW = 4; int MH = 4;
        //    Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);
        //    for (int i = MW / 2; i < sourceImage.Width - MW / 2; i++)
        //    {
        //        worker.ReportProgress((int)((float)i / resultImage.Width * 100));
        //        if (worker.CancellationPending)
        //            return null;
        //        for (int j = MH / 2; j < sourceImage.Height - MH / 2; j++)
        //        {
        //            int minR = 255; int minG = 255; int minB = 255;
        //            for (int i1 = -MW / 2; i1 <= MW / 2; i1++)
        //                for (int j1 = -MH / 2; j1 <= MH / 2; j1++)
        //                {
        //                    if ((mask[i1 + MW / 2, j1 + MH / 2]) && (sourceImage.GetPixel(i + i1, j + j1).R < minR))
        //                        minR = sourceImage.GetPixel(i + i1, j + j1).R;
        //                    if ((mask[i1 + MW / 2, j1 + MH / 2]) && (sourceImage.GetPixel(i + i1, j + j1).G < minG))
        //                        minG = sourceImage.GetPixel(i + i1, j + j1).G;
        //                    if ((mask[i1 + MW / 2, j1 + MH / 2]) && (sourceImage.GetPixel(i + i1, j + j1).B < minB))
        //                        minB = sourceImage.GetPixel(i + i1, j + j1).B;
        //                }
        //            resultImage.SetPixel(i, j, Color.FromArgb(minR, minG, minB));
        //        }
        //    }
        //    return resultImage;
        //}
    }

    class Opening : Filters // Морфологическое открытие
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            return sourceImage.GetPixel(x, y);
        }
        public override Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
        {
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);
            Filters filter1 = new Erosion();
            Bitmap result = filter1.processImage(sourceImage, worker);
            Filters filter2 = new Dilation();
            result = filter2.processImage(result, worker);
            return result;
        }
    }

    class Closing : Filters // Морфологическое закрытие
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            return sourceImage.GetPixel(x, y);
        }
        public override Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
        {
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);
            Filters filter1 = new Dilation();
            Bitmap result = filter1.processImage(sourceImage, worker);
            Filters filter2 = new Erosion();
            result = filter2.processImage(result, worker);
            return result;
        }
    }

    class Grad : Filters // Градиент
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            return sourceImage.GetPixel(x, y);
        }
        public override Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
        {
            Bitmap result = new Bitmap(sourceImage.Width, sourceImage.Height);
            Filters filter1 = new Dilation();
            Bitmap result1 = filter1.processImage(sourceImage, worker);
            Filters filter2 = new Erosion();
            Bitmap result2 = filter2.processImage(sourceImage, worker);
            for (int i = 0; i < sourceImage.Width; i++)
                {
                    worker.ReportProgress((int)((float)i / sourceImage.Width * 100));
                    if (worker.CancellationPending)
                        return null;
                    for (int j = 0; j < sourceImage.Height; j++)
                    {
                        int newR = Clamp(result1.GetPixel(i, j).R - result2.GetPixel(i, j).R, 0, 255);
                        int newG = Clamp(result1.GetPixel(i, j).G - result2.GetPixel(i, j).G, 0, 255);
                        int newB = Clamp(result1.GetPixel(i, j).B - result2.GetPixel(i, j).B, 0, 255);
                        result.SetPixel(i, j, Color.FromArgb(newR, newG, newB));
                    }
                }
            return result;
        }
    }
}
