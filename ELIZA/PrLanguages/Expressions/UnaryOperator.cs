using System;
using System.Collections.Generic;

namespace PrLanguages.Expressions
{
    public class UnaryOperator: Operator
    {
        protected Func<dynamic, dynamic> action;
        protected Expression left;

        public Expression Left
        {
            get { return left; }
            set { left = value; }
        }

        public UnaryOperator(Func<dynamic, dynamic> action, string sign = "",
            Associativity associativity = Expressions.Associativity.None, int precendence = 0)
            :base(sign, associativity, 1, precendence)
        {
            this.action = action;
        }


        public override dynamic Calculate()
        {
            return action(left.Calculate());
        }

        public override Expression Simplify()
        {
            this.left = left.Simplify();
            if(this.left is Constant)
            {
                return new Constant(this.Calculate());
            }
            else
            {
                return this;
            }
        }
        public override HashSet<string> VariableNames
        {
            get 
            {
                return left.VariableNames;
            }
        }
        public override void SetVariable<T>(string name, T value)
        {
            left.SetVariable<T>(name, value);
        }
        public override void SetVariable(string name, dynamic value)
        {
            left.SetVariable(name, value);
        }
        public override int VariableCount
        {
            get 
            {
                return this.VariableNames.Count;
            }
        }

        public override object Clone()
        {
            return new UnaryOperator(action, sign, associativity, precendence);
        }

        public override string ToString()
        {
            return sign + left.ToString();
        }
    }
}
