using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace ELIZA.Morphology
{
    /// <summary>
    /// Предоставляет механизм для чтения предложений из файла корпуса.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public interface ICorporaReader: IDisposable
    {
        /// <summary>
        /// Читает заданное количество предложений из открытого файла.
        /// </summary>
        /// <param name="amount">Количество предложений..</param>
        /// <returns>Возвращает перечисление, содержащее списки лексем для каждого предложения.</returns>
        IEnumerable<List<WordForm>> ReadSentences(int amount);
        /// <summary>
        /// Читает одно предложение из заданного элемента.
        /// </summary>
        /// <param name="element">XML элемент, содержащий предложение.</param>
        /// <returns>Возвращает список лексем прочитанного предложения.</returns>
        List<WordForm> ReadOneSentence(XElement element);
        /// <summary>
        /// OОткрывает файл с заданным именем.
        /// </summary>
        /// <param name="fileName">имя файла.</param>
        void Open(string fileName);
        /// <summary>
        /// Читает из исходного файла-словаря заданной количество словоформ.
        /// </summary>
        /// <param name="wordCount">Количество слов.</param>
        /// <returns>Возвращает перечисление, содержащее прочитанные слова.</returns>
        IEnumerable<WordForm> ReadDictionary(long wordCount);
        /// <summary>
        /// Закрывает все открытые файлы.
        /// </summary>
        void Close();
    }
}
