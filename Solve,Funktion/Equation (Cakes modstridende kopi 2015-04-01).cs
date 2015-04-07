using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using System.Threading;

namespace Solve_Funktion
{
    [Serializable]
    public class Equation
    {
        public List<List<Operator>> SortedOperators = new List<List<Operator>>(Info.MaxSize);
        public List<Operator> AllOperators = new List<Operator>(Info.MaxSize);
        public int OperatorsLeft
        {
            get
            {
                return Info.MaxSize - AllOperators.Count;
                //return Info.MaxSize - SortedOperators.Sum(x => x.Count);
                //return AllOperators[0].Count;
            }
        }
        public Stack<Operator> OPStorage;
        public List<Operator> EquationParts = new List<Operator>(Info.MaxSize);
        public double OffSet;

        public Equation(ref Stack<Operator> opstorage)
        {
            this.OPStorage = opstorage;
            SortedOperators.Add(EquationParts);
        }

        public void MakeRandom()
        {
            // the number is atleast 1, so there is actually something in the equation
            int AmountToAdd = SynchronizedRandom.Next(0, OperatorsLeft - 1);
            while (0 < OperatorsLeft - AmountToAdd)
            {
                Operator ToAdd = OPStorage.Pop();
                ToAdd.MakeRandom(this, EquationParts);
            }
        }

        public void CalcOffSet()
        {
            double offset = 0;
            foreach (Point Coord in Info.Seq)
            {
                double FunctionResult = GetFunctionResult(Coord.X);
                if (!Tools.IsANumber(FunctionResult))
                {
                    this.OffSet = double.NaN;
                    return;
                }
                //offset += Math.Abs(Math.Abs(FunctionResult - Coord.Y) / Coord.Y);
                offset += Math.Abs(FunctionResult - Coord.Y);
            }
            this.OffSet = (offset / (double)Info.Seq.Count);
        }

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
            const string Variable = "x";
            string Result = Variable;
            foreach (Operator EquationPart in EquationParts)
            {
                Result = EquationPart.ShowOperator(Result, Variable);
            }
            return Result;
        }

        public List<string> GetFunctionResults()
        {
            List<string> Results = new List<string>();
            for (int i = 0; i < Info.Seq.Count; i++)
            {
                Results.Add(GetFunctionResult(Info.Seq[i].X).ToString("N"+Info.Rounding.ToString()));
            }
            return Results;
        }

        public void StoreAndCleanup()
        {
            // the EquationParts list is being altered in this loop so it can't be a foreach loop
            // that's why it's done this way
            while (EquationParts.Count > 0)
            {
                EquationParts.First().StoreAndCleanup();
            }
#if DEBUG
            if (OperatorsLeft != Info.MaxSize)
            {
                System.Diagnostics.Debugger.Break();
            }
#endif
        }

        public Equation GetClone(Equation Copy)
        {
            Copy.OffSet = OffSet;
            foreach (Operator EPart in EquationParts)
            {
                //copys each operator and puts it in the new equation so their equation parts are identical
                //this should also copy the EInfo indirectly, by inserting everything into the SortedOperators list
                EPart.GetCopy(OPStorage.Pop(), Copy, Copy.EquationParts);
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
            Operator ToChange = AllOperators.ElementAt(Index);
            List<Operator> ContainedList = ToChange.ContainedList;

            ToChange.StoreAndCleanup();
            ToChange = OPStorage.Pop();
            ToChange.MakeRandom(this, ContainedList);
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
            AllOperators.ElementAt(Index).StoreAndCleanup();
        }
    }
}
