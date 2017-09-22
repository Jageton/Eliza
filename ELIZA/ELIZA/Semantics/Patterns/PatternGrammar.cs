using System.Text.RegularExpressions;
using Diggins.Jigsaw;

namespace ELIZA.Semantics.Patterns
{
    internal class PatternGrammar: SharedGrammar
    {
        public static Rule Name = Node(MatchRegex(new Regex("([a-zA-zа-яА-Я0-9]|ё|Ё)+")));
        public static Rule MatchDep = Node(MatchStringSet("SENT COOR REL A1 A2 A3 AGGR PROP ATTR MAGN"));
        public static Rule MatchLemma = Node(Name);
        public static Rule MatchLemmaPattern = Node(Opt(Parenthesize(Name)) + MatchLemma);
        public static Rule MatchDepPattern = Node(Opt(Parenthesize(Name)) + MatchDep);

        public static Rule MatchDepAndLemmaPattern =
            Node(Opt(Parenthesize(Name)) + MatchDep + MatchChar('^') + MatchLemma);
        public static Rule RecPattern = Recursive(() => StringPattern);
        //оператор, определяющий структуру ветвления дерева (порядок не важен)
        public static Rule BranchedPattern =
            Node(Parenthesize(RecPattern + OneOrMore(WS + MatchChar(',') + WS + RecPattern)));
        //соответствует одному из паттернов
        public static Rule OrPattern = Node(Parenthesize(RecPattern + (OneOrMore(WS + MatchChar('|') + WS + RecPattern))));
        //оператор, определящую структуру в глубину
        //два паттерна следуют друг за другом
        //ветвление не может быть началом такого паттерна,
        //т.к. дерево не объединяется после ветвления
        public static Rule ChainedPattern =
            Node((OrPattern | MatchDepAndLemmaPattern | MatchDepPattern | MatchLemmaPattern) +
            WS + MatchChar('-') + WS + RecPattern);

        //этот паттерн пропускает всё, пока внутренний паттерн не найдёт совпадаение
        //если существует несколько ветвей, проверяются все
        public static Rule SkipablePattern = Node(MatchString("...") + WS + RecPattern);

        public static Rule StringPattern = SkipablePattern | ChainedPattern | MatchDepAndLemmaPattern |
                                           MatchDepPattern | MatchLemmaPattern | BranchedPattern | OrPattern;

        static PatternGrammar()
        {
            Grammar.InitGrammar(typeof(PatternGrammar));
        }
    }
}
