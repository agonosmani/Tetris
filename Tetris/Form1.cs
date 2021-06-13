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
using System.Media;
using WMPLib;

namespace Tetris
{
    public partial class TetrisForm : Form
    {
        public Scene scene;
        public WindowsMediaPlayer bgMusicPlayer { get; set; }
        public HighScore highScore { get; set; }

        public bool isPaused { get; set; }

        public TetrisForm()
        {
            bgMusicPlayer = new WindowsMediaPlayer();
            bgMusicPlayer.URL = "Tetris Theme.mp3";
            bgMusicPlayer.settings.setMode("loop", true);
            highScore = getHighScore();
            isPaused = false;
            InitializeComponent();
            DoubleBuffered = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            scene = new Scene(GameFieldPictureBox.Width, GameFieldPictureBox.Height);
            FallTimer.Start();
            //DoubleBuffered = true; 
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
            updateScoreTexts();
            if (scene.CurrentScore > highScore.highScore) 
                changeHighScore(); //Serialize new highscore
            if (scene.Lost)
            {
                bgMusicPlayer.controls.pause();
                FallTimer.Enabled = false;
                FallTimer.Stop();
                if (MessageBox.Show("Play Again?", "Game Over, You Lost.", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    NewGame();
                }
                else
                {
                    Application.Exit();
                }

            }
            Invalidate(true);
        }

        public void updateScoreTexts()
        {
            tbHighScore.Text = highScore.highScore.ToString();
            tbCurrentScore.Text = scene.CurrentScore.ToString();
        }

        public void changeHighScore()
        {
            highScore.highScore = scene.CurrentScore;
            using (FileStream fs = new FileStream("highscore", FileMode.OpenOrCreate))
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs,highScore);
            }
        }
        public HighScore getHighScore()
        {
            try
            {
                using (FileStream fs = new FileStream("highscore", FileMode.Open))
                {
                    IFormatter formatter = new BinaryFormatter();
                    return (HighScore)formatter.Deserialize(fs);
                }
            }
            catch
            {
               return new HighScore();
            }
        }
        private void TetrisForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (!isPaused) { 
                if (e.KeyValue == ' ')
                {
                    scene.rotateShape();
                }
                else if (e.KeyValue == 'A' || e.KeyValue == (char)Keys.Left)
                {
                    scene.moveLeft();
                }
                else if (e.KeyValue == 'D' || e.KeyValue == (char)Keys.Right)
                {
                    scene.moveRight();
                }
                else if (e.KeyValue == 'S' || e.KeyValue == (char)Keys.Down)
                {
                    FallTimer.Interval = 50;
                }
                else if (e.KeyValue == 'W' || e.KeyValue == (char)Keys.Up)
                {
                    scene.bottom();
                }
            }
            Invalidate(true);
        }

        private void pictureBox1_Paint_1(object sender, PaintEventArgs e)
        {
            scene.drawNextShape(e.Graphics, pictureBox1.Width, pictureBox1.Height);
        }

        public void NewGame()
        {
            FallTimer.Enabled = true;
            bgMusicPlayer.controls.play();
            scene = new Scene(GameFieldPictureBox.Width, GameFieldPictureBox.Height);
            FallTimer.Start();
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

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewGame();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void musicToolStripMenuItem_Click(object sender, EventArgs e)
        {
            musicToolStripMenuItem.Checked = !musicToolStripMenuItem.Checked;
            if (musicToolStripMenuItem.Checked)
            {
                bgMusicPlayer.settings.mute = true;
            } 
            else
            {
                bgMusicPlayer.settings.mute = false;
            }
        }

        private void soundFXToolStripMenuItem_Click(object sender, EventArgs e)
        {
            soundFXToolStripMenuItem.Checked = !soundFXToolStripMenuItem.Checked;
            if (soundFXToolStripMenuItem.Checked)
            {
                scene.sfxPlayer.settings.mute = true;
            }
            else
            {
                scene.sfxPlayer.settings.mute = false;
            }
        }

        private void pauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pauseToolStripMenuItem.Text == "Pause")
            {
                FallTimer.Stop();
                isPaused = true;
                pauseToolStripMenuItem.Text = "Resume";
            }
            else
            {
                FallTimer.Start();
                isPaused = false;
                pauseToolStripMenuItem.Text = "Pause";
            }
        }
    }
}
