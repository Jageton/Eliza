using ELIZA.Morphology;

namespace ELIZA.Syntax.SurfaceRelations
{
    /// <summary>
    /// Инфинитивно-модальное ПСО.
    /// </summary>
    /// <seealso cref="AbstractSSR" />
    public class InfinitiveModalSSR: AbstractSSR
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
            Lexem f = first.Key;
            Lexem s = second.Key;
            head = first;
            if (SamePos(f.Tag, Tag.Infinitive))
            {
                if (SamePos(s.Tag, Tag.Noun | Tag.NounLike) && SameCase(s.Tag, Tag.Dative))
                {
                    first.AddChild(second, SurfaceRelationName.InfinitiveModal);
                    return true;
                }
            }
            return false;
        }
    }
}
