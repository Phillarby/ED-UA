using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace UALib
{
    public enum AudioElement
    {
        Howl,
        Chitter,
        Purr
    };

    public class Import
    {
        string ImportPath = @".\ImportFiles\Labels" ;

        public List<Sequence> go()
        {
            var ImportFiles = getFiles().ToList<string>();
            var seqs = new List<Sequence>();

            foreach (string fn in ImportFiles)
            {
                var ie = Process(Readlines(fn));
                seqs.Add(BuildSequence(ie));
            }

            return seqs;
        }

        private Sequence BuildSequence(ImportElements ie)
        {
            //Create a new seqeunce and segment counter
            Sequence seq = new Sequence(ie.Author, ie.Description);
            int cnt = 0;

            //Construct segments based on Howl postions
            while (ie.Howls.Count > 0)
            {
                //Identify all the elements starting before the end of the next howl
                var h = ie.Howls.OrderBy(x => x.Start).First();
                var p = ie.Purrs.Where(x => x.Start <= h.End).ToList().OrderBy(x => x.Start);
                var c = ie.Chitters.Where(x => x.Start <= h.End).FirstOrDefault();

                //Construct a new segment using the identified elements and add it to the sequence
                Segment s = new Segment(cnt, p.ToArray(), c, h);
                seq.AddSegment(s);

                //Remove the added elements from the source collections
                p.ToList().ForEach(x => { ie.Purrs.Remove(x); });
                ie.Chitters.Remove(c);
                ie.Howls.Remove(h);

                //Move to next segment
                cnt++;
            }

            //If there are remaining elements then add them to a final segment
            if (ie.Purrs.Count > 0 || ie.Chitters.Count > 0)
            {
                //Construct a new segment using the identified elements and add it to the sequence
                Segment s = new Segment(cnt, ie.Purrs.ToArray(), ie.Chitters.First());
                seq.AddSegment(s);
            }

            return seq;
        }

        /// <summary>
        /// Process the text file line by line, separating out into Howls, Purrs and Chitters and identifying other tags
        /// </summary>
        /// <param name="lines">Collection of text file files</param>
        /// <returns>Is this a sequence terminator (howl)</returns>
        private ImportElements Process(LinkedList<string> lines)
        {
            //instantiate temporary container for use during parsing
            var ie = new ImportElements();

            //Process line by line
            for (LinkedListNode<string> node = lines.First; node != lines.Last.Next; node = node.Next)
            {
                string ln = node.Value;
                string[] elements = ln.Split('\t');

                switch (elements[2].Substring(0, 1))
                {
                    case "A":
                    case "a":
                        ie.Author = elements[2].Substring(2).TrimStart();
                        break;
                    case "D":
                    case "d":
                        ie.Description = elements[2].Substring(2).TrimStart();
                        break;
                    case "S":
                    case "s":
                        ie.System = elements[2].Substring(2).TrimStart();
                        break;
                    case "T":
                    case "t":
                        ie.Captured = DateTime.Parse(elements[2].Substring(2).TrimStart());
                        break;
                    case "N":
                    case "n":
                        ie.Notes.AddLast(new Note(
                            double.Parse(elements[0]),
                            double.Parse(elements[1]),
                            elements[2].Substring(2).TrimStart()));
                        break;
                    case "h": //It's a howl
                    case "H":
                        ie.Howls.AddLast(new Howl(
                            int.Parse(elements[2].Substring(1,1)),  //Howl type
                            double.Parse(elements[0]),              //Start Time (Seconds)
                            double.Parse(elements[1])));            //End Time
                        break;
                    case "c": //It's a chitter
                    case "C":
                        ie.Chitters.AddLast(new Chitter(           
                            double.Parse(elements[0]),   //Start Time (Seconds)
                            double.Parse(elements[1]))); //End Time (Seconds)
                        break;
                    case "p": //It's a purr
                    case "P":
                        ie.Purrs.AddLast(new Purr(
                            double.Parse(elements[0]),                          //Start Time
                            double.Parse(elements[1]),                          //End Time
                            int.Parse(elements[2].Substring(1,1)),              //Tone Identifier
                            elements[2].Length == 3 && 
                            elements[2].Substring(2, 1) == "q" ? true : false,  //Is it a quiet tone
                            null));                                             //Don't have frequency data at the moment
                        break;
                    default: throw new Exception("oh shit, what the hell is that?");
                }
            }
            return ie;
        }

        //Read lines from a file
        private LinkedList<string> Readlines(string filename)
        {
            LinkedList<string> lines = new LinkedList<string>();
            StreamReader fs = new StreamReader(filename);

            string line;
            while ((line = fs.ReadLine()) != null) lines.AddLast(line);

            return lines;
        }

        private string[] getFiles()
        {
            return Directory.GetFiles(ImportPath);
        }

        /// <summary>
        /// Internal class for temporary storage of elements during parsing
        /// </summary>
        private class ImportElements
        {
            public LinkedList<Howl> Howls { get; internal set; }
            public LinkedList<Purr> Purrs { get; internal set; }
            public LinkedList<Chitter> Chitters { get; internal set; } 
            public LinkedList<Note> Notes { get; internal set; }
            public string Author { get; set; }
            public string Description { get; set; }
            public string System { get; set; }
            public DateTime? Captured { get; set; }

            public ImportElements()
            {
                Howls = new LinkedList<Howl>();
                Purrs = new LinkedList<Purr>();
                Chitters = new LinkedList<Chitter>();
                Notes = new LinkedList<Note>();
                Captured = null;
            }
        }

    }
}
