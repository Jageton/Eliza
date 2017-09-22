using System.Collections.Generic;
using ELIZA.Morphology;
using ELIZA.Syntax.DeepRelations;
using ELIZA.Syntax.SurfaceRelations;

namespace ELIZA.Syntax
{
    /// <summary>
    /// Представляет синтаксическую модель языка.
    /// </summary>
    public interface ISyntaxModel
    {
        /// <summary>
        /// Выполяет поверхностный синтаксический анализ заданного предложения.
        /// </summary>
        /// <param name="sentence">Предложение.</param>
        /// <returns>Возвращает дерево поверхностного разбора.</returns>
        Tree<Lexem, SurfaceRelationName> SurfaceAnalysis(List<Lexem> sentence);
        /// <summary>
        /// Выполяет глубинный синтаксический анализ заданного предложения.
        /// </summary>
        /// <param name="tree">Дерево воерхностного анализа.</param>
        /// <param name="sentence">Предложение.</param>
        /// <returns>Возвращает дерево глубинного анализа.</returns>
        Tree<DForm, DeepRelationName> DeepAnalysis(Tree<Lexem, SurfaceRelationName> tree);
    }
}
