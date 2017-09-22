using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;

namespace ELIZA.Morphology
{
    [ProtoContract]
    [Serializable]
    public class TagNGramm
    {
        [ProtoMember(1)]
        protected NGramm<ulong> innerNGramm;
        public ulong Count
        {
            get { return innerNGramm.Count; }
        }
        public NGramm<ulong> InnerNgramm
        {
            get { return innerNGramm; }
        }

        public TagNGramm()
        {
            this.innerNGramm = new NGramm<ulong>();
        }

        public void AddNGramm(IEnumerable<Tag> key)
        {
            IEnumerable<ulong> convertedKey = key.Select(item => (ulong)item);
            Tag last = key.Last();
            //если слово имело аттрибуты
            if(last != Tag.NoWord && last != Tag.Unfixed)
            {
                //откидываем пунктуацию, союзы и предлоги
                if((last & (Tag.Punctuation | Tag.Conjunction | Tag.Preposition)) == 0)
                {
                    innerNGramm.AddNGramm(convertedKey);
                }
            }                
        }
        public double Compute(IEnumerable<Tag> key)
        {
            IEnumerable<ulong> convertedKey = key.Select(item => (ulong)item);
            return innerNGramm.Compute(convertedKey);
        }
        public Tuple<Tag, double> GetMostPossibleTag(IEnumerable<Tag> key)
        {
            IEnumerable<ulong> convertedKey = key.Select(item => (ulong)item);
            ulong max = 0;
            ulong maxValue = 0;
            ulong sum = 0;
            foreach(var node in InnerNgramm.Trie.GetChildNodes(convertedKey))
            {
                if (node.HasValue)
                {
                    sum += node.Value;
                    if (node.Value > max)
                    {
                        max = node.Value;
                        maxValue = node.Key;
                    }
                }
            }
            if (sum == 0)
                sum = 1;
            return new Tuple<Tag, double>((Tag)maxValue, (double)max / sum);
        }
    }
}
