using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    public partial class forma : Form
    {
        public forma()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //Brush b = new SolidBrush(Color.Black);

            Pen p = new Pen(Color.Black);
            e.Graphics.DrawRectangle(p, 40, 20, 400, 400);
            p.Dispose();

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Pen border = new Pen(Color.Black, 2F);
            SolidBrush fone = new SolidBrush(Color.Blue);

            e.Graphics.DrawRectangle(border, 4, 4, 100 * 10 + 2, 100 * 10 + 2);
            e.Graphics.FillRectangle(fone, 5, 5, 100 * 10, 100 * 10);
        }
    }
}
