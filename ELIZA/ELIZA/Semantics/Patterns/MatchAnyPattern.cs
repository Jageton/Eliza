using System;
using System.Collections.Generic;
using System.Linq;
using ELIZA.Syntax;
using ELIZA.Syntax.DeepRelations;

namespace ELIZA.Semantics.Patterns
{
    [Serializable]
    public class MatchAnyPattern: ComplexPattern
    {

        public MatchAnyPattern(Pattern innerPattern)
        {
            patterns.Add(innerPattern);
        }

        public override bool Match(List<Tree<DForm, DeepRelationName>> trees)
        {
            return trees.Any(tree => patterns[0].Match(tree));
        }

        public override string ToString()
        {
            if(patterns != null && patterns.First() != null)
                return patterns.First().ToString();
            return string.Empty;
        }
    }
}
