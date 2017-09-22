using System;
using System.Collections.Generic;
using Diggins.Jigsaw;
using ELIZA.Semantics.Patterns.Builders;
using ELIZA.Syntax;
using ELIZA.Syntax.DeepRelations;

namespace ELIZA.Semantics.Patterns
{
    [Serializable]
    public abstract class Pattern: ISyntaxPattern
    {
        private static Dictionary<string, IPatternBuilder> builders;
        protected bool save = false;
        protected Tree<DForm, DeepRelationName> lastMatchedNode;


        public string Name { get; set; }

        public Tree<DForm, DeepRelationName> LastMatchedNode
        {
            get { return lastMatchedNode; }
            set { lastMatchedNode = value; }
        }

        protected Pattern(string name, bool save = true)
        {
            Name = name;
            this.save = save;
            lastMatchedNode = null;
        }

        static Pattern()
        {
            builders = new Dictionary<string, IPatternBuilder>();
            builders.Add(PatternGrammar.MatchDepPattern.Name, new SingleRelationPatternBuilder());
            builders.Add(PatternGrammar.MatchLemmaPattern.Name, new SingleLemmaPatternBuilder());
            builders.Add(PatternGrammar.MatchDepAndLemmaPattern.Name, new RelationAndLemmaPatternBuilder());
            builders.Add(PatternGrammar.BranchedPattern.Name, new BranchedPatternBuilder());
            builders.Add(PatternGrammar.ChainedPattern.Name, new ChainedPatternBuilder());
            builders.Add(PatternGrammar.SkipablePattern.Name, new SkipablePatternBuilder());
            builders.Add(PatternGrammar.OrPattern.Name, new OrPatternBuilder());
            //TODO: добавить билдеры для всех типов паттернов
        }

        public abstract bool Match(Tree<DForm, DeepRelationName> tree);
        public abstract bool TryGetSavedValue(string name, out string value);

        public static Pattern Create(Node node)
        {
            if (builders.ContainsKey(node.Label))
            {
                return builders[node.Label].Build(node);
            }
            return null;
        }
        public static Pattern Create(string line)
        {
            if (PatternGrammar.StringPattern.Match(line)) //найдено хотя бы частичное совпадение
            {
                var node = PatternGrammar.StringPattern.Parse(line)[0];
                if (node.Text.Length == line.Length) //паттерн полностью совпал
                {
                    return Create(node);
                }
            }
            return null;
        }

        public static ComplexPattern CreateAny(Node node)
        {
            var pattern = Pattern.Create(node);
            if (pattern is ComplexPattern)
                return (ComplexPattern)pattern;
            return new MatchAnyPattern(pattern);
        }
        public static ComplexPattern CreateAny(string line)
        {
            var pattern = Pattern.Create(line);
            if (pattern is ComplexPattern)
                return (ComplexPattern)pattern;
            return new MatchAnyPattern(pattern);
        }
    }
}
