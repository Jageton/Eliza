using System.Text;

namespace OGESolver
{
    public abstract class AbstractAlgorithm<T>: IAlgorithm<T>
    {
        protected StringBuilder sb;
        protected string name;

        #region IAlgorithm<T> Members

        /// <summary>
        /// Получает программный идентификатор алгоритма.
        /// </summary>
        /// <value>Программный идентификатор алгоритма.</value>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// Возвращает результат работы алгоритма с ранее установленными параметрами.
        /// </summary>
        /// <returns>Возвращает объект типа <see cref="{T}" />, являющийся результатом
        /// работы алгоритма.</returns>
        public abstract T Execute();
        /// <summary>
        /// Возвращает строку с объяснениями алгоритма.
        /// /// </summary>
        /// <returns>Возвращает строку с объяснениями алгоритма.</returns>
        public string GetIllustration()
        {
            return sb.ToString();
        }
        #endregion
    }
}
