using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Threading;

namespace Solve_Funktion
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Specie> Species;
        object LockerUpdateBestInfo = new object();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_ContentRendered_1(object sender, EventArgs e)
        {
            //Task.Factory.StartNew(()=> Logic());
            Stopwatch Watch = new Stopwatch();

            Watch.Start();
            int Total = 0;
            for (int i = 0; i < 20000; i++)
            {
                Candidate Test = new Candidate();
                Test.MakeRandom();
                Test.CalcOffSet();
                Total += (int)Test.OffSet;
            }
        }

        private Candidate CreateStaticCand()
        {
            Candidate Test = new Candidate();
            foreach (MathOperator MathO in Info.OPS)
            {
                OP OO = new OP(false);
                OO.Number = 10;
                OO.Operator = MathO;
                OO.Side = 0;
                OO.UseNumber = false;
                Test.Operators.Add(OO);
            }
            return Test;
        }

        private void Logic()
        {
            StartUp();
            List<double> Seq = GetSequence();
            FindFunctionWithSpecies(Seq);
        }

        private void StartUp()
        {
            RandomCand.StartUp(this);
            EvolveCand.StartUp(this);
            SmartCand.StartUp(this);
        }

        private List<double> GetSequence()
        {
            string Seq = "2,3,5,7,9,11,13,17,19,23,29,31,37,41,43,47,53";//,59,61,67,71,73,79,83,89,97";
            //string Seq = "1,1,2,3,5,8,13,21,34,55,89,144,233,377";
            List<double> SeqR = Seq.Split(',').ToList().Select(x => Convert.ToDouble(x)).ToList();
            return SeqR;
        }

        private async void FindFunctionWithSpecies(List<double> Seq)
        {
            Species = CreateSpecies(Info.SpeciesAmount);
            List<Task<Specie>> Tasks = new List<Task<Specie>>();
            for (int i = 0; i < Species.Count; i++)
            {
                int Temp = i;
                Tasks.Add(Task<Specie>.Run(() => Species[Temp].FindFunction(Seq)));
            }

            while (true)
            {
                Task<Specie> Finished = await Task.WhenAny(Tasks);

                if (Finished.Result.BestCandidate.OffSet != 0)
                {
                    lock (LockerUpdateBestInfo)
                    {
                        int Index = Tasks.IndexOf(Finished);
                        Tasks[Index] = Task<Specie>.Run(() => Finished.Result.FindFunction(Seq));
                    }
                }
                else
                {
                    global::System.Windows.Forms.MessageBox.Show("Done");
                    break;
                }
            }
        }

        private List<Specie> CreateSpecies(int Amount)
        {
            List<Specie> Species = new List<Specie>();
            List<SpecieInfoControl> SpecControls = new List<SpecieInfoControl>() { SC1, SC2, SC3, SC4, SC5, SC6, SC7, SC8 };
            for (int i = 0; i < Amount; i++)
            {
                Specie Spec = new Specie(this);
                Spec.SpecInfoControl = SpecControls[i];
                Species.Add(Spec);
            }
            return Species;
        }

        public void UpdateBestInfo()
        {
            lock (LockerUpdateBestInfo)
            {
                double MinOffSet = Species.Min(x => x.BestCandidate.OffSet);
                SpeciesInfo SpecInfo = Species.Find(x => x.BestCandidate.OffSet == MinOffSet).SpecInfo;
                if (BCandControl.SpecInfo == null || BCandControl.SpecInfo.Offset > SpecInfo.Offset && SpecInfo.FunctionText != null)
                {
                    BCandControl.InsertInfo(SpecInfo);
                }
            }
        }
    }

    public enum MathOperator
    {
        Plus,
        Minus,
        Multiply,
        Divide,
        PowerOf,
        Exponent,
        Ln,
        Log,
        Floor,
        Ceil,
        Sin,
        Cos,
        Tan,
        ASin,
        ACos,
        ATan
    }
}
