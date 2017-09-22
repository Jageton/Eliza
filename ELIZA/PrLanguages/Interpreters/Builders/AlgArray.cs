namespace PrLanguages.Interpreters.Builders
{
    public class AlgArray<T>
    {
        protected int startValue;
        protected T[] array;

        public T this[int index]
        {
            get
            {
                return array[index - startValue];
            }
            set
            {
                array[index - startValue] = value;
            }
        }

        public AlgArray(int startValue, int endValue)
        {
            this.startValue = startValue;
            array = new T[endValue - startValue + 1];
        }
        public AlgArray(int capacity):this(1, 1 + capacity)
        {

        }
    }
}
