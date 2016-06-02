using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace EquationCreator
{
    [Serializable]
    public class UIUpdater : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void UpdateProperty(string Property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Property));
        }
    }
}
