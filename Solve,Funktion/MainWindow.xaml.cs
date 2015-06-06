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
            Task.Factory.StartNew(() =>             FindFunctionWithSpecies());
        }

        private Point[] GetSequence(string SequenceX, string SequenceY)
        {
            double[] SeqRX = SequenceX.Split(',').Select(x => Convert.ToDouble(x, CultureInfo.InvariantCulture.NumberFormat)).ToArray();
            double[] SeqRY = SequenceY.Split(',').Select(x => Convert.ToDouble(x, CultureInfo.InvariantCulture.NumberFormat)).ToArray();
            Point[] Seq = new Point[SeqRX.Length];
            for (int i = 0; i < SeqRX.Length; i++)
            {
                Seq[i] = new Point(SeqRX[i], SeqRY[i]);
            }
            return Seq;
        }

        private void FindFunctionWithSpecies()
        {
            var SpecieEnviroment = new IndividualSpecieEnviroment<SingleSpecieEvolution>();
            SpecieEnviroment.OnBestEquationChanged += SpecieEnviroment_OnBestEquationChanged;
            SpecieEnviroment.OnSubscribeToSpecies += SpecieEnviroment_OnSubscribeToSpecies;

            //const string SequenceX = "1,2,3,4, 5, 6, 7, 8, 9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24, 25";
            //const string SequenceY = "2,3,5,7,11,13,17,23,29,31,37,41,43,47,53,59,61,67,71,73,79,83,89,97,101";

            //const string SequenceX = "1,2,3,4, 5, 6, 7, 8, 9,10";
            //const string SequenceY = "2,3,5,7,11,13,17,19,23,29";

            //const string SequenceX = "  1,   2,  3, 4,  5,6,  7,8, 9,10";
            //const string SequenceY = "432,4567,987,23,765,2,678,9,34,23";

            //const string SequenceX = "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54";
            //const string SequenceY = "1,0,1,0,1,0,0,0,1,1,1,0,0,1,1,0,1,0,1,0,1,1,1,1,0,0,1,0,1,0,0,1,0,1,0,0,0,1,1,0,1,0,1,1,1,0,0,1,0,1,0,0,0,1";

            //const string SequenceX = "     2,     3,     4";
            //const string SequenceY = "182014,364572,495989";

            //const string SequenceX = "1,2,3,      4, 5,    6, 7,          8, 9,10";
            //const string SequenceY = "2,4,6,2342238,10,23432,14,12223332116,18,20";

            const string SequenceX = " 1,  2, 3,  4, 5, 6,7,  8,  9, 10";
            const string SequenceY = "74,143,34,243,23,52,9,253,224,231";

            //const string SequenceX = "  1";
            //const string SequenceY = "276";

            Point[] Seq = GetSequence(SequenceX,
                                      SequenceY);

            MathFunction[] Operators = new MathFunction[]
        {
            new Plus(),
            new Subtract(),
            new Multiply(),
            new Divide(),

            new PowerOf(),
            new Root(),
            new Exponent(),
            new NaturalLog(),
            new Log(),

            //new Modulos(),
            //new Floor(),
            //new Ceil(),
            //new Round(),

            //new Sin(),
            //new Cos(),
            //new Tan(),
            //new ASin(),
            //new ACos(),
            //new ATan(),

            new Parentheses(),
            //new Absolute(),

            //new AND(),
            //new NAND(),
            //new OR(),
            //new NOR(),
            //new XOR(),
            //new XNOR(),
            //new NOT()
        };
            EvolutionInfo EInfo = new EvolutionInfo(
                Seq,    // Sequence
                40,    // MaxSize
                5,     // MaxChange
                30000,  // CandidatesPerGen
                102,    // NumberRangeMax
                0,      // NumberRangeMin
                8,      // SpeciesAmount
                100,    // MaxStuckGens
                0.8,    // EvolvedCandidatesPerGen
                0,      // RandomCandidatesPerGen
                0.2,    // SmartCandidatesPerGen
                Operators // Operatres that can be used in an equation
                );

            GeneralInfo GInfo = SpecieEnviroment.SetupEviroment(EInfo);
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