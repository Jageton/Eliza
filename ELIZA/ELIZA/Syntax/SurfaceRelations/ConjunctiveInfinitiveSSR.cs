using System.Linq;
using ELIZA.Morphology;

namespace ELIZA.Syntax.SurfaceRelations
{
    /// <summary>
    /// Союзно-инфинитивное ПСО.
    /// </summary>
    /// <seealso cref="AbstractSSR" />
    public class ConjunctiveInfinitiveSSR: AbstractSSR
    {

        protected static string[] usedConj = { "если", "чтобы", "чем" };
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
            //если союз из списка перечисленных
            if(SamePos(f.Tag, Tag.Conjunction) && usedConj.Contains(f.Word))
            {
                if(SamePos(s.Tag, Tag.Infinitive))
                {
                    first.AddChild(second, SurfaceRelationName.ConjunctiveInfinitive);
                    return true;
                }
            }
            return false;
        }
    }
}
