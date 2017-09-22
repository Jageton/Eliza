using ELIZA.Morphology;

namespace ELIZA.Syntax.SurfaceRelations
{
    /// <summary>
    /// Первое вспомогательное ПСО.
    /// </summary>
    /// <seealso cref="AbstractSSR" />
    public class Accessory1SSR: AbstractSSR
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
            if (SamePos(f.Tag, Tag.Numeric) && SamePos(s.Tag, Tag.Numeric))
            {
                if (s.LexemPosition - f.LexemPosition == 1)
                {
                    first.AddChild(second, SurfaceRelationName.Accessory1);
                    return true;
                }
            }
            else if (f.LexemPosition - s.LexemPosition == 1)
            {
                if (SamePos(f.Tag, Tag.Adjective) && (f.Tag & Tag.Apro) != 0)
                {
                    if (SamePos(s.Tag, Tag.Particle) || SamePos(s.Tag, Tag.Conjunction))
                    {
                        first.AddChild(second, SurfaceRelationName.Accessory1);
                        return true;
                    }
                }
                else if (SamePos(f.Tag, Tag.Conjunction))
                {
                    if (SamePos(s.Tag, Tag.Particle) || SamePos(s.Tag, Tag.Conjunction) ||
                        (SamePos(s.Tag, Tag.Particle) || SamePos(s.Tag, Tag.Conjunction)))
                    {
                        first.AddChild(second, SurfaceRelationName.Accessory1);
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
