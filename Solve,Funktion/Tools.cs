using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Numerics;

namespace Solve_Funktion
{
    public static class Tools
    {
        public static bool IsANumber(Vector<double> ToDecide)
        {
            for (int i = 0; i < Vector<double>.Count; i++)
            {
                if (double.IsNaN(ToDecide[i]) || double.IsInfinity(ToDecide[i]))
                {
                    return false;
                }
            }
            return true;
        }
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
                Operator OrigOper = Original.AllOperators[i];
                Operator CopyOper = Copy.AllOperators[i];
                if (OrigOper.Number    != CopyOper.Number    ||
                    OrigOper.MFunction != CopyOper.MFunction ||
                    OrigOper.ResultOnRightSide      != CopyOper.ResultOnRightSide      ||
                    OrigOper.UseNumber != CopyOper.UseNumber)
                {
                    return false;
                }
            }
            return true;
        }

        public static Vector<double> CreateVector(double number)
        {
            double[] vectorValues = new double[Vector<double>.Count];
            for (int i = 0; i < Vector<double>.Count; i++)
            {
                vectorValues[i] = number;
            }
            return new Vector<double>(vectorValues);
        }
    }
}
