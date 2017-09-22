namespace FrameRedactor
{
    partial class FrameRedactorForm
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonLoadFrame = new System.Windows.Forms.Button();
            this.buttonRemoveLast = new System.Windows.Forms.Button();
            this.buttonSaveFrame = new System.Windows.Forms.Button();
            this.buttonNewSlot = new System.Windows.Forms.Button();
            this.buttonNewFrame = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.slotsGrid = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxFrameName = new System.Windows.Forms.TextBox();
            this.SlotName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Type = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.DefaultValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.slotsGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(545, 277);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonLoadFrame);
            this.groupBox1.Controls.Add(this.buttonRemoveLast);
            this.groupBox1.Controls.Add(this.buttonSaveFrame);
            this.groupBox1.Controls.Add(this.buttonNewSlot);
            this.groupBox1.Controls.Add(this.buttonNewFrame);
            this.groupBox1.Location = new System.Drawing.Point(384, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(158, 262);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // buttonLoadFrame
            // 
            this.buttonLoadFrame.Location = new System.Drawing.Point(6, 151);
            this.buttonLoadFrame.Name = "buttonLoadFrame";
            this.buttonLoadFrame.Size = new System.Drawing.Size(143, 27);
            this.buttonLoadFrame.TabIndex = 4;
            this.buttonLoadFrame.Text = "Загрузить фрейм";
            this.buttonLoadFrame.UseVisualStyleBackColor = true;
            this.buttonLoadFrame.Click += new System.EventHandler(this.buttonLoadFrame_Click);
            // 
            // buttonRemoveLast
            // 
            this.buttonRemoveLast.Location = new System.Drawing.Point(6, 85);
            this.buttonRemoveLast.Name = "buttonRemoveLast";
            this.buttonRemoveLast.Size = new System.Drawing.Size(143, 27);
            this.buttonRemoveLast.TabIndex = 3;
            this.buttonRemoveLast.Text = "Удалить последний слот";
            this.buttonRemoveLast.UseVisualStyleBackColor = true;
            this.buttonRemoveLast.Click += new System.EventHandler(this.buttonRemoveLast_Click);
            // 
            // buttonSaveFrame
            // 
            this.buttonSaveFrame.Location = new System.Drawing.Point(6, 118);
            this.buttonSaveFrame.Name = "buttonSaveFrame";
            this.buttonSaveFrame.Size = new System.Drawing.Size(143, 27);
            this.buttonSaveFrame.TabIndex = 2;
            this.buttonSaveFrame.Text = "Сохранить фрейм";
            this.buttonSaveFrame.UseVisualStyleBackColor = true;
            this.buttonSaveFrame.Click += new System.EventHandler(this.buttonSaveFrame_Click);
            // 
            // buttonNewSlot
            // 
            this.buttonNewSlot.Location = new System.Drawing.Point(6, 52);
            this.buttonNewSlot.Name = "buttonNewSlot";
            this.buttonNewSlot.Size = new System.Drawing.Size(143, 27);
            this.buttonNewSlot.TabIndex = 1;
            this.buttonNewSlot.Text = "Новый слот";
            this.buttonNewSlot.UseVisualStyleBackColor = true;
            this.buttonNewSlot.Click += new System.EventHandler(this.buttonNewSlot_Click);
            // 
            // buttonNewFrame
            // 
            this.buttonNewFrame.Location = new System.Drawing.Point(6, 19);
            this.buttonNewFrame.Name = "buttonNewFrame";
            this.buttonNewFrame.Size = new System.Drawing.Size(143, 27);
            this.buttonNewFrame.TabIndex = 0;
            this.buttonNewFrame.Text = "Новый фрейм";
            this.buttonNewFrame.UseVisualStyleBackColor = true;
            this.buttonNewFrame.Click += new System.EventHandler(this.buttonNewFrame_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBoxFrameName);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.slotsGrid);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(375, 271);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            // 
            // slotsGrid
            // 
            this.slotsGrid.AllowUserToAddRows = false;
            this.slotsGrid.AllowUserToDeleteRows = false;
            this.slotsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.slotsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.slotsGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SlotName,
            this.Type,
            this.DefaultValue});
            this.slotsGrid.Location = new System.Drawing.Point(0, 52);
            this.slotsGrid.Name = "slotsGrid";
            this.slotsGrid.Size = new System.Drawing.Size(375, 219);
            this.slotsGrid.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Имя фрейма";
            // 
            // textBoxFrameName
            // 
            this.textBoxFrameName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFrameName.Location = new System.Drawing.Point(88, 16);
            this.textBoxFrameName.Name = "textBoxFrameName";
            this.textBoxFrameName.Size = new System.Drawing.Size(280, 20);
            this.textBoxFrameName.TabIndex = 3;
            // 
            // SlotName
            // 
            this.SlotName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.SlotName.HeaderText = "Имя";
            this.SlotName.MinimumWidth = 150;
            this.SlotName.Name = "SlotName";
            this.SlotName.Width = 150;
            // 
            // Type
            // 
            this.Type.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.Type.HeaderText = "Тип";
            this.Type.MinimumWidth = 150;
            this.Type.Name = "Type";
            this.Type.Width = 150;
            // 
            // DefaultValue
            // 
            this.DefaultValue.HeaderText = "По умолчанию";
            this.DefaultValue.MinimumWidth = 150;
            this.DefaultValue.Name = "DefaultValue";
            this.DefaultValue.Width = 150;
            // 
            // FrameRedactorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(545, 277);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FrameRedactorForm";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.slotsGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonRemoveLast;
        private System.Windows.Forms.Button buttonSaveFrame;
        private System.Windows.Forms.Button buttonNewSlot;
        private System.Windows.Forms.Button buttonNewFrame;
        private System.Windows.Forms.Button buttonLoadFrame;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBoxFrameName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView slotsGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn SlotName;
        private System.Windows.Forms.DataGridViewComboBoxColumn Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn DefaultValue;
    }
}

