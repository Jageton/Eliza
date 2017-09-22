using ELIZA.Morphology;

namespace ELIZA.Syntax.SurfaceRelations
{
    /// <summary>
    /// Дополнительное комплетивное ПСО.
    /// </summary>
    /// <seealso cref="AbstractSSR" />
    public class AdditionalComplementarySSR: AbstractSSR
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
            Tree<Lexem, SurfaceRelationName> second, out Tree<Lexem,
            SurfaceRelationName> head)
        {
            Lexem f = first.Key;
            Lexem s = second.Key;
            head = first;
            if((f.Tag & (Tag.Verb | Tag.Gerund | Tag.Infinitive)) != 0)
            {
                //без модели управления для каждого слова будем считать, что пользователь употребляет их правильно
                //дополнительные актанты добавляется с отношением "второе комплетивное"
                //в предложении глагол редко имеет валентность > 2
                //так же часто актанты находятся близко к глаголу
                //данная реализация исходит из этих предположений и покрывает большое количество случаев
                if (((f.Tag & (Tag.FirstPerson | Tag.SecondPerson | Tag.ThirdPerson)) != 0 ||
                    SamePos(f.Tag, Tag.Infinitive | Tag.Gerund | Tag.Participle | Tag.ShortParticiple)) && 
                    (s.Tag & (Tag.Noun | Tag.NounLike | Tag.Preposition)) != 0)
                {
                    head.AddChild(second, SurfaceRelationName.Completive2);
                    return true;
                }
            }
            return false;
        }
    }
}
