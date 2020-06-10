using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Maps.Report
{
    internal class ReportComparer : IReport
    {
        string path = "MapsComparer.csv";

        void IReport.Save(string text)
        {
            File.WriteAllText(path, text.ToString());
        }

        void IReport.SaveAndDisplay(string text)
        {
            Console.WriteLine(text);

            var csv = new StringBuilder(text);
            csv.AppendLine("");
            File.AppendAllText(path, csv.ToString());
        }
    }
}
