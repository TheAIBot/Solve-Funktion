using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solve_Funktion
{
    public abstract class OperatorHolder
    {
        public readonly Operator[] Operators;
        public int NumberOfOperators = 0;

        public OperatorHolder(int operatorSize)
        {
            Operators = new Operator[operatorSize];
        }



        public abstract void OperatorChanged();



        public void AddOperatorToHolder(Operator oper, int index)
        {
            Operators[index] = oper;
            NumberOfOperators++;
        }

        public Operator AddOperator(Stack<Operator> operatorStorage)
        {
            return AddOperator(operatorStorage, GetFirstFreeIndex());
        }

        public Operator AddOperator(Stack<Operator> operatorStorage, int index)
        {
            Operator ToAdd = operatorStorage.Pop();
            ToAdd.MakeRandom(Operators, this, index);
            return ToAdd;
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

        public void RemoveOperatorFromHolder(int index)
        {
            Operators[index] = null;
            NumberOfOperators--;
        }

        public void Compress(Equation eq)
        {
            if (NumberOfOperators > 0 && NumberOfOperators < eq.EInfo.MaxSize)
            {
                Tools.CompressOperatorArray(Operators, NumberOfOperators, eq, true);
            }
        }
    }
}
