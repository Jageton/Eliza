using Diggins.Jigsaw;

namespace PrLanguages.Grammars
{
    public class PascalGrammar: SharedGrammar
    {
        //start, end and multioperator terminator
        public static Rule OT = WS + MatchChar(';') + WS;
        public static Rule PrStart = MatchString("Begin");
        public static Rule PrEnd = MatchString("End.");

        //expression
        public static Rule Expression = Node(ExpressionsGrammar.Expression);

        //numbers
        new public static Rule Integer = SharedGrammar.Integer;
        public static Rule Double = SharedGrammar.Float;
        public static Rule Number = Integer | Double;

        //variables and indexers
        public static Rule Variable = Node(SharedGrammar.Identifier);
        public static Rule Indexer = Node(Variable + WS + MatchChar('[') + WS + Expression +
            ZeroOrMore(WS + Comma + Expression) + WS + MatchChar(']'));
        public static Rule IndexerOrVariable = Indexer | Variable;

        //types(not complete)
        public static Rule Type = Node(MatchStringSet("integer"));

        //functions
        public static Rule FunctionCall = Node(ExpressionsGrammar.FunctionCall);

        public static Rule ArrayInit =  
            Node(Variable + WS + MatchChar('[') + WS + Integer + WS + MatchChar(':')
            + WS + Integer + WS + MatchChar(']'));

        //recursive operator
        public static Rule RecOperator = Recursive(() => Statement);

        //declaration
        public static Rule ArrayKeyword = MatchString("array[");
        public static Rule ArrayDecl = Node(OneOrMore(Variable) + WS + MatchChar(':') + WS +
            ArrayKeyword + WS + Expression + MatchString("...") + WS + Expression + MatchChar(']') +
            WS + MatchString("of") + WS + Type + WS + MatchChar(';'));
        public static Rule AtomicDecl = Node(MatchString("Var") + WS + Variable + WS +
            ZeroOrMore(Comma + WS + Variable + WS) + MatchChar(':') + WS + Type + WS + MatchChar(';'));
        public static Rule Declaration = Node(ArrayDecl | AtomicDecl);
        public static Rule DeclBlock = OneOrMore(Declaration);

        //assignment
        public static Rule Assignment =
            Node((IndexerOrVariable) + WS + MatchString(":=") + Expression);

        //if operator
        public static Rule IfCondition = Node(Expression);
        public static Rule ThenBranch = Node(MatchString("then") + WS + RecOperator);
        public static Rule ElseBranch = Node(MatchString("else") + WS + RecOperator);
        public static Rule IfOperator = Node(MatchString("if") + WS + IfCondition + WS + ThenBranch +
            WS + Opt(ElseBranch + WS));

        //for cycle
        public static Rule ForStartExp = Node(Expression);
        public static Rule ForEndExp = Node(Expression);
        public static Rule ForCycleDecl = Node(MatchString("for") + WS +
            IndexerOrVariable + WS + MatchString(":=") + WS + ForStartExp + WS +
            MatchString("to") + WS + ForEndExp + WS + MatchString("do"));
        public static Rule ForCycle = Node(ForCycleDecl + WS + ZeroOrMore(RecOperator + WS));

        //operators and blocks
        public static Rule FlowControlOperator = IfOperator | ForCycle | FunctionCall;
        public static Rule SingleOperator = Declaration | Assignment;
        public static Rule OperatorBlock = MatchString("begin") + WS +
            OneOrMore(RecOperator + WS) + MatchString("end;");
        public static Rule Operator = Node((FlowControlOperator | SingleOperator) + WS);
        public static Rule Statement = (Operator + ZeroOrMore(OT + Operator)) | OperatorBlock;

        //program
        public static Rule Program = Node(WS + Opt(DeclBlock) + WS + PrStart + WS + ZeroOrMore(Operator) + WS + PrEnd);

        static PascalGrammar()
        {
            InitGrammar(typeof(PascalGrammar));
        }
    }
}
