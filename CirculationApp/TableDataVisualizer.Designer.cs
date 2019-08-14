namespace CirculationApp
{
    partial class TableDataVisualizer
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
            this.dgViewer = new System.Windows.Forms.DataGridView();
            this.bOk = new System.Windows.Forms.Button();
            this.cbStatuses = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.bSaveToFile = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgViewer)).BeginInit();
            this.SuspendLayout();
            // 
            // dgViewer
            // 
            this.dgViewer.AllowUserToAddRows = false;
            this.dgViewer.AllowUserToDeleteRows = false;
            this.dgViewer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgViewer.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgViewer.Location = new System.Drawing.Point(16, 15);
            this.dgViewer.Margin = new System.Windows.Forms.Padding(4);
            this.dgViewer.MultiSelect = false;
            this.dgViewer.Name = "dgViewer";
            this.dgViewer.RowHeadersVisible = false;
            this.dgViewer.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgViewer.Size = new System.Drawing.Size(1469, 486);
            this.dgViewer.TabIndex = 0;
            // 
            // bOk
            // 
            this.bOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bOk.Location = new System.Drawing.Point(1385, 509);
            this.bOk.Margin = new System.Windows.Forms.Padding(4);
            this.bOk.Name = "bOk";
            this.bOk.Size = new System.Drawing.Size(100, 28);
            this.bOk.TabIndex = 1;
            this.bOk.Text = "ОК";
            this.bOk.UseVisualStyleBackColor = true;
            this.bOk.Click += new System.EventHandler(this.bOk_Click);
            // 
            // cbStatuses
            // 
            this.cbStatuses.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbStatuses.FormattingEnabled = true;
            this.cbStatuses.Location = new System.Drawing.Point(199, 509);
            this.cbStatuses.Name = "cbStatuses";
            this.cbStatuses.Size = new System.Drawing.Size(467, 24);
            this.cbStatuses.TabIndex = 2;
            this.cbStatuses.Visible = false;
            this.cbStatuses.SelectedIndexChanged += new System.EventHandler(this.cbStatuses_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 509);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(177, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "Выберите статуст заказа";
            this.label1.Visible = false;
            // 
            // bSaveToFile
            // 
            this.bSaveToFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bSaveToFile.Location = new System.Drawing.Point(1229, 509);
            this.bSaveToFile.Name = "bSaveToFile";
            this.bSaveToFile.Size = new System.Drawing.Size(149, 29);
            this.bSaveToFile.TabIndex = 4;
            this.bSaveToFile.Text = "Сохранить в файл";
            this.bSaveToFile.UseVisualStyleBackColor = true;
            this.bSaveToFile.Click += new System.EventHandler(this.bSaveToFile_Click);
            // 
            // TableDataVisualizer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1501, 550);
            this.Controls.Add(this.bSaveToFile);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbStatuses);
            this.Controls.Add(this.bOk);
            this.Controls.Add(this.dgViewer);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "TableDataVisualizer";
            this.Text = "TableDataVisualizer";
            ((System.ComponentModel.ISupportInitialize)(this.dgViewer)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgViewer;
        private System.Windows.Forms.Button bOk;
        private System.Windows.Forms.ComboBox cbStatuses;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bSaveToFile;
    }
}