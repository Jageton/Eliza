namespace OGESolver.Builders
{
    public class ConvertingToTenBuilder : ICommandBuilder<ReferenceOf<int>>
    {

        #region ICommandBuilder<int> Members

        public IAlgorithm<ReferenceOf<int>> Build(object[] args)
        {
            return new ConvertingToTen((string)args[0], (int)args[1]);
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
                    case ("Number"): array[0] = nameAndValue[1]; break;
                    case ("Notation"): array[1] = int.Parse(nameAndValue[1]); break;
                }
            }
            return this.Build(array);
        }

        #endregion
    }
}
