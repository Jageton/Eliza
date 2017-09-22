using System.Linq;
using ELIZA.Morphology;

namespace ELIZA.Syntax.SurfaceRelations
{
    /// <summary>
    /// Служебное ПСО.
    /// </summary>
    /// <seealso cref="AbstractSSR" />
    public class ServiceSSR: AbstractSSR
    {
        protected static string[] modifiers = {"более", "менее", "самый"};

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
            if (SamePos(f.Tag, Tag.Adjective | Tag.ShortAdjective))
            {
                if (modifiers.Contains(s.Word))
                {
                    first.AddChild(second, SurfaceRelationName.Service);
                    return true;
                }
            }
            else if (SamePos(f.Tag, Tag.Verb | Tag.Infinitive))
            {
                if (f.LexemPosition > s.LexemPosition)
                {
                    if (SamePos(s.Tag, Tag.Particle | Tag.Predicate))
                    {
                        first.AddChild(second, SurfaceRelationName.Service);
                        return true;
                    }
                }
                else if (SamePos(s.Tag, Tag.Verb | Tag.Infinitive) && s.LexemPosition - f.LexemPosition == 1)
                {
                    first.AddChild(second, SurfaceRelationName.Service);
                    return true;
                }
                return false;
            }
            return false;
        }
    }
}
