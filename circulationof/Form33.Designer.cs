namespace Circulation
{
    partial class Form33
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
            this.Book = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.Current = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.History = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.Book)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Current)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.History)).BeginInit();
            this.SuspendLayout();
            // 
            // Book
            // 
            this.Book.AllowUserToAddRows = false;
            this.Book.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Book.Location = new System.Drawing.Point(12, 32);
            this.Book.MultiSelect = false;
            this.Book.Name = "Book";
            this.Book.ReadOnly = true;
            this.Book.RowHeadersVisible = false;
            this.Book.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.Book.Size = new System.Drawing.Size(970, 190);
            this.Book.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "Книга";
            // 
            // Current
            // 
            this.Current.AllowUserToAddRows = false;
            this.Current.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Current.Location = new System.Drawing.Point(12, 248);
            this.Current.MultiSelect = false;
            this.Current.Name = "Current";
            this.Current.ReadOnly = true;
            this.Current.RowHeadersVisible = false;
            this.Current.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.Current.Size = new System.Drawing.Size(970, 208);
            this.Current.TabIndex = 4;
            this.Current.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.Current_ColumnHeaderMouseClick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 225);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(127, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "Текущий статус";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // History
            // 
            this.History.AllowUserToAddRows = false;
            this.History.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.History.Location = new System.Drawing.Point(12, 482);
            this.History.MultiSelect = false;
            this.History.Name = "History";
            this.History.ReadOnly = true;
            this.History.RowHeadersVisible = false;
            this.History.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.History.Size = new System.Drawing.Size(970, 204);
            this.History.TabIndex = 6;
            this.History.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.History_ColumnHeaderMouseClick);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 459);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(144, 20);
            this.label3.TabIndex = 7;
            this.label3.Text = "Прошлые выдачи";
            // 
            // Form33
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(994, 698);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.History);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Current);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Book);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form33";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "История инвентарного номера";
            ((System.ComponentModel.ISupportInitialize)(this.Book)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Current)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.History)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView Book;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView Current;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView History;
        private System.Windows.Forms.Label label3;

    }
}