using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class I_block : Shape
    {
        public I_block(Point Location, Color Color) : base(Location, Color)
        {
        }

        public override void draw(Graphics g)
        {
            throw new NotImplementedException();
        }
    }
}
