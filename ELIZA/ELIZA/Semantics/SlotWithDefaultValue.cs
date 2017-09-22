using System;

namespace ELIZA.Semantics
{
    
    /// <summary>
    /// Слот со значением по умолчанию. Значение по умолчанию используется, когда 
    /// требуется значение, но слот формально его не содержит.
    /// </summary>
    /// <typeparam name="T">Тип значения слота</typeparam>
    /// <seealso cref="ELIZA.Semantics.Slot{T}" />
    [Serializable]
    public class SlotWithDefaultValue<T>: Slot<T>
    {
        protected T defaultValue;

        public T DefaultValue
        {
            get { return defaultValue; }
        }

        public SlotWithDefaultValue(T defaultValue, string name) : this(default(T), "", defaultValue)
        {
            hasValue = false;
            Name = name;
            
        }
        public SlotWithDefaultValue(T value, string name, T defaultValue = default(T)): base(value, name)
        {
            this.defaultValue = defaultValue;
        }
        public SlotWithDefaultValue() : this(default(T), "", default(T))
        {
            
        }



        private void UseDefaultValue(object sender, SlotEventArgs e)
        {
            if (!hasValue)
            {
                hasValue = true;
                value = defaultValue;
            }
        }

        public override object GetValue(Frame f)
        {
            var v = base.GetValue(f);
            if (!hasValue)
                return defaultValue;
            return v;
        }
    }
}
