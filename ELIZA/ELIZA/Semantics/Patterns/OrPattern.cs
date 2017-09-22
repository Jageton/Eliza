using System;
using System.Collections.Generic;
using System.Text;
using ELIZA.Syntax;
using ELIZA.Syntax.DeepRelations;

namespace ELIZA.Semantics.Patterns
{
    [Serializable]
    public class OrPattern: ComplexPattern
    {
        public OrPattern(List<Pattern> patterns) : base(patterns)
        {
            
        }

        public override bool Match(List<Tree<DForm, DeepRelationName>> trees)
        {
            return new MatchAnyPattern(this).Match(trees);
        }

        public override bool Match(Tree<DForm, DeepRelationName> tree)
        {
            foreach (var pattern in patterns)
            {
                if (pattern.Match(tree))
                {
                    LastMatchedNode = pattern.LastMatchedNode;
                    return true;
                }
            }
            return false;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var pattern in patterns)
            {
                if (patterns.Count > 0)
                    sb.AppendFormat("| {0}", pattern);
                else sb.Append(pattern);
            }
            return string.Format("({0})", sb.ToString().Substring(2));
        }
    }
}
