using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace UALib
{
    public class Dictionary
    {
        string ImportFilename = @".\ImportFiles\dictionary.txt";
        List<DictionaryEntry> Entries = new List<DictionaryEntry>();

        //Read lines from a file
        private List<string> Readlines(string filename)
        {
            var lines = new List<string>();
            StreamReader fs = new StreamReader(filename);

            string line;
            while ((line = fs.ReadLine()) != null) lines.Add(line);

            return lines;
        }

        public void go()
        {
            var words = Readlines(ImportFilename);

            foreach (string s in words)
            {
                Entries.Add(new DictionaryEntry(s));
            } 
        }

        private class DictionaryEntry
        {
            public String Word { get; private set; }
            public String Morse { get; private set; }

            public DictionaryEntry(string word)
            {
                Word = word;
                Morse = MorseTable.TranslateWord(word, false);
            }
        }
    }
}
