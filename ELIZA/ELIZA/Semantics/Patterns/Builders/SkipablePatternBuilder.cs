using Diggins.Jigsaw;

namespace ELIZA.Semantics.Patterns.Builders
{
    public class SkipablePatternBuilder: IPatternBuilder
    {

        public Pattern Build(Node node)
        {
            var inner = Pattern.Create(node.Nodes[0]);
            return  new SkipablePattern(inner);
        }
    }
}
