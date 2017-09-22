using DawgSharp;

namespace ELIZA.Morphology
{
    /// <summary>
    /// Представляет методы для создания и использования модели классов энтропии.
    /// </summary>
    /// <seealso cref="ELIZA.Morphology.IEntropyClassModel" />
    public class DawgEntropyClassModel: IEntropyClassModel
    {
        protected DawgBuilder<ulong> builder;
        protected Dawg<ulong> dawg;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="DawgEntropyClassModel"/>.
        /// </summary>
        public DawgEntropyClassModel()
        {
            builder = new DawgBuilder<ulong>();
        }

        /// <summary>
        /// Добавляет новую лексему к модели.
        /// </summary>
        /// <param name="lexem">Лексема.</param>
        public void AddLexem(WordForm lexem)
        {
            ulong tagOut = (ulong)Tag.NoWord;
            ulong tag = (ulong)lexem.Tag;
            if (builder.TryGetValue(lexem.Word, out tagOut))
                tag |= tagOut;
            builder.Insert(lexem.Word, tag);
        }
        /// <summary>
        /// Получает класс энтропии для заданной словоформы.
        /// </summary>
        /// <param name="word">Словоформа.</param>
        /// <returns>
        /// Возвращает класс энтропии для заданного слова, если оно есть в модели,
        /// иначе возвращает максимальный класс энтропии.
        /// </returns>
        public Tag GetEntropyClass(string word)
        {
            if (Contains(word))
            {
                return (Tag)dawg[word];
            }
            else return Utils.MaximumEntropyTag;
        }
        /// <summary>
        /// Сохраняет модель в заданный поток.
        /// </summary>
        /// <param name="fs">Поток.</param>
        public void SaveTo(System.IO.Stream fs)
        {
            dawg = builder.BuildDawg();
            dawg.SaveTo(fs);
        }
        /// <summary>
        /// Загружает модель из заданого потока.
        /// </summary>
        /// <param name="fs">Поток.</param>
        public void Load(System.IO.Stream fs)
        {
            builder = new DawgBuilder<ulong>();
            dawg = Dawg<ulong>.Load(fs);
        }
        /// <summary>
        /// Проверяет, содержится ли заданная словоформа в модели.
        /// </summary>
        /// <param name="key">Словоформа.</param>
        /// <returns>
        ///   <c>true</c>, если модель содержит словоформу; иначе, <c>false</c>.
        /// </returns>
        public bool Contains(string key)
        {
            Tag tag = (Tag)dawg[key];
            return tag != Tag.NoWord;
        }
        public void Build()
        {
            this.dawg = builder.BuildDawg();
        }
    }
}
