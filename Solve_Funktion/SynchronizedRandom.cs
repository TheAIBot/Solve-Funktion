using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Solve_Funktion
{
    public class SynchronizedRandom
    {
        private static int seedCounter = 100;// new Random().Next();

        private readonly Random RDom;

        public SynchronizedRandom()
        {
            int seed = Interlocked.Increment(ref seedCounter);
            RDom = new Random(seed);
        }

        public int Next(int Start, int End)
        {
            return RDom.Next(Start, End);
        }

        public bool RandomBool()
        {
            return RDom.Next(0, 2) == 1 ? true : false;
        }
    }
}