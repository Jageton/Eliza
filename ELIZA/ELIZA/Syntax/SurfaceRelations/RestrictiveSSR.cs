using ELIZA.Morphology;

namespace ELIZA.Syntax.SurfaceRelations
{
    /// <summary>
    /// Ограничительное ПСО.
    /// </summary>
    /// <seealso cref="AbstractSSR" />
    public class RestrictiveSSR: AbstractSSR
    {
        /// <summary>
        /// Пытается построить поверхностсон-синтаксическое отношение между двумя синтаксическими
        /// деревьями.
        /// </summary>
        /// <param name="first">Первое дерево.</param>
        /// <param name="second">Второе дерево.</param>
        /// <param name="head"></param>
        /// <returns>
        /// Возвращает <c>true</c>, если удалось построить отношение,
        /// иначе возвращает <c>false</c>.
        /// </returns>
        protected override bool TryBuildRelation(Tree<Lexem, SurfaceRelationName> first,
            Tree<Lexem, SurfaceRelationName> second,
            out Tree<Lexem, SurfaceRelationName> head)
        {
            //TODO: дополнительное тестирование
            Lexem f = first.Key;
            Lexem s = second.Key;
            head = first;
            //управляющее слово - всегда частица
            if((f.Tag & Tag.Particle) != 0)
            {
                if((f.Tag & (Tag.Noun | Tag.NounLike | Tag.Adjective | Tag.ShortAdjective | Tag.Adverb |
                    Tag.Verb | Tag.Infinitive | Tag.Gerund | Tag.Participle | Tag.ShortParticiple)) != 0)
                {
                    first.AddChild(second, SurfaceRelationName.Restictive);
                    return true;
                }
            }
            return false;
        }
    }
}
