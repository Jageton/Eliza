using System.Collections.Generic;
using System.Text;
using PrLanguages.Expressions;
using PrLanguages.Interpreters.VariableManagers;
using Diggins.Jigsaw;


namespace PrLanguages.Interpreters.Statements
{
    public class ForCycleWithIndexer: Statement
    {
        protected IEnumerable<Statement> body;
        protected Expression fromExp;
        protected Expression toExp;
        protected Expression step;
        protected string variable;
        protected List<Expression> indexerParams;

        public ForCycleWithIndexer(string variable, List<Expression> indexerParams, IEnumerable<Statement> body, Expression fromExp, Expression toExp,
            Expression step, Node node, IVariableManager varManager): base(node, varManager)
        {
            this.variable = variable;
            this.step = step;
            this.body = body;
            this.fromExp = fromExp;
            this.toExp = toExp;
            this.indexerParams = indexerParams;
        }

        public override dynamic Exectute()
        {
            varManager.SetAllVariables(fromExp);
            sb = new StringBuilder();

            dynamic from = fromExp.Calculate();
            dynamic counter = varManager.GetValue(variable);
            dynamic counterValue = fromExp.Calculate();
            IndexerCaller.SetIndexerValue(counter, CalculateIndexes(), counterValue);
            varManager.SetValue(variable, counter);
            int i = 1;
            dynamic result = null;

            while(counterValue <= toExp.Calculate())
            {
                sb.AppendLine(string.Format("Итерация {0}. Значение счётчика: {1}\n", i, counterValue));
                foreach(var st in body)
                {
                    result = st.Exectute();
                    sb.AppendLine(st.GetDebugInfo());
                }
                varManager.SetAllVariables(step);
                counterValue += step.Calculate();
                varManager.SetAllVariables(toExp);
                IndexerCaller.SetIndexerValue(counter, CalculateIndexes(), counterValue);
                varManager.SetValue(variable, counter);
                i++;
            }
            IndexerCaller.SetIndexerValue(counter, CalculateIndexes(), counterValue);
            varManager.SetValue(variable, counter);
            return result;
        }

        private dynamic[] CalculateIndexes()
        {
            dynamic[] result = new dynamic[indexerParams.Count];
            for(int i = 0; i < result.Length; i++)
            {
                varManager.SetAllVariables(indexerParams[i]);
                result[i] = indexerParams[i].Calculate();
            }
            return result;
        }
    }
}
