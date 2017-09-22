using System;
using System.Linq;
using ELIZA.Syntax;
using ELIZA.Syntax.DeepRelations;

namespace ELIZA.Semantics.Patterns
{
    [Serializable]
    public class ChainedPattern: Pattern
    {
        protected Pattern first;
        protected Pattern second;

        public ChainedPattern(Pattern first, Pattern second) : base("", true)
        {
            this.first = first;
            this.second = second;
        }

        public override bool Match(Tree<DForm, DeepRelationName> tree)
        {
            if (first.Match(tree)) //если первый паттерн удачно совпал
            {
                var complex = second as ComplexPattern;
                if (complex != null)
                {
                    //если паттерн сложный, то передаём ему всех наследников
                    return complex.Match(first.LastMatchedNode.Children.ToList());
                }
                else
                {
                    //если паттерн простой, то пытаемся пойти в глубину по одной из ветвей
                    if (first.LastMatchedNode != null)
                    {
                        foreach (var child in first.LastMatchedNode.Children)
                        {
                            if (second.Match(child))
                                return true;
                        }
                    }
                }
            }
            return false;
        }

        public override bool TryGetSavedValue(string name, out string value)
        {
            //проверяем сохранённые значения двух паттернов
            return first.TryGetSavedValue(name, out value) ||
                second.TryGetSavedValue(name, out value);
        }

        public override string ToString()
        {
            return first.ToString() + " - " + second.ToString();
        }
    }
}
