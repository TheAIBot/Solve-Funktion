using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EquationCreator
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

        public CoordInfo(string SequenceX, string SequenceY)
        {
            string[] lines = Regex.Split(SequenceX, "} *,");
            string[] refined = lines.Select(x => Regex.Replace(x, "[ =}]", String.Empty)).ToArray();
            string[][] namesAndValues = refined.Select(x => x.Split('{')).ToArray();
            this.parameterNames = namesAndValues.Select(x => x[0]).ToArray();
            this.parameters = namesAndValues.Select(x => x[1].Split(',').Select(z => Convert.ToDouble(z, CultureInfo.InvariantCulture.NumberFormat)).ToArray()).ToArray();
            this.expectedResults = SequenceY.Split(',').Select(x => Convert.ToDouble(x, CultureInfo.InvariantCulture.NumberFormat)).ToArray();
        }
    }
}
