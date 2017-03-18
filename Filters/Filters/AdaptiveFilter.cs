using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel;

namespace Filters
{
    class AdaptiveFilter : Filters // Адаптивный фильтр
    {
        int radius, threshold;
        public AdaptiveFilter(int _radius, int _threshold)
        {
            radius = _radius;
            threshold = _threshold;
        }
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            return sourceImage.GetPixel(x, y);
        }
        public override Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
        {
            Bitmap result = new Bitmap(sourceImage.Width, sourceImage.Height);
            int tot_red, tot_green, tot_blue, count_red, count_green, count_blue;
            double R, G, B;
            for (int i = radius; i < sourceImage.Width - radius; i++)
            {
                worker.ReportProgress((int)((float)i / sourceImage.Width * 100));
                if (worker.CancellationPending)
                    return null;
                for (int j = radius; j < sourceImage.Height - radius; j++)
                {
                    Color c = sourceImage.GetPixel(i, j);
                    tot_red = tot_green = tot_blue = 0;
                    count_red = count_green = count_blue = 0;
                    for (int l = i - radius; l <= i + radius; l++)
                        for (int k = j - radius; k <= j + radius; k++)
                        {
                            Color tmp = sourceImage.GetPixel(l, k);
                            if (Math.Abs(tmp.R - c.R) < threshold)
                            {
                                tot_red += tmp.R;
                                count_red++;
                            }
                            if (Math.Abs(tmp.G - c.G) < threshold)
                            {
                                tot_green += tmp.G;
                                count_green++;
                            }
                            if (Math.Abs(tmp.B - c.B) < threshold)
                            {
                                tot_blue += tmp.B;
                                count_blue++;
                            }
                        }
                    R = tot_red / count_red;
                    G = tot_green / count_green;
                    B = tot_blue / count_blue;
                    result.SetPixel(i, j, Color.FromArgb((int)R, (int)G, (int)B));
                }
            }
            return result;
        }
    }
}
