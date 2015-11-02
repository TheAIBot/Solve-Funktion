using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Numerics;

namespace Solve_Funktion
{
    public class Equation
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
            Results = new double[EInfo.GoalLength];
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
                ToAdd.MakeRandom(EquationParts);
            }
        }

        /// <summary>
        /// calculates the total offset using all the points available
        /// </summary>
        public void CalcTotalOffSet()
        {
            CalcPartialOffSet(EInfo.GoalLength);
        }

        /// <summary>
        /// calculates a part of the offset using some of the points available defined by toCalc
        /// </summary>
        /// <param name="toCalc">the amount of points to use to calculate the offset</param>
        public void CalcPartialOffSet(int toCalc)
        {
            _toCalc = toCalc;
            toCalc = (int)Math.Ceiling((double)toCalc / (double)Constants.VECTOR_LENGTH);
            double offset = 0;
            int index = 0;
            for (int i = 0; i < toCalc ; i++)
            {
                VectorPoint Coord = EInfo.Goal[i];
                Vector<double> FunctionResult = GetFunctionResult(Coord.Parameters);
                double[] partResult = Tools.GetPartOfVectorResult(FunctionResult, Coord.Count);
                offset += CalcOffset(partResult, Coord.Result, Coord.Count);
                if (!Tools.IsANumber(offset))
                {
                    this.OffSet = double.NaN;
                    return;
                }
                Array.Copy(partResult, 0, Results, index, partResult.Length);

                index += Coord.Count;
            }
            this.OffSet = offset;
        }

        /// <summary>
        /// calculates the offset of a vector
        /// </summary>
        /// <param name="functionResult">vector result</param>
        /// <param name="coordY"> expected result</param>
        /// <param name="count">amount of vector results to use</param>
        /// <returns></returns>
        private double CalcOffset(double[] functionResult, Vector<double> coordY, int count)
        {
            double offset = 0;
            for (int i = 0; i < count; i++)
            {
                double fResult = functionResult[i];
                if (fResult == 0)
                {
                    offset++;
                }
                else if (fResult < coordY[i])
                {
                    offset += (1 - (fResult / coordY[i]));
                }
                else
                {
                    offset += (1 - (coordY[i] / fResult));
                }
            }
            return offset;
        }

        /// <summary>
        /// gets the result of the equation for a given vector of x values
        /// </summary>
        /// <param name="x">x vaules as a vector</param>
        /// <returns>equation result</returns>
        private Vector<double> GetFunctionResult(Vector<double>[] parameters)
        {
            Vector<double> Result = parameters[0];
            foreach (Operator EquationPart in EquationParts)
            {
                Result = EquationPart.Calculate(Result, parameters);
                if (!Tools.IsANumber(Result))
                {
                    return Constants.NAN_VECTOR;
                }
            }
            return Result;
        }

        /// <summary>
        /// builds the equation as a string
        /// </summary>
        /// <returns>the equation as a string</returns>
        public string CreateFunction()
        {
            StringBuilder Forwards = new StringBuilder();
            StringBuilder Backwards = new StringBuilder();
            Forwards.Append(EInfo.Goal[0].ParameterNames[0]);
            foreach (Operator EquationPart in EquationParts)
            {
                EquationPart.ShowOperator(Forwards, Backwards);
            }
            StringBuilder Result = new StringBuilder(Backwards.Length + Forwards.Length);
            for (int i = Backwards.Length - 1; i >= 0; i--)
            {
                Result.Append(Backwards[i]);
            }
            Result.Append(Forwards.ToString());
            return Result.ToString();
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
            Array.Copy(Results, Copy.Results, Results.Length);
            foreach (Operator EPart in EquationParts)
            {
                //copys each operator and puts it in the new equation so their equation parts are identical
                //this should also copy the EInfo indirectly, by inserting everything into the SortedOperators list
                EPart.GetCopy(Copy.OPStorage.Pop(), Copy, Copy.EquationParts);
            }
            return Copy;
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
            List<Operator> ContainedList = ToChange.ContainedList;

            ToChange.StoreAndCleanup();
            ToChange = OPStorage.Pop();
            ToChange.MakeRandom(ContainedList);
        }

        /// <summary>
        /// removes a random operator
        /// </summary>
        public void RemoveRandomOperator()
        {
            int Index = SynchronizedRandom.Next(0, AllOperators.Count);
            RemoveOperator(Index);
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
            AllOperators[Index].StoreAndCleanup();
        }
    }
}