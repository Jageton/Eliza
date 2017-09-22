using System.Collections.Generic;
using System.Text;

namespace OGESolver
{
    public class Codification: AbstractAlgorithm<string>
    {
        protected Dictionary<int, char> table;
        protected string alphabet = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
        protected List<int> encryption;

        public Codification(List<int> encryption)
        {
            this.name = "CODIFICATION";
            this.table = new Dictionary<int, char>();
            for(int i = 0; i < alphabet.Length; i++)
            {
                table.Add((i + 1), alphabet[i]);
            }
            this.encryption = encryption;
        }
        public override string Execute()
        {
            sb = new StringBuilder();
            string res = string.Empty;
            sb.AppendLine("Можно решить задачу методом перебора (пройти по каждой шифровке и найти варианты расшифровки):");
            for(int i = 0; i < encryption.Count; i++)
            {
                string copy = encryption[i].ToString();
                res = string.Empty;
                int prevNum = 0;
                int j = 0;
                bool bad = false;
                while (copy.Length > 0 && !bad)
                {
                    int currNum = copy[0].GetNotificationValue();
                    copy = copy.Substring(1);
                    int key = prevNum * 10 + currNum;
                    if (key > 10 && key < 34)
                    {
                        bad = true;
                        sb.AppendLine(string.Format("Шифровка {0}: часть шифровки до {1} цифры (включительно) расшифровывается как {2}, "
                            + "следующие две цифры ({5}) могут расшифровываться как {3} или {4}. " +
                            "Значит, эта шифровка не подходит.", encryption[i], j - 1,
                            res.Substring(0, res.Length - 1), res.Substring(res.Length - 2, 1) + table[currNum],
                            table[key], key));
                    }
                    else
                    {
                        if (key == 10) res = res.Substring(0, res.Length - 1);
                        else key = currNum;
                        res += table[key];
                    }
                    j++;
                    prevNum = currNum;
                }
                if (bad)
                {
                    sb.AppendLine("Переходим к следующей шифровке.");
                    continue;
                }
                else
                {
                    sb.AppendLine(string.Format("Шифровка {0} расшифровывается однозначно: {1}.",
                        encryption[i], res));
                    return res;
                }
            }
            return string.Empty;
        }
    }
}
