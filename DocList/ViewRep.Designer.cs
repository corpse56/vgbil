namespace DocList
{
    partial class ViewRep
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
            this.Viewer = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.SuspendLayout();
            // 
            // Viewer
            // 
            this.Viewer.ActiveViewIndex = -1;
            this.Viewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Viewer.DisplayGroupTree = false;
            this.Viewer.Location = new System.Drawing.Point(16, 15);
            this.Viewer.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Viewer.Name = "Viewer";
            this.Viewer.SelectionFormula = "";
            this.Viewer.ShowCloseButton = false;
            this.Viewer.ShowGroupTreeButton = false;
            this.Viewer.Size = new System.Drawing.Size(985, 640);
            this.Viewer.TabIndex = 6;
            this.Viewer.ViewTimeSelectionFormula = "";
            // 
            // ViewRep
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1014, 668);
            this.Controls.Add(this.Viewer);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ViewRep";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Просмотр списка";
            this.ResumeLayout(false);

        }

        #endregion

        private CrystalDecisions.Windows.Forms.CrystalReportViewer Viewer;
    }
}