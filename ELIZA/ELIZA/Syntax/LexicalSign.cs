using System;

namespace ELIZA.Syntax
{
    /// <summary>
    /// Лексический знак. Признак отрицания.
    /// </summary>
    [Serializable]
    public enum LexicalSign
    {
        /// <summary>
        /// Без знака. Подразумевается +.
        /// </summary>
        NoSign,
        /// <summary>
        /// Лексическое отрицание. Подразумевается -.
        /// </summary>
        Negative,
        /// <summary>
        /// Любой из двух знаков.
        /// </summary>
        Both
    }
}
