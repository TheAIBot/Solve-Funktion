﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solve_Funktion
{
    [Serializable]
    public class Operator
    {
        //AllOperators
        //ContainedList
        public bool ResultOnRightSide;
        public MathFunction MFunction;
        public MathFunction ExtraMathFunction;
        public double Number;
        public bool UseNumber;
        public Equation Eq;
        public List<Operator> ContainedList;
        public List<Operator> Operators = new List<Operator>(Info.MaxSize);

        public void MakeRandom(Equation eq, List<Operator> containedList)
        {
            Eq = eq;
            ContainedList = containedList;
            Eq.AllOperators.Add(this);
            ContainedList.Add(this);
            ResultOnRightSide = SynchronizedRandom.RandomBool();
            do
            {
                MFunction = Info.Operators[SynchronizedRandom.Next(0, Info.Operators.Count)];
            // should never be an infinete loop because this function should only be called when there is 1 or more operators left
            // and there should always be an operator that doesn't need a min of operators
            } while (!MFunction.CanUseOperator(this));
            UseNumber = SynchronizedRandom.RandomBool();
            Number = SynchronizedRandom.NextDouble(Info.NumberRangeMin, Info.NumberRangeMax);
            MFunction.MakeRandom(this);
        }

        public double Calculate(double Result, double x)
        {
            return MFunction.Calculate(Result, x, this);
        }

        public void ShowOperator(string x, StringBuilder Forwards, StringBuilder Backwards)
        {
            MFunction.ShowOperator(x, this, Forwards, Backwards);
            //return MFunction.ShowOperator(Result, x, this);
        }

        public int GetOperatorCount()
        {
            return MFunction.GetOperatorCount(this);
        }

        public void StoreAndCleanup()
        {
            Eq.OPStorage.Push(this);
            Eq.AllOperators.Remove(this);
            ContainedList.Remove(this);
            MFunction.StoreAndCleanup(this);
            // these actions might not be required yet but if there is any errors then it will be easier to spot if a null exception is thrown
            MFunction = null;
            Eq = null;
        }

        public void StoreAndCleaupAll()
        {
            Eq.OPStorage.Push(this);
            MFunction.StoreAndCleanupAll(this);
            // these actions might not be required yet but if there is any errors then it will be easier to spot if a null exception is thrown
            MFunction = null;
            Eq = null;
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
