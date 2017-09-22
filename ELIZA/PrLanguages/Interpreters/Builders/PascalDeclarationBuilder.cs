using System;
using System.Collections.Generic;
using PrLanguages.Grammars;
using PrLanguages.Interpreters.VariableManagers;
using Diggins.Jigsaw;
using PrLanguages.Interpreters.Statements;

namespace PrLanguages.Interpreters.Builders
{
    public class PascalDeclarationBuilder: IStatementBuilder
    {
        protected IVariableManager vm;
        protected Dictionary<string, Func<bool, dynamic[], dynamic>> builders;

        public PascalDeclarationBuilder(IVariableManager vm)
        {
            this.vm = vm;
            builders = new Dictionary<string, Func<bool, dynamic[], dynamic>>();

        }

        #region IStatementBuilder Members

        public Statement Build(Node node)
        {
            string type = node[PascalGrammar.Type.Name].Text;
            return null;
        }

        #endregion
    }
}
