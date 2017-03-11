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
    public partial class Form2 : Form
    {
        public double x0, y0, alfa;
        public Form2()
        {
            x0 = 0; y0 = 0; alfa = 0;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            x0 = Convert.ToInt32(textBox1.Text);
            y0 = Convert.ToInt32(textBox2.Text);
            alfa = Convert.ToInt32(textBox3.Text);
            Close();
        }
    }
}
