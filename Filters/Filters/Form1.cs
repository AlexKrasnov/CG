using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Filters
{
    public partial class Form1 : Form
    {
        Bitmap image, resultimage;
        Stack<Bitmap> st1;
        Stack<Bitmap> st2;
        public Form1()
        {
            InitializeComponent();
            st1 = new Stack<Bitmap>();
            st2 = new Stack<Bitmap>();
        }

        #region Open/Save File
        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Image Files | *.png; *.jpg; *.bmp; | All Files (*.*)| *.*";
            dialog.Title = "Сохранение файла";
            if (pictureBox2.Image != null)
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                    resultimage.Save(dialog.FileName);
            }
            else
            {
                MessageBox.Show("Изображение не найдено!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image Files | *.png; *.jpg; *.bmp; | All Files (*.*)| *.*";
            dialog.Title = "Открытие файла";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                image = new Bitmap(dialog.FileName);
                resultimage = image;
            }
            pictureBox1.Image = image;
            //pictureBox2.Image = image;
            pictureBox1.Refresh();
            //pictureBox2.Refresh();
        }
        #endregion

        #region ProgressIndicator
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            st1.Push(resultimage);
            Bitmap newImage = ((Filters)e.Argument).processImage(image, backgroundWorker1);
            if (backgroundWorker1.CancellationPending != true)
                resultimage = newImage;
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            pictureBox2.Image = resultimage;
            pictureBox2.Refresh();
            progressBar1.Value = 0;
        }
        #endregion

        private void инверсияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new InvertFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void размытиеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new BlurFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void фильтрГауссаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new GaussianFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void чернобелыйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new GrayScaleFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void сепияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new SepiaFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void эффектСтеклаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new GlassEffect();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void увеличениеЯркостиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new BrightnessFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void эффектСтеклаToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Filters filter = new GlassEffect();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void эффектВолныToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new WavesFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void эфектСтеклаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new GlassEffect();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void переносToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 F3 = new Form3();
            F3.ShowDialog();
            Filters filter = new Shift(F3.x0, F3.y0);
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void поворотToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 F2 = new Form2();
            F2.ShowDialog();
            Filters filter = new Rotation(F2.x0, F2.y0, F2.alfa);
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void бинарныйФильтрToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new BinaryFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void отменитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (st1.Any())
            {
                st2.Push(resultimage);
                resultimage = st1.Pop();
                pictureBox2.Image = resultimage;
                pictureBox2.Refresh();
            }
        }

        private void вернутьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (st2.Any())
            {
                st1.Push(resultimage);
                resultimage = st2.Pop();
                pictureBox2.Image = resultimage;
                pictureBox2.Refresh();
            }
        }

        private void резкостьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new HarshnessFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void операторСобеляToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new SobelFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void операторЩарраToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new ScharrFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void операторПрюиттаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new PrewitteFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void операторРобертсаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new RobertsFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void расширениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new Dilation();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void сужениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new Erosion();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void открытиеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new Opening();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void закрытиеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new Closing();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void градиентToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new Grad();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void коррекцияСОпорнымЦветомToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 F1 = new Form1();
            Color cc;
            if (F1.colorDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                cc = F1.colorDialog1.Color;
            else return;
            Filters filter = new MainColorFilter(cc);
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void идеальныйОтражательToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new IdealFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void серыйМирToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new GrayWorldFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void медианныйФильтрToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new MedianFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void эффектКарандашаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new ColorPen();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void адаптивныйToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void линейнаяКоррекцияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Filters filter = new LinearCorrection();
            //backgroundWorker1.RunWorkerAsync(filter);
        }

        private void topHatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new TopHat();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void blackHatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new BlackHat();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void цветоваяКоррекцияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new ColorCorrection();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
        }
    }
}
