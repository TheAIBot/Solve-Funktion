using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Solve_Funktion
{
    public static class Constants
    {
        /// a vector only containing NaN values
        public static readonly Vector<double> NAN_VECTOR = Tools.CreateVector(double.NaN);
        /// this is not supposed to be commented out ut there is a bug in the compiler :(
        public const int VECTOR_LENGTH = 2;// = Vector<double>.Count;
    }
}
