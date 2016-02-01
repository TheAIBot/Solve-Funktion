using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Solve_Funktion
{
    public static class ShittyVectorMath
    {
        public static Vector<double> Modulus(Vector<double> num, Vector<double> mod)
        {
            double[] powed = new double[Constants.VECTOR_LENGTH];
            for (int i = 0; i < Constants.VECTOR_LENGTH; i++)
            {
                powed[i] = num[i] % mod[i];
            }
            return new Vector<double>(powed);
        }
        public static Vector<double> BetterModulus(Vector<double> num, Vector<double> mod)
        {
            return num - BetterFloor(num / mod) * mod;
        }

        public static Vector<double> Pow(Vector<double> num, Vector<double> pow)
        {
            double[] powed = new double[Constants.VECTOR_LENGTH];
            for (int i = 0; i < Constants.VECTOR_LENGTH; i++)
            {
                powed[i] = Math.Pow(num[i], pow[i]);
            }
            return new Vector<double>(powed);
        }

        public static Vector<double> Floor(Vector<double> num)
        {
            double[] powed = new double[Constants.VECTOR_LENGTH];
            for (int i = 0; i < Constants.VECTOR_LENGTH; i++)
            {
                powed[i] = Math.Floor(num[i]);
            }
            return new Vector<double>(powed);
        }
        public static Vector<double> BetterFloor(Vector<double> num)
        {
            double[] data = new double[Constants.VECTOR_LENGTH];
            num.CopyTo(data);
            //return new Vector<double>(new Vector<int>(num));
            return Vector.AsVectorDouble<int>(Vector.AsVectorInt32<double>(num));
        }

        public static Vector<double> Ceiling(Vector<double> num)
        {
            double[] powed = new double[Constants.VECTOR_LENGTH];
            for (int i = 0; i < Constants.VECTOR_LENGTH; i++)
            {
                powed[i] = Math.Ceiling(num[i]);
            }
            return new Vector<double>(powed);
        }
        public static Vector<double> BetterCeiling(Vector<double> num)
        {
            return -BetterFloor(-num);
        }

        public static Vector<double> Round(Vector<double> num)
        {
            double[] powed = new double[Constants.VECTOR_LENGTH];
            for (int i = 0; i < Constants.VECTOR_LENGTH; i++)
            {
                powed[i] = Math.Round(num[i]);
            }
            return new Vector<double>(powed);
        }

        public static Vector<double> Acos(Vector<double> num)
        {
            double[] powed = new double[Constants.VECTOR_LENGTH];
            for (int i = 0; i < Constants.VECTOR_LENGTH; i++)
            {
                powed[i] = Math.Acos(num[i]);
            }
            return new Vector<double>(powed);
        }

        public static Vector<double> Asin(Vector<double> num)
        {
            double[] powed = new double[Constants.VECTOR_LENGTH];
            for (int i = 0; i < Constants.VECTOR_LENGTH; i++)
            {
                powed[i] = Math.Asin(num[i]);
            }
            return new Vector<double>(powed);
        }

        public static Vector<double> Atan(Vector<double> num)
        {
            double[] powed = new double[Constants.VECTOR_LENGTH];
            for (int i = 0; i < Constants.VECTOR_LENGTH; i++)
            {
                powed[i] = Math.Atan(num[i]);
            }
            return new Vector<double>(powed);
        }

        public static Vector<double> Atan2(Vector<double> num, Vector<double> num2)
        {
            double[] powed = new double[Constants.VECTOR_LENGTH];
            for (int i = 0; i < Constants.VECTOR_LENGTH; i++)
            {
                powed[i] = Math.Atan2(num[i], num2[i]);
            }
            return new Vector<double>(powed);
        }

        public static Vector<double> Cos(Vector<double> num)
        {
            double[] powed = new double[Constants.VECTOR_LENGTH];
            for (int i = 0; i < Constants.VECTOR_LENGTH; i++)
            {
                powed[i] = Math.Cos(num[i]);
            }
            return new Vector<double>(powed);
        }

        public static Vector<double> Cosh(Vector<double> num)
        {
            double[] powed = new double[Constants.VECTOR_LENGTH];
            for (int i = 0; i < Constants.VECTOR_LENGTH; i++)
            {
                powed[i] = Math.Cosh(num[i]);
            }
            return new Vector<double>(powed);
        }

        public static Vector<double> Exp(Vector<double> num)
        {
            double[] powed = new double[Constants.VECTOR_LENGTH];
            for (int i = 0; i < Constants.VECTOR_LENGTH; i++)
            {
                powed[i] = Math.Exp(num[i]);
            }
            return new Vector<double>(powed);
        }

        public static Vector<double> Log(Vector<double> num)
        {
            double[] powed = new double[Constants.VECTOR_LENGTH];
            for (int i = 0; i < Constants.VECTOR_LENGTH; i++)
            {
                powed[i] = Math.Log(num[i]);
            }
            return new Vector<double>(powed);
        }

        public static Vector<double> Log10(Vector<double> num)
        {
            double[] powed = new double[Constants.VECTOR_LENGTH];
            for (int i = 0; i < Constants.VECTOR_LENGTH; i++)
            {
                powed[i] = Math.Log10(num[i]);
            }
            return new Vector<double>(powed);
        }

        public static Vector<double> Sin(Vector<double> num)
        {
            double[] powed = new double[Constants.VECTOR_LENGTH];
            for (int i = 0; i < Constants.VECTOR_LENGTH; i++)
            {
                powed[i] = Math.Sin(num[i]);
            }
            return new Vector<double>(powed);
        }

        public static Vector<double> Sinh(Vector<double> num)
        {
            double[] powed = new double[Constants.VECTOR_LENGTH];
            for (int i = 0; i < Constants.VECTOR_LENGTH; i++)
            {
                powed[i] = Math.Sinh(num[i]);
            }
            return new Vector<double>(powed);
        }

        public static Vector<double> Tan(Vector<double> num)
        {
            double[] powed = new double[Constants.VECTOR_LENGTH];
            for (int i = 0; i < Constants.VECTOR_LENGTH; i++)
            {
                powed[i] = Math.Tan(num[i]);
            }
            return new Vector<double>(powed);
        }

        public static Vector<double> Tanh(Vector<double> num)
        {
            double[] powed = new double[Constants.VECTOR_LENGTH];
            for (int i = 0; i < Constants.VECTOR_LENGTH; i++)
            {
                powed[i] = Math.Tanh(num[i]);
            }
            return new Vector<double>(powed);
        }
    }
}
