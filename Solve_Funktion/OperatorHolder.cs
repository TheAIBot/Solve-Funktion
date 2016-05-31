﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solve_Funktion
{
    public interface OperatorHolder
    {
        void OperatorChanged();

        void AddOperatorToHolder(Operator oper, int index);

        void AddOperator();

        void AddOperator(int index);

        void RemoveOperatorFromHolder(int index);
    }
}
