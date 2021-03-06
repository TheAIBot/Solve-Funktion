﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquationCreator
{
    public class Operator : OperatorHolder
    {
        public const int NONE_CALCULATED = -1;

        public bool ResultOnRightSide;
        public MathFunction MFunction;
        public int ParameterIndex;
        public float RandomNumber;
        public bool UseRandomNumber;
        public int ContainedIndex;
        public Connector ExtraMathFunction;
        public int MaxCalculated = NONE_CALCULATED;
        public OperatorHolder Holder;
        public readonly Equation Eq;
        public int AllOperatorsContainedIndex;
        public readonly float[] parenthesesResults;

        //public readonly Vector<double>[] OperatorResults;

        public Operator(Equation OEq) : base(OEq.EInfo.MaxSize)
        {
            Eq = OEq;
            parenthesesResults = new float[OEq.EInfo.coordInfo.expectedResults.Length];
        }

        public void MakeRandom(OperatorHolder OHolder, int CIndex)
        {
            Holder = OHolder;
            ContainedIndex = CIndex;
            Holder.AddOperatorToHolder(this, ContainedIndex);
            AllOperatorsContainedIndex = Eq.AddOperatorToAlloperators(this);
            
            ChangeOperator();
        }

        public void ChangeOperator()
        {
            ResultOnRightSide = Eq.Randomizer.RandomBool();
            do
            {
                MFunction = Eq.EInfo.Operators[Eq.Randomizer.Next(0, Eq.EInfo.Operators.Length)];
                // should never be an infinete loop because this function should only be called when there is 1 or more operators left
                // and there should always be an operator that doesn't need a min of operators
            } while (!MFunction.CanUseOperator(this));
            UseRandomNumber = Eq.Randomizer.RandomBool();
            RandomNumber = Eq.Randomizer.Next(Eq.EInfo.NumberRangeMin, Eq.EInfo.NumberRangeMax);
            ParameterIndex = Eq.Randomizer.Next(0, Eq.EInfo.coordInfo.parameters.Length);
            MFunction.MakeRandom(this);
        }

        /// <summary>
        /// calcualtes the next step in an equation using this operator
        /// </summary>
        /// <param name="Result">number returned by previous operators or initial number from equation</param>
        /// <param name="x"> value of x</param>
        /// <returns>result of Result and this operator</returns>
        public bool Calculate(float[] Result, float[][] parameters)
        {
            //if (MaxCalculated >= Index)
            //{
            //    return OperatorResults[Index];
            //}
            //else
            //{
                //MaxCalculated = Index;
                return MFunction.Calculate(Result, parameters, this);
            //}
        }

        /// <summary>
        /// adds this operator to the currently created equation
        /// </summary>
        /// <param name="x"></param>
        /// <param name="Forwards"></param>
        /// <param name="Backwards"></param>
        public void ShowOperator(StringBuilder Forwards, StringBuilder Backwards)
        {
            MFunction.ShowOperator(this, Forwards, Backwards);
        }

        /// <summary>
        /// gets the amount of operators in use by this operator including itself
        /// </summary>
        /// <returns>the amount of operators in use by this operator including itself</returns>
        public int GetOperatorCount()
        {
            return MFunction.GetOperatorCount(this);
        }

        /// <summary>
        /// resets the operator so it can be used to make a new random operator
        /// </summary>
        public void StoreAndCleanup()
        {
            Eq.OPStorage.Push(this);
            Eq.RemoveOperatorFromAllOperators(AllOperatorsContainedIndex);

            MFunction.StoreAndCleanup(this);
            MFunction = null;

            Holder.RemoveOperatorFromHolder(ContainedIndex);
            Holder = null;

            ContainedIndex = -1;
            AllOperatorsContainedIndex = -1;
            ResetMaxCalculated();
            //NumberOfOperators = 0;
        }

        public void ChangeCleanup()
        {
            MFunction.StoreAndCleanup(this);
            // these actions might not be required yet but if there is any errors then it will be easier to spot if a null exception is thrown
            MFunction = null;
            //Holder = null;
            //ContainedIndex = -1;
            ResetMaxCalculated();
            //NumberOfOperators = 0;
        }

        /// <summary>
        /// faster way of resetting the operator when all operatos in an equation are reset at once
        /// </summary>
        public void StoreAndCleanupAll()
        {
            Eq.OPStorage.Push(this);

            MFunction.StoreAndCleanupAll(this);
            MFunction = null;

            Holder.RemoveOperatorFromHolder(ContainedIndex);
            Holder = null;
            // these actions might not be required yet but if there is any errors then it will be easier to spot if a null exception is thrown
            ContainedIndex = -1;
            AllOperatorsContainedIndex = -1;
            ResetMaxCalculated();
            //NumberOfOperators = 0;
        }

        public void ResetMaxCalculated()
        {
            MaxCalculated = NONE_CALCULATED;
        }

        /// <summary>
        /// Copys itself into a new OP Can only be called from within Equations copy method
        /// </summary>
        /// <param name="Copy">The OP to copy into</param>
        /// <param name="CopyEInfo">The Equationinfo the copy should use</param>
        /// <returns></returns>
        public Operator GetCopy(Operator Copy, Equation CopyEq, OperatorHolder CopyHolder)
        {
            Copy.ResultOnRightSide = ResultOnRightSide;
            Copy.MFunction = MFunction;
            Copy.ParameterIndex = ParameterIndex;
            Copy.RandomNumber = RandomNumber;
            Copy.UseRandomNumber = UseRandomNumber;
            //Copy.AllOperatorsContainedIndex = AllOperatorsContainedIndex;
            Copy.AllOperatorsContainedIndex = Tools.GetFirstFreeIndex(Copy.Eq.AllOperators);
            Copy.Eq.AddOperatorToAlloperators(Copy, Copy.AllOperatorsContainedIndex);
            Copy.ContainedIndex = ContainedIndex;
            //Copy.ContainedList[ContainedIndex] = Copy;
            
            Copy.Holder = CopyHolder;
            Copy.Holder.AddOperatorToHolder(Copy, Copy.ContainedIndex);
            Copy.MFunction.GetCopy(this, Copy);
            Copy.MaxCalculated = MaxCalculated;
            return Copy;
        }

        public override string ToString()
        {
            return MFunction.ToString();
        }

        public override void OperatorChanged()
        {
            //if (/*maxCalculated != NONE_CALCULATED*/)
            //{
                //MFunction.OperatorChanged(this);
            //}
        }

        public Operator AddOperator(bool OResultOmRightSide, MathFunction OMFunction, int OParameterIndex, float ORandomNumber, bool OUseRandomNumber, Connector OExtraMathFunction, OperatorHolder OHolder)
        {
            Operator toAdd = Eq.OPStorage.Pop();
            toAdd.SetupOperator(OResultOmRightSide, OMFunction, OParameterIndex, ORandomNumber, OUseRandomNumber, NumberOfOperators, OExtraMathFunction, OHolder);
            return toAdd;
        }

        public void SetupOperator(bool OResultOmRightSide, MathFunction OMFunction, int OParameterIndex, float ORandomNumber, bool OUseRandomNumber, int CIndex, Connector OExtraMathFunction, OperatorHolder OHolder)
        {
            ResultOnRightSide = OResultOmRightSide;
            MFunction = OMFunction;
            ParameterIndex = OParameterIndex;
            RandomNumber = ORandomNumber;
            UseRandomNumber = OUseRandomNumber;
            ExtraMathFunction = OExtraMathFunction;
            Holder = OHolder;
            Holder.AddOperatorToHolder(this);

            AllOperatorsContainedIndex = Eq.AddOperatorToAlloperators(this);
            ContainedIndex = CIndex;
        }
    }
}
