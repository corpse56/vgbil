namespace ExportBJ_XML
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.all = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.exportSingleRecord = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btnbjvvv = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnredkostj = new System.Windows.Forms.Button();
            this.btnbrit_sovet = new System.Windows.Forms.Button();
            this.btnbjacc = new System.Windows.Forms.Button();
            this.btnbjfcc = new System.Windows.Forms.Button();
            this.btnbjscc = new System.Windows.Forms.Button();
            this.btnPeriod = new System.Windows.Forms.Button();
            this.btnPearson = new System.Windows.Forms.Button();
            this.btnLitres = new System.Windows.Forms.Button();
            this.bjvvvCovers = new System.Windows.Forms.Button();
            this.litresCovers = new System.Windows.Forms.Button();
            this.allCovers = new System.Windows.Forms.Button();
            this.redkostjCovers = new System.Windows.Forms.Button();
            this.brit_sovetCovers = new System.Windows.Forms.Button();
            this.bjaccCovers = new System.Windows.Forms.Button();
            this.bjfccCovers = new System.Windows.Forms.Button();
            this.bjsccCovers = new System.Windows.Forms.Button();
            this.periodCovers = new System.Windows.Forms.Button();
            this.pearsonCovers = new System.Windows.Forms.Button();
            this.getLitresSource = new System.Windows.Forms.Button();
            this.getPearsonSource = new System.Windows.Forms.Button();
            this.txtSingleRecordId = new System.Windows.Forms.TextBox();
            this.btnJBH = new System.Windows.Forms.Button();
            this.btnGetJBHSource = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // all
            // 
            this.all.Location = new System.Drawing.Point(12, 366);
            this.all.Name = "all";
            this.all.Size = new System.Drawing.Size(75, 23);
            this.all.TabIndex = 0;
            this.all.Text = "all";
            this.all.UseVisualStyleBackColor = true;
            this.all.Click += new System.EventHandler(this.all_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(217, 459);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "test";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(121, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "label1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(121, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "label2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(121, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "label3";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(121, 61);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "label4";
            // 
            // exportSingleRecord
            // 
            this.exportSingleRecord.Location = new System.Drawing.Point(445, 395);
            this.exportSingleRecord.Name = "exportSingleRecord";
            this.exportSingleRecord.Size = new System.Drawing.Size(109, 23);
            this.exportSingleRecord.TabIndex = 6;
            this.exportSingleRecord.Text = "exportSingleRecord";
            this.exportSingleRecord.UseVisualStyleBackColor = true;
            this.exportSingleRecord.Click += new System.EventHandler(this.exportSingleRecord_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // btnbjvvv
            // 
            this.btnbjvvv.Location = new System.Drawing.Point(12, 33);
            this.btnbjvvv.Name = "btnbjvvv";
            this.btnbjvvv.Size = new System.Drawing.Size(75, 23);
            this.btnbjvvv.TabIndex = 7;
            this.btnbjvvv.Text = "bjvvv";
            this.btnbjvvv.UseVisualStyleBackColor = true;
            this.btnbjvvv.Click += new System.EventHandler(this.bjvvv_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(110, 137);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(444, 252);
            this.textBox1.TabIndex = 8;
            // 
            // btnredkostj
            // 
            this.btnredkostj.Location = new System.Drawing.Point(12, 62);
            this.btnredkostj.Name = "btnredkostj";
            this.btnredkostj.Size = new System.Drawing.Size(75, 23);
            this.btnredkostj.TabIndex = 9;
            this.btnredkostj.Text = "redkostj";
            this.btnredkostj.UseVisualStyleBackColor = true;
            this.btnredkostj.Click += new System.EventHandler(this.redkostj_Click);
            // 
            // btnbrit_sovet
            // 
            this.btnbrit_sovet.Location = new System.Drawing.Point(12, 91);
            this.btnbrit_sovet.Name = "btnbrit_sovet";
            this.btnbrit_sovet.Size = new System.Drawing.Size(75, 23);
            this.btnbrit_sovet.TabIndex = 10;
            this.btnbrit_sovet.Text = "brit_sovet";
            this.btnbrit_sovet.UseVisualStyleBackColor = true;
            this.btnbrit_sovet.Click += new System.EventHandler(this.brit_sovet_Click);
            // 
            // btnbjacc
            // 
            this.btnbjacc.Location = new System.Drawing.Point(12, 120);
            this.btnbjacc.Name = "btnbjacc";
            this.btnbjacc.Size = new System.Drawing.Size(75, 23);
            this.btnbjacc.TabIndex = 10;
            this.btnbjacc.Text = "bjacc";
            this.btnbjacc.UseVisualStyleBackColor = true;
            this.btnbjacc.Click += new System.EventHandler(this.bjacc_Click);
            // 
            // btnbjfcc
            // 
            this.btnbjfcc.Location = new System.Drawing.Point(12, 149);
            this.btnbjfcc.Name = "btnbjfcc";
            this.btnbjfcc.Size = new System.Drawing.Size(75, 23);
            this.btnbjfcc.TabIndex = 10;
            this.btnbjfcc.Text = "bjfcc";
            this.btnbjfcc.UseVisualStyleBackColor = true;
            this.btnbjfcc.Click += new System.EventHandler(this.bjfcc_Click);
            // 
            // btnbjscc
            // 
            this.btnbjscc.Location = new System.Drawing.Point(12, 178);
            this.btnbjscc.Name = "btnbjscc";
            this.btnbjscc.Size = new System.Drawing.Size(75, 23);
            this.btnbjscc.TabIndex = 10;
            this.btnbjscc.Text = "bjscc";
            this.btnbjscc.UseVisualStyleBackColor = true;
            this.btnbjscc.Click += new System.EventHandler(this.bjscc_Click);
            // 
            // btnPeriod
            // 
            this.btnPeriod.Location = new System.Drawing.Point(12, 207);
            this.btnPeriod.Name = "btnPeriod";
            this.btnPeriod.Size = new System.Drawing.Size(75, 23);
            this.btnPeriod.TabIndex = 10;
            this.btnPeriod.Text = "period";
            this.btnPeriod.UseVisualStyleBackColor = true;
            this.btnPeriod.Click += new System.EventHandler(this.period_Click);
            // 
            // btnPearson
            // 
            this.btnPearson.Location = new System.Drawing.Point(12, 236);
            this.btnPearson.Name = "btnPearson";
            this.btnPearson.Size = new System.Drawing.Size(75, 23);
            this.btnPearson.TabIndex = 10;
            this.btnPearson.Text = "pearson";
            this.btnPearson.UseVisualStyleBackColor = true;
            this.btnPearson.Click += new System.EventHandler(this.pearson_Click);
            // 
            // btnLitres
            // 
            this.btnLitres.Location = new System.Drawing.Point(12, 265);
            this.btnLitres.Name = "btnLitres";
            this.btnLitres.Size = new System.Drawing.Size(75, 23);
            this.btnLitres.TabIndex = 10;
            this.btnLitres.Text = "litres";
            this.btnLitres.UseVisualStyleBackColor = true;
            this.btnLitres.Click += new System.EventHandler(this.litres_Click);
            // 
            // bjvvvCovers
            // 
            this.bjvvvCovers.Location = new System.Drawing.Point(580, 38);
            this.bjvvvCovers.Name = "bjvvvCovers";
            this.bjvvvCovers.Size = new System.Drawing.Size(86, 23);
            this.bjvvvCovers.TabIndex = 11;
            this.bjvvvCovers.Text = "bjvvvCovers";
            this.bjvvvCovers.UseVisualStyleBackColor = true;
            this.bjvvvCovers.Click += new System.EventHandler(this.bjvvvCovers_Click);
            // 
            // litresCovers
            // 
            this.litresCovers.Location = new System.Drawing.Point(580, 265);
            this.litresCovers.Name = "litresCovers";
            this.litresCovers.Size = new System.Drawing.Size(86, 23);
            this.litresCovers.TabIndex = 11;
            this.litresCovers.Text = "litresCovers";
            this.litresCovers.UseVisualStyleBackColor = true;
            this.litresCovers.Click += new System.EventHandler(this.litresCovers_Click);
            // 
            // allCovers
            // 
            this.allCovers.Location = new System.Drawing.Point(580, 356);
            this.allCovers.Name = "allCovers";
            this.allCovers.Size = new System.Drawing.Size(75, 23);
            this.allCovers.TabIndex = 12;
            this.allCovers.Text = "all Covers";
            this.allCovers.UseVisualStyleBackColor = true;
            this.allCovers.Click += new System.EventHandler(this.allCovers_Click);
            // 
            // redkostjCovers
            // 
            this.redkostjCovers.Location = new System.Drawing.Point(580, 67);
            this.redkostjCovers.Name = "redkostjCovers";
            this.redkostjCovers.Size = new System.Drawing.Size(86, 23);
            this.redkostjCovers.TabIndex = 13;
            this.redkostjCovers.Text = "redkostjCovers";
            this.redkostjCovers.UseVisualStyleBackColor = true;
            // 
            // brit_sovetCovers
            // 
            this.brit_sovetCovers.Location = new System.Drawing.Point(580, 96);
            this.brit_sovetCovers.Name = "brit_sovetCovers";
            this.brit_sovetCovers.Size = new System.Drawing.Size(86, 23);
            this.brit_sovetCovers.TabIndex = 13;
            this.brit_sovetCovers.Text = "brit_sovetCovers";
            this.brit_sovetCovers.UseVisualStyleBackColor = true;
            // 
            // bjaccCovers
            // 
            this.bjaccCovers.Location = new System.Drawing.Point(580, 125);
            this.bjaccCovers.Name = "bjaccCovers";
            this.bjaccCovers.Size = new System.Drawing.Size(86, 23);
            this.bjaccCovers.TabIndex = 13;
            this.bjaccCovers.Text = "bjaccCovers";
            this.bjaccCovers.UseVisualStyleBackColor = true;
            // 
            // bjfccCovers
            // 
            this.bjfccCovers.Location = new System.Drawing.Point(580, 154);
            this.bjfccCovers.Name = "bjfccCovers";
            this.bjfccCovers.Size = new System.Drawing.Size(86, 23);
            this.bjfccCovers.TabIndex = 13;
            this.bjfccCovers.Text = "bjfccCovers";
            this.bjfccCovers.UseVisualStyleBackColor = true;
            // 
            // bjsccCovers
            // 
            this.bjsccCovers.Location = new System.Drawing.Point(580, 183);
            this.bjsccCovers.Name = "bjsccCovers";
            this.bjsccCovers.Size = new System.Drawing.Size(86, 23);
            this.bjsccCovers.TabIndex = 13;
            this.bjsccCovers.Text = "bjsccCovers";
            this.bjsccCovers.UseVisualStyleBackColor = true;
            // 
            // periodCovers
            // 
            this.periodCovers.Location = new System.Drawing.Point(580, 212);
            this.periodCovers.Name = "periodCovers";
            this.periodCovers.Size = new System.Drawing.Size(86, 23);
            this.periodCovers.TabIndex = 13;
            this.periodCovers.Text = "periodCovers";
            this.periodCovers.UseVisualStyleBackColor = true;
            // 
            // pearsonCovers
            // 
            this.pearsonCovers.Location = new System.Drawing.Point(580, 236);
            this.pearsonCovers.Name = "pearsonCovers";
            this.pearsonCovers.Size = new System.Drawing.Size(86, 23);
            this.pearsonCovers.TabIndex = 13;
            this.pearsonCovers.Text = "pearsonCovers";
            this.pearsonCovers.UseVisualStyleBackColor = true;
            this.pearsonCovers.Click += new System.EventHandler(this.pearsonCovers_Click);
            // 
            // getLitresSource
            // 
            this.getLitresSource.Location = new System.Drawing.Point(12, 468);
            this.getLitresSource.Name = "getLitresSource";
            this.getLitresSource.Size = new System.Drawing.Size(111, 23);
            this.getLitresSource.TabIndex = 14;
            this.getLitresSource.Text = "getLitresSource";
            this.getLitresSource.UseVisualStyleBackColor = true;
            this.getLitresSource.Click += new System.EventHandler(this.getLitresSource_Click);
            // 
            // getPearsonSource
            // 
            this.getPearsonSource.Location = new System.Drawing.Point(12, 497);
            this.getPearsonSource.Name = "getPearsonSource";
            this.getPearsonSource.Size = new System.Drawing.Size(111, 23);
            this.getPearsonSource.TabIndex = 14;
            this.getPearsonSource.Text = "getPearsonSource";
            this.getPearsonSource.UseVisualStyleBackColor = true;
            this.getPearsonSource.Click += new System.EventHandler(this.getPearsonSource_Click);
            // 
            // txtSingleRecordId
            // 
            this.txtSingleRecordId.Location = new System.Drawing.Point(124, 395);
            this.txtSingleRecordId.Name = "txtSingleRecordId";
            this.txtSingleRecordId.Size = new System.Drawing.Size(301, 20);
            this.txtSingleRecordId.TabIndex = 15;
            // 
            // btnJBH
            // 
            this.btnJBH.Location = new System.Drawing.Point(12, 294);
            this.btnJBH.Name = "btnJBH";
            this.btnJBH.Size = new System.Drawing.Size(75, 23);
            this.btnJBH.TabIndex = 16;
            this.btnJBH.Text = "jbh";
            this.btnJBH.UseVisualStyleBackColor = true;
            this.btnJBH.Click += new System.EventHandler(this.btnJBH_Click);
            // 
            // btnGetJBHSource
            // 
            this.btnGetJBHSource.Location = new System.Drawing.Point(12, 439);
            this.btnGetJBHSource.Name = "btnGetJBHSource";
            this.btnGetJBHSource.Size = new System.Drawing.Size(111, 23);
            this.btnGetJBHSource.TabIndex = 17;
            this.btnGetJBHSource.Text = "getJBHSource";
            this.btnGetJBHSource.UseVisualStyleBackColor = true;
            this.btnGetJBHSource.Click += new System.EventHandler(this.btnGetJBHSource_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(693, 524);
            this.Controls.Add(this.btnGetJBHSource);
            this.Controls.Add(this.btnJBH);
            this.Controls.Add(this.txtSingleRecordId);
            this.Controls.Add(this.getPearsonSource);
            this.Controls.Add(this.getLitresSource);
            this.Controls.Add(this.pearsonCovers);
            this.Controls.Add(this.periodCovers);
            this.Controls.Add(this.bjsccCovers);
            this.Controls.Add(this.bjfccCovers);
            this.Controls.Add(this.bjaccCovers);
            this.Controls.Add(this.brit_sovetCovers);
            this.Controls.Add(this.redkostjCovers);
            this.Controls.Add(this.allCovers);
            this.Controls.Add(this.litresCovers);
            this.Controls.Add(this.bjvvvCovers);
            this.Controls.Add(this.btnLitres);
            this.Controls.Add(this.btnPearson);
            this.Controls.Add(this.btnPeriod);
            this.Controls.Add(this.btnbjscc);
            this.Controls.Add(this.btnbjfcc);
            this.Controls.Add(this.btnbjacc);
            this.Controls.Add(this.btnbrit_sovet);
            this.Controls.Add(this.btnredkostj);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btnbjvvv);
            this.Controls.Add(this.exportSingleRecord);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.all);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button all;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button exportSingleRecord;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnbjvvv;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.Label label2;
        public System.Windows.Forms.Label label3;
        public System.Windows.Forms.Label label4;
        public System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnredkostj;
        private System.Windows.Forms.Button btnbrit_sovet;
        private System.Windows.Forms.Button btnbjacc;
        private System.Windows.Forms.Button btnbjfcc;
        private System.Windows.Forms.Button btnbjscc;
        private System.Windows.Forms.Button btnPeriod;
        private System.Windows.Forms.Button btnPearson;
        private System.Windows.Forms.Button btnLitres;
        private System.Windows.Forms.Button bjvvvCovers;
        private System.Windows.Forms.Button litresCovers;
        private System.Windows.Forms.Button allCovers;
        private System.Windows.Forms.Button redkostjCovers;
        private System.Windows.Forms.Button brit_sovetCovers;
        private System.Windows.Forms.Button bjaccCovers;
        private System.Windows.Forms.Button bjfccCovers;
        private System.Windows.Forms.Button bjsccCovers;
        private System.Windows.Forms.Button periodCovers;
        private System.Windows.Forms.Button pearsonCovers;
        private System.Windows.Forms.Button getLitresSource;
        private System.Windows.Forms.Button getPearsonSource;
        private System.Windows.Forms.TextBox txtSingleRecordId;
        private System.Windows.Forms.Button btnJBH;
        private System.Windows.Forms.Button btnGetJBHSource;
    }
}

