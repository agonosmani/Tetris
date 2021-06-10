using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class T_block : Shape
    {
        public T_block(Point Location, Color Color) : base(Location, Color)
        {
        }

        public override void draw(Graphics g)
        {
            throw new NotImplementedException();
        }
    }
}
