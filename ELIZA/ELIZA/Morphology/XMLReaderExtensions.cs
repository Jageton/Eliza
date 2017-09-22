using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace ELIZA.Morphology
{
    /// <summary>
    /// Содержит методы-расширения для <see cref="System.Xml.XmlReader"/>.
    /// </summary>
    public static class XMLReaderExtensions
    {
        /// <summary>
        /// Последовательно возвращает все элементы с заданным именем.
        /// </summary>
        /// <param name="reader">Текущий XML reader.</param>
        /// <param name="matchName">Имя искомого XML элемента..</param>
        /// <returns>Последовательно возвращает все элементы с заданным именем.</returns>
        public static IEnumerable<XElement> GetAllElements(this XmlReader reader, string matchName)
        {
            while (reader.ReadToFollowing(matchName))
                yield return (XElement)XElement.ReadFrom(reader);
        }
    }
}
