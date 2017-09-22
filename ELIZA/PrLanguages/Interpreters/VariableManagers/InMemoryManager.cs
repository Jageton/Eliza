using System.Collections.Generic;
using PrLanguages.Expressions;

namespace PrLanguages.Interpreters.VariableManagers
{
    public class InMemoryManager: IVariableManager
    {
        protected Dictionary<string, dynamic> variables;

        public Dictionary<string, dynamic> Variables
        {
            get { return variables; }
            set { variables = value; }
        }

        public InMemoryManager()
        {
            variables = new Dictionary<string, dynamic>();
        }

        public dynamic GetValue(string name)
        {
            return variables[name];
        }
        public void SetValue(string name, dynamic value)
        {
            variables[name] = value;
        }
        public void Declare(string name, dynamic value = null)
        {
            variables.Add(name, value);
        }
        public void Reset()
        {
            variables = new Dictionary<string, dynamic>();
        }
        public void SetAllVariables(Expression exp)
        {
            foreach (string varName in exp.VariableNames)
                exp.SetVariable(varName, GetValue(varName));
        }
    }
}
