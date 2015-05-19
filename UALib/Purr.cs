using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UALib
{
    public class Purr
    {
        public int? Identifer { get; private set; }
        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }
        public float? Frequency { get; private set; }
        public bool Quiet { get; private set; }

        public Purr(double start, double end, int? identifier, bool quiet = false, float? frequency = null)
        {
            Identifer = identifier;
            Start = Utils.TimeConvert(start);
            End = Utils.TimeConvert(end);
            Frequency = frequency;
            Quiet = Quiet; 
        }
    }


}
