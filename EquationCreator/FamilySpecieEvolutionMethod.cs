using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquationCreator
{
    public class FamilySpecieEvolutionMethod : FamilyGenome
    {

        public override Family EvolveFamily(Family family)
        {
            StartFinding();
            GetNextGen(family);
            UpdateInfo();
            return family;
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

                if (//parents[randomParentIndex].NumberOfOperators > index &&
                    parents[randomParentIndex].Operators[index] != null &&
                    parents[randomParentIndex].Operators[index].GetOperatorCount() <= childLength - child.NumberOfAllOperators)
                {
                    parents[randomParentIndex].Operators[index].GetCopy(child.OPStorage.Pop(), child, child);
                }
                else
                {
                    bool anyIsSmallEnough = false;
                    for (int i = 0; i < parents.Length; i++)
                    {
                        if (//parents[i].NumberOfOperators > index &&
                            parents[i].Operators[index] != null &&
                            parents[i].Operators[index].GetOperatorCount() <= childLength - child.NumberOfAllOperators)
                        {
                            if (!firstCheck && randomNumber != randomParentIndex)
                            {
                                alwaysSameRandomNumber = false;
                            }
                            firstCheck = false;
                            randomNumber = randomParentIndex;

                            anyIsSmallEnough = true;
                            parents[i].Operators[index].GetCopy(child.OPStorage.Pop(), child, child);
                            break;
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

        private void GetNextGen(Family family)
        {
            //int simplestCount = 0;
            for (double i = 0; i < EInfo.CandidatesPerGen; i++)
            {
                if (GetChild(family.parents, child))
                {
                    if (child.NumberOfAllOperators > 0) // maybe not need but just to be sure
                    {
                        EvolveCand.EvolveCandidate(EInfo, child);
                        child.CalcTotalOffSet();
                        if (Tools.IsANumber(child.OffSet))
                        {
                            family.CheckNewChild(child);
                        }
                        //if (EvolvedEquation.CreateFunction() == "f(x) = x")
                        //{
                        //    simplestCount++;
                        //}
                        //Debug.WriteLine(EvolvedEquation.CreateFunction());
                        //Thread.Sleep(500);
                    }
                }
                
                ResetSingle(child);
            }
            //Debug.WriteLine(simplestCount);
            //Thread.Sleep(500);
        }
    }
}
