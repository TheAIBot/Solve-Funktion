using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solve_Funktion
{
    [Serializable]
    internal class OP
    {
        public bool Side;
        public Calculatables Operator;
        //public FunctionParam FParam;
        public double Number;
        public bool UseNumber;

        public void MakeRandom(ref EquationInfo EInfo)
        {
            Side = SynchronizedRandom.Next(0, 2) == 1 ? true : false;
            do
            {
                Operator = Info.Operatos[SynchronizedRandom.Next(0, Info.Operatos.Count)];
            } while (!Operator.CanUseOperator(ref EInfo));
            Operator = Operator.GetCalculatable(ref EInfo);
            //int Index = SynchronizedRandom.Next(0, EInfo.Params.Count);
            //FParam = EInfo.Params[Index];
            UseNumber = SynchronizedRandom.Next(0, 2) == 1 ? true : false;
            Number = SynchronizedRandom.Next(-Info.NumberRange, Info.NumberRange);
            Operator.MakeRandom(ref EInfo);
        }

        public double Calculate(double Result, double x)
        {
            return Operator.Calculate(Result, Number, x, Side, UseNumber);
        }

        public string ShowOperator(string Result,string x)
        {
            return Operator.ShowOperator(Result, Number.ToString(), x, Side, UseNumber);
        }

        public void StoreAndCleanup(ref EquationInfo EInfo)
        {
            Operator.StoreAndCleanup(ref EInfo);
            EInfo.OPStorage.Push(this);
        }

        public OP GetCopy(ref EquationInfo EInfo)
        {
            OP TOP = EInfo.OPStorage.Pop();
            TOP.Side = Side;
            TOP.Operator = Operator.GetCopy(ref EInfo);
            TOP.Number = Number;
            TOP.UseNumber = UseNumber;
            return TOP;
        }
    }
}
