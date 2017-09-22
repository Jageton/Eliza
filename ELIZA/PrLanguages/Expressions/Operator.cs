namespace PrLanguages.Expressions
{
    public abstract class Operator: Expression
    {
        protected int paramCount;
        protected string sign;
        protected Associativity associativity;
        protected int precendence;

        public Operator(string sign = "", Associativity associativity = Expressions.Associativity.None,
            int paramCount = 1, int precendence = 0)
        {
            this.associativity = associativity;
            this.sign = sign;
            this.paramCount = paramCount;
            this.precendence = precendence;
        }

        public Associativity Associativity
        {
            get { return this.associativity; }
        }
        public string Sign
        {
            get { return sign; }
        }
        public int ParametersCount
        {
            get { return paramCount; }
        }
        public int Precendence
        {
            get { return precendence; }
        }
    }
}
