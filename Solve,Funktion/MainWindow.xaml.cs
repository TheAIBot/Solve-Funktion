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
using System.Globalization;

namespace Solve_Funktion
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_ContentRendered_1(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() => Logic());
        }

        private void Logic()
        {
            GetSequence();
            FindFunctionWithSpecies();
        }

        private void GetSequence()
        {
            string SeqX = Info.SequenceX;
            List<double> SeqRX = SeqX.Split(',').ToList().Select(x => Convert.ToDouble(x, CultureInfo.InvariantCulture.NumberFormat)).ToList();
            string SeqY = Info.SequenceY;
            List<double> SeqRY = SeqY.Split(',').ToList().Select(x => Convert.ToDouble(x, CultureInfo.InvariantCulture.NumberFormat)).ToList();
            for (int i = 0; i < SeqRX.Count; i++)
            {
                Info.Seq.Add(new Point(SeqRX[i], SeqRY[i]));
            }
        }

        List<Genome> Species;
        object LockerUpdateBestInfo = new object();

        private async void FindFunctionWithSpecies()
        {
            Species = CreateSpecies(Info.SpeciesAmount);
            List<Task<Genome>> Tasks = new List<Task<Genome>>();
            for (int i = 0; i < Species.Count; i++)
            {
                int Temp = i;
                Tasks.Add(Task<Genome>.Run(() => Species[Temp].EvolveSolution()));
            }

            while (true)
            {
                Task<Genome> Finished = await Task.WhenAny(Tasks);
                if (Finished.Result.BestCandidate.OffSet != 0)
                {
                    lock (LockerUpdateBestInfo)
                    {
                        int Index = Tasks.IndexOf(Finished);
                        Tasks[Index] = Task<Genome>.Run(() => Finished.Result.EvolveSolution());
                    }
                }
                else
                {
                    MessageBox.Show("Done");
                    break;
                }
            }
        }

        private List<Genome> CreateSpecies(int Amount)
        {
            object LockerAdd = new object();
            List<Genome> Species = new List<Genome>();
            GeneralInfo GInfo = new GeneralInfo();
            GeneralInfoControl.InsertInfo(GInfo);
            // the amount of threads to use is tied to the SpecieInfoControl amount
            List<SpecieInfoControl> SpecControls = new List<SpecieInfoControl>() { SC1, SC2, SC3, SC4, SC5, SC6, SC7, SC8 };
            Parallel.For(0, Amount, i =>
            {
                Genome Spec = new SingleParentEvolution(this);
                //Genome Spec = new EdgeEvolution(this);
                Spec.CreateStorages();
                Spec.SpecInfoControl = SpecControls[i];
                Spec.GInfoControl = GeneralInfoControl;
                lock (LockerAdd)
                {
                    Species.Add(Spec);
                }
            });
            return Species;
        }

        public void UpdateBestInfo()
        {
            lock (LockerUpdateBestInfo)
            {
                List<Genome> SpecInfos = Species.Where(x => Tools.IsANumber(x.BestCandidate.OffSet))
                                      .OrderBy(x => x.BestCandidate.OffSet)
                                      .ThenByDescending(x => x.BestCandidate.OperatorsLeft).ToList();
                SpeciesInfo SpecInfo = SpecInfos.First().SpecInfo;

                if (BCandControl.SpecInfo == null ||
                    BCandControl.SpecInfo.Offset > SpecInfo.Offset && SpecInfo.FunctionText != null)
                {
                    BCandControl.InsertInfo(SpecInfo);
                }
            }
        }
    }
}
