using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Numerics;

namespace Solve_Funktion
{
    public static class SynchronizedRandom
    {
        private static int seedCounter = 100;// new Random().Next();

        [ThreadStatic]
        private static Random RDom;

        public static void CreateRandom()
        {
            if (RDom == null)
            {
                int seed = Interlocked.Increment(ref seedCounter);
                RDom = new Random(seed);
            }
        }

        public static int Next(int Start, int End)
        {
            return RDom.Next(Start, End);
        }

        public static Vector<double> NextVector(int Start, int End)
        {
            return new Vector<double>(RDom.Next(Start, End));
        }

        public static bool RandomBool()
        {
            return RDom.Next(0, 2) == 1 ? true : false;
        }
    }
}