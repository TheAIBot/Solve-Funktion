﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;
using System.Diagnostics;

namespace EquationCreator
{
    public static class RandomCand
    {
        public static void MakeRandomEquations(Equation[] Candidates, int StartIndex, int Amount)
        {
            for (int i = StartIndex; i < StartIndex + Amount; i++)
            {
                MakeRandomEquation(Candidates[i]);
                Candidates[i].CalcTotalOffSet();
            }
        }

        public static void MakeRandomEquation(Equation Cand)
        {
            Cand.MakeRandom();
        }

        public static void MakeValidRandomEquation(Equation eq)
        {
            do
            {
                eq.Cleanup();
                MakeRandomEquation(eq);
                eq.CalcTotalOffSet();
            } while (!Tools.IsANumber(eq.OffSet));
        }
    }
}