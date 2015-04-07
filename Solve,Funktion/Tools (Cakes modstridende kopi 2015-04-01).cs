using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Solve_Funktion
{
    public static class Tools
    {
        public static bool IsANumber(double ToDecide)
        {
            if (double.IsNaN(ToDecide) || double.IsInfinity(ToDecide))
            {
                return false;
            }
            return true;
        }

        public static T DeepCopy<T>(T Other)
        {
            using (MemoryStream Ms = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(Ms, Other);
                Ms.Position = 0;
                return (T)formatter.Deserialize(Ms);
            }
        }

        public static bool IsEquationsTheSame(Equation Original, Equation Copy)
        {
            if (Original.AllOperators.Count != Copy.AllOperators.Count)
            {
                return false;
            }
            for (int i = 0; i < Original.AllOperators.Count; i++)
            {
                Operator OrigOper = Original.AllOperators.ElementAt(i);
                Operator CopyOper = Copy.AllOperators.ElementAt(i);
                if (OrigOper.Number != CopyOper.Number ||
                    OrigOper.MFunction != CopyOper.MFunction ||
                    OrigOper.Side != CopyOper.Side ||
                    OrigOper.UseNumber != CopyOper.UseNumber)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
