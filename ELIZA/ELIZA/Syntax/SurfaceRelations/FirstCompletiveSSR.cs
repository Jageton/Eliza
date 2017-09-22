using System;
using System.Linq;
using ELIZA.Morphology;

namespace ELIZA.Syntax.SurfaceRelations
{
    public class FirstCompletiveSSR: AbstractSSR
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
            if((f.Tag & (Tag.Verb | Tag.Infinitive | Tag.Gerund)) != 0) //главное слово - глагол
            {
                if((f.Tag & Tag.Transitive) != 0) //переходный глагол
                {
                    if((s.Tag & (Tag.Noun | Tag.NounLike)) != 0 &&
                        SameCase(Tag.Accusative | Tag.Accusative2, s.Tag) || 
                        (first.Children.Any((a) => a.Key.Word == "не") && 
                        SameCase(Tag.Genitive | Tag.Genitive1 | Tag.Genitive2, s.Tag)) || 
                        s.Word == "что")
                    {
                        first.AddChild(second, SurfaceRelationName.Completive1);
                        return true;
                    }
                }
            }
            else if((f.Tag & (Tag.Participle | Tag.ShortParticiple)) != 0)
            {
                if((s.Tag & (Tag.Noun | Tag.NounLike)) != 0 && SameCase(Tag.Genitive, s.Tag))
                {
                    first.AddChild(second, SurfaceRelationName.Completive1);
                    return true;
                }
            }
            else if ((f.Tag & (Tag.Noun | Tag.NounLike)) != 0)
            {
                if(SamePos(s.Tag, Tag.Noun | Tag.NounLike) &&
                    SameCase(Tag.Genitive | Tag.Genitive1 | Tag.Genitive2 | Tag.Dative, s.Tag)
                    && Math.Abs(s.LexemPosition - f.LexemPosition) <= 2 && 
                    s.LexemPosition > f.LexemPosition)
                {
                    first.AddChild(second, SurfaceRelationName.Completive1);
                    return true;
                }
            }
            return false;
        }
    }
}
