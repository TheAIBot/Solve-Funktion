using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
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
            if (Original.NumberOfAllOperators != Copy.NumberOfAllOperators)
            {
                return false;
            }
            for (int i = 0; i < Original.AllOperators.Length; i++)
            {
                Operator OrigOper = Original.AllOperators[i];
                Operator CopyOper = Copy.AllOperators[i];
                if (OrigOper != null &&
                    CopyOper != null)
                {
                    if (OrigOper.RandomNumber != CopyOper.RandomNumber ||
                        OrigOper.MFunction != CopyOper.MFunction ||
                        OrigOper.ResultOnRightSide != CopyOper.ResultOnRightSide ||
                        OrigOper.UseRandomNumber != CopyOper.UseRandomNumber)
                    {
                        return false;
                    }
                }
                else if ((OrigOper == null &&
                         CopyOper != null) ||
                         (OrigOper != null &&
                         CopyOper == null))
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

        public static void CompressOperatorArray(Operator[] Operators, int NumberOfOperators, Equation Eq, bool isNotAllOperators) //Crap solution with the bool
        {
            int smallestFreeIndex = 0;
            while (Operators[smallestFreeIndex] != null)
            {
                smallestFreeIndex++;
            }
            int OperatorsToCompressLeft = NumberOfOperators;
            int OperatorToCompressIndex = 0;
            while (OperatorsToCompressLeft > 0)
            {
                if (Operators[OperatorToCompressIndex] != null)
                {
                    OperatorsToCompressLeft--;
                    if (isNotAllOperators)
                    {
                        Operators[OperatorToCompressIndex].Compress(Eq);
                    }
                    if (smallestFreeIndex < OperatorToCompressIndex)
                    {
                        Operators[smallestFreeIndex] = Operators[OperatorToCompressIndex];
                        Operators[OperatorToCompressIndex] = null;
                        if (isNotAllOperators)
                        {
                            Operators[smallestFreeIndex].ContainedIndex = smallestFreeIndex;
                        }
                        else
                        {
                            Operators[smallestFreeIndex].AllOperatorsContainedIndex = smallestFreeIndex;
                        }
                        
                        while (Operators[smallestFreeIndex] != null)
                        {
                            smallestFreeIndex++;
                        }
                    }
                }
                OperatorToCompressIndex++;
            }
        }
    }
}
