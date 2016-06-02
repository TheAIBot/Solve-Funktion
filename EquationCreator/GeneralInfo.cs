using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace EquationCreator
{
    public class GeneralInfo : UIUpdater
    {
        private object Locker = new object();

        private long totalattempts = 0;
        private int totalspecies = 0;

        public long TotalAttempts
        {
            get
            {
                return Interlocked.Read(ref totalattempts);
            }
            set
            {
                Interlocked.Exchange(ref totalattempts, value);
                UpdateProperty("GetTotalAttempts");
            }
        }
        public void IncrementTotalAttemps()
        {
            Interlocked.Increment(ref totalattempts);
            UpdateProperty("GetTotalAttempts");
        }
        public void AddTotalAttempts(long ToAdd)
        {
            Interlocked.Add(ref totalattempts, ToAdd);
            UpdateProperty("GetTotalAttempts");
        }
        public string GetTotalAttempts
        {
            get
            {
                return TotalAttempts.ToString("N0");
            }
        }
        
        public int TotalSpecies
        {
            get
            {
                int ToReturn = 0;
                Interlocked.Exchange(ref ToReturn, totalspecies);
                return ToReturn;
            }
            set
            {
                Interlocked.Exchange(ref totalspecies, value);
                UpdateProperty("TotalSpecies");
            }
        }
        public void IncrementTotalSpecies()
        {
            Interlocked.Increment(ref totalspecies);
            UpdateProperty("TotalSpecies");
        }
        public void AddTotalSpecies(int ToAdd)
        {
            Interlocked.Add(ref totalspecies, ToAdd);
            UpdateProperty("TotalSpecies");
        }
    }
}