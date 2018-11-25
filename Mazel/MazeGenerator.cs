using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mazel
{
    static class MazeGenerator
    {
        static Random random = new Random();

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
                    ArrayPoint2D target = avail[random.Next(avail.Count())];
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

        public static void Kruskal(Maze maze)
        {
            maze.SetUsage(Usage.SECTOR);

            int sectorNumber = 0;
            for (int i = 0; i < maze.GetSize().r; i++)
            {
                for (int j = 0; j < maze.GetSize().c; j++)
                {
                    maze.SetSector(new ArrayPoint2D(i, j), sectorNumber++);
                }
            }
        }

        public static void HuntAndKill(Maze maze)
        {
            maze.SetUsage(Usage.VISITATION);

            ArrayPoint2D size = maze.GetSize();
            ArrayPoint2D current = new ArrayPoint2D(random.Next(size.r), random.Next(size.c));
            maze.Visit(current);
            
            while (!maze.HasVisitedAll())
            {
                List<ArrayPoint2D> avail = new List<ArrayPoint2D>();
                AddNeighbor(avail, current, size);
                avail.RemoveAll(maze.HasVisited);
                while (avail.Count() != 0)
                {
                    ArrayPoint2D target = avail[random.Next(avail.Count())];
                    maze.RemoveWallBetween(current, target);
                    maze.Visit(target);
                    current = target;
                    avail.Clear();
                    AddNeighbor(avail, current, size);
                    avail.RemoveAll(maze.HasVisited);
                }

                bool restart = false;
                ArrayPoint2D loop;
                for (int i = 0; i < size.r; i++)
                {
                    for (int j = 0; j < size.c; j++)
                    {
                        loop = new ArrayPoint2D(i, j);
                        if (maze.HasVisited(loop))
                        {
                            List<ArrayPoint2D> related = new List<ArrayPoint2D>();
                            AddNeighbor(related, loop, size);
                            related.RemoveAll(s => maze.HasVisited(s));

                            if (related.Count() == 0)
                                continue;

                            ArrayPoint2D target = related[random.Next(related.Count())];
                            maze.RemoveWallBetween(loop, target);
                            maze.Visit(loop);
                            current = loop;
                            restart = true;
                            break;
                        }
                    }
                    if (restart) break;
                }
            }
        }
    }
}
