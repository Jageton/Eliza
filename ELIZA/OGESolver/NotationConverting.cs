using System.Text;

namespace OGESolver
{
    public class NotationConverting: AbstractAlgorithm<string>
    {
        protected string number;
        protected int from;
        protected int to;

        public NotationConverting(string number, int from, int to)
        {
            this.number = number;
            this.from = from;
            this.to = to;
            this.name = "CONVERT_NOTATION";
        }
        public override string Execute()
        {
            sb = new StringBuilder();
            int inTen = 0;
            if (@from == 10)
            {
                inTen = int.Parse(number);
                IAlgorithm<string> fromTen = new ConvertingFromTen(inTen, to);
                var result = fromTen.Execute();
                sb.Append(fromTen.GetIllustration());
                return result;
            }
            else if (to == 10)
            {
                IAlgorithm<ReferenceOf<int>> toTen = new ConvertingToTen(number, from);
                inTen = toTen.Execute();
                sb.Append(toTen.GetIllustration());
                return inTen.ToString();
            }
            else
            {
                sb.AppendLine("Чтобы перевести число N из системы с основанием P в систему с основанием Q, " +
                              "проще всего сначала перевести его из P в десятичную систему, а потом - из десятичной в Q.");
                IAlgorithm<ReferenceOf<int>> toTen = new ConvertingToTen(number, from);
                inTen = toTen.Execute();
                sb.AppendLine(toTen.GetIllustration());
                IAlgorithm<string> fromTen = new ConvertingFromTen(inTen, to);
                var result = fromTen.Execute();
                sb.AppendLine(fromTen.GetIllustration());
                return result;
            }
        }
    }
}
