using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Mazel
{
    class MazeGame
    {
        public static bool isRunning = false;
        public static ArrayPoint2D playerPos = new ArrayPoint2D();

        public static bool MovePlayer(Maze maze, Key dir)
        {
            maze.Cells[playerPos.r][playerPos.c] = 0;

            switch (dir)
            {
                case Key.W:
                    if (playerPos.r > 0 && maze.HolWalls[playerPos.r - 1][playerPos.c] == false)
                    {
                        playerPos.r--;
                    }
                    break;

                case Key.S:
                    if (playerPos.r < maze.GetSize().r - 1 && maze.HolWalls[playerPos.r][playerPos.c] == false)
                    {
                        playerPos.r++;
                    }
                    break;

                case Key.A:
                    if (playerPos.c > 0 && maze.VerWalls[playerPos.r][playerPos.c - 1] == false)
                    {
                        playerPos.c--;
                    }
                    break;

                case Key.D:
                    if (playerPos.c < maze.GetSize().c - 1 && maze.VerWalls[playerPos.r][playerPos.c] == false)
                    {
                        playerPos.c++;
                    }
                    break;

                default:
                    maze.Cells[playerPos.r][playerPos.c] = 4;
                    return false;
            }

            maze.Cells[playerPos.r][playerPos.c] = 4;
            return true;
        }
    }
}
