using System;
using System.Text;

namespace OGESolver
{
    public class ConvertingFromTen: AbstractAlgorithm<string>
    {
        protected int number;
        protected int toNot;

        public ConvertingFromTen(int number, int toNot)
        {
            this.name = "CONVERT_FROM_TEN";
            this.number = number;
            this.toNot = toNot;
        }

        public override string Execute()
        {
            sb = new StringBuilder();
            string result = string.Empty;
            sb.AppendLine("Чтобы перевести число N из десятичной системы в систему с основанием Q, " +
                "нужно последовательно выполнять две операции:");
            sb.AppendLine("1) Получить остаток от деления N на Q. Этот остаток даёт последнюю цифру числа N в системе Q.");
            sb.AppendLine("2) Получить результат деления нацело (K) числа N на Q и положить N = K на следующем шаге.");
            sb.AppendLine("Таким образом, число N можно представить в виде N = K * Q + C, где " + 
                "K - результат деления (число N без последней цифры), С - последняя цифра числа N в системе " +
                "c основанием Q.");
            sb.AppendLine("Этот процесс продолжается для тех пор, пока новое значение N не станет равным 0.");
            sb.Append(Environment.NewLine);
            while(number > 0)
            {
                sb.Append(string.Format("Делим {0} на {1}: ", number, toNot));
                int rem = number % toNot;
                number /= toNot;
                sb.Append(string.Format("результат = {0}, остаток = {1}. ", number, rem));
                char nChar = rem.GetCharValue();
                sb.Append(string.Format("В системе с основанием {0} последняя цифра будет равна {1}. ",
                    toNot, nChar));
                result = nChar + result;
                sb.AppendLine(string.Format("Дописываем получившийся символ в начало результата: {0}.", result));
                sb.Append(Environment.NewLine);
            }
            sb.AppendLine(string.Format("Т.к. результат деления равен 0, то алгоритм закончен. Результат = {0}.", result));
            return result;
        }
    }
}
