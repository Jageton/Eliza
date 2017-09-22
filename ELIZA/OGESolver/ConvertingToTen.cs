using System;
using System.Text;

namespace OGESolver
{
    public class ConvertingToTen : AbstractAlgorithm<ReferenceOf<int>>
    {
        protected string number;
        protected int fromNot;

        public ConvertingToTen(string number, int fromNot)
        {
            this.name = "CONVERT_TO_TEN";
            this.number = number;
            this.fromNot = fromNot;
        }

        public override ReferenceOf<int> Execute()
        {
            sb = new StringBuilder();
            sb.AppendLine("Чтобы перевести число N из системы P в десятичную, нужно просто вычислить сумму:");
            sb.AppendLine("A[1] * (P ^ (n - 1)) + A[2] * (P ^ (n - 2)) + ... + A[n - 1] * (P ^ 1) + A[n] * (P ^ 0) где A[i] - i-тая цифра числа N, n - общее количество цифр");
            sb.AppendLine(Environment.NewLine);
            sb.Append("В нашем случае это: ");
            int result = 0;
            string res = string.Empty;
            for(int i = 0; i < number.Length; i++)
            {
                res += (String.Format("{0} * ({1} ^ {2}) + ",
                    number[i].GetNotificationValue(), fromNot, number.Length - i - 1));
                result *= fromNot;
                result += number[i].GetNotificationValue();
            }
            sb.Append(res.Substring(0, res.Length - 2) + ".");
            sb.AppendLine(Environment.NewLine);
            sb.AppendLine(string.Format("Результат = {0}.", result));
            return result;
        }
    }
}
