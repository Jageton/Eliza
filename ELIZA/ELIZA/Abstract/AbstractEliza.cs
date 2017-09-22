namespace ELIZA.Abstract
{
    public delegate void DisplayFunction(string text);
    public abstract class AbstractEliza
    {
        protected LanguageModel langModel;
        //модель общения

        public AbstractEliza(LanguageModel langModel)
        {
            this.langModel = langModel;
        }
        public abstract string GetResponse(string input);
    }
}
