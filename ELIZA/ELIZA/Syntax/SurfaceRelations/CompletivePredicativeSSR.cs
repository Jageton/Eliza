using System.Linq;
using ELIZA.Morphology;

namespace ELIZA.Syntax.SurfaceRelations
{
    /// <summary>
    /// Комплетивно-копредикативное ПСО.
    /// </summary>
    /// <seealso cref="AbstractSSR" />
    public class CompletivePredicativeSSR: AbstractSSR
    {
        //вспомогатльеный массив местоимений
        protected static string[] nounLike = { "его", "её", "нас", "их", "меня" };

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
            if((f.Tag & Tag.Verb) !=0 && (f.Tag & Tag.Transitive) != 0) //главное слово - транзитивный глагол
            {
                if(first.Children.Any((a) => nounLike.Contains(a.Key.Word)))
                {
                    if((s.Tag & Tag.Instrumental) != 0)
                    {
                        if((s.Tag & (Tag.Noun | Tag.Adjective)) != 0)
                        {
                            first.AddChild(second, SurfaceRelationName.CompletivePredicative);
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
