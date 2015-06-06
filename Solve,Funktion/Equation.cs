using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

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
        public EvolutionInfo EInfo;
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
            Results = new double[EInfo.Goal.Length];
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
            double offset = 0;
            int Index = 0;
            foreach (Point Coord in EInfo.Goal)
            {
                double FunctionResult = GetFunctionResult(Coord.X);
                offset += Math.Pow((Math.Abs(FunctionResult - Coord.Y) + 1), 2) - 1;
                if (!Tools.IsANumber(offset))
                {
                    this.OffSet = double.NaN;
                    return;
                }
                Results[Index] = FunctionResult;
                Index++;
            }
            this.OffSet = Math.Abs((offset / (double)EInfo.Goal.Length));
        }

        //private void SetResults(int SeqLength)
        //{
        //    //if it's not set or if the length is not correct then
        //    //a new array is created with the correct length
        //    if (Results == null)
        //    {
        //        Results = new double[SeqLength];
        //    }
        //    else if (Results.Length != SeqLength)
        //    {
        //        Results = new double[SeqLength]; ;
        //    }
        //}
        
        private double GetFunctionResult(double x)
        {
            double Result = x;
            foreach (Operator EquationPart in EquationParts)
            {
                Result = EquationPart.Calculate(Result, x);
                if (!Tools.IsANumber(Result))
                {
                    return double.NaN;
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
            string[] TextResults = new string[EInfo.Goal.Length];
            for (int i = 0; i < EInfo.Goal.Length; i++)
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