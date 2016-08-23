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
using System.Timers;

namespace EquationCreator
{
    /// <summary>
    /// Interaction logic for GeneralInformationControl.xaml
    /// </summary>
    public partial class GeneralInformationControl : UserControl
    {
        public GeneralInfo GInfo;
        public long OldTotalAttemps = 0;
        private const int STORED_ATTEMPS_SEC = 15;
        private Queue<long> OldAttempsSec = new Queue<long>(STORED_ATTEMPS_SEC);

        public GeneralInformationControl()
        {
            InitializeComponent();
        }

        private void AttempsTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            long Change = GInfo.TotalAttempts - OldTotalAttemps;
            OldTotalAttemps = GInfo.TotalAttempts;
            OldAttempsSec.Enqueue(Change);
            if (OldAttempsSec.Count > STORED_ATTEMPS_SEC)
            {
                OldAttempsSec.Dequeue();
            }
            if (OldAttempsSec.Count > 0)
            {
                this.Dispatcher.Invoke(() => AttempsSec.Text = OldAttempsSec.Average().ToString("N0") + " Attemps/Sec");
            }
        }

        public void InsertInfo(GeneralInfo gInfo)
        {
            GInfo = gInfo;
            Dispatcher.Invoke(() => this.DataContext = gInfo);
            Timer AttempsTimer = new Timer();
            AttempsTimer.AutoReset = true;
            AttempsTimer.Interval = 1000;
            AttempsTimer.Elapsed += AttempsTimer_Elapsed;
            AttempsTimer.Enabled = true;
        }
    }
}
