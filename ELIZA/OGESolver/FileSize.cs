using System;
using System.Text;

namespace OGESolver
{
    public class FileSize: AbstractAlgorithm<ReferenceOf<Double>>
    {
        protected double speed;
        protected TimeUnit timeUnit;
        protected double elapsedTime;
        protected InformationUnit speedUnit;
        protected TimeUnit elapsedUnit;
        protected InformationUnit sizeUnit;

        public FileSize(int speed, InformationUnit speedUnit, TimeUnit timeUnit,
            int elapsedTime, TimeUnit elapsedUnit, InformationUnit sizeUnit)
        {
            this.speed = speed;
            this.timeUnit = timeUnit;
            this.elapsedTime = elapsedTime;
            this.speedUnit = speedUnit;
            this.sizeUnit = sizeUnit;
            this.elapsedUnit = elapsedUnit;
        }

        public override ReferenceOf<double> Execute()
        {
            sb = new StringBuilder();
            sb.Append(string.Format("Для начала вычислим размер файла в {0}. ", speedUnit.GetFriendlyName()));
            sb.Append("Для этого умножим скорость передачи на время. ");
            //единицы изменерения время для параметра скорость не совпадают с единицами измерения времени
            double fileSize = 0;
            if (timeUnit != elapsedUnit)
            {
                string l;
                sb.Append("При этом не забудем о разных единицах измерения времени. ");
                sb.AppendLine();
                elapsedTime = elapsedUnit.Convert(elapsedTime, timeUnit, out l);
                elapsedUnit = timeUnit;
                sb.AppendLine(l);
            }
            else sb.AppendLine();
            fileSize = elapsedTime*speed;
            sb.AppendLine(string.Format("Размер файла = {0} ({1}) * {2} ({3}) = {4} ({5})", speed,
                string.Format("{0}/{1}", speedUnit.GetFriendlyName(), timeUnit.GetFriendlyName()), elapsedTime,
                elapsedUnit.GetFriendlyName(), fileSize, speedUnit.GetFriendlyName()));
            if (speedUnit != sizeUnit) //необходимо привести единицы измерения
            {
                sb.AppendLine(string.Format("Теперь переведём полученный размер в {0}.",
                    sizeUnit.GetFriendlyName()));
                string l;
                fileSize = speedUnit.Convert(fileSize, sizeUnit, out l);
                sb.AppendLine(l);
            }
            sb.AppendLine(string.Format("Ответ: {0} {1}", fileSize, sizeUnit.GetFriendlyName()));
            return fileSize;
        }
    }
}
