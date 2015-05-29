using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace UALib
{
    public class FFTImport
    {
        string ImportFilename = @".\ImportFiles\dictionary.txt";

        /// <summary>
        /// Read FFT data lines from a file
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private List<string> Readlines(string filename)
        {
            var lines = new List<string>();
            StreamReader fs = new StreamReader(filename);

            string line;
            while ((line = fs.ReadLine()) != null) lines.Add(line);

            //Remove header line
            lines.First().Remove(0,1);

            return lines;
        }

        /// <summary>
        /// Convert a collection of line strings to a collection of double pairs
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        private List<double[]> processLines(List<string> lines)
        {
            //Split line into value pairs
            var lst = new List<string[]>();
            lines.ForEach(x => { lst.Add(x.Split('\t')); });

            //Convert value pairs to double
            var dbl = new List<double[]>();
            lst.ForEach(x => { dbl.Add(new double[2] { double.Parse(x[0]), double.Parse(x[1]) }); });

            return dbl;
        }

        private List<double[]> IdentifyPeaks(List<double[]> values, double tollerance = 1)
        {
            var RemoveIndices = new List<int>();

            //Idenitfy value elements to be removed as they are not peaks or do not met the tollerance measure
            for (int i = 1; i < values.Count - 1; i++)
            {
                var ele = values.ElementAt(i);
                var pel = values.ElementAt(i - 1);
                var nel = values.ElementAt(i - 1);

                if (ele[1] - tollerance < pel[1] && ele[1] - tollerance < nel[1])
                {
                    RemoveIndices.Add(i);
                }
            }

            //Remove any idenitfied non-peak values
            for (int i = RemoveIndices.Count; i >= 0; i--)
            {
                values.RemoveAt(i);
            }

            //Sort peaks in order of descending magnitude
            values.OrderByDescending(x => x[1]);

            return values;
        }
    }
}
