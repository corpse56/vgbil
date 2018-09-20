namespace LibflClassLibrary.Controls.Readers
{
    partial class ReaderRightsView
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

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.lvRights = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // lvRights
            // 
            this.lvRights.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.lvRights.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvRights.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvRights.Location = new System.Drawing.Point(0, 0);
            this.lvRights.MultiSelect = false;
            this.lvRights.Name = "lvRights";
            this.lvRights.Size = new System.Drawing.Size(450, 114);
            this.lvRights.TabIndex = 0;
            this.lvRights.UseCompatibleStateImageBehavior = false;
            this.lvRights.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 446;
            // 
            // ReaderRightsView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lvRights);
            this.Name = "ReaderRightsView";
            this.Size = new System.Drawing.Size(450, 114);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvRights;
        private System.Windows.Forms.ColumnHeader columnHeader1;
    }
}
