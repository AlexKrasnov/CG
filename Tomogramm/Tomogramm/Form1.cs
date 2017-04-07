using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tomogramm
{
    public partial class Form1 : Form
    {
        bool IsTexture = false;
        Bin binfile = new Bin();
        View view = new View();
        bool loaded = false;
        bool needReload = false;
        int currentLayer = 0;
        int currentMax = 1;
        int currentMin = 0;
        int FrameCount;
        DateTime NextFPSUpdate = DateTime.Now.AddSeconds(1);

        public Form1()
        {
            InitializeComponent();
        }

        private void открытьФайлToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Binary Files | *.bin; | All Files (*.*)| *.*";
            dialog.Title = "Открытие файла";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string str = dialog.FileName;
                binfile.ReadBIN(str);
                MessageBox.Show("Бинарник обработан!", "Ура!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                view.SetupView(glControl1.Width, glControl1.Height);
                loaded = true;
                glControl1.Invalidate();
                view.generateTextureImage(currentLayer, currentMax, currentMin);
                view.Load2DTexture();
            }
        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            if (loaded)
            {
                if (IsTexture)
                {
                    if (needReload)
                    {
                        view.generateTextureImage(currentLayer, currentMax, currentMin);
                        view.Load2DTexture();
                        needReload = false;
                    }
                    view.DrawTexture();
                    glControl1.SwapBuffers();
                }
                else
                {
                    view.DrawQuads(currentLayer, currentMax, currentMin);
                    glControl1.SwapBuffers();
                }
            }
        }

        private void glControl1_Resize(object sender, EventArgs e)
        {
            if (loaded)
                view.SetupView(glControl1.Width, glControl1.Height);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            currentLayer = trackBar1.Value;
            needReload = true;
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            currentMin = trackBar2.Value;
            if (IsTexture)
            {
                view.generateTextureImage(currentLayer, currentMax, currentMin);
                view.Load2DTexture();
            }
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            currentMax = trackBar3.Value;
            if (IsTexture)
            {
                view.generateTextureImage(currentLayer, currentMax, currentMin);
                view.Load2DTexture();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Application.Idle += Application_Idle;
        }

        private void Application_Idle(Object sender, EventArgs e)
        {
            while (glControl1.IsIdle)
            {
                displayFPS();
                glControl1.Invalidate();
            }
        }

        void displayFPS()
        {
            if (DateTime.Now >= NextFPSUpdate)
            {
                Text = String.Format("CT Visualizer (fps = {0})", FrameCount);
                NextFPSUpdate = DateTime.Now.AddSeconds(1);
                FrameCount = 0;
            }
            FrameCount++;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            IsTexture = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            IsTexture = true;
        }
    }
}
