using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UALib
{
    public class Sequence
    {
        public List<Segment> Segments;
        public string Recorder { get; internal set; }
        public Uri Link { get; internal set; }
        public string Description { get; internal set; }

        public Sequence(string recorder, string description)
        {
            Description = description;
            Recorder = recorder;
            Segments = new List<Segment>();
        }

        public int AddSegment(Segment s)
        {
            Segments.Add(s);
            return Segments.Count;
        }


    }
}
