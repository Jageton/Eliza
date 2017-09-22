using System.Collections.Generic;

namespace PrLanguages.Expressions
{
    public class Variable: Expression
    {
        protected string name;
        protected dynamic value;


        public Variable(string name):this(name, null)
        {

        }
        public Variable(string name, object value)
        {
            this.name = name;
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
            get 
            {
                HashSet<string> names = new HashSet<string>();
                names.Add(this.name);
                return names;
            }
        }
        public override void SetVariable<T>(string name, T value)
        {
            if (this.name.Equals(name))
                this.value = value;
        }
        public override void SetVariable(string name, dynamic value)
        {
            if (this.name == name)
                this.value = value;
        }
        public override int VariableCount
        {
            get 
            { 
                return 1;
            }
        }

        public override object Clone()
        {
            return new Variable(name, value);
        }

        public override string ToString()
        {
            return name;
        }
    }
}
