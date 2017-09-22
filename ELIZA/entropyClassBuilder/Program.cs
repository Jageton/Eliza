using System;
using ELIZA.Morphology;
using System.IO;

namespace entropyClassBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputFile = args.Length > 0 ? args[0] : "dict.opcorpora.xml";
            string outputFile = args.Length > 1 ? args[1] : "entClass.mdl";
            int count = args.Length > 2? int.Parse(args[2]): 10000;
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            OpenCorporaReader reader = new OpenCorporaReader();
            reader.Open(inputFile);
            DawgEntropyClassModel entClass = new DawgEntropyClassModel();
            foreach(var lexem in reader.ReadDictionary(count))
            {
                Console.WriteLine(lexem);
                entClass.AddLexem(lexem);
            }
            entClass.Build();
            using(FileStream fs = File.Create(outputFile))
            {
                entClass.SaveTo(fs);
            }
            Console.ReadKey();
        }
    }
}
