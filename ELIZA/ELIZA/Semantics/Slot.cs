using System;

namespace ELIZA.Semantics
{
    [Serializable]
    public class Slot<T>: AbstractSlot
    {
        protected T value;
        protected bool hasValue;

        public Slot()
        {
            hasValue = false;
            value = default(T);
        }

        public Slot(T value) : this()
        {
            hasValue = true;
            this.value = value;
        }

        public Slot(T value, string name) : this(value)
        {
            Name = name;
        }

        public T Value
        {
            get
            {
                return hasValue ? value : default(T);
            }
        }

        //операции со слотом
        public override object GetValue(Frame f)
        {
            OnValueNeeded(f);
            if (hasValue)
                return value;
            else
                return null;
        }
        public override void SetValue(object value, Frame f)
        {
            if (value is T)
            {
                this.value = (T)value;
                hasValue = true;
                OnValueChanged(f);
            }
            else throw new ArgumentException("Значение не совпадает с типом слота.", "value");
        }
        public override void RemoveValue(Frame f)
        {
            hasValue = false;
            OnValueRemoved(f);
        }
    }
}
