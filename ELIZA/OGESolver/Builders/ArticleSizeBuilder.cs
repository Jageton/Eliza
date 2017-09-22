namespace OGESolver.Builders
{
    public class ArticleSizeBuilder : ICommandBuilder<ReferenceOf<int>>
    {
        #region ICommandBuilder Members

        public IAlgorithm<ReferenceOf<int>> Build(object[] args)
        {
            return new ArticleSize((int)args[0], (int)args[1], (int)args[2], (int)args[3]);
        }

        public IAlgorithm<ReferenceOf<int>> Build(string formattedString)
        {
            string[] args = formattedString.Split(new char[] { ';' });
            object[] array = new object[4];
            for(int i = 0; i < args.Length; i++)
            {
                string[] nameAndValue = args[i].Split(new char[] { '=' });
                string name = nameAndValue[0].Trim();
                switch(name)
                {
                    case ("PageNumber"): array[0] = int.Parse(nameAndValue[1]); break;
                    case ("LineNumber"): array[1] = int.Parse(nameAndValue[1]); break;
                    case ("SymbolNumber"): array[2] = int.Parse(nameAndValue[1]); break;
                    case ("SymbolWeight"): array[3] = int.Parse(nameAndValue[1]); break;
                }
            }
            return this.Build(array);
        }

        #endregion

    }
}
