using ELIZA.Morphology;
using ELIZA.Syntax.SurfaceRelations;

namespace ELIZA.Syntax
{
    /// <summary>
    /// Представляет поверзностно-синтаксическое отношение.
    /// </summary>
    public interface ISurfaceRelation
    {
        /// <summary>
        /// Пытается построить поверхностсон-синтаксическое отношение между двумя синтаксическими 
        /// деревьями.
        /// </summary>
        /// <param name="first">Первое дерево.</param>
        /// <param name="second">Второе дерево.</param>
        /// <param name="head">Главное дерево в отношении.</param>
        /// <returns>Возвращает <c>true</c>, если удалось построить отношение, 
        /// иначе возвращает <c>false</c>.</returns>
        bool BuildRelation(Tree<Lexem, SurfaceRelationName> first, Tree<Lexem,
            SurfaceRelationName> second, out Tree<Lexem, SurfaceRelationName> head);
    }
}
