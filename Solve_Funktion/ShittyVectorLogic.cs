using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Solve_Funktion
{
    public static class ShittyVectorLogic
    {
        public static Vector<double> NOT(Vector<double> a)
        {
            return Vector.Abs<double>(Vector<double>.One - a);
        }

        public static Vector<double> AND(Vector<double> a, Vector<double> b)
        {
            double[] result = new double[Constants.VECTOR_LENGTH];
            for (int i = 0; i < Constants.VECTOR_LENGTH; i++)
            {
                result[i] = (a[i] == 1 &&
                             b[i] == 1) ? 1 : 0;
            }
            return new Vector<double>(result);
        }

        public static Vector<double> NAND(Vector<double> a, Vector<double> b)
        {
            return NOT(AND(a, b));
        }

        public static Vector<double> OR(Vector<double> a, Vector<double> b)
        {
            double[] result = new double[Constants.VECTOR_LENGTH];
            for (int i = 0; i < Constants.VECTOR_LENGTH; i++)
            {
                result[i] = (a[i] != b[i] ||
                             a[i] == b[i] &&
                             a[i] != 0) ? 1 : 0;
            }
            return new Vector<double>(result);
        }

        public static Vector<double> NOR(Vector<double> a, Vector<double> b)
        {
            return NOT(OR(a, b));
        }

        public static Vector<double> XOR(Vector<double> a, Vector<double> b)
        {
            double[] result = new double[Constants.VECTOR_LENGTH];
            for (int i = 0; i < Constants.VECTOR_LENGTH; i++)
            {
                result[i] = (a[i] != b[i] &&
                             (a[i] == 1 ||
                              a[i] == 0) &&
                             (b[i] == 1 ||
                              b[i] == 0)) ? 1 : 0;
            }
            return new Vector<double>(result);
        }

        public static Vector<double> XNOR(Vector<double> a, Vector<double> b)
        {
            return NOT(XOR(a, b));
        }
    }
}
