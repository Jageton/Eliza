using System;

namespace ELIZA.Morphology
{
    public interface IStemmer
    {
        Tuple<string, string> Stem(string word);
    }
}
