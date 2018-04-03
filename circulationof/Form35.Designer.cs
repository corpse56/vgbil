namespace Circulation
{
    partial class Form35
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
            this.InputService = new System.Windows.Forms.DataGridView();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.InputService)).BeginInit();
            this.SuspendLayout();
            // 
            // InputService
            // 
            this.InputService.AllowUserToAddRows = false;
            this.InputService.AllowUserToDeleteRows = false;
            this.InputService.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.InputService.Location = new System.Drawing.Point(16, 15);
            this.InputService.Margin = new System.Windows.Forms.Padding(4);
            this.InputService.MultiSelect = false;
            this.InputService.Name = "InputService";
            this.InputService.ReadOnly = true;
            this.InputService.RowHeadersVisible = false;
            this.InputService.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.InputService.Size = new System.Drawing.Size(440, 440);
            this.InputService.TabIndex = 4;
            this.InputService.CellValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.InputService_CellValidated);
            this.InputService.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.InputService_DataError);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(300, 462);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "ОК";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(381, 462);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 6;
            this.button2.Text = "Отмена";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form35
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(469, 493);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.InputService);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form35";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Введите количество оказанных услуг";
            this.Load += new System.EventHandler(this.Form35_Load);
            ((System.ComponentModel.ISupportInitialize)(this.InputService)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView InputService;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}