namespace PrLanguages.Interpreters.Builders
{
    public interface IStatementBuilder
    {
        Statements.Statement Build(Diggins.Jigsaw.Node node);
    }
}
