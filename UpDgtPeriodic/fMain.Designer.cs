namespace UpDgtPeriodic
{
    partial class fMain
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.привязатьЦифровуюКопиюПериодикиВФорматеJPGToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.привязатьЦифровуюКопиюПериодикиВФорматеPDFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.выходToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.trDgtPeriodic = new System.Windows.Forms.TreeView();
            this.label1 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.файлToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(806, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // файлToolStripMenuItem
            // 
            this.файлToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.привязатьЦифровуюКопиюПериодикиВФорматеJPGToolStripMenuItem,
            this.привязатьЦифровуюКопиюПериодикиВФорматеPDFToolStripMenuItem,
            this.toolStripSeparator1,
            this.выходToolStripMenuItem});
            this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            this.файлToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.файлToolStripMenuItem.Text = "Файл";
            // 
            // привязатьЦифровуюКопиюПериодикиВФорматеJPGToolStripMenuItem
            // 
            this.привязатьЦифровуюКопиюПериодикиВФорматеJPGToolStripMenuItem.Name = "привязатьЦифровуюКопиюПериодикиВФорматеJPGToolStripMenuItem";
            this.привязатьЦифровуюКопиюПериодикиВФорматеJPGToolStripMenuItem.Size = new System.Drawing.Size(381, 22);
            this.привязатьЦифровуюКопиюПериодикиВФорматеJPGToolStripMenuItem.Text = "Привязать цифровую копию периодики в формате JPG";
            this.привязатьЦифровуюКопиюПериодикиВФорматеJPGToolStripMenuItem.Click += new System.EventHandler(this.JPGToolStripMenuItem_Click);
            // 
            // привязатьЦифровуюКопиюПериодикиВФорматеPDFToolStripMenuItem
            // 
            this.привязатьЦифровуюКопиюПериодикиВФорматеPDFToolStripMenuItem.Name = "привязатьЦифровуюКопиюПериодикиВФорматеPDFToolStripMenuItem";
            this.привязатьЦифровуюКопиюПериодикиВФорматеPDFToolStripMenuItem.Size = new System.Drawing.Size(381, 22);
            this.привязатьЦифровуюКопиюПериодикиВФорматеPDFToolStripMenuItem.Text = "Привязать цифровую копию периодики в формате PDF";
            this.привязатьЦифровуюКопиюПериодикиВФорматеPDFToolStripMenuItem.Click += new System.EventHandler(this.PDFToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(378, 6);
            // 
            // выходToolStripMenuItem
            // 
            this.выходToolStripMenuItem.Name = "выходToolStripMenuItem";
            this.выходToolStripMenuItem.Size = new System.Drawing.Size(381, 22);
            this.выходToolStripMenuItem.Text = "Выход";
            this.выходToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // trDgtPeriodic
            // 
            this.trDgtPeriodic.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.trDgtPeriodic.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.trDgtPeriodic.Location = new System.Drawing.Point(12, 80);
            this.trDgtPeriodic.Name = "trDgtPeriodic";
            this.trDgtPeriodic.Size = new System.Drawing.Size(782, 362);
            this.trDgtPeriodic.TabIndex = 1;
            this.trDgtPeriodic.NodeMouseHover += new System.Windows.Forms.TreeNodeMouseHoverEventHandler(this.trDgtPeriodic_NodeMouseHover);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(12, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(650, 29);
            this.label1.TabIndex = 2;
            this.label1.Text = "Имеющиеся цифровые копии периодических изданий";
            // 
            // fMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(806, 454);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.trDgtPeriodic);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "fMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Привязка цифровых копий периодики";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem файлToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem привязатьЦифровуюКопиюПериодикиВФорматеJPGToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem привязатьЦифровуюКопиюПериодикиВФорматеPDFToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem выходToolStripMenuItem;
        private System.Windows.Forms.TreeView trDgtPeriodic;
        private System.Windows.Forms.Label label1;
    }
}

