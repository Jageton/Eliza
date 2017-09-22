using System.Collections.Generic;
using System.Linq;
using PrLanguages.Expressions;
using PrLanguages.Grammars;
using PrLanguages.Interpreters.VariableManagers;
using Diggins.Jigsaw;
using PrLanguages.Interpreters.Statements;

namespace PrLanguages.Interpreters.Builders
{
    public class IfStatementBuilder: IStatementBuilder
    {
        protected Dictionary<string, IStatementBuilder> builders;
        protected IVariableManager vm;
        protected ExpressionHelper eh;


        public IfStatementBuilder(Dictionary<string, IStatementBuilder> builders, IVariableManager vm,
            ExpressionHelper eh)
        {
            this.builders = builders;
            this.eh = eh;
            this.vm = vm;
        }

        #region IStatementBuilder Members

        public Statements.Statement Build(Diggins.Jigsaw.Node node)
        {
            string condition = node[AlgLanguageGrammar.IfCondition.Name].Text;
            Node thenBranch = node[1];
            Node elseBranch = node.Nodes.Count > 2 ? node.Nodes[2] : null;
            IEnumerable<Statement> thenOperators = from op in thenBranch.Nodes
                                              where op.Label == AlgLanguageGrammar.Operator.Name
                                              select builders[op[0].Label].Build(op[0]);
            IEnumerable<Statement> elseOperators = null;
            if(elseBranch != null)
            {
                elseOperators = from op in elseBranch.Nodes
                                    where op.Label == AlgLanguageGrammar.Operator.Name
                                select builders[op[0].Label].Build(op[0]);
            }
            return new IfStatement(eh.CreateExpression(condition),
                thenOperators, elseOperators, node, vm, condition);
        }

        #endregion
    }
}
