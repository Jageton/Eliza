namespace OGESolver.Builders
{
    public class ConvertingFromTenBuilder: ICommandBuilder<string>
    {
        #region ICommandBuilder<string> Members

        public IAlgorithm<string> Build(object[] args)
        {
            return new ConvertingFromTen((int)args[0], (int)args[1]);
        }

        public IAlgorithm<string> Build(string formattedString)
        {
            string[] args = formattedString.Split(new char[] { ';' });
            object[] array = new object[2];
            for (int i = 0; i < args.Length; i++)
            {
                string[] nameAndValue = args[i].Split(new char[] { '=' });
                string name = nameAndValue[0].Trim();
                switch (name)
                {
                    case ("Number"): array[0] = int.Parse(nameAndValue[1]); break;
                    case ("Notation"): array[1] = int.Parse(nameAndValue[1]); break;
                }
            }
            return this.Build(array);
        }

        #endregion
    }
}
