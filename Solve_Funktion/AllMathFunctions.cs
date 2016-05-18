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

        public abstract Vector<double> Calculate(Vector<double> Result, Vector<double>[] parameters, Operator Oper, int Index);
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
        public virtual void OperatorChanged(Operator Oper)
        {
            Oper.Holder.OperatorChanged();
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
            string Num = (Oper.UseRandomNumber) ? Oper.RandomNumber[0].ToString() : Oper.Eq.EInfo.Goal[0].ParameterNames[Oper.ParameterIndex];
            DrawConnector(Oper, Forwards, Backwards, Num);
        }

        protected void DrawConnector(Operator Oper, StringBuilder Forwards, StringBuilder Backwards, string ToInsert)
        {
            if (Oper.ResultOnRightSide)
            {
                Backwards.Append(ReversedMiddleFix);
                Backwards.Append(RevertString(ToInsert));
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
                Forwards.Append(ToInsert);
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

    public abstract class Connector : MathFunction
    {
        public abstract Vector<double> CalculateConnector(Vector<double> Left, Vector<double> Right, Operator Oper);

        public string ShowConnector(Operator Oper, string Left, string Right)
        {
            return (Oper.ResultOnRightSide) ? ("(" + Left + MiddleFix + Right + ")") : ("(" + Right + MiddleFix + Left + ")");
        }
    }

    //Standard
    public sealed class Plus : Connector
    {
        public Plus()
        {
            MiddleFix = " + ";
            CreateReversedStrings();
        }
        public override Vector<double> Calculate(Vector<double> Result, Vector<double>[] parameters, Operator Oper, int Index)
        {
            Vector<double> Num = (Oper.UseRandomNumber) ? Oper.RandomNumber : parameters[Oper.ParameterIndex];
            return (Oper.ResultOnRightSide) ? (Num + Result) : (Result + Num);
        }
        public override void ShowOperator(Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(Oper, Forwards, Backwards);
        }
        public override Vector<double> CalculateConnector(Vector<double> Left, Vector<double> Right, Operator Oper)
        {
            return (Oper.ResultOnRightSide) ? (Left + Right) : (Right + Left);
        }
    }
    public sealed class Subtract : Connector
    {
        public Subtract()
        {
            MiddleFix = " - ";
            CreateReversedStrings();
        }
        public override Vector<double> Calculate(Vector<double> Result, Vector<double>[] parameters, Operator Oper, int Index)
        {
            Vector<double> Num = (Oper.UseRandomNumber) ? Oper.RandomNumber : parameters[Oper.ParameterIndex];
            return (Oper.ResultOnRightSide) ? (Num - Result) : (Result - Num);
        }
        public override void ShowOperator(Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(Oper, Forwards, Backwards);
        }
        public override Vector<double> CalculateConnector(Vector<double> Left, Vector<double> Right, Operator Oper)
        {
            return (!Oper.ResultOnRightSide) ? (Left - Right) : (Right - Left);
        }
    }
    public sealed class Multiply : Connector
    {
        public Multiply()
        {
            MiddleFix = " * ";
            CreateReversedStrings();
        }
        public override Vector<double> Calculate(Vector<double> Result, Vector<double>[] parameters, Operator Oper, int Index)
        {
            Vector<double> Num = (Oper.UseRandomNumber) ? Oper.RandomNumber : parameters[Oper.ParameterIndex];
            return (Oper.ResultOnRightSide) ? (Num * Result) : (Result * Num);
        }
        public override void ShowOperator(Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(Oper, Forwards, Backwards);
        }
        public override Vector<double> CalculateConnector(Vector<double> Left, Vector<double> Right, Operator Oper)
        {
            return (Oper.ResultOnRightSide) ? (Left * Right) : (Right * Left);
        }
    }
    public sealed class Divide : Connector
    {
        public Divide()
        {
            MiddleFix = " / ";
            CreateReversedStrings();
        }
        public override Vector<double> Calculate(Vector<double> Result, Vector<double>[] parameters, Operator Oper, int Index)
        {
            Vector<double> Num = (Oper.UseRandomNumber) ? Oper.RandomNumber : parameters[Oper.ParameterIndex];
            return (Oper.ResultOnRightSide) ? (Num / Result) : (Result / Num);
        }
        public override void ShowOperator(Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(Oper, Forwards, Backwards);
        }
        public override Vector<double> CalculateConnector(Vector<double> Left, Vector<double> Right, Operator Oper)
        {
            return (!Oper.ResultOnRightSide) ? (Left / Right) : (Right / Left);
        }
    }
    public class Modulos : Connector
    {
        public Modulos()
        {
            MiddleFix = " % ";
            CreateReversedStrings();
        }
        public override Vector<double> Calculate(Vector<double> Result, Vector<double>[] parameters, Operator Oper, int Index)
        {
            Vector<double> Num = (Oper.UseRandomNumber) ? Oper.RandomNumber : parameters[Oper.ParameterIndex];
            return (Oper.ResultOnRightSide) ? (ShittyVectorMath.Modulus(Num, Result)) : (ShittyVectorMath.Modulus(Result, Num));
        }
        public override void ShowOperator(Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(Oper, Forwards, Backwards);
        }
        public override Vector<double> CalculateConnector(Vector<double> Left, Vector<double> Right, Operator Oper)
        {
            return (Oper.ResultOnRightSide) ? (ShittyVectorMath.Modulus(Left, Right)) : (ShittyVectorMath.Modulus(Right, Left));
        }
    }
    public sealed class PowerOf : Connector
    {
        public PowerOf()
        {
            MiddleFix = " ^ ";
            CreateReversedStrings();
        }
        public override Vector<double> Calculate(Vector<double> Result, Vector<double>[] parameters, Operator Oper, int Index)
        {
            Vector<double> Num = (Oper.UseRandomNumber) ? Oper.RandomNumber : parameters[Oper.ParameterIndex];
            return (Oper.ResultOnRightSide) ? ShittyVectorMath.Pow(Num, Result) : ShittyVectorMath.Pow(Result, Num);
        }
        public override void ShowOperator(Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(Oper, Forwards, Backwards);
        }
        public override Vector<double> CalculateConnector(Vector<double> Left, Vector<double> Right, Operator Oper)
        {
            return (Oper.ResultOnRightSide) ? (ShittyVectorMath.Pow(Left, Right)) : (ShittyVectorMath.Pow(Right, Left));
        }
    }
    public sealed class Root : MathFunction
    {
        public Root()
        {
            IsConnecter = false;
            PreFix = "sqrt";
            CreateReversedStrings();
        }
        public override Vector<double> Calculate(Vector<double> Result, Vector<double>[] parameters, Operator Oper, int Index)
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
            PreFix = "exp";
            CreateReversedStrings();
        }
        public override Vector<double> Calculate(Vector<double> Result, Vector<double>[] parameters, Operator Oper, int Index)
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
            PreFix = "log";
            CreateReversedStrings();
        }
        public override Vector<double> Calculate(Vector<double> Result, Vector<double>[] parameters, Operator Oper, int Index)
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
            PreFix = "log10";
            CreateReversedStrings();
        }
        public override Vector<double> Calculate(Vector<double> Result, Vector<double>[] parameters, Operator Oper, int Index)
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
            PreFix = "floor";
            CreateReversedStrings();
        }
        public override Vector<double> Calculate(Vector<double> Result, Vector<double>[] parameters, Operator Oper, int Index)
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
            PreFix = "ceil";
            CreateReversedStrings();
        }
        public override Vector<double> Calculate(Vector<double> Result, Vector<double>[] parameters, Operator Oper, int Index)
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
            PreFix = "round";
            CreateReversedStrings();
        }
        public override Vector<double> Calculate(Vector<double> Result, Vector<double>[] parameters, Operator Oper, int Index)
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
            PreFix = "sin";
            CreateReversedStrings();
        }
        public override Vector<double> Calculate(Vector<double> Result, Vector<double>[] parameters, Operator Oper, int Index)
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
            PreFix = "cos";
            CreateReversedStrings();
        }
        public override Vector<double> Calculate(Vector<double> Result, Vector<double>[] parameters, Operator Oper, int Index)
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
            PreFix = "tan";
            CreateReversedStrings();
        }
        public override Vector<double> Calculate(Vector<double> Result, Vector<double>[] parameters, Operator Oper, int Index)
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
            PreFix = "asin";
            CreateReversedStrings();
        }
        public override Vector<double> Calculate(Vector<double> Result, Vector<double>[] parameters, Operator Oper, int Index)
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
            PreFix = "acos";
            CreateReversedStrings();
        }
        public override Vector<double> Calculate(Vector<double> Result, Vector<double>[] parameters, Operator Oper, int Index)
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
            PreFix = "atan";
            CreateReversedStrings();
        }
        public override Vector<double> Calculate(Vector<double> Result, Vector<double>[] parameters, Operator Oper, int Index)
        {
            return ShittyVectorMath.Atan(Result);
        }
        public override void ShowOperator(Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(Oper, Forwards, Backwards);
        }
    }

    //Misc
    public class Parentheses : MathFunction
    {
        public Parentheses()
        {
            IsConnecter = false;
        }
        public override Vector<double> Calculate(Vector<double> Result, Vector<double>[] parameters, Operator Oper, int Index)
        {
            Vector<double> Res = parameters[Oper.ParameterIndex];
            foreach (Operator OP in Oper.Operators)
            {
                Res = OP.Calculate(Res, parameters, Index);
                if (!Tools.IsANumber(Res))
                {
                    return Constants.NAN_VECTOR;
                }
            }
            Oper.OperatorResults[Index] = Oper.ExtraMathFunction.CalculateConnector(Res, Result, Oper);
            Oper.MaxCalculated = Index;
            return Oper.OperatorResults[Index];
        }

        public override void ShowOperator(Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            string Left = Tools.ReverseAddStringBuilder(Backwards, Forwards);
            Forwards.Clear();
            Backwards.Clear();

            Forwards.Append(Oper.Eq.EInfo.Goal[0].ParameterNames[Oper.ParameterIndex]);
            foreach (Operator OP in Oper.Operators)
            {
                OP.ShowOperator(Forwards, Backwards);
            }

            string Right = Tools.ReverseAddStringBuilder(Backwards, Forwards);
            Forwards.Clear();
            Backwards.Clear();

            Forwards.Append(Oper.ExtraMathFunction.ShowConnector(Oper, Left, Right));
        }

        public override void MakeRandom(Operator Oper)

        {
            Oper.Eq.SortedOperators.Add(Oper.Operators);
            Oper.UseRandomNumber = false;
            //Oper.parameterIndex = 0;

            //the method CanUseOperator makes sure there is atleast 1 available Operator to use in the parentheses
            int AmountToAdd = SynchronizedRandom.Next(1, Oper.Eq.OperatorsLeft - 1);

            while (0 < Oper.Eq.OperatorsLeft - AmountToAdd)
            {
                Operator ToAdd = Oper.Eq.OPStorage.Pop();
                // by adding the operator now there is space for 1 less operator
                ToAdd.MakeRandom(Oper.Operators, Oper);
            }
            Oper.ExtraMathFunction = Oper.Eq.EInfo.Connectors[SynchronizedRandom.Next(0, Oper.Eq.EInfo.Connectors.Length)];
        }
        public override bool CanUseOperator(Operator Oper)
        {
            return Oper.Eq.OperatorsLeft > 1;
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
                Oper.GetCopy(Copy.Eq.OPStorage.Pop(), Copy.Eq, Copy.Operators, Copy);
            }
            Array.Copy(Original.OperatorResults, Copy.OperatorResults, Original.MaxCalculated + 1);
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
        public override void OperatorChanged(Operator Oper)
        {
            Oper.ResetMaxCalculated();
            Oper.Holder.OperatorChanged();
        }
    }
    public sealed class Constant : Parentheses
    {
        public override Vector<double> Calculate(Vector<double> Result, Vector<double>[] parameters, Operator Oper, int Index)
        {
            Vector<double> Res = Oper.RandomNumber;
            foreach (Operator OP in Oper.Operators)
            {
                Res = OP.Calculate(Res, parameters, Index);
                if (!Tools.IsANumber(Res))
                {
                    return Constants.NAN_VECTOR;
                }
            }
            Oper.OperatorResults[Index] = Oper.ExtraMathFunction.CalculateConnector(Res, Result, Oper);
            Oper.MaxCalculated = Index;
            return Oper.OperatorResults[Index];
        }
        public override void ShowOperator(Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            string Left = Tools.ReverseAddStringBuilder(Backwards, Forwards);
            Forwards.Clear();
            Backwards.Clear();

            Forwards.Append(Oper.RandomNumber[0].ToString());
            foreach (Operator OP in Oper.Operators)
            {
                OP.ShowOperator(Forwards, Backwards);
            }

            string Right = Tools.ReverseAddStringBuilder(Backwards, Forwards);
            Forwards.Clear();
            Backwards.Clear();

            Forwards.Append(Oper.ExtraMathFunction.ShowConnector(Oper, Left, Right));
        }
        public override void MakeRandom(Operator Oper)
        {
            base.MakeRandom(Oper);
            foreach (Operator Op in Oper.Operators)
            {
                Op.UseRandomNumber = true;
            }
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
        public override Vector<double> Calculate(Vector<double> Result, Vector<double>[] parameters, Operator Oper, int Index)
        {
            return Vector.Abs<double>(Result);
        }
        public override void ShowOperator(Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(Oper, Forwards, Backwards);
        }
    }

    //Logic Operators
    public sealed class AND : Connector
    {
        public AND()
        {
            MiddleFix = " and ";
            CreateReversedStrings();
        }
        public override Vector<double> Calculate(Vector<double> Result, Vector<double>[] parameters, Operator Oper, int Index)
        {
            Vector<double> Num = (Oper.UseRandomNumber) ? Oper.RandomNumber : parameters[Oper.ParameterIndex];
            return ShittyVectorLogic.AND(Num, Result);
        }
        public override void ShowOperator(Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(Oper, Forwards, Backwards);
        }

        public override Vector<double> CalculateConnector(Vector<double> Left, Vector<double> Right, Operator Oper)
        {
            return ShittyVectorLogic.AND(Left, Right);
        }
    }
    public sealed class NAND : Connector
    {
        public NAND()
        {
            MiddleFix = " nand ";
            CreateReversedStrings();
        }
        public override Vector<double> Calculate(Vector<double> Result, Vector<double>[] parameters, Operator Oper, int Index)
        {
            Vector<double> Num = (Oper.UseRandomNumber) ? Oper.RandomNumber : parameters[Oper.ParameterIndex];
            return ShittyVectorLogic.NAND(Num, Result);
        }
        public override void ShowOperator(Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(Oper, Forwards, Backwards);
        }

        public override Vector<double> CalculateConnector(Vector<double> Left, Vector<double> Right, Operator Oper)
        {
            return ShittyVectorLogic.NAND(Left, Right);
        }
    }
    public sealed class OR : Connector
    {
        public OR()
        {
            MiddleFix = " or ";
            CreateReversedStrings();
        }
        public override Vector<double> Calculate(Vector<double> Result, Vector<double>[] parameters, Operator Oper, int Index)
        {
            Vector<double> Num = (Oper.UseRandomNumber) ? Oper.RandomNumber : parameters[Oper.ParameterIndex];
            return ShittyVectorLogic.OR(Num, Result);
        }
        public override void ShowOperator(Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(Oper, Forwards, Backwards);
        }

        public override Vector<double> CalculateConnector(Vector<double> Left, Vector<double> Right, Operator Oper)
        {
            return ShittyVectorLogic.OR(Left, Right);
        }
    }
    public sealed class NOR : Connector
    {
        public NOR()
        {
            MiddleFix = " nor ";
            CreateReversedStrings();
        }
        public override Vector<double> Calculate(Vector<double> Result, Vector<double>[] parameters, Operator Oper, int Index)
        {
            Vector<double> Num = (Oper.UseRandomNumber) ? Oper.RandomNumber : parameters[Oper.ParameterIndex];
            return ShittyVectorLogic.NOR(Num, Result);
        }
        public override void ShowOperator(Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(Oper, Forwards, Backwards);
        }

        public override Vector<double> CalculateConnector(Vector<double> Left, Vector<double> Right, Operator Oper)
        {
            return ShittyVectorLogic.NOR(Left, Right);
        }
    }
    public sealed class XOR : Connector
    {
        public XOR()
        {
            MiddleFix = " xor ";
            CreateReversedStrings();
        }
        public override Vector<double> Calculate(Vector<double> Result, Vector<double>[] parameters, Operator Oper, int Index)
        {
            Vector<double> Num = (Oper.UseRandomNumber) ? Oper.RandomNumber : parameters[Oper.ParameterIndex];
            return ShittyVectorLogic.XOR(Num, Result);
        }
        public override void ShowOperator(Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(Oper, Forwards, Backwards);
        }

        public override Vector<double> CalculateConnector(Vector<double> Left, Vector<double> Right, Operator Oper)
        {
            return ShittyVectorLogic.XOR(Left, Right);
        }
    }
    public sealed class XNOR : Connector
    {
        public XNOR()
        {
            MiddleFix = " xnor ";
            CreateReversedStrings();
        }
        public override Vector<double> Calculate(Vector<double> Result, Vector<double>[] parameters, Operator Oper, int Index)
        {
            Vector<double> Num = (Oper.UseRandomNumber) ? Oper.RandomNumber : parameters[Oper.ParameterIndex];
            return ShittyVectorLogic.XNOR(Num, Result);
        }
        public override void ShowOperator(Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(Oper, Forwards, Backwards);
        }

        public override Vector<double> CalculateConnector(Vector<double> Left, Vector<double> Right, Operator Oper)
        {
            return ShittyVectorLogic.XNOR(Left, Right);
        }
    }
    public sealed class NOT : MathFunction
    {
        public NOT()
        {
            IsConnecter = false;
            PreFix = " not ";
            CreateReversedStrings();
        }
        public override Vector<double> Calculate(Vector<double> Result, Vector<double>[] parameters, Operator Oper, int Index)
        {
            return ShittyVectorLogic.NOT(Result);
        }
        public override void ShowOperator(Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(Oper, Forwards, Backwards);
        }
    }
}