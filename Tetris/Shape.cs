using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public abstract class Shape
    {
        public Point Location { get; set; }
        public Color Color { get; set; }
        public Shape(Point Location, Color Color)
        {
            this.Color = Color;
            this.Location = Location;
        }

        public abstract void draw(Graphics g);
    }
}
