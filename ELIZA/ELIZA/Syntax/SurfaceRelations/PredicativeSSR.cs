using System.Linq;
using ELIZA.Morphology;

namespace ELIZA.Syntax.SurfaceRelations
{
    public class PredicativeSSR: AbstractSSR
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
            //TODO: разделить на два отношения
            //одно слабое, при отсутствии сильного
            //другое - сильное
            Lexem f = first.Key;
            Lexem s = second.Key;
            head = first;
            if((f.Tag & (Tag.Verb | Tag.Infinitive)) != 0) //если главное слово - глагол
            {
                //если глагол безличный
                if ((f.Tag & (Tag.FirstPerson | Tag.SecondPerson | Tag.ThirdPerson)) == 0)  
                {
                    if (SamePos(s.Tag, Tag.Noun) && SameGender(s.Tag, Tag.Genitive))
                    {
                        first.AddChild(second, SurfaceRelationName.Predicative);
                        return true;
                    }
                }
                //если зависимое слово существительное или другая часть речи в роли сущ.
                if ((s.Tag & (Tag.Noun | Tag.NounLike | Tag.Apro | Tag.Anaphoric)) != 0)
                {
                    if ((s.Tag & Tag.Nominative) != 0 && SameNumber(f.Tag, s.Tag))
                    {
                        if ((f.Tag & Tag.Past) != 0 && SameGender(f.Tag, s.Tag) ||
                            SamePos(s.Tag, Tag.Noun | Tag.Apro))
                        {
                            first.AddChild(second, SurfaceRelationName.Predicative);
                            return true;
                        }
                    }
                }
                else  if((s.Tag & Tag.Infinitive) != 0 && s.LexemPosition - f.LexemPosition > 1)
                {
                    first.AddChild(second, SurfaceRelationName.Predicative);
                    return true;
                }
            }
            else if(f.Word == "это") //если главно слово - указательное местоимение "это"
            {
                //если нет детей и одна из предшествующих лексем - существительное
                //в именительном падеже или инфинитив
                if (first.Key.LexemPosition > second.Key.LexemPosition &&
                    first.Children.Count() == 0 &&
                    ((s.Tag & (Tag.NounLike | Tag.Noun)) != 0 ||
                     (s.Tag & Tag.Infinitive) != 0))
                {
                    first.AddChild(second, SurfaceRelationName.Predicative);
                    return true;
                }
            }
            return false;
        }
    }
}
