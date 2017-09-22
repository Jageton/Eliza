using System;
using ELIZA.Morphology;

namespace ELIZA.Syntax
{
    [Serializable]
    public class DForm
    {
        public Lexem Lexem { get; set; }
        public LexicalSign Sign { get; set; }
        /// <summary>
        /// Получает или задаёт другую форму, которая синонимична данной форме.
        /// </summary>
        public DForm CoReference { get; set; }
        /// <summary>
        /// Получает или задаёт форму, которая является названием данной.
        /// </summary>
        public DForm Label { get; set; }

        public DForm(Lexem lexem = null, LexicalSign sign = LexicalSign.NoSign)
        {
            Lexem = lexem;
            Sign = sign;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(Lexem.Lemma);
        }
    }
}
