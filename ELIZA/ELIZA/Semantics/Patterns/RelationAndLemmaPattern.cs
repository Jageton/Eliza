using System;
using ELIZA.Semantics.Patterns.Builders;
using ELIZA.Syntax;
using ELIZA.Syntax.DeepRelations;

namespace ELIZA.Semantics.Patterns
{
    [Serializable]
    public class RelationAndLemmaPattern: Pattern
    {
        protected string lemma;
        protected DeepRelationName relation;
        protected string matchedValue;

        public RelationAndLemmaPattern(string name, string lemma,
            DeepRelationName relation, bool save) :
            base(name, save)
        {
            this.lemma = lemma;
            this.relation = relation;
        }

        public override bool Match(Tree<DForm, DeepRelationName> tree)
        {
            matchedValue = null;
            LastMatchedNode = null;
            if (tree.DependencyType == relation &&
                Synonims.Get(tree.Key.Lexem.Lemma) == Synonims.Get(lemma)) //сравниваем имя отношения
            {
                if (save)
                {
                    //если надо сохранить значение, сохраняем
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
                if (rel[key] == relation)
                {
                    result += key + "^";
                    break;
                }
            }
            result += lemma;
            return result;
        }
    }
}
