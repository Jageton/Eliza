using ELIZA.Morphology;

namespace ELIZA.Syntax.SurfaceRelations
{
    /// <summary>
    /// Количественное ПСО.
    /// </summary>
    /// <seealso cref="AbstractSSR" />
    public class QuantitiveSSR: AbstractSSR
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
            if (f.Lemma == "число" && (s.Tag & (Tag.Numeric | Tag.Number)) != 0)
            {
                first.AddChild(second, SurfaceRelationName.Quantitive);
                return true;
            }
            if ((f.Tag & Tag.Noun) != 0)
            {
                //если зависимое слово - это словарное числительное
                if ((s.Tag & Tag.Numeric) != 0)
                {
                    //согласовано с числительным
                    if (((f.Tag & Tag.Genitive) != 0 && (s.Tag & Tag.Nominative) != 0) ||
                        (SameCase(s.Tag, f.Tag)))
                    {
                        first.AddChild(second, SurfaceRelationName.Quantitive);
                        return true;
                    }
                }
                else if ((s.Tag & Tag.Number) != 0 && s.LexemPosition < f.LexemPosition)
                {
                    //число левее существительного
                    if ((f.Tag & (Tag.Genitive | Tag.Nominative)) != 0)
                    {
                        first.AddChild(second, SurfaceRelationName.Quantitive);
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
