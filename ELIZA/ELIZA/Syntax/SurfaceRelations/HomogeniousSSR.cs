using System.Linq;
using ELIZA.Morphology;

namespace ELIZA.Syntax.SurfaceRelations
{
    /// <summary>
    /// Однородное ПСО.
    /// </summary>
    /// <seealso cref="AbstractSSR" />
    public class HomogeniousSSR: AbstractSSR
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
            //в данном ПСО зависимое слово находится всегда позже главного
            if(s.LexemPosition > f.LexemPosition)
            {
                //если зависимое слово сочинительный союз
                if((f.Tag & Tag.Conjunction) != 0 && coorinate.Contains(f.Word)) 
                {
                    Lexem t = second.Children.First().Key; //зависимое слово союза
                    //с союзом связана предложная группа или существительное
                    if((t.Tag & (Tag.Preposition | Tag.Noun | Tag.NounLike)) != 0)
                    {
                        //зависимое слово тоже предложная группа или существительное
                        if((s.Tag & (Tag.Preposition | Tag.Noun | Tag.NounLike)) != 0)
                        {
                            //если совпадает падеж
                            if(SameCase(t.Tag, s.Tag))
                            {
                                first.AddChild(second, SurfaceRelationName.Homogenious);
                                return true;
                            }
                        }
                    }
                    else 
                    {
                        //если частица, то смотрим на следующее слово в дереве
                        if((t.Tag & Tag.Participle) != 0) 
                            t = second.Children.First().Children.First().Key;
                        if(SamePos(t.Tag, s.Tag)) //если совпадают части речи
                        {
                            first.AddChild(second, SurfaceRelationName.Homogenious);
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
