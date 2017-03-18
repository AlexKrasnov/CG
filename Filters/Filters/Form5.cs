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
    public partial class Form5 : Form
    {
        public int radius, threshold;
        public Form5()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if ((textBox1.Text == "1" || textBox2.Text == ""))
                MessageBox.Show("Введите значения Radius и Threshold", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                radius = Convert.ToInt32(textBox1.Text);
                threshold = Convert.ToInt32(textBox2.Text);
                Close();
            }
        }
    }
}
