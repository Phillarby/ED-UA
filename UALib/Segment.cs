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
        public int Index;               //The position of this segment in the sequence (Zero Indexed)

        public Segment(
            int pos,
            Howl howl,
            Purr[] purrs)
        {
            Index = pos;
            Howl = howl;

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
