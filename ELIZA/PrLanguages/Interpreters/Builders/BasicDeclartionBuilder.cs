using System;
using System.Collections.Generic;
using System.Linq;
using PrLanguages.Expressions;
using PrLanguages.Grammars;
using PrLanguages.Interpreters.VariableManagers;
using Diggins.Jigsaw;
using PrLanguages.Interpreters.Statements;

namespace PrLanguages.Interpreters.Builders
{
    public class BasicDeclartionBuilder: IStatementBuilder
    {
        protected IVariableManager vm;
        protected Dictionary<string, Func<bool, dynamic[], dynamic>> builders;
        protected ExpressionHelper eh;

        public BasicDeclartionBuilder(IVariableManager vm, ExpressionHelper eh)
        {
            this.vm = vm;
            this.eh = eh;
            builders = new Dictionary<string, Func<bool, dynamic[], dynamic>>();
            builders.Add("INTEGER", (a, b) => { if (a) return new AlgArray<int>(b[0]); return 0; });
        }

        #region IStatementBuilder Members

        public Statement Build(Node node)
        {
            string type = node[BasicGrammar.Type.Name].Text;
            IEnumerable<Node> variables = from n in node.Nodes
                                          where n.Label == BasicGrammar.Variable.Name
                                          select n;
            IEnumerable<Node> indexers = from n in node.Nodes
                                         where n.Label == BasicGrammar.Indexer.Name
                                         select n;
            List<string> names = new List<string>();
            List<dynamic> values = new List<dynamic>();
            foreach(var n in variables)
            {
                names.Add(n.Text);
                values.Add(builders[type](false, null));
            }
            foreach(var n in indexers)
            {
                names.Add(n[0].Text);
                dynamic[] args = new dynamic[n.Nodes.Count - 1];
                for (int i = 1; i < n.Nodes.Count; i++)
                {
                    Expression exp = eh.CreateExpression(n.Nodes[i].Text);
                    vm.SetAllVariables(exp);
                    args[i - 1] = exp.Calculate();
                }
                values.Add(builders[type](true, args));
            }
            return new Declaration(names, values, node, vm);
        }

        #endregion
    }
}
