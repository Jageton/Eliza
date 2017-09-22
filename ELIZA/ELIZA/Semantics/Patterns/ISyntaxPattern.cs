using ELIZA.Syntax;
using ELIZA.Syntax.DeepRelations;

namespace ELIZA.Semantics.Patterns
{
    public interface ISyntaxPattern
    {
        /// <summary>
        /// Проверяет соответствие паттерна заданному дереву.
        /// </summary>
        /// <param name="tree">Проверяемое дерево.</param>
        /// <returns>Возвращает <c>true</c>, если заданное дерево соответствует паттерну, иначе 
        /// возвращает <c>false</c>.</returns>
        bool Match(Tree<DForm, DeepRelationName> tree);
        /// <summary>
        /// Получает сохранённое значение, если таковое имеется, по имени.
        /// </summary>
        /// <param name="name">Имя сохраняемого значения.</param>
        /// <returns>Возвращает <c>true</c>, если значение найдно, иначе 
        /// возвращает <c>false</c>.</returns>
        bool TryGetSavedValue(string name, out string value);
    }
}
