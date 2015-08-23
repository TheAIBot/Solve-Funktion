﻿using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Numerics;
using System;

namespace Solve_Funktion
{
    public static class Tools
    {
        public static bool IsANumber(Vector<double> ToDecide)
        {
            for (int i = 0; i < Constants.VECTOR_LENGTH; i++)
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
            double[] vectorValues = new double[Constants.VECTOR_LENGTH];
            for (int i = 0; i < Constants.VECTOR_LENGTH; i++)
            {
                vectorValues[i] = number;
            }
            return new Vector<double>(vectorValues);
        }

        public static double[] GetPartOfVectorResult(Vector<double> vec, int count)
        {
            double[] result = new double[Constants.VECTOR_LENGTH];
            vec.CopyTo(result);
            double[] toReturn = new double[count];
            Array.Copy(result, toReturn, count);
            return toReturn;
        }
    }
}
