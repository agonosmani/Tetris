using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class O_block : Shape
    {
        public O_block(string Orientation) : base(Color.Yellow, Orientation)
        {
        }

        public override List<Tile> draw(int cellWidth, int cellHeight)
        {
            List<Tile> points = new List<Tile>();

            points.Add(new Tile(Location.X + 1, Location.Y, cellWidth, cellHeight, Color));
            points.Add(new Tile(Location.X, Location.Y, cellWidth, cellHeight, Color));
            points.Add(new Tile(Location.X, Location.Y + 1, cellWidth, cellHeight, Color));
            points.Add(new Tile(Location.X + 1, Location.Y + 1, cellWidth, cellHeight, Color));

            return points;
        }
    }
}
