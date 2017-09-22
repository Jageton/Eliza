using Diggins.Jigsaw;

namespace ELIZA.Semantics.Patterns.Builders
{
    public class ChainedPatternBuilder: IPatternBuilder
    {

        public Pattern Build(Node node)
        {
            var first = Pattern.Create(node.Nodes[0]);
            var second = Pattern.Create(node.Nodes[1]);
            return new ChainedPattern(first, second);
        }
    }
}
