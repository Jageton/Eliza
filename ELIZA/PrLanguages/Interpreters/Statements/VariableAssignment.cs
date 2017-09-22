using System.Text;
using Diggins.Jigsaw;
using PrLanguages.Interpreters.VariableManagers;
using PrLanguages.Expressions;

namespace PrLanguages.Interpreters.Statements
{
    public class VariableAssignment: Statement
    {
        protected string varName;
        protected Expression exp;

        public override dynamic Exectute()
        {
            sb = new StringBuilder();
            varManager.SetAllVariables(exp);
            dynamic value = exp.Calculate();
            sb.Append(string.Format("После оператора {0} значение переменной {1} = {2}.",
                ToString(), varName, value));
            varManager.SetValue(varName, value);
            return value;
        }

        public VariableAssignment(string varName, Expression exp, Node node, IVariableManager manager):base(node, manager)
        {
            this.varName = varName;
            this.exp = exp;
        }
    }
}
