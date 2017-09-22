using System;
using System.Collections.Generic;
using System.Text;
using PrLanguages.Interpreters.VariableManagers;
using PrLanguages.Interpreters.Builders;
using PrLanguages.Expressions;
using PrLanguages.Interpreters.Statements;
using Diggins.Jigsaw;

namespace PrLanguages.Interpreters
{
    public abstract class Interpreter
    {
        protected IVariableManager varManager = new InMemoryManager();
        protected StringBuilder sb = new StringBuilder();
        protected Dictionary<string, IStatementBuilder> builders;
        protected ExpressionHelper eh = new ExpressionHelper();

        public Interpreter()
        {
            builders = new Dictionary<string, IStatementBuilder>();
        }

        
        public dynamic Execute(string program)
        {
            dynamic result = null;
            sb = new StringBuilder();
            foreach(Statement st in Parse(program))
            {
                result = st.Exectute();
                if(st.GetDebugInfo().Length > 0)
                    sb.AppendLine(st.GetDebugInfo() + Environment.NewLine);
            }
            return result;
        }
        protected abstract IEnumerable<Statement> Parse(string program);
        protected abstract IEnumerable<Statement> Parse(Node node);
        public string GetDebugInfo()
        {
            return sb.ToString();
        }
    }
}
