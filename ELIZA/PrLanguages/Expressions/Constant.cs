using System.Collections.Generic;

namespace PrLanguages.Expressions
{
    public class Constant: Expression
    {
        protected dynamic value;

        public Constant(object value)
        {
            this.value = value;
        }

        public override dynamic Calculate()
        {
            return value;
        }
        public override Expression Simplify()
        {
            return this;
        }
        public override HashSet<string> VariableNames
        {
            get { return new HashSet<string>(); }
        }
        public override void SetVariable<T>(string name, T value)
        {
            return;
        }
        public override void SetVariable(string name, dynamic value)
        {
            return;
        }
        public override int VariableCount
        {
            get { return 0; }
        }

        public override object Clone()
        {
            return new Constant(value);
        }
    }
}
