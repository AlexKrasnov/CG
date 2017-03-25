using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenGL
{
    public partial class Form1 : Form
    {
        Bin binfile = new Bin();
        View view = new View();
        bool loaded = new bool();
        int currentLayer;
        int FrameCount;
        DateTime NextFPSUpdate = DateTime.Now.AddSeconds(1);
        public Form1()
        {
            InitializeComponent();
        }
        
        private void simpleOpenGlControl1_Paint(object sender, PaintEventArgs e)
        {
            //if (loaded)
            //{
            //    view.DrawQuads(currentLayer);
            //    simpleOpenGlControl1.SwapBuffers();
            //}
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            currentLayer = trackBar1.Value;
            view.DrawQuads(currentLayer);
            simpleOpenGlControl1.SwapBuffers();
        }

        private void открытьФайлToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Binary Files | *.bin; | All Files (*.*)| *.*";
            dialog.Title = "Открытие файла";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                currentLayer = 0;
                string str = dialog.FileName;
                binfile.ReadBIN(str);
                MessageBox.Show("Бинарник обработан!", "Ура!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                view.SetupView(simpleOpenGlControl1.Width, simpleOpenGlControl1.Height);
                loaded = true;
                simpleOpenGlControl1.Invalidate();
                view.DrawQuads(currentLayer);
                simpleOpenGlControl1.SwapBuffers();
                //simpleOpenGlControl1.Refresh();
            }
        }

        private void Application_Idle(Object sender, EventArgs e)
        {
            while (simpleOpenGlControl1.IsIdle)
            {
                displayFPS();
                simpleOpenGlControl1.Invalidate();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Application.Idle += Application_Idle;
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

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (loaded)
            {
                view.SetupView(simpleOpenGlControl1.Width, simpleOpenGlControl1.Height);
                simpleOpenGlControl1.Invalidate();
                view.DrawQuads(currentLayer);
                simpleOpenGlControl1.SwapBuffers();
            }
        }
    }
}
