using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Numerics;

namespace Solve_Funktion
{
    [Serializable]
    public class Equation
    {
        public List<List<Operator>> SortedOperators;
        public List<Operator> AllOperators;
        public List<Operator> EquationParts;
        public int OperatorsLeft
        {
            get
            {
                return EInfo.MaxSize - AllOperators.Count;
            }
        }
        public Stack<Operator> OPStorage;
        public readonly EvolutionInfo EInfo;
        public double OffSet;
        public double[] Results;

        public Equation(EvolutionInfo einfo)
        {
            EInfo = einfo;
            SortedOperators = new List<List<Operator>>(EInfo.MaxSize);
            AllOperators = new List<Operator>(EInfo.MaxSize);
            EquationParts = new List<Operator>(EInfo.MaxSize);
            OPStorage = new Stack<Operator>(EInfo.MaxSize);
            for (int i = 0; i < EInfo.MaxSize; i++)
            {
                OPStorage.Push(new Operator(this));
            }
            SortedOperators.Add(EquationParts);
            Results = new double[EInfo.GoalLength];
        }

        public void MakeRandom()
        {
            // the number is atleast 1, so there is actually something in the equation
            int AmountToAdd = SynchronizedRandom.Next(0, OperatorsLeft - 1);
            while (0 < OperatorsLeft - AmountToAdd)
            {
                Operator ToAdd = OPStorage.Pop();
                ToAdd.MakeRandom(EquationParts);
            }
        }

        public void CalcTotalOffSet()
        {
            CalcTotalOffSet(EInfo.GoalLength);
        }

        public void CalcTotalOffSet(int toCalc)
        {
            toCalc = (int)Math.Ceiling((double)toCalc / (double)Vector<double>.Count);
            double offset = 0;
            int index = 0;
            for (int i = 0; i < toCalc ; i++)
            {
                VectorPoint Coord = EInfo.Goal[i];
                Vector<double> FunctionResult = GetFunctionResult(Coord.X);
                offset += CalcOffset(FunctionResult, Coord.Y, Coord.Count);
                if (!Tools.IsANumber(offset))
                {
                    this.OffSet = double.NaN;
                    return;
                }
                double[] partResult = Tools.GetPartOfVectorResult(FunctionResult, Coord.Count);
                Array.Copy(partResult, 0, Results, index, partResult.Length);

                index += Coord.Count;
            }
            this.OffSet = offset;
        }

        private double CalcOffset(Vector<double> functionResult, Vector<double> coordY, int count)
        {
            //return (((y - y1) + 1) ^ 2) - 1
            Vector<double> diff = System.Numerics.Vector.Abs<double>(functionResult - coordY) + Vector<double>.One;
            Vector<double> vectorOffset = (diff * diff) - Vector<double>.One;
            if (count == Vector<double>.Count)
            {
                double[] vectorOffsets = new double[Vector<double>.Count];
                vectorOffset.CopyTo(vectorOffsets);
                return vectorOffsets.Sum();
            }
            else
            {
                return Tools.GetPartOfVectorResult(vectorOffset, count).Sum();
            }
        }

        private Vector<double> GetFunctionResult(Vector<double> x)
        {
            Vector<double> Result = x;
            foreach (Operator EquationPart in EquationParts)
            {
                Result = EquationPart.Calculate(Result, x);
                if (!Tools.IsANumber(Result))
                {
                    return Constants.NAN_VECTOR;
                }
            }
            return Result;
        }

        public string CreateFunction()
        {
            StringBuilder Forwards = new StringBuilder();
            StringBuilder Backwards = new StringBuilder();
            const string Variable = "x";
            Forwards.Append(Variable);
            foreach (Operator EquationPart in EquationParts)
            {
                EquationPart.ShowOperator(Variable, Forwards, Backwards);
            }
            StringBuilder Result = new StringBuilder(Backwards.Length + Forwards.Length);
            for (int i = Backwards.Length - 1; i >= 0; i--)
            {
                Result.Append(Backwards[i]);
            }
            Result.Append(Forwards.ToString());
            return Result.ToString();
        }

        public string[] GetFunctionResults()
        {
            string[] TextResults = new string[Results.Length];
            for (int i = 0; i < Results.Length; i++)
            {
                TextResults[i] = Results[i].ToString(Info.SRounding);
            }
            return TextResults;
        }

        public void Cleanup()
        {
            // the EquationParts list is being altered in this loop so it can't be a foreach loop
            // that's why it's done this way
            foreach (Operator Oper in EquationParts)
            {
                Oper.StoreAndCleaupAll();
            }
            EquationParts.Clear();
            AllOperators.Clear();
            SortedOperators.Clear();
            SortedOperators.Add(EquationParts);

#if DEBUG
            if (OperatorsLeft != EInfo.MaxSize)
            {
                System.Diagnostics.Debugger.Break();
            }
#endif
        }

        public Equation MakeClone(Equation Copy)
        {
            Copy.OffSet = OffSet;
            Array.Copy(Results, Copy.Results, Results.Length);
            foreach (Operator EPart in EquationParts)
            {
                //copys each operator and puts it in the new equation so their equation parts are identical
                //this should also copy the EInfo indirectly, by inserting everything into the SortedOperators list
                EPart.GetCopy(Copy.OPStorage.Pop(), Copy, Copy.EquationParts);
            }
            return Copy;
        }

        public void ChangeRandomOperator()
        {
            int Index = SynchronizedRandom.Next(0, AllOperators.Count);
            ChangeOperator(Index);
        }
        public void ChangeOperator(int Index)
        {
            Operator ToChange = AllOperators[Index];
            List<Operator> ContainedList = ToChange.ContainedList;

            ToChange.StoreAndCleanup();
            ToChange = OPStorage.Pop();
            ToChange.MakeRandom(ContainedList);
        }

        public void RemoveRandomOperator()
        {
            int Index = SynchronizedRandom.Next(0, AllOperators.Count);
            RemoveOperator(Index);
        }
        public void RemoveSingleOperator()
        {
            //there will always be atleast one operato that doesn't have any other operators
            Operator SingleOP = AllOperators.Single(x => x.GetOperatorCount() == 1);
            int Index = AllOperators.IndexOf(SingleOP);
            RemoveOperator(Index);
        }
        public void RemoveOperator(int Index)
        {
            AllOperators[Index].StoreAndCleanup();
        }
    }
}