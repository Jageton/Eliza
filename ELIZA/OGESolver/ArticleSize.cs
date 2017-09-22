using System.Text;

namespace OGESolver
{
    public class ArticleSize : AbstractAlgorithm<ReferenceOf<int>>
    {
        protected int pageNum;
        protected int lineNum;
        protected int symbNum;
        protected int symbWeight;

        public ArticleSize(int pageNum, int lineNum, int symbNum, int symbWeight)
        {
            this.pageNum = pageNum;
            this.lineNum = lineNum;
            this.symbNum = symbNum;
            this.symbWeight = symbWeight;
            this.name = "ARTICLE_SIZE";
        }
        /// <summary>
        /// Возвращает результат работы алгоритма с ранее установленными параметрами.
        /// </summary>
        /// <returns>Возвращает объект типа <see cref="{T}" />, являющийся результатом
        /// работы алгоритма.</returns>
        public override ReferenceOf<int> Execute()
        {
            sb = new StringBuilder();
            int res = symbWeight * symbNum;
            sb.Append(string.Format("Если информационный объём одного символа составляет {0}, то " +
                "информационный объём строки составляет {1} бит, страницы - {2} бит, " +
                "всей статьи - {3} бит.", symbWeight, res, res*=lineNum, res*= pageNum));
            if(res > 8)
            {
                sb.Append(string.Format("Чтобы перевести биты в байты, нужно разделить " +
                    "полученный результат на 8, т.к. в одном байте соедржится 8 бит. " +
                    "Получится {0} байт.", res /= 8));
            }
            if(res > 1024)
            {
                sb.Append(string.Format("Чтобы перевести байты в Кбайты, нужно разделить " +
                    "полученный результат на 1024, т.к. в одном Кбайте соедржится 1024 байт. " +
                    "Получится {0} Кбайт.", res /= 1024));
            }
            if (res > 1024)
            {
                sb.Append(string.Format("Чтобы перевести Кбайты в Гбайты, нужно разделить " +
                    "полученный результат на 1024, т.к. в одном Гбайте соедржится 1024 Кбайт. " +
                    "Получится {0} Гбайт.", res /= 1024));
            }
            sb.Append(string.Format("Чтобы упростить вычисления, можно переводить в более крупные единцы " +
                "на этапе умножения."));
            return res;
        }
    }
}
