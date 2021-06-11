using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class Tile
    {
        public bool set { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int cellWidth { get; set; }
        public int cellHeight { get; set; }
        public Color color { get; set; }

        public Tile(int x, int y, int cellWidth, int cellHeight, Color color)
        {
            set = false; 
            this.x = x;
            this.y = y;
            this.cellWidth = cellWidth;
            this.cellHeight = cellHeight;
            this.color = color; 
        }

        public void draw(Graphics g)
        {
            Brush brush = new SolidBrush(color);
            g.FillRectangle(brush, x*cellWidth, y*cellHeight, cellWidth, cellHeight);
            brush.Dispose();
        }
    }
}
