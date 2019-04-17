namespace BookkeepingForOrder
{
    partial class Form3
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
            this.dgwReaders = new System.Windows.Forms.DataGridView();
            this.button1 = new System.Windows.Forms.Button();
            this.bRefuseSelected = new System.Windows.Forms.Button();
            this.bPrintSelected = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgwReaders)).BeginInit();
            this.SuspendLayout();
            // 
            // dgwReaders
            // 
            this.dgwReaders.AllowUserToAddRows = false;
            this.dgwReaders.AllowUserToDeleteRows = false;
            this.dgwReaders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgwReaders.Location = new System.Drawing.Point(16, 15);
            this.dgwReaders.Margin = new System.Windows.Forms.Padding(4);
            this.dgwReaders.MultiSelect = false;
            this.dgwReaders.Name = "dgwReaders";
            this.dgwReaders.RowHeadersVisible = false;
            this.dgwReaders.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgwReaders.Size = new System.Drawing.Size(989, 585);
            this.dgwReaders.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(878, 609);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(127, 33);
            this.button1.TabIndex = 1;
            this.button1.Text = "Закрыть";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // bRefuseSelected
            // 
            this.bRefuseSelected.Location = new System.Drawing.Point(16, 607);
            this.bRefuseSelected.Name = "bRefuseSelected";
            this.bRefuseSelected.Size = new System.Drawing.Size(188, 35);
            this.bRefuseSelected.TabIndex = 2;
            this.bRefuseSelected.Text = "Дать отказ выделенной";
            this.bRefuseSelected.UseVisualStyleBackColor = true;
            this.bRefuseSelected.Click += new System.EventHandler(this.bRefuseSelected_Click);
            // 
            // bPrintSelected
            // 
            this.bPrintSelected.Location = new System.Drawing.Point(210, 607);
            this.bPrintSelected.Name = "bPrintSelected";
            this.bPrintSelected.Size = new System.Drawing.Size(216, 35);
            this.bPrintSelected.TabIndex = 3;
            this.bPrintSelected.Text = "Распечатать выделенный";
            this.bPrintSelected.UseVisualStyleBackColor = true;
            this.bPrintSelected.Click += new System.EventHandler(this.bPrintSelected_Click);
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1022, 655);
            this.Controls.Add(this.bPrintSelected);
            this.Controls.Add(this.bRefuseSelected);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dgwReaders);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "Form3";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Книги со статусом \"Сотрудник обрабатывает заказ\"";
            ((System.ComponentModel.ISupportInitialize)(this.dgwReaders)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgwReaders;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button bRefuseSelected;
        private System.Windows.Forms.Button bPrintSelected;
    }
}