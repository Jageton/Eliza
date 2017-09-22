using Microsoft.VisualStudio.TestTools.UnitTesting;
using OGESolver;
using PrLanguages.Expressions;

namespace Tests.OGESolver
{
    [TestClass]
    public class SolverTests
    {
        [TestMethod]
        public void TestSolveLe()
        {
            ExpressionHelper eh = new ExpressionHelper();
            Expression left = eh.CreateExpression("(x1|x2)&(x1&x2->x3)&!(x1&y1)");
            SmartLESSolver smartLesSolverSolver = new SmartLESSolver(left, true);
            var solutions = smartLesSolverSolver.Execute();
            Assert.IsTrue(solutions.Count == 7);
            string expl = smartLesSolverSolver.GetIllustration();
        }
    }
}
