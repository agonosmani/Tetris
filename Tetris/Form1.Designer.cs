
namespace Tetris
{
    partial class TetrisForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.GameFieldPictureBox = new System.Windows.Forms.PictureBox();
            this.FallTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.GameFieldPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // GameFieldPictureBox
            // 
            this.GameFieldPictureBox.Location = new System.Drawing.Point(133, 30);
            this.GameFieldPictureBox.Name = "GameFieldPictureBox";
            this.GameFieldPictureBox.Size = new System.Drawing.Size(278, 410);
            this.GameFieldPictureBox.TabIndex = 0;
            this.GameFieldPictureBox.TabStop = false;
            this.GameFieldPictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            // 
            // FallTimer
            // 
            this.FallTimer.Enabled = true;
            this.FallTimer.Interval = 300;
            this.FallTimer.Tick += new System.EventHandler(this.FallTimer_Tick);
            // 
            // TetrisForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(638, 500);
            this.Controls.Add(this.GameFieldPictureBox);
            this.Name = "TetrisForm";
            this.Text = "Tetris";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TetrisForm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.GameFieldPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox GameFieldPictureBox;
        private System.Windows.Forms.Timer FallTimer;
    }
}

