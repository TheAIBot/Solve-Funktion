﻿using System;
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

        public abstract bool Calculate(double[] Result, double[][] parameters, Operator Oper);
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
            string Num = (Oper.UseRandomNumber) ? Oper.RandomNumber.ToString() : Oper.Eq.EInfo.coordInfo.parameterNames[Oper.ParameterIndex];
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
        public abstract void CalculateConnector(double[] results, double[] scalar, Operator oper);

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
        public override bool Calculate(double[] result, double[][] parameters, Operator oper)
        {
            for (int i = 0; i < result.Length; i++)
            {
                double Num = (oper.UseRandomNumber) ? oper.RandomNumber : parameters[oper.ParameterIndex][i];
                result[i] = (oper.ResultOnRightSide) ? (Num + result[i]) : (result[i] + Num);
            }
            return true;
        }
        public override void ShowOperator(Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(Oper, Forwards, Backwards);
        }
        public override void CalculateConnector(double[] results, double[] scalars, Operator oper)
        {
            for (int i = 0; i < results.Length; i++)
            {
                results[i] = (oper.ResultOnRightSide) ? (results[i] + scalars[i]) : (scalars[i] + results[i]);
            }
        }
    }
    public sealed class Subtract : Connector
    {
        public Subtract()
        {
            MiddleFix = " - ";
            CreateReversedStrings();
        }
        public override bool Calculate(double[] result, double[][] parameters, Operator oper)
        {
            for (int i = 0; i < result.Length; i++)
            {
                double Num = (oper.UseRandomNumber) ? oper.RandomNumber : parameters[oper.ParameterIndex][i];
                result[i] = (oper.ResultOnRightSide) ? (Num - result[i]) : (result[i] - Num);
            }
            return true;
        }
        public override void ShowOperator(Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(Oper, Forwards, Backwards);
        }
        public override void CalculateConnector(double[] results, double[] scalars, Operator oper)
        {
            for (int i = 0; i < results.Length; i++)
            {
                results[i] = (oper.ResultOnRightSide) ? (results[i] - scalars[i]) : (scalars[i] - results[i]);
            }
        }
    }
    public sealed class Multiply : Connector
    {
        public Multiply()
        {
            MiddleFix = " * ";
            CreateReversedStrings();
        }
        public override bool Calculate(double[] result, double[][] parameters, Operator oper)
        {
            for (int i = 0; i < result.Length; i++)
            {
                double Num = (oper.UseRandomNumber) ? oper.RandomNumber : parameters[oper.ParameterIndex][i];
                result[i] = (oper.ResultOnRightSide) ? (Num * result[i]) : (result[i] * Num);
            }
            return true;
        }
        public override void ShowOperator(Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(Oper, Forwards, Backwards);
        }
        public override void CalculateConnector(double[] results, double[] scalars, Operator oper)
        {
            for (int i = 0; i < results.Length; i++)
            {
                results[i] = (oper.ResultOnRightSide) ? (results[i] * scalars[i]) : (scalars[i] * results[i]);
            }
        }
    }
    public sealed class Divide : Connector
    {
        public Divide()
        {
            MiddleFix = " / ";
            CreateReversedStrings();
        }
        public override bool Calculate(double[] result, double[][] parameters, Operator oper)
        {
            for (int i = 0; i < result.Length; i++)
            {
                double Num = (oper.UseRandomNumber) ? oper.RandomNumber : parameters[oper.ParameterIndex][i];
                result[i] = (oper.ResultOnRightSide) ? (Num / result[i]) : (result[i] / Num);
            }
            return true;
        }
        public override void ShowOperator(Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(Oper, Forwards, Backwards);
        }
        public override void CalculateConnector(double[] results, double[] scalars, Operator oper)
        {
            for (int i = 0; i < results.Length; i++)
            {
                results[i] = (oper.ResultOnRightSide) ? (results[i] / scalars[i]) : (scalars[i] / results[i]);
            }
        }
    }
    public class Modulos : Connector
    {
        public Modulos()
        {
            MiddleFix = " % ";
            CreateReversedStrings();
        }
        public override bool Calculate(double[] result, double[][] parameters, Operator oper)
        {
            for (int i = 0; i < result.Length; i++)
            {
                double Num = (oper.UseRandomNumber) ? oper.RandomNumber : parameters[oper.ParameterIndex][i];
                result[i] = (oper.ResultOnRightSide) ? (Num % result[i]) : (result[i] % Num);
            }
            return true;
        }
        public override void ShowOperator(Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(Oper, Forwards, Backwards);
        }
        public override void CalculateConnector(double[] results, double[] scalars, Operator oper)
        {
            for (int i = 0; i < results.Length; i++)
            {
                results[i] = (oper.ResultOnRightSide) ? (results[i] % scalars[i]) : (scalars[i] % results[i]);
            }
        }
    }
    public sealed class PowerOf : Connector
    {
        public PowerOf()
        {
            MiddleFix = " ^ ";
            CreateReversedStrings();
        }
        public override bool Calculate(double[] result, double[][] parameters, Operator oper)
        {
            for (int i = 0; i < result.Length; i++)
            {
                double Num = (oper.UseRandomNumber) ? oper.RandomNumber : parameters[oper.ParameterIndex][i];
                result[i] = (oper.ResultOnRightSide) ? (Math.Pow(Num, result[i])) : (Math.Pow(result[i], Num));
            }
            return true;
        }
        public override void ShowOperator(Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(Oper, Forwards, Backwards);
        }
        public override void CalculateConnector(double[] results, double[] scalars, Operator oper)
        {
            for (int i = 0; i < results.Length; i++)
            {
                results[i] = (oper.ResultOnRightSide) ? (Math.Pow(results[i], scalars[i])) : (Math.Pow(scalars[i], results[i]));
            }
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
        public override bool Calculate(double[] result, double[][] parameters, Operator oper)
        {
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = Math.Sqrt(result[i]);
            }
            return true;
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
        public override bool Calculate(double[] result, double[][] parameters, Operator oper)
        {
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = Math.Exp(result[i]);
            }
            return true;
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
        public override bool Calculate(double[] result, double[][] parameters, Operator oper)
        {
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = Math.Log(result[i]);
            }
            return true;
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
        public override bool Calculate(double[] result, double[][] parameters, Operator oper)
        {
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = Math.Log10(result[i]);
            }
            return true;
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
        public override bool Calculate(double[] result, double[][] parameters, Operator oper)
        {
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = Math.Floor(result[i]);
            }
            return true;
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
        public override bool Calculate(double[] result, double[][] parameters, Operator oper)
        {
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = Math.Ceiling(result[i]);
            }
            return true;
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
        public override bool Calculate(double[] result, double[][] parameters, Operator oper)
        {
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = Math.Round(result[i]);
            }
            return true;
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
        public override bool Calculate(double[] result, double[][] parameters, Operator oper)
        {
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = Math.Sin(result[i]);
            }
            return true;
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
        public override bool Calculate(double[] result, double[][] parameters, Operator oper)
        {
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = Math.Cos(result[i]);
            }
            return true;
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
        public override bool Calculate(double[] result, double[][] parameters, Operator oper)
        {
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = Math.Tan(result[i]);
            }
            return true;
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
        public override bool Calculate(double[] result, double[][] parameters, Operator oper)
        {
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = Math.Asin(result[i]);
            }
            return true;
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
        public override bool Calculate(double[] result, double[][] parameters, Operator oper)
        {
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = Math.Acos(result[i]);
            }
            return true;
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
        public override bool Calculate(double[] result, double[][] parameters, Operator oper)
        {
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = Math.Atan(result[i]);
            }
            return true;
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
        public override bool Calculate(double[] result, double[][] parameters, Operator oper)
        {
            //need to find out how i can remove the creation of this array
            double[] parenthesesResults = new double[result.Length];
            Array.Copy(parameters[oper.ParameterIndex], parenthesesResults, parenthesesResults.Length);
            for (int i = 0; i < oper.Operators.Length; i++)
            {
                if (oper.Operators[i] != null)
                {
                    if (oper.Operators[i].Calculate(parenthesesResults, parameters))
                    {
                        if (!Tools.IsANumber(parenthesesResults))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            oper.ExtraMathFunction.CalculateConnector(result, parenthesesResults, oper);
            return true;
        }

        public override void ShowOperator(Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            string Left = Tools.ReverseAddStringBuilder(Backwards, Forwards);
            Forwards.Clear();
            Backwards.Clear();

            Forwards.Append(Oper.Eq.EInfo.coordInfo.parameterNames[Oper.ParameterIndex]);
            for (int i = 0; i < Oper.Operators.Length; i++) // can optimize this to only run untill all operators have been cleaned
            {
                if (Oper.Operators[i] != null)
                {
                    Oper.Operators[i].ShowOperator(Forwards, Backwards);
                }

            }

            string Right = Tools.ReverseAddStringBuilder(Backwards, Forwards);
            Forwards.Clear();
            Backwards.Clear();

            Forwards.Append(Oper.ExtraMathFunction.ShowConnector(Oper, Left, Right));
        }

        public override void MakeRandom(Operator Oper)
        {
            Oper.Eq.Holders.Add(Oper);
            Oper.UseRandomNumber = false;
            //Oper.parameterIndex = 0;

            //the method CanUseOperator makes sure there is atleast 1 available Operator to use in the parentheses
            int AmountToAdd = SynchronizedRandom.Next(1, Oper.Eq.OperatorsLeft - 1);

            while (0 < Oper.Eq.OperatorsLeft - AmountToAdd)
            {
                Oper.AddOperator(Oper.Eq.OPStorage);
            }
            Oper.ExtraMathFunction = Oper.Eq.EInfo.Connectors[SynchronizedRandom.Next(0, Oper.Eq.EInfo.Connectors.Length)];
        }
        public override bool CanUseOperator(Operator Oper)
        {
            return Oper.Eq.OperatorsLeft > 1;
        }
        public override int GetOperatorCount(Operator Oper)
        {
            // the parentheses is an operator in itself so the amount starts at 1
            // so the parentheses opearator is accounted for
            int Amount = 1;
            for (int i = 0; i < Oper.Operators.Length; i++)
            {
                if (Oper.Operators[i] != null)
                {
                    Amount += Oper.Operators[i].GetOperatorCount();
                }
            }
            return Amount;
        }

        public override void GetCopy(Operator Original, Operator Copy)
        {
            Copy.Eq.Holders.Add(Copy);
            Copy.ExtraMathFunction = Original.ExtraMathFunction;
            for (int i = 0; i < Original.Operators.Length; i++) // can optimize this to only run untill all operators have been cleaned
            {
                if (Original.Operators[i] != null)
                {
                    Original.Operators[i].GetCopy(Copy.Eq.OPStorage.Pop(), Copy.Eq, Copy.Operators, Copy);
                }
            }
        }
        public override void StoreAndCleanup(Operator Oper)
        {
            Oper.Eq.Holders.Remove(Oper);
            Oper.ExtraMathFunction = null;
            // the Operators list is being altered in this loop so it can't be a foreach loop
            // that's why it's done this way
            for (int i = 0; i < Oper.Operators.Length; i++) // can optimize this to only run untill all operators have been cleaned
            {
                if (Oper.Operators[i] != null)
                {
                    Oper.Operators[i].StoreAndCleanup();
                }
            }
        }
        public override void StoreAndCleanupAll(Operator Oper)
        {
            for (int i = 0; i < Oper.Operators.Length; i++) // can optimize this to only run untill all operators have been cleaned
            {
                if (Oper.Operators[i] != null)
                {
                    Oper.Operators[i].StoreAndCleanupAll();
                }
            }
            //Oper.NumberOfOperators = 0;
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
        public override bool Calculate(double[] result, double[][] parameters, Operator oper)
        {
            double[] parenthesesResults = new double[result.Length];
            parenthesesResults.Fill(oper.RandomNumber);
            for (int i = 0; i < oper.Operators.Length; i++)
            {
                if (oper.Operators[i] != null)
                {
                    if (oper.Operators[i].Calculate(parenthesesResults, parameters))
                    {
                        if (!Tools.IsANumber(parenthesesResults))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            oper.ExtraMathFunction.CalculateConnector(result, parenthesesResults, oper);
            return true;
        }
        public override void ShowOperator(Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            string Left = Tools.ReverseAddStringBuilder(Backwards, Forwards);
            Forwards.Clear();
            Backwards.Clear();

            Forwards.Append(Oper.RandomNumber.ToString());
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
            for (int i = 0; i < Oper.Operators.Length; i++)
            {
                if (Oper.Operators[i] != null)
                {
                    Oper.Operators[i].UseRandomNumber = true;
                }
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
        public override bool Calculate(double[] result, double[][] parameters, Operator oper)
        {
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = Math.Abs(result[i]);
            }
            return true;
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
        public override bool Calculate(double[] result, double[][] parameters, Operator oper)
        {
            for (int i = 0; i < result.Length; i++)
            {
                double Num = (oper.UseRandomNumber) ? oper.RandomNumber : parameters[oper.ParameterIndex][i];
                result[i] = (oper.ResultOnRightSide) ? (MathLogic.AND(Num, result[i])) : (MathLogic.AND(result[i], Num));
            }
            return true;
        }
        public override void ShowOperator(Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(Oper, Forwards, Backwards);
        }
        public override void CalculateConnector(double[] results, double[] scalars, Operator oper)
        {
            for (int i = 0; i < results.Length; i++)
            {
                results[i] = (oper.ResultOnRightSide) ? (MathLogic.AND(results[i], scalars[i])) : (MathLogic.AND(scalars[i], results[i]));
            }
        }
    }
    public sealed class NAND : Connector
    {
        public NAND()
        {
            MiddleFix = " nand ";
            CreateReversedStrings();
        }
        public override bool Calculate(double[] result, double[][] parameters, Operator oper)
        {
            for (int i = 0; i < result.Length; i++)
            {
                double Num = (oper.UseRandomNumber) ? oper.RandomNumber : parameters[oper.ParameterIndex][i];
                result[i] = (oper.ResultOnRightSide) ? (MathLogic.NAND(Num, result[i])) : (MathLogic.NAND(result[i], Num));
            }
            return true;
        }
        public override void ShowOperator(Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(Oper, Forwards, Backwards);
        }
        public override void CalculateConnector(double[] results, double[] scalars, Operator oper)
        {
            for (int i = 0; i < results.Length; i++)
            {
                results[i] = (oper.ResultOnRightSide) ? (MathLogic.NAND(results[i], scalars[i])) : (MathLogic.NAND(scalars[i], results[i]));
            }
        }
    }
    public sealed class OR : Connector
    {
        public OR()
        {
            MiddleFix = " or ";
            CreateReversedStrings();
        }
        public override bool Calculate(double[] result, double[][] parameters, Operator oper)
        {
            for (int i = 0; i < result.Length; i++)
            {
                double Num = (oper.UseRandomNumber) ? oper.RandomNumber : parameters[oper.ParameterIndex][i];
                result[i] = (oper.ResultOnRightSide) ? (MathLogic.OR(Num, result[i])) : (MathLogic.OR(result[i], Num));
            }
            return true;
        }
        public override void ShowOperator(Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(Oper, Forwards, Backwards);
        }
        public override void CalculateConnector(double[] results, double[] scalars, Operator oper)
        {
            for (int i = 0; i < results.Length; i++)
            {
                results[i] = (oper.ResultOnRightSide) ? (MathLogic.OR(results[i], scalars[i])) : (MathLogic.OR(scalars[i], results[i]));
            }
        }
    }
    public sealed class NOR : Connector
    {
        public NOR()
        {
            MiddleFix = " nor ";
            CreateReversedStrings();
        }
        public override bool Calculate(double[] result, double[][] parameters, Operator oper)
        {
            for (int i = 0; i < result.Length; i++)
            {
                double Num = (oper.UseRandomNumber) ? oper.RandomNumber : parameters[oper.ParameterIndex][i];
                result[i] = (oper.ResultOnRightSide) ? (MathLogic.NOR(Num, result[i])) : (MathLogic.NOR(result[i], Num));
            }
            return true;
        }
        public override void ShowOperator(Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(Oper, Forwards, Backwards);
        }
        public override void CalculateConnector(double[] results, double[] scalars, Operator oper)
        {
            for (int i = 0; i < results.Length; i++)
            {
                results[i] = (oper.ResultOnRightSide) ? (MathLogic.NOR(results[i], scalars[i])) : (MathLogic.NOR(scalars[i], results[i]));
            }
        }
    }
    public sealed class XOR : Connector
    {
        public XOR()
        {
            MiddleFix = " xor ";
            CreateReversedStrings();
        }
        public override bool Calculate(double[] result, double[][] parameters, Operator oper)
        {
            for (int i = 0; i < result.Length; i++)
            {
                double Num = (oper.UseRandomNumber) ? oper.RandomNumber : parameters[oper.ParameterIndex][i];
                result[i] = (oper.ResultOnRightSide) ? (MathLogic.XOR(Num, result[i])) : (MathLogic.XOR(result[i], Num));
            }
            return true;
        }
        public override void ShowOperator(Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(Oper, Forwards, Backwards);
        }
        public override void CalculateConnector(double[] results, double[] scalars, Operator oper)
        {
            for (int i = 0; i < results.Length; i++)
            {
                results[i] = (oper.ResultOnRightSide) ? (MathLogic.XOR(results[i], scalars[i])) : (MathLogic.XOR(scalars[i], results[i]));
            }
        }
    }
    public sealed class XNOR : Connector
    {
        public XNOR()
        {
            MiddleFix = " xnor ";
            CreateReversedStrings();
        }
        public override bool Calculate(double[] result, double[][] parameters, Operator oper)
        {
            for (int i = 0; i < result.Length; i++)
            {
                double Num = (oper.UseRandomNumber) ? oper.RandomNumber : parameters[oper.ParameterIndex][i];
                result[i] = (oper.ResultOnRightSide) ? (MathLogic.XNOR(Num, result[i])) : (MathLogic.XNOR(result[i], Num));
            }
            return true;
        }
        public override void ShowOperator(Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(Oper, Forwards, Backwards);
        }
        public override void CalculateConnector(double[] results, double[] scalars, Operator oper)
        {
            for (int i = 0; i < results.Length; i++)
            {
                results[i] = (oper.ResultOnRightSide) ? (MathLogic.XNOR(results[i], scalars[i])) : (MathLogic.XNOR(scalars[i], results[i]));
            }
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
        public override bool Calculate(double[] result, double[][] parameters, Operator oper)
        {
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = MathLogic.NOT(result[i]);
            }
            return true;
        }
        public override void ShowOperator(Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawOperator(Oper, Forwards, Backwards);
        }
    }
}