using System;
using System.Collections.Generic;

namespace PrLanguages.Expressions
{
    public abstract class Expression: ICloneable
    {
        public abstract dynamic Calculate();
        public abstract Expression Simplify();
        public abstract HashSet<string> VariableNames
        {
            get;
        }
        public abstract void SetVariable<T>(string name, T value);
        public abstract void SetVariable(string name, dynamic value);
        public abstract int VariableCount
        {
            get;
        }

        #region ICloneable Members

        public abstract object Clone();

        #endregion
    }
}
