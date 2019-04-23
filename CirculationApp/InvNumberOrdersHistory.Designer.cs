namespace CirculationApp
{
    partial class fInvNumberOrdersHistory
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
            this.label1 = new System.Windows.Forms.Label();
            this.tbInvNumber = new System.Windows.Forms.TextBox();
            this.bShowHistory = new System.Windows.Forms.Button();
            this.lbOrders = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dgOrderFlow = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgOrderFlow)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 15);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(199, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Введите инвентарный номер";
            // 
            // tbInvNumber
            // 
            this.tbInvNumber.Location = new System.Drawing.Point(216, 12);
            this.tbInvNumber.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbInvNumber.Name = "tbInvNumber";
            this.tbInvNumber.Size = new System.Drawing.Size(504, 22);
            this.tbInvNumber.TabIndex = 1;
            // 
            // bShowHistory
            // 
            this.bShowHistory.Location = new System.Drawing.Point(727, 12);
            this.bShowHistory.Name = "bShowHistory";
            this.bShowHistory.Size = new System.Drawing.Size(75, 23);
            this.bShowHistory.TabIndex = 2;
            this.bShowHistory.Text = "Найти";
            this.bShowHistory.UseVisualStyleBackColor = true;
            this.bShowHistory.Click += new System.EventHandler(this.bShowHistory_Click);
            // 
            // lbOrders
            // 
            this.lbOrders.FormattingEnabled = true;
            this.lbOrders.ItemHeight = 16;
            this.lbOrders.Location = new System.Drawing.Point(15, 122);
            this.lbOrders.Name = "lbOrders";
            this.lbOrders.Size = new System.Drawing.Size(117, 420);
            this.lbOrders.TabIndex = 3;
            this.lbOrders.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 67);
            this.label2.TabIndex = 4;
            this.label2.Text = "Список заказов на найденный инвентарный номер:";
            // 
            // dgOrderFlow
            // 
            this.dgOrderFlow.AllowUserToAddRows = false;
            this.dgOrderFlow.AllowUserToDeleteRows = false;
            this.dgOrderFlow.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgOrderFlow.Location = new System.Drawing.Point(138, 122);
            this.dgOrderFlow.MultiSelect = false;
            this.dgOrderFlow.Name = "dgOrderFlow";
            this.dgOrderFlow.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgOrderFlow.Size = new System.Drawing.Size(736, 420);
            this.dgOrderFlow.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(388, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 16);
            this.label3.TabIndex = 6;
            this.label3.Text = "Заглавие";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(152, 103);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(269, 16);
            this.label4.TabIndex = 7;
            this.label4.Text = "Список операций с выбранным заказом";
            // 
            // fInvNumberOrdersHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(886, 554);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dgOrderFlow);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lbOrders);
            this.Controls.Add(this.bShowHistory);
            this.Controls.Add(this.tbInvNumber);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.Name = "fInvNumberOrdersHistory";
            this.Text = "История заказов инвентарного номера";
            ((System.ComponentModel.ISupportInitialize)(this.dgOrderFlow)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbInvNumber;
        private System.Windows.Forms.Button bShowHistory;
        private System.Windows.Forms.ListBox lbOrders;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dgOrderFlow;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}