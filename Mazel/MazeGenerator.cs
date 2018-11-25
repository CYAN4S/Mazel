using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mazel
{
    static class MazeGenerator
    {
        static Random r = new Random();

        public static void AddNeighbor(List<ArrayPoint2D> list, ArrayPoint2D position, ArrayPoint2D mazeSize)
        {
            if (position.r != 0)
                list.Add(new ArrayPoint2D(position.r - 1, position.c));

            if (position.c != 0)
                list.Add(new ArrayPoint2D(position.r, position.c - 1));

            if (position.r != mazeSize.r - 1)
                list.Add(new ArrayPoint2D(position.r + 1, position.c));

            if (position.c != mazeSize.c - 1)
                list.Add(new ArrayPoint2D(position.r, position.c + 1));
        }

        public static void RecursiveBacktracker(Maze maze)
        {
            maze.SetUsage(Usage.VISITATION);

            ArrayPoint2D current = maze.StartPoint;
            maze.Visit(current);

            ArrayPoint2D size = maze.GetSize();

            Stack<ArrayPoint2D> PointStack = new Stack<ArrayPoint2D>();

            while (!maze.HasVisitedAll())
            {
                List<ArrayPoint2D> avail = new List<ArrayPoint2D>();
                AddNeighbor(avail, current, size);
                avail.RemoveAll(maze.HasVisited);
                if (avail.Count() != 0)
                {
                    ArrayPoint2D target = avail[r.Next(avail.Count())];
                    PointStack.Push(current);
                    maze.RemoveWallBetween(current, target);
                    maze.Visit(target);
                    current = target;
                }
                else
                {
                    current = PointStack.Pop();
                }
            }
        }
    }
}
