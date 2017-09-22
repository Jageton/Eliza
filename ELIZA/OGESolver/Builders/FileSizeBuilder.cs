using System;

namespace OGESolver.Builders
{
    public class FileSizeBuilder: ICommandBuilder<ReferenceOf<Double>>
    {

        public IAlgorithm<ReferenceOf<double>> Build(object[] args)
        {
            return new FileSize((ReferenceOf<int>)args[0],(ReferenceOf<InformationUnit>)args[1],
                (ReferenceOf<TimeUnit>)args[2], (ReferenceOf<int>)args[3],
                (ReferenceOf<TimeUnit>)args[4], (ReferenceOf<InformationUnit>)args[5]);
        }

        public IAlgorithm<ReferenceOf<double>> Build(string formattedString)
        {
            throw new NotImplementedException();
        }
    }
}
