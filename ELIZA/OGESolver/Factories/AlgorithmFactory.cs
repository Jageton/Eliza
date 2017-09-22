using System.Collections.Generic;
using OGESolver.Builders;

namespace OGESolver.Factories
{
    public static class AlgorithmFactory
    {
        private static Dictionary<string, ICommandBuilder<object>>builders;
        private static IAlgorithm<object> builded;
        static AlgorithmFactory()
        {
            builded = null;
            builders = new Dictionary<string, ICommandBuilder<object>>();
            builders.Add("Размер статьи", new ArticleSizeBuilder());
            builders.Add("Кодирование",  new CodificationBuilder());
            builders.Add("Перевести из десятичной", new ConvertingFromTenBuilder());
            builders.Add("Перевести в десятичную", new ConvertingToTenBuilder());
            builders.Add("Найти количество совпадений в записи", new GetAmountOfBuilder());
            builders.Add("NotationConverting", new NotationConvertingBuilder());
            builders.Add("SolveLES", new LESSolvingBuilder());
            builders.Add("FileSize", new FileSizeBuilder());
            builders.Add("FileSum", new FileSumBuilder());
        }

        public static List<string> GetFunctionNames()
        {
            List<string> names = new List<string>();
            foreach(var pair in builders)
            {
                names.Add(pair.Key);
            }
            return names;
        }
        public static object Execute(string name, string formattedString)
        {
            builded = builders[name].Build(formattedString);
            return builded.Execute();
        }
        public static string GetDescription()
        {
            return builded.GetIllustration();
        }

        public static object Execute(string name, object[] paramObjects)
        {
            builded = builders[name].Build(paramObjects);
            return builded.Execute();
        }
    }
}
