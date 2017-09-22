using System;

namespace OGESolver.Builders
{
    public class FileSumBuilder: ICommandBuilder<ReferenceOf<Double>>
    {
        public IAlgorithm<ReferenceOf<double>> Build(object[] args)
        {
            return new FileSum((ReferenceOf<int>) args[0], (ReferenceOf<InformationUnit>) args[1],
                (ReferenceOf<int>) args[2], (ReferenceOf<InformationUnit>) args[3],
                (ReferenceOf<InformationUnit>) args[4]);
        }

        public IAlgorithm<ReferenceOf<double>> Build(string formattedString)
        {
            throw new NotImplementedException();
        }
    }
}
