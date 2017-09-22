using Diggins.Jigsaw;

namespace PrLanguages.Grammars
{
    public class BasicGrammar: SharedGrammar
    {
        //singl line operator terminator
        public static Rule OT = WS + MatchChar(':') + WS;

        //expression(indexers should be handeled like new functions)
        public static Rule Expression = Node(ExpressionsGrammar.Expression);

        //numbers
        new public static Rule Integer = SharedGrammar.Integer;
        public static Rule Double = SharedGrammar.Float;
        public static Rule Number = Node(Integer | Double);

        //variables and indexers
        public static Rule Variable = Node(SharedGrammar.Identifier);
        public static Rule Indexer = Node(Variable + WS + MatchChar('(') + WS + Expression +
            ZeroOrMore(WS + Comma + Expression) + WS + MatchChar(')'));
        public static Rule IndexerOrVariable = Indexer | Variable;

        //types (not complete)
        public static Rule Type = Node(MatchStringSet("INTEGER"));

        //functions
        public static Rule ReservedFunctionName = MatchStringSet("PRINT");
        public static Rule FunctionName = Node(ReservedFunctionName);
        public static Rule FunctionCall = Node(FunctionName + WS +
            Expression + WS + ZeroOrMore(Comma + WS + Expression + WS));

        //array initialization
        public static Rule ArrLenght = Node(Integer);
        public static Rule ArrayInit =
            Node(Variable + WS + MatchChar('(') + WS + ArrLenght + WS +
            ZeroOrMore(Comma + WS + ArrLenght + WS) + MatchChar(')'));

        //recursive operator
        public static Rule RecOperator = Recursive(() => Statement);

        //declaration
        public static Rule Delcaration = Node(MatchString("DIM") + WS + (OneOrMore(ArrayInit) | OneOrMore(Variable)) +
            WS + MatchString("AS") + WS + Type);

        //assignment
        public static Rule Assignment =
            Node((IndexerOrVariable) + WS + Eq + Expression);

        //if operator
        public static Rule IfCondition = Node(Expression);
        public static Rule ThenBranch = Node(MatchString("THEN") + WS + OneOrMore(RecOperator + WS));
        public static Rule ElseBranch = Node(MatchString("ELSE") + WS + OneOrMore(RecOperator + WS));
        public static Rule IfOperator = Node(MatchString("IF") + WS + IfCondition + WS + ThenBranch +
            WS + Opt(ElseBranch + WS) + MatchString("END") + WS + MatchString("IF"));

        //for cycle
        public static Rule ForStartExp = Node(Expression);
        public static Rule ForEndExp = Node(Expression);
        public static Rule ForCycleDecl = Node(MatchString("FOR") + WS + IndexerOrVariable +
            WS + Eq + WS + ForStartExp + WS + MatchString("TO") + WS + ForEndExp);
        public static Rule ForCycle = Node(ForCycleDecl + WS + ZeroOrMore(RecOperator + WS) + MatchString("NEXT") + WS + IndexerOrVariable);

        //operators
        public static Rule FlowControlOperator = IfOperator | ForCycle | FunctionCall;
        public static Rule SingleOperator = Delcaration | Assignment;
        public static Rule Operator = Node((FlowControlOperator | SingleOperator) + WS);
        public static Rule Statement = Operator + ZeroOrMore(OT + Operator);


        public static Rule Program = Node(ZeroOrMore(Operator));

        static BasicGrammar()
        {
            InitGrammar(typeof(BasicGrammar));
        }
    }
}
