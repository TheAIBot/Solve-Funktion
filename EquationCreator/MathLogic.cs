using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquationCreator
{
    public static class MathLogic
    {
        public static float NOT(float a)
        {
            return 1 - a;
        }

        public static float AND(float a, float b)
        {
            return (a == 1 && b == 1) ? 1 : 0;
        }

        public static float NAND(float a, float b)
        {
            return NOT(AND(a, b));
        }

        public static float OR(float a, float b)
        {
            return (a != b || a == b && a != 0) ? 1 : 0;
        }

        public static float NOR(float a, float b)
        {
            return NOT(OR(a, b));
        }

        public static float XOR(float a, float b)
        {
            return  (a != b &&
                     (a == 1 ||
                      a == 0) &&
                     (b == 1 ||
                      b == 0)) ? 1 : 0;
        }

        public static float XNOR(float a, float b)
        {
            return NOT(XOR(a, b));
        }
    }
}
