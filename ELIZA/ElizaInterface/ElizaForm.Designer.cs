namespace ElizaInterface
{
    partial class ElizaForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mainTable = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxInput = new ElizaInterface.WatermarkTextbox();
            this.textBoxResult = new ElizaInterface.WatermarkTextbox();
            this.menu = new System.Windows.Forms.MenuStrip();
            this.optionsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.resultsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.morphological = new System.Windows.Forms.ToolStripMenuItem();
            this.syntax = new System.Windows.Forms.ToolStripMenuItem();
            this.semantics = new System.Windows.Forms.ToolStripMenuItem();
            this.справкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainTable.SuspendLayout();
            this.menu.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainTable
            // 
            this.mainTable.ColumnCount = 1;
            this.mainTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainTable.Controls.Add(this.textBoxInput, 0, 0);
            this.mainTable.Controls.Add(this.textBoxResult, 0, 1);
            this.mainTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTable.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.mainTable.Location = new System.Drawing.Point(0, 24);
            this.mainTable.Name = "mainTable";
            this.mainTable.RowCount = 2;
            this.mainTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.mainTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.mainTable.Size = new System.Drawing.Size(670, 276);
            this.mainTable.TabIndex = 0;
            // 
            // textBoxInput
            // 
            this.textBoxInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxInput.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxInput.Location = new System.Drawing.Point(3, 3);
            this.textBoxInput.Multiline = true;
            this.textBoxInput.Name = "textBoxInput";
            this.textBoxInput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxInput.Size = new System.Drawing.Size(664, 76);
            this.textBoxInput.TabIndex = 2;
            this.textBoxInput.WatermarkColor = System.Drawing.Color.Gray;
            this.textBoxInput.WatermarkText = "Введите ваш вопрос здесь и нажмите клавишу Enter.";
            this.textBoxInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.GetAnswer);
            // 
            // textBoxResult
            // 
            this.textBoxResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxResult.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxResult.Location = new System.Drawing.Point(3, 85);
            this.textBoxResult.Multiline = true;
            this.textBoxResult.Name = "textBoxResult";
            this.textBoxResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxResult.Size = new System.Drawing.Size(664, 188);
            this.textBoxResult.TabIndex = 3;
            this.textBoxResult.WatermarkColor = System.Drawing.Color.Gray;
            this.textBoxResult.WatermarkText = "Ответ системы будет выводиться здесь.";
            // 
            // menu
            // 
            this.menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsMenu,
            this.справкаToolStripMenuItem});
            this.menu.Location = new System.Drawing.Point(0, 0);
            this.menu.Name = "menu";
            this.menu.Size = new System.Drawing.Size(670, 24);
            this.menu.TabIndex = 1;
            this.menu.Text = "menuStrip1";
            // 
            // optionsMenu
            // 
            this.optionsMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resultsMenu});
            this.optionsMenu.Name = "optionsMenu";
            this.optionsMenu.Size = new System.Drawing.Size(80, 20);
            this.optionsMenu.Text = "Preferences";
            // 
            // resultsMenu
            // 
            this.resultsMenu.CheckOnClick = true;
            this.resultsMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.morphological,
            this.syntax,
            this.semantics});
            this.resultsMenu.Name = "resultsMenu";
            this.resultsMenu.Size = new System.Drawing.Size(183, 22);
            this.resultsMenu.Text = "Результаты анализа";
            this.resultsMenu.CheckedChanged += new System.EventHandler(this.PrintResultsCheckedChanged);
            // 
            // morphological
            // 
            this.morphological.CheckOnClick = true;
            this.morphological.Name = "morphological";
            this.morphological.Size = new System.Drawing.Size(180, 22);
            this.morphological.Text = "Морфологический";
            this.morphological.CheckedChanged += new System.EventHandler(this.MorphologicalCheckedChanged);
            // 
            // syntax
            // 
            this.syntax.CheckOnClick = true;
            this.syntax.Name = "syntax";
            this.syntax.Size = new System.Drawing.Size(180, 22);
            this.syntax.Text = "Синтаксический";
            this.syntax.CheckedChanged += new System.EventHandler(this.SyntaxCheckedChanged);
            // 
            // semantics
            // 
            this.semantics.CheckOnClick = true;
            this.semantics.Name = "semantics";
            this.semantics.Size = new System.Drawing.Size(180, 22);
            this.semantics.Text = "Семантический";
            this.semantics.CheckedChanged += new System.EventHandler(this.SemanticsCheckedChanged);
            // 
            // справкаToolStripMenuItem
            // 
            this.справкаToolStripMenuItem.Name = "справкаToolStripMenuItem";
            this.справкаToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.справкаToolStripMenuItem.Text = "Help";
            this.справкаToolStripMenuItem.Click += new System.EventHandler(this.ShowHelp);
            // 
            // ElizaForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(670, 300);
            this.Controls.Add(this.mainTable);
            this.Controls.Add(this.menu);
            this.MainMenuStrip = this.menu;
            this.Name = "ElizaForm";
            this.mainTable.ResumeLayout(false);
            this.mainTable.PerformLayout();
            this.menu.ResumeLayout(false);
            this.menu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel mainTable;
        private System.Windows.Forms.MenuStrip menu;
        private System.Windows.Forms.ToolStripMenuItem optionsMenu;
        private System.Windows.Forms.ToolStripMenuItem resultsMenu;
        private System.Windows.Forms.ToolStripMenuItem morphological;
        private System.Windows.Forms.ToolStripMenuItem syntax;
        private System.Windows.Forms.ToolStripMenuItem semantics;
        private WatermarkTextbox textBoxInput;
        private WatermarkTextbox textBoxResult;
        private System.Windows.Forms.ToolStripMenuItem справкаToolStripMenuItem;
    }
}

