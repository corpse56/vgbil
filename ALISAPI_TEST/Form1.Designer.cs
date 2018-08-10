namespace ALISAPI_TEST
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
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
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.ReadersGet = new System.Windows.Forms.Button();
            this.tbResponse = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // ReadersGet
            // 
            this.ReadersGet.Location = new System.Drawing.Point(14, 14);
            this.ReadersGet.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.ReadersGet.Name = "ReadersGet";
            this.ReadersGet.Size = new System.Drawing.Size(160, 35);
            this.ReadersGet.TabIndex = 0;
            this.ReadersGet.Text = "Readers/Get";
            this.ReadersGet.UseVisualStyleBackColor = true;
            this.ReadersGet.Click += new System.EventHandler(this.ReadersGet_Click);
            // 
            // tbResponse
            // 
            this.tbResponse.Location = new System.Drawing.Point(805, 19);
            this.tbResponse.Margin = new System.Windows.Forms.Padding(5);
            this.tbResponse.Multiline = true;
            this.tbResponse.Name = "tbResponse";
            this.tbResponse.Size = new System.Drawing.Size(626, 854);
            this.tbResponse.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1445, 887);
            this.Controls.Add(this.tbResponse);
            this.Controls.Add(this.ReadersGet);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ReadersGet;
        private System.Windows.Forms.TextBox tbResponse;
    }
}

