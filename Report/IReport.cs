using System;
using System.Collections.Generic;
using System.Text;

namespace Maps.Report
{
    interface IReport
    {
        public void Save(string text);

        public void SaveAndDisplay(string text);
    }
}

