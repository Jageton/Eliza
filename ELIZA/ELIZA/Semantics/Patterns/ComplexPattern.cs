using System;
using System.Collections.Generic;
using ELIZA.Syntax;
using ELIZA.Syntax.DeepRelations;

namespace ELIZA.Semantics.Patterns
{
    [Serializable]
    public abstract class ComplexPattern: Pattern, IComplexPattern
    {
        protected List<Pattern> patterns;

        //сложные паттерны не имеют имени
        //и всегда сохраняют значения, точнее передают эти обязанности внутренним паттернам
        protected ComplexPattern() : base("", true)
        {
            patterns = new List<Pattern>();
        }
        protected ComplexPattern(IEnumerable<Pattern> patterns) : this()
        {
            foreach(var pattern in patterns)
                this.patterns.Add(pattern);
        }

        public abstract bool Match(List<Tree<DForm, DeepRelationName>> trees);
        public override bool Match(Tree<DForm, DeepRelationName> tree)
        {
            //по умолчанию просто применяем первый паттерн к заданному дереву
            return patterns[0].Match(tree);
        }
        public override bool TryGetSavedValue(string name, out string value)
        {
            value = null;
            //просто опрашиваем другие паттерны
            //имена собираемых переменных не должны совпадать, иначе будет выбрана первая 
            //из встреченных
            foreach (var pattern in patterns)
            {
                if (pattern.TryGetSavedValue(name, out value))
                    return true;
            }
            return false;
        }
    }
}
