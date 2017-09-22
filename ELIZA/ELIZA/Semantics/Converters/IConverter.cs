namespace ELIZA.Semantics.Converters
{
    public interface IConverter<out T>
    {
        T Convert(string value);
    }
}
