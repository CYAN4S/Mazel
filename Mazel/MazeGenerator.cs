using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace Mazel
{
    static class MazeGenerator
    {
        static Random random = new Random();

        // Place to Add, Current Position, Maze Size
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

        #region Log Writer
        public static void StartWrite()
        {
            using (FileStream fs = new FileStream("test.log", FileMode.Create))
            {
                StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
                sw.WriteLine("WRITE START");
                sw.Flush();
            }
        }
        public static void AppendWrite(string line)
        {
            using (FileStream fs = new FileStream("test.log", FileMode.Append))
            {
                StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
                sw.WriteLine(line);
                sw.Flush();
            }
        }
        #endregion

        static bool HasVisitedAll(List<List<bool>> vs)
        {
            foreach (var i in vs)
            {
                if (i.Contains(false))
                    return false;
            }
            return true;
        }

        public static void RecursiveBacktracker(Maze maze, Action action)
        {
            #region INITIALIZE
            ArrayPoint2D size = maze.GetSize();
            List<List<bool>> hasVisited = new List<List<bool>>();

            for (int i = 0; i < size.r; i++)
            {
                hasVisited.Add(new List<bool>(size.c));
                for (int j = 0; j < size.c; j++)
                    hasVisited[i].Add(false);
            }
            #endregion

            using (FileStream fs = new FileStream("test.log", FileMode.Create))
            {
                StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
                #region ALGORITHM
                // CODE STARTS HERE //
                ArrayPoint2D current = maze.StartPoint;
                hasVisited[current.r][current.c] = true;
                sw.WriteLine("Recursive Backtracker 생성입니다. 현재 위치는 " + current + " 입니다."); // LOG

                Stack<ArrayPoint2D> PointStack = new Stack<ArrayPoint2D>();

                while (!HasVisitedAll(hasVisited))
                {
                    List<ArrayPoint2D> avail = new List<ArrayPoint2D>();
                    AddNeighbor(avail, current, size);
                    avail.RemoveAll(_ => hasVisited[_.r][_.c]);
                    if (avail.Count() != 0)
                    {
                        ArrayPoint2D target = avail[random.Next(avail.Count())];
                        PointStack.Push(current);
                        sw.WriteLine("스택에 " + current + " 가 푸시되었습니다."); // LOG
                        maze.RemoveWallBetween(current, target, action);
                        sw.WriteLine(current + " 와 " + target + " 사이 벽이 제거되었습니다."); // LOG
                        action(); // GRAPHIC
                        hasVisited[target.r][target.c] = true;
                        current = target;
                        sw.WriteLine("현재 위치는 이제 " + current + " 입니다."); // LOG
                    }
                    else
                    {
                        current = PointStack.Pop();
                        sw.WriteLine("현재 위치를 스택에서 팝된 데이터로 설정합니다. " + current + " 가 팝되었습니다."); // LOG
                    }
                }
                #endregion
                sw.Flush();
            }
        }

        class WallWithDirection
        {
            public WallWithDirection(bool isVerWall, ArrayPoint2D wall)
            {
                IsVerWall = isVerWall;
                Wall = wall;
            }
            public bool IsVerWall { get; set; }
            public ArrayPoint2D Wall { get; set; }
        }

        public static void Kruskal(Maze maze, Action action)
        {
            #region INITIALIZE

            ArrayPoint2D size = maze.GetSize();

            DisjointSet<ArrayPoint2D> disjointSet = new DisjointSet<ArrayPoint2D>();

            List<WallWithDirection> WallList = new List<WallWithDirection>();
            List<List<int>> CellIndex = new List<List<int>>();

            for (int i = 0; i < size.r - 1; i++)
            {
                for (int j = 0; j < size.c; j++)
                {
                    WallList.Add(new WallWithDirection(false, new ArrayPoint2D(i, j)));
                }
            }
            for (int i = 0; i < size.r; i++)
            {
                for (int j = 0; j < size.c - 1; j++)
                {
                    WallList.Add(new WallWithDirection(true, new ArrayPoint2D(i, j)));
                }
            }
            for (int i = 0; i < size.r; i++)
            {
                CellIndex.Add(new List<int>());
                for (int j = 0; j < size.c; j++)
                {
                    CellIndex[i].Add(-1);
                }
            }

            #endregion

            using (FileStream fs = new FileStream("test.log", FileMode.Create))
            {
                StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);

                #region ALGORITHM
                // CODE STARTS HERE //
                while (WallList.Count != 0)
                {
                    sw.WriteLine("Recursive Backtracker 생성입니다."); // LOG
                    int rand = random.Next(WallList.Count);
                    WallWithDirection target = WallList[rand];
                    ArrayPoint2D ULPos = target.Wall;
                    ArrayPoint2D DRPos = target.Wall + (target.IsVerWall ? new ArrayPoint2D(0, 1) : new ArrayPoint2D(1, 0));
                    sw.WriteLine("현재 대상 벽은 " + ULPos + " 와 " + DRPos + " 사이의 벽입니다."); // LOG

                    ArrayPoint2D[] targetCells = { ULPos, DRPos };
                    foreach (var item in targetCells)
                    {
                        if (CellIndex[item.r][item.c] == -1)
                        {
                            CellIndex[item.r][item.c] = disjointSet.GetTreeCount();
                            disjointSet.MakeSet(item);
                            sw.WriteLine("Disjoint Set에 " + item + "을 추가합니다."); // LOG
                        }
                    }
                    
                    if (disjointSet.Find(CellIndex[ULPos.r][ULPos.c]) != disjointSet.Find(CellIndex[DRPos.r][DRPos.c]))
                    {
                        disjointSet.Union(CellIndex[ULPos.r][ULPos.c], CellIndex[DRPos.r][DRPos.c]);
                        maze.RemoveWallBetween(ULPos, DRPos, action);
                        sw.WriteLine(ULPos + " 와 " + DRPos + " 사이 벽이 제거되었습니다."); // LOG
                    }

                    WallList.RemoveAt(rand);
                }

                // CODE ENDS HERE //
                #endregion

                sw.Flush();
            }
        }

        public static void HuntAndKill(Maze maze, Action action)
        {
            #region INITIALIZE
            ArrayPoint2D size = maze.GetSize();
            List<List<bool>> hasVisited = new List<List<bool>>();

            for (int i = 0; i < size.r; i++)
            {
                hasVisited.Add(new List<bool>(size.c));
                for (int j = 0; j < size.c; j++)
                    hasVisited[i].Add(false);
            }
            #endregion

            using (FileStream fs = new FileStream("test.log", FileMode.Create))
            {
                StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);

                #region ALGORITHM
                // CODE STARTS HERE //
                ArrayPoint2D current = new ArrayPoint2D(random.Next(size.r), random.Next(size.c));
                hasVisited[current.r][current.c] = true;
                sw.WriteLine("Hunt-And-Kill 생성입니다. 현재 위치는 " + current + " 입니다."); // LOG

                while (!HasVisitedAll(hasVisited))
                {
                    List<ArrayPoint2D> avail = new List<ArrayPoint2D>();
                    AddNeighbor(avail, current, size);
                    avail.RemoveAll(_ => hasVisited[_.r][_.c]);
                    while (avail.Count() != 0)
                    {
                        ArrayPoint2D target = avail[random.Next(avail.Count())];
                        maze.RemoveWallBetween(current, target, action);
                        sw.WriteLine("미로를 생성 중입니다. " + current + " 와 " + target + " 사이 벽이 제거되었습니다.");
                        hasVisited[target.r][target.c] = true;
                        current = target;
                        avail.Clear();
                        AddNeighbor(avail, current, size);
                        avail.RemoveAll(_ => hasVisited[_.r][_.c]);
                    }

                    bool restart = false; // 이중 반복문 탈출을 위해
                    ArrayPoint2D loop;
                    for (int i = 0; i < size.r; i++)
                    {
                        for (int j = 0; j < size.c; j++)
                        {
                            loop = new ArrayPoint2D(i, j);
                            if (!hasVisited[loop.r][loop.c])
                            {
                                List<ArrayPoint2D> related = new List<ArrayPoint2D>();
                                AddNeighbor(related, loop, size);
                                related.RemoveAll(_ => !hasVisited[_.r][_.c]);

                                if (related.Count == 0)
                                    continue;

                                ArrayPoint2D target = related[random.Next(related.Count())];
                                maze.RemoveWallBetween(loop, target, action);
                                sw.WriteLine("새로운 지역을 탐색합니다. " + loop + ", " + target + " 사이 벽이 제거되었습니다.");
                                hasVisited[loop.r][loop.c] = true;
                                current = loop;
                                restart = true;
                                sw.WriteLine("새로운 현재 위치는 " + current + " 입니다."); // LOG
                                break;
                            }
                        }
                        if (restart) break;
                    }
                }

                // CODE ENDS HERE //
                #endregion

                sw.Flush();
            }
        }
    }
}
