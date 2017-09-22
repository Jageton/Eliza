using System;
using ELIZA.Morphology; 

namespace ELIZA.Syntax.SurfaceRelations
{
    /// <summary>
    /// Обстоятельственное ПСО.
    /// </summary>
    /// <seealso cref="AbstractSSR" />
    public class AdverbalSSR: AbstractSSR
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
            if((f.Tag & (Tag.Participle | Tag.ShortParticiple | Tag.Verb |
                Tag.Infinitive | Tag.Gerund)) !=0) //если главное слово - глагольная форма
            {
                if((s.Tag & (Tag.Adverb | Tag.Gerund)) != 0)
                {
                    first.AddChild(second, SurfaceRelationName.Adverbial);
                    return true;
                }
                /*else if((s.Tag & (Tag.Preposition | Tag.Noun)) != 0)
                {
                    first.AddChild(second, SurfaceRelationName.Adverbial);
                    return true;
                }*/
            }
            //если главное слово - форма прилагательного
            else if((f.Tag & (Tag.Adjective | Tag.ShortAdjective | Tag.Participle |
                Tag.ShortParticiple)) != 0)
            {
                if(Math.Abs(f.LexemPosition - s.LexemPosition) <= 2 && (s.Tag & Tag.Particle) != 0)
                {
                    first.AddChild(second, SurfaceRelationName.Adverbial);
                    return true;
                }
            }
            return false;
        }
    }
}
