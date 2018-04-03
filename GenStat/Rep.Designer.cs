namespace GenStat
{
    partial class Rep
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
            this.RViewer = new DBCrystalReportViewer();
            this.SuspendLayout();
            // 
            // RViewer
            // 
            
            this.RViewer.ActiveViewIndex = -1;
            this.RViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.RViewer.DisplayBackgroundEdge = false;
            this.RViewer.DisplayGroupTree = false;
            this.RViewer.DisplayStatusBar = false;
            this.RViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RViewer.Location = new System.Drawing.Point(0, 0);
            this.RViewer.Name = "RViewer";
            this.RViewer.SelectionFormula = "";
            this.RViewer.ShowCloseButton = false;
            this.RViewer.ShowGotoPageButton = false;
            this.RViewer.ShowGroupTreeButton = false;
            this.RViewer.ShowRefreshButton = false;
            this.RViewer.ShowTextSearchButton = false;
            this.RViewer.Size = new System.Drawing.Size(935, 548);
            this.RViewer.TabIndex = 0;
            this.RViewer.ViewTimeSelectionFormula = "";
            // 
            // Rep
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(935, 548);
            this.Controls.Add(this.RViewer);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Rep";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Печать/сохранение в файл";
            this.ResumeLayout(false);

        }

        #endregion

        private DBCrystalReportViewer RViewer;
    }
}