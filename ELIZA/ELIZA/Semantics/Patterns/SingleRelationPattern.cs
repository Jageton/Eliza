using System;
using ELIZA.Semantics.Patterns.Builders;
using ELIZA.Syntax;
using ELIZA.Syntax.DeepRelations;

namespace ELIZA.Semantics.Patterns
{
    [Serializable]
    public class SingleRelationPattern: Pattern
    {
        protected string matchedValue = null;        
        protected DeepRelationName match;

        public SingleRelationPattern(string name, DeepRelationName match,
            bool save = true) : base(name, save)
        {
            this.match = match;
        }

        public override bool Match(Tree<DForm, DeepRelationName> tree)
        {
            matchedValue = null;
            LastMatchedNode = null;
            if (tree.DependencyType == match) //сравниваем имя отношения
            {
                if (save)
                {
                    //если надо сохранить значение, сохраняем, предварительно
                    //получая синоним
                    matchedValue = Synonims.Get(tree.Key.Lexem.Lemma);
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
            var rel = RelationAndLemmaPatternBuilder.Relations;
            foreach (var key in rel.Keys)
            {
                if (rel[key] == match)
                {
                    result += key;
                    break;
                }
            }
            return result;
        }
    }
}
