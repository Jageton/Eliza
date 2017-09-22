using System;
using OGESolver;

namespace ELIZA.Semantics.Converters
{
    public class InformationUnitConverter : IConverter<ReferenceOf<InformationUnit>>
    {
        public ReferenceOf<InformationUnit> Convert(string value)
        {
            return (InformationUnit)Enum.Parse(typeof (InformationUnit), value, true);
        }
    }
}
