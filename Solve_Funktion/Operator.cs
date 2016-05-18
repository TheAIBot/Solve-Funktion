using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

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
        public List<Operator> ContainedList;
        public Connector ExtraMathFunction;
        public int MaxCalculated = NONE_CALCULATED;
        public OperatorHolder Holder;
        public readonly Equation Eq;
        public readonly List<Operator> Operators;
        //public readonly Vector<double>[] OperatorResults;

        public Operator(Equation OEq)
        {
            Eq = OEq;
            Operators = new List<Operator>(Eq.EInfo.MaxSize);
        }

        /// <summary>
        /// makes this operator a random operator and adds it to the equation by itself
        /// </summary>
        /// <param name="containedList">the list this operator is contained in</param>
        public void MakeRandom(List<Operator> OContainedList, OperatorHolder OHolder)
        {
            Holder = OHolder;
            ContainedList = OContainedList;
            Eq.AllOperators.Add(this);
            ContainedList.Add(this);
            ChangeOperator();
        }
        public void MakeRandom(List<Operator> OContainedList, OperatorHolder OHolder, int CIndex)
        {
            Holder = OHolder;
            ContainedList = OContainedList;
            Eq.AllOperators.Add(this);
            ContainedList.Insert(CIndex, this);
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
            ContainedList.Remove(this);
            MFunction.StoreAndCleanup(this);
            // these actions might not be required yet but if there is any errors then it will be easier to spot if a null exception is thrown
            MFunction = null;
            Holder = null;
            ResetMaxCalculated();
        }

        public void ChangeCleanup()
        {
            MFunction.StoreAndCleanup(this);
            // these actions might not be required yet but if there is any errors then it will be easier to spot if a null exception is thrown
            MFunction = null;
            Holder = null;
            ResetMaxCalculated();
        }

        /// <summary>
        /// faster way of resetting the operator when all operatos in an equation are reset at once
        /// </summary>
        public void StoreAndCleaupAll()
        {
            Eq.OPStorage.Push(this);
            MFunction.StoreAndCleanupAll(this);
            // these actions might not be required yet but if there is any errors then it will be easier to spot if a null exception is thrown
            MFunction = null;
            Holder = null;
            ResetMaxCalculated();
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
        public Operator GetCopy(Operator Copy, Equation CopyEq, List<Operator> CopyContainedList, OperatorHolder CopyHolder)
        {
            Copy.ResultOnRightSide = ResultOnRightSide;
            Copy.MFunction = MFunction;
            Copy.ParameterIndex = ParameterIndex;
            Copy.RandomNumber = RandomNumber; // this might be wrong
            Copy.UseRandomNumber = UseRandomNumber;
            Copy.ContainedList = CopyContainedList;
            Copy.Eq.AllOperators.Add(Copy);
            Copy.ContainedList.Add(Copy);
            Copy.MFunction.GetCopy(this, Copy);
            Copy.Holder = CopyHolder;
            Copy.MaxCalculated = MaxCalculated;
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

        public void SetupOperator(bool OResultOmRightSide, MathFunction OMFunction, int OParameterIndex, double ORandomNumber, bool OUseRandomNumber,
                          List<Operator> OContainedlist, Connector OExtraMathFunction, OperatorHolder OHolder)
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
            ContainedList.Add(this);
        }
    }
}
