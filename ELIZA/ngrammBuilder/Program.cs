using System;
using System.Linq;
using ELIZA.Morphology;
using System.IO;
using ProtoBuf;

namespace ngrammBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputFile = args[0];
            string outputFile = args[1];
            int nGrammSize = int.Parse(args[2]);
            int sentences = args.Length > 3? int.Parse(args[3]) : 50000000;
            OpenCorporaReader reader = new OpenCorporaReader();
            reader.Open(inputFile);
            TagNGramm tagNgramm = new TagNGramm();
            long memory = GC.GetTotalMemory(true);
            long count = 0;
            foreach (var sentense in reader.ReadSentences(sentences))
            {
                count++;
                string se = string.Empty;
                foreach (WordForm lexem in sentense)
                {
                    se += lexem.Word.ToString() + " ";
                }
                Console.WriteLine(se);
                if (sentense.Count > 0 && sentense.Count(a => a.Tag == Tag.NoWord) == 0)
                {
                    foreach (var ngamm in sentense.BuildNGramms(nGrammSize))
                    {
                        tagNgramm.AddNGramm(ngamm);
                    }
                }
            }
            using(FileStream fs = File.Create(outputFile))
            {
                Serializer.Serialize<TagNGramm>(fs, tagNgramm);
            }
            Console.WriteLine(string.Format("Прочитано предложений: {0}.", count));
            Console.WriteLine(string.Format("Построено {0}-грамм: {1}.", nGrammSize, tagNgramm.Count));
            Console.ReadKey();
        }
    }
}

