using ELIZA.Morphology;

namespace ELIZA.Syntax.SurfaceRelations
{
    /// <summary>
    /// Предикативное (слабое) ПСО.
    /// </summary>
    /// <seealso cref="AbstractSSR" />
    public class PredicativeWeakSSR: AbstractSSR
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
            if((f.Tag & (Tag.Noun | Tag.NounLike | Tag.Anaphoric)) != 0) //если главное слово - сущ.
            {
                //если зависимое слово существительное, и оба в именительном падеже
                if(SamePos(s.Tag, Tag.Noun | Tag.NounLike | Tag.Adjective | Tag.ShortAdjective) &&
                    (f.Tag & s.Tag & Tag.Nominative) != 0)
                {
                    head = DependencyGrammar.NullVerbLexem;
                    head.AddChild(first, SurfaceRelationName.Predicative);
                    head.AddChild(second, SurfaceRelationName.Completive1);
                    return true;
                }
            }
            return false;
        }
    }
}
