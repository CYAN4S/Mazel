using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Windows.Threading;
using System.Threading;

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

        public override bool Equals(object obj)
        {
            if (!(obj is ArrayPoint2D))
            {
                return false;
            }

            var d = (ArrayPoint2D)obj;
            return r == d.r &&
                   c == d.c;
        }

        public override int GetHashCode()
        {
            var hashCode = -1382643793;
            hashCode = hashCode * -1521134295 + r.GetHashCode();
            hashCode = hashCode * -1521134295 + c.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            return "(" + r + ", " + c + ")";
        }

        public static ArrayPoint2D operator +(ArrayPoint2D left, ArrayPoint2D right)
        {
            ArrayPoint2D result;
            result.r = left.r + right.r;
            result.c = left.c + right.c;
            return result;
        }

        public static ArrayPoint2D operator -(ArrayPoint2D left, ArrayPoint2D right)
        {
            ArrayPoint2D result;
            result.r = left.r - right.r;
            result.c = left.c - right.c;
            return result;
        }

        //public static ArrayPoint2D operator *(ArrayPoint2D arrayPoint2D, int num)
        //{
        //    ArrayPoint2D result;
        //    result.r = arrayPoint2D.r * num;
        //    result.c = arrayPoint2D.c * num;
        //    return result;
        //}

        public static bool operator ==(ArrayPoint2D left, ArrayPoint2D right)
        {
            return (left.r == right.r && left.c == right.c);
        }

        public static bool operator !=(ArrayPoint2D left, ArrayPoint2D right)
        {
            return (left.r != right.r || left.c != right.c);
        }

        public bool IsValidPosition(ArrayPoint2D size)
        {
            return r > -1 && c > -1 && r < size.r && c < size.c;
        }
    }
}