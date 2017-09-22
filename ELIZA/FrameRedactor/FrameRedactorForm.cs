using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;
using ELIZA.Semantics;
using ELIZA.Semantics.Converters;
using ELIZA.Semantics.Patterns;

namespace FrameRedactor
{
    public partial class FrameRedactorForm : Form
    {
        protected TaskFrame frame;
        protected int slotsStart;
        protected List<Type> list;
        protected List<string> displayed; 

        public FrameRedactorForm()
        {
            InitializeComponent();
            list = ConverterFactory.Converters.Keys.ToList();
            list.Add(typeof(string));
            displayed = list.Select(TypeToString).ToList();
            ((DataGridViewComboBoxColumn) slotsGrid.Columns[1]).DataSource = displayed;
            //((DataGridViewComboBoxColumn) slotsGrid.Columns[1]).DisplayMember = "Name";
            //((DataGridViewComboBoxColumn)slotsGrid.Columns[1]).ValueMember = "Self";
            //((DataGridViewComboBoxColumn) slotsGrid.Columns[1]).DefaultCellStyle.NullValue = "String";
        }

        private void buttonRemoveLast_Click(object sender, EventArgs e)
        {
            if (slotsGrid.RowCount > 1)
            {
                slotsGrid.Rows.RemoveAt(slotsGrid.RowCount - 1);
                if(frame.Slots.Count > slotsStart)
                    frame.Slots.RemoveAt(frame.Slots.Count - 1);
            }
        }

        private void buttonNewSlot_Click(object sender, EventArgs e)
        {
            var row = (DataGridViewRow) slotsGrid.Rows[0].Clone();
            row.Cells[0].Value = "Имя слота";
            row.Cells[2].Value = "";

            slotsGrid.Rows.Add(row);
        }

        private void buttonLoadFrame_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                buttonNewFrame_Click(this, null);
                var fileName = dialog.FileName;
                using (FileStream fs = File.Open(fileName, FileMode.Open))
                {
                    var bf = new BinaryFormatter();
                    frame = (TaskFrame) bf.Deserialize(fs);
                }
                var patterns = (List<ComplexPattern>) frame["Patterns"].GetValue(frame);
                var v = patterns.First().ToString();
                for (var i = 1; i < patterns.Count; i++)
                    v += ";" + patterns[i];
                slotsGrid.Rows[0].Cells[2].Value = v;
                for (var i = slotsStart; i < frame.Slots.Count; i++)
                {
                    var slot = frame.Slots[i];
                    var type = slot.GetValue(frame).GetType();
                    slotsGrid.Rows.Add(slot.Name, TypeToString(type), slot.GetValue(frame));
                }
                textBoxFrameName.Text = frame.Name;
            }
        }

        private void buttonNewFrame_Click(object sender, EventArgs e)
        {
            slotsGrid.Rows.Clear();
            slotsGrid.Rows.Add("Patterns");
            frame = new TaskFrame();
            slotsStart = frame.Slots.Count;
            textBoxFrameName.Text = "Имя фрейма";
        }

        private void buttonSaveFrame_Click(object sender, EventArgs e)
        {
            var dialog = new SaveFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var fileName = dialog.FileName;
                var patternStrings = slotsGrid.Rows[0].Cells[2].Value.ToString()
                    .Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                var patternList = new List<ComplexPattern>();
                foreach (var pattern in patternStrings)
                {
                    try
                    {
                        patternList.Add(Pattern.CreateAny(pattern));
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Шаблон содержит ошибку.");
                    }
                }
                frame["Patterns"].SetValue(patternList, frame);
                frame.Name = textBoxFrameName.Text;
                var method = typeof(ConverterFactory).GetMethod("Convert");
                var slotType = typeof(SlotWithConvertibleDefaultValue<>);
                for (var i = 1; i < slotsGrid.RowCount; i++)
                {
                    var name = slotsGrid.Rows[i].Cells[0].Value.ToString();
                    var type = list[displayed.IndexOf(slotsGrid.Rows[i].Cells[1].Value.ToString())];
                    var defaultValue = slotsGrid.Rows[i].Cells[2].Value.ToString();
                    var generic = method.MakeGenericMethod(type);
                    var defaultV = generic.Invoke(null, new object[] { defaultValue });
                    var slot =
                        (AbstractSlot)
                            Activator.CreateInstance(slotType.MakeGenericType(new Type[] { type }),
                                new object[] { defaultV, name });
                    slot.RemoveValue(frame);
                    if (frame[slot.Name] == null)
                        frame.Slots.Add(slot);
                    else frame[slot.Name] = slot;
                }
                using (FileStream fs = File.Create(fileName))
                {
                    var bf = new BinaryFormatter();
                    bf.Serialize(fs, frame);
                }
            }
        }

        private string TypeToString(Type type)
        {
            var sb = new StringBuilder();
            if (type.IsGenericType)
            {
                var parentType = type.Name.Split('`');
                var args = type.GetGenericArguments();
                var argsList = new StringBuilder();
                foreach (var t in args)
                {
                    var arg = TypeToString(t);
                    if (argsList.Length > 0)
                        argsList.AppendFormat(", {0}", arg);
                    else argsList.Append(arg);
                }
                if (argsList.Length > 0)
                    sb.AppendFormat("{0}<{1}>", parentType[0], argsList.ToString());
                return sb.ToString();
            }
            return type.ToString();
        }
    }
}
