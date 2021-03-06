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

namespace EquationCreator
{
    /// <summary>
    /// Interaction logic for SpecieInfoControl.xaml
    /// </summary>
    public partial class SpecieInfoControl : UserControl
    {
        public SpecieInfoControl()
        {
            InitializeComponent();
        }

        public void InsertInfo(SpecieCreatedEventArgs e)
        {
            try
            {
                Dispatcher.Invoke(() => this.DataContext = e.SpecInfo);
            }
            catch (Exception) { }
        }
    }
}
