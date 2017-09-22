using System.Collections.Generic;
using System.Text;
using Diggins.Jigsaw;
using PrLanguages.Interpreters.VariableManagers;
using PrLanguages.Expressions;

namespace PrLanguages.Interpreters.Statements
{
    public class IfStatement: Statement
    {
        protected IEnumerable<Statement> thenBranch;
        protected IEnumerable<Statement> elseBranch;
        protected Expression exp;
        protected string expression;

        public IfStatement(Expression exp, IEnumerable<Statement> thenBranch,
            IEnumerable<Statement> elseBranch, Node node, IVariableManager varManager,
            string expression):base(node, varManager)
        {
            this.exp = exp;
            this.elseBranch = elseBranch;
            this.thenBranch = thenBranch;
            this.expression = expression;
        }

        public override dynamic Exectute()
        {
            sb = new StringBuilder();
            varManager.SetAllVariables(exp);
            bool res = exp.Calculate();
            dynamic result = 0;
            if(res.Equals(true))
            {
                sb.AppendLine(string.Format("Условие {0} истинно. Выполняется ветвь \"then\".", expression));
                foreach(var st in thenBranch)
                {
                    result = st.Exectute();
                    sb.Append(st.GetDebugInfo());
                }
            }
            else 
            {
                sb.AppendLine(string.Format("Условие ({0}) ложно.", expression));
                if (elseBranch != null)
                {
                    sb.AppendLine("Выполняется ветвь \"else\".");
                    foreach (var st in elseBranch)
                    {
                        result = st.Exectute();
                        sb.AppendLine(st.GetDebugInfo());
                    }
                }
                else sb.Append("Операторы тела \"if\" не выполняются.");
            }
            return result;
        }

    }
}
