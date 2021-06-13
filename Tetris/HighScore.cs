using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    [Serializable]
    public class HighScore
    {
        public int highScore { get; set; }

        public HighScore()
        {
            highScore = 0;
        }
    }
}
