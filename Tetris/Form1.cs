using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Tetris
{
    public partial class TetrisForm : Form
    {
        public Scene scene;
        public TetrisForm()
        {
            
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            scene = new Scene(GameFieldPictureBox.Width, GameFieldPictureBox.Height);
            FallTimer.Start();
            DoubleBuffered = true; 
            Invalidate(true);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
           
        }


        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            scene.drawTable(e.Graphics);
        }

        private void FallTimer_Tick(object sender, EventArgs e)
        {
            FallTimer.Interval = 450;
            scene.fall();
            Invalidate(true);
        }

        private void TetrisForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 'T')
            {
                scene.rotateShape();
            }
            else if (e.KeyValue == 'A')
            {
                scene.moveLeft();
            }
            else if (e.KeyValue == 'D')
            {
                scene.moveRight();
            }
            else if (e.KeyValue == 'S')
            {
                FallTimer.Interval = 50;
            }
            else if (e.KeyValue == 'W')
            {
                scene.bottom();
            }
            Invalidate(true);
        }

        private void pictureBox1_Paint_1(object sender, PaintEventArgs e)
        {
            scene.drawNextShape(e.Graphics, pictureBox1.Width, pictureBox1.Height);
        }

        private void save(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, scene);
            }
        }

        private void read(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                IFormatter formatter = new BinaryFormatter();
                scene = (Scene)formatter.Deserialize(fs);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FallTimer.Stop();
            SaveFileDialog sfd = new SaveFileDialog();
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                save(sfd.FileName);
            }
            FallTimer.Start();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FallTimer.Stop();
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                read(ofd.FileName);
            }
            FallTimer.Start();
        }
    }
}
