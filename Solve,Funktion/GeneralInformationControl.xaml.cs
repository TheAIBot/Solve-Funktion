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
using System.Timers;

namespace Solve_Funktion
{
    /// <summary>
    /// Interaction logic for GeneralInformationControl.xaml
    /// </summary>
    public partial class GeneralInformationControl : UserControl
    {
        public GeneralInfo GInfo;
        public long OldTotalAttemps = 0;
        private const int STORED_ATTEMPS_SEC = 100;
        List<long> OldAttempsSec = new List<long>(STORED_ATTEMPS_SEC);

        public GeneralInformationControl()
        {
            InitializeComponent();
        }

        void AttempsTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            long Change = GInfo.TotalAttempts - OldTotalAttemps;
            OldTotalAttemps = GInfo.TotalAttempts;
            OldAttempsSec.Add(Change);
            if (OldAttempsSec.Count > STORED_ATTEMPS_SEC)
            {
                OldAttempsSec.RemoveAt(0);
            }
            this.Dispatcher.Invoke(() => AttempsSec.Text = OldAttempsSec.Average().ToString("N0") + " Attemps/Sec");

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
