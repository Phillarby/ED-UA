using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UALib
{
    public class Caesar : Substitution
    {
        public override void GenerateSubstitutions(int? seed = default(int?))
        {
            //If no seed specifed then assume no offset
            seed = seed ?? 0;
        }

        public Caesar(int offset) : base()
        {
            GenerateSubstitutions(offset);
        }

        private LinkedList<char> buildAlphabet()
        {
            var alpha = new LinkedList<char>();

            for (int i = 97; i <= 122; i++)
                alpha.AddLast(Convert.ToChar(i));

            return alpha;

        }
    }
}
