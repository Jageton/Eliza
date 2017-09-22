using System.Linq;
using Diggins.Jigsaw;

namespace ELIZA.Semantics.Patterns.Builders
{
    public class BranchedPatternBuilder: IPatternBuilder
    {
        public Pattern Build(Node node)
        {
            var patterns = node.Nodes.Select(n => Pattern.Create(n)).ToList();
            return new BranchedPattern(patterns);
        }
    }
}
