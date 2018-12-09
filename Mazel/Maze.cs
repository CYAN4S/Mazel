using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Threading;

namespace Mazel
{
    class Maze
    {
        public List<List<int>> Cells { get; }
        
        public List<List<bool>> HolWalls { get; }
        public List<List<bool>> VerWalls { get; }

        public ArrayPoint2D StartPoint { get; set; }
        public ArrayPoint2D EndPoint { get; set; }

        public bool isMaze = false;

        
        public Maze(ArrayPoint2D size, ArrayPoint2D start, ArrayPoint2D end)
        {
            Cells = new List<List<int>>(size.r);
            for (int i = 0; i < size.r; i++)
            {
                Cells.Add(new List<int>(size.c));
                for (int j = 0; j < size.c; j++)
                    Cells[i].Add(0);
            }

            HolWalls = new List<List<bool>>(size.r - 1);
            for (int i = 0; i < size.r - 1; i++)
            {
                HolWalls.Add(new List<bool>(size.c));
                for (int j = 0; j < size.c; j++)
                    HolWalls[i].Add(true);
            }

            VerWalls = new List<List<bool>>(size.r);
            for (int i = 0; i < size.r; i++)
            {
                VerWalls.Add(new List<bool>(size.c - 1));
                for (int j = 0; j < size.c - 1; j++)
                    VerWalls[i].Add(true);
            }
        }

        public ArrayPoint2D GetSize()
        {
            ArrayPoint2D result;
            result.r = Cells.Count();
            result.c = Cells[0].Count();
            return result;
        }

        public bool RemoveWallBetween(ArrayPoint2D pos0, ArrayPoint2D pos1, Action action)
        {
            if (pos0.r == pos1.r)
            {
                VerWalls[pos0.r][pos0.c > pos1.c ? pos1.c : pos0.c] = false;
            }
            else if (pos0.c == pos1.c)
            {
                HolWalls[pos0.r > pos1.r ? pos1.r : pos0.r][pos0.c] = false;
            }
            else
            {
                return false;
            }
            action();
            Thread.Sleep(MainWindow.delayTime);
            return true;
        }

        public bool HasWallBetween(ArrayPoint2D pos0, ArrayPoint2D pos1)
        {
            if (pos0.r == pos1.r)
            {
                return VerWalls[pos0.r][pos0.c > pos1.c ? pos1.c : pos0.c];
            }
            else if (pos0.c == pos1.c)
            {
                return HolWalls[pos0.r > pos1.r ? pos1.r : pos0.r][pos0.c];
            }
            else
            {
                return false;
            }
        }
    }
}
