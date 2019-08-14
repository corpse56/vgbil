namespace WriteOff
{
    partial class FSelectAct
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
            this.bCreateAct = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.bMakeActForYear = new System.Windows.Forms.Button();
            this.bMakeActPerYearOF = new System.Windows.Forms.Button();
            this.bAnotherFundholder = new System.Windows.Forms.Button();
            this.bByYearInActNameOF = new System.Windows.Forms.Button();
            this.bByYearInActNameAB = new System.Windows.Forms.Button();
            this.bByYearInActNameAnotherFundholder = new System.Windows.Forms.Button();
            this.bSpecifiedActNumbers = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // bCreateAct
            // 
            this.bCreateAct.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bCreateAct.Location = new System.Drawing.Point(479, 203);
            this.bCreateAct.Margin = new System.Windows.Forms.Padding(4);
            this.bCreateAct.Name = "bCreateAct";
            this.bCreateAct.Size = new System.Drawing.Size(204, 36);
            this.bCreateAct.TabIndex = 0;
            this.bCreateAct.Text = "Сформировать акт";
            this.bCreateAct.UseVisualStyleBackColor = true;
            this.bCreateAct.Click += new System.EventHandler(this.bCreateAct_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.DropDownWidth = 700;
            this.comboBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(392, 46);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(4);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(399, 32);
            this.comboBox1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(16, 49);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(322, 24);
            this.label1.TabIndex = 2;
            this.label1.Text = "Выберите отдел фондодержателя";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(16, 94);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(137, 24);
            this.label2.TabIndex = 2;
            this.label2.Text = "Выберите год";
            // 
            // comboBox2
            // 
            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(392, 86);
            this.comboBox2.Margin = new System.Windows.Forms.Padding(4);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(399, 32);
            this.comboBox2.TabIndex = 1;
            this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            // 
            // comboBox3
            // 
            this.comboBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Location = new System.Drawing.Point(392, 126);
            this.comboBox3.Margin = new System.Windows.Forms.Padding(4);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(399, 32);
            this.comboBox3.TabIndex = 1;
            this.comboBox3.SelectedIndexChanged += new System.EventHandler(this.comboBox3_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(16, 129);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(169, 24);
            this.label3.TabIndex = 2;
            this.label3.Text = "Выберите № акта";
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button2.Location = new System.Drawing.Point(691, 203);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(100, 36);
            this.button2.TabIndex = 3;
            this.button2.Text = "Закрыть";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // bMakeActForYear
            // 
            this.bMakeActForYear.Location = new System.Drawing.Point(258, 203);
            this.bMakeActForYear.Name = "bMakeActForYear";
            this.bMakeActForYear.Size = new System.Drawing.Size(214, 36);
            this.bMakeActForYear.TabIndex = 4;
            this.bMakeActForYear.Text = "Сделать акт за целый год АБ";
            this.bMakeActForYear.UseVisualStyleBackColor = true;
            this.bMakeActForYear.Click += new System.EventHandler(this.bMakeActForYear_Click);
            // 
            // bMakeActPerYearOF
            // 
            this.bMakeActPerYearOF.Location = new System.Drawing.Point(12, 203);
            this.bMakeActPerYearOF.Name = "bMakeActPerYearOF";
            this.bMakeActPerYearOF.Size = new System.Drawing.Size(240, 36);
            this.bMakeActPerYearOF.TabIndex = 5;
            this.bMakeActPerYearOF.Text = "Распечатать акт за целый год ОФ";
            this.bMakeActPerYearOF.UseVisualStyleBackColor = true;
            this.bMakeActPerYearOF.Click += new System.EventHandler(this.bMakeActPerYearOF_Click);
            // 
            // bAnotherFundholder
            // 
            this.bAnotherFundholder.Location = new System.Drawing.Point(12, 245);
            this.bAnotherFundholder.Name = "bAnotherFundholder";
            this.bAnotherFundholder.Size = new System.Drawing.Size(326, 36);
            this.bAnotherFundholder.TabIndex = 5;
            this.bAnotherFundholder.Text = "Распечатать акт за целый год не ОФ и не АБ";
            this.bAnotherFundholder.UseVisualStyleBackColor = true;
            this.bAnotherFundholder.Click += new System.EventHandler(this.bAnotherFundholder_Click);
            // 
            // bByYearInActNameOF
            // 
            this.bByYearInActNameOF.Location = new System.Drawing.Point(12, 287);
            this.bByYearInActNameOF.Name = "bByYearInActNameOF";
            this.bByYearInActNameOF.Size = new System.Drawing.Size(460, 23);
            this.bByYearInActNameOF.TabIndex = 6;
            this.bByYearInActNameOF.Text = "Сделать акт за целый год ОФ год взять из названия акта";
            this.bByYearInActNameOF.UseVisualStyleBackColor = true;
            this.bByYearInActNameOF.Click += new System.EventHandler(this.bByYearInActNameOF_Click);
            // 
            // bByYearInActNameAB
            // 
            this.bByYearInActNameAB.Location = new System.Drawing.Point(12, 315);
            this.bByYearInActNameAB.Name = "bByYearInActNameAB";
            this.bByYearInActNameAB.Size = new System.Drawing.Size(460, 23);
            this.bByYearInActNameAB.TabIndex = 7;
            this.bByYearInActNameAB.Text = "Сделать акт за целый год АБ год взять из названия акта";
            this.bByYearInActNameAB.UseVisualStyleBackColor = true;
            this.bByYearInActNameAB.Click += new System.EventHandler(this.bByYearInActNameAB_Click);
            // 
            // bByYearInActNameAnotherFundholder
            // 
            this.bByYearInActNameAnotherFundholder.Location = new System.Drawing.Point(12, 344);
            this.bByYearInActNameAnotherFundholder.Name = "bByYearInActNameAnotherFundholder";
            this.bByYearInActNameAnotherFundholder.Size = new System.Drawing.Size(609, 23);
            this.bByYearInActNameAnotherFundholder.TabIndex = 8;
            this.bByYearInActNameAnotherFundholder.Text = "Сделать акт за целый год ДРУГИЕ ФОНДОДЕРЖАТЕЛИ год взять из названия акта";
            this.bByYearInActNameAnotherFundholder.UseVisualStyleBackColor = true;
            this.bByYearInActNameAnotherFundholder.Click += new System.EventHandler(this.bByYearInActNameAnotherFundholder_Click);
            // 
            // bSpecifiedActNumbers
            // 
            this.bSpecifiedActNumbers.Location = new System.Drawing.Point(12, 373);
            this.bSpecifiedActNumbers.Name = "bSpecifiedActNumbers";
            this.bSpecifiedActNumbers.Size = new System.Drawing.Size(609, 23);
            this.bSpecifiedActNumbers.TabIndex = 8;
            this.bSpecifiedActNumbers.Text = "Сделать акт из указанных номеров актов";
            this.bSpecifiedActNumbers.UseVisualStyleBackColor = true;
            this.bSpecifiedActNumbers.Click += new System.EventHandler(this.bSpecifiedActNumbers_Click);
            // 
            // FSelectAct
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 412);
            this.Controls.Add(this.bSpecifiedActNumbers);
            this.Controls.Add(this.bByYearInActNameAnotherFundholder);
            this.Controls.Add(this.bByYearInActNameAB);
            this.Controls.Add(this.bByYearInActNameOF);
            this.Controls.Add(this.bAnotherFundholder);
            this.Controls.Add(this.bMakeActPerYearOF);
            this.Controls.Add(this.bMakeActForYear);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox3);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.bCreateAct);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FSelectAct";
            this.Text = "Списание книг";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bCreateAct;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button bMakeActForYear;
        private System.Windows.Forms.Button bMakeActPerYearOF;
        private System.Windows.Forms.Button bAnotherFundholder;
        private System.Windows.Forms.Button bByYearInActNameOF;
        private System.Windows.Forms.Button bByYearInActNameAB;
        private System.Windows.Forms.Button bByYearInActNameAnotherFundholder;
        private System.Windows.Forms.Button bSpecifiedActNumbers;
    }
}

