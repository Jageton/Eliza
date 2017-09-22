using Diggins.Jigsaw;

namespace ELIZA.Semantics.Patterns.Builders
{
    public interface IPatternBuilder
    {
        Pattern Build(Node node);
    }
}
