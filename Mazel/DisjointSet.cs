using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mazel
{
    class DisjointSet<T>
    {
        struct DisjointSetNode
        {
            public T data;
            public int parent;
            public int rank;

            public DisjointSetNode(T data, int parent) : this()
            {
                this.data = data;
                rank = 0;
            }

            public void ChangeParent(int parent) => this.parent = parent;
            public void PlusOneRank() => rank++;
        }

        List<DisjointSetNode> Set;

        public DisjointSet() => Set = new List<DisjointSetNode>();

        public void MakeSet(T input)
        {
            DisjointSetNode node = new DisjointSetNode(input, Set.Count);
            Set.Add(node);
            return;
        }

        public int Find(int index)
        {
            if (index != Set[index].parent)
            {
                Set[index].ChangeParent(Find(Set[index].parent));
            }
            return Set[index].parent;
        }

        public void Union(int a, int b)
        {
            int roota = Find(a);
            int rootb = Find(b);

            if (roota == rootb)
            {
                return;
            }
            if (Set[roota].rank < Set[rootb].rank)
            {
                Set[roota].ChangeParent(rootb);
            }
            else if (Set[roota].rank > Set[rootb].rank)
            {
                Set[rootb].ChangeParent(roota);
            }
            else
            {
                Set[rootb].ChangeParent(roota);
                Set[roota].PlusOneRank();
            }

        }
    }
}
