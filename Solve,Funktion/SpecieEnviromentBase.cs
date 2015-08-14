﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Solve_Funktion
{
    public abstract class SpecieEnviromentBase//<T> where T : Genome, new()
    {
        public event BestEquationEventHandler OnBestEquationChanged;
        public event SubscribeEventEventHandler OnSubscribeToSpecies;
        protected EvolutionInfo EInfo;
        protected SpeciesInfo BestEquationInfo;
        protected GeneralInfo GInfo;
        protected Genome[] Species;

        public abstract GeneralInfo SetupEviroment(EvolutionInfo EInfo);

        public abstract void SimulateEnviroment();

        public void DoSubscribeEvent(Genome Spec)
        {
            if (OnSubscribeToSpecies != null)
            {
                OnSubscribeToSpecies(new SubscribeEventEventArgs() { Specie = Spec });
            }
        }

        public void CheckBestCandidate()
        {
            if (OnBestEquationChanged != null)
            {
                List<Genome> SpecInfos = Species.Where(x => x.BestCandidate != null && Tools.IsANumber(x.BestCandidate.OffSet))
                                        .OrderByDescending(x => x._toCalc)
                                        .ThenBy(x => x.BestCandidate.OffSet)
                                        .ThenByDescending(x => x.BestCandidate.OperatorsLeft).ToList();
                if (SpecInfos.Count > 0)
                {
                    SpeciesInfo SpecInfo = SpecInfos.First().SpecInfo;
                    OnBestEquationChanged(new BestEquationEventArgs
                    {
                        BestEquationInfo = SpecInfo
                    });
                }
            }
        }
    }

    public delegate void BestEquationEventHandler(BestEquationEventArgs e);
    public delegate void SpecieCreatedEventHandler(SpecieCreatedEventArgs e);
    public delegate void SubscribeEventEventHandler(SubscribeEventEventArgs e);

    public class BestEquationEventArgs : EventArgs
    {
        public SpeciesInfo BestEquationInfo;
    }
    public class SpecieCreatedEventArgs : EventArgs
    {
        public SpeciesInfo SpecInfo;
    }
    public class SubscribeEventEventArgs : EventArgs
    {
        public GeneralInfo GInfo;
        public Genome Specie;
    }

    public sealed class VectorPoint
    {
        public Vector<double> X;
        public Vector<double> Y;

        public VectorPoint(Vector<double> x, Vector<double> y)
        {
            X = x;
            Y = y;
        }
    }
}