using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UALib
{
    public class Segment
    {
        public LinkedList<Purr> Purrs;  //Bits in the sequence.  
        public Howl Howl;               //The type of the howl from the END of the sequence
        public Chitter Chitter;
        public int Index;               //The position of this segment in the sequence (Zero Indexed)

        public Segment(
            int pos,
            Purr[] purrs,
            Chitter chitter = null,
            Howl howl = null)
        {
            Index = pos;
            Howl = howl;
            Chitter = chitter;

            //Init purrs container and add purrs
            Purrs = new LinkedList<Purr>();
            for (int j = 0; j < purrs.Length; j++)
                Purrs.AddLast(purrs[j]);

            //Set position based on start timing
            int i = 0;
            var pl = purrs.OrderBy(x => x.Start).ToList(); 
            foreach (Purr pu in pl) pu.Position = i++;

        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            Purrs.OrderBy(x => x.Position).ToList().ForEach(x => { sb.Append(x.Position); });
            return sb.ToString();
        }

        //Get the number of bits in the segment
        public int Length
        {
            get { return Purrs.Count; }
        }
    }
}
