namespace CirculationApp
{
    partial class OrderFromHall
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
            this.dgvHallOrders = new System.Windows.Forms.DataGridView();
            this.bClose = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHallOrders)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvHallOrders
            // 
            this.dgvHallOrders.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvHallOrders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHallOrders.Location = new System.Drawing.Point(12, 12);
            this.dgvHallOrders.Name = "dgvHallOrders";
            this.dgvHallOrders.Size = new System.Drawing.Size(1098, 527);
            this.dgvHallOrders.TabIndex = 0;
            // 
            // bClose
            // 
            this.bClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bClose.Location = new System.Drawing.Point(1035, 551);
            this.bClose.Name = "bClose";
            this.bClose.Size = new System.Drawing.Size(75, 32);
            this.bClose.TabIndex = 1;
            this.bClose.Text = "Закрыть";
            this.bClose.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(837, 551);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(192, 32);
            this.button1.TabIndex = 1;
            this.button1.Text = "Заказ готов к выдаче";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // OrderFromHall
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1122, 586);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.bClose);
            this.Controls.Add(this.dgvHallOrders);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "OrderFromHall";
            this.Text = "Заказ из открытого доступа";
            this.Load += new System.EventHandler(this.OrderFromHall_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvHallOrders)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvHallOrders;
        private System.Windows.Forms.Button bClose;
        private System.Windows.Forms.Button button1;
    }
}