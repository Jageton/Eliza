using System.Collections.Generic;
using System.Linq;
using PrLanguages.Interpreters.Builders;
using PrLanguages.Grammars;
using PrLanguages.Interpreters.Statements;
using Diggins.Jigsaw;

namespace PrLanguages.Interpreters
{
    public class AlgLanguageInterpreter: Interpreter
    {

        public AlgLanguageInterpreter():base()
        {
            builders.Add(AlgLanguageGrammar.Declaration.Name,
                new AlgLanguageDeclarationBuilder(varManager, eh));
            builders.Add(AlgLanguageGrammar.Assignment.Name,
                new AssignmentBuilder(eh, varManager));
            builders.Add(AlgLanguageGrammar.IfOperator.Name,
                new IfStatementBuilder(builders, varManager, eh));
            builders.Add(AlgLanguageGrammar.ForCycle.Name,
                new ForCycleBuilder(builders, varManager, eh));
        }

        protected override IEnumerable<Statement> Parse(string program)
        {
            return Parse(AlgLanguageGrammar.Program.Parse(program)[0]);
        }

        protected override IEnumerable<Statement> Parse(Node node)
        {
            IEnumerable<Node> operators = from n in node.Nodes
                                          where n.Label == AlgLanguageGrammar.Operator.Name
                                          select n.Nodes[0];
            foreach(var n in operators)
            {
                yield return builders[n.Label].Build(n);
            }
        }
    }
}
