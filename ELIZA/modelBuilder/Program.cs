using System;
using System.Runtime.Serialization.Formatters.Binary;
using ELIZA.Morphology;
using System.IO;

namespace modelBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            string dictionary = args.Length > 0 ? args[0] : "dict.opcorpora.xml";
            string nGrammCorpora = args.Length > 1 ? args[1] : "annot.opcorpora.xml";
            string trainingCorpora = args.Length > 2 ? args[2] : "annot.opcorpora.no_ambig.xml";
            int sentenceNum = args.Length > 3 ? int.Parse(args[3]) : 200;
            bool concurrent = args.Length >= 5 && (args[4] == "1");
            if (File.Exists("log.txt"))
                File.Delete("log.txt");
            ICorporaReader reader = new OpenCorporaReader();
            MorphologyModel model = new MorphologyModel("morphology", "full7z-mlteast-ru.lem");
            model.TrainFull(dictionary, nGrammCorpora, trainingCorpora,
                reader, sentenceNum, concurrent, Console.WriteLine);
            using (FileStream fs = File.Create("morphology/morhp.mdl"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, model);
            }
        }
    }
}
