using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class Scene
    {
        public Shape FallingShape { get; set; }
        public Shape NextShape { get; set; }
        public Tile [,] tileMatrix { get; set; }
        public int cellWidth { get; set; }
        public int cellHeight { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Scene(int width, int height)
        {
            //FallingShape = fallingShape;
            //NextShape = nextShape;

            Width = width - 5;
            Height = height - 5;
            this.cellWidth = Width / 10;
            this.cellHeight = Height / 15;

            this.tileMatrix = new Tile[(int)Height / cellHeight,(int)Width / cellWidth];
            for(int i=0; i < (int)Height / cellHeight; i++)
            {
                for(int j=0; j < (int)Width / cellWidth; j++)
                {
                    tileMatrix[i, j] = new Tile(j, i, cellWidth, cellHeight, Color.White);
                }
            }
        }

        public void generateFallingShape()
        {
            FallingShape = new T_block(new Point(3, 0), Color.Red, "down");
        }

        public void deleteRows()
        {
            List<int> rowInd = new List<int>();

            for (int i = 1; i < ((int)Height / cellHeight); i++)
            {
                bool fullRow = true;
                for (int j = 0; j < (int)Width / cellWidth; j++)
                {
                    if (tileMatrix[i, j].set == false)
                    {
                        fullRow = false;
                        break;
                    }
                }

                if (fullRow)
                {
                    rowInd.Add(i);
                }
            }


            foreach(int ind in rowInd)
            {
                for(int i=ind; i>0; i--)
                {
                    for(int j=0; j< (int)Width / cellWidth; j++)
                    {
                        tileMatrix[i, j].color = tileMatrix[i - 1, j].color;
                        tileMatrix[i, j].set = tileMatrix[i - 1, j].set;
                    }
                }
            }

        }
        public void fall()
        {
            bool collision = false;

            List<Tile> tiles = FallingShape.draw(cellWidth, cellHeight);

            foreach(Tile t in tiles)
            {
                if((t.y + 1) * cellHeight >= Height){
                    collision = true;
                    setTiles(tiles);
                    deleteRows();
                    break;
                }

                foreach(Tile m in tileMatrix)
                {
                    if (m.set)
                    {
                        if (t.y + 1 == m.y && t.x == m.x)
                        {
                            collision = true;
                            setTiles(tiles);
                            deleteRows();
                            break;
                        }
                    }
                    
                }

                if (collision)
                    break;
            }

            if(collision)
            {
                generateFallingShape();
                //setTiles(tiles);
            }
            else
            { 
                FallingShape.Location = new Point(FallingShape.Location.X, FallingShape.Location.Y + 1);
            }
        }

        private void setTiles(List<Tile> tiles)
        {
            foreach (Tile t in tiles)
            {
                tileMatrix[t.y, t.x] = t;
                tileMatrix[t.y, t.x].set = true;
            }
        }

        public void rotateShape()
        {
            bool canRotate = true;

            foreach (Tile t in FallingShape.draw(cellWidth, cellHeight))
            {
                if (t.y == (int)Height / cellHeight - 1)
                {
                    canRotate = false;
                    break;
                }

                bool f = false;
                foreach(Tile m in tileMatrix)
                {
                    if (m.set)
                    {
                        if (t.y + 1 == m.y && t.x == m.x)
                        {
                            canRotate = false;
                            f = true;
                            break;
                        }
                    }
                    
                }
                if (f) break;
            }

            if (canRotate)
            {
                if (FallingShape.Orientation == "up") FallingShape.Orientation = "right";
                else if (FallingShape.Orientation == "right") FallingShape.Orientation = "down";
                else if (FallingShape.Orientation == "down") FallingShape.Orientation = "left";
                else if (FallingShape.Orientation == "left") FallingShape.Orientation = "up";


                foreach (Tile t in FallingShape.draw(cellWidth, cellHeight))
                {
                    if (t.x < 0)
                    {
                        moveRight();
                    }
                    else if (t.x >= (int)Width / cellWidth)
                    {
                        moveLeft();
                    }
                }
            }

        }
        public void moveLeft()
        {
            bool collision = false;
            foreach(Tile t in FallingShape.draw(cellWidth, cellHeight))
            {
                if(t.x - 1 < 0)
                {
                    collision = true;
                    break;
                }
            }

            if (!collision)
                FallingShape.Location = new Point(FallingShape.Location.X-1, FallingShape.Location.Y);
        }
        public void moveRight()
        {
            bool collision = false;
            foreach (Tile t in FallingShape.draw(cellWidth, cellHeight))
            {
                if ((t.x + 2) * cellWidth > Width)
                {
                    collision = true;
                    break;
                }
            }

            if (!collision)
                FallingShape.Location = new Point(FallingShape.Location.X + 1, FallingShape.Location.Y);
        }
        public void drawTable(Graphics g)
        {
            Pen gridLines = new Pen(Color.Black, 1.5F); 
            
            //falling shape
            Brush brush = new SolidBrush(FallingShape.Color);
            foreach (Tile t in FallingShape.draw(cellWidth, cellHeight))
            {
                t.draw(g);
            }

            foreach (Tile t in tileMatrix)
            {
                if (t.set)
                {
                    Brush b = new SolidBrush(t.color);
                    t.draw(g);
                    b.Dispose();
                }
            }


            //grid lines
            for (int i = cellHeight; i <= Height; i += cellHeight)
            {
                for (int j = cellWidth; j <= Width; j += cellWidth)
                {
                    g.DrawRectangle(gridLines, j - cellHeight, i - cellWidth, cellWidth, cellHeight);
                }
            }

            
            gridLines.Dispose();
            brush.Dispose();
        }
    }
}
