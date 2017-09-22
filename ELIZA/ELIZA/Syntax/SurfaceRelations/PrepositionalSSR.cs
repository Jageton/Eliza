using System.Linq;
using ELIZA.Morphology;

namespace ELIZA.Syntax.SurfaceRelations
{
    /// <summary>
    /// Отсоюзное (отпредложное) ПСО.
    /// </summary>
    /// <seealso cref="AbstractSSR" />
    public class PrepositionalSSR: AbstractSSR
    {
        protected static string[] coorinate = { "и", "или", "а", "но" };

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
            if((f.Tag & Tag.Preposition) != 0 && s.LexemPosition > f.LexemPosition) //если предлог
            {
                if((s.Tag & (Tag.Noun | Tag.NounLike)) != 0)
                {
                    if(SameCase(f.Tag, s.Tag))
                    {
                        first.AddChild(second, SurfaceRelationName.Prepositional);
                        return true;
                    }
                }
                /*else if((s.Tag & Tag.Apro) != 0) 
                {
                    first.AddChild(second, SurfaceRelationName.Prepositional);
                    return true;
                }*/
            }
            else if((f.Tag & Tag.Conjunction) != 0) //если союз
            {
                if(coorinate.Contains(f.Word)) //если союз сочинительный
                {
                    if(first.Children.Count() == 0) //если с союзом ничего не связано
                    {
                        if(s.LexemPosition > f.LexemPosition) //зависимая лексема идёт после союза
                        {
                            //если зависимое слово - самостоятельная часть речи
                            if((s.Tag & (Tag.Preposition | Tag.Conjunction | Tag.Interjunction)) == 0)
                            {
                                first.AddChild(second, SurfaceRelationName.Prepositional);
                                return true;
                            }
                        }
                    }
                }
                else //если союз подчинительный
                {
                    if((s.Tag & (Tag.Verb | Tag.Infinitive)) != 0) //глагольная форма
                    {
                        first.AddChild(second, SurfaceRelationName.Prepositional);
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
