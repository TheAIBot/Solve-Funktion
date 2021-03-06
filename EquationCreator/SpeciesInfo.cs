﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EquationCreator
{
    [Serializable]
    public class SpeciesInfo : UIUpdater
    {

        private int generation = 0;
        private string functiontext;
        private string sequencetext;
        private string resulttext;
        private double attemts = 0;
        private double offset;
        public int toCalc { get; set; } = 0;


        public string TitleText
        {
            get
            {
                return "Generation: " + Generation.ToString("N0");
            }
        }
        public int Generation
        {
            get
            {
                return generation;
            }
            set
            {
                generation = value;

                UpdateProperty(nameof(TitleText));
            }
        }
        public string FunctionText
        {
            get
            {
                return functiontext;
            }
            set
            {
                functiontext = value;
                UpdateProperty(nameof(FunctionText));
            }
        }
        public double Offset
        {
            get
            {
                return offset;
            }
            set
            {
                offset = value;
                UpdateProperty(nameof(OffsetText));
            }
        }
        public string OffsetText
        {
            get
            {
                if (Offset == 0)
                {
                    return "0";
                }
                int Number;
                List<char> OffSetToShow = new List<char>();
                for (Number = Info.Rounding; Number < 10; Number++)
                {
                    OffSetToShow = Offset.ToString('N' + Number.ToString()).ToCharArray().ToList();
                    OffSetToShow.RemoveAll(x => x == ',' || x == '0');
                    if (OffSetToShow.Count != 0)
                    {
                        break;
                    }
                }
                OffSetToShow = Offset.ToString('N' + Number.ToString()).ToCharArray().ToList();
                return String.Join(String.Empty, OffSetToShow);
            }
        }
        public string SequenceText
        {
            get
            {
                return sequencetext;
            }
            set
            {
                sequencetext = value;
                UpdateProperty(nameof(SequenceText));
            }
        }
        public string ResultText
        {
            get
            {
                return resulttext;
            }
            set
            {
                resulttext = value;
                UpdateProperty(nameof(ResultText));
            }
        }
        public double Attempts
        {
            get
            {
                return attemts;
            }
            set
            {
                attemts = value;
                UpdateProperty(nameof(AttemptsText));
            }
        }
        public string AttemptsText
        {
            get
            {
                return Attempts.ToString("N0");
            }
        }
        public int OperatorCount = 0;

        public SpeciesInfo  GetCopy()
        {
            SpeciesInfo copy = new SpeciesInfo();
            copy.Generation = Generation;
            copy.FunctionText = FunctionText;
            copy.Offset = Offset;
            copy.SequenceText = SequenceText;
            copy.ResultText = ResultText;
            copy.Attempts = Attempts;
            copy.OperatorCount = OperatorCount;

            return copy;
        }
    }
}
