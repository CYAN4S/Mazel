using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;

namespace Mazel
{
    class MazeConverter
    {
        public static void Save(Maze maze)
        {
            using (FileStream fs = new FileStream("maze.mzl", FileMode.Create))
            {
                StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);

                sw.WriteLine(maze.GetSize().r + " " + maze.GetSize().c);
                
                foreach (List<bool> i in maze.HolWalls)
                {
                    foreach (bool j in i)
                    {
                        sw.Write((j ? 1 : 0) + " ");
                    }
                    sw.WriteLine();
                }
                
                foreach (List<bool> i in maze.VerWalls)
                {
                    foreach (bool j in i)
                    {
                        sw.Write((j ? 1 : 0) + " ");
                    }
                    sw.WriteLine();
                }

                sw.Flush();
            }
        }

        public static bool Open(ref Maze maze)
        {
            StreamReader sr;
            try
            {
                sr = new StreamReader("maze.mzl");
            }
            catch (FileNotFoundException)
            {
                MessageBoxResult result = MessageBox.Show("maze.mzl 파일이 존재하지 않습니다.", "Wait...");
                return false;
            }
            
            string line;

            line = sr.ReadLine().Trim();
            string[] sizeString = line.Split(' ');

            ArrayPoint2D size = new ArrayPoint2D(int.Parse(sizeString[0]), int.Parse(sizeString[1]));
            maze = new Maze(size, new ArrayPoint2D(0, 0), new ArrayPoint2D(0, 0));

            for (int i = 0; i < size.r - 1; i++)
            {
                int[] read = Array.ConvertAll(sr.ReadLine().Trim().Split(' '), int.Parse);
                for (int j = 0; j < size.c; j++)
                {
                    maze.HolWalls[i][j] = read[j] == 1 ? true : false;
                }
            }

            for (int i = 0; i < size.r; i++)
            {
                int[] read = Array.ConvertAll(sr.ReadLine().Trim().Split(' '), int.Parse);
                for (int j = 0; j < size.c - 1; j++)
                {
                    maze.VerWalls[i][j] = read[j] == 1 ? true : false;
                }
            }
            return true;
        }
    }
}
