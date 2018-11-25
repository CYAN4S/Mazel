using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mazel
{
    struct ArrayPoint2D
    {
        public int r;
        public int c;

        public ArrayPoint2D(int r, int c)
        {
            this.r = r;
            this.c = c;
        }

        public static ArrayPoint2D operator+(ArrayPoint2D left, ArrayPoint2D right)
        {
            ArrayPoint2D result;
            result.r = left.r + right.r;
            result.c = left.c + right.c;
            return result;
        }
    }

    enum Usage
    {
        VISITATION,
        SECTOR
    }

    class Maze
    {
        List<List<int>> Cells;
        Usage usage;

        public List<List<bool>> HolWalls { get; }
        public List<List<bool>> VerWalls { get; }

        public ArrayPoint2D StartPoint { get; set; }
        public ArrayPoint2D EndPoint { get; set; }

        public ArrayPoint2D GetSize()
        {
            ArrayPoint2D result;
            result.r = Cells.Count();
            result.c = Cells[0].Count();
            return result;
        }

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

        public bool RemoveWallBetween(ArrayPoint2D pos0, ArrayPoint2D pos1)
        {
            if (pos0.r == pos1.r)
            {
                VerWalls[pos0.r][pos0.c > pos1.c ? pos1.c : pos0.c] = false;
                return true;
            }
            else if (pos0.c == pos1.c)
            {
                HolWalls[pos0.r > pos1.r ? pos1.r : pos0.r][pos0.c] = false;
                return true;
            }
            else
                return false;
        }
        
        public void SetUsage(Usage usage)
        {
            this.usage = usage;
            foreach (List<int> list in Cells)
            {
                for (int i = 0; i < list.Count; i++)
                    list[i] = 0;
            }
        }

        // VISITATION //
        public bool Visit(ArrayPoint2D point)
        {
            if (usage == Usage.VISITATION)
            {
                Cells[point.r][point.c] = 1;
                return true;
            }
            else
                return false;
        }

        public bool HasVisited(ArrayPoint2D point)
        {
            return Cells[point.r][point.c] == 1;
        }

        public bool HasVisitedAll()
        {
            if (usage != Usage.VISITATION)
                return false;

            foreach (var i in Cells)
            {
                if (i.Contains(0))
                    return false;
            }
            return true;
        }

        // SECTOR //
    }
}
