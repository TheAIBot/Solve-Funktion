using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquationCreator
{
    public class FamilySpecieEvolutionMethod : FamilyGenome
    {

        public override Genome EvolveFamily(Equation[] parents)
        {
            StartFinding();
            GetNextGen(parents);
            UpdateInfo();
            return this;
        }

        protected bool GetChild(Equation[] parents, Equation child)
        {
            int childLengthIndex = Randomizer.Next(0, parents.Length);
            int childLength = parents[childLengthIndex].NumberOfAllOperators;
            int index = 0;

            int randomNumber = -1;
            bool alwaysSameRandomNumber = true;
            bool firstCheck = true;

            while (child.NumberOfAllOperators < childLength)
            {
                
                int randomParentIndex = Randomizer.Next(0, parents.Length);

                if (!firstCheck && randomNumber != randomParentIndex)
                {
                    alwaysSameRandomNumber = false;
                }
                firstCheck = false;
                randomNumber = randomParentIndex;

                if (parents[randomParentIndex].NumberOfOperators > index &&
                    parents[randomParentIndex].Operators[index].GetOperatorCount() <= childLength - child.NumberOfAllOperators)
                {
                    parents[randomParentIndex].Operators[index].GetCopy(child.OPStorage.Pop(), child, child.Operators, child);
                    child.NumberOfOperators++;
                }
                else
                {
                    bool anyIsSmallEnough = false;
                    for (int i = 0; i < parents.Length; i++)
                    {
                        if (parents[i].NumberOfOperators > index &&
                            parents[i].Operators[index].GetOperatorCount() <= childLength - child.NumberOfAllOperators)
                        {
                            anyIsSmallEnough = true;
                            parents[i].Operators[index].GetCopy(child.OPStorage.Pop(), child, child.Operators, child);
                            child.NumberOfOperators++;
                        }
                    }
                    if (!anyIsSmallEnough)
                    {
                        break;
                    }
                }
                index++;
            }
            return !alwaysSameRandomNumber;
        }

        private Equation GetNextGen(Equation[] parents)
        {
            child.Cleanup();
            while (!GetChild(parents, child))
            {
                ResetSingle(child);
            }
            child.CalcTotalOffSet();
            BestCandidate.Cleanup();
            child.MakeClone(BestCandidate);
            ResetSingle(child);
            //int simplestCount = 0;
            for (double i = 0; i < EInfo.CandidatesPerGen; i++)
            {
                if (GetChild(parents, child))
                {
                    EvolveCand.EvolveCandidate(EInfo, child);
                    child.CalcTotalOffSet();
                    //if (EvolvedEquation.CreateFunction() == "f(x) = x")
                    //{
                    //    simplestCount++;
                    //}
                    //Debug.WriteLine(EvolvedEquation.CreateFunction());
                    //Thread.Sleep(500);
                    bool EvolvedToBetter = ChangeIfBetter(child);
                }
                
                ResetSingle(child);
            }
            //Debug.WriteLine(simplestCount);
            //Thread.Sleep(500);
            return BestCandidate;
        }

        protected bool ChangeIfBetter(Equation Eq)
        {
            if (Tools.IsANumber(Eq.OffSet))
            {
                if (Eq.OffSet < BestCandidate.OffSet &&
                    Eq._toCalc >= BestCandidate._toCalc ||
                    Eq.OffSet == BestCandidate.OffSet &&
                    Eq.NumberOfAllOperators < BestCandidate.NumberOfAllOperators &&
                    Eq._toCalc >= BestCandidate._toCalc)
                //if (Eq.OffSet < BestEvolvedCand.OffSet)
                //if (Eq.OffSet < BestEvolvedCand.OffSet || Eq.OffSet == BestEvolvedCand.OffSet && Eq.OperatorsLeft < BestEvolvedCand.OperatorsLeft)
                {
                    ResetSingle(BestCandidate);
                    Eq.MakeClone(BestCandidate);
                    return true;
                }
            }
            return false;
        }
    }
}
