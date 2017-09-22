using System;
using OGESolver;

namespace ELIZA.Semantics.Converters
{
    public class TimeUnitConverter: IConverter<ReferenceOf<TimeUnit>>
    {
        public ReferenceOf<TimeUnit> Convert(string value)
        {
            return (TimeUnit) Enum.Parse(typeof (TimeUnit), value, true);
        }
    }
}
