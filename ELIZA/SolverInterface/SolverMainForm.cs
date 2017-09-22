using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Expert;

namespace SolverInterface
{
    public partial class SolverMainForm : Form
    {
        protected Dictionary<string, Type> forms;  

        public SolverMainForm()
        {
            InitializeComponent();
            forms = new Dictionary<string, Type>();
            forms.Add("Системы счисления", typeof(ConvertingForm));
            forms.Add("Логичекие уравнения", typeof(SolveLESForm));
            foreach (var key in forms.Keys)
            {
                comboBoxTask.Items.Add(key);
            }
            comboBoxTask.SelectedIndex = 0;
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            if (comboBoxTask.Text != "")
            {
                var form = (Form) Activator.CreateInstance(forms[comboBoxTask.Text]);
                form.Closed += (o, args) => Show();
                Hide();
                form.ShowDialog();
            }
        }
    }
}
