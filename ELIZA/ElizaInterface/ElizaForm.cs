using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using ELIZA;
using ELIZA.Morphology;
using System.IO;
using ELIZA.Semantics;
using ELIZA.Syntax;

namespace ElizaInterface
{
    public partial class ElizaForm : Form
    {
        protected Eliza eliza;

        public ElizaForm()
        {
            InitializeComponent();
            MorphologyModel model;
            using (FileStream fs = File.Open("morphology/morhp.mdl", FileMode.Open))
            {
                BinaryFormatter bf = new BinaryFormatter();
                model = (MorphologyModel)bf.Deserialize(fs);
            }
            eliza = new Eliza(model, new SyntaxModel(), new FrameSemanticsModel(GetFrames()));
            Synonims.InitializeFromFile("Resources/All_Synonims.txt");
            resultsMenu.Checked = true;
            ActiveControl = menu;
        }

        private List<TaskFrame> GetFrames()
        {
            var result = new List<TaskFrame>();
            var bf = new BinaryFormatter();
            foreach (var file in Directory.GetFiles("semantics/"))
            {
                try
                {

                    using (FileStream fs = File.Open(file, FileMode.Open))
                    {
                        var frame = (TaskFrame) bf.Deserialize(fs);
                        result.Add(frame);
                    }
                }
                catch (Exception)
                {

                }
                
            }
            return result;
        }

        private void PrintResultsCheckedChanged(object sender, EventArgs e)
        {
            if (!resultsMenu.Checked)
            {
                morphological.CheckedChanged -= MorphologicalCheckedChanged;
                morphological.Checked = false;
                morphological.CheckedChanged += MorphologicalCheckedChanged;
                syntax.CheckedChanged -= MorphologicalCheckedChanged;
                syntax.Checked = false;
                syntax.CheckedChanged += MorphologicalCheckedChanged;
                semantics.CheckedChanged -= MorphologicalCheckedChanged;
                semantics.Checked = false;
                semantics.CheckedChanged += MorphologicalCheckedChanged;
            }
            else
            {
                morphological.CheckedChanged -= MorphologicalCheckedChanged;
                morphological.Checked = true;
                morphological.CheckedChanged += MorphologicalCheckedChanged;
                syntax.CheckedChanged -= MorphologicalCheckedChanged;
                syntax.Checked = true;
                syntax.CheckedChanged += MorphologicalCheckedChanged;
                semantics.CheckedChanged -= MorphologicalCheckedChanged;
                semantics.Checked = true;
                semantics.CheckedChanged += MorphologicalCheckedChanged;
            }
        }

        private void MorphologicalCheckedChanged(object sender, EventArgs e)
        {
            if (morphological.Checked)
                CheckIfUnchecked();
            else 
                UncheckIfAllUnchecked();
        }

        protected void CheckIfUnchecked()
        {
            if (!resultsMenu.Checked)
            {
                resultsMenu.CheckedChanged -= PrintResultsCheckedChanged;
                resultsMenu.Checked = true;
                resultsMenu.CheckedChanged += PrintResultsCheckedChanged;
            }
        }

        protected void UncheckIfAllUnchecked()
        {
            if (!morphological.Checked && !syntax.Checked && !semantics.Checked &&
                resultsMenu.Checked)
            {
                resultsMenu.CheckedChanged -= PrintResultsCheckedChanged;
                resultsMenu.Checked = false;
                resultsMenu.CheckedChanged += PrintResultsCheckedChanged;
            }
        }

        private void SyntaxCheckedChanged(object sender, EventArgs e)
        {
            if (syntax.Checked)
                CheckIfUnchecked();
            else
                UncheckIfAllUnchecked();
        }

        private void SemanticsCheckedChanged(object sender, EventArgs e)
        {

            if (semantics.Checked)
                CheckIfUnchecked();
            else
                UncheckIfAllUnchecked();
        }

        private void GetAnswer(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                e.Handled = true;
                eliza.AppendMorphologicalResults = morphological.Checked;
                eliza.AppendSyntaxResults = syntax.Checked;
                eliza.AppendSemanticsResults = syntax.Checked;
                textBoxResult.AppendText(string.Format(Environment.NewLine + "Вы: {0}", textBoxInput.Text));
                textBoxResult.AppendText(Environment.NewLine);
                var result = eliza.GetResponse(textBoxInput.Text);
                textBoxResult.AppendText(string.Format(Environment.NewLine + "\n\rEliza: {0}", result));
                textBoxInput.Select(0, 0);
                this.textBoxInput.Text = string.Empty;
            }
        }

        private void ShowHelp(object sender, EventArgs e)
        {
            Help.ShowHelp(this, "help.chm");
        }
    }
}
