﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;

namespace Solve_Funktion
{
    public static class Info
    {
        public static List<MathFunction> Operators = new List<MathFunction>() 
        {
            new Parentheses(),

            new Plus(),
            new Subtract(),
            new Multiply(),
            new Divide(),

            new PowerOf(),
            new Exponent(),
            new NaturalLog(),
            new Log(),

            //new Modulos(),
            new Floor(),
            new Ceil(),
            new Round(),

            new Sin(),
            new Cos(),
            new Tan(),
            new ASin(),
            new ACos(),
            new ATan(),

            //new AND(),
            //new OR(),
            //new NOT()
        };

        public const int MaxSize = 40;
        public const int MaxChange = 5;
        public const double CandidatesPerGen = 30000;
        public const int NumberRange = 300;
        public const int SpeciesAmount = 8;
        public const int MaxStuckGens = 100;
        public const double EvolvedCandidatesPerGen = 0.7;
        public const double RandomCandidatesPerGen = 0.0;
        public const double SmartCandidatesPerGen = 0.3;
        //public const double EvolvedCandidatesPerGen = 0.0;
        //public const double RandomCandidatesPerGen = 1.0;
        //public const double SmartCandidatesPerGen = 0.0;
        public static List<Point> Seq = new List<Point>();
        public const int Rounding = 2;
        //public const string SequenceX = "1,2,3,4, 5, 6, 7, 8, 9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25";
        //public const string SequenceY = "2,3,5,7,11,13,17,23,29,31,37,41,43,47,53,59,61,67,71,73,79,83,89,97,101";

        //public const string SequenceX = "1,2,3,4, 5, 6, 7, 8, 9,10";
        //public const string SequenceY = "2,3,5,7,11,13,17,19,23,29";

        //public const string SequenceX = "  1,   2,  3, 4,  5,6,  7,8, 9,10";
        //public const string SequenceY = "432,4567,987,23,765,2,678,9,34,23";

        //public const string SequenceX = "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54";
        //public const string SequenceY = "1,0,1,0,1,0,0,0,1,1,1,0,0,1,1,0,1,0,1,0,1,1,1,1,0,0,1,0,1,0,0,1,0,1,0,0,0,1,1,0,1,0,1,1,1,0,0,1,0,1,0,0,0,1";

        //public const string SequenceX = "2,3,4";
        //public const string SequenceY = "182014,364572,495989";

        //public const string SequenceX = "1,2,3,4,5,6,7,8,9,10";
        //public const string SequenceY = "2,4,6,2342238,10,23432,14,12223332116,18,20";

        public const string SequenceX = "1,2,3,4,5,6,7,8,9,10";
        public const string SequenceY = "74,143,34,243,23,52,9,253,224,231";
    }

    public class GeneralInfo : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private Stopwatch Watch = new Stopwatch();

        private object Locker = new object();

        private long totalattempts = 0;
        private int totalspecies = 0;

        public long TotalAttempts
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
