using System.IO;

namespace ELIZA.Morphology
{
    /// <summary>
    /// Представляет методы для создания и использования модели классов энтропии.
    /// </summary>
    public interface IEntropyClassModel
    {
        /// <summary>
        /// Добавляет новую лексему к модели.
        /// </summary>
        /// <param name="lexem">Лексема.</param>
        void AddLexem(WordForm lexem);
        /// <summary>
        /// Получает класс энтропии для заданной словоформы.
        /// </summary>
        /// <param name="word">Словоформа.</param>
        /// <returns>Возвращает класс энтропии для заданного слова, если оно есть в модели, 
        /// иначе возвращает максимальный класс энтропии.</returns>
        Tag GetEntropyClass(string word);
        /// <summary>
        /// Сохраняет модель в заданный поток.
        /// </summary>
        /// <param name="fs">Поток.</param>
        void SaveTo(Stream fs);
        /// <summary>
        /// Загружает модель из заданого потока.
        /// </summary>
        /// <param name="fs">Поток.</param>
        void Load(Stream fs);
        /// <summary>
        /// Проверяет, содержится ли заданная словоформа в модели.
        /// </summary>
        /// <param name="key">Словоформа.</param>
        /// <returns>
        ///   <c>true</c>, если модель содержит словоформу; иначе, <c>false</c>.
        /// </returns>
        bool Contains(string key);
    }
}
