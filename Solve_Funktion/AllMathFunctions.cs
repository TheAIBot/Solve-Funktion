using System;
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

        public abstract Vector<double> Calculate(Vector<double> Result, Vector<double>[] parameters, Operator Oper);
        public abstract void ShowOperator(Operator Oper, StringBuilder Forwards, StringBuilder Backwards);

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

        protected void DrawOperator(Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            if (IsConnecter)
                DrawConnector(Oper, Forwards, Backwards);
            else
                DrawSingle(Forwards, Backwards);
        }
        protected void CreateReversedStrings()
        {
            ReversedPreFix = RevertString(PreFix);
            ReversedMiddleFix = RevertString(MiddleFix);
        }
        private void DrawConnector(Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            string Num = (Oper.UseRandomNumber) ? Oper.randomNumber[0].ToString() : Oper.Eq.EInfo.Goal[0].ParameterNames[Oper.parameterIndex];
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

    public interface IConnecter
    {
        Vector<double> CalculateConnector(Vector<double> Left, Vector<double> Right, Operator Oper);

        void ShowOperator(Operator Oper, StringBuilder Forwards, StringBuilder Backwards);
    }

    //Standard
    public sealed class Plus : MathFunction, IConnecter
    {
        public Plus()
        {
            MiddleFix = " + ";
            CreateReversedStrings();
        }
        public override Vector<double> Calculate(Vector<double> Result, Vector<double>[] parameters, Operator Oper)
        {
            Vector<double> Num = (Oper.UseRandomNumber) ? Oper.randomNumber : parameters[Oper.parameterIndex];
            return (Oper.ResultOnRightSide) ? (Num + Result) : (Result + Num);
        }
        public override void ShowOperator(Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(Oper, Forwards, Backwards);
        }
        public Vector<double> CalculateConnector(Vector<double> Left, Vector<double> Right, Operator Oper)
        {
            return (Oper.ResultOnRightSide) ? (Left + Right) : (Right + Left);
        }
    }
    public sealed class Subtract : MathFunction, IConnecter
    {
        public Subtract()
        {
            MiddleFix = " - ";
            CreateReversedStrings();
        }
        public override Vector<double> Calculate(Vector<double> Result, Vector<double>[] parameters, Operator Oper)
        {
            Vector<double> Num = (Oper.UseRandomNumber) ? Oper.randomNumber : parameters[Oper.parameterIndex];
            return (Oper.ResultOnRightSide) ? (Num - Result) : (Result - Num);
        }
        public override void ShowOperator(Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(Oper, Forwards, Backwards);
        }
        public Vector<double> CalculateConnector(Vector<double> Left, Vector<double> Right, Operator Oper)
        {
            return (!Oper.ResultOnRightSide) ? (Left - Right) : (Right - Left);
        }
    }
    public sealed class Multiply : MathFunction, IConnecter
    {
        public Multiply()
        {
            MiddleFix = " * ";
            CreateReversedStrings();
        }
        public override Vector<double> Calculate(Vector<double> Result, Vector<double>[] parameters, Operator Oper)
        {
            Vector<double> Num = (Oper.UseRandomNumber) ? Oper.randomNumber : parameters[Oper.parameterIndex];
            return (Oper.ResultOnRightSide) ? (Num * Result) : (Result * Num);
        }
        public override void ShowOperator(Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(Oper, Forwards, Backwards);
        }
        public Vector<double> CalculateConnector(Vector<double> Left, Vector<double> Right, Operator Oper)
        {
            return (Oper.ResultOnRightSide) ? (Left * Right) : (Right * Left);
        }
    }
    public sealed class Divide : MathFunction, IConnecter
    {
        public Divide()
        {
            MiddleFix = " / ";
            CreateReversedStrings();
        }
        public override Vector<double> Calculate(Vector<double> Result, Vector<double>[] parameters, Operator Oper)
        {
            Vector<double> Num = (Oper.UseRandomNumber) ? Oper.randomNumber : parameters[Oper.parameterIndex];
            return (Oper.ResultOnRightSide) ? (Num / Result) : (Result / Num);
        }
        public override void ShowOperator(Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(Oper, Forwards, Backwards);
        }
        public Vector<double> CalculateConnector(Vector<double> Left, Vector<double> Right, Operator Oper)
        {
            return (!Oper.ResultOnRightSide) ? (Left / Right) : (Right / Left);
        }
    }
    public class Modulos : MathFunction, IConnecter
    {
        public Modulos()
        {
            MiddleFix = " % ";
            CreateReversedStrings();
        }
        public override Vector<double> Calculate(Vector<double> Result, Vector<double>[] parameters, Operator Oper)
        {
            Vector<double> Num = (Oper.UseRandomNumber) ? Oper.randomNumber : parameters[Oper.parameterIndex];
            return (Oper.ResultOnRightSide) ? (ShittyVectorMath.Modulus(Num, Result)) : (ShittyVectorMath.Modulus(Result, Num));
        }
        public override void ShowOperator(Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(Oper, Forwards, Backwards);
        }
        public Vector<double> CalculateConnector(Vector<double> Left, Vector<double> Right, Operator Oper)
        {
            return (Oper.ResultOnRightSide) ? (ShittyVectorMath.Modulus(Left, Right)) : (ShittyVectorMath.Modulus(Right, Left));
        }
    }
    public sealed class PowerOf : MathFunction, IConnecter
    {
        public PowerOf()
        {
            MiddleFix = " ^ ";
            CreateReversedStrings();
        }
        public override Vector<double> Calculate(Vector<double> Result, Vector<double>[] parameters, Operator Oper)
        {
            Vector<double> Num = (Oper.UseRandomNumber) ? Oper.randomNumber : parameters[Oper.parameterIndex];
            return (Oper.ResultOnRightSide) ? ShittyVectorMath.Pow(Num, Result) : ShittyVectorMath.Pow(Result, Num);
        }
        public override void ShowOperator(Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(Oper, Forwards, Backwards);
        }
        public Vector<double> CalculateConnector(Vector<double> Left, Vector<double> Right, Operator Oper)
        {
            return (Oper.ResultOnRightSide) ? (ShittyVectorMath.Pow(Left, Right)) : (ShittyVectorMath.Pow(Right, Left));
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
        public override Vector<double> Calculate(Vector<double> Result, Vector<double>[] parameters, Operator Oper)
        {
            return Vector.SquareRoot<double>(Result);
        }
        public override void ShowOperator(Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(Oper, Forwards, Backwards);
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
        public override Vector<double> Calculate(Vector<double> Result, Vector<double>[] parameters, Operator Oper)
        {
            return ShittyVectorMath.Exp(Result);
        }
        public override void ShowOperator(Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(Oper, Forwards, Backwards);
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
        public override Vector<double> Calculate(Vector<double> Result, Vector<double>[] parameters, Operator Oper)
        {
            return ShittyVectorMath.Log(Result);
        }
        public override void ShowOperator(Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(Oper, Forwards, Backwards);
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
        public override Vector<double> Calculate(Vector<double> Result, Vector<double>[] parameters, Operator Oper)
        {
            return ShittyVectorMath.Log10(Result);
        }
        public override void ShowOperator(Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(Oper, Forwards, Backwards);
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
        public override Vector<double> Calculate(Vector<double> Result, Vector<double>[] parameters, Operator Oper)
        {
            return ShittyVectorMath.Floor(Result);
        }
        public override void ShowOperator(Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(Oper, Forwards, Backwards);
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
        public override Vector<double> Calculate(Vector<double> Result, Vector<double>[] parameters, Operator Oper)
        {
            return ShittyVectorMath.Ceiling(Result);
        }
        public override void ShowOperator(Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(Oper, Forwards, Backwards);
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
        public override Vector<double> Calculate(Vector<double> Result, Vector<double>[] parameters, Operator Oper)
        {
            return ShittyVectorMath.Round(Result);
        }
        public override void ShowOperator(Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(Oper, Forwards, Backwards);
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
        public override Vector<double> Calculate(Vector<double> Result, Vector<double>[] parameters, Operator Oper)
        {
            return ShittyVectorMath.Sin(Result);
        }
        public override void ShowOperator(Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(Oper, Forwards, Backwards);
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
        public override Vector<double> Calculate(Vector<double> Result, Vector<double>[] parameters, Operator Oper)
        {
            return ShittyVectorMath.Cos(Result);
        }
        public override void ShowOperator(Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(Oper, Forwards, Backwards);
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
        public override Vector<double> Calculate(Vector<double> Result, Vector<double>[] parameters, Operator Oper)
        {
            return ShittyVectorMath.Tan(Result);
        }
        public override void ShowOperator(Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(Oper, Forwards, Backwards);
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
        public override Vector<double> Calculate(Vector<double> Result, Vector<double>[] parameters, Operator Oper)
        {
            return ShittyVectorMath.Asin(Result);
        }
        public override void ShowOperator(Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(Oper, Forwards, Backwards);
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
        public override Vector<double> Calculate(Vector<double> Result, Vector<double>[] parameters, Operator Oper)
        {
            return ShittyVectorMath.Acos(Result);
        }
        public override void ShowOperator(Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(Oper, Forwards, Backwards);
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
        public override Vector<double> Calculate(Vector<double> Result, Vector<double>[] parameters, Operator Oper)
        {
            return ShittyVectorMath.Atan(Result);
        }
        public override void ShowOperator(Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(Oper, Forwards, Backwards);
        }
    }

    //Misc
    public sealed class Parentheses : MathFunction
    {
        public Parentheses()
        {
            IsConnecter = false;
        }
        public override Vector<double> Calculate(Vector<double> Result, Vector<double>[] parameters, Operator Oper)
        {
            Vector<double> Res = parameters[Oper.parameterIndex];
            foreach (Operator OP in Oper.Operators)
            {
                Res = OP.Calculate(Res, parameters);
                if (!Tools.IsANumber(Res))
                {
                    return Constants.NAN_VECTOR;
                }
            }
            return Oper.ExtraMathFunction.CalculateConnector(Res, Result, Oper);
        }

        public override void ShowOperator(Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            string currentFunction = ReverseAddStringBuilder(Backwards, Forwards);
            Forwards.Clear();
            Forwards.Append(currentFunction);
            Backwards.Clear();

            foreach (Operator OP in Oper.Operators)
            {
                OP.ShowOperator(Forwards, Backwards);
            }
            Oper.ExtraMathFunction.ShowOperator(Oper, Forwards, Backwards);
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
            Oper.UseRandomNumber = false;
            Oper.parameterIndex = 0;

            //the method CanUseOperator makes sure there is atleast 1 available Operator to use in the parentheses
            int AmountToAdd = SynchronizedRandom.Next(0, Oper.Eq.OperatorsLeft - 1);

            while (0 < Oper.Eq.OperatorsLeft - AmountToAdd)
            {
                Operator ToAdd = Oper.Eq.OPStorage.Pop();
                // by adding the operator now there is space for 1 less operator
                ToAdd.MakeRandom(Oper.Operators);
            }
            Oper.ExtraMathFunction = Oper.Eq.EInfo.Connectors[SynchronizedRandom.Next(0, Oper.Eq.EInfo.Connectors.Length)];
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
        public override Vector<double> Calculate(Vector<double> Result, Vector<double>[] parameters, Operator Oper)
        {
            return Vector.Abs<double>(Result);
        }
        public override void ShowOperator(Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(Oper, Forwards, Backwards);
        }
    }

    //Logic Operators
    public class LogicBase : MathFunction
    {
        protected readonly Vector<double> _one = Vector<double>.One;
        protected readonly Vector<double> _zero = Vector<double>.Zero;

        public override Vector<double> Calculate(Vector<double> Result, Vector<double>[] parameters, Operator Oper)
        {
            throw new NotImplementedException();
        }
        public override void ShowOperator(Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            throw new NotImplementedException();
        }


    }

    //Logic Operators
    public sealed class AND : LogicBase, IConnecter
    {
        public AND()
        {
            MiddleFix = " AND ";
            CreateReversedStrings();
        }
        public override Vector<double> Calculate(Vector<double> Result, Vector<double>[] parameters, Operator Oper)
        {
            Vector<double> Num = (Oper.UseRandomNumber) ? Oper.randomNumber : parameters[Oper.parameterIndex];
            return ShittyVectorLogic.AND(Num, Result);
        }
        public override void ShowOperator(Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(Oper, Forwards, Backwards);
        }

        public Vector<double> CalculateConnector(Vector<double> Left, Vector<double> Right, Operator Oper)
        {
            return ShittyVectorLogic.AND(Left, Right);
        }
    }
    public sealed class NAND : LogicBase, IConnecter
    {
        public NAND()
        {
            MiddleFix = " NAND ";
            CreateReversedStrings();
        }
        public override Vector<double> Calculate(Vector<double> Result, Vector<double>[] parameters, Operator Oper)
        {
            Vector<double> Num = (Oper.UseRandomNumber) ? Oper.randomNumber : parameters[Oper.parameterIndex];
            return ShittyVectorLogic.NAND(Num, Result);
        }
        public override void ShowOperator(Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(Oper, Forwards, Backwards);
        }

        public Vector<double> CalculateConnector(Vector<double> Left, Vector<double> Right, Operator Oper)
        {
            return ShittyVectorLogic.NAND(Left, Right);
        }
    }
    public sealed class OR : LogicBase, IConnecter
    {
        public OR()
        {
            MiddleFix = " OR ";
            CreateReversedStrings();
        }
        public override Vector<double> Calculate(Vector<double> Result, Vector<double>[] parameters, Operator Oper)
        {
            Vector<double> Num = (Oper.UseRandomNumber) ? Oper.randomNumber : parameters[Oper.parameterIndex];
            return ShittyVectorLogic.OR(Num, Result);
        }
        public override void ShowOperator(Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(Oper, Forwards, Backwards);
        }

        public Vector<double> CalculateConnector(Vector<double> Left, Vector<double> Right, Operator Oper)
        {
            return ShittyVectorLogic.OR(Left, Right);
        }
    }
    public sealed class NOR : LogicBase, IConnecter
    {
        public NOR()
        {
            MiddleFix = " NOR ";
            CreateReversedStrings();
        }
        public override Vector<double> Calculate(Vector<double> Result, Vector<double>[] parameters, Operator Oper)
        {
            Vector<double> Num = (Oper.UseRandomNumber) ? Oper.randomNumber : parameters[Oper.parameterIndex];
            return ShittyVectorLogic.NOR(Num, Result);
        }
        public override void ShowOperator(Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(Oper, Forwards, Backwards);
        }

        public Vector<double> CalculateConnector(Vector<double> Left, Vector<double> Right, Operator Oper)
        {
            return ShittyVectorLogic.NOR(Left, Right);
        }
    }
    public sealed class XOR : LogicBase, IConnecter
    {
        public XOR()
        {
            MiddleFix = " XOR ";
            CreateReversedStrings();
        }
        public override Vector<double> Calculate(Vector<double> Result, Vector<double>[] parameters, Operator Oper)
        {
            Vector<double> Num = (Oper.UseRandomNumber) ? Oper.randomNumber : parameters[Oper.parameterIndex];
            return ShittyVectorLogic.XOR(Num, Result);
        }
        public override void ShowOperator(Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(Oper, Forwards, Backwards);
        }

        public Vector<double> CalculateConnector(Vector<double> Left, Vector<double> Right, Operator Oper)
        {
            return ShittyVectorLogic.XOR(Left, Right);
        }
    }
    public sealed class XNOR : LogicBase, IConnecter
    {
        public XNOR()
        {
            MiddleFix = " XNOR ";
            CreateReversedStrings();
        }
        public override Vector<double> Calculate(Vector<double> Result, Vector<double>[] parameters, Operator Oper)
        {
            Vector<double> Num = (Oper.UseRandomNumber) ? Oper.randomNumber : parameters[Oper.parameterIndex];
            return ShittyVectorLogic.XNOR(Num, Result);
        }
        public override void ShowOperator(Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(Oper, Forwards, Backwards);
        }

        public Vector<double> CalculateConnector(Vector<double> Left, Vector<double> Right, Operator Oper)
        {
            return ShittyVectorLogic.XNOR(Left, Right);
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
        public override Vector<double> Calculate(Vector<double> Result, Vector<double>[] parameters, Operator Oper)
        {
            return ShittyVectorLogic.NOT(Result);
        }
        public override void ShowOperator(Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(Oper, Forwards, Backwards);
        }
    }
}