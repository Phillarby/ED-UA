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
            for (int i = 0; i < purrs.Length; i++)
                Purrs.AddLast(purrs[i]);
        }

        //Get the number of bits in the segment
        public int Length
        {
            get { return Purrs.Count; }
        }
    }
}
