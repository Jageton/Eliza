using System;
using System.Collections.Generic;

namespace PrLanguages.Expressions
{
    public class BinaryOperator: Operator
    {
        protected Func<dynamic, dynamic, dynamic> action;
        protected Expression left;
        protected Expression right;

        public Expression Left
        {
            get { return left; }
            set { left = value; }
        }
        public Expression Right
        {
            get { return right; }
            set { right = value; }
        }

        public BinaryOperator(Func<dynamic, dynamic, dynamic> action, string sign = "",
            Associativity associativity = Expressions.Associativity.None, int precendence = 0):
            base(sign, associativity, 2, precendence)
        {
            this.action = action;
        }

        public override dynamic Calculate()
        {
            return action(left.Calculate(), right.Calculate());
        }
        public override Expression Simplify()
        {
            this.left = left.Simplify();
            this.right = right.Simplify();
            if (left is Constant && right is Constant)
            {
                return new Constant(action(left.Calculate(), right.Calculate()));
            }
            else return this;
        }
        public override HashSet<string> VariableNames
        {
            get 
            {
                HashSet<string> names = left.VariableNames;
                names.UnionWith(right.VariableNames);
                return names;
            }
        }
        public override void SetVariable<T>(string name, T value)
        {
            left.SetVariable<T>(name, value);
            right.SetVariable<T>(name, value);
        }
        public override void SetVariable(string name, dynamic value)
        {
            left.SetVariable(name, value);
            right.SetVariable(name, value);
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
            return new BinaryOperator(action, sign, associativity, precendence);
        }

        public override string ToString()
        {
            return "(" + left.ToString() + " " + sign + " " + right.ToString() + ")";
        }
    }
}
