using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Mazel
{
    class MazeGame : Maze
    {
        public MazeGame(ArrayPoint2D size, ArrayPoint2D start, ArrayPoint2D end) : base(size, start, end)
        {
        }
    }
}
