using System.Collections.Generic;
using System.Text;
using PrLanguages.Interpreters.VariableManagers;
using Diggins.Jigsaw;

namespace PrLanguages.Interpreters.Statements
{
    public class Declaration: Statement
    {
        protected List<string> names;
        protected List<dynamic> values;

        public Declaration(List<string> names, List<dynamic> values, Node node,
            IVariableManager varManager): base(node, varManager)
        {
            this.names = names;
            this.values = values;
        }

        public override dynamic Exectute()
        {
            //no debug info for declaration
            sb = new StringBuilder();
            for(int i = 0; i < names.Count; i++)
            {
                varManager.Declare(names[i], values[i]);
            }
            return 0;
        }
    }
}
