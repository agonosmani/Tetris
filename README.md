# Tetris
Windows Forms проектна задача изработена од: Бехар Кадри (191046), Агон Османи (191025) и Агрон Бајрами (191042).

# Опис на апликацијата
Изработената апликација е класичната игра Tetris. Апликацијата овозможува играње на играта Tetris, со цел да обезбедиме најдобро корисничко искуство имплементиравме позадинска музика, звучни ефекти и различни "quality of life features" (Pause/Resume, Speed Up, Instant Drop). Oвозможува зачување на играта со цел да може да се продолжи подоцна и зачува Highscore на ниво на компјутер.

Играта е направена со минималистичен дизајн со цел да биде поупотреблива од страна на играчите.

![image](https://user-images.githubusercontent.com/60615745/121821280-59493f80-cc98-11eb-955d-52eecc4c344d.png)


# Упатство за употреба
1. При стартување на апликацијата веднаш почнува и играта.

![image](https://user-images.githubusercontent.com/60615745/121821310-885fb100-cc98-11eb-8c7d-547f8eacbd1a.png)

2. Апликацијата има само еден прозорец (слика 1/2) каде има можност да започнеме нова игра (File New), да продолжиме веќе постоечка игра (File Load) да зачуваме игра (File Save) да се прави Pause/Resume на играта и Mute на Музиката/Ефектите. Исто така во овој прозорец се игра играта каде лево (слика 1,2) се прикажуваат тековниот score, глобалниот highscore, фигурата што паѓа следно и упатството а десно (слика 1,2) се наоѓа главниот PictureBox каде се рендерира играта.
3. Правилата на играта се едноставни: Играта завршува кога фигурата што паѓа направи колизија со горната граница и ако тековниот score е поголем од highscore, highscore се ажурира соодветно.

# Опис на структури за податоци

Главните податоци и функции за играта се чуваат во класа public ```class Scene``` која содржи при сешто друго две објекти од ```class Shape``` (FallingShape и NextShape) и матрица од објекти од ```class Tile```.

```csharp
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
    }
```

```class Shape``` е апстрактна класа од која произлегуваат соодветните класи за секоја фигура.

```csharp
    [Serializable]
    public abstract class Shape
    {
        public Point Location { get; set; }
        public Tile[] tiles { get; set; }
        public Color Color { get; set; }
        public string Orientation { get; set; }
     }
```

Класите што произлегуваат од класата Shape (```class I_block```, ```class J_block``` ...) не чуваат други податоци само го имплементират аптстрактниот метод ```void draw```.

Секоја фигура се состои од листа со објекти на ```class Tile```

```csharp
    [Serializable]
    public class Tile
    {
        public bool set { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int cellWidth { get; set; }
        public int cellHeight { get; set; }
        public Color color { get; set; }
     }
 ```

За да се зачува глобален highscore употребуваме ```class HighScore```.

```csharp
    [Serializable]
    public class HighScore
    {
        public int highScore { get; set; }
    }
```

Сите класи имаат анотација [Serializable] за да може да се зачува играта/highscore.

НАПОМЕНА! Прикажаниот код го прикажува само податочниот аспект на класите а не и методите што тие ги содржат.

# Опис на најважните функции за апликацијата

Ќе се разгледат три најважните методи/функции за апликацијата.

Прво е tick евентот на FallTimer

```csharp
private void FallTimer_Tick(object sender, EventArgs e)
        {
            FallTimer.Interval = 450;
            scene.fall();
            updateScoreTexts();
            if (scene.CurrentScore > highScore.highScore)
                changeHighScore();//Serialize new highscore
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
```

```fall()``` функцијата:

```csharp
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
```

функцијата ```deleteRows()```:

```csharp
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
```

НАПОМЕНА! Сите три функции се објаснети со коментарите.



