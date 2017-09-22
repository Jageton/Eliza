using ELIZA.Morphology;

namespace ELIZA.Syntax.SurfaceRelations
{
    /// <summary>
    /// Субъективно-предикативное ПСО.
    /// </summary>
    /// <seealso cref="AbstractSSR" />
    public class ComparavieCopredicativeSSR: AbstractSSR
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
            if((f.Tag & Tag.Verb) != 0) //если главное слово - глагол
            {
                if ((s.Tag & (Tag.Adjective & Tag.Nominative)) != 0) 
                {
                    first.AddChild(second, SurfaceRelationName.Comparative);
                    return true;
                }
                else if((s.Tag & (Tag.Adjective & Tag.Participle)) != 0)
                {
                    if((s.Tag & Tag.Nominative) != 0)
                    {
                        first.AddChild(second, SurfaceRelationName.Comparative);
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
