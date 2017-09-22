using ELIZA.Morphology;

namespace ELIZA.Syntax.SurfaceRelations
{
    public abstract class AbstractSSR: ISurfaceRelation
    {

        #region ISurfaceRelation Members

        /// <summary>
        /// Пытается построить поверхностсон-синтаксическое отношение между двумя синтаксическими
        /// деревьями.
        /// </summary>
        /// <param name="first">Первое дерево.</param>
        /// <param name="second">Второе дерево.</param>
        /// <param name="head">Главное дерево в отношении.</param>
        /// <returns>
        /// Возвращает <c>true</c>, если удалось построить отношение,
        /// иначе возвращает <c>false</c>.
        /// </returns>
        public bool BuildRelation(Tree<Lexem, SurfaceRelationName> first,
            Tree<Lexem, SurfaceRelationName> second, out Tree<Lexem,
            SurfaceRelationName> head)
        {
            if(TryBuildRelation(first, second, out head))
                return true;
            head = null;
            return false;
        }

        #endregion

        /// <summary>
        /// Пытается построить поверхностсон-синтаксическое отношение между двумя синтаксическими
        /// деревьями.
        /// </summary>
        /// <param name="first">Первое дерево.</param>
        /// <param name="second">Второе дерево.</param>
        /// <returns>
        /// Возвращает <c>true</c>, если удалось построить отношение,
        /// иначе возвращает <c>false</c>.
        /// </returns>
        protected abstract bool TryBuildRelation(Tree<Lexem, SurfaceRelationName> first,
            Tree<Lexem, SurfaceRelationName> second, out Tree<Lexem,
            SurfaceRelationName> head);

        /// <summary>
        /// Проверяет, что у тэгов совпадает подтэг "род".
        /// </summary>
        /// <param name="first">Первый тэг.</param>
        /// <param name="second">Второй тэг.</param>
        /// <returns>Возвращает <c>true</c>, если у тэгов совпадает заданный подтэг.</returns>
        protected bool SameGender(Tag first, Tag second)
        {
            return (first & second & (Tag.Feminine | Tag.Masculine | Tag.Neutral)) != 0;
        }
        /// <summary>
        /// Проверяет, что у тэгов совпадает подтэг "число".
        /// </summary>
        /// <param name="first">Первый тэг.</param>
        /// <param name="second">Второй тэг.</param>
        /// <returns>Возвращает <c>true</c>, если у тэгов совпадает заданный подтэг.</returns>
        protected bool SameNumber(Tag first, Tag second)
        {
            return (first & second & (Tag.Single | Tag.Plural)) != 0;
        }
        /// <summary>
        /// Проверяет, что у тэгов совпадает подтэг "падеж".
        /// </summary>
        /// <param name="first">Первый тэг.</param>
        /// <param name="second">Второй тэг.</param>
        /// <returns>Возвращает <c>true</c>, если у тэгов совпадает заданный подтэг.</returns>
        protected bool SameCase(Tag first, Tag second)
        {
            return (first & second & (Tag.Accusative | Tag.Accusative2 | Tag.Dative |
                                      Tag.Genitive | Tag.Genitive1 | Tag.Genitive2 | Tag.Instrumental |
                                      Tag.Nominative | Tag.Prepositional | Tag.Prepositional1 |
                                      Tag.Prepositional2 | Tag.Vocative)) != 0 ||
                   ((first & (Tag.Genitive | Tag.Genitive1 | Tag.Genitive2)) != 0 &&
                    (second & (Tag.Genitive | Tag.Genitive1 | Tag.Genitive2)) != 0) ||
                   ((first & (Tag.Prepositional | Tag.Prepositional1 | Tag.Prepositional2)) != 0 &&
                    (second & (Tag.Prepositional | Tag.Prepositional1 | Tag.Prepositional2)) != 0);
        }
        /// <summary>
        /// Проверяет, что у тэгов совпадает пожтгэ "часть речи".
        /// </summary>
        /// <param name="first">Первый тэг.</param>
        /// <param name="second">Второй тэг.</param>
        /// <returns>Возвращает <c>true</c>, если у тэгов совпадает заданный подтэг.</returns>
        protected bool SamePos(Tag first, Tag second)
        {
            return (first & second & (Tag.Adjective | Tag.Adverb | Tag.Comparative | Tag.Conjunction | Tag.Gerund |
                Tag.Infinitive | Tag.Interjunction | Tag.Noun | Tag.NounLike | Tag.Number | Tag.Numeric |
                Tag.Participle | Tag.Particle | Tag.Predicate | Tag.Preposition | Tag.ShortAdjective | Tag.ShortParticiple |
                Tag.Verb)) != 0;
        }
    }
}
