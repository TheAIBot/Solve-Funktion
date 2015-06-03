using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Solve_Funktion
{
    public class UIUpdateLimiter : INotifyPropertyChanged, IDisposable
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public double UpdateDelay = 500;
        private Timer UIUpdater = new Timer();
        public Stack<string> UpdatedProperties = new Stack<string>();
        private object UpdatedPropertiesLocker = new object();

        public UIUpdateLimiter()
        {
            UIUpdater.AutoReset = false;
            UIUpdater.Elapsed += UIUpdater_Elapsed;
            UIUpdater.Interval = UpdateDelay;
            UIUpdater.Start();
        }

        public void UpdateProperty(string Property)
        {
            if (!UpdatedProperties.Contains(Property))
            {
                UpdatedProperties.Push(Property);
            }
        }

        void UIUpdater_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock (UpdatedPropertiesLocker)
            {
                while (UpdatedProperties.Count > 0)
                {
                    OnPropertyChanged(UpdatedProperties.Pop());
                }
            }
            UIUpdater.Start();
        }

        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        public void Dispose()
        {
            UIUpdater.Dispose();
        }
    }
}
