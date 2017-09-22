using System;
using System.Linq;
using System.Windows.Forms;
using OGESolver;

namespace Expert
{
    public partial class ConvertingForm : Form
    {
        public ConvertingForm()
        {
            InitializeComponent();
        }

        private void buttonSolve_Click(object sender, EventArgs e)
        {
            string number = textBoxNumber.Text;
            int from = (int)P.Value;
            string alphabet = string.Empty;
            for (int i = 0; i < from; i++)
            {
                if (i < 10)
                    alphabet += i.ToString();
                else
                    alphabet += (char)('A' + (i - 10));
            }
            bool reallyInNotation = number.All(c => alphabet.Contains(c));
            if (reallyInNotation)
            {
                NotationConverting task = new NotationConverting(textBoxNumber.Text, (int) P.Value,
                    (int) Q.Value);
                task.Execute();
                textBoxResult.Text = task.GetIllustration();
            }
            else
                MessageBox.Show(
                    string.Format(
                        "Число {0} не принадлежит системе с основанием {1}. Измените число или исходную систему.",
                        number, from),
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
