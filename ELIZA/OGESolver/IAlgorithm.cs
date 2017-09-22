using System;

namespace OGESolver
{
    public interface IAlgorithm<out T>
    {
        /// <summary>
        /// Получает программный идентификатор алгоритма.
        /// </summary>
        /// <value>Программный идентификатор алгоритма.</value>
        string Name 
        { get; }

        /// <summary>
        /// Возвращает результат работы алгоритма с ранее установленными параметрами.
        /// </summary>
        /// <returns>Возвращает объект типа <see cref="{T}"/>, являющийся результатом
        /// работы алгоритма.</returns>
        T Execute();

        
        /// <summary>
        /// Возвращает строку с объяснениями алгоритма.
        /// </summary>
        /// <returns>Возвращает строку с объяснениями алгоритма.</returns>
        [Obsolete("Возможно, стоит вынести объяснения во внешний модуль.")]
        string GetIllustration();
    }
}
