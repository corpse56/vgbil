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
            this.dgViewer.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
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
            this.bOk.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.bOk.Name = "bOk";
            this.bOk.Size = new System.Drawing.Size(100, 28);
            this.bOk.TabIndex = 1;
            this.bOk.Text = "ОК";
            this.bOk.UseVisualStyleBackColor = true;
            // 
            // TableDataVisualizer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1501, 550);
            this.Controls.Add(this.bOk);
            this.Controls.Add(this.dgViewer);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "TableDataVisualizer";
            this.Text = "TableDataVisualizer";
            ((System.ComponentModel.ISupportInitialize)(this.dgViewer)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgViewer;
        private System.Windows.Forms.Button bOk;
    }
}