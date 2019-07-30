namespace Cyriller.Checker
{
    partial class StartForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StartForm));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.msiFile = new System.Windows.Forms.ToolStripMenuItem();
            this.msiNoun = new System.Windows.Forms.ToolStripMenuItem();
            this.msiAdjective = new System.Windows.Forms.ToolStripMenuItem();
            this.msiNumber = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.msiPhrase = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.msiExit = new System.Windows.Forms.ToolStripMenuItem();
            this.msiDictionary = new System.Windows.Forms.ToolStripMenuItem();
            this.msiExportToJson = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlContainer = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.menuStrip1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.pnlContainer, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(632, 512);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.msiFile,
            this.msiDictionary});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(632, 30);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // msiFile
            // 
            this.msiFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.msiNoun,
            this.msiAdjective,
            this.msiNumber,
            this.toolStripSeparator1,
            this.msiPhrase,
            this.toolStripSeparator2,
            this.msiExit});
            this.msiFile.Name = "msiFile";
            this.msiFile.Size = new System.Drawing.Size(48, 26);
            this.msiFile.Text = "Файл";
            // 
            // msiNoun
            // 
            this.msiNoun.Name = "msiNoun";
            this.msiNoun.Size = new System.Drawing.Size(226, 22);
            this.msiNoun.Text = "Склонять существительное";
            this.msiNoun.Click += new System.EventHandler(this.msiNoun_Click);
            // 
            // msiAdjective
            // 
            this.msiAdjective.Name = "msiAdjective";
            this.msiAdjective.Size = new System.Drawing.Size(226, 22);
            this.msiAdjective.Text = "Склонять прилагательное";
            this.msiAdjective.Click += new System.EventHandler(this.msiAdjective_Click);
            // 
            // msiNumber
            // 
            this.msiNumber.Name = "msiNumber";
            this.msiNumber.Size = new System.Drawing.Size(226, 22);
            this.msiNumber.Text = "Склонять число";
            this.msiNumber.Click += new System.EventHandler(this.msiNumber_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(223, 6);
            // 
            // msiPhrase
            // 
            this.msiPhrase.Name = "msiPhrase";
            this.msiPhrase.Size = new System.Drawing.Size(226, 22);
            this.msiPhrase.Text = "Склонять словосочетание";
            this.msiPhrase.Click += new System.EventHandler(this.msiPhrase_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(223, 6);
            // 
            // msiExit
            // 
            this.msiExit.Name = "msiExit";
            this.msiExit.Size = new System.Drawing.Size(226, 22);
            this.msiExit.Text = "Выход";
            this.msiExit.Click += new System.EventHandler(this.msiExit_Click);
            // 
            // msiDictionary
            // 
            this.msiDictionary.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.msiExportToJson});
            this.msiDictionary.Name = "msiDictionary";
            this.msiDictionary.Size = new System.Drawing.Size(66, 26);
            this.msiDictionary.Text = "Словарь";
            // 
            // msiExportToJson
            // 
            this.msiExportToJson.Name = "msiExportToJson";
            this.msiExportToJson.Size = new System.Drawing.Size(171, 22);
            this.msiExportToJson.Text = "Выгрузить в JSON";
            this.msiExportToJson.Click += new System.EventHandler(this.msiExportToJson_Click);
            // 
            // pnlContainer
            // 
            this.pnlContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContainer.Location = new System.Drawing.Point(3, 33);
            this.pnlContainer.Name = "pnlContainer";
            this.pnlContainer.Size = new System.Drawing.Size(626, 476);
            this.pnlContainer.TabIndex = 1;
            // 
            // StartForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(632, 512);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(648, 551);
            this.Name = "StartForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Кириллер - бесплатная программа склонения по падежам";
            this.Load += new System.EventHandler(this.StartForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem msiFile;
        private System.Windows.Forms.ToolStripMenuItem msiNumber;
        private System.Windows.Forms.Panel pnlContainer;
        private System.Windows.Forms.ToolStripMenuItem msiNoun;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem msiExit;
        private System.Windows.Forms.ToolStripMenuItem msiAdjective;
        private System.Windows.Forms.ToolStripMenuItem msiPhrase;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem msiDictionary;
        private System.Windows.Forms.ToolStripMenuItem msiExportToJson;
    }
}