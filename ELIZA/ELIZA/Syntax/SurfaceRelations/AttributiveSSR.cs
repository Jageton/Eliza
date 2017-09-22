using ELIZA.Morphology;

namespace ELIZA.Syntax.SurfaceRelations
{
    /// <summary>
    /// Определительное ПСО.
    /// </summary>
    /// <seealso cref="AbstractSSR" />
    public class AttributiveSSR: AbstractSSR
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
            //анафорические прилагательные не рассматриваются
            if ((s.Tag & Tag.Anaphoric) != 0) return false;
            //главная форма - существительное
            if((f.Tag & (Tag.Noun | Tag.NounLike)) != 0)
            {
                //подчинённая форма - части речи, обозначающие признаки (кроме наречия) (связь предмет - признак предмета)
                if((s.Tag & (Tag.Adjective | Tag.ShortAdjective | Tag.Participle | Tag.ShortParticiple)) != 0)
                {
                    if(SameCase(f.Tag, s.Tag)  && SameNumber(f.Tag, s.Tag))
                    {
                        if (SameNumber(f.Tag, Tag.Single) && !SameGender(f.Tag, s.Tag))
                            return false;
                        first.AddChild(second, SurfaceRelationName.Attributive);
                        return true;
                    }
                }
            }
            //если главное слово - глагольная форма
            else if((f.Tag & (Tag.Gerund | Tag.Verb | Tag.Infinitive | Tag.Participle | Tag.ShortParticiple)) != 0)
            {
                if(SamePos(s.Tag, Tag.ShortAdjective | Tag.Adverb)) //подчинённая форма - наречие (связь действие - признак действия)
                {
                    first.AddChild(second, SurfaceRelationName.Attributive);
                    return true;
                }
            }
            //если главное слово - признак
            else if((f.Tag & (Tag.Adjective | Tag.ShortAdjective | Tag.Participle | Tag.ShortParticiple)) != 0)
            {
                if((s.Tag & Tag.Particle) != 0) //подчинённая форма - частица (связь признак - признак признака)
                {
                    first.AddChild(second, SurfaceRelationName.Attributive);
                    return true;
                }
            }
            return false;
        }
    }
}
