using System;
using System.Collections.Generic;
using System.Linq;
using PrLanguages.Interpreters.Statements;
using Diggins.Jigsaw;
using PrLanguages.Expressions;
using PrLanguages.Grammars;
using PrLanguages.Interpreters.VariableManagers;

namespace PrLanguages.Interpreters.Builders
{
    public class AlgLanguageDeclarationBuilder: IStatementBuilder
    {
        protected Dictionary<string, Func<bool, dynamic[], dynamic>> creators;
        protected IVariableManager varManager;
        protected ExpressionHelper eh;

        public AlgLanguageDeclarationBuilder(IVariableManager varManager, ExpressionHelper eh)
        {
            this.varManager = varManager;
            this.eh = eh;
            creators = new Dictionary<string, Func<bool, dynamic[], dynamic>>();
            creators.Add("цел", (a, b) => { if (a) return new AlgArray<int>(b[0], b[1]); return 0; });
        }

        #region IStatementBuilder Members

        public Statements.Statement Build(Diggins.Jigsaw.Node node)
        {
            List<string> variables = new List<string>();
            List<dynamic> expressions = new List<dynamic>();
            Node typeNode  = node.Descendants.FirstOrDefault((a) =>
                { return a.Label == AlgLanguageGrammar.Type.Name; });
            string type = typeNode.Nodes[0].Text;
            bool arrayMod = typeNode.Nodes.Count > 1;
            foreach(var n in node.Nodes)
            {
                if(n.Label == AlgLanguageGrammar.Variable.Name)
                {
                    variables.Add(n.Text);
                    expressions.Add(creators[type](arrayMod, null));
                }
                if(n.Label == AlgLanguageGrammar.ArrayInit.Name)
                {
                    variables.Add(n.Nodes[0].Text);
                    dynamic[] args = new dynamic[n.Nodes.Count - 1];
                    for (int i = 1; i < n.Nodes.Count; i++)
                    {
                        Expression exp = eh.CreateExpression(n.Nodes[i].Text);
                        varManager.SetAllVariables(exp);
                        args[i - 1] = exp.Calculate();
                    }
                    expressions.Add(creators[type](arrayMod, args));
                }
            }
            return new Declaration(variables, expressions, node, varManager);
        }

        #endregion

    }
}
