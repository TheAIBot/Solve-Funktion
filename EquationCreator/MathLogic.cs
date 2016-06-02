using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquationCreator
{
    public static class MathLogic
    {
        public static double NOT(double a)
        {
            return 1 - a;
        }

        public static double AND(double a, double b)
        {
            return (a == 1 && b == 1) ? 1 : 0;
        }

        public static double NAND(double a, double b)
        {
            return NOT(AND(a, b));
        }

        public static double OR(double a, double b)
        {
            return (a != b || a == b && a != 0) ? 1 : 0;
        }

        public static double NOR(double a, double b)
        {
            return NOT(OR(a, b));
        }

        public static double XOR(double a, double b)
        {
            return  (a != b &&
                     (a == 1 ||
                      a == 0) &&
                     (b == 1 ||
                      b == 0)) ? 1 : 0;
        }

        public static double XNOR(double a, double b)
        {
            return NOT(XOR(a, b));
        }
    }
}
