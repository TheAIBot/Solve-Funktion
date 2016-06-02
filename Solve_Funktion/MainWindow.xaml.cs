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
using System.Text.RegularExpressions;
using System.Windows.Forms.DataVisualization.Charting;

namespace EquationCreator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ConcurrentStack<SpecieInfoControl> SpecControls;
        IndividualSpecieEnviroment<SingleSpecieEvolution> SpecieEnviroment;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_ContentRendered_1(object sender, EventArgs e)
        {
            SpecControls = new ConcurrentStack<SpecieInfoControl>(new[] { SC8, SC7, SC6, SC5, SC4, SC3, SC2, SC1 });
            Task.Factory.StartNew(() => FindFunctionWithSpecies());
        }

        private void FindFunctionWithSpecies()
        {
            try
            {
                //MessageBox.Show(Vector<double>.Count.ToString());

                //const string SequenceX = "x = {1,2,3,4, 5, 6, 7, 8, 9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24, 25}";
                //const string SequenceY = "2,3,5,7,11,13,17,23,29,31,37,41,43,47,53,59,61,67,71,73,79,83,89,97,101";

                //const string SequenceX = "x = {1,2,3,4, 5, 6, 7, 8, 9,10}";
                //const string SequenceY = "2,3,5,7,11,13,17,19,23,29";

                //const string SequenceX = "x = {  1,   2,  3, 4,  5,6,  7,8, 9,10}";
                //const string SequenceY = "432,4567,987,23,765,2,678,9,34,23";

                //const string SequenceX = "x = {1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54}";
                //const string SequenceY = "     1,0,1,0,1,0,0,0,1, 1, 1, 0, 0, 1, 1, 0, 1, 0, 1, 0, 1, 1, 1, 1, 0, 0, 1, 0, 1, 0, 0, 1, 0, 1, 0, 0, 0, 1, 1, 0, 1, 0, 1, 1, 1, 0, 0, 1, 0, 1, 0, 0, 0, 1";

                //const string SequenceX = "     2,     3,     4";
                //const string SequenceY = "182014,364572,495989";

                //const string SequenceX = "x = {1,2,3,      4, 5,    6, 7,          8, 9,10}";
                //const string SequenceY = "2,4,6,2342238,10,23432,14,12232116,18,20";

                //const string SequenceX = "x = { 1,  2, 3,  4, 5, 6,7,  8,  9, 10}";
                //const string SequenceY = "74,143,34,243,23,52,9,253,224,231";

                //const string SequenceX = "x = {384, 357, 221, 9, 18, 357, 221, 6}, y = {18, 357, 221, 6, 384, 357, 221, 9}";
                //const string SequenceY = "     6, 1, 17, 3, 6, 1, 17, 3";

                //const string SequenceX = "x = {1, 2, 3,  4,  5,   6,   7,   8,    9,   10,    11}";
                //const string SequenceY = "     1, 3, 8, 21, 55, 144, 377, 987, 2584, 6765, 17711";

                //const string SequenceX = "x = {1,2,3,4,5,6, 7, 8, 9,10,11}";
                //const string SequenceY = "     1,1,2,3,5,8,13,21,34,55,89";

                //const string SequenceX = "x = {1.1,1.5,2,2.1,10.8,200.8}";
                //const string SequenceY = "       1,  2,2,  2,  11,  201";

                //const string SequenceX = "x = {86, 86, 86, 86, 76, 76, 76, 76, 123, 123, 123, 123}, y = {1, 0.5, 0.25, 0.125, 1, 0.5, 0.25, 0.125, 1, 0.5, 0.25, 0.125}";
                //const string SequenceY = "     86, 76, 66, 56, 76, 66, 56, 46, 123, 113, 103,  93";
                //List<string> xx = new List<string>();
                //List<string> yy = new List<string>();
                //for (double i = -Math.PI; i < Math.PI; i += 0.2)
                //{
                //    xx.Add(i.ToString("N6", CultureInfo.InvariantCulture));
                //    yy.Add(Math.Sin(i).ToString("N6", CultureInfo.InvariantCulture));
                //}

                //string SequenceX = "x = {" + String.Join(", ", xx) + "}";
                //string SequenceY = String.Join(", ", yy);

                //const string SequenceX = "x = {2,3,4, 5, 6, 7, 8, 9,10,11}";
                //const string SequenceY = "     1,3,6,10,15,21,28,36,45,55";

                //const string SequenceX = "x = {1, 4, 3}, y = {1, 1, 3}, z = {1, 2, 3}";
                //const string SequenceY = "3,2,1";

                //const string SequenceX = "x = {1, 2, 3, 4, 5, 6,  7,  8,  9, 10, 11,  12,  13,  14}";
                //const string SequenceY = "     1, 1, 2, 3, 5, 8, 13, 21, 34, 55, 89, 144, 233, 377";

                //const string SequenceX = "x = {0.0,0.1,0.2,0.7,0.8,0.9,1.0,1.6,1.7,1.8,1.9,2.0,2.1,2.2,2.3,2.9,3.0}";
                //const string SequenceY = "     0.0,0.0,0.0,0.0,0.0,0.0,1.0,1.0,1.0,1.0,1.0,1.0,1.0,1.0,1.0,1.0,1.0";

                //const string SequenceX = "0.0,0.1,0.2,0.7,0.8,0.9,1.0,1.6,1.7,1.8,1.9,2.0,2.1,2.2,2.3,2.9,3.0";
                //const string SequenceY = "0.0,0.1,0.2,0.7,0.8,0.9,0.0,0.6,0.7,0.8,0.9,0.0,0.1,0.2,0.3,0.9,0.0";

                //const string SequenceX = "x = {0.0,0.1,0.2,0.3,0.4,0.5,0.6,0.7,0.8,0.9,0.999,1.0,1.001,1.1,1.2,1.3,1.4,1.5,1.6,1.7,1.8,1.9,1.999,2.0,2.001,2.1,2.2,2.3,2.4,2.5,2.6,2.7,2.8,2.9,3.0,4.0,5.0,5.0,6.0}";
                //const string SequenceY = "     0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,  0.0,1.0,  1.0,1.0,1.0,1.0,1.0,1.0,1.0,1.0,1.0,1.0,  1.0,0.0,  0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0";

                //const string SequenceX = "x = {0.0,0.1,0.2,0.7,0.8,0.9,1.0,1.6,1.7,1.8,1.9,2.0,2.1,2.2,2.3,2.9,3.0}";
                //const string SequenceY = "     0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,1.0,0.0,0.0,0.0,0.0,0.0";

                //const string SequenceX = "x = {0.0,0.1,0.2,0.7,0.8,0.9,1.0,1.6,1.7,1.8,1.9,2.0,2.1,2.2,2.3,2.9,3.0,3.1,3.2,3.7,3.8,3.9,4.0,4.6,4.7,4.8,4.9,5.0,5.1,5.2,5.3,5.9,6.0,6.1,6.2,6.3,6.9,7.0,7.1,7.2,7.7,7.8,7.9,8.0}";
                //const string SequenceY = "     0.0,0.0,0.0,0.0,0.0,0.0,1.0,0.0,0.0,0.0,0.0,1.0,0.0,0.0,0.0,0.0,1.0,0.0,0.0,0.0,0.0,0.0,1.0,0.0,0.0,0.0,0.0,1.0,0.0,0.0,0.0,0.0,1.0,0.0,0.0,0.0,0.0,1.0,0.0,0.0,0.0,0.0,0.0,1.0";

                //const string SequenceX = "  1";
                //const string SequenceY = "276";

                string str = "Fish and cake makes everything great again";
                byte[] bytes = Encoding.ASCII.GetBytes(str);
                //byte[] bytes = new byte[str.Length * sizeof(char)];
                //System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
                string SequenceX = "x = {" + String.Join(", ", Enumerable.Range(0, bytes.Length)) + "}";
                string SequenceY = String.Join(", ", bytes);

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
                CoordInfo Seq = new CoordInfo(SequenceX, SequenceY);

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

                    new Modulos(),
                    new Floor(),
                    new Ceil(),
                    new Round(),

                    new Sin(),
                    new Cos(),
                    new Tan(),
                    new ASin(),
                    new ACos(),
                    new ATan(),

                    new Parentheses(),
                    //new Constant(),
                    new Absolute(),

                    //new AND(),
                    //new NAND(),
                    //new OR(),
                    //new NOR(),
                    //new XOR(),
                    //new XNOR(),
                    //new NOT()
                };
                EvolutionInfo EInfo = new EvolutionInfo(
                    Seq,      // Sequence
                    40,       // MaxSize
                    0.2,        // MaxChange
                    30000,    // CandidatesPerGen
                    GetMaxNumberFromVectorPointArray(Seq) + 1,   // NumberRangeMax
                    0,     // NumberRangeMin
                    6,        // SpeciesAmount
                    100,      // MaxStuckGens
                    0.8,      // EvolvedCandidatesPerGen
                    0,        // RandomCandidatesPerGen
                    0.2,      // SmartCandidatesPerGen
                    Operators // Operators that can be used in an equation
                    );

                SpecieEnviroment = new IndividualSpecieEnviroment<SingleSpecieEvolution>();
                SpecieEnviroment.OnBestEquationChanged += SpecieEnviroment_OnBestEquationChanged;
                SpecieEnviroment.OnSubscribeToSpecies += SpecieEnviroment_OnSubscribeToSpecies;

                GeneralInfo GInfo = SpecieEnviroment.SetupEviroment(EInfo);
                GeneralInfoControl.InsertInfo(GInfo);
                SpecieEnviroment.SimulateEnviroment();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + Environment.NewLine + e.StackTrace);
                throw;
            }


            MessageBox.Show("Done");
        }

        private int GetMaxNumberFromVectorPointArray(CoordInfo coordInfo)
        {
            return (int)Math.Ceiling(Math.Max(coordInfo.expectedResults.Max(), coordInfo.parameters.Max(x => x.Max())));
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
                BCandControl.BestFunction.toCalc <= e.BestEquationInfo.toCalc ||
                BCandControl.BestFunction.Offset == e.BestEquationInfo.Offset &&
                BCandControl.BestFunction.toCalc <= e.BestEquationInfo.toCalc &&
                BCandControl.BestFunction.OperatorCount > e.BestEquationInfo.OperatorCount)
            {
                lock (e.BestEquationInfo)
                {
                    BCandControl.InsertInfo(e.BestEquationInfo);
                }
                System.Diagnostics.Debug.WriteLine(e.BestEquationInfo.Offset);
            }
            DrawGraph();
        }

        private object ChartLocker = new object();

        public void DrawGraph()
        {
            lock (ChartLocker)
            {
                this.Dispatcher.Invoke(() =>
                {
                    Chart Charter = (Chart)WFHChart.Child;
                    if (Charter.ChartAreas.Count == 0)
                    {
                        Charter.ChartAreas.Add("newchartarea");
                    }
                    Charter.Series.Clear();
                    int GenerationIndex = 0;
                    foreach (Genome Spec in SpecieEnviroment.Species)
                    {
                        Series Serie = Charter.Series.Add(GenerationIndex.ToString());
                        Serie.ChartType = SeriesChartType.Spline;
                        Serie.BorderWidth = 2;
                        if (Spec.BestCandidate.Results.All(x => x < (double)Decimal.MaxValue))
                        {
                            for (int i = 0; i < Spec.EInfo.coordInfo.expectedResults.Length; i++)
                            {
                                Serie.Points.AddXY(Spec.EInfo.coordInfo.parameters[0][i], Spec.BestCandidate.Results[i]);
                            }
                        }
                        GenerationIndex++;
                    }
                    Series Seride = Charter.Series.Add("Correct");
                    Seride.ChartType = SeriesChartType.Spline;
                    Seride.BorderWidth = 4;
                    Seride.Color = System.Drawing.Color.Black;
                    for (int i = 0; i < SpecieEnviroment.Species[0].EInfo.coordInfo.expectedResults.Length; i++)
                    {
                        Seride.Points.AddXY(SpecieEnviroment.Species[0].EInfo.coordInfo.parameters[0][i], SpecieEnviroment.Species[0].EInfo.coordInfo.expectedResults[i]);
                    }
                });
            }
        }
    }
}