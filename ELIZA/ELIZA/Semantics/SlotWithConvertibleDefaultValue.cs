using System;
using ELIZA.Semantics.Converters;

namespace ELIZA.Semantics
{
    [Serializable]
    public class SlotWithConvertibleDefaultValue<T>: SlotWithDefaultValue<T>
    {
        public SlotWithConvertibleDefaultValue(T defaultValue, string name)
            : base(default(T), name, defaultValue)
        {
        }
        public  SlotWithConvertibleDefaultValue(T value, string name, T defaultValue = default(T)): base(value, name, defaultValue)
        {
        }
        public SlotWithConvertibleDefaultValue()
            : this(default(T), "", default(T))
        {
        }

        public override void SetValue(object value, Frame f)
        {
            value = (T) ConverterFactory.Convert<T>(value.ToString());
            base.SetValue(value, f);
        }
    }
}
