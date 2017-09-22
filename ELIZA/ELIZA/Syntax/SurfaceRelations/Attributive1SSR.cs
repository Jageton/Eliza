using System;
using System.Linq;
using ELIZA.Morphology;

namespace ELIZA.Syntax.SurfaceRelations
{
    /// <summary>
    /// Аттрибутивное ПСО.
    /// </summary>
    /// <seealso cref="AbstractSSR" />
    public class Attributive1SSR: AbstractSSR
    {
        protected override bool TryBuildRelation(Tree<Lexem, SurfaceRelationName> first,
            Tree<Lexem, SurfaceRelationName> second,
            out Tree<Lexem, SurfaceRelationName> head)
        {
            Lexem f = first.Key;
            Lexem s = second.Key;
            head = first;
            //свалка
            //в эту группу помещаются все несогласованные определения
            //чаще всего они стоят недалеко от определяемого слова
            if((s.LexemPosition > f.LexemPosition && Math.Abs(f.LexemPosition - s.LexemPosition) <= 2) ||
                second.Count() < 4)
            {
                //если главное слово - существительное
                if((f.Tag & (Tag.Noun | Tag.Noun)) != 0)
                {
                    //если зависимое слово - группа существительного
                    if((s.Tag & (Tag.NounLike | Tag.Noun)) != 0 && 
                        s.LexemPosition > f.LexemPosition)
                    {
                        first.AddChild(second, SurfaceRelationName.Attributive1);
                        return true;
                    }
                    else if ((s.Tag & Tag.Preposition) != 0 && 
                        second.Dependencies.Any(a => a.Key == SurfaceRelationName.Prepositional) && 
                        s.LexemPosition - f.LexemPosition == 1)
                    {
                        first.AddChild(second, SurfaceRelationName.Attributive1);
                        return true;
                    }
                }
            }
            else //но может стоять далеко
            {
                //если главное не слово - существительное
                if ((s.Tag & (Tag.Noun | Tag.Noun)) == 0) return false;
                //если зависимое слово - личное предложение, содержащее анафорическую ссылку (который, где, т.е. и др.)
                if ((f.Tag & Tag.Verb) != 0 &
                    first.Children.Any((a) => (a.Key.Tag & (Tag.Apro | Tag.Anaphoric)) != 0))
                {
                    first.AddChild(second, SurfaceRelationName.Attributive1);
                    //удалить анафорическую ссылку
                    return true;
                }
            }
            return false;
        }
    }
}
