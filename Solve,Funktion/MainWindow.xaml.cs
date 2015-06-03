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
using System.Globalization;
using System.Collections.Concurrent;

namespace Solve_Funktion
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ConcurrentStack<SpecieInfoControl> SpecControls;
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_ContentRendered_1(object sender, EventArgs e)
        {
            SpecControls = new ConcurrentStack<SpecieInfoControl>(new[] { SC8, SC7, SC6, SC5, SC4, SC3, SC2, SC1 });
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
            double[] SeqRX = SeqX.Split(',').Select(x => Convert.ToDouble(x, CultureInfo.InvariantCulture.NumberFormat)).ToArray();
            string SeqY = Info.SequenceY;
            double[] SeqRY = SeqY.Split(',').Select(x => Convert.ToDouble(x, CultureInfo.InvariantCulture.NumberFormat)).ToArray();
            Info.Seq = new Point[SeqRX.Length];
            for (int i = 0; i < SeqRX.Length; i++)
            {
                Info.Seq[i] = new Point(SeqRX[i], SeqRY[i]);
            }
        }

        private void FindFunctionWithSpecies()
        {
            var SpecieEnviroment = new IndividualSpecieEnviroment<SingleSpecieEvolution>();
            SpecieEnviroment.OnBestEquationChanged += SpecieEnviroment_OnBestEquationChanged;
            SpecieEnviroment.OnSubscribeToSpecies += SpecieEnviroment_OnSubscribeToSpecies;

            GeneralInfo GInfo = SpecieEnviroment.SetupEviroment(Info.SpeciesAmount);
            GeneralInfoControl.InsertInfo(GInfo);
            SpecieEnviroment.SimulateEnviroment();

            MessageBox.Show("Done");
        }

        public void SpecieEnviroment_OnSubscribeToSpecies(SubscribeEventEventArgs e)
        {
            SpecieInfoControl SpecControl;
            if (SpecControls.TryPop(out SpecControl))
            {
                e.Specie.OnSpecieCreated += SpecControl.InsertInfo;
            }
        }

        public void SpecieEnviroment_OnBestEquationChanged(BestEquationEventArgs e)
        {
            if (BCandControl.BestFunction == null ||
                BCandControl.BestFunction.Offset > e.BestEquationInfo.Offset ||
                BCandControl.BestFunction.Offset == e.BestEquationInfo.Offset && 
                BCandControl.BestFunction.OperatorCount > e.BestEquationInfo.OperatorCount)
            {
                BCandControl.InsertInfo(e.BestEquationInfo);
            }
        }
    }
}