namespace OGESolver.Builders
{
    public class NotationConvertingBuilder: ICommandBuilder<string>
    {
        #region ICommandBuilder<string> Members

        public IAlgorithm<string> Build(object[] args)
        {
            return new NotationConverting((string)args[0], (ReferenceOf<int>)args[1],
                (ReferenceOf<int>)args[2]);
        }

        public IAlgorithm<string> Build(string formattedString)
        {
            string[] args = formattedString.Split(new char[] { ';' });
            object[] array = new object[3];
            for (int i = 0; i < args.Length; i++)
            {
                string[] nameAndValue = args[i].Split(new char[] { '=' });
                string name = nameAndValue[0].Trim();
                switch (name)
                {
                    case ("Number"): array[0] = (nameAndValue[1]); break;
                    case ("FromNotation"): array[1] = int.Parse(nameAndValue[1]); break;
                    case ("ToNotation"): array[2] = int.Parse(nameAndValue[1]); break;
                }
            }
            return this.Build(array);
        }

        #endregion
    }
}
