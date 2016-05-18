using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Numerics;
using System;
using System.Text;

namespace Solve_Funktion
{
    public static class Tools
    {
        public static bool IsANumber(double[] ToDecide)
        {
            for (int i = 0; i < ToDecide.Length; i++)
            {
                if (!IsANumber(ToDecide[i]))
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

        public static T DeepCopy<T>(T ToSer)
        {
            using (MemoryStream Ms = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(Ms, ToSer);
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
                if (OrigOper.RandomNumber != CopyOper.RandomNumber ||
                    OrigOper.MFunction != CopyOper.MFunction ||
                    OrigOper.ResultOnRightSide != CopyOper.ResultOnRightSide ||
                    OrigOper.UseRandomNumber != CopyOper.UseRandomNumber)
                {
                    return false;
                }
            }
            return true;
        }

        public static string ReverseAddStringBuilder(StringBuilder toReverse, StringBuilder toAdd)
        {
            //reverse
            char[] toReverseAdd = new char[toReverse.Length + toAdd.Length];
            toReverse.CopyTo(0, toReverseAdd, toAdd.Length, toReverse.Length);
            Array.Reverse(toReverseAdd);

            //add
            toAdd.CopyTo(0, toReverseAdd, toReverse.Length, toAdd.Length);
            return new String(toReverseAdd);
        }
    }
}
