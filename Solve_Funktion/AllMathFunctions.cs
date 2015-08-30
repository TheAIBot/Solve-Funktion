﻿using System;
using System.Linq;
using System.Text;
using System.Numerics;

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

        public abstract Vector<double> Calculate(Vector<double> Result, Vector<double> x, Operator Oper);
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
            string Num = (Oper.UseNumber) ? Oper.Number[0].ToString() : x;
            if (Oper.ResultOnRightSide)
            {
                Backwards.Append(ReversedMiddleFix);
                Backwards.Append(String.Concat(Num.Reverse()));
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
    public sealed class Plus : MathFunction
    {
        public Plus()
        {
            MiddleFix = " + ";
            CreateReversedStrings();
        }
        public override Vector<double> Calculate(Vector<double> Result, Vector<double> x, Operator Oper)
        {
            Vector<double> Num = (Oper.UseNumber) ? Oper.Number : x;
            return (Oper.ResultOnRightSide) ? (Num + Result) : (Result + Num);
        }
        public override void ShowOperator(string x, Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(x, Oper, Forwards, Backwards);
        }
    }
    public sealed class Subtract : MathFunction
    {
        public Subtract()
        {
            MiddleFix = " - ";
            CreateReversedStrings();
        }
        public override Vector<double> Calculate(Vector<double> Result, Vector<double> x, Operator Oper)
        {
            Vector<double> Num = (Oper.UseNumber) ? Oper.Number : x;
            return (Oper.ResultOnRightSide) ? (Num - Result) : (Result - Num);
        }
        public override void ShowOperator(string x, Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(x, Oper, Forwards, Backwards);
        }
    }
    public sealed class Multiply : MathFunction
    {
        public Multiply()
        {
            MiddleFix = " * ";
            CreateReversedStrings();
        }
        public override Vector<double> Calculate(Vector<double> Result, Vector<double> x, Operator Oper)
        {
            Vector<double> Num = (Oper.UseNumber) ? Oper.Number : x;
            return (Oper.ResultOnRightSide) ? (Num * Result) : (Result * Num);
        }
        public override void ShowOperator(string x, Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(x, Oper, Forwards, Backwards);
        }
    }
    public sealed class Divide : MathFunction
    {
        public Divide()
        {
            MiddleFix = " / ";
            CreateReversedStrings();
        }
        public override Vector<double> Calculate(Vector<double> Result, Vector<double> x, Operator Oper)
        {
            Vector<double> Num = (Oper.UseNumber) ? Oper.Number : x;
            return (Oper.ResultOnRightSide) ? (Num / Result) : (Result / Num);
        }
        public override void ShowOperator(string x, Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(x, Oper, Forwards, Backwards);
        }
    }
    //public class Modulos : MathFunction
    //{
    //    public Modulos()
    //    {
    //        MiddleFix = " % ";
    //        CreateReversedStrings();
    //    }
    //    public override Vector<double> Calculate(Vector<double> Result, Vector<double> x, Operator Oper)
    //    {
    //        Vector<double> Num = (Oper.UseNumber) ? Oper.Number : x;
    //        return (Oper.ResultOnRightSide) ? (Num % Result) : (Result % Num);
    //    }
    //    public override void ShowOperator(string x, Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
    //    {
    //        DrawOperator(x, Oper, Forwards, Backwards);
    //    }
    //}
    public sealed class PowerOf : MathFunction
    {
        public PowerOf()
        {
            MiddleFix = " ^ ";
            CreateReversedStrings();
        }
        public override Vector<double> Calculate(Vector<double> Result, Vector<double> x, Operator Oper)
        {
            Vector<double> Num = (Oper.UseNumber) ? Oper.Number : x;
            return (Oper.ResultOnRightSide) ? VectorMath.Pow(Num, Result) : VectorMath.Pow(Result, Num);
        }
        public override void ShowOperator(string x, Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(x, Oper, Forwards, Backwards);
        }
    }
    public sealed class Root : MathFunction
    {
        public Root()
        {
            IsConnecter = false;
            PreFix = "Sqrt";
            CreateReversedStrings();
        }
        public override Vector<double> Calculate(Vector<double> Result, Vector<double> x, Operator Oper)
        {
            return Vector.SquareRoot<double>(Result);
        }
        public override void ShowOperator(string x, Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(x, Oper, Forwards, Backwards);
        }
    }
    //public class Exponent : MathFunction
    //{
    //    public Exponent()
    //    {
    //        IsConnecter = false;
    //        PreFix = "Exp";
    //        CreateReversedStrings();
    //    }
    //    public override double Calculate(double Result, double x, Operator Oper)
    //    {
    //        return Math.Exp(Result);
    //    }
    //    public override void ShowOperator(string x, Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
    //    {
    //        DrawOperator(x, Oper, Forwards, Backwards);
    //    }
    //}
    //public class NaturalLog : MathFunction
    //{
    //    public NaturalLog()
    //    {
    //        IsConnecter = false;
    //        PreFix = "Ln";
    //        CreateReversedStrings();
    //    }
    //    public override double Calculate(double Result, double x, Operator Oper)
    //    {
    //        return Math.Log(Result);
    //    }
    //    public override void ShowOperator(string x, Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
    //    {
    //        DrawOperator(x, Oper, Forwards, Backwards);
    //    }
    //}
    //public class Log : MathFunction
    //{
    //    public Log()
    //    {
    //        IsConnecter = false;
    //        PreFix = "Log";
    //        CreateReversedStrings();
    //    }
    //    public override double Calculate(double Result, double x, Operator Oper)
    //    {
    //        return Math.Log10(Result);
    //    }
    //    public override void ShowOperator(string x, Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
    //    {
    //        DrawOperator(x, Oper, Forwards, Backwards);
    //    }
    //}

    //Rounders
    //public class Floor : MathFunction
    //{
    //    public Floor()
    //    {
    //        IsConnecter = false;
    //        PreFix = "Floor";
    //        CreateReversedStrings();
    //    }
    //    public override double Calculate(double Result, double x, Operator Oper)
    //    {
    //        return Math.Floor(Result);
    //    }
    //    public override void ShowOperator(string x, Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
    //    {
    //        DrawOperator(x, Oper, Forwards, Backwards);
    //    }
    //}
    //public class Ceil : MathFunction
    //{
    //    public Ceil()
    //    {
    //        IsConnecter = false;
    //        PreFix = "Ceil";
    //        CreateReversedStrings();
    //    }
    //    public override double Calculate(double Result, double x, Operator Oper)
    //    {
    //        return Math.Ceiling(Result);
    //    }
    //    public override void ShowOperator(string x, Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
    //    {
    //        DrawOperator(x, Oper, Forwards, Backwards);
    //    }
    //}
    //public class Round : MathFunction
    //{
    //    public Round()
    //    {
    //        IsConnecter = false;
    //        PreFix = "Round";
    //        CreateReversedStrings();
    //    }
    //    public override double Calculate(double Result, double x, Operator Oper)
    //    {
    //        return Math.Round(Result);
    //    }
    //    public override void ShowOperator(string x, Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
    //    {
    //        DrawOperator(x, Oper, Forwards, Backwards);
    //    }
    //}

    //Trigonomic
    //public class Sin : MathFunction
    //{
    //    public Sin()
    //    {
    //        IsConnecter = false;
    //        PreFix = "Sin";
    //        CreateReversedStrings();
    //    }
    //    public override double Calculate(double Result, double x, Operator Oper)
    //    {
    //        return Math.Sin(Result);
    //    }
    //    public override void ShowOperator(string x, Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
    //    {
    //        DrawOperator(x, Oper, Forwards, Backwards);
    //    }
    //}
    //public class Cos : MathFunction
    //{
    //    public Cos()
    //    {
    //        IsConnecter = false;
    //        PreFix = "Cos";
    //        CreateReversedStrings();
    //    }
    //    public override double Calculate(double Result, double x, Operator Oper)
    //    {
    //        return Math.Cos(Result);
    //    }
    //    public override void ShowOperator(string x, Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
    //    {
    //        DrawOperator(x, Oper, Forwards, Backwards);
    //    }
    //}
    //public class Tan : MathFunction
    //{
    //    public Tan()
    //    {
    //        IsConnecter = false;
    //        PreFix = "Tan";
    //        CreateReversedStrings();
    //    }
    //    public override double Calculate(double Result, double x, Operator Oper)
    //    {
    //        return Math.Tan(Result);
    //    }
    //    public override void ShowOperator(string x, Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
    //    {
    //        DrawOperator(x, Oper, Forwards, Backwards);
    //    }
    //}
    //public class ASin : MathFunction
    //{
    //    public ASin()
    //    {
    //        IsConnecter = false;
    //        PreFix = "ASin";
    //        CreateReversedStrings();
    //    }
    //    public override double Calculate(double Result, double x, Operator Oper)
    //    {
    //        return Math.Asin(Result);
    //    }
    //    public override void ShowOperator(string x, Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
    //    {
    //        DrawOperator(x, Oper, Forwards, Backwards);
    //    }
    //}
    //public class ACos : MathFunction
    //{
    //    public ACos()
    //    {
    //        IsConnecter = false;
    //        PreFix = "ACos";
    //        CreateReversedStrings();
    //    }
    //    public override double Calculate(double Result, double x, Operator Oper)
    //    {
    //        return Math.Acos(Result);
    //    }
    //    public override void ShowOperator(string x, Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
    //    {
    //        DrawOperator(x, Oper, Forwards, Backwards);
    //    }
    //}
    //public class ATan : MathFunction
    //{
    //    public ATan()
    //    {
    //        IsConnecter = false;
    //        PreFix = "ATan";
    //        CreateReversedStrings();
    //    }
    //    public override double Calculate(double Result, double x, Operator Oper)
    //    {
    //        return Math.Atan(Result);
    //    }
    //    public override void ShowOperator(string x, Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
    //    {
    //        DrawOperator(x, Oper, Forwards, Backwards);
    //    }
    //}

    //Misc
    public sealed class Parentheses : MathFunction
    {
        public Parentheses()
        {
            IsConnecter = false;
        }
        public override Vector<double> Calculate(Vector<double> Result, Vector<double> x, Operator Oper)
        {
            Vector<double> Res = x;
            foreach (Operator OP in Oper.Operators)
            {
                Res = OP.Calculate(Res, x);
                if (!Tools.IsANumber(Res))
                {
                    return Constants.NAN_VECTOR;
                }
            }
            return Oper.ExtraMathFunction.Calculate(Result, Res, Oper);
        }
        public override void ShowOperator(string x, Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            string currentFunction = ReverseAddStringBuilder(Backwards, Forwards);
            Forwards.Clear();
            Forwards.Append(currentFunction);
            Backwards.Clear();

            //StringBuilder PForwards = new StringBuilder();
            //StringBuilder PBackwards = new StringBuilder();
            //PForwards.Append(x);
            foreach (Operator OP in Oper.Operators)
            {
                OP.ShowOperator(x, Forwards, Backwards);
            }
            //if (Oper.ResultOnRightSide)
            //{
            //    StringBuilder Result = ReverseStringBuilder(PForwards, PBackwards.Length + PForwards.Length);
            //    Result.Append(PBackwards);
            //    Oper.ExtraMathFunction.ShowOperator(Result.ToString(), Oper, Forwards, Backwards);
            //}
            //else
            //{
            //    StringBuilder Result = ReverseStringBuilder(PBackwards, PBackwards.Length + PForwards.Length);
            //    Result.Append(PForwards);
            //    Oper.ExtraMathFunction.ShowOperator(Result.ToString(), Oper, Forwards, Backwards);
            //}
        }
        private string ReverseAddStringBuilder(StringBuilder toReverse, StringBuilder toAdd)
        {
            //reverse
            char[] toReverseAdd = new char[toReverse.Length + toAdd.Length];
            toReverse.CopyTo(0, toReverseAdd, toAdd.Length, toReverse.Length);
            Array.Reverse(toReverseAdd);

            //add
            toAdd.CopyTo(0, toReverseAdd, toReverse.Length, toAdd.Length);
            return new String(toReverseAdd);
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
                ToAdd.MakeRandom(Oper.Operators);
            }
            do
            {
                Oper.ExtraMathFunction = Oper.Eq.EInfo.Operators[SynchronizedRandom.Next(0, Oper.Eq.EInfo.Operators.Length)];
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
    public sealed class Absolute : MathFunction
    {
        public Absolute()
        {
            IsConnecter = false;
            PreFix = "|";
            PostFix = "|";
            CreateReversedStrings();
        }
        public override Vector<double> Calculate(Vector<double> Result, Vector<double> x, Operator Oper)
        {
            return Vector.Abs<double>(Result);
        }
        public override void ShowOperator(string x, Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(x, Oper, Forwards, Backwards);
        }
    }


    public class LogicBase : MathFunction
    {
        protected readonly Vector<double> _one = Vector<double>.One;
        protected readonly Vector<double> _zero = Vector<double>.Zero;

        public override Vector<double> Calculate(Vector<double> Result, Vector<double> x, Operator Oper)
        {
            throw new NotImplementedException();
        }
        public override void ShowOperator(string x, Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            throw new NotImplementedException();
        }


    }

    //Logic Operators
    public sealed class AND : LogicBase
    {
        public AND()
        {
            MiddleFix = " AND ";
            CreateReversedStrings();
        }
        public override Vector<double> Calculate(Vector<double> Result, Vector<double> x, Operator Oper)
        {
            Vector<double> Num = (Oper.UseNumber) ? Oper.Number : x;
            return (Num == _one && Result == _zero) ? _one : _zero;
        }
        public override void ShowOperator(string x, Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(x, Oper, Forwards, Backwards);
        }
    }
    public sealed class NAND : LogicBase
    {
        public NAND()
        {
            MiddleFix = " NAND ";
            CreateReversedStrings();
        }
        public override Vector<double> Calculate(Vector<double> Result, Vector<double> x, Operator Oper)
        {
            Vector<double> Num = (Oper.UseNumber) ? Oper.Number : x;
            return (Num == _one && Result == _zero) ? _zero : _one;
        }
        public override void ShowOperator(string x, Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(x, Oper, Forwards, Backwards);
        }
    }
    public sealed class OR : LogicBase
    {
        public OR()
        {
            MiddleFix = " OR ";
            CreateReversedStrings();
        }
        public override Vector<double> Calculate(Vector<double> Result, Vector<double> x, Operator Oper)
        {
            Vector<double> Num = (Oper.UseNumber) ? Oper.Number : x;
            return (Num == _one || Result == _one) ? _one : _zero;
        }
        public override void ShowOperator(string x, Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(x, Oper, Forwards, Backwards);
        }
    }
    public sealed class NOR : LogicBase
    {
        public NOR()
        {
            MiddleFix = " NOR ";
            CreateReversedStrings();
        }
        public override Vector<double> Calculate(Vector<double> Result, Vector<double> x, Operator Oper)
        {
            Vector<double> Num = (Oper.UseNumber) ? Oper.Number : x;
            return (Num == _one || Result == _one) ? _zero : _one;
        }
        public override void ShowOperator(string x, Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(x, Oper, Forwards, Backwards);
        }
    }
    public sealed class XOR : LogicBase
    {
        public XOR()
        {
            MiddleFix = " XOR ";
            CreateReversedStrings();
        }
        public override Vector<double> Calculate(Vector<double> Result, Vector<double> x, Operator Oper)
        {
            Vector<double> Num = (Oper.UseNumber) ? Oper.Number : x;
            return (Num != Result && (Num == _one || Result == _one)) ? _one : _zero;
        }
        public override void ShowOperator(string x, Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(x, Oper, Forwards, Backwards);
        }
    }
    public sealed class XNOR : LogicBase
    {
        public XNOR()
        {
            MiddleFix = " XNOR ";
            CreateReversedStrings();
        }
        public override Vector<double> Calculate(Vector<double> Result, Vector<double> x, Operator Oper)
        {
            Vector<double> Num = (Oper.UseNumber) ? Oper.Number : x;
            return (Num != Result && (Num == _one || Result == _one)) ? _zero : _one;
        }
        public override void ShowOperator(string x, Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(x, Oper, Forwards, Backwards);
        }
    }
    public sealed class NOT : LogicBase
    {
        public NOT()
        {
            IsConnecter = false;
            PreFix = " NOT ";
            CreateReversedStrings();
        }
        public override Vector<double> Calculate(Vector<double> Result, Vector<double> x, Operator Oper)
        {
            return (Result == _zero) ? _one : _zero;
        }
        public override void ShowOperator(string x, Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(x, Oper, Forwards, Backwards);
        }
    }
}