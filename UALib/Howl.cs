using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UALib
{
    public class Howl
    {
        public int Identifer { get; private set; }
        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }

        public Howl(int identifier, double start, double end)
        {
            Identifer = identifier;
            Start = Utils.TimeConvert(start);
            End = Utils.TimeConvert(end);
        }
    }
}
