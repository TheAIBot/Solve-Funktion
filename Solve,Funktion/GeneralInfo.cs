using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.ComponentModel;


namespace Solve_Funktion
{
    internal class GeneralInfo : INotifyPropertyChanged
    {
        internal event PropertyChangedEventHandler PropertyChanged;
        private Stopwatch Watch = new Stopwatch();

        private object Locker = new object();

        private int totalattempts = 0;
        private int totalspecies;

        internal int TotalAttempts
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
        internal string GetTotalAttempts
        {
            get
            {
                return totalattempts.ToString("N0");
            }
        }
        internal int TotalSpecies
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
