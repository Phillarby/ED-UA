using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UALib
{
    public class MorseTable
    {
        private static Dictionary<char, string> Table = new Dictionary<char, string>()
        {
            {'a', ".-" },
            {'b', "-..."},
            {'c', "-.-."},
            {'d', "-.."},
            {'e', "."},
            {'f', "..-."},
            {'g', "--."},
            {'h', "...."},
            {'i', ".."},
            {'j', ".---"},
            {'k', "-.-"},
            {'l', ".-.."},
            {'m', "--"},
            {'n', "-."},
            {'o', "---"},
            {'p', ".--."},
            {'q', "--.-"},
            {'r', ".-."},
            {'s', "..."},
            {'t', "-"},
            {'u', "..-"},
            {'v', "...-"},
            {'w', ".--"},
            {'x', "-..-"},
            {'y', "-.--"},
            {'z', "--.."},
            {'1', ".----"},
            {'2', "..---"},
            {'3', "...--"},
            {'4', "....-"},
            {'5', "....."},
            {'6', "-...."},
            {'7', "--..."},
            {'8', "---.."},
            {'9', "----."},
            {'0', "-----"}
        };

        public static string TranslateWord(string word, bool includeSpace = true)
        {
            var sb = new StringBuilder();

            foreach (char c in word)
                sb.Append(String.Format("{0}{1}", Table[c], includeSpace ? " " : ""));

            return sb.ToString().TrimEnd();
        }
    }
}
