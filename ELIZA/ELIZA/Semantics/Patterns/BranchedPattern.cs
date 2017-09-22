using System;
using System.Collections.Generic;
using System.Linq;
using ELIZA.Syntax;
using ELIZA.Syntax.DeepRelations;

namespace ELIZA.Semantics.Patterns
{
    [Serializable]
    public class BranchedPattern: ComplexPattern
    {
        public BranchedPattern(IEnumerable<Pattern> patterns) : base(patterns)
        {
            
        }

        public override bool Match(List<Tree<DForm, DeepRelationName>> trees)
        {
            var matchedPatterns =  new List<Pattern>();
            var matchedTrees = new List<Tree<DForm, DeepRelationName>>();
            foreach (var pattern in patterns)
            {
                //паттерн ещё не использовался
                if (!matchedPatterns.Contains(pattern))
                {
                    foreach (var tree in trees)
                    {
                        //если дерево ещё не подошло ни под один паттерн
                        if (!matchedTrees.Contains(tree))
                        {
                            //дерево подошло
                            if (pattern.Match(tree))
                            {
                                //отмечаем дерево и паттерн
                                matchedTrees.Add(tree);
                                matchedPatterns.Add(pattern);
                                break;
                            }
                        }
                    }
                }
            }
            //если были использованы все паттерны, то паттерн подошёл
            return matchedPatterns.Count() == patterns.Count;
        }
        public override bool Match(Tree<DForm, DeepRelationName> tree)
        {
            //если этому паттерну передано 1 дерево, то ищем совпадения в его наследниках
            return Match(tree.Children.ToList());
        }

        public override string ToString()
        {
            var result = patterns.First().ToString();
            for (int i = 1; i < patterns.Count; i++)
            {
                result += ", " + patterns[i].ToString();
            }
            return "(" + result + ")";
        }
    }
}
