using System.Collections.Generic;
using System.Text;
using PrLanguages.Expressions;

namespace OGESolver
{
    public class SolveLeSystem: AbstractAlgorithm<List<Dictionary<string, bool>>>
    {
        protected Expression[] left;
        protected bool[] right;
        protected ExpressionHelper eh;

        public SolveLeSystem(Expression[] left, bool[] right)
        {
            this.left = left;
            this.right = right;
            eh = new ExpressionHelper();
        }

        public override List<Dictionary<string, bool>> Execute()
        {
            sb = new StringBuilder();
            sb.AppendLine("Преобразуем систему уравнений к одному уравнению.");
            BinaryOperator root = (BinaryOperator)eh.BinaryOperators["&"].Clone(); //уравнение
            BinaryOperator conj = (BinaryOperator)eh.BinaryOperators["&"].Clone(); //текущая кон.
            for (int i = 0; i < left.Length; i++)
            {
                if (!right[i]) //0 в правом столбце, получим 1
                {
                    //для этого применим оператор НЕ
                    UnaryOperator not = (UnaryOperator) eh.UnaryOperators["!"].Clone();
                    not.Left = left[i];
                    left[i] = not;
                }
                if (i == left.Length - 1) //последний элемент просто дописывается в конъюнкцию
                    conj.Right = left[i];
                else
                {
                    BinaryOperator conjRight = (BinaryOperator)eh.BinaryOperators["&"].Clone();
                    conj.Left = left[i];
                    conj.Right = conjRight;
                    conj = conjRight;
                }
            }
            //теперь решим одно уравнение
            SmartLESSolver solver = new SmartLESSolver(root, true);
            var result = solver.Execute();
            sb.AppendLine(solver.GetIllustration());
            return result;
        }
    }
}
