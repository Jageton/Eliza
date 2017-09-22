using ELIZA.Morphology;

namespace ELIZA.Syntax.SurfaceRelations
{
    /// <summary>
    /// Аппроксимативное ПСО.
    /// </summary>
    /// <seealso cref="AbstractSSR" />
    public class ApproximativeSSR: AbstractSSR
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
            //в данном ПСО зависимая форма всегда позже главной
            if (s.LexemPosition - f.LexemPosition == 1)
            {
                //главное слово - существительное в родительмно падеже во множественном числе
                if (SamePos(Tag.Noun, f.Tag) &&
                    SameCase(f.Tag, Tag.Genitive | Tag.Genitive1 | Tag.Genitive2) && 
                    SameNumber(Tag.Plural,f.Tag))
                {
                    //зависимое слово - числительное в цифровой форме
                    //или другое числительное в именительном падеже
                    if(SamePos(Tag.Number, s.Tag) ||
                        (SamePos(Tag.Numeric, s.Tag) && SameCase(Tag.Nominative, s.Tag)))
                    {
                        first.AddChild(second, SurfaceRelationName.Approximative);
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
