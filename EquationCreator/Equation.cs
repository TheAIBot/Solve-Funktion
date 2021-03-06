﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EquationCreator
{
    public class Equation : OperatorHolder
    {
        /*
        operators can be located within other operators making it possible to have the equation split up by ()
        to keep track of this every single operator that is contained within a single () is kept in its own list of operators
        so it's possible to change a specific operator within another operator without changing or potentially deleting multiple operators.
        */
        public readonly List<OperatorHolder> Holders;
        public readonly Operator[] AllOperators; // a list of all operators currently in use by the equation
        public int NumberOfAllOperators = 0;
        public int OperatorsLeft
        {
            get
            {
                return EInfo.MaxSize - NumberOfAllOperators;
            }
        } // the amount of operators that isn't in use by the equation
        public readonly Stack<Operator> OPStorage; // stores all the operators when they are not used so they don't have to be remade all the time
        public readonly EvolutionInfo EInfo; // the parameters the equation has to work with in order to make the equation
        public float OffSet; // the total offset of the equation
        public readonly float[] Results; // is a list of all the calculated results returned by the equation
        public int _toCalc; // the amount of points that had been used to calcualte the offset
        public readonly SynchronizedRandom Randomizer;
        //public readonly string parameterNames;

        public Equation(EvolutionInfo einfo, SynchronizedRandom Rand) : base(einfo.MaxSize)
        {
            Randomizer = Rand;
            EInfo = einfo;
            Holders = new List<OperatorHolder>(EInfo.MaxSize);
            AllOperators = new Operator[EInfo.MaxSize];
            OPStorage = new Stack<Operator>(EInfo.MaxSize);
            for (int i = 0; i < EInfo.MaxSize; i++)
            {
                OPStorage.Push(new Operator(this));
            }
            Holders.Add(this);
            Results = new float[EInfo.coordInfo.expectedResults.Length];
        }

        /// <summary>
        /// crates a random equation with the operators available
        /// </summary>
        public void MakeRandom()
        {
            // the number is atleast 1, so there is actually something in the equation
            int AmountToAdd = Randomizer.Next(0, OperatorsLeft - 1);
            while (0 < OperatorsLeft - AmountToAdd)
            {
                AddOperator(OPStorage);
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
            Array.Copy(EInfo.coordInfo.parameters[0], Results, Results.Length);
            int OperatorsToCompressLeft = NumberOfOperators;
            int OperatorToCompressIndex = 0;
            while (OperatorsToCompressLeft > 0)
            {
                if (Operators[OperatorToCompressIndex] != null)
                {
                    OperatorsToCompressLeft--;
                    if (Operators[OperatorToCompressIndex].Calculate(Results, EInfo.coordInfo.parameters))
                    {
                        if (!Tools.IsANumber(Results))
                        {
                            OffSet = float.NaN;
                            return;
                        }
                    }
                    else
                    {
                        OffSet = float.NaN;
                        return;
                    }
                }
                OperatorToCompressIndex++;
            }
            OffSet = CalcOffset(Results, EInfo.coordInfo.expectedResults);
        }

        /// <summary>
        /// calculates the offset of an array
        /// </summary>
        /// <param name="functionResult">results</param>
        /// <param name="coordY"> expected result</param>
        /// <returns></returns>
        private float CalcOffset(float[] functionResult, float[] coordY)
        {
            float offset = 0;
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
            for (int i = 0; i < Operators.Length; i++) // can optimize this to only run untill all operators have been cleaned
            {
                if (Operators[i] != null)
                {
                    Operators[i].ShowOperator(Forwards, Backwards);
                }
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
            for (int i = 0; i < Operators.Length; i++) // can optimize this to only run untill all operators have been cleaned
            {
                if (Operators[i] != null)
                {
                    Operators[i].StoreAndCleanupAll();
                }
            }
            //EquationParts.Fill(null);
            AllOperators.Fill(null);
            NumberOfAllOperators = 0;
            Holders.Clear();
            Holders.Add(this);

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
            for (int i = 0; i < Operators.Length; i++)
            {
                if (Operators[i] != null)
                {
                    //copys each operator and puts it in the new equation so their equation parts are identical
                    //this should also copy the EInfo indirectly, by inserting everything into the SortedOperators list
                    Operators[i].GetCopy(Copy.OPStorage.Pop(), Copy, Copy);
                }
            }
            return Copy;
        }

        public int InsertOperator(Equation Cand)
        {
            int WhereToAdd = Randomizer.Next(0, Cand.Holders.Count);
            Operator[] LLOper = Cand.Holders[WhereToAdd].Operators;
            int WhereToAddOP = Randomizer.Next(0, LLOper.Length);
            if (LLOper[WhereToAddOP] != null)
            {
                MakeSpaceForOperator(LLOper, WhereToAddOP);
            }
            Operator addedOperator = Cand.Holders[WhereToAdd].AddOperator(OPStorage, WhereToAddOP);
            return addedOperator.GetOperatorCount();
        }

        public Operator AddOperator(bool OResultOmRightSide, MathFunction OMFunction, int OParameterIndex, float ORandomNumber, bool OUseRandomNumber, Connector OExtraMathFunction, OperatorHolder OHolder)
        {
            Operator toAdd = OPStorage.Pop();
            toAdd.SetupOperator(OResultOmRightSide, OMFunction, OParameterIndex, ORandomNumber, OUseRandomNumber, NumberOfOperators, OExtraMathFunction, OHolder);
            return toAdd;
        }

        private void MakeSpaceForOperator(Operator[] makeSpaceIn, int indexForSpace)
        {
            const int NO_SPACE_FOUND = -1;


            int forwardFirstSpaceIndex = NO_SPACE_FOUND;
            for (int i = indexForSpace + 1; i < makeSpaceIn.Length; i++)
            {
                if (makeSpaceIn[i] == null)
                {
                    forwardFirstSpaceIndex = i;
                    break;
                }
            }
            int forwardDistance = (forwardFirstSpaceIndex == NO_SPACE_FOUND) ? NO_SPACE_FOUND : forwardFirstSpaceIndex - indexForSpace;


            int backwardsFirstSpaceIndex = NO_SPACE_FOUND;
            for (int i = indexForSpace - 1; i >= 0; i--) // optimize with the distance from forward
            {
                if (makeSpaceIn[i] == null)
                {
                    backwardsFirstSpaceIndex = i;
                    break;
                }
            }
            int backwardsDistance = (backwardsFirstSpaceIndex == NO_SPACE_FOUND) ? NO_SPACE_FOUND : indexForSpace - backwardsFirstSpaceIndex;


            if (forwardDistance != NO_SPACE_FOUND && 
                backwardsDistance != NO_SPACE_FOUND && 
                forwardDistance < backwardsDistance ||
                forwardDistance != NO_SPACE_FOUND &&
                backwardsDistance == NO_SPACE_FOUND)
            {
                //make space forward

                for (int i = forwardFirstSpaceIndex; i > indexForSpace; i--)
                {
                    makeSpaceIn[i] = makeSpaceIn[i - 1];
                    makeSpaceIn[i - 1] = null; //not needed but makes it easier to debug
                    makeSpaceIn[i].ContainedIndex++;
                }
            }
            else
            {
                //make space backwards

                for (int i = backwardsFirstSpaceIndex; i < indexForSpace; i++)
                {
                    makeSpaceIn[i] = makeSpaceIn[i + 1];
                    makeSpaceIn[i + 1] = null; //not needed but makes it easier to debug
                    makeSpaceIn[i].ContainedIndex--;
                }
            }
        }
        
        public int ChangeRandomOperator(int maxChangedOperators)
        {
            int index;
            int changedOperatorCount;
            do
            {
                index = GetRandomOperatorIndexFromAllOperators();
                changedOperatorCount = AllOperators[index].GetOperatorCount();
            } while (changedOperatorCount > maxChangedOperators);
            ChangeOperator(index);
            return changedOperatorCount;
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
        public int RemoveRandomOperator(int maxRemovedOperators)
        {
            int index;
            int removedOperatorCount;
            do
            {
                index = GetRandomOperatorIndexFromAllOperators();
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
            int Index = Array.IndexOf(AllOperators, SingleOP);
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

        public override void OperatorChanged()
        {
            
        }

        public int AddOperatorToAlloperators(Operator oper)
        {
            int insertionIndex = (AllOperators[NumberOfAllOperators] == null) ? NumberOfAllOperators : Tools.GetFirstFreeIndex(AllOperators);
            AddOperatorToAlloperators(oper, insertionIndex);
            return insertionIndex;
        }

        public void AddOperatorToAlloperators(Operator oper, int insertionIndex)
        {
            AllOperators[insertionIndex] = oper;
            NumberOfAllOperators++;
        }

        

        public void RemoveOperatorFromAllOperators(int index)
        {
            AllOperators[index] = null;
            NumberOfAllOperators--;
        }

        public int GetRandomOperatorIndexFromAllOperators()
        {
            int index;
            do
            {
                index = Randomizer.Next(0, AllOperators.Length);
            } while (AllOperators[index] == null);
            return index;
        }

        public void CompressEquation()
        {
            if (NumberOfAllOperators > 0 && NumberOfAllOperators < EInfo.MaxSize)
            {
                Tools.CompressOperatorArray(AllOperators, NumberOfAllOperators, this, false);
            }
            Compress(this);
        }
    }
}