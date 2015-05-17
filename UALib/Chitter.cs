using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UALib
{
    public class Chitter
    {
        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }

        public Chitter(double start, double end)
        {
            Start = Utils.TimeConvert(start);
            End = Utils.TimeConvert(end);
        }
    }
}
