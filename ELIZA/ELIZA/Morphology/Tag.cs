using System;
using ProtoBuf;

namespace ELIZA.Morphology
{
    /// <summary>
    /// Представляет собой тэг и все возможные подтэги части речи.
    /// </summary>
    [Flags]    
    [Serializable]
    [ProtoContract(EnumPassthru = true)]
    [MorphologyGroup(13, "TAG")]
    public enum Tag: ulong
    {
        /// <summary>
        /// Тэг, обозначающий пустое или отсутствующее слово.
        /// </summary>
        [OpenCorporaName("none")]
        [Localisation("ru-RU", "Тэг отсутствует")]
        NoWord = 0,
        /// <summary>
        /// Одушевленный.
        /// </summary>
        [OpenCorporaName("anim")]
        [Localisation("ru-RU", "Одушевлённый")]
        Animated = 1,
        /// <summary>
        /// Неодушевлённый.
        /// </summary>
        [OpenCorporaName("inan")]
        [Localisation("ru-RU", "Неодушевлённый")]
        Inanimate = (ulong)1 << 1,
        /// <summary>
        /// Именительный падеж.
        /// </summary>
        [OpenCorporaName("nomn")]
        [MorphologyGroup(7, "CASE")]
        [Localisation("ru-RU", "Именительный")]
        Nominative = (ulong)1 << 2,
        /// <summary>
        /// Родительный падеж.
        /// </summary>
        [OpenCorporaName("gent")]
        [MorphologyGroup(7, "CASE")]
        [Localisation("ru-RU", "Родительный")]
        Genitive = (ulong)1 << 3,
        /// <summary>
        /// Дательный падеж.
        /// </summary>
        [OpenCorporaName("datv")]
        [MorphologyGroup(7, "CASE")]
        [Localisation("ru-RU", "Дательный")]
        Dative = (ulong)1 << 4,
        /// <summary>
        /// Винительный падеж.
        /// </summary>
        [OpenCorporaName("accs")]
        [MorphologyGroup(7, "CASE")]
        [Localisation("ru-RU", "Винительный")]
        Accusative = (ulong)1 << 5,
        /// <summary>
        /// Творительный падеж.
        /// </summary>
        [OpenCorporaName("ablt")]
        [MorphologyGroup(7, "CASE")]
        [Localisation("ru-RU", "Творительный")]
        Instrumental = (ulong)1 << 6,
        /// <summary>
        /// Предложный падеж.
        /// </summary>
        [OpenCorporaName("loct")]
        [MorphologyGroup(7, "CASE")]
        [Localisation("ru-RU", "Предложный")]
        Prepositional = (ulong)1 << 7,
        /// <summary>
        /// Звательный падеж.
        /// </summary>
        [OpenCorporaName("voct")]
        [MorphologyGroup(7, "CASE")]
        [Localisation("ru-RU", "Звательный")]
        Vocative = (ulong)1 << 8,
        /// <summary>
        /// Первый родительный падеж.
        /// </summary>
        [OpenCorporaName("gen1")]
        [MorphologyGroup(7, "CASE")]
        [Localisation("ru-RU", "Первый родительный")]
        Genitive1 = (ulong)1 << 9,
        /// <summary>
        /// Второй родительный падеж.
        /// </summary>
        [OpenCorporaName("gen2")]
        [MorphologyGroup(7, "CASE")]
        [Localisation("ru-RU", "Второй родительный")]
        Genitive2 = (ulong)1 << 10,
        /// <summary>
        /// Второй винительный падеж.
        /// </summary>
        [OpenCorporaName("acc2")]
        [MorphologyGroup(7, "CASE")]
        [Localisation("ru-RU", "Второй винительный")]
        Accusative2 = (ulong)1 << 11,
        /// <summary>
        /// Первый предложный падеж.
        /// </summary>
        [OpenCorporaName("loc1")]
        [MorphologyGroup(7, "CASE")]
        [Localisation("ru-RU", "Первый предложный")]
        Prepositional1 = (ulong)1 << 12,
        /// <summary>
        /// Второй предложный (местный) падеж.
        /// </summary>
        [OpenCorporaName("loc2")]
        [MorphologyGroup(7, "CASE")]
        [Localisation("ru-RU", "Второй предложный")]
        Prepositional2 = (ulong)1 << 13,
        /// <summary>
        /// Неизменяемый.
        /// </summary>
        [OpenCorporaName("Fixd")]
        [Localisation("ru-RU", "Неизменяемый")]
        Fixed = (ulong)1 << 14,
        /// <summary>
        /// Совершенный вид.
        /// </summary>
        [OpenCorporaName("perf")]
        [MorphologyGroup(20, "FORM")]
        [Localisation("ru-RU", "Совершенный вид")]
        Perfect = (ulong)1 << 15,
        /// <summary>
        /// Несовершенный вид.
        /// </summary>
        [OpenCorporaName("impf")]
        [MorphologyGroup(20, "FORM")]
        [Localisation("ru-RU", "Несовершенный вид")]
        Imperfect = (ulong)1 << 16,
        /// <summary>
        /// Мужской род.
        /// </summary>
        [OpenCorporaName("masc")]
        [MorphologyGroup(4, "GENDER")]
        [Localisation("ru-RU", "Мужской род")]
        Masculine = (ulong)1 << 17,
        /// <summary>
        /// Женский род.
        /// </summary>
        [OpenCorporaName("femn")]
        [MorphologyGroup(4, "GENDER")]
        [Localisation("ru-RU", "Женский род")]
        Feminine = (ulong)1 << 18,
        /// <summary>
        /// Средний род.
        /// </summary>
        [OpenCorporaName("neut")]
        [MorphologyGroup(4, "GENDER")]
        [Localisation("ru-RU", "Средний род")]
        Neutral = (ulong)1 << 19,
        /// <summary>
        /// Изъявительное наклонение.
        /// </summary>
        [OpenCorporaName("indc")]
        [MorphologyGroup(5, "MOOD")]
        [Localisation("ru-RU", "Изъявительное наклонение")]
        Indicative = (ulong)1 << 20,
        /// <summary>
        /// Повелительное наклонение.
        /// </summary>
        [OpenCorporaName("impr")]
        [MorphologyGroup(5, "MOOD")]
        [Localisation("ru-RU", "Повелительное наклонение")]
        Imperative = (ulong)1 << 21,
        /// <summary>
        /// Единственное.
        /// </summary>
        [OpenCorporaName("sing")]
        [MorphologyGroup(3, "NUMBER")]
        [Localisation("ru-RU", "Единственное число")]
        Single = (ulong)1 << 22,
        /// <summary>
        /// Множественное.
        /// </summary>
        [OpenCorporaName("plur")]
        [MorphologyGroup(3, "NUMBER")]
        [Localisation("ru-RU", "Множественное число")]
        Plural = (ulong)1 << 23,
        [OpenCorporaName("NOUN")]
        [MorphologyGroup(1, "POS")]
        [Localisation("ru-RU", "Существительное")]
        Noun = (ulong)1 << 24,
        [OpenCorporaName("ADJF")]
        [MorphologyGroup(1, "POS")]
        [Localisation("ru-RU", "Прилагательное")]
        Adjective = (ulong)1 << 25,
        [OpenCorporaName("ADJS")]
        [MorphologyGroup(1, "POS")]
        [Localisation("ru-RU", "Краткое прилагательное")]
        ShortAdjective = (ulong)1 << 26,
        [OpenCorporaName("COMP")]
        [MorphologyGroup(1, "POS")]
        [Localisation("ru-RU", "Компаратив")]
        Comparative = (ulong)1 << 27,
        [OpenCorporaName("VERB")]
        [MorphologyGroup(1, "POS")]
        [Localisation("ru-RU", "глагол")]
        Verb = (ulong)1 << 28,
        [OpenCorporaName("INFN")]
        [MorphologyGroup(1, "POS")]
        [Localisation("ru-RU", "Инфинитив")]
        Infinitive = (ulong)1 << 29,
        [OpenCorporaName("PRTF")]
        [MorphologyGroup(1, "POS")]
        [Localisation("ru-RU", "Причастие")]
        Participle = (ulong)1 << 30,
        [OpenCorporaName("PRTS")]
        [MorphologyGroup(1, "POS")]
        [Localisation("ru-RU", "Краткое причастие")]
        ShortParticiple = (ulong)1 << 31,
        [OpenCorporaName("GRND")]
        [MorphologyGroup(1, "POS")]
        [Localisation("ru-RU", "Деепричастие")]
        Gerund = (ulong)1 << 32,
        [OpenCorporaName("NUMR")]
        [MorphologyGroup(1, "POS")]
        [Localisation("ru-RU", "Числительное")]
        Numeric = (ulong)1 << 33,
        [OpenCorporaName("ADVB")]
        [MorphologyGroup(1, "POS")]
        [Localisation("ru-RU", "Наречие")]
        Adverb = (ulong)1 << 34,
        [OpenCorporaName("NPRO")]
        [MorphologyGroup(1, "POS")]
        [Localisation("ru-RU", "Местоимение")]
        NounLike = (ulong)1 << 35,
        [OpenCorporaName("PRED")]
        [MorphologyGroup(1, "POS")]
        [Localisation("ru-RU", "Предикатив")]
        Predicate = (ulong)1 << 36,
        [OpenCorporaName("PREP")]
        [MorphologyGroup(1, "POS")]
        [Localisation("ru-RU", "Предлог")]
        Preposition = (ulong)1 << 37,
        [OpenCorporaName("CONJ")]
        [MorphologyGroup(1, "POS")]
        [Localisation("ru-RU", "Союз")]
        Conjunction = (ulong)1 << 38,
        [OpenCorporaName("PRCL")]
        [MorphologyGroup(1, "POS")]
        [Localisation("ru-RU", "Частица")]
        Particle = (ulong)1 << 39,
        [OpenCorporaName("INTJ")]
        [MorphologyGroup(1, "POS")]
        [Localisation("ru-RU", "Междометье")]
        Interjunction = (ulong)1 << 40,
        [OpenCorporaName("PNCT")]
        [Localisation("ru-RU", "Пунктуация")]
        Punctuation = (ulong)1 << 41,
        /// <summary>
        /// Первое лицо.
        /// </summary>
        [OpenCorporaName("1per")]
        [MorphologyGroup(8, "PERSONA")]
        [Localisation("ru-RU", "Первое лицо")]
        FirstPerson = (ulong)1 << 42,
        /// <summary>
        /// Второй лицо.
        /// </summary>
        [OpenCorporaName("2per")]
        [MorphologyGroup(8, "PERSONA")]
        [Localisation("ru-RU", "Второе лицо")]
        SecondPerson = (ulong)1 << 43,
        /// <summary>
        /// Третьве лицо.
        /// </summary>
        [OpenCorporaName("3per")]
        [MorphologyGroup(8, "PERSONA")]
        [Localisation("ru-RU", "Третье лицо")]
        ThirdPerson = (ulong)1 << 44,
        /// <summary>
        /// Настоящее время.
        /// </summary>
        [OpenCorporaName("pres")]
        [MorphologyGroup(10, "TIME")]
        [Localisation("ru-RU", "Настоящее время")]
        Present = (ulong)1 << 45,
        /// <summary>
        /// Прошедшее время.
        /// </summary>
        [OpenCorporaName("past")]
        [MorphologyGroup(10, "TIME")]
        [Localisation("ru-RU", "Прошедшее время")]
        Past = (ulong)1 << 46,
        /// <summary>
        /// Будущее время.
        /// </summary>
        [OpenCorporaName("futr")]
        [MorphologyGroup(10, "TIME")]
        [Localisation("ru-RU", "Будущее время")]
        Future = (ulong)1 << 47,
        /// <summary>
        /// Переходный.
        /// </summary>
        [OpenCorporaName("tran")]
        [MorphologyGroup(11, "TRANS")]
        [Localisation("ru-RU", "Переходный")]
        Transitive = (ulong)1 << 48,
        /// <summary>
        /// Непереходный.
        /// </summary>
        [OpenCorporaName("intr")]
        [MorphologyGroup(11, "TRANS")]
        [Localisation("ru-RU", "Непереходный")]
        Intransitive = (ulong)1 << 49,
        /// <summary>
        /// Активный залог.
        /// </summary>
        [OpenCorporaName("actv")]
        [MorphologyGroup(12, "VOIC")]
        [Localisation("ru-RU", "Активный залог")]
        Active = (ulong)1 << 50,
        /// <summary>
        /// Пассивный залог.
        /// </summary>
        [OpenCorporaName("pssv")]
        [MorphologyGroup(12, "VOIC")]
        [Localisation("ru-RU", "Пассивный залог")]
        Passive = (ulong)1 << 51,
        [OpenCorporaName("NUMB")]
        [Localisation("ru-RU", "Число")]
        Number = (ulong)1 << 52,
        [OpenCorporaName("no_such_name")]
        [Localisation("ru-RU", "Изменяемое")]
        Unfixed = (ulong)1 << 53,
        /// <summary>
        /// Анафорическое местоимение.
        /// </summary>
        [OpenCorporaName("Anph")]
        [Localisation("ru-RU", "Анафорическая ссылка")]
        Anaphoric = (ulong) 1 << 54,
        /// <summary>
        /// Местоимённое определение.
        /// </summary>        
        [Localisation("ru-RU", "Местоимённое")]
        [OpenCorporaName("Apro")]
        Apro = (ulong) 1 << 55,
        [Localisation("ru-RU", "Специальный символ")]
        Phantom = (ulong) 1 << 56,
        [OpenCorporaName("Dmns")]
        [Localisation("ru-RU", "Указательное")]
        Pointing = (ulong) 1 << 57,
        [OpenCorporaName("Qual")]
        [Localisation("ru-RU", "Качественное")]
        Qualitative = (ulong) 1 << 58
    }
}
