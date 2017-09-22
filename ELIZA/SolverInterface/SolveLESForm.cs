using System;
using System.Windows.Forms;
using OGESolver;
using PrLanguages.Expressions;

namespace Expert
{
    public partial class SolveLESForm : Form
    {
        public SolveLESForm()
        {
            InitializeComponent();
        }

        private void buttonFind_Click(object sender, EventArgs e)
        {
            string equation = textBoxEquation.Text;
            equation = equation.Replace(" ", "");
            var splitted = equation.Split(new char[] {'='});
            var formula = splitted[0];
            var right = splitted[1] != "0";
            var eh = new ExpressionHelper();
            var left = eh.CreateExpression(formula);
            var solver = new SmartLESSolver(left, right);
            solver.Execute();
            textBoxResult.Text = solver.GetIllustration();
        }
    }
}
