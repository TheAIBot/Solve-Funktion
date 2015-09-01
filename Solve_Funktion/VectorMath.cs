using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Solve_Funktion
{
    public static class VectorMath
    {
        //public static Vector<double> Modulus(Vector<double> num, Vector<double> mod)
        //{
        //    Vector<double> result = System.Numerics.Vector.Abs(num);
        //    Vector<double> isNegative = Vector.LessThan<double>(num, Vector<double>.Zero);
        //    while ()
        //    {

        //    }
        //}

        public static Vector<double> Pow(Vector<double> num, Vector<double> pow)
        {
            Vector<double> result = num;
            for (double i = 0; i < pow[0]; i++)
            {
                result = result * pow;
            }
            return result;
        }
    }
}
