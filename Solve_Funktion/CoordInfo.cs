using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solve_Funktion
{
    public sealed class CoordInfo
    {
        public double[] expectedResults;
        public double[][] parameters;
        public string[] parameterNames;

        public CoordInfo(double[] resultss, double[][] parameterss, string[] parameterNamess)
        {
            this.expectedResults = resultss;
            this.parameters = parameterss;
            this.parameterNames = parameterNamess;
        }
    }
}
