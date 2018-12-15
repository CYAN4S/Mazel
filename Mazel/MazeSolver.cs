using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mazel
{
    class MazeSolver
    {
        static void AddNeighbor(List<ArrayPoint2D> list, ArrayPoint2D position, ArrayPoint2D mazeSize)
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

        static bool HasVisitedAll(List<List<bool>> vs)
        {
            foreach (var i in vs)
            {
                foreach (var j in i)
                {
                    if (!j)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        
        struct PathNode
        {
            public ArrayPoint2D point;
            public List<ArrayPoint2D> path;

            public PathNode(ArrayPoint2D point)
            {
                this.point = point;
                path = new List<ArrayPoint2D>
                {
                    this.point
                };
            }

            public PathNode(ArrayPoint2D point, PathNode history)
            {
                this.point = point;
                path = new List<ArrayPoint2D>(history.path)
                {
                    this.point
                };
            }
        }

        public static void BFS(Maze maze, Action action)
        {
            #region INITIALIZE
            ArrayPoint2D size = maze.GetSize();
            Queue<PathNode> que = new Queue<PathNode>();
            List<List<bool>> hasVisited = new List<List<bool>>();
            ArrayPoint2D[] dirs = { new ArrayPoint2D(-1, 0), new ArrayPoint2D(1, 0), new ArrayPoint2D(0, -1), new ArrayPoint2D(0, 1)};
            PathNode answer = new PathNode();

            for (int i = 0; i < size.r; i++)
            {
                hasVisited.Add(new List<bool>(size.c));
                for (int j = 0; j < size.c; j++)
                    hasVisited[i].Add(false);
            }
            #endregion
            
            que.Enqueue(new PathNode(maze.StartPoint));
            hasVisited[maze.StartPoint.r][maze.StartPoint.c] = true;

            while (que.Count != 0)
            {
                PathNode target = que.Dequeue();
                maze.Cells[target.point.r][target.point.c] = 1;

                if (target.point == maze.EndPoint)
                {
                    answer = target;
                    break;
                }

                foreach (var item in dirs)
                {
                    ArrayPoint2D enqueuePos = target.point + item;
                    if (!enqueuePos.IsValidPosition(size))
                        continue;

                    if (maze.HasWallBetween(target.point, enqueuePos))
                        continue;

                    if (hasVisited[enqueuePos.r][enqueuePos.c])
                        continue;

                    que.Enqueue(new PathNode(enqueuePos, target));
                    hasVisited[enqueuePos.r][enqueuePos.c] = true;
                }
                action();
                Thread.Sleep(MainWindow.delayTime);
            }

            foreach (var item in answer.path)
            {
                maze.Cells[item.r][item.c] = 2;
                action();
                Thread.Sleep(MainWindow.delayTime);
            }
            
        }
    }
}
