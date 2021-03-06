﻿using System;
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
        System.Timers.Timer chartUpdateTimer = new System.Timers.Timer(100);

        ConcurrentStack<SpecieInfoControl> SpecControls;
        IndividualSpecieEnviroment<SingleSpecieEvolutionMethod> singleSpecieEnviroment;
        FamilyEnviroment<FamilySpecieEvolutionMethod> familyEnviroment;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_ContentRendered_1(object sender, EventArgs e)
        {

            SpecControls = new ConcurrentStack<SpecieInfoControl>(new[] { SC8, SC7, SC6, SC5, SC4, SC3, SC2, SC1 });
            Task.Factory.StartNew(() => FindFunctionWithSpecies());

            chartUpdateTimer.Elapsed += ChartUpdateTimer_Elapsed;
            chartUpdateTimer.Start();
        }

        private void ChartUpdateTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            DrawGraph();
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

                const string SequenceX = "x = { 1,  2, 3,  4, 5, 6,7,  8,  9, 10}";
                const string SequenceY = "74,143,34,243,23,52,9,253,224,231";

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

                //string str = "Math Memes";
                //byte[] bytes = Encoding.ASCII.GetBytes(str);
                //byte[] bytes = new byte[str.Length * sizeof(char)];
                //System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
                //string SequenceX = "x = {" + String.Join(", ", Enumerable.Range(0, bytes.Length)) + "}";
                //((string SequenceY = String.Join(", ", bytes);

                //List<double> SeqRX = new List<double>();
                //List<double> SeqRY = new List<double>();
                //for (double i = -Math.PI; i < Math.PI; i += 0.4)
                //{
                //    SeqRX.Add(i);
                //    SeqRY.Add(Math.Sin(i));
                //}
                //string SequenceX = "x = {" + String.Join(", ", SeqRX.Select(x => x.ToString("N2", CultureInfo.GetCultureInfo("en-US")))) + "}";
                //string SequenceY = String.Join(", ", SeqRY.Select(x => x.ToString("N2", CultureInfo.GetCultureInfo("en-US"))));
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

                    //new Modulos(),
                    //new Floor(),
                    //new Ceil(),
                    //new Round(),

                    new Sin(),
                    new Cos(),
                    new Tan(),
                    new ASin(),
                    new ACos(),
                    new ATan(),

                    new Parentheses(),
                    //new Constant(),
                    //new Absolute(),

                    //new AND(),
                    //new NAND(),
                    //new OR(),
                    //new NOR(),
                    //new XOR(),
                    //new XNOR(),
                    //new NOT()
                };

                //EvolutionInfo EInfo = new EvolutionInfo(
                //    Seq,      // Sequence
                //    20,       // MaxSize
                //    0.2,        // MaxChange
                //    30000,    // CandidatesPerGen
                //    Math.Max(0, GetMaxNumber(Seq)) + 1,   // NumberRangeMax
                //    0,     // NumberRangeMin
                //    6,        // SpeciesAmount
                //    100,      // MaxStuckGens
                //    0.8,      // EvolvedCandidatesPerGen
                //    0,        // RandomCandidatesPerGen
                //    0.2,      // SmartCandidatesPerGen
                //    Operators // Operators that can be used in an equation
                //);


                //singleSpecieEnviroment = new IndividualSpecieEnviroment<SingleSpecieEvolutionMethod>();
                //singleSpecieEnviroment.OnBestEquationChanged += SpecieEnviroment_OnBestEquationChanged;
                //singleSpecieEnviroment.OnSubscribeToSpecies += SpecieEnviroment_OnSubscribeToSpecies;

                //GeneralInfo GInfo = singleSpecieEnviroment.SetupEviroment(EInfo);
                //GeneralInfoControl.InsertInfo(GInfo);
                //singleSpecieEnviroment.SimulateEnviroment();

                EvolutionInfo EInfo = new EvolutionInfo(
                    Seq,      // Sequence
                    20,       // MaxSize
                    0.2,        // MaxChange
                    400,    // CandidatesPerGen
                    Math.Max(0, GetMaxNumber(Seq)) + 1,   // NumberRangeMax
                    0,     // NumberRangeMin
                    100,        // SpeciesAmount
                    100,      // MaxStuckGens
                    0.8,      // EvolvedCandidatesPerGen
                    0,        // RandomCandidatesPerGen
                    0.2,      // SmartCandidatesPerGen
                    Operators // Operators that can be used in an equation
                );

                familyEnviroment = new FamilyEnviroment<FamilySpecieEvolutionMethod>();
                familyEnviroment.OnBestEquationChanged += SpecieEnviroment_OnBestEquationChanged;
                familyEnviroment.OnSubscribeToSpecies += SpecieEnviroment_OnSubscribeToSpecies;

                GeneralInfo GInfo = familyEnviroment.SetupEviroment(EInfo);
                GeneralInfoControl.InsertInfo(GInfo);
                familyEnviroment.SimulateEnviroment();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + Environment.NewLine + e.StackTrace);
                throw;
            }


            MessageBox.Show("Done");
        }

        private int GetMaxNumber(CoordInfo coordInfo)
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

        private object checkBestEquationLocker = new object();

        public void SpecieEnviroment_OnBestEquationChanged(BestEquationEventArgs e)
        {
            lock (checkBestEquationLocker)
            {
                if (e.BestEquationInfo.OperatorCount > 0)
                {
                    if (BCandControl.BestFunction == null ||
                    BCandControl.BestFunction.Offset > e.BestEquationInfo.Offset &&
                    BCandControl.BestFunction.toCalc <= e.BestEquationInfo.toCalc ||
                    BCandControl.BestFunction.Offset == e.BestEquationInfo.Offset &&
                    BCandControl.BestFunction.toCalc <= e.BestEquationInfo.toCalc &&
                    BCandControl.BestFunction.OperatorCount > e.BestEquationInfo.OperatorCount)
                    {
                        BCandControl.InsertInfo(e.BestEquationInfo);
                        System.Diagnostics.Debug.WriteLine(e.BestEquationInfo.Offset);
                    }
                }
            }
        }

        private object ChartLocker = new object();

        public void DrawGraph()
        {
            Genome[] Species = (Genome[])singleSpecieEnviroment?.Species ?? (Genome[])familyEnviroment?.Species;
            if (Species != null)
            {
                lock (ChartLocker)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        Chart Charter = (Chart)WFHChart.Child;
                        if (Charter.ChartAreas.Count == 0)
                        {
                            ChartArea cArea = Charter.ChartAreas.Add("newchartarea");
                            const double EXTRA_SPACE = 1.00;
                            cArea.AxisX.Minimum = Species[0].EInfo.coordInfo.parameters[0].Min() * EXTRA_SPACE;
                            cArea.AxisX.Maximum = Species[0].EInfo.coordInfo.parameters[0].Max() * EXTRA_SPACE;
                            cArea.AxisY.Minimum = Species[0].EInfo.coordInfo.expectedResults.Min() * EXTRA_SPACE;
                            cArea.AxisY.Maximum = Species[0].EInfo.coordInfo.expectedResults.Max() * EXTRA_SPACE;
                        }
                        Charter.Series.Clear();
                        int GenerationIndex = 0;
                        foreach (Genome Spec in Species)
                        {
                            if (Spec.BestCandidate != null)
                            {
                                AddSerieToChart(GenerationIndex.ToString(), SeriesChartType.Spline, 2, Spec.EInfo.coordInfo.parameters[0], Spec.BestCandidate.Results, null);
                                GenerationIndex++;
                            }
                            else
                            {
                                return;
                            }
                        }
                        AddSerieToChart("Correct", SeriesChartType.Spline, 6, Species[0].EInfo.coordInfo.parameters[0], Species[0].EInfo.coordInfo.expectedResults, System.Drawing.Color.Black);

                        if (BCandControl.BestFunction != null)
                        {
                            string[] floats = BCandControl.BestFunction.ResultText.Split(' ');
                            var floatsWithDot = floats.Select(x => x.Replace(",", "."));
                            var floatsWithDotNoLastDot = floatsWithDot.Select(x => x.Substring(0,x.Length - 1));
                            float[] bestResult = floatsWithDotNoLastDot.Select(x => float.Parse(x, CultureInfo.InvariantCulture.NumberFormat)).ToArray();
                            AddSerieToChart("Best", SeriesChartType.Spline, 4, Species[0].EInfo.coordInfo.parameters[0], bestResult, System.Drawing.Color.Red);
                        }

                        AddSerieToChart("Average", SeriesChartType.Spline, 4, Species[0].EInfo.coordInfo.parameters[0], GetAverageResult(), System.Drawing.Color.Green);

                        Charter.Legends.Clear();
                        Charter.Legends.Add("Correct");
                        Charter.Legends.Add("Best");
                        Charter.Legends.Add("Average");
                    });
                }
            }
        }

        public Series AddSerieToChart(string name, SeriesChartType chartType, int borderWidth, float[] parameters, float[] results, System.Drawing.Color? color)
        {
            if (results.All(x => x < 2000000 && x > -2000000))
            {
                Chart Charter = (Chart)WFHChart.Child;

                Series serie = Charter.Series.Add(name);
                serie.ChartType = chartType;
                serie.BorderWidth = borderWidth;
                if (color != null)
                {
                    serie.Color = color.Value;
                }

                for (int i = 0; i < results.Length; i++)
                {
                    serie.Points.AddXY(parameters[i], results[i]);
                }

                return serie;
            }
            return null;
        }

        private float[] GetAverageResult()
        {
            Genome[] Species = (Genome[])singleSpecieEnviroment?.Species ?? (Genome[])familyEnviroment?.Species;
            float[] averageResult = new float[Species[0].EInfo.coordInfo.parameters[0].Length];
            for (int i = 0; i < averageResult.Length; i++)
            {
                float sum = 0;
                for (int y = 0; y < Species.Length; y++)
                {
                    sum += Species[y].BestCandidate.Results[i];
                }
                float average = sum / Species.Length;
                averageResult[i] = average;
            }
            return averageResult;
        }
    }
}