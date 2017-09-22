using ELIZA.Morphology;

namespace ELIZA.Syntax.SurfaceRelations
{
    /// <summary>
    /// Агентивное ПСО.
    /// </summary>
    /// <seealso cref="AbstractSSR" />
    public class AgentiveSSR: AbstractSSR
    {
        protected override bool TryBuildRelation(Tree<Lexem, SurfaceRelationName> first,
            Tree<Lexem, SurfaceRelationName> second,
            out Tree<Lexem, SurfaceRelationName> head)
        {
            Lexem f = first.Key;
            Lexem s = second.Key;
            head = first;
            //зависимая форма - существительное в творительном падеже, отвечающая на вопрос "кем?"
            //или существительное в родительном падеже, отвечающее на вопрос "кого?"
            if ((s.Tag & (Tag.NounLike | Tag.Noun)) != 0 &&
                (s.Tag & (Tag.Instrumental)) != 0 && 
                (s.Tag & Tag.Animated) != 0)
            {
                //главная слово - глагольная форма или существительное
       
                if (((f.Tag & (Tag.Gerund | Tag.Infinitive | Tag.Participle | Tag.ShortParticiple)) != 0) ||
                    (f.Tag & (Tag.Noun | Tag.Noun)) != 0)
                {
                    head.AddChild(second, SurfaceRelationName.Agentive);
                    return true;
                }
            }
            return false;
        }
    }
}
