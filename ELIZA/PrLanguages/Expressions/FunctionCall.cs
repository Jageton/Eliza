using System;
using System.Collections.Generic;

namespace PrLanguages.Expressions
{
    public class FunctionCall: Expression
    {
        protected string name;
        protected Func<dynamic[], dynamic> action;
        protected Expression[] args;
        protected int argsCount;

        public Expression[] Arguments
        {
            get { return args; }
            set { args = value; }
        }
        public string Name
        {
            get { return name; }
        }
        public int ArgumentsCount
        {
            get { return argsCount; }
            set
            {
                Expression[] argsNew = new Expression[value];
                for(int i = 0; i < args.Length; i++)
                {
                    argsNew[i] = args[i];
                }
                args = argsNew;
                argsCount = value;
            }
        }

        public FunctionCall(Func<dynamic[], dynamic> action, string name = "", int argsCount = 0)
        {
            this.name = name;
            this.action = action;
            this.args = new Expression[argsCount];
            this.argsCount = argsCount;
        }

        public override dynamic Calculate()
        {
            if (this.args == null) return action(null); 
            dynamic[] computedArgs = new object[args.Length];
            for(int i = 0; i < args.Length; i++)
            {
                computedArgs[i] = args[i].Calculate();
            }
            return action(computedArgs);
        }

        public override Expression Simplify()
        {
            //if(this.args == null || this.args.Length == 0) return new Constant(this.Calculate());
            //bool toConstant = true;
            //for (int i = 0; i < args.Length; i++)
            //{
            //    args[i] = args[i].Simplify();
            //    toConstant &= args[i] is Constant;
            //}
            //if (toConstant) return new Constant(this.Calculate());
            //else return this;
            return this;
        }
        public override HashSet<string> VariableNames
        {
            get 
            {
                HashSet<string> names = new HashSet<string>();
                foreach(Expression exp in args)
                {
                    names.UnionWith(exp.VariableNames);
                }
                return names;
            }
        }
        public override void SetVariable<T>(string name, T value)
        {
            foreach(Expression exp in args)
            {
                exp.SetVariable<T>(name, value);
            }
        }
        public override void SetVariable(string name, dynamic value)
        {
            foreach (Expression exp in args)
            {
                exp.SetVariable(name, value);
            }
        }
        public override int VariableCount
        {
            get 
            {
                return this.VariableNames.Count;
            }
        }

        #region ICloneable Members

        public override object Clone()
        {
            return new FunctionCall(action, name, argsCount);
        }

        #endregion
    }
}
