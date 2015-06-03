using System;
using System.Linq;
using System.Text;

namespace Solve_Funktion
{
    [Serializable]
    public abstract class MathFunction
    {
        public bool IsConnecter = true;
        protected string PreFix = String.Empty;
        protected string MiddleFix = String.Empty;
        protected string PostFix = String.Empty;
        private string ReversedPreFix;
        private string ReversedMiddleFix;

        public abstract double Calculate(double Result, double x, Operator Oper);
        public abstract void ShowOperator(string x, Operator Oper, StringBuilder Forwards, StringBuilder Backwards);

        public virtual void MakeRandom(Operator Oper)
        {

        }
        public virtual bool CanUseOperator(Operator Oper)
        {
            return true;
        }
        public virtual int GetOperatorCount(Operator Oper)
        {
            return 1;
        }

        public virtual void GetCopy(Operator Original, Operator Copy)
        {

        }
        public virtual void StoreAndCleanup(Operator Oper)
        {

        }
        public virtual void StoreAndCleanupAll(Operator Oper)
        {

        }

        protected void DrawOperator(string x, Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            if (IsConnecter)
                DrawConnector(x, Oper, Forwards, Backwards);
            else
                DrawSingle(Forwards, Backwards);
        }
        protected void CreateReversedStrings()
        {
            ReversedPreFix = RevertString(PreFix);
            ReversedMiddleFix = RevertString(MiddleFix);
        }
        private void DrawConnector(string x, Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            //if IsResultOnRight is true
            //example (2 + Result)
            //Backward.Append(MiddleFix);
            //Backward.Append(Num);
            //Backward.Append("(");
            //Backward.Append(PreFix);    
            //Forward.Append(")");
            //Forward.Append(PostFix);
            //else (Result + 2)
            //Backward.Append("(");
            //Backward.Append(PreFix);
            //Forward.Append(MiddleFix);
            //Forward.Append(Num);
            //Forward.Append(")");
            //Forward.Append(PostFix);
            string Num = (Oper.UseNumber) ? Oper.Number.ToString() : x;
            if (Oper.ResultOnRightSide)
            {
                Backwards.Append(ReversedMiddleFix);
                Backwards.Append(Num);
                Backwards.Append("(");
                Backwards.Append(ReversedPreFix);
                Forwards.Append(")");
                Forwards.Append(PostFix);
            }
            else
            {
                Backwards.Append("(");
                Backwards.Append(ReversedPreFix);
                Forwards.Append(MiddleFix);
                Forwards.Append(Num);
                Forwards.Append(")");
                Forwards.Append(PostFix);
            }
        }
        private void DrawSingle(StringBuilder Forwards, StringBuilder Backwards)
        {
            Backwards.Append("(");
            Backwards.Append(ReversedPreFix);
            Forwards.Append(")");
            Forwards.Append(PostFix);
        }
        private string RevertString(string x)
        {
            char[] Text = x.ToArray();
            Array.Reverse(Text);
            return String.Join(String.Empty, Text);
        }
    }

    //Standard
    public class Plus : MathFunction
    {
        public Plus()
        {
            MiddleFix = " + ";
            CreateReversedStrings();
        }
        public override double Calculate(double Result, double x, Operator Oper)
        {
            double Num = (Oper.UseNumber) ? Oper.Number : x;
            return (Oper.ResultOnRightSide) ? (Num + Result) : (Result + Num);
        }
        public override void ShowOperator(string x, Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(x, Oper, Forwards, Backwards);
        }
    }
    public class Subtract : MathFunction
    {
        public Subtract()
        {
            MiddleFix = " - ";
            CreateReversedStrings();
        }
        public override double Calculate(double Result, double x, Operator Oper)
        {
            double Num = (Oper.UseNumber) ? Oper.Number : x;
            return (Oper.ResultOnRightSide) ? (Num - Result) : (Result - Num);
        }
        public override void ShowOperator(string x, Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(x, Oper, Forwards, Backwards);
        }
    }
    public class Multiply : MathFunction
    {
        public Multiply()
        {
            MiddleFix = " * ";
            CreateReversedStrings();
        }
        public override double Calculate(double Result, double x, Operator Oper)
        {
            double Num = (Oper.UseNumber) ? Oper.Number : x;
            return (Oper.ResultOnRightSide) ? (Num * Result) : (Result * Num);
        }
        public override void ShowOperator(string x, Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(x, Oper, Forwards, Backwards);
        }
    }
    public class Divide : MathFunction
    {
        public Divide()
        {
            MiddleFix = " / ";
            CreateReversedStrings();
        }
        public override double Calculate(double Result, double x, Operator Oper)
        {
            double Num = (Oper.UseNumber) ? Oper.Number : x;
            return (Oper.ResultOnRightSide) ? (Num / Result) : (Result / Num);
        }
        public override void ShowOperator(string x, Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(x, Oper, Forwards, Backwards);
        }
    }
    public class Modulos : MathFunction
    {
        public Modulos()
        {
            MiddleFix = " % ";
            CreateReversedStrings();
        }
        public override double Calculate(double Result, double x, Operator Oper)
        {
            double Num = (Oper.UseNumber) ? Oper.Number : x;
            return (Oper.ResultOnRightSide) ? (Num % Result) : (Result % Num);
        }
        public override void ShowOperator(string x, Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(x, Oper, Forwards, Backwards);
        }
    }
    public class PowerOf : MathFunction
    {
        public PowerOf()
        {
            MiddleFix = " ^ ";
            CreateReversedStrings();
        }
        public override double Calculate(double Result, double x, Operator Oper)
        {
            double Num = (Oper.UseNumber) ? Oper.Number : x;
            return (Oper.ResultOnRightSide) ? Math.Pow(Num, Result) : Math.Pow(Result, Num);
        }
        public override void ShowOperator(string x, Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(x, Oper, Forwards, Backwards);
        }
    }
    public class Root : MathFunction
    {
        public Root()
        {
            IsConnecter = false;
            PreFix = "Sqrt";
            CreateReversedStrings();
        }
        public override double Calculate(double Result, double x, Operator Oper)
        {
            return Math.Sqrt(Result);
        }
        public override void ShowOperator(string x, Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(x, Oper, Forwards, Backwards);
        }
    }
    public class Exponent : MathFunction
    {
        public Exponent()
        {
            IsConnecter = false;
            PreFix = "Exp";
            CreateReversedStrings();
        }
        public override double Calculate(double Result, double x, Operator Oper)
        {
            return Math.Exp(Result);
        }
        public override void ShowOperator(string x, Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(x, Oper, Forwards, Backwards);
        }
    }
    public class NaturalLog : MathFunction
    {
        public NaturalLog()
        {
            IsConnecter = false;
            PreFix = "Ln";
            CreateReversedStrings();
        }
        public override double Calculate(double Result, double x, Operator Oper)
        {
            return Math.Log(Result);
        }
        public override void ShowOperator(string x, Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(x, Oper, Forwards, Backwards);
        }
    }
    public class Log : MathFunction
    {
        public Log()
        {
            IsConnecter = false;
            PreFix = "Log";
            CreateReversedStrings();
        }
        public override double Calculate(double Result, double x, Operator Oper)
        {
            return Math.Log10(Result);
        }
        public override void ShowOperator(string x, Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(x, Oper, Forwards, Backwards);
        }
    }

    //Rounders
    public class Floor : MathFunction
    {
        public Floor()
        {
            IsConnecter = false;
            PreFix = "Floor";
            CreateReversedStrings();
        }
        public override double Calculate(double Result, double x, Operator Oper)
        {
            return Math.Floor(Result);
        }
        public override void ShowOperator(string x, Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(x, Oper, Forwards, Backwards);
        }
    }
    public class Ceil : MathFunction
    {
        public Ceil()
        {
            IsConnecter = false;
            PreFix = "Ceil";
            CreateReversedStrings();
        }
        public override double Calculate(double Result, double x, Operator Oper)
        {
            return Math.Ceiling(Result);
        }
        public override void ShowOperator(string x, Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(x, Oper, Forwards, Backwards);
        }
    }
    public class Round : MathFunction
    {
        public Round()
        {
            IsConnecter = false;
            PreFix = "Round";
            CreateReversedStrings();
        }
        public override double Calculate(double Result, double x, Operator Oper)
        {
            return Math.Round(Result);
        }
        public override void ShowOperator(string x, Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(x, Oper, Forwards, Backwards);
        }
    }

    //Trigonomic
    public class Sin : MathFunction
    {
        public Sin()
        {
            IsConnecter = false;
            PreFix = "Sin";
            CreateReversedStrings();
        }
        public override double Calculate(double Result, double x, Operator Oper)
        {
            return Math.Sin(Result);
        }
        public override void ShowOperator(string x, Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(x, Oper, Forwards, Backwards);
        }
    }
    public class Cos : MathFunction
    {
        public Cos()
        {
            IsConnecter = false;
            PreFix = "Cos";
            CreateReversedStrings();
        }
        public override double Calculate(double Result, double x, Operator Oper)
        {
            return Math.Cos(Result);
        }
        public override void ShowOperator(string x, Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(x, Oper, Forwards, Backwards);
        }
    }
    public class Tan : MathFunction
    {
        public Tan()
        {
            IsConnecter = false;
            PreFix = "Tan";
            CreateReversedStrings();
        }
        public override double Calculate(double Result, double x, Operator Oper)
        {
            return Math.Tan(Result);
        }
        public override void ShowOperator(string x, Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(x, Oper, Forwards, Backwards);
        }
    }
    public class ASin : MathFunction
    {
        public ASin()
        {
            IsConnecter = false;
            PreFix = "ASin";
            CreateReversedStrings();
        }
        public override double Calculate(double Result, double x, Operator Oper)
        {
            return Math.Asin(Result);
        }
        public override void ShowOperator(string x, Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(x, Oper, Forwards, Backwards);
        }
    }
    public class ACos : MathFunction
    {
        public ACos()
        {
            IsConnecter = false;
            PreFix = "ACos";
            CreateReversedStrings();
        }
        public override double Calculate(double Result, double x, Operator Oper)
        {
            return Math.Acos(Result);
        }
        public override void ShowOperator(string x, Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(x, Oper, Forwards, Backwards);
        }
    }
    public class ATan : MathFunction
    {
        public ATan()
        {
            IsConnecter = false;
            PreFix = "ATan";
            CreateReversedStrings();
        }
        public override double Calculate(double Result, double x, Operator Oper)
        {
            return Math.Atan(Result);
        }
        public override void ShowOperator(string x, Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(x, Oper, Forwards, Backwards);
        }
    }

    //Misc
    public class Parentheses : MathFunction
    {
        public Parentheses()
        {
            IsConnecter = false;
        }
        public override double Calculate(double Result, double x, Operator Oper)
        {
            double Res = x;
            foreach (Operator OP in Oper.Operators)
            {
                Res = OP.Calculate(Res, x);
                if (!Tools.IsANumber(Res))
                {
                    return double.NaN;
                }
            }
            return Oper.ExtraMathFunction.Calculate(Result, Res, Oper);
        }
        public override void ShowOperator(string x, Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            StringBuilder PForwards = new StringBuilder();
            StringBuilder PBackwards = new StringBuilder();
            PForwards.Append(x);
            foreach (Operator OP in Oper.Operators)
            {
                OP.ShowOperator(x, PForwards, PBackwards);
            }
            StringBuilder Result = new StringBuilder(PBackwards.Length + PForwards.Length);
            for (int i = PBackwards.Length - 1; i >= 0; i--)
            {
                Result.Append(PBackwards[i]);
            }
            Result.Append(PForwards);
            //////////StringBuilder Forwards = new StringBuilder();
            //////////StringBuilder Backwards = new StringBuilder();
            //////////const string Variable = "x";
            //////////Forwards.Append(Variable);
            //////////foreach (Operator EquationPart in EquationParts)
            //////////{
            //////////    EquationPart.ShowOperator(Variable, Forwards, Backwards);
            //////////}
            //////////StringBuilder Result = new StringBuilder(Backwards.Length + Forwards.Length);
            //////////for (int i = Backwards.Length - 1; i >= 0; i--)
            //////////{
            //////////    Result.Append(Backwards[i]);
            //////////}
            //////////Result.Append(Forwards.ToString());
            //////////return Result.ToString();
            //if (Oper.ResultOnRightSide)
            //{
            //    Result.Append(PForwards);
            //    for (int i = PBackwards.Length - 1; i >= 0; i--)
            //    {
            //        Result.Append(PBackwards[i]);
            //    }
            //    Result.Append(PForwards);
            //    Oper.ExtraMathFunction.ShowOperator(Result.ToString(), Oper, Forwards, Backwards);
            //}
            //else
            //{
            //    Result.Append(PBackwards);
            //    for (int i = PForwards.Length - 1; i >= 0; i--)
            //    {
            //        Result.Append(PForwards[i]);
            //    }
            //    Result.Append(PBackwards);
            //    Oper.ExtraMathFunction.ShowOperator(Result.ToString(), Oper, Forwards, Backwards);
            //}
        }
        private StringBuilder ReverseStringBuilder(StringBuilder ToReverse)
        {
            StringBuilder Result = new StringBuilder(ToReverse.Length);
            for (int i = ToReverse.Length - 1; i >= 0; i--)
            {
                Result.Append(ToReverse[i]);
            }
            return Result;
        }

        public override void MakeRandom(Operator Oper)
        {
            Oper.Eq.SortedOperators.Add(Oper.Operators);
            Oper.UseNumber = false;
            //the method CanUseOperator makes sure there is atleast 1 available Operator to use in the parentheses
            int AmountToAdd = SynchronizedRandom.Next(0, Oper.Eq.OperatorsLeft - 1);

            while (0 < Oper.Eq.OperatorsLeft - AmountToAdd)
            {
                Operator ToAdd = Oper.Eq.OPStorage.Pop();
                // by adding the operator now there is space for 1 less operator
                ToAdd.MakeRandom(Oper.Eq, Oper.Operators);
            }
            do
            {
                Oper.ExtraMathFunction = Info.Operators[SynchronizedRandom.Next(0, Info.Operators.Count)];
            } while (!Oper.ExtraMathFunction.IsConnecter);
        }
        public override bool CanUseOperator(Operator Oper)
        {
            return Oper.Eq.OperatorsLeft > 0;
        }
        public override int GetOperatorCount(Operator Oper)
        {
            // the parentheses is an operator in itself so the amount starts as 1
            // so the parentheses opearator is accounted for
            int Amount = 1;
            foreach (Operator OP in Oper.Operators)
            {
                Amount += OP.GetOperatorCount();
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
        public override void StoreAndCleanup(Operator Oper)
        {
            Oper.Eq.SortedOperators.Remove(Oper.Operators);
            Oper.ExtraMathFunction = null;
            // the Operators list is being altered in this loop so it can't be a foreach loop
            // that's why it's done this way
            while (Oper.Operators.Count > 0)
            {
                Oper.Operators.First().StoreAndCleanup();
            }
        }
        public override void StoreAndCleanupAll(Operator Oper)
        {
            foreach (Operator OP in Oper.Operators)
            {
                OP.StoreAndCleaupAll();
            }
            Oper.Operators.Clear();
            Oper.ExtraMathFunction = null;
        }
    }
    public class Absolute : MathFunction
    {
        public Absolute()
        {
            IsConnecter = false;
            PreFix = "|";
            PostFix = "|";
            CreateReversedStrings();
        }
        public override double Calculate(double Result, double x, Operator Oper)
        {
            return Math.Abs(Result);
        }
        public override void ShowOperator(string x, Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(x, Oper, Forwards, Backwards);
        }
    }

    //Logic Operators
    public class AND : MathFunction
    {
        public AND()
        {
            MiddleFix = " AND ";
            CreateReversedStrings();
        }
        public override double Calculate(double Result, double x, Operator Oper)
        {
            double Num = (Oper.UseNumber) ? Oper.Number : x;
            return (Num == 1 && Result == 1) ? 1 : 0;
        }
        public override void ShowOperator(string x, Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(x, Oper, Forwards, Backwards);
        }
    }
    public class NAND : MathFunction
    {
        public NAND()
        {
            MiddleFix = " NAND ";
            CreateReversedStrings();
        }
        public override double Calculate(double Result, double x, Operator Oper)
        {
            double Num = (Oper.UseNumber) ? Oper.Number : x;
            return (Num == 1 && Result == 1) ? 0 : 1;
        }
        public override void ShowOperator(string x, Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(x, Oper, Forwards, Backwards);
        }
    }
    public class OR : MathFunction
    {
        public OR()
        {
            MiddleFix = " OR ";
            CreateReversedStrings();
        }
        public override double Calculate(double Result, double x, Operator Oper)
        {
            double Num = (Oper.UseNumber) ? Oper.Number : x;
            return (Num == 1 || Result == 1) ? 1 : 0;
        }
        public override void ShowOperator(string x, Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(x, Oper, Forwards, Backwards);
        }
    }
    public class NOR : MathFunction
    {
        public NOR()
        {
            MiddleFix = " NOR ";
            CreateReversedStrings();
        }
        public override double Calculate(double Result, double x, Operator Oper)
        {
            double Num = (Oper.UseNumber) ? Oper.Number : x;
            return (Num == 1 || Result == 1) ? 0 : 1;
        }
        public override void ShowOperator(string x, Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(x, Oper, Forwards, Backwards);
        }
    }
    public class XOR : MathFunction
    {
        public XOR()
        {
            MiddleFix = " XOR ";
            CreateReversedStrings();
        }
        public override double Calculate(double Result, double x, Operator Oper)
        {
            double Num = (Oper.UseNumber) ? Oper.Number : x;
            return (Num != Result && (Num == 1 || Result == 1)) ? 1 : 0;
        }
        public override void ShowOperator(string x, Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(x, Oper, Forwards, Backwards);
        }
    }
    public class XNOR : MathFunction
    {
        public XNOR()
        {
            MiddleFix = " XNOR ";
            CreateReversedStrings();
        }
        public override double Calculate(double Result, double x, Operator Oper)
        {
            double Num = (Oper.UseNumber) ? Oper.Number : x;
            return (Num != Result && (Num == 1 || Result == 1)) ? 0 : 1;
        }
        public override void ShowOperator(string x, Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(x, Oper, Forwards, Backwards);
        }
    }
    public class NOT : MathFunction
    {
        public NOT()
        {
            IsConnecter = false;
            PreFix = " NOT ";
            CreateReversedStrings();
        }
        public override double Calculate(double Result, double x, Operator Oper)
        {
            return (Result == 0) ? 1 : 0;
        }
        public override void ShowOperator(string x, Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(x, Oper, Forwards, Backwards);
        }
    }
}