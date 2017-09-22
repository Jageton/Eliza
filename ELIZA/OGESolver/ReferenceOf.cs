using System;

namespace OGESolver
{
    [Serializable]
    public class ReferenceOf<T> where T:struct 
    {
        public T Value { get; set; }

        public ReferenceOf(T value)
        {
            Value = value;
        }

        public static implicit operator T(ReferenceOf<T> reference)
        {
            return reference.Value;
        }

        public static implicit operator ReferenceOf<T>(T value)
        {
            return new ReferenceOf<T>(value);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
