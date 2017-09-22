using System;
using ELIZA.Syntax;
using ELIZA.Syntax.DeepRelations;

namespace ELIZA.Semantics.Patterns
{
    [Serializable]
    public class SingleLemmaPattern: Pattern
    {
        protected string match;
        protected string matchedValue;

        public SingleLemmaPattern(string match, string name, bool save): base(name, save)
        {
            this.match = match;
        }

        public override bool Match(Tree<DForm, DeepRelationName> tree)
        {
            matchedValue = null;
            LastMatchedNode = null;
            if (Synonims.Get(tree.Key.Lexem.Lemma) == Synonims.Get(match)) //сравниваем имя отношения
            {
                if (save)
                {
                    //если надо сохранить значение, сохраняем
                    matchedValue = Synonims.Get(match);                    
                }
                LastMatchedNode = tree;
                return true;
            }
            return false;
        }

        public override bool TryGetSavedValue(string name, out string value)
        {
            value = matchedValue;
            if (name.Equals(Name))
            {
                return value != null;
            }
            return false;
        }

        public override string ToString()
        {
            var result = string.Empty;
            if (save)
                result = "(" + Name + ")";
            result += match;
            return result;
        }
    }
}
