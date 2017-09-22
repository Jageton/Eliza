using System.Text;
using Diggins.Jigsaw;
using PrLanguages.Interpreters.VariableManagers;

namespace PrLanguages.Interpreters.Statements
{
    public abstract class Statement
    {
        protected Node node;
        protected IVariableManager varManager;
        protected StringBuilder sb;

        public abstract dynamic Exectute();
        public string GetDebugInfo()
        {
            return sb.ToString();
        }

        public Statement(Node node, IVariableManager varManager)
        {
            this.node = node;
            this.varManager = varManager;
        }

        public override string ToString()
        {
            return node.Text;
        }
    }
}
