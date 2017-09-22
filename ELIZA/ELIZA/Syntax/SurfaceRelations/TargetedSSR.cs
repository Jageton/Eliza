using System.Linq;
using ELIZA.Morphology;

namespace ELIZA.Syntax.SurfaceRelations
{
    /// <summary>
    /// Адресатное ПСО.
    /// </summary>
    /// <seealso cref="AbstractSSR" />
    public class TargetedSSR: AbstractSSR
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
            if (SamePos(f.Tag, Tag.Verb | Tag.Infinitive))
            {
                Lexem t = s;
                if (SamePos(s.Tag, Tag.Preposition))
                {
                    var p = second.Dependencies.FirstOrDefault((a) =>
                        a.Key == SurfaceRelationName.Prepositional);
                    if (p.Value != null)
                        t = p.Value.Key;
                }
                if (SamePos(t.Tag, Tag.Noun | Tag.NounLike) &&
                    SameCase(t.Tag, Tag.Dative) && (t.Tag & Tag.Animated) != 0)
                {
                    first.AddChild(second, SurfaceRelationName.Targeted);
                    return true;
                }
            }
            return false;
        }
    }
}
