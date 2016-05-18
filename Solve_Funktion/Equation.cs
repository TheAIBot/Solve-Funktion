using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace Solve_Funktion
{
    public class Equation : OperatorHolder
    {
        /*
        operators can be located within other operators making it possible to have the equation split up by ()
        to keep track of this every single operator that is contained within a single () is kept in its own list of operators
        so it's possible to change a specific operator within another operator without changing or potentially deleting multiple operators.
        */
        public List<List<Operator>> SortedOperators;
        public List<Operator> AllOperators; // a list of all operators currently in use by the equation
        public List<Operator> EquationParts; // the base operators that has to be computed first to allow the operators in the next layer to be calculated
        public int OperatorsLeft
        {
            get
            {
                return EInfo.MaxSize - AllOperators.Count;
            }
        } // the amount of operators that isn't in use by the equation
        public Stack<Operator> OPStorage; // stores all the operators when they are not used so they don't have to be remade all the time
        public readonly EvolutionInfo EInfo; // the parameters the equation has to work with in order to make the equation
        public double OffSet; // the total offset of the equation
        public double[] Results; // is a list of all the calculated results returned by the equation
        public int _toCalc; // the amount of points that had been used to calcualte the offset
        //public readonly string parameterNames;

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
            Results = new double[EInfo.coordInfo.expectedResults.Length];
        }

        /// <summary>
        /// crates a random equation with the operators available
        /// </summary>
        public void MakeRandom()
        {
            // the number is atleast 1, so there is actually something in the equation
            int AmountToAdd = SynchronizedRandom.Next(0, OperatorsLeft - 1);
            while (0 < OperatorsLeft - AmountToAdd)
            {
                Operator ToAdd = OPStorage.Pop();
                ToAdd.MakeRandom(EquationParts, this);
            }
        }

        /// <summary>
        /// calculates the total offset using all the points available
        /// </summary>
        public void CalcTotalOffSet()
        {
            CalcPartialOffSet(Results.Length);
        }

        /// <summary>
        /// calculates a part of the offset using some of the points available defined by toCalc
        /// </summary>
        /// <param name="toCalc">the amount of points to use to calculate the offset</param>
        public void CalcPartialOffSet(int toCalc)
        {
            _toCalc = toCalc;
            Results.Fill(EInfo.coordInfo.parameterNames[0][0]);

            foreach (Operator oper in EquationParts)
            {
                if (oper.Calculate(Results, EInfo.coordInfo.parameters))
                {
                    if (!Tools.IsANumber(Results))
                    {
                        OffSet = double.NaN;
                        return;
                    }
                }
                else
                {
                    OffSet = double.NaN;
                    return;
                }
            }
            OffSet = CalcOffset(Results, EInfo.coordInfo.expectedResults);
        }

        /// <summary>
        /// calculates the offset of an array
        /// </summary>
        /// <param name="functionResult">results</param>
        /// <param name="coordY"> expected result</param>
        /// <returns></returns>
        private double CalcOffset(double[] functionResult, double[] coordY)
        {
            double offset = 0;
            for (int i = 0; i < functionResult.Length; i++)
            {
                if (coordY[i] == 0)
                {
                    offset += Math.Abs(functionResult[i]);
                }
                else
                {
                    offset += Math.Abs(Math.Abs(coordY[i] - functionResult[i]) / coordY[i]);
                }
            }
            return offset;
        }

        /// <summary>
        /// builds the equation as a string
        /// </summary>
        /// <returns>the equation as a string</returns>
        public string CreateFunction()
        {
            StringBuilder Forwards = new StringBuilder();
            StringBuilder Backwards = new StringBuilder();
            Forwards.Append(EInfo.coordInfo.parameterNames[0]);
            foreach (Operator EquationPart in EquationParts)
            {
                EquationPart.ShowOperator(Forwards, Backwards);
            }
            string Result = Tools.ReverseAddStringBuilder(Backwards, Forwards);
            return "f(" + String.Join(", ", EInfo.coordInfo.parameterNames) +") = " + Result;
        }

        /// <summary>
        /// makes a string with all the y values calculated by the equation
        /// </summary>
        /// <returns>y values</returns>
        public string[] GetFunctionResults()
        {
            string[] TextResults = new string[Results.Length];
            for (int i = 0; i < Results.Length; i++)
            {
                TextResults[i] = Results[i].ToString(Info.SRounding);
            }
            return TextResults;
        }

        /// <summary>
        /// Resets the equation so it can be reused to make another equation
        /// </summary>
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

        /// <summary>
        /// clones the current equation
        /// </summary>
        /// <param name="Copy">equation to copy this equation to</param>
        /// <returns>a clone of the current equation</returns>
        public Equation MakeClone(Equation Copy)
        {
            Copy.OffSet = OffSet;
            Copy._toCalc = _toCalc;
            Array.Copy(Results, Copy.Results, Results.Length);
            foreach (Operator EPart in EquationParts)
            {
                //copys each operator and puts it in the new equation so their equation parts are identical
                //this should also copy the EInfo indirectly, by inserting everything into the SortedOperators list
                EPart.GetCopy(Copy.OPStorage.Pop(), Copy, Copy.EquationParts, Copy);
            }
            return Copy;
        }

        public int ChangeRandomOperator(int maxChangedOperators)
        {
            int index;
            int changedOperatorCount;
            do
            {
                index = SynchronizedRandom.Next(0, AllOperators.Count);
                changedOperatorCount = AllOperators[index].GetOperatorCount();
            } while (changedOperatorCount > maxChangedOperators);
            ChangeOperator(index);
            return changedOperatorCount;
        }
        /// <summary>
        /// changes a random operator
        /// </summary>
        public void ChangeRandomOperator()
        {
            int Index = SynchronizedRandom.Next(0, AllOperators.Count);
            ChangeOperator(Index);
        }
        /// <summary>
        /// changes a specific operator
        /// </summary>
        /// <param name="Index">index of the operator to change in AllOperators</param>
        public void ChangeOperator(int Index)
        {
            Operator ToChange = AllOperators[Index];
            ToChange.ChangeCleanup();
            ToChange.ChangeOperator();
            ToChange.OperatorChanged();
        }


        /// <summary>
        /// removes a random operator
        /// </summary>
        public void RemoveRandomOperator()
        {
            int Index = SynchronizedRandom.Next(0, AllOperators.Count);
            RemoveOperator(Index);
        }
        public int RemoveRandomOperator(int maxRemovedOperators)
        {
            int index;
            int removedOperatorCount;
            do
            {
                index = SynchronizedRandom.Next(0, AllOperators.Count);
                removedOperatorCount = AllOperators[index].GetOperatorCount();
            } while (removedOperatorCount > maxRemovedOperators);
            RemoveOperator(index);
            return removedOperatorCount;
        }
        /// <summary>
        /// makes sure only 1 operator is removed
        /// </summary>
        public void RemoveSingleOperator()
        {
            //there will always be atleast one operato that doesn't have any other operators
            Operator SingleOP = AllOperators.Single(x => x.GetOperatorCount() == 1);
            int Index = AllOperators.IndexOf(SingleOP);
            RemoveOperator(Index);
        }
        /// <summary>
        /// removes a specific operator
        /// </summary>
        /// <param name="Index">index of operator to remove in AllOperators</param>
        public void RemoveOperator(int Index)
        {
            AllOperators[Index].OperatorChanged();
            AllOperators[Index].StoreAndCleanup();
        }

        public void OperatorChanged()
        {
            
        }
    }
}