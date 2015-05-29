using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UALib
{
    //Perhaps there is some kind of substitution Cypher.  Base class for general substitution cypher 
    public abstract class Substitution
    {
        private Dictionary<char, string> Table = new Dictionary<char, string>();

        //Substitution cypher requires a method to populate the substituion table
        public abstract void GenerateSubstitutions(int? seed = null);

        //Apply substitution to phrase using substition scheme defined in derived class
        public virtual string Translate(string phrase)
        {
            var sb = new StringBuilder();

            //Substitute character by character
            foreach (char c in phrase)
            {
                if (!Table.ContainsKey(c))
                    throw new Exception(String.Format("Substitution table does not contain a mapping for {0}", c));

                sb.Append(Table[c]);
            }

            return sb.ToString();
        }
    }
}
