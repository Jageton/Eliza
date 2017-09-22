using System.Linq;
using ELIZA.Morphology;

namespace ELIZA.Syntax.SurfaceRelations
{
    /// <summary>
    /// Элективное ПСО.
    /// </summary>
    /// <seealso cref="AbstractSSR" />
    public class ElectiveSSR: AbstractSSR
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
            string word = s.Word.ToLower();
            if((f.Tag & (Tag.Adjective)) != 0 && (f.Lemma == "каждый" || f.Lemma == "любой"))
            {
                if((word == "из" || word == "среди") && 
                    second.Dependencies.Any(d => d.Key == SurfaceRelationName.Prepositional))
                {
                    first.AddChild(second, SurfaceRelationName.Elective);
                    return true;
                }
            }
            return false;
        }
    }
}
