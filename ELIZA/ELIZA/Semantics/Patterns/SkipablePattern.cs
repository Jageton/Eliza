using System;
using System.Collections.Generic;
using System.Linq;
using ELIZA.Syntax;
using ELIZA.Syntax.DeepRelations;

namespace ELIZA.Semantics.Patterns
{
    [Serializable]
    public class SkipablePattern: Pattern
    {
        protected Pattern inner;

        public SkipablePattern(Pattern inner)
            : base("", true)
        {
            this.inner = inner;
        }

        public override bool Match(Tree<DForm, DeepRelationName> tree)
        {
            var compelex = inner as ComplexPattern;
            if (compelex == null)
            {
                //простой паттерн
                var stack = new Stack<Tree<DForm, DeepRelationName>>();
                stack.Push(tree);
                while (stack.Count > 0)
                {
                    //текущая рассматриваемая вершина
                    var current = stack.Pop();
                    //если паттерн подошёл, то возвращаем
                    if (inner.Match(current))
                        return true;
                    //иначе проверим все остальные ветви
                    foreach (var child in current.Children)
                        stack.Push(child); 
                }
            }
            else
            {
                //сложный паттерн
                //сделаем ветвление в корне и проверим
                var list = new List<Tree<DForm, DeepRelationName>>();
                list.Add(tree);
                if (compelex.Match(list))
                    return true;
                var stack = new Stack<Tree<DForm, DeepRelationName>>();
                stack.Push(tree);
                while (stack.Count > 0)
                {
                    //делаем тоже самое, только проверяем всех потомков, а не сам корень
                    var current = stack.Pop();
                    if (compelex.Match(current.Children.ToList()))
                        return true;
                    foreach (var child in current.Children)
                        stack.Push(child);
                }
            }
            return false;
        }
        public override bool TryGetSavedValue(string name, out string value)
        {
            return inner.TryGetSavedValue(name, out value);
        }

        public override string ToString()
        {
            return "..." + inner.ToString();
        }
    }
}
