using ELIZA.Morphology;

namespace ELIZA.Syntax.SurfaceRelations
{
    /// <summary>
    /// Присвязочно-генитивное ПСО.
    /// </summary>
    /// <seealso cref="AbstractSSR" />
    public class CopularGenitiveSSR: AbstractSSR
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
            //в большенстве случаев, такое ПСО проходит через одно слово
            if(s.LexemPosition - f.LexemPosition == 2)
            {
                //главное слово - глагольная форма
                if((f.Tag & (Tag.Verb | Tag.Infinitive)) != 0)
                {
                    //непереходная форма
                    if((f.Tag & (Tag.Transitive)) == 0)
                    {
                        //зависимое слово - существительное в родительном падеже
                        if((s.Tag & (Tag.Noun | Tag.NounLike)) != 0 &&
                            (SameCase(Tag.Genitive | Tag.Genitive1 | Tag.Genitive2, s.Tag)))
                        {
                            first.AddChild(second, SurfaceRelationName.CopularGenitive);
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
