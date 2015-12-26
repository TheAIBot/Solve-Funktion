using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Solve_Funktion
{
    public sealed class VectorPoint
    {
        public Vector<double>[] Parameters;
        public string[] ParameterNames;
        public Vector<double> Result;
        public int Count;

        public VectorPoint(Vector<double>[] parameters, string[] parameterNames, Vector<double> result, int count)
        {
            Parameters = parameters;
            ParameterNames = parameterNames;
            Result = result;
            Count = count;
        }
    }
}
