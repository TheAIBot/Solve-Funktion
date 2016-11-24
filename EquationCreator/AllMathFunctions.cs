using System;
using System.Linq;
using System.Text;

namespace EquationCreator
{
    public enum OperatorType
    {
        Connector,
        Single,
        Complex
    }

    [Serializable]
    public abstract class SimpleOperator
    {
        public bool IsConnecter = true;
        protected string PreFix = String.Empty;
        protected string MiddleFix = String.Empty;
        protected string PostFix = String.Empty;
        private string ReversedPreFix;
        private string ReversedMiddleFix;

        public virtual OperatorType getOperatorType()
        {
            return OperatorType.Complex;
        }

        public abstract bool Calculate(float[] Result, float[][] parameters, Operator Oper);
        public abstract void ShowOperator(Operator Oper, StringBuilder Forwards, StringBuilder Backwards);

        public abstract void MakeRandom(Operator Oper);
        public abstract bool CanUseOperator(Operator Oper);
        public abstract int GetOperatorCount(Operator Oper);

        public abstract void GetCopy(Operator Original, Operator Copy);
        public abstract void StoreAndCleanup(Operator Oper);
        public abstract void StoreAndCleanupAll(Operator Oper);
        public abstract void OperatorChanged(Operator Oper);
       

        protected void CreateReversedStrings()
        {
            ReversedPreFix = RevertString(PreFix);
            ReversedMiddleFix = RevertString(MiddleFix);
        }
        protected void DrawConnector(Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            string Num = (Oper.UseRandomNumber) ? Oper.RandomNumber.ToString() : Oper.Eq.EInfo.coordInfo.parameterNames[Oper.ParameterIndex];
            DrawConnector(Oper, Forwards, Backwards, Num);
        }

        private void DrawConnector(Operator Oper, StringBuilder Forwards, StringBuilder Backwards, string ToInsert)
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

        protected void DrawSingle(StringBuilder Forwards, StringBuilder Backwards)
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

    public abstract class Connector : SimpleOperator
    {
        public override OperatorType getOperatorType()
        {
            return OperatorType.Connector;
        }

        public override bool Calculate(float[] result, float[][] parameters, Operator oper)
        {
            return true;
        }

        public abstract void CalculateConnector(float[] result, float[] parameter, bool useRandomNumber, float randomNumber, bool resultOnRightSide);

        public abstract void CalculateFixedConnector(float[] results, float[] scalar, bool resultOnRightSide);

        public override void ShowOperator(Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawConnector(Oper, Forwards, Backwards);
        }

        public string ShowConnector(Operator Oper, string Left, string Right)
        {
            return (Oper.ResultOnRightSide) ? ("(" + Left + MiddleFix + Right + ")") : ("(" + Right + MiddleFix + Left + ")");
        }

        public override void MakeRandom(Operator Oper)
        {

        }
        public override bool CanUseOperator(Operator Oper)
        {
            return true;
        }
        public override int GetOperatorCount(Operator Oper)
        {
            return 1;
        }

        public override void GetCopy(Operator Original, Operator Copy)
        {

        }
        public override void StoreAndCleanup(Operator Oper)
        {

        }
        public override void StoreAndCleanupAll(Operator Oper)
        {

        }
        public override void OperatorChanged(Operator Oper)
        {
            Oper.Holder.OperatorChanged();
        }
    }

    public abstract class OperatorSingle : SimpleOperator
    {
        public override OperatorType getOperatorType()
        {
            return OperatorType.Single;
        }

        public override bool Calculate(float[] result, float[][] parameters, Operator oper)
        {
            return true;
        }

        public abstract void CalculateSingle(float[] result);

        public override void ShowOperator(Operator Oper, StringBuilder Forwards, StringBuilder Backwards)
        {
            DrawSingle(Forwards, Backwards);
        }

        public override void MakeRandom(Operator Oper)
        {

        }
        public override bool CanUseOperator(Operator Oper)
        {
            return true;
        }
        public override int GetOperatorCount(Operator Oper)
        {
            return 1;
        }

        public override void GetCopy(Operator Original, Operator Copy)
        {

        }
        public override void StoreAndCleanup(Operator Oper)
        {

        }
        public override void StoreAndCleanupAll(Operator Oper)
        {

        }
        public override void OperatorChanged(Operator Oper)
        {
            Oper.Holder.OperatorChanged();
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
        public override void CalculateConnector(float[] result, float[] parameter, bool useRandomNumber, float randomNumber, bool resultOnRightSide)
        {
            if (useRandomNumber)
            {
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = result[i] + randomNumber;
                }
            }
            else
            {
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = result[i] + parameter[i];
                }
            }
        }

        public override void CalculateFixedConnector(float[] results, float[] scalars, bool resultOnRightSide)
        {
            for (int i = 0; i < results.Length; i++)
            {
                results[i] = results[i] + scalars[i];
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
        public override void CalculateConnector(float[] result, float[] parameter, bool useRandomNumber, float randomNumber, bool resultOnRightSide)
        {
            if (useRandomNumber)
            {
                if (!resultOnRightSide)
                {
                    for (int i = 0; i < result.Length; i++)
                    {
                        result[i] = result[i] - randomNumber;
                    }
                }
                else
                {
                    for (int i = 0; i < result.Length; i++)
                    {
                        result[i] = randomNumber - result[i];
                    }
                }
            }
            else
            {
                if (!resultOnRightSide)
                {
                    for (int i = 0; i < result.Length; i++)
                    {
                        result[i] = result[i] - parameter[i];
                    }
                }
                else
                {
                    for (int i = 0; i < result.Length; i++)
                    {
                        result[i] = parameter[i] - result[i];
                    }
                }
            }
        }
        public override void CalculateFixedConnector(float[] results, float[] scalars, bool resultOnRightSide)
        {
            if (resultOnRightSide)
            {
                for (int i = 0; i < results.Length; i++)
                {
                    results[i] = results[i] - scalars[i];
                }
            }
            else
            {
                for (int i = 0; i < results.Length; i++)
                {
                    results[i] = scalars[i] - results[i];
                }
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
        public override void CalculateConnector(float[] result, float[] parameter, bool useRandomNumber, float randomNumber, bool resultOnRightSide)
        {
            if (useRandomNumber)
            {
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = result[i] * randomNumber;
                }
            }
            else
            {
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = result[i] * parameter[i];
                }
            }
        }
        public override void CalculateFixedConnector(float[] results, float[] scalars, bool resultOnRightSide)
        {
            for (int i = 0; i < results.Length; i++)
            {
                results[i] = results[i] * scalars[i];
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
        public override void CalculateConnector(float[] result, float[] parameter, bool useRandomNumber, float randomNumber, bool resultOnRightSide)
        {
            if (useRandomNumber)
            {
                if (!resultOnRightSide)
                {
                    for (int i = 0; i < result.Length; i++)
                    {
                        result[i] = result[i] / randomNumber;
                    }
                }
                else
                {
                    for (int i = 0; i < result.Length; i++)
                    {
                        result[i] = randomNumber / result[i];
                    }
                }
            }
            else
            {
                if (!resultOnRightSide)
                {
                    for (int i = 0; i < result.Length; i++)
                    {
                        result[i] = result[i] / parameter[i];
                    }
                }
                else
                {
                    for (int i = 0; i < result.Length; i++)
                    {
                        result[i] = parameter[i] / result[i];
                    }
                }
            }
        }
        public override void CalculateFixedConnector(float[] results, float[] scalars, bool resultOnRightSide)
        {
            if (resultOnRightSide)
            {
                for (int i = 0; i < results.Length; i++)
                {
                    results[i] = results[i] / scalars[i];
                }
            }
            else
            {
                for (int i = 0; i < results.Length; i++)
                {
                    results[i] = scalars[i] / results[i];
                }
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
        public override void CalculateConnector(float[] result, float[] parameter, bool useRandomNumber, float randomNumber, bool resultOnRightSide)
        {
            if (useRandomNumber)
            {
                if (resultOnRightSide)
                {
                    for (int i = 0; i < result.Length; i++)
                    {
                        result[i] = result[i] % randomNumber;
                    }
                }
                else
                {
                    for (int i = 0; i < result.Length; i++)
                    {
                        result[i] = randomNumber % result[i];
                    }
                }
            }
            else
            {
                if (resultOnRightSide)
                {
                    for (int i = 0; i < result.Length; i++)
                    {
                        result[i] = result[i] % parameter[i];
                    }
                }
                else
                {
                    for (int i = 0; i < result.Length; i++)
                    {
                        result[i] = parameter[i] % result[i];
                    }
                }
            }
        }
        public override void CalculateFixedConnector(float[] results, float[] scalars, bool resultOnRightSide)
        {
            if (resultOnRightSide)
            {
                for (int i = 0; i < results.Length; i++)
                {
                    results[i] = results[i] % scalars[i];
                }
            }
            else
            {
                for (int i = 0; i < results.Length; i++)
                {
                    results[i] = scalars[i] % results[i];
                }
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
        public override void CalculateConnector(float[] result, float[] parameter, bool useRandomNumber, float randomNumber, bool resultOnRightSide)
        {
            if (useRandomNumber)
            {
                if (resultOnRightSide)
                {
                    for (int i = 0; i < result.Length; i++)
                    {
                        result[i] = (float)Math.Pow(result[i], randomNumber);
                    }
                }
                else
                {
                    for (int i = 0; i < result.Length; i++)
                    {
                        result[i] = (float)Math.Pow(randomNumber, result[i]);
                    }
                }
            }
            else
            {
                if (resultOnRightSide)
                {
                    for (int i = 0; i < result.Length; i++)
                    {
                        result[i] = (float)Math.Pow(result[i], parameter[i]);
                    }
                }
                else
                {
                    for (int i = 0; i < result.Length; i++)
                    {
                        result[i] = (float)Math.Pow(parameter[i], result[i]);
                    }
                }
            }
        }
        public override void CalculateFixedConnector(float[] results, float[] scalars, bool resultOnRightSide)
        {
            if (resultOnRightSide)
            {
                for (int i = 0; i < results.Length; i++)
                {
                    results[i] = (float)Math.Pow(results[i], scalars[i]);
                }
            }
            else
            {
                for (int i = 0; i < results.Length; i++)
                {
                    results[i] = (float)Math.Pow(scalars[i], results[i]);
                }
            }
        }
    }


    public sealed class Root : OperatorSingle
    {
        public Root()
        {
            IsConnecter = false;
            PreFix = "sqrt";
            CreateReversedStrings();
        }
        public override void CalculateSingle(float[] result)
        {
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = (float)Math.Sqrt(result[i]);
            }
        }
    }
    public class Exponent : OperatorSingle
    {
        public Exponent()
        {
            IsConnecter = false;
            PreFix = "exp";
            CreateReversedStrings();
        }
        public override void CalculateSingle(float[] result)
        {
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = (float)Math.Exp(result[i]);
            }
        }
    }
    public class NaturalLog : OperatorSingle
    {
        public NaturalLog()
        {
            IsConnecter = false;
            PreFix = "log";
            CreateReversedStrings();
        }
        public override void CalculateSingle(float[] result)
        {
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = (float)Math.Log(result[i]);
            }
        }
    }
    public class Log : OperatorSingle
    {
        public Log()
        {
            IsConnecter = false;
            PreFix = "log10";
            CreateReversedStrings();
        }
        public override void CalculateSingle(float[] result)
        {
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = (float)Math.Log10(result[i]);
            }
        }
    }

    //Rounders
    public class Floor : OperatorSingle
    {
        public Floor()
        {
            IsConnecter = false;
            PreFix = "floor";
            CreateReversedStrings();
        }
        public override void CalculateSingle(float[] result)
        {
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = (float)Math.Floor(result[i]);
            }
        }
    }
    public class Ceil : OperatorSingle
    {
        public Ceil()
        {
            IsConnecter = false;
            PreFix = "ceil";
            CreateReversedStrings();
        }
        public override void CalculateSingle(float[] result)
        {
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = (float)Math.Ceiling(result[i]);
            }
        }
    }
    public class Round : OperatorSingle
    {
        public Round()
        {
            IsConnecter = false;
            PreFix = "round";
            CreateReversedStrings();
        }
        public override void CalculateSingle(float[] result)
        {
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = (float)Math.Round(result[i]);
            }
        }
    }

    //Trigonomic
    public class Sin : OperatorSingle
    {
        public Sin()
        {
            IsConnecter = false;
            PreFix = "sin";
            CreateReversedStrings();
        }
        public override void CalculateSingle(float[] result)
        {
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = (float)Math.Sin(result[i]);
            }
        }
    }
    public class Cos : OperatorSingle
    {
        public Cos()
        {
            IsConnecter = false;
            PreFix = "cos";
            CreateReversedStrings();
        }
        public override void CalculateSingle(float[] result)
        {
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = (float)Math.Cos(result[i]);
            }
        }
    }
    public class Tan : OperatorSingle
    {
        public Tan()
        {
            IsConnecter = false;
            PreFix = "tan";
            CreateReversedStrings();
        }
        public override void CalculateSingle(float[] result)
        {
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = (float)Math.Tan(result[i]);
            }
        }
    }
    public class ASin : OperatorSingle
    {
        public ASin()
        {
            IsConnecter = false;
            PreFix = "asin";
            CreateReversedStrings();
        }
        public override void CalculateSingle(float[] result)
        {
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = (float)Math.Asin(result[i]);
            }
        }
    }
    public class ACos : OperatorSingle
    {
        public ACos()
        {
            IsConnecter = false;
            PreFix = "acos";
            CreateReversedStrings();
        }
        public override void CalculateSingle(float[] result)
        {
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = (float)Math.Acos(result[i]);
            }
        }
    }
    public class ATan : OperatorSingle
    {
        public ATan()
        {
            IsConnecter = false;
            PreFix = "atan";
            CreateReversedStrings();
        }
        public override void CalculateSingle(float[] result)
        {
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = (float)Math.Atan(result[i]);
            }
        }
    }

    //Misc
    public class Parentheses : SimpleOperator
    {
        public Parentheses()
        {
            IsConnecter = false;
        }
        public override bool Calculate(float[] result, float[][] parameters, Operator oper)
        {
            Array.Copy(parameters[oper.ParameterIndex], oper.parenthesesResults, oper.parenthesesResults.Length);
            int OperatorsToCompressLeft = oper.NumberOfOperators;
            int OperatorToCompressIndex = 0;
            while (OperatorsToCompressLeft > 0)
            {
                if (oper.Operators[OperatorToCompressIndex] != null)
                {
                    OperatorsToCompressLeft--;
                    if (oper.Operators[OperatorToCompressIndex].Calculate(oper.parenthesesResults, parameters))
                    {
                        if (!Tools.IsANumber(oper.parenthesesResults))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                OperatorToCompressIndex++;
            }
            oper.ExtraMathFunction.CalculateFixedConnector(result, oper.parenthesesResults, oper.ResultOnRightSide);
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
            int AmountToAdd = Oper.Eq.Randomizer.Next(1, Oper.Eq.OperatorsLeft - 1);

            while (0 < Oper.Eq.OperatorsLeft - AmountToAdd)
            {
                Oper.AddOperator(Oper.Eq.OPStorage);
            }
            Oper.ExtraMathFunction = Oper.Eq.EInfo.Connectors[Oper.Eq.Randomizer.Next(0, Oper.Eq.EInfo.Connectors.Length)];
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
            int OperatorsToCompressLeft = Oper.NumberOfOperators;
            int OperatorToCompressIndex = 0;
            while (OperatorsToCompressLeft > 0)
            {
                if (Oper.Operators[OperatorToCompressIndex] != null)
                {
                    OperatorsToCompressLeft--;
                    Amount += Oper.Operators[OperatorToCompressIndex].GetOperatorCount();
                }
                OperatorToCompressIndex++;
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
                    Original.Operators[i].GetCopy(Copy.Eq.OPStorage.Pop(), Copy.Eq, Copy);
                }
            }
        }
        public override void StoreAndCleanup(Operator Oper)
        {
            Oper.Eq.Holders.Remove(Oper);
            Oper.ExtraMathFunction = null;
            // the Operators list is being altered in this loop so it can't be a foreach loop
            // that's why it's done this way
            int OperatorsToCompressLeft = Oper.NumberOfOperators;
            int OperatorToCompressIndex = 0;
            while (OperatorsToCompressLeft > 0)
            {
                if (Oper.Operators[OperatorToCompressIndex] != null)
                {
                    OperatorsToCompressLeft--;
                    Oper.Operators[OperatorToCompressIndex].StoreAndCleanup();
                }
                OperatorToCompressIndex++;
            }
        }
        public override void StoreAndCleanupAll(Operator Oper)
        {
            int OperatorsToCompressLeft = Oper.NumberOfOperators;
            int OperatorToCompressIndex = 0;
            while (OperatorsToCompressLeft > 0)
            {
                if (Oper.Operators[OperatorToCompressIndex] != null)
                {
                    OperatorsToCompressLeft--;
                    Oper.Operators[OperatorToCompressIndex].StoreAndCleanupAll();
                }
                OperatorToCompressIndex++;
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
        public override bool Calculate(float[] result, float[][] parameters, Operator oper)
        {
            oper.parenthesesResults.Fill(oper.RandomNumber);
            for (int i = 0; i < oper.Operators.Length; i++)
            {
                if (oper.Operators[i] != null)
                {
                    if (oper.Operators[i].Calculate(oper.parenthesesResults, parameters))
                    {
                        if (!Tools.IsANumber(oper.parenthesesResults))
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
            oper.ExtraMathFunction.CalculateFixedConnector(result, oper.parenthesesResults, oper.ResultOnRightSide);
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
    public sealed class Absolute : OperatorSingle
    {
        public Absolute()
        {
            IsConnecter = false;
            PreFix = "|";
            PostFix = "|";
            CreateReversedStrings();
        }
        public override void CalculateSingle(float[] result)
        {
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = Math.Abs(result[i]);
            }
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
        public override void CalculateConnector(float[] result, float[] parameter, bool useRandomNumber, float randomNumber, bool resultOnRightSide)
        {
            if (useRandomNumber)
            {
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = MathLogic.AND(result[i], randomNumber);
                }
            }
            else
            {
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = MathLogic.AND(result[i], parameter[i]);
                }
            }
        }
        public override void CalculateFixedConnector(float[] results, float[] scalars, bool resultOnRightSide)
        {
            for (int i = 0; i < results.Length; i++)
            {
                results[i] = MathLogic.AND(results[i], scalars[i]);
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
        public override void CalculateConnector(float[] result, float[] parameter, bool useRandomNumber, float randomNumber, bool resultOnRightSide)
        {
            if (useRandomNumber)
            {
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = MathLogic.NAND(result[i], randomNumber);
                }
            }
            else
            {
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = MathLogic.NAND(result[i], parameter[i]);
                }
            }
        }
        public override void CalculateFixedConnector(float[] results, float[] scalars, bool resultOnRightSide)
        {
            for (int i = 0; i < results.Length; i++)
            {
                results[i] = MathLogic.NAND(results[i], scalars[i]);
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
        public override void CalculateConnector(float[] result, float[] parameter, bool useRandomNumber, float randomNumber, bool resultOnRightSide)
        {
            if (useRandomNumber)
            {
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = MathLogic.OR(result[i], randomNumber);
                }
            }
            else
            {
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = MathLogic.OR(result[i], parameter[i]);
                }
            }
        }
        public override void CalculateFixedConnector(float[] results, float[] scalars, bool resultOnRightSide)
        {
            for (int i = 0; i < results.Length; i++)
            {
                results[i] = MathLogic.OR(results[i], scalars[i]);
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
        public override void CalculateConnector(float[] result, float[] parameter, bool useRandomNumber, float randomNumber, bool resultOnRightSide)
        {
            if (useRandomNumber)
            {
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = MathLogic.NOR(result[i], randomNumber);
                }
            }
            else
            {
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = MathLogic.NOR(result[i], parameter[i]);
                }
            }
        }
        public override void CalculateFixedConnector(float[] results, float[] scalars, bool resultOnRightSide)
        {
            for (int i = 0; i < results.Length; i++)
            {
                results[i] = MathLogic.NOR(results[i], scalars[i]);
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
        public override void CalculateConnector(float[] result, float[] parameter, bool useRandomNumber, float randomNumber, bool resultOnRightSide)
        {
            if (useRandomNumber)
            {
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = MathLogic.XOR(result[i], randomNumber);
                }
            }
            else
            {
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = MathLogic.XOR(result[i], parameter[i]);
                }
            }
        }
        public override void CalculateFixedConnector(float[] results, float[] scalars, bool resultOnRightSide)
        {
            for (int i = 0; i < results.Length; i++)
            {
                results[i] = MathLogic.XOR(results[i], scalars[i]);
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
        public override void CalculateConnector(float[] result, float[] parameter, bool useRandomNumber, float randomNumber, bool resultOnRightSide)
        {
            if (useRandomNumber)
            {
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = MathLogic.XNOR(result[i], randomNumber);
                }
            }
            else
            {
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = MathLogic.XNOR(result[i], parameter[i]);
                }
            }
        }
        public override void CalculateFixedConnector(float[] results, float[] scalars, bool resultOnRightSide)
        {
            for (int i = 0; i < results.Length; i++)
            {
                results[i] = MathLogic.XNOR(results[i], scalars[i]);
            }
        }
    }



    public sealed class NOT : OperatorSingle
    {
        public NOT()
        {
            IsConnecter = false;
            PreFix = " not ";
            CreateReversedStrings();
        }
        public override void CalculateSingle(float[] result)
        {
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = MathLogic.NOT(result[i]);
            }
        }
    }
}