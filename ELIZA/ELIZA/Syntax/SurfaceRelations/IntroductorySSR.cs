using System.Linq;
using ELIZA.Morphology;

namespace ELIZA.Syntax.SurfaceRelations
{
    /// <summary>
    /// Вводное ПСО.
    /// </summary>
    /// <seealso cref="AbstractSSR" />
    public class IntroductorySSR: AbstractSSR
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
            if((f.Tag & (Tag.Verb | Tag.Infinitive)) != 0)
            {
                if((s.Tag & Tag.Interjunction) != 0)
                {
                    bool isPersonal = second.Dependencies.Any((a) => ((a.Key & SurfaceRelationName.Predicative) != 0) &&
                        ((a.Value.Key.Tag & (Tag.FirstPerson | Tag.SecondPerson | Tag.FirstPerson)) == 0));
                    if(!isPersonal) //если предложение безличное
                    {
                        first.AddChild(second, SurfaceRelationName.Introductory);
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
