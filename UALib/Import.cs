using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace UALib
{
    public static class Import
    {
        static string ImportPath = @".\ImportFiles" ;

        public static void go()
        {
            List<string> ImportFiles = getFiles().ToList<string>();

            foreach (string fn in ImportFiles)
            {
                Process(Readlines(fn));
            }
        }

        /// <summary>
        /// Process the text file line by line, separating out into Howls, Purrs and Chitters and idetifying other tags
        /// </summary>
        /// <param name="lines">Collection of text file files</param>
        /// <returns>Is this a sequence terminator (howl)</returns>
        private static void Process(LinkedList<string> lines)
        {
            List<Howl> Howls = new List<Howl>();
            List<Purr> Purrs = new List<Purr>();
            List<Chitter> Chitters = new List<Chitter>();
            List<string> Notes = new List<string>();
            string Author;
            string Description;
            string System;
            DateTime? Captured = null;

            for (LinkedListNode<string> node = lines.First; node != lines.Last.Next; node = node.Next)
            {
                string ln = node.Value;
                string[] elements = ln.Split('\t');

                switch (elements[2].Substring(0, 1))
                {
                    case "A":
                    case "a":
                        Author = elements[2];
                        break;
                    case "D":
                    case "d":
                        Description = elements[2];
                        break;
                    case "S":
                    case "s":
                        System = elements[2];
                        break;
                    case "T":
                    case "t":
                        Captured = DateTime.Parse(elements[2]);
                        break;
                    case "N":
                    case "n":
                        Notes.Add(elements[2].Substring(2));
                        break;
                    case "h": //It's a howl
                    case "H":
                        Howls.Add(new Howl(
                            int.Parse(elements[2].Substring(1,1)),  //Howl type
                            double.Parse(elements[0]),              //Start Time (Seconds)
                            double.Parse(elements[1])));            //End Time
                        break;
                    case "c": //It's a chitter
                    case "C":
                        Chitters.Add(new Chitter(           
                            double.Parse(elements[0]),   //Start Time (Seconds)
                            double.Parse(elements[1]))); //End Time (Seconds)
                        break;
                    case "p": //It's a purr
                    case "P":
                        Purrs.Add(new Purr(
                            double.Parse(elements[0]),                          //Start Time
                            double.Parse(elements[1]),                          //End Time
                            int.Parse(elements[2].Substring(1,1)),              //Tone Identifier
                            elements[2].Length == 3 && elements[2].Substring(2, 1) == "q" ? true : false,   //Is it a quiet tone
                            null));                                             //Don't have frequency data at the moment
                        break;
                    default: throw new Exception("oh shit, what the hell is that?");
                }
            }
        }

        //Read lines from a file
        private static LinkedList<string> Readlines(string filename)
        {
            LinkedList<string> lines = new LinkedList<string>();
            StreamReader fs = new StreamReader(filename);

            string line;
            while ((line = fs.ReadLine()) != null) lines.AddLast(line);

            return lines;
        }

        private static string[] getFiles()
        {
            return Directory.GetFiles(ImportPath);
        }
    }
}
