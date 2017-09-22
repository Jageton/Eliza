namespace OGESolver.Builders
{
    public class GetAmountOfBuilder : ICommandBuilder<ReferenceOf<int>>
    {
        #region ICommandBuilder<int> Members

        public IAlgorithm<ReferenceOf<int>> Build(object[] args)
        {
            return new GetAmountOf((string)args[0], (string)args[1]);
        }

        public IAlgorithm<ReferenceOf<int>> Build(string formattedString)
        {
            string[] args = formattedString.Split(new char[] { ';' });
            object[] array = new object[2];
            for (int i = 0; i < args.Length; i++)
            {
                string[] nameAndValue = args[i].Split(new char[] { '=' });
                string name = nameAndValue[0].Trim();
                switch (name)
                {
                    case ("Line"): array[0] = nameAndValue[1]; break;
                    case ("Pattern"): array[1] = nameAndValue[1]; break;
                }
            }
            return this.Build(array);
        }

        #endregion
    }
}
