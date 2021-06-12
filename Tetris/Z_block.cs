using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    [Serializable]
    public class Z_block : Shape
    {
        public Z_block(string Orientation) : base(Color.Red, Orientation)
        {
        }

        public override List<Tile> draw(int cellWidth, int cellHeight)
        {
            List<Tile> points = new List<Tile>();

            if (Orientation == "up" || Orientation == "down")
            {
                points.Add(new Tile(Location.X, Location.Y + 1, cellWidth, cellHeight, Color));
                points.Add(new Tile(Location.X, Location.Y, cellWidth, cellHeight, Color));
                points.Add(new Tile(Location.X + 1, Location.Y, cellWidth, cellHeight, Color));
                points.Add(new Tile(Location.X + 1, Location.Y - 1, cellWidth, cellHeight, Color));
            }
            else if (Orientation == "right" || Orientation == "left")
            {
                points.Add(new Tile(Location.X - 1, Location.Y - 1, cellWidth, cellHeight, Color));
                points.Add(new Tile(Location.X, Location.Y, cellWidth, cellHeight, Color));
                points.Add(new Tile(Location.X, Location.Y - 1, cellWidth, cellHeight, Color));
                points.Add(new Tile(Location.X + 1, Location.Y, cellWidth, cellHeight, Color));

            }

            return points;
        }
    }
}
