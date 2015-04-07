using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solve_Funktion
{
    [Serializable]
    public abstract class MathFunction
    {
        public abstract double Calculate(double Result, double x, Operator Operator);
        public abstract string ShowOperator(string Result, string x, Operator Operator);

        public virtual void MakeRandom(Operator Operator)
        {

        }
        public virtual bool CanUseOperator(Operator Operator)
        {
            return true;
        }
        public virtual bool IsConnecter()
        {
            return true;
        }
        public virtual int GetOperatorCount(Operator Operator)
        {
            return 1;
        }

        public virtual void GetCopy(Operator Original, Operator Copy)
        {

        }
        public virtual void StoreAndCleanup(Operator Operator)
        {

        }
    }

    //Standard
    public class Plus : MathFunction
    {
        public override double Calculate(double Result, double x, Operator Operator)
        {
            double Num = (Operator.UseNumber) ? Operator.Number : x;
            return (Operator.Side) ? (Num + Result) : (Result + Num);
        }
        public override string ShowOperator(string Result, string x, Operator Operator)
        {
            string Num = (Operator.UseNumber) ? Operator.Number.ToString() : x;
            return (Operator.Side) ? ("(" + Num + " + " + Result + ")") : ("(" + Result + " + " + Num + ")");
        }
    }
    public class Subtract : MathFunction
    {
        public override double Calculate(double Result, double x, Operator Operator)
        {
            double Num = (Operator.UseNumber) ? Operator.Number : x;
            return (Operator.Side) ? (Num - Result) : (Result - Num);
        }
        public override string ShowOperator(string Result, string x, Operator Operator)
        {
            string Num = (Operator.UseNumber) ? Operator.Number.ToString() : x;
            return (Operator.Side) ? ("(" + Num + " - " + Result + ")") : ("(" + Result + " - " + Num + ")");
        }
    }
    public class Multiply : MathFunction
    {
        public override double Calculate(double Result, double x, Operator Operator)
        {
            double Num = (Operator.UseNumber) ? Operator.Number : x;
            return (Operator.Side) ? (Num * Result) : (Result * Num);
        }
        public override string ShowOperator(string Result, string x, Operator Operator)
        {
            string Num = (Operator.UseNumber) ? Operator.Number.ToString() : x;
            return (Operator.Side) ? ("(" + Num + " * " + Result + ")") : ("(" + Result + " * " + Num + ")");
        }
    }
    public class Divide : MathFunction
    {
        public override double Calculate(double Result, double x, Operator Operator)
        {
            double Num = (Operator.UseNumber) ? Operator.Number : x;
            return (Operator.Side) ? (Num / Result) : (Result / Num);
        }
        public override string ShowOperator(string Result, string x, Operator Operator)
        {
            string Num = (Operator.UseNumber) ? Operator.Number.ToString() : x;
            return (Operator.Side) ? ("(" + Num + " / " + Result + ")") : ("(" + Result + " / " + Num + ")");
        }
    }
    public class Modulos : MathFunction
    {
        public override double Calculate(double Result, double x, Operator Operator)
        {
            double Num = (Operator.UseNumber) ? Operator.Number : x;
            return (Operator.Side) ? (Num % Result) : (Result % Num);
        }
        public override string ShowOperator(string Result, string x, Operator Operator)
        {
            string Num = (Operator.UseNumber) ? Operator.Number.ToString() : x;
            return (Operator.Side) ? ("(" + Num + " % " + Result + ")") : ("(" + Result + " % " + Num + ")");
        }
    }
    public class PowerOf : MathFunction
    {
        public override double Calculate(double Result, double x, Operator Operator)
        {
            double Num = (Operator.UseNumber) ? Operator.Number : x;
            return (Operator.Side) ? Math.Pow(Num, Result) : Math.Pow(Result, Num);
        }
        public override string ShowOperator(string Result, string x, Operator Operator)
        {
            string Num = (Operator.UseNumber) ? Operator.Number.ToString() : x;
            return (Operator.Side) ? ("(" + Num + " ^ " + Result + ")") : ("(" + Result + " ^ " + Num + ")");
        }
    }
    public class Exponent : MathFunction
    {
        public override double Calculate(double Result, double x, Operator Operator)
        {
            return Math.Exp(Result);
        }
        public override string ShowOperator(string Result, string x, Operator Operator)
        {
            return "exp(" + Result + ")";
        }
    }
    public class NaturalLog : MathFunction
    {
        public override double Calculate(double Result, double x, Operator Operator)
        {
            return Math.Log(Result);
        }
        public override string ShowOperator(string Result, string x, Operator Operator)
        {
            return "Ln(" + Result + ")";
        }
    }
    public class Log : MathFunction
    {
        public override double Calculate(double Result, double x, Operator Operator)
        {
            return Math.Log10(Result);
        }
        public override string ShowOperator(string Result, string x, Operator Operator)
        {
            return "Log(" + Result + ")";
        }
    }

    //Rounders
    public class Floor : MathFunction
    {
        public override double Calculate(double Result, double x, Operator Operator)
        {
            return Math.Floor(Result);
        }
        public override string ShowOperator(string Result, string x, Operator Operator)
        {
            return "Floor(" + Result + ")";
        }
    }
    public class Ceil : MathFunction
    {
        public override double Calculate(double Result, double x, Operator Operator)
        {
            return Math.Ceiling(Result);
        }
        public override string ShowOperator(string Result, string x, Operator Operator)
        {
            return "Ceil(" + Result + ")";
        }
    }
    public class Round : MathFunction
    {
        public override double Calculate(double Result, double x, Operator Operator)
        {
            return Math.Round(Result);
        }
        public override string ShowOperator(string Result, string x, Operator Operator)
        {
            return "Round(" + Result + ")";
        }
    }

    //Trigonomic
    public class Sin : MathFunction
    {
        public override double Calculate(double Result, double x, Operator Operator)
        {
            return Math.Sin(Result);
        }
        public override string ShowOperator(string Result, string x, Operator Operator)
        {
            return "Sin(" + Result + ")";
        }
    }
    public class Cos : MathFunction
    {
        public override double Calculate(double Result, double x, Operator Operator)
        {
            return Math.Cos(Result);
        }
        public override string ShowOperator(string Result, string x, Operator Operator)
        {
            return "Cos(" + Result + ")";
        }
    }
    public class Tan : MathFunction
    {
        public override double Calculate(double Result, double x, Operator Operator)
        {
            return Math.Tan(Result);
        }
        public override string ShowOperator(string Result, string x, Operator Operator)
        {
            return "Tan(" + Result + ")";
        }
    }
    public class ASin : MathFunction
    {
        public override double Calculate(double Result, double x, Operator Operator)
        {
            return Math.Asin(Result);
        }
        public override string ShowOperator(string Result, string x, Operator Operator)
        {
            return "ASin(" + Result + ")";
        }
    }
    public class ACos : MathFunction
    {
        public override double Calculate(double Result, double x, Operator Operator)
        {
            return Math.Acos(Result);
        }
        public override string ShowOperator(string Result, string x, Operator Operator)
        {
            return "ACos(" + Result + ")";
        }
    }
    public class ATan : MathFunction
    {
        public override double Calculate(double Result, double x, Operator Operator)
        {
            return Math.Atan(Result);
        }
        public override string ShowOperator(string Result, string x, Operator Operator)
        {
            return "ATan(" + Result + ")";
        }
    }

    //Misc
    [Serializable]
    public class Parentheses : MathFunction
    {
        public override double Calculate(double Result, double x, Operator Operator)
        {
            double Res = x;
            foreach (Operator Oper in Operator.Operators)
            {
                Res = Oper.Calculate(Res, x);
                if (!Tools.IsANumber(Res))
                {
                    return double.NaN;
                }
            }
            return Operator.ExtraMathFunction.Calculate(Result, Res, Operator);
        }
        public override string ShowOperator(string Result, string x, Operator Operator)
        {
            string Res = x;
            foreach (Operator Oper in Operator.Operators)
            {
                Res = Oper.ShowOperator(Res, x);
            }
            return Operator.ExtraMathFunction.ShowOperator(Result, Res, Operator);
        }

        public override void MakeRandom(Operator Operator)
        {
            Operator.Eq.SortedOperators.Add(Operator.Operators);
            Operator.UseNumber = false;
            //the method CanUseOperator makes sure there is atleast 1 available Operator to use in the parentheses
            int AmountToAdd = SynchronizedRandom.Next(0, Operator.Eq.OperatorsLeft - 1);

            while (0 < Operator.Eq.OperatorsLeft - AmountToAdd)
            {
                Operator ToAdd = Operator.Eq.OPStorage.Pop();
                // by adding the operator now there is space for 1 less operator
                ToAdd.MakeRandom(Operator.Eq, Operator.Operators);
            }
            do
            {
                Operator.ExtraMathFunction = Info.Operators[SynchronizedRandom.Next(0, Info.Operators.Count)];
            } while (!Operator.ExtraMathFunction.IsConnecter());
        }
        public override bool CanUseOperator(Operator Operator)
        {
            return Operator.Eq.OperatorsLeft > 0;
        }
        public override bool IsConnecter()
        {
            return false;
        }
        public override int GetOperatorCount(Operator Operator)
        {
            // the parentheses is an operator in itself so the amount starts as 1
            // so the parentheses opearator is accounted for
            int Amount = 1;
            foreach (Operator Oper in Operator.Operators)
            {
                Amount += Oper.GetOperatorCount();
            }
            return Amount;
        }

        public override void GetCopy(Operator Original, Operator Copy)
        {
            Copy.Eq.SortedOperators.Add(Copy.Operators);
            Copy.ExtraMathFunction = Original.ExtraMathFunction;
            foreach (Operator Oper in Original.Operators)
            {
                Oper.GetCopy(Copy.Eq.OPStorage.Pop(), Copy.Eq, Copy.Operators);
            }
        }
        public override void StoreAndCleanup(Operator Operator)
        {
            Operator.Eq.SortedOperators.Remove(Operator.Operators);
            Operator.ExtraMathFunction = null;
            // the Operators list is being altered in this loop so it can't be a foreach loop
            // that's why it's done this way
            while (Operator.Operators.Count > 0)
            {
                Operator.Operators.First().StoreAndCleanup();
            }
        }
    }
    //internal class Summation : Calculatables
    //{

    //}

    //Logic Operators

    public class AND : MathFunction
    {
        public override double Calculate(double Result, double x, Operator Operator)
        {
            double Num = (Operator.UseNumber) ? Operator.Number : x;
            return (Num == Result) ? 1 : 0;
        }
        public override string ShowOperator(string Result, string x, Operator Operator)
        {
            string Num = (Operator.UseNumber) ? Operator.Number.ToString() : x;
            return Num + " AND " + Result;
        }
    }
    public class OR : MathFunction
    {
        public override double Calculate(double Result, double x, Operator Operator)
        {
            double Num = (Operator.UseNumber) ? Operator.Number : x;
            return (Num != Result && (Num == 1 || Result == 1)) ? 1 : 0;
        }
        public override string ShowOperator(string Result, string x, Operator Operator)
        {
            string Num = (Operator.UseNumber) ? Operator.Number.ToString() : x;
            return Num + " OR " + Result;
        }
    }
    public class NOT : MathFunction
    {
        public override double Calculate(double Result, double x, Operator Operator)
        {
            return (Result == 0) ? 1 : 0;
        }
        public override string ShowOperator(string Result, string x, Operator Operator)
        {
            return "Not(" + Result + ")";
        }
    }
}
