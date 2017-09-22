namespace OGESolver.Builders
{
    public interface ICommandBuilder<out T>
    {
        IAlgorithm<T> Build(object[] args);
        IAlgorithm<T> Build(string formattedString);
    }
}
