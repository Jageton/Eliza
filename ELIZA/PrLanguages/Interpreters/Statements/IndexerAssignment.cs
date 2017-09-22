using System.Collections.Generic;
using System.Text;
using Diggins.Jigsaw;
using PrLanguages.Interpreters.VariableManagers;
using PrLanguages.Expressions;

namespace PrLanguages.Interpreters.Statements
{
    public class IndexerAssignment: Statement
    {
        protected string varName;
        protected Expression expression;
        protected List<Expression> indexerParams;

        public IndexerAssignment(string varName, Expression expression,
            List<Expression> indexerParams, Node node, IVariableManager varManager):base(node, varManager)
        {
            this.varName = varName;
            this.expression = expression;
            this.indexerParams = indexerParams;
        }

        public override dynamic Exectute()
        {
            sb = new StringBuilder();
            varManager.SetAllVariables(expression);
            dynamic value = expression.Calculate();
            dynamic[] args = CalculateIndexes();
            sb.Append(string.Format("После оператора {0} значение {1} = {2}.",
                ToString(), IndexerToString(varName, args), value));
            dynamic target = varManager.GetValue(varName);
            IndexerCaller.SetIndexerValue(target, args, value);
            varManager.SetValue(varName, target);
            return value;
        }

        protected dynamic[] CalculateIndexes()
        {
            dynamic[] result = new dynamic[indexerParams.Count];
            for(int i = 0; i < indexerParams.Count; i++)
            {
                varManager.SetAllVariables(indexerParams[i]);
                result[i] = indexerParams[i].Calculate();
            }
            return result;
        }

        protected string IndexerToString(string name, dynamic[] args)
        {
            string result = name;
            result += "[";
            foreach(dynamic arg in args)
            {
                result += arg.ToString() + ", ";
            }
            return result.Substring(0, result.Length - 2) + "]";
        }
    }
}
