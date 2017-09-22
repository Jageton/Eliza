using System.Collections.Generic;
using PrLanguages.Interpreters.Statements;
using Diggins.Jigsaw;
using PrLanguages.Expressions;
using PrLanguages.Grammars;
using PrLanguages.Interpreters.VariableManagers;

namespace PrLanguages.Interpreters.Builders
{
    public class AssignmentBuilder: IStatementBuilder
    {
        protected ExpressionHelper eh;
        protected IVariableManager vm;

        public AssignmentBuilder(ExpressionHelper eh, IVariableManager vm)
        {
            this.eh = eh;
            this.vm = vm;
        }

        #region IStatementBuilder Members

        public Statements.Statement Build(Diggins.Jigsaw.Node node)
        {
            Node indexerOrVariable = node[0];
            string variable = string.Empty;
            string expression = node[AlgLanguageGrammar.Expression.Name].Text;
            if(indexerOrVariable.Label == "Variable")
            {
                variable = indexerOrVariable.Text;
                return new VariableAssignment(variable, eh.CreateExpression(expression), node, vm);
            }
            else
            {
                variable = indexerOrVariable[0].Text;
                List<Expression> args = new List<Expression>();
                for (int i = 1; i < indexerOrVariable.Nodes.Count; i++)
                    args.Add(eh.CreateExpression(indexerOrVariable[i].Text));
                return new IndexerAssignment(variable, eh.CreateExpression(expression), args,
                    node, vm);
            }

        }

        #endregion
    }
}
