using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Solve_Funktion
{
    public class Operator
    {
        public bool ResultOnRightSide; // decides which side the result should be on
        public MathFunction MFunction; // the operator this operator is using
        public Vector<double> Number;
        public bool UseNumber;
        public Equation Eq; // the equation this operator is a part of
        public List<Operator> ContainedList; // the list this operator is contained in in SortedOperators
        public MathFunction ExtraMathFunction; // extra operator taht can be used if needed by the operator
        public List<Operator> Operators; // list of other operators this operator contains

        public Operator(Equation eq)
        {
            Eq = eq;
            Operators = new List<Operator>(Eq.EInfo.MaxSize);
        }

        /// <summary>
        /// makes this operator a random operator and adds it to the equation by itself
        /// </summary>
        /// <param name="containedList">the list this operator is contained in</param>
        public void MakeRandom(List<Operator> containedList)
        {
            ContainedList = containedList;
            Eq.AllOperators.Add(this);
            ContainedList.Add(this);
            ResultOnRightSide = SynchronizedRandom.RandomBool();
            do
            {
                MFunction = Eq.EInfo.Operators[SynchronizedRandom.Next(0, Eq.EInfo.Operators.Length)];
                // should never be an infinete loop because this function should only be called when there is 1 or more operators left
                // and there should always be an operator that doesn't need a min of operators
            } while (!MFunction.CanUseOperator(this));
            UseNumber = SynchronizedRandom.RandomBool();
            Number = SynchronizedRandom.NextVector(Eq.EInfo.NumberRangeMin, Eq.EInfo.NumberRangeMax);
            MFunction.MakeRandom(this);
        }
        public void MakeRandom(List<Operator> containedList, int CIndex)
        {
            ContainedList = containedList;
            Eq.AllOperators.Add(this);
            ContainedList.Insert(CIndex, this);
            ResultOnRightSide = SynchronizedRandom.RandomBool();
            do
            {
                MFunction = Eq.EInfo.Operators[SynchronizedRandom.Next(0, Eq.EInfo.Operators.Length)];
                // should never be an infinete loop because this function should only be called when there is 1 or more operators left
                // and there should always be an operator that doesn't need a min of operators
            } while (!MFunction.CanUseOperator(this));
            UseNumber = SynchronizedRandom.RandomBool();
            Number = SynchronizedRandom.NextVector(Eq.EInfo.NumberRangeMin, Eq.EInfo.NumberRangeMax);
            MFunction.MakeRandom(this);
        }

        /// <summary>
        /// calcualtes the next step in an equation using this operator
        /// </summary>
        /// <param name="Result">number returned by previous operators or initial number from equation</param>
        /// <param name="x"> value of x</param>
        /// <returns>result of Result and this operator</returns>
        public Vector<double> Calculate(Vector<double> Result, Vector<double> x)
        {
            return MFunction.Calculate(Result, x, this);
        }

        /// <summary>
        /// adds this operator to the currently created equation
        /// </summary>
        /// <param name="x"></param>
        /// <param name="Forwards"></param>
        /// <param name="Backwards"></param>
        public void ShowOperator(string x, StringBuilder Forwards, StringBuilder Backwards)
        {
            MFunction.ShowOperator(x, this, Forwards, Backwards);
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
        }

        /// <summary>
        /// Copys itself into a new OP Can only be called from within Equations copy method
        /// </summary>
        /// <param name="Copy">The OP to copy into</param>
        /// <param name="CopyEInfo">The Equationinfo the copy should use</param>
        /// <returns></returns>
        public Operator GetCopy(Operator Copy, Equation CopyEq, List<Operator> CopyContainedList)
        {
            Copy.ResultOnRightSide = ResultOnRightSide;
            Copy.MFunction = MFunction;
            Copy.Number = Number;
            Copy.UseNumber = UseNumber;
            Copy.Eq = CopyEq;
            Copy.ContainedList = CopyContainedList;
            Copy.Eq.AllOperators.Add(Copy);
            Copy.ContainedList.Add(Copy);
            Copy.MFunction.GetCopy(this, Copy);
            return Copy;
        }

        public override string ToString()
        {
            return MFunction.ToString();
        }
    }
}
