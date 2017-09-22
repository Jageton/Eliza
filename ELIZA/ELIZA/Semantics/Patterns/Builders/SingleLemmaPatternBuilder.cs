using System.Linq;
using Diggins.Jigsaw;

namespace ELIZA.Semantics.Patterns.Builders
{
    public class SingleLemmaPatternBuilder: IPatternBuilder
    {

        public Pattern Build(Node node)
        {
            var nameNode = node.Nodes.FirstOrDefault((n) => n.Label == PatternGrammar.Name.Name);
            var name = string.Empty;
            var save = false;
            if (nameNode != null)
            {
                save = true;
                name = nameNode.Text;
            }
            var match = node.Descendants.First((n) =>
                n.Label == PatternGrammar.MatchLemma.Name).Descendants.First().Text;
            return new SingleLemmaPattern(match, name, save);
        }
    }
}
