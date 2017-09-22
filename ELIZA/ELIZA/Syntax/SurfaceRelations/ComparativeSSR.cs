using ELIZA.Morphology;

namespace ELIZA.Syntax.SurfaceRelations
{
    /// <summary>
    /// Сравнительное ПСО. Все сравнительное ПСО (сравнительно-субъективное, сравнительно-копмлетивные, 
    /// сравнительно-обстоятельственное) были объеденены в одно. Возможно, придётся разделить.
    /// </summary>
    /// <seealso cref="AbstractSSR" />
    public class ComparativeSSR: AbstractSSR
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
            string word = f.Word.ToLower();
            string word2 = s.Word.ToLower();
            //если главное слово - сравнительная cтепень прилагательного
            if((f.Tag & Tag.Comparative) != 0 || (word == "более" || word == "менее"))
            {
                if((s.Tag & (Tag.Noun | Tag.NounLike)) != 0) //существительное
                {
                    if((s.Tag & (Tag.Genitive | Tag.Genitive1 | Tag.Genitive2)) != 0) //в родительном падеже
                    {
                        first.AddChild(second, SurfaceRelationName.Comparative);
                        return true;
                    }
                }
                else if(word2 == "чем" || word2 == "нежели")
                {
                    first.AddChild(second, SurfaceRelationName.Comparative);
                    return true;
                }
            }
            return false;
        }
    }
}
