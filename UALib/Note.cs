using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UALib
{
    class Note
    {
        public string Message { get; private set; }
        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }

        public Note(double start, double end, string message)
        {
            Message = message;
            Start = Utils.TimeConvert(start);
            End = Utils.TimeConvert(end);
        }
    }
}
