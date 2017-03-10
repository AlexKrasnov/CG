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
        public Form1()
        {
            InitializeComponent();
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
                    image.Save(dialog.FileName);
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
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                image = new Bitmap(dialog.FileName);
                resultimage = image;
            }
            pictureBox1.Image = image;
            pictureBox2.Image = image;
            pictureBox1.Refresh();
            pictureBox2.Refresh();
        }
        #endregion
    }
}
