﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UALib;

namespace UA
{
    class Program
    {
        static void Main(string[] args)
        {
            var seqs = new Import().go();

            Console.WriteLine(String.Format("Imported {0} files:", seqs.Count()));
            seqs.ForEach(x => { int c = 1;  Console.WriteLine(String.Format("{2}: \t{0} by {1}",x.Description, x.Recorder, c)); c++; });
            Console.WriteLine();

            foreach (Sequence seq in seqs)
            {
                string title = seq.Description + " by " + seq.Recorder;
                Console.WriteLine(title);
                foreach (char c in title) Console.Write("=");
                Console.WriteLine();
                Console.WriteLine();

                title = "Sequence Content:";
                Console.WriteLine(title);

                foreach (Segment seg in seq.Segments)
                {
                    if (seg.Chitter != null) Console.Write("c,");
                    seg.Purrs.OrderBy(x => x.Start).ToList().ForEach(x => { Console.Write(String.Format("{0},", x.Identifer)); });
                    if (seg.Howl != null) Console.Write(String.Format("h{0}", seg.Howl.Identifer.ToString()));
                    if (seg != seq.Segments.Last()) Console.Write(" / ");
                }
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine(String.Format("Segments: {0}", seq.Segments.Count));

                for (int i = 1; i <= 7; i++)
                {
                    if (seq.Segments.Where(x => x.Length == i).Count() > 0)
                    {
                        Console.Write(String.Format("{0} bit segments: {1} - ", i, seq.Segments.Where(x => x.Length == i).Count()));
                        Console.WriteLine(String.Format(" Howl Distribution (h1:{0}, h2:{1})",
                            seq.Segments.Where(x => x.Length == i && x.Howl.Identifer == 1).Count(),
                            seq.Segments.Where(x => x.Length == i && x.Howl.Identifer == 2).Count()));
                    }
                }

                Console.WriteLine();
                title = "Bit counts:";
                Console.WriteLine(title);
                var purs = seq.Segments.SelectMany(x => x.Purrs);
                for (int i = 1; i <= 4; i++)
                {
                    Console.WriteLine(String.Format("{1}: {0}", purs.Where(x => x.Identifer == i).Count(), i));
                }

                //Count how often the bits appear in each position
                Console.WriteLine();
                title = "Bit Distribution:";
                Console.WriteLine(title);
                var p = seq.Segments.SelectMany(x => x.Purrs);
                for (int i = 1; i <= 4; i++)
                {
                    Console.Write(String.Format("bit {0}: ", i));

                    for (int b = 0; b < 7; b++)
                    {
                        Console.Write(String.Format("{0} ", seq.Segments.SelectMany(x => x.Purrs).Where(x => x.Position == b && x.Identifer == i).Count()));
                    }
                    Console.WriteLine();
                }

            }


            Console.Read();
        }
    }
}
