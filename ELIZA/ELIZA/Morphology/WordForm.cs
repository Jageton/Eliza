using System;
using System.Globalization;

namespace ELIZA.Morphology
{
    /// <summary>
    /// Представляет слово исходного языка с морфологическими аттрибутами.
    /// </summary>
    [Serializable]
    public class WordForm
    {
        protected string word;
        protected Tag tag;

        /// <summary>
        /// Получает или задаёт слово исходного языка.
        /// </summary>
        public string Word
        {
            get { return word; }
            set { this.word = value; }
        }

        public string Lemma { get; set; }

        /// <summary>
        /// Получает или задаёт морфологические аттрибуты лексемы.
        /// </summary>
        public Tag Tag
        {
            get { return tag; }
            set { this.tag = value; }
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="WordForm"/>.
        /// </summary>
        /// <param name="tag">Морфологические аттрибуты слова.</param>
        public WordForm(): this(string.Empty, Tag.NoWord)
        {

        }
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="WordForm"/>.
        /// </summary>
        /// <param name="word">Слово исходного языка.</param>
        public WordForm(string word): this(word, Utils.MaximumEntropyTag)
        {

        }
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="WordForm"/>.
        /// </summary>
        /// <param name="word">Слово исходного языка.</param>
        /// <param name="tag">Морфологические аттрибуты слова.</param>
        public WordForm(string word, Tag tag)
        {
            this.word = word;
            this.tag = tag;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}: {1} [{2}]", word, Lemma,
                tag.ToString(CultureInfo.CurrentCulture, true));
        }
    }
}
