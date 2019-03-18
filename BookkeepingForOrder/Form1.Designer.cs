namespace BookkeepingForOrder
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpEmployeeOrders = new System.Windows.Forms.TabPage();
            this.button2 = new System.Windows.Forms.Button();
            this.bEmployeeOrder = new System.Windows.Forms.Button();
            this.dgwEmp = new System.Windows.Forms.DataGridView();
            this.tpReaderOrders = new System.Windows.Forms.TabPage();
            this.bRefusual = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.bPrintReaderOrder = new System.Windows.Forms.Button();
            this.dgwReaders = new System.Windows.Forms.DataGridView();
            this.tabHis = new System.Windows.Forms.TabPage();
            this.button12 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.dgwHis = new System.Windows.Forms.DataGridView();
            this.tpReaderHistoryOrders = new System.Windows.Forms.TabPage();
            this.button14 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.bReadersHistory = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.dgwRHis = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.button5 = new System.Windows.Forms.Button();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.button6 = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tpEmployeeOrders.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgwEmp)).BeginInit();
            this.tpReaderOrders.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgwReaders)).BeginInit();
            this.tabHis.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgwHis)).BeginInit();
            this.tpReaderHistoryOrders.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgwRHis)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tpEmployeeOrders);
            this.tabControl1.Controls.Add(this.tpReaderOrders);
            this.tabControl1.Controls.Add(this.tabHis);
            this.tabControl1.Controls.Add(this.tpReaderHistoryOrders);
            this.tabControl1.ItemSize = new System.Drawing.Size(150, 21);
            this.tabControl1.Location = new System.Drawing.Point(1, 47);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1205, 550);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tpEmployeeOrders
            // 
            this.tpEmployeeOrders.Controls.Add(this.button2);
            this.tpEmployeeOrders.Controls.Add(this.bEmployeeOrder);
            this.tpEmployeeOrders.Controls.Add(this.dgwEmp);
            this.tpEmployeeOrders.Location = new System.Drawing.Point(4, 25);
            this.tpEmployeeOrders.Margin = new System.Windows.Forms.Padding(4);
            this.tpEmployeeOrders.Name = "tpEmployeeOrders";
            this.tpEmployeeOrders.Padding = new System.Windows.Forms.Padding(4);
            this.tpEmployeeOrders.Size = new System.Drawing.Size(1122, 521);
            this.tpEmployeeOrders.TabIndex = 0;
            this.tpEmployeeOrders.Text = "Заказы сотрудников на сегодня";
            this.tpEmployeeOrders.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(1015, 489);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(100, 28);
            this.button2.TabIndex = 2;
            this.button2.Text = "Выход";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // bEmployeeOrder
            // 
            this.bEmployeeOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bEmployeeOrder.Location = new System.Drawing.Point(782, 489);
            this.bEmployeeOrder.Margin = new System.Windows.Forms.Padding(4);
            this.bEmployeeOrder.Name = "bEmployeeOrder";
            this.bEmployeeOrder.Size = new System.Drawing.Size(225, 28);
            this.bEmployeeOrder.TabIndex = 1;
            this.bEmployeeOrder.Text = "Распечатать заказ";
            this.bEmployeeOrder.UseVisualStyleBackColor = true;
            this.bEmployeeOrder.Click += new System.EventHandler(this.bEmployeeOrder_Click);
            // 
            // dgwEmp
            // 
            this.dgwEmp.AllowUserToAddRows = false;
            this.dgwEmp.AllowUserToDeleteRows = false;
            this.dgwEmp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightCoral;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgwEmp.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgwEmp.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.LightCoral;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgwEmp.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgwEmp.EnableHeadersVisualStyles = false;
            this.dgwEmp.Location = new System.Drawing.Point(2, 7);
            this.dgwEmp.Margin = new System.Windows.Forms.Padding(4);
            this.dgwEmp.MultiSelect = false;
            this.dgwEmp.Name = "dgwEmp";
            this.dgwEmp.ReadOnly = true;
            this.dgwEmp.RowHeadersVisible = false;
            this.dgwEmp.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgwEmp.Size = new System.Drawing.Size(1112, 478);
            this.dgwEmp.TabIndex = 0;
            // 
            // tpReaderOrders
            // 
            this.tpReaderOrders.Controls.Add(this.bRefusual);
            this.tpReaderOrders.Controls.Add(this.button7);
            this.tpReaderOrders.Controls.Add(this.bPrintReaderOrder);
            this.tpReaderOrders.Controls.Add(this.dgwReaders);
            this.tpReaderOrders.Location = new System.Drawing.Point(4, 25);
            this.tpReaderOrders.Name = "tpReaderOrders";
            this.tpReaderOrders.Padding = new System.Windows.Forms.Padding(3);
            this.tpReaderOrders.Size = new System.Drawing.Size(1197, 521);
            this.tpReaderOrders.TabIndex = 2;
            this.tpReaderOrders.Text = "Заказы читателей на неделю";
            this.tpReaderOrders.UseVisualStyleBackColor = true;
            // 
            // bRefusual
            // 
            this.bRefusual.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bRefusual.Location = new System.Drawing.Point(690, 484);
            this.bRefusual.Name = "bRefusual";
            this.bRefusual.Size = new System.Drawing.Size(160, 28);
            this.bRefusual.TabIndex = 5;
            this.bRefusual.Text = "Дать отказ";
            this.bRefusual.UseVisualStyleBackColor = true;
            this.bRefusual.Click += new System.EventHandler(this.bRefusual_Click);
            // 
            // button7
            // 
            this.button7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button7.Location = new System.Drawing.Point(1090, 484);
            this.button7.Margin = new System.Windows.Forms.Padding(4);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(100, 28);
            this.button7.TabIndex = 4;
            this.button7.Text = "Выход";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // bPrintReaderOrder
            // 
            this.bPrintReaderOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bPrintReaderOrder.Location = new System.Drawing.Point(857, 484);
            this.bPrintReaderOrder.Margin = new System.Windows.Forms.Padding(4);
            this.bPrintReaderOrder.Name = "bPrintReaderOrder";
            this.bPrintReaderOrder.Size = new System.Drawing.Size(225, 28);
            this.bPrintReaderOrder.TabIndex = 3;
            this.bPrintReaderOrder.Text = "Распечатать заказ";
            this.bPrintReaderOrder.UseVisualStyleBackColor = true;
            this.bPrintReaderOrder.Click += new System.EventHandler(this.bPrintReaderOrder_Click);
            // 
            // dgwReaders
            // 
            this.dgwReaders.AllowUserToAddRows = false;
            this.dgwReaders.AllowUserToDeleteRows = false;
            this.dgwReaders.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.LightCoral;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgwReaders.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgwReaders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.LightCoral;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgwReaders.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgwReaders.EnableHeadersVisualStyles = false;
            this.dgwReaders.Location = new System.Drawing.Point(2, 7);
            this.dgwReaders.Margin = new System.Windows.Forms.Padding(4);
            this.dgwReaders.MultiSelect = false;
            this.dgwReaders.Name = "dgwReaders";
            this.dgwReaders.ReadOnly = true;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.LightCoral;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgwReaders.RowHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgwReaders.RowHeadersVisible = false;
            this.dgwReaders.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgwReaders.Size = new System.Drawing.Size(1188, 471);
            this.dgwReaders.TabIndex = 1;
            // 
            // tabHis
            // 
            this.tabHis.Controls.Add(this.button12);
            this.tabHis.Controls.Add(this.button4);
            this.tabHis.Controls.Add(this.button3);
            this.tabHis.Controls.Add(this.dgwHis);
            this.tabHis.Location = new System.Drawing.Point(4, 25);
            this.tabHis.Name = "tabHis";
            this.tabHis.Padding = new System.Windows.Forms.Padding(3);
            this.tabHis.Size = new System.Drawing.Size(1122, 521);
            this.tabHis.TabIndex = 1;
            this.tabHis.Text = "История заказов сотрудников за 10 дней";
            this.tabHis.UseVisualStyleBackColor = true;
            // 
            // button12
            // 
            this.button12.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button12.Location = new System.Drawing.Point(636, 567);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(141, 28);
            this.button12.TabIndex = 7;
            this.button12.Text = "Отказ";
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Click += new System.EventHandler(this.button12_Click);
            // 
            // button4
            // 
            this.button4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button4.Location = new System.Drawing.Point(783, 567);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(225, 28);
            this.button4.TabIndex = 2;
            this.button4.Text = "Распечатать заказ";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.Location = new System.Drawing.Point(1016, 567);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(100, 28);
            this.button3.TabIndex = 1;
            this.button3.Text = "Выход";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click_1);
            // 
            // dgwHis
            // 
            this.dgwHis.AllowUserToAddRows = false;
            this.dgwHis.AllowUserToDeleteRows = false;
            this.dgwHis.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgwHis.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.LightYellow;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgwHis.DefaultCellStyle = dataGridViewCellStyle6;
            this.dgwHis.Location = new System.Drawing.Point(2, 7);
            this.dgwHis.Margin = new System.Windows.Forms.Padding(4);
            this.dgwHis.Name = "dgwHis";
            this.dgwHis.ReadOnly = true;
            this.dgwHis.RowHeadersVisible = false;
            this.dgwHis.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgwHis.Size = new System.Drawing.Size(1113, 555);
            this.dgwHis.TabIndex = 0;
            // 
            // tpReaderHistoryOrders
            // 
            this.tpReaderHistoryOrders.Controls.Add(this.button14);
            this.tpReaderHistoryOrders.Controls.Add(this.button11);
            this.tpReaderHistoryOrders.Controls.Add(this.bReadersHistory);
            this.tpReaderHistoryOrders.Controls.Add(this.button10);
            this.tpReaderHistoryOrders.Controls.Add(this.dgwRHis);
            this.tpReaderHistoryOrders.Location = new System.Drawing.Point(4, 25);
            this.tpReaderHistoryOrders.Name = "tpReaderHistoryOrders";
            this.tpReaderHistoryOrders.Padding = new System.Windows.Forms.Padding(3);
            this.tpReaderHistoryOrders.Size = new System.Drawing.Size(1122, 521);
            this.tpReaderHistoryOrders.TabIndex = 3;
            this.tpReaderHistoryOrders.Text = "История заказов читателей за 10 дней";
            this.tpReaderHistoryOrders.UseVisualStyleBackColor = true;
            // 
            // button14
            // 
            this.button14.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button14.Location = new System.Drawing.Point(31, 428);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(227, 28);
            this.button14.TabIndex = 7;
            this.button14.Text = "Список подбираемых книг";
            this.button14.UseVisualStyleBackColor = true;
            this.button14.Visible = false;
            this.button14.Click += new System.EventHandler(this.button14_Click);
            // 
            // button11
            // 
            this.button11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button11.Location = new System.Drawing.Point(264, 428);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(141, 28);
            this.button11.TabIndex = 6;
            this.button11.Text = "Отказ";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Visible = false;
            this.button11.Click += new System.EventHandler(this.button11_Click_1);
            // 
            // bReadersHistory
            // 
            this.bReadersHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bReadersHistory.Location = new System.Drawing.Point(783, 490);
            this.bReadersHistory.Name = "bReadersHistory";
            this.bReadersHistory.Size = new System.Drawing.Size(225, 28);
            this.bReadersHistory.TabIndex = 4;
            this.bReadersHistory.Text = "Распечатать заказ";
            this.bReadersHistory.UseVisualStyleBackColor = true;
            this.bReadersHistory.Click += new System.EventHandler(this.bReadersHistory_Click);
            // 
            // button10
            // 
            this.button10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button10.Location = new System.Drawing.Point(1016, 490);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(100, 28);
            this.button10.TabIndex = 3;
            this.button10.Text = "Выход";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // dgwRHis
            // 
            this.dgwRHis.AllowUserToAddRows = false;
            this.dgwRHis.AllowUserToDeleteRows = false;
            this.dgwRHis.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgwRHis.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.LightYellow;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgwRHis.DefaultCellStyle = dataGridViewCellStyle7;
            this.dgwRHis.Location = new System.Drawing.Point(2, 7);
            this.dgwRHis.Margin = new System.Windows.Forms.Padding(4);
            this.dgwRHis.Name = "dgwRHis";
            this.dgwRHis.ReadOnly = true;
            this.dgwRHis.RowHeadersVisible = false;
            this.dgwRHis.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgwRHis.Size = new System.Drawing.Size(1113, 476);
            this.dgwRHis.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(106, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 29);
            this.label1.TabIndex = 1;
            this.label1.Text = "label1";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 300000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(1041, 9);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(90, 23);
            this.button5.TabIndex = 2;
            this.button5.Text = "button5";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Visible = false;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // timer2
            // 
            this.timer2.Interval = 3000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(25, 9);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(75, 23);
            this.button6.TabIndex = 4;
            this.button6.Text = "button6";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Visible = false;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1213, 597);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Заказы литературы сотрудниками и читателями на сегодня";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.tabControl1.ResumeLayout(false);
            this.tpEmployeeOrders.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgwEmp)).EndInit();
            this.tpReaderOrders.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgwReaders)).EndInit();
            this.tabHis.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgwHis)).EndInit();
            this.tpReaderHistoryOrders.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgwRHis)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpEmployeeOrders;
        private System.Windows.Forms.Button bEmployeeOrder;
        private System.Windows.Forms.DataGridView dgwEmp;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TabPage tabHis;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.DataGridView dgwHis;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.TabPage tpReaderOrders;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button bPrintReaderOrder;
        private System.Windows.Forms.DataGridView dgwReaders;
        private System.Windows.Forms.TabPage tpReaderHistoryOrders;
        private System.Windows.Forms.Button bReadersHistory;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.DataGridView dgwRHis;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Button button14;
        private System.Windows.Forms.Button bRefusual;
    }
}

