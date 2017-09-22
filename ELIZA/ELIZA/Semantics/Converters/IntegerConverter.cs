using OGESolver;

namespace ELIZA.Semantics.Converters
{
    public class IntegerConverter : IConverter<ReferenceOf<int>>
    {
        public ReferenceOf<int> Convert(string value)
        {
            return new ReferenceOf<int>(int.Parse(value));
        }
    }
}
