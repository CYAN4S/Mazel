using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mazel
{
    class DisjointSet<T>
    {
        List<DisjointSetNode> Tree;

        class DisjointSetNode
        {
            public T Data { get; set; }
            public int Parent { get; set; }
            public int Rank { get; set; }

            public DisjointSetNode(T data)
            {
                Data = data;
                Rank = 0;
            }
        }

        public int GetTreeCount()
        {
            return Tree.Count;
        }
        
        public DisjointSet()
        {
            Tree = new List<DisjointSetNode>();
        }

        public void MakeSet(T input)
        {
            DisjointSetNode node = new DisjointSetNode(input)
            {
                Parent = Tree.Count
            };
            Tree.Add(node);
            return;
        }

        public int Find(int index)
        {
            if (index != Tree[index].Parent)
            {
                Tree[index].Parent = Find(Tree[index].Parent);
            }
            return Tree[index].Parent;
        }

        public void Union(int a, int b)
        {
            int roota = Find(a);
            int rootb = Find(b);

            if (roota == rootb)
            {
                return;
            }

            if (Tree[roota].Rank < Tree[rootb].Rank)
            {
                Tree[roota].Parent = rootb;
            }
            else if (Tree[roota].Rank > Tree[rootb].Rank)
            {
                Tree[rootb].Parent = roota;
            }
            else
            {
                Tree[rootb].Parent = roota;
                Tree[roota].Rank++;
            }
        }
    }
}
