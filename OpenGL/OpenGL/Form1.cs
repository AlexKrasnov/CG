using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace OpenGL
{
    public partial class Form1 : Form
    {
        GLgraphics glgraphics = new GLgraphics();

        public Form1()
        {
            InitializeComponent();
        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            glgraphics.Setup(glControl1.Width, glControl1.Height);
            glgraphics.Update();
            glControl1.SwapBuffers();
        }

        private void glControl1_Load(object sender, EventArgs e)
        {
            GLgraphics glgraphics = new GLgraphics();
        }
        
        

        private void Form1_Load(object sender, EventArgs e)
        {
            Application.Idle += Application_Idle;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            
        }

        private void glControl1_MouseMove(object sender, MouseEventArgs e)
        {
            float widthCoef = (e.X - glControl1.Width * 0.5f) / (float)glControl1.Width;
            float heightCoef = (-e.Y + glControl1.Height * 0.5f) / (float)glControl1.Height;
            glgraphics.latitude = heightCoef * 180;
            glgraphics.longitude = widthCoef * 360;
        }

        void Application_Idle(object sender, EventArgs e)
        {
            while (glControl1.IsIdle)
                glControl1.Refresh();
        }

    }
}
