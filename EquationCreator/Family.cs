using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquationCreator
{
    public class Family
    {
        public readonly Equation[] parents;
        public readonly Equation[] children;

        public Family(EvolutionInfo eInfo, int parentCount)
        {
            parents = new Equation[parentCount];
            children = new Equation[parentCount];

            SynchronizedRandom random = new SynchronizedRandom();
            
            for (int i = 0; i < parents.Length; i++)
            {
                parents[i] = new Equation(eInfo, random);
                children[i] = new Equation(eInfo, random);

                RandomCand.MakeValidRandomEquation(parents[i]);

                parents[i].CalcTotalOffSet();
            }
        }

        public void CheckNewChild(Equation child)
        {
            for (int i = children.Length - 1; i >= 0; i--)
            {
                if (children[i].OffSet > child.OffSet || children[i].NumberOfAllOperators == 0)
                {
                    Equation worstChild = children[0];
                    for (int y = 0; y < i; y++)
                    {
                        children[y] = children[y + 1];
                    }
                    worstChild.Cleanup();
                    child.MakeClone(worstChild);
                    children[i] = worstChild;
                    break;
                }
            }
        }

        public void MakeChildrenParents()
        {
            for (int i = 0; i < parents.Length; i++)
            {
                if (children[i].NumberOfAllOperators > 0)
                {
                    parents[i].Cleanup();
                    children[i].MakeClone(parents[i]);
                }
                children[i].Cleanup();
            }
        }
    }
}
