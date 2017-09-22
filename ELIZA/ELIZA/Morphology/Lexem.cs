using System;

namespace ELIZA.Morphology
{
    [Serializable]
    public class Lexem: WordForm
    {
        protected int pos;

        public int LexemPosition
        {
            get { return pos; }
        }

        public Lexem(int pos = 0, string word = "", Tag tag = Tag.NoWord): base(word, tag)
        {
            this.pos = pos;
        }
    }
}
