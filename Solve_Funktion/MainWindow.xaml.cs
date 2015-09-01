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
using System.Numerics;

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
            Task.Factory.StartNew(() => FindFunctionWithSpecies());
        }

        private VectorPoint[] GetSequence(string SequenceX, string SequenceY)
        {
            double[] SeqRX = SequenceX.Split(',').Select(x => Convert.ToDouble(x, CultureInfo.InvariantCulture.NumberFormat)).ToArray();
            double[] SeqRY = SequenceY.Split(',').Select(x => Convert.ToDouble(x, CultureInfo.InvariantCulture.NumberFormat)).ToArray();
            return GetSequence(SeqRX, SeqRY);
        }
        private VectorPoint[] GetSequence(double[] SeqRX, double[] SeqRY)
        {
            VectorPoint[] Seq = new VectorPoint[(int)Math.Ceiling((double)SeqRX.Length / (double)Constants.VECTOR_LENGTH)];
            int index = 0;
            for (int i = 0; i < SeqRX.Length; i += Constants.VECTOR_LENGTH)
            {
                int sizeLeft = SeqRX.Length - i;
                Vector<double> sRX;
                Vector<double> sRY;
                int vectorSize;
                if (sizeLeft >= Constants.VECTOR_LENGTH)
                {
                    sRX = new Vector<double>(SeqRX, i);
                    sRY = new Vector<double>(SeqRY, i);
                    vectorSize = Constants.VECTOR_LENGTH;
                }
                else
                {
                    vectorSize = sizeLeft;
                    double[] rXData = new double[Constants.VECTOR_LENGTH];
                    double[] rYData = new double[Constants.VECTOR_LENGTH];

                    Array.Copy(SeqRX, i, rXData, 0, sizeLeft);
                    Array.Copy(SeqRY, i, rYData, 0, sizeLeft);

                    int missingNumbers = Constants.VECTOR_LENGTH - sizeLeft;
                    for (int y = 1; y < missingNumbers + 1; y++)
                    {
                        rXData[y] = rXData[0];
                        rYData[y] = rYData[0];
                    }
                    sRX = new Vector<double>(rXData);
                    sRY = new Vector<double>(rYData);
                }
                Seq[index] = new VectorPoint(sRX, sRY, vectorSize);
                index++;
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

            //const string SequenceX = "0.0,0.1,0.2,0.7,0.8,0.9,1.0,1.6,1.7,1.8,1.9,2.0,2.1,2.2,2.3,2.9,3.0";
            //const string SequenceY = "0.0,0.0,0.0,0.0,0.0,0.0,1.0,1.0,1.0,1.0,1.0,1.0,1.0,1.0,1.0,1.0,1.0";

            //const string SequenceX = "0.0,0.1,0.2,0.7,0.8,0.9,1.0,1.6,1.7,1.8,1.9,2.0,2.1,2.2,2.3,2.9,3.0";
            //const string SequenceY = "0.0,0.1,0.2,0.7,0.8,0.9,0.0,0.6,0.7,0.8,0.9,0.0,0.1,0.2,0.3,0.9,0.0";

            //const string SequenceX = "0.0,0.1,0.2,0.3,0.4,0.5,0.6,0.7,0.8,0.9,0.999,1.0,1.001,1.1,1.2,1.3,1.4,1.5,1.6,1.7,1.8,1.9,1.999,2.0,2.001,2.1,2.2,2.3,2.4,2.5,2.6,2.7,2.8,2.9,3.0";
            //const string SequenceY = "0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,  1.0,1.0,  1.0,1.0,1.0,1.0,1.0,1.0,1.0,1.0,1.0,1.0,  1.0,0.0,  0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0";

            //const string SequenceX = "0.0,0.1,0.2,0.7,0.8,0.9,1.0,1.6,1.7,1.8,1.9,2.0,2.1,2.2,2.3,2.9,3.0";
            //const string SequenceY = "0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,1.0,0.0,0.0,0.0,0.0,0.0";

            //const string SequenceX = "0.0,0.1,0.2,0.7,0.8,0.9,1.0,1.6,1.7,1.8,1.9,2.0,2.1,2.2,2.3,2.9,3.0,3.1,3.2,3.7,3.8,3.9,4.0,4.6,4.7,4.8,4.9,5.0,5.1,5.2,5.3,5.9,6.0,6.1,6.2,6.3,6.9,7.0,7.1,7.2,7.7,7.8,7.9,8.0";
            //const string SequenceY = "0.0,0.0,0.0,0.0,0.0,0.0,1.0,0.0,0.0,0.0,0.0,1.0,0.0,0.0,0.0,0.0,1.0,0.0,0.0,0.0,0.0,0.0,1.0,0.0,0.0,0.0,0.0,1.0,0.0,0.0,0.0,0.0,1.0,0.0,0.0,0.0,0.0,1.0,0.0,0.0,0.0,0.0,0.0,1.0";

            //const string SequenceX = "  1";
            //const string SequenceY = "276";

            //List<double> SeqRX = new List<double>();
            //List<double> SeqRY = new List<double>();
            ////for (double i = -Math.PI; i < Math.PI; i += 0.1)
            ////{
            ////    SeqRX.Add(i);
            ////    SeqRY.Add(Math.Sin(i));
            ////}
            //for (double i = -30; i < 150; i += 10)
            //{
            //    SeqRX.Add(i);
            //    SeqRY.Add(Math.Exp(i));
            //}
            //VectorPoint[] Seq = GetSequence(SeqRX.ToArray(), SeqRY.ToArray());
            VectorPoint[] Seq = GetSequence(SequenceX,
                                            SequenceY);

            MathFunction[] Operators = new MathFunction[]
        {
            new Plus(), //SIMD
            new Subtract(), //SIMD
            new Multiply(), //SIMD
            new Divide(), //SIMD

            //new PowerOf(), //SIMD
            //new Root(), //SIMD
            //new Exponent(),
            //new NaturalLog(),
            //new Log(),

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

            new Parentheses(), //SIMD
            new Absolute(), //SIMD

            //new AND(), //SIMD
            //new NAND(), //SIMD
            //new OR(), //SIMD
            //new NOR(), //SIMD
            //new XOR(), //SIMD
            //new XNOR(), //SIMD
            //new NOT() //SIMD
        };
            EvolutionInfo EInfo = new EvolutionInfo(
                Seq,      // Sequence
                20,       // MaxSize
                5,        // MaxChange
                30000,    // CandidatesPerGen
                150,      // NumberRangeMax
                -30,     // NumberRangeMin
                7,        // SpeciesAmount
                50,      // MaxStuckGens
                0.8,      // EvolvedCandidatesPerGen
                0,        // RandomCandidatesPerGen
                0.2,      // SmartCandidatesPerGen
                Operators // Operators that can be used in an equation
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
                BCandControl.BestFunction.Offset > e.BestEquationInfo.Offset && 
                BCandControl.BestFunction.toCalc >= e.BestEquationInfo.toCalc ||
                BCandControl.BestFunction.Offset == e.BestEquationInfo.Offset &&
                BCandControl.BestFunction.toCalc >= e.BestEquationInfo.toCalc &&
                BCandControl.BestFunction.OperatorCount > e.BestEquationInfo.OperatorCount)
            {
                BCandControl.InsertInfo(e.BestEquationInfo);
            }
        }
    }
}