using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMPLib;

namespace Tetris
{
    [Serializable]
    public class Scene
    {
        public Shape FallingShape { get; set; }
        public Shape NextShape { get; set; }
        public Tile [,] tileMatrix { get; set; }
        public int cellWidth { get; set; }
        public int cellHeight { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Random rand { get; set; }
        public bool Lost { get; set; }

        public int CurrentScore { get; set; }
        public bool wasTetris { get; set; }

        public WindowsMediaPlayer sfxPlayer { get; set; }

        public Scene(int width, int height)
        {
            Lost = false;
            rand = new Random();
            sfxPlayer = new WindowsMediaPlayer();
            CurrentScore = 0;
            wasTetris = false;

            generateFallingShape();
            FallingShape = new O_block("down");


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
            string[] randOrient = new string[4] { "up", "down", "left", "right" };

            int x = rand.Next(0, 7);
            string or = randOrient[rand.Next(0, 4)];

            switch (x)
            {
                case 0:
                    NextShape = new I_block(or);
                    break;
                case 1:
                    NextShape = new J_block(or);
                    break;
                case 2:
                    NextShape = new L_block(or);
                    break;
                case 3:
                    NextShape = new Z_block(or);
                    break;
                case 4:
                    NextShape = new S_block(or);
                    break;
                case 5:
                    NextShape = new T_block(or);
                    break;
                case 6:
                    NextShape = new O_block(or);
                    break;
            }
        }

        public void deleteRows()
        {
            List<int> rowInd = new List<int>();

            //Iterate through all tileMatrix and find indices of full rows. (rows that are filled with set tiles)
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

            if (rowInd.Count > 0) //If there are full rows play the clear sfx
                sfxPlayer.URL = "clear.wav";
            if (rowInd.Count == 4 && wasTetris) //If there are 4 full rows and the last deletion was also 4 rows (Tetris)
            {
                CurrentScore += 1200;
            } else if (rowInd.Count == 4)
            {
                CurrentScore += 800;
                wasTetris = true;
            } else if (rowInd.Count!=0)
            {
                CurrentScore += (rowInd.Count * 100);
                wasTetris = false;
            }

            //Displace the upper rows to where the full rows are
            foreach (int ind in rowInd)
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

        public void drawNextShape(Graphics g, int p2Width, int p2Height)
        {
            int p2CellWidth = (int)p2Width / 6;
            int p2CellHeight = (int)p2Height / 6;

            Pen gridLines = new Pen(Color.Black, 1.5F);
            Brush brush = new SolidBrush(NextShape.Color);

            foreach (Tile t in NextShape.draw(p2CellWidth, p2CellHeight))
            {
                g.FillRectangle(brush, (t.x-4) * p2CellWidth, (t.y+3) * p2CellHeight, p2CellWidth, p2CellHeight);
            }

            for (int i = p2CellHeight; i <= p2Height; i += p2CellHeight)
            {
                for (int j = p2CellWidth; j <= p2Width; j += p2CellWidth)
                {
                    g.DrawRectangle(gridLines, j - p2CellHeight, i - p2CellWidth, p2CellWidth, p2CellHeight);
                }
            }

            gridLines.Dispose();
            brush.Dispose();
        }

        public bool checkGameLost()
        {
            foreach (Tile t in FallingShape.draw(cellWidth, cellHeight))
            {
                if (t.y < 0)
                {
                    return true;
                }
            }

            return false;
        }

        public bool fall()
        {
            bool collision = false;

            List<Tile> tiles = FallingShape.draw(cellWidth, cellHeight);

            foreach (Tile t in tiles)
            {
                if ((t.y + 1) * cellHeight >= Height)//If any tile of the falling shape crosses the lower border of the game picture box
                {
                    collision = true;
                    sfxPlayer.URL = "fall.wav";
                    setTiles(tiles);//Update matrix with tiles of the falling shape
                    deleteRows();//Delete rows and update score
                    break;
                }
                else
                {
                    foreach (Tile m in tileMatrix)
                    {
                        if (m.set)//If the tile is set(there is a tile on that place)
                        {
                            if (t.y + 1 == m.y && t.x == m.x)//If any tile of the falling shape is one tile above the other set tile
                            {
                                collision = true;
                                sfxPlayer.URL = "fall.wav";//Play fall sfx
                                if (checkGameLost())
                                {
                                    sfxPlayer.URL = "gameover.wav";//Play game over sfx
                                    Lost = true;
                                    return false;
                                }
                                setTiles(tiles);
                                deleteRows();
                                break;
                            }
                        }

                    }
                }
                if (collision)
                    break;
            }
            if (collision)
            {
                FallingShape = NextShape;
                generateFallingShape();
                return false;
            }
            else
            {
                //If the falling shape doesn't collide with the lower border or other set tiles, move it down 1 tile.
                FallingShape.Location = new Point(FallingShape.Location.X, FallingShape.Location.Y + 1);
                return true;
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
        public void bottom()
        {
            while (fall());
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
            foreach (Tile t in FallingShape.draw(cellWidth, cellHeight))
            {
                bool f = false;
                foreach (Tile m in tileMatrix)
                {
                    if (m.set)
                    {
                        if (t.y == m.y && t.x == m.x)
                        { 
                            f = true;
                            rotateBack();
                            break;
                        }
                    }

                }
                if (f) break;
            }
        }

        public void rotateBack()
        {
            if (FallingShape.Orientation == "up") FallingShape.Orientation = "left";
            else if (FallingShape.Orientation == "left") FallingShape.Orientation = "down";
            else if (FallingShape.Orientation == "down") FallingShape.Orientation = "right";
            else if (FallingShape.Orientation == "right") FallingShape.Orientation = "up";
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
                foreach (Tile m in tileMatrix)
                {
                    if (m.set)
                    {
                        if (t.y == m.y && t.x-1 == m.x)
                        {
                            collision = true;
                            break;
                        }
                    }
                }
                if (collision)
                    break;
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
                foreach (Tile m in tileMatrix)
                {
                    if (m.set)
                    {
                        if (t.y == m.y && t.x + 1 == m.x)
                        {
                            collision = true;
                            break;
                        }
                    }
                }
                if (collision)
                    break;
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

            //mountain pile
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
