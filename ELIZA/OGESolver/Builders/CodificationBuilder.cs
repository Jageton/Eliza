using System;
using System.Collections.Generic;
using System.Linq;

namespace OGESolver.Builders
{
    public class CodificationBuilder: ICommandBuilder<string>
    {

        #region ICommandBuilder<string> Members

        public IAlgorithm<string> Build(object[] args)
        {
            List<int> encryption = (List<int>)args[0];
            return new Codification(encryption);
        }

        public IAlgorithm<string> Build(string formattedString)
        {
            string[] args = formattedString.Split(new char[] { ';' });
            object[] array = new object[1];
            for (int i = 0; i < args.Length; i++)
            {
                string[] nameAndValue = args[i].Split(new char[] { '=' });
                string name = nameAndValue[0].Trim();
                switch (name)
                {
                    case ("Codifications"):
                        array[0] = nameAndValue[1].Split(new char[] { ' ', ',' }, 
                            StringSplitOptions.RemoveEmptyEntries).Convert<int>(int.Parse).ToList();
                        break;
                }
            }
            return this.Build(array);
        }

        #endregion
    }
}
