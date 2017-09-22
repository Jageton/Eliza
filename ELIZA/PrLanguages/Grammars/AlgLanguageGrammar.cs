using Diggins.Jigsaw;

namespace PrLanguages.Grammars
{
    public class AlgLanguageGrammar: SharedGrammar
    {
        //delimiters, start and end
        public static Rule OT = WS + MatchChar(';') + WS;
        public static Rule PrStart = MatchString("алг");
        public static Rule PrEnd = MatchString("кон");

        //numbers
        new public static Rule Integer = SharedGrammar.Integer;
        public static Rule Double = SharedGrammar.Float;
        public static Rule Number = Node(Integer | Double);

        //expression
        public static Rule Expression = Node(ExpressionsGrammar.Expression);

        //variables
        public static Rule Variable = Node(SharedGrammar.Identifier);
        public static Rule Indexer = Node(Variable + WS + MatchChar('[') + WS + Expression +
            ZeroOrMore(WS + Comma + Expression) + WS + MatchChar(']'));
        public static Rule IndexerOrVariable = Indexer | Variable;

        //types (not complete)
        public static Rule AtomicType = Node(MatchStringSet("цел"));
        public static Rule ArraySuffix = Node(MatchString("таб"));
        public static Rule Type = Node(AtomicType + Opt(ArraySuffix));

        //functions (not complete)
        public static Rule ReservedFunctionName = MatchStringSet("print");
        public static Rule FunctionName = Node(ReservedFunctionName);
        public static Rule FunctionCall = Node(FunctionName + WS + ZeroOrMore(Expression));

        //declaration block
        public static Rule ArrStartValue = Node(Integer);
        public static Rule ArrEndValue = Node(Integer);
        public static Rule ArrayInit =  
            Node(Variable + WS + MatchChar('[') + WS + ArrStartValue + WS + MatchChar(':')
            + WS + ArrEndValue + WS + MatchChar(']'));
        public static Rule InitList =
            ((ArrayInit | Variable) + WS + ZeroOrMore(SharedGrammar.Comma + WS + (ArrayInit | Variable) + WS));
        public static Rule Declaration = Node(Type + WS + InitList);

        //assignment
        public static Rule Assignment =
            Node((IndexerOrVariable) + WS + MatchString(":=") + WS + Expression);

        //recursive operator
        public static Rule RecOperator = Recursive(() => Statement);

        //if operator
        public static Rule IfCondition = Node(Expression);
        public static Rule ThenBranch = Node(MatchString("то") + WS + OneOrMore(RecOperator + WS));
        public static Rule ElseBranch = Node(MatchString("иначе") + WS + OneOrMore(RecOperator + WS));
        public static Rule IfOperator = Node(MatchString("если") + WS + IfCondition + WS + ThenBranch +
            WS + Opt(ElseBranch + WS) + MatchString("все"));

        //for cycle
        public static Rule ForStartExp = Node(Expression);
        public static Rule ForEndExp = Node(Expression);
        public static Rule ForCycleDecl = Node(MatchString("нц") + WS + MatchString("для") + WS +
            IndexerOrVariable + WS + MatchString("от") + WS + ForStartExp + WS +
            MatchString("до") + WS + ForEndExp);
        public static Rule ForCycle = Node(ForCycleDecl + WS + ZeroOrMore(RecOperator + WS) + MatchString("кц"));

        //operators
        public static Rule FlowControlOperator = IfOperator | ForCycle | FunctionCall;
        public static Rule SingleOperator = (Declaration | Assignment) + WS;   
        public static Rule Operator = Node((FlowControlOperator | SingleOperator));
        public static Rule Statement = WS + Operator + ZeroOrMore(OT + Operator) + WS;
        
        //program
        public static Rule Program = Node(WS + PrStart + WS + ZeroOrMore(Operator) + WS + PrEnd + WS);

        static AlgLanguageGrammar()
        {
            InitGrammar(typeof(AlgLanguageGrammar));
        }
    }
}
