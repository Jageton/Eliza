using Diggins.Jigsaw;
using System.Text.RegularExpressions;

namespace PrLanguages.Grammars
{
    public class ExpressionsGrammar: SharedGrammar
    {
        public static Rule RecExpr = Recursive(() => Expression);

        new public static Rule Integer = Node(SharedGrammar.Integer);
        public static Rule Double = Node(SharedGrammar.Float);
        public static Rule LeftParan = Node(CharToken('('));
        public static Rule RightParan = Node(CharToken(')'));
        public static Rule Variable = Node(Pattern("[a-zA-z]+[a-zA-z0-9]*"));
        public static Rule LeftSqParan = Node(MatchChar('['));
        public static Rule RightSqParan = Node(MatchChar(']'));
        new public static Rule Comma = Node(SharedGrammar.Comma);
        public static Rule IndexerDelimiter = Node(SharedGrammar.Comma);
        public static Rule IndexerName = Node(SharedGrammar.Identifier);
        public static Rule Indexer = (IndexerName + LeftSqParan + WS + RecExpr +
            ZeroOrMore(WS + IndexerDelimiter + WS + RecExpr) + WS + RightSqParan);
        public static Rule Boolean = Node(MatchStringSet("true false True False TRUE FALSE"));
        public static Rule String =
            Node(MatchChar('\'') + MatchRegex(new Regex("[^']*")) + MatchChar('\''));
        
        public static Rule AtomicValue =
            ((Integer | Double | Boolean | String | Indexer | Variable));
        public static Rule FunctionName = Node(SharedGrammar.Identifier);
       
        public static Rule PrefixOp = Node(MatchStringSet("++ -- ! - ~"));
        public static Rule PostfixOp = Node(MatchStringSet("++ --"));
        public static Rule BinaryOp =
          Node(MatchStringSet("-> <= >= == != << >> && || < > & | + - * % / ^ ="));

        public static Rule ParanExpr = (LeftParan + WS + RecExpr + WS + RightParan);
        public static Rule PrefixExp = (PrefixOp + Recursive(() => SimpleExpr));
        public static Rule FunctionCall = FunctionName + LeftParan +
                               (WS + RecExpr + WS + ZeroOrMore(Comma + WS + RecExpr) + RightParan | WS + RightParan);
        public static Rule SimpleExpr = ((PrefixExp | FunctionCall | ParanExpr | AtomicValue) + ZeroOrMore(PostfixOp));
        public static Rule Expression = Node(SimpleExpr + ZeroOrMore(WS + BinaryOp + WS + SimpleExpr));

        static ExpressionsGrammar()
        {
            InitGrammar(typeof(ExpressionsGrammar));
        }
    }
}
