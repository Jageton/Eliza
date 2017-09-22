using System.Collections.Generic;
using System.Linq;
using PrLanguages.Expressions;
using PrLanguages.Grammars;
using PrLanguages.Interpreters.VariableManagers;
using Diggins.Jigsaw;
using PrLanguages.Interpreters.Statements;

namespace PrLanguages.Interpreters.Builders
{
    public class ForCycleBuilder: IStatementBuilder
    {
        protected Dictionary<string, IStatementBuilder> builders;
        protected IVariableManager vm;
        protected ExpressionHelper eh;

        public ForCycleBuilder(Dictionary<string, IStatementBuilder> builders, IVariableManager vm,
            ExpressionHelper eh)
        {
            this.builders = builders;
            this.vm = vm;
            this.eh = eh;
        }

        #region IStatementBuilder Members

        public Statement Build(Node node)
        {
            Node decl = node[0];
            Node indexerOrVariable = decl[0];
            Expression startExp = eh.CreateExpression(decl[1].Text);
            Expression endExp = eh.CreateExpression(decl[2].Text);
            IEnumerable<Statement> body = from op in node.Nodes
                                          where op.Label == AlgLanguageGrammar.Operator.Name
                                          select builders[op[0].Label].Build(op[0]);
            string variable = string.Empty;
            if (indexerOrVariable.Label == "Variable")
            {
                variable = indexerOrVariable.Text;
                return new ForCycleWithVariable(variable, body, startExp, endExp,
                    eh.CreateExpression("1"), node, vm);
            }
            else
            {
                variable = indexerOrVariable[0].Text;
                List<Expression> args = new List<Expression>();
                for (int i = 1; i < indexerOrVariable.Nodes.Count; i++)
                    args.Add(eh.CreateExpression(indexerOrVariable[i].Text));
                return new ForCycleWithIndexer(variable, args, body, startExp, endExp,
                    eh.CreateExpression("1"), node, vm);
            }
        }

        #endregion
    }
}
