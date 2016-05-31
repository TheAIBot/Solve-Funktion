using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solve_Funktion
{
    public class Operator : OperatorHolder
    {
        public const int NONE_CALCULATED = -1;

        public bool ResultOnRightSide;
        public MathFunction MFunction;
        public int ParameterIndex;
        public double RandomNumber;
        public bool UseRandomNumber;
        public int ContainedIndex;
        public Operator[] ContainedList;
        public Connector ExtraMathFunction;
        public int MaxCalculated = NONE_CALCULATED;
        public OperatorHolder Holder;
        public readonly Equation Eq;
        public int NumberOfOperators = 0;
        public readonly Operator[] Operators;
        //public readonly Vector<double>[] OperatorResults;

        public Operator(Equation OEq)
        {
            Eq = OEq;
            Operators = new Operator[Eq.EInfo.MaxSize];
        }

        public void MakeRandom(Operator[] OContainedList, OperatorHolder OHolder, int CIndex)
        {
            Holder = OHolder;
            ContainedList = OContainedList;
            Eq.AllOperators.Add(this);
            ContainedIndex = CIndex;
            Holder.AddOperatorToHolder(this, ContainedIndex);
            ChangeOperator();
        }

        public void ChangeOperator()
        {
            ResultOnRightSide = SynchronizedRandom.RandomBool();
            do
            {
                MFunction = Eq.EInfo.Operators[SynchronizedRandom.Next(0, Eq.EInfo.Operators.Length)];
                // should never be an infinete loop because this function should only be called when there is 1 or more operators left
                // and there should always be an operator that doesn't need a min of operators
            } while (!MFunction.CanUseOperator(this));
            UseRandomNumber = SynchronizedRandom.RandomBool();
            RandomNumber = SynchronizedRandom.Next(Eq.EInfo.NumberRangeMin, Eq.EInfo.NumberRangeMax);
            ParameterIndex = SynchronizedRandom.Next(0, Eq.EInfo.coordInfo.parameters.Length);
            MFunction.MakeRandom(this);
        }

        /// <summary>
        /// calcualtes the next step in an equation using this operator
        /// </summary>
        /// <param name="Result">number returned by previous operators or initial number from equation</param>
        /// <param name="x"> value of x</param>
        /// <returns>result of Result and this operator</returns>
        public bool Calculate(double[] Result, double[][] parameters)
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
            Eq.AllOperators.Remove(this);

            MFunction.StoreAndCleanup(this);
            MFunction = null;

            Holder.RemoveOperatorFromHolder(ContainedIndex);
            Holder = null;

            ContainedIndex = -1;
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
            ResetMaxCalculated();
            NumberOfOperators = 0;
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
        public Operator GetCopy(Operator Copy, Equation CopyEq, Operator[] CopyContainedList, OperatorHolder CopyHolder)
        {
            Copy.ResultOnRightSide = ResultOnRightSide;
            Copy.MFunction = MFunction;
            Copy.ParameterIndex = ParameterIndex;
            Copy.RandomNumber = RandomNumber; // this might be wrong
            Copy.UseRandomNumber = UseRandomNumber;
            Copy.Eq.AllOperators.Add(Copy);
            Copy.ContainedIndex = ContainedIndex;
            Copy.ContainedList = CopyContainedList;
            Copy.ContainedList[ContainedIndex] = Copy;
            Copy.MFunction.GetCopy(this, Copy);
            Copy.Holder = CopyHolder;
            Copy.MaxCalculated = MaxCalculated;
            Copy.NumberOfOperators = NumberOfOperators;
            return Copy;
        }

        public override string ToString()
        {
            return MFunction.ToString();
        }

        public void OperatorChanged()
        {
            //if (/*maxCalculated != NONE_CALCULATED*/)
            //{
                //MFunction.OperatorChanged(this);
            //}
        }

        public void AddOperator()
        {
            Operator ToAdd = Eq.OPStorage.Pop();
            ToAdd.MakeRandom(Operators, this, GetFirstFreeIndex());
        }

        private int GetFirstFreeIndex()
        {
            if (Operators[NumberOfOperators] == null)
            {
                return NumberOfOperators;
            }
            for (int i = 0; i < Operators.Length; i++)
            {
                if (Operators[i] == null)
                {
                    return i;
                }
            }
            throw new Exception("no free space for operator");
        }

        public void AddOperatorToHolder(Operator oper, int index)
        {
            Operators[index] = oper;
            NumberOfOperators++;
        }

        public void RemoveOperatorFromHolder(int index)
        {
            Operators[index] = null;
            NumberOfOperators--;
        }

        public Operator AddOperator(bool OResultOmRightSide, MathFunction OMFunction, int OParameterIndex, double ORandomNumber, bool OUseRandomNumber, Connector OExtraMathFunction, OperatorHolder OHolder)
        {
            Operator toAdd = Eq.OPStorage.Pop();
            toAdd.SetupOperator(OResultOmRightSide, OMFunction, OParameterIndex, ORandomNumber, OUseRandomNumber, Operators, NumberOfOperators, OExtraMathFunction, OHolder);
            NumberOfOperators++;
            return toAdd;
        }

        public void SetupOperator(bool OResultOmRightSide, MathFunction OMFunction, int OParameterIndex, double ORandomNumber, bool OUseRandomNumber,
                          Operator[] OContainedlist, int CIndex, Connector OExtraMathFunction, OperatorHolder OHolder)
        {
            ResultOnRightSide = OResultOmRightSide;
            MFunction = OMFunction;
            ParameterIndex = OParameterIndex;
            RandomNumber = ORandomNumber;
            UseRandomNumber = OUseRandomNumber;
            ContainedList = OContainedlist;
            ExtraMathFunction = OExtraMathFunction;
            Holder = OHolder;

            Eq.AllOperators.Add(this);
            ContainedIndex = CIndex;
            ContainedList[ContainedIndex] = this;
        }
    }
}
