using System;
using System.Collections.Generic;
using System.Text;
using PrLanguages.Expressions;
using PrLanguages.Interpreters.VariableManagers;
using Diggins.Jigsaw;

namespace PrLanguages.Interpreters.Statements
{
    public class ForCycleWithVariable : Statement
    {
        protected IEnumerable<Statement> body;
        protected Expression fromExp;
        protected Expression toExp;
        protected Expression step;
        protected string variable;

        public ForCycleWithVariable(string variable, IEnumerable<Statement> body, Expression fromExp, Expression toExp,
            Expression step, Node node, IVariableManager varManager): base(node, varManager)
        {
            this.variable = variable;
            this.step = step;
            this.body = body;
            this.fromExp = fromExp;
            this.toExp = toExp;
        }

        public override dynamic Exectute()
        {
            varManager.SetAllVariables(fromExp);
            sb = new StringBuilder();

            dynamic from = fromExp.Calculate();
            dynamic counter = varManager.GetValue(variable);
            counter = from;
            int i = 1;
            dynamic result = null;
            varManager.SetValue(variable, counter);
            while(counter <= toExp.Calculate())
            {
                sb.AppendLine(string.Format("Итерация {0}. Значение счётчика: {1}", i, counter));
                foreach(var st in body)
                {
                    result = st.Exectute();
                    sb.AppendLine(st.GetDebugInfo());
                }
                varManager.SetAllVariables(step);
                counter += step.Calculate();
                varManager.SetAllVariables(toExp);
                sb.AppendLine(Environment.NewLine);
                varManager.SetValue(variable, counter);
                i++;
            }
            varManager.SetValue(variable, counter);
            return result;
        }
    }
}
