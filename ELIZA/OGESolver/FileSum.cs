using System;
using System.Text;

namespace OGESolver
{
    public class FileSum: AbstractAlgorithm<ReferenceOf<Double>>
    {
        protected double size1;
        protected InformationUnit unit1;
        protected double size2;
        protected InformationUnit unit2;
        protected InformationUnit resUnit;

        public FileSum(double size1, InformationUnit unit1, double size2, InformationUnit unit2,
            InformationUnit resUnit)
        {
            this.size1 = size1;
            this.unit1 = unit1;
            this.size2 = size2;
            this.unit2 = unit2;
            this.resUnit = resUnit;
        }

        public override ReferenceOf<double> Execute()
        {
            sb = new StringBuilder();
            if (unit1 != resUnit)
            {
                sb.AppendLine(string.Format("Переведём размер первого файла в {0}.",
                    resUnit.GetFriendlyName()));
                string l;
                size1 = unit1.Convert(size1, resUnit, out l);
                sb.AppendLine(l);
            }
            if (unit2 != resUnit)
            {
                sb.AppendLine(string.Format("Переведём размер второго файла в {0}.",
                    resUnit.GetFriendlyName()));
                string l;
                size2 = unit1.Convert(size2, resUnit, out l);
                sb.AppendLine(l);
            }
            var result = size1 + size2;
            sb.AppendLine("Размеры обоих файлов в одних и тех же единицах. Можно складывать.");
            sb.AppendLine(string.Format("{0} ({3}) + {1} ({3}) = {2} ({3})", size1, size2,
                result, resUnit.GetFriendlyName()));
            return result;
        }
    }
}
