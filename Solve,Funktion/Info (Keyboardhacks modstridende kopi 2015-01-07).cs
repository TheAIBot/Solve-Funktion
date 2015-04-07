using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;

namespace Solve_Funktion
{
    internal static class Info
    {
        public static List<Calculatables> Operatos = new List<Calculatables>() 
        {
            new Parentheses(),

            new Plus(),
            new Subtract(),
            new Multiply(),
            new Divide(),
            //new PowerOf(),
            //new Exponent(),
            //new NaturalLog(),
            //new Log(),
            new Modulos(),
            new Floor(),
            new Ceil(),
            new Round(),
            //new Sin(),
            //new Cos(),
            //new Tan(),
            //new ASin(),
            //new ACos(),
            //new ATan()

            //new AND(),
            //new OR(),
            //new NOT()
        };

        public const int MaxSize = 10;
        public const int MaxChange = 3;
        public const double CandidatesPerGen = 20000;
        public const int NumberRange = 40;
        public const int SpeciesAmount = 8;
        public const int MaxStuckGens = 50;
        public const double EvolvedCandidatesPerGen = 0.7;
        public const double RandomCandidatesPerGen = 0.0;
        public const double SmartCandidatesPerGen = 0.3;
        public static List<Point> Seq = new List<Point>();
        public const int Rounding = 2;
        //public const string SequenceX = "1,2,3,4,5,6,7,8,9,10,11,12";
        //public const string SequenceY = "31,28,31,30,31,30,31,31,30,31,30,31";
        public const string SequenceX = "1,2,3,4, 5, 6, 7, 8, 9,10,11";
        public const string SequenceY = "2,3,5,7,11,13,17,23,29,31,37";
    }

    public class GeneralInfo : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private Stopwatch Watch = new Stopwatch();

        private object Locker = new object();

        private int totalattempts = 0;
        private int totalspecies;

        public int TotalAttempts
        {
            get
            {
                lock (Locker)
                {
                    return totalattempts;
                }
            }
            set
            {
                lock (Locker)
                {
                    totalattempts = value;
                    OnPropertyChanged("GetTotalAttempts");
                }
            }
        }
        public string GetTotalAttempts
        {
            get
            {
                return totalattempts.ToString("N0");
            }
        }
        public int TotalSpecies
        {
            get
            {
                return totalspecies;
            }
            set
            {
                totalspecies = value;
                OnPropertyChanged("TotalSpecies");
            }
        }

        private void OnPropertyChanged(string property)
        {
            if (!Watch.IsRunning)
            {
                Watch.Start();
            }
            //the number is the time to wait between updates
            if (Watch.ElapsedMilliseconds > 128)
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(property));
                    Watch.Reset();
                }
            }
        }
    }
}
