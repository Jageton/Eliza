using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrLanguages.Expressions;

namespace OGESolver
{
    public class SmartLESSolver: AbstractAlgorithm<List<Dictionary<string, bool>>>
    {
        protected Expression left;
        protected bool right;
        protected ExpressionHelper eh = new ExpressionHelper();
        protected List<string> variables;
        protected bool[,] truthTable;
        protected long tableSize;
        protected LESolvingMethod method;

        public SmartLESSolver(Expression left, bool right)
        {
            this.left = left;
            this.right = right;
            name = "Solve_Logical_Equation";
            variables = left.VariableNames.ToList();
            tableSize = (long)Math.Pow(2, variables.Count);
            truthTable = new bool[tableSize, variables.Count + 1];
            FillTable();
        }

        public override List<Dictionary<string, bool>> Execute()
        {
            sb = new StringBuilder();
            var result = new List<Dictionary<string, bool>>();
            if (left.VariableNames.Count > 4)
            {
                sb.AppendLine(string.Format("Количество переменных равно {0}."+
                    " Таблица истинности будет слишком велика. Решим уравнение методом декомпозиции.",
                    left.VariableNames.Count));
                result = SolveSmart();
            }
            else
            {
                sb.AppendLine(string.Format("Количество переменных достаточно мало ({0}). Можно построить таблицу истинности.",
                    left.VariableNames.Count));
                result = SolveTruthTable();
            }
            if (result.Count == 0)
                sb.AppendLine("Уравнение не имеет решений.");
            else
                sb.AppendLine(string.Format("Таким образом уравнение имеет {0} решений.", result.Count));
            return result;
        }

        private List<Dictionary<string, bool>> SolveTruthTable()
        {
            left.Simplify();
            var result = new List<Dictionary<string, bool>>();
            var variables = left.VariableNames;
            sb.AppendLine();
            foreach (var variable in variables)
                sb.Append((variable + "  "));
            sb.Append("Выражение  ");
            if (right)
            {
                for (int i = 0; i < tableSize; i++)
                    truthTable[i, variables.Count] = true;
                result = RestoreSolution(true);
                sb.AppendLine();
                sb.AppendLine("Нас интересуют строки, графа \"Выражение\" которых равна 1.");
            }
            else
            {
                for (int i = 0; i < tableSize; i++)
                    truthTable[i, variables.Count] = false;
                result = RestoreSolution(false);
                sb.AppendLine();
                sb.AppendLine("Нас интересуют строки, графа \"Выражение\" которых равна 0.");
            }
            return result;
        }
        private List<Dictionary<string, bool>> SolveSmart()
        {
            sb.AppendLine(string.Format("Рассмотрим более мелкие выражения, входящие в состав выражения {0}.", left));
            left.Simplify();
            var result = new List<Dictionary<string, bool>>();
            if (right)
            {
                return AnalyzeTrue(left);
            }
            else
            {
                return AnalyzeFalse(left);
            }
        }
        private void FillTable()
        {
            for (int i = 0; i < tableSize; i++)
            {
                for (int j = 0; j < variables.Count; j++)
                {
                    truthTable[i, j] = (i & (1 << (variables.Count - j - 1))) > 0;
                }
            }
        }
        private List<Dictionary<string, bool>> RestoreSolution(bool v)
        {
            sb.Append("\n\r");
            List<Dictionary<string, bool>> result = new List<Dictionary<string, bool>>();
            for (int i = 0; i < tableSize; i++)
            {
                sb.Append("\n\r");
                if (truthTable[i, variables.Count] == v) // найдено искомое значение
                {
                    Dictionary<string, bool> current = new Dictionary<string, bool>();
                    for (int j = 0; j < variables.Count; j++)
                    {
                        string c = truthTable[i, j] ? "1  " : "0  ";
                        c = c.PadRight(variables[j].Length + 3);
                        sb.Append(c);
                        current[variables[j]] = truthTable[i, j];
                        left.SetVariable(variables[j], truthTable[i, j]);
                    }
                    if (left.Calculate().Equals(v))
                    {
                        sb.Append("1");
                        result.Add(current);
                    }
                    else sb.Append("0");
                }
                sb.AppendLine();
            }
            return result;
        }

        /// <summary>
        /// Проверяет все возможные варианты, когда выражение истинно..
        /// </summary>
        /// <param name="expression">Выражение.</param>
        private List<Dictionary<string, bool>> AnalyzeTrue(Expression expression)
        {            
            if (expression is BinaryOperator)
            {
                var bo = expression as BinaryOperator;
                if (bo.Sign == "&")
                {
                    //sb.AppendLine(string.Format("Конъюнкция {0} истинна, если истинны все конъюнкты. Проверим условия истиннности конъюнктов.", bo));
                    var leftSolutions = AnalyzeTrue(bo.Left);
                    var rightSolutions = AnalyzeTrue(bo.Right);
                    var result = MergeConjSolutions(leftSolutions, rightSolutions);
                    sb.AppendLine(string.Format("Конъюнкция {0} истинна при следующих наборах: ", bo));
                    PrintSolutions(result);
                    return result;
                }
                else if (bo.Sign == "|")
                {
                    //sb.AppendLine(string.Format("Дизъюнкция {0} истинна, если истиннен хотя бы 1 дизъюнкт. Проверим условия стиннности дизъюнктов.", bo));
                    var leftSolutions = AnalyzeTrue(bo.Left);
                    var rightSolutions = AnalyzeTrue(bo.Right);
                    var result = MergeDisjSolutions(leftSolutions, rightSolutions);
                    sb.AppendLine(string.Format("Дизъюнкция {0} истинна при следующих наборах: ", bo));
                    PrintSolutions(result);
                    return result;
                }
                else
                {
                    var l = bo.Left;
                    var r = bo.Right;
                    var eh = new ExpressionHelper();
                    var not = (UnaryOperator)eh.UnaryOperators["!"].Clone();
                    var or = (BinaryOperator)eh.BinaryOperators["|"].Clone();
                    not.Left = l;
                    or.Left = not;
                    or.Right = r;
                    sb.AppendLine(string.Format("Заменим импликацию {0} на дизъюнкцию {1}.", bo, or));
                    var result = AnalyzeTrue(or);
                    sb.AppendLine("Таким образом, импликация истинна при таких же наборах.");
                    return result;
                }
            }
            else if (expression is UnaryOperator)
            {
                var uo = expression as UnaryOperator;
                sb.AppendLine(string.Format("Выражение {0} должно быть ложно.", uo.Left));
                return AnalyzeFalse(uo.Left);
            }
            else
            {
                //sb.AppendLine(string.Format("Переменная {0} должна быть равной 1.", expression));
                var variable = expression as Variable;
                var result = new List<Dictionary<string, bool>>();
                result.Add(new Dictionary<string, bool>());
                result[0].Add(variable.ToString(), true);
                return result;
            }
        }

        private List<Dictionary<string, bool>> MergeConjSolutions(List<Dictionary<string, bool>> left,
            List<Dictionary<string, bool>> right)
        {
            var result = new List<Dictionary<string, bool>>();
            foreach (var sol1 in left)
            {
                foreach (var sol2 in right)
                {
                    if(!Conflicts(sol1, sol2))
                        result.Add(Merge(sol1, sol2));
                }
            }
            return result;
        }
        private List<Dictionary<string, bool>> MergeDisjSolutions(List<Dictionary<string, bool>> left,
            List<Dictionary<string, bool>> right)
        {
            var result = new List<Dictionary<string, bool>>();
            var all = left.Union(right).ToList();
            //найдём имена всех использованных переменных
            var variablesUsed = new List<string>();
            foreach (var solution in all)
            {
                foreach(var key in solution.Keys)
                    if(!variablesUsed.Contains(key))
                        variablesUsed.Add(key);
            }
            foreach (var solution in all)
            {
                var solutionExtended = ExtendSolution(solution, variablesUsed);
                foreach (var sol in solutionExtended)
                {
                    bool alreadyIn = false;
                    foreach (var sol2 in result)
                    {
                        if (sol.Count == sol2.Count && !sol.Except(sol2).Any())
                        {
                            alreadyIn = true;
                            break;
                        }
                    }
                    if(!alreadyIn)
                        result.Add(sol);
                }
            }
            return result;
        }

        private List<Dictionary<string, bool>> ExtendSolution(Dictionary<string, bool> solution,
            List<string> variables)
        {
            var result = new List<Dictionary<string, bool>>();
            var variablesNotUsed = variables.Except(solution.Keys).ToList(); //находим неиспользованные переменные
            if (variablesNotUsed.Count == 0)
            {
                result.Add(solution);
            }
            else
            {
                result.Add(new Dictionary<string, bool>(solution));
                //для каждой неиспользованной переменной будем увеличивать количество решений на 2
                foreach (var variable in variablesNotUsed)
                {
                    var cloned = new List<Dictionary<string, bool>>();
                    foreach (var sol in result)
                    {
                        var clone = new Dictionary<string, bool>(sol);
                        clone.Add(variable, false);
                        cloned.Add(clone);
                    }
                    foreach (var sol in result)
                        sol.Add(variable, true);
                    result.AddRange(cloned);
                }
            }
            return result;
        }

        private Dictionary<string, bool> Merge(Dictionary<string, bool> left,
            Dictionary<string, bool> right)
        {
            var result = new Dictionary<string, bool>();
            foreach (var key in left.Keys)
                result.Add(key, left[key]);
            foreach (var key in right.Keys)
            {
                if(!result.ContainsKey(key))
                    result.Add(key, right[key]);
            }
            return result;
        }

        private bool Conflicts(Dictionary<string, bool> left,
            Dictionary<string, bool> right)
        {
            foreach (var key in left.Keys)
            {
                if (right.ContainsKey(key) && right[key] != left[key]) return true;
            }
            return false;
        }

        private void PrintSolution(Dictionary<string, bool> solution, bool appendNotUsed = false)
        {            
            if (!appendNotUsed)
            {
                foreach (var key in solution.Keys)
                {
                    sb.Append(key + "=" + (solution[key] ? "1 " : "0 "));
                }
                sb.Append("\n\r");
            }
            sb.AppendLine();
        }

        private void PrintSolutions(List<Dictionary<string, bool>> solutions, bool appendNotUsed = false)
        {
            sb.AppendLine();
            foreach(var solution in solutions)
                PrintSolution(solution, appendNotUsed);
            sb.AppendLine();
        }

        private List<Dictionary<string, bool>> AnalyzeFalse(Expression expression)
        {
            if (expression is BinaryOperator)
            {
                var bo = expression as BinaryOperator;
                if (bo.Sign == "&")
                {
                    //sb.AppendLine(string.Format("Конъюнкция {0} ложна, если ложен хотя бы 1 конъюнкт. Проверим условия ложности конъюнктов.", bo));
                    var leftSolutions = AnalyzeFalse(bo.Left);
                    var rightSolutions = AnalyzeFalse(bo.Right);
                    var result = MergeDisjSolutions(leftSolutions, rightSolutions);
                    sb.AppendLine(string.Format("Конъюнкция {0} ложна при следующих наборах: ", bo));
                    PrintSolutions(result);
                    return result;
                }
                else if (bo.Sign == "|")
                {
                    //sb.AppendLine(string.Format("Дизъюнкция {0} ложна, если ложны все дизъюнкты. Проверим условия ложности дизъюнктов.", bo));
                    var leftSolutions = AnalyzeFalse(bo.Left);
                    var rightSolutions = AnalyzeFalse(bo.Right);
                    var result = MergeConjSolutions(leftSolutions, rightSolutions);
                    sb.AppendLine(string.Format("Дизъюнкция {0} ложна при следующих наборах: ", bo));
                    PrintSolutions(result);
                    return result;
                }
                else
                {
                    var l = bo.Left;
                    var r = bo.Right;
                    var eh = new ExpressionHelper();
                    var not = (UnaryOperator)eh.UnaryOperators["!"].Clone();
                    var or = (BinaryOperator)eh.BinaryOperators["|"].Clone();
                    not.Left = l;
                    or.Left = not;
                    or.Right = r;
                    sb.AppendLine(string.Format("Заменим импликацию {0} на дизъюнкцию {1}.", bo, or));
                    var result = AnalyzeFalse(or);
                    sb.AppendLine("Таким образом, импликация ложна при таких же наборах.");
                    return result;
                }
            }
            else if (expression is UnaryOperator)
            {
                var uo = expression as UnaryOperator;
                sb.AppendLine(string.Format("Выражение {0} должно быть истинно.", uo.Left));
                return AnalyzeTrue(uo.Left);
            }
            else
            {
                //sb.AppendLine(string.Format("Переменная {0} должна быть равной 0.", expression));
                var variable = expression as Variable;
                var result = new List<Dictionary<string, bool>>();
                result.Add(new Dictionary<string, bool>());
                result[0].Add(variable.ToString(), false);
                return result;
            }
        }

        private List<Expression> SplitBinaryOperator(Expression expression, string operatorSign)
        {
            List<Expression> result = new List<Expression>();
            Stack<Expression> expressionStack = new Stack<Expression>();
            expressionStack.Push(expression);
            while (expressionStack.Count > 0)
            {
                Expression currentExpression = expressionStack.Pop();
                if (currentExpression is BinaryOperator)
                {
                    BinaryOperator bo = currentExpression as BinaryOperator;
                    if (bo.Sign.Equals(operatorSign))
                    {
                        expressionStack.Push(bo.Left);
                        expressionStack.Push(bo.Right);
                    }
                }
                else result.Add(currentExpression);
            }
            return result;
        }
    }
}
