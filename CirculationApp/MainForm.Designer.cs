namespace CirculationApp
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.label3 = new System.Windows.Forms.Label();
            this.bChangeAuthorization = new System.Windows.Forms.Button();
            this.tbCurrentEmployee = new System.Windows.Forms.TextBox();
            this.MainTabContainer = new System.Windows.Forms.TabControl();
            this.MainTab = new System.Windows.Forms.TabPage();
            this.lBooksCountInHall = new System.Windows.Forms.Label();
            this.bMainEmulation = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.bConfirm = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.RPhoto = new System.Windows.Forms.PictureBox();
            this.lReader = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.lTitle = new System.Windows.Forms.Label();
            this.lAuthor = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.dgvLog = new System.Windows.Forms.DataGridView();
            this.label4 = new System.Windows.Forms.Label();
            this.FormularTab = new System.Windows.Forms.TabPage();
            this.readerRightsView1 = new LibflClassLibrary.Controls.Readers.ReaderRightsView();
            this.bReaderRegistration = new System.Windows.Forms.Button();
            this.bComment = new System.Windows.Forms.Button();
            this.bRemoveResponsibility = new System.Windows.Forms.Button();
            this.bFormularSendEmail = new System.Windows.Forms.Button();
            this.bProlong = new System.Windows.Forms.Button();
            this.bSearchReaderByFIO = new System.Windows.Forms.Button();
            this.bReaderView = new System.Windows.Forms.Button();
            this.bOrdersHistory = new System.Windows.Forms.Button();
            this.bFormularFindById = new System.Windows.Forms.Button();
            this.numericUpDown3 = new System.Windows.Forms.NumericUpDown();
            this.label22 = new System.Windows.Forms.Label();
            this.lFromularNumber = new System.Windows.Forms.Label();
            this.lFormularName = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.dgvFormular = new System.Windows.Forms.DataGridView();
            this.N = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Avtor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Zaglavie = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.God = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DateIssue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DateVozv = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DateVozvFact = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Penalt = new System.Windows.Forms.DataGridViewButtonColumn();
            this.pbFormular = new System.Windows.Forms.PictureBox();
            this.ReferenceTab = new System.Windows.Forms.TabPage();
            this.bSaveReferenceToFile = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.lReferenceName = new System.Windows.Forms.Label();
            this.Statistics = new System.Windows.Forms.DataGridView();
            this.bShowReference = new System.Windows.Forms.Button();
            this.AttendanceTab = new System.Windows.Forms.TabPage();
            this.bEmulation = new System.Windows.Forms.Button();
            this.lInfoAttendance = new System.Windows.Forms.Label();
            this.lAttendance = new System.Windows.Forms.Label();
            this.AcceptBooksTab = new System.Windows.Forms.TabPage();
            this.dgvTransfer = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.bEmulationTransfer = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.выданныеКнигиНаТекущийМоментToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.просроченныеКнигиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.списокДействийОператораЗаПериодToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.отчётОтделаЗаПериодToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.отчётТекущегоОператораЗаПериодToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.всеКнигиЦентраАмериканскойКультурыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.обращаемостьКнигToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.списокКнигСКоторыхСнятаОтветственностьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.списокНарушителейСроковПользованияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.заказToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.историяЗаказовПоИнвентарномуНомеруToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.справкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HallServiceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ActiveHallOrdersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FinishedHallOrdersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pbLogo = new System.Windows.Forms.PictureBox();
            this.timerIssuedInHallCount = new System.Windows.Forms.Timer(this.components);
            this.dataGridViewButtonColumn1 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.MainTabContainer.SuspendLayout();
            this.MainTab.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RPhoto)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLog)).BeginInit();
            this.FormularTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFormular)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbFormular)).BeginInit();
            this.ReferenceTab.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Statistics)).BeginInit();
            this.AttendanceTab.SuspendLayout();
            this.AcceptBooksTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTransfer)).BeginInit();
            this.contextMenuStrip2.SuspendLayout();
            this.MainMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.SystemColors.Control;
            this.label3.Location = new System.Drawing.Point(12, 114);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(266, 17);
            this.label3.TabIndex = 6;
            this.label3.Text = "В данный момент работает сотрудник:";
            // 
            // bChangeAuthorization
            // 
            this.bChangeAuthorization.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bChangeAuthorization.Location = new System.Drawing.Point(948, 110);
            this.bChangeAuthorization.Margin = new System.Windows.Forms.Padding(4);
            this.bChangeAuthorization.Name = "bChangeAuthorization";
            this.bChangeAuthorization.Size = new System.Drawing.Size(215, 28);
            this.bChangeAuthorization.TabIndex = 7;
            this.bChangeAuthorization.Text = "Сменить сотрудника";
            this.bChangeAuthorization.UseVisualStyleBackColor = true;
            this.bChangeAuthorization.Click += new System.EventHandler(this.bChangeAuthorization_Click);
            // 
            // tbCurrentEmployee
            // 
            this.tbCurrentEmployee.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbCurrentEmployee.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.tbCurrentEmployee.Location = new System.Drawing.Point(292, 115);
            this.tbCurrentEmployee.Margin = new System.Windows.Forms.Padding(4);
            this.tbCurrentEmployee.Name = "tbCurrentEmployee";
            this.tbCurrentEmployee.ReadOnly = true;
            this.tbCurrentEmployee.Size = new System.Drawing.Size(648, 23);
            this.tbCurrentEmployee.TabIndex = 8;
            // 
            // MainTabContainer
            // 
            this.MainTabContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MainTabContainer.Controls.Add(this.MainTab);
            this.MainTabContainer.Controls.Add(this.FormularTab);
            this.MainTabContainer.Controls.Add(this.ReferenceTab);
            this.MainTabContainer.Controls.Add(this.AttendanceTab);
            this.MainTabContainer.Controls.Add(this.AcceptBooksTab);
            this.MainTabContainer.Location = new System.Drawing.Point(15, 138);
            this.MainTabContainer.Margin = new System.Windows.Forms.Padding(4);
            this.MainTabContainer.Name = "MainTabContainer";
            this.MainTabContainer.SelectedIndex = 0;
            this.MainTabContainer.Size = new System.Drawing.Size(1148, 533);
            this.MainTabContainer.TabIndex = 9;
            this.MainTabContainer.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // MainTab
            // 
            this.MainTab.Controls.Add(this.lBooksCountInHall);
            this.MainTab.Controls.Add(this.bMainEmulation);
            this.MainTab.Controls.Add(this.bCancel);
            this.MainTab.Controls.Add(this.bConfirm);
            this.MainTab.Controls.Add(this.label1);
            this.MainTab.Controls.Add(this.groupBox2);
            this.MainTab.Controls.Add(this.groupBox1);
            this.MainTab.Controls.Add(this.dgvLog);
            this.MainTab.Controls.Add(this.label4);
            this.MainTab.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.MainTab.Location = new System.Drawing.Point(4, 25);
            this.MainTab.Margin = new System.Windows.Forms.Padding(4);
            this.MainTab.Name = "MainTab";
            this.MainTab.Padding = new System.Windows.Forms.Padding(4);
            this.MainTab.Size = new System.Drawing.Size(1140, 504);
            this.MainTab.TabIndex = 0;
            this.MainTab.Text = "Приём/выдача изданий";
            this.MainTab.UseVisualStyleBackColor = true;
            // 
            // lBooksCountInHall
            // 
            this.lBooksCountInHall.AutoSize = true;
            this.lBooksCountInHall.Location = new System.Drawing.Point(703, 8);
            this.lBooksCountInHall.Name = "lBooksCountInHall";
            this.lBooksCountInHall.Size = new System.Drawing.Size(105, 17);
            this.lBooksCountInHall.TabIndex = 8;
            this.lBooksCountInHall.Text = "Выдано в зал: ";
            // 
            // bMainEmulation
            // 
            this.bMainEmulation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bMainEmulation.Location = new System.Drawing.Point(1021, 469);
            this.bMainEmulation.Name = "bMainEmulation";
            this.bMainEmulation.Size = new System.Drawing.Size(92, 24);
            this.bMainEmulation.TabIndex = 7;
            this.bMainEmulation.Text = "эмуляция";
            this.bMainEmulation.UseVisualStyleBackColor = true;
            this.bMainEmulation.Click += new System.EventHandler(this.bMainEmulation_Click);
            // 
            // bCancel
            // 
            this.bCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.bCancel.Location = new System.Drawing.Point(461, 466);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 31);
            this.bCancel.TabIndex = 6;
            this.bCancel.Text = "Отмена";
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // bConfirm
            // 
            this.bConfirm.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.bConfirm.Location = new System.Drawing.Point(331, 466);
            this.bConfirm.Name = "bConfirm";
            this.bConfirm.Size = new System.Drawing.Size(116, 31);
            this.bConfirm.TabIndex = 1;
            this.bConfirm.Text = "Подтвердить";
            this.bConfirm.UseVisualStyleBackColor = true;
            this.bConfirm.Click += new System.EventHandler(this.bConfirm_Click);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.ForeColor = System.Drawing.Color.Green;
            this.label1.Location = new System.Drawing.Point(299, 302);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(373, 31);
            this.label1.TabIndex = 5;
            this.label1.Text = "Считайте штрихкод издания";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.RPhoto);
            this.groupBox2.Controls.Add(this.lReader);
            this.groupBox2.Location = new System.Drawing.Point(409, 336);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(688, 127);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Читатель";
            // 
            // RPhoto
            // 
            this.RPhoto.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.RPhoto.Cursor = System.Windows.Forms.Cursors.Hand;
            this.RPhoto.ErrorImage = global::CirculationApp.Properties.Resources.nofoto;
            this.RPhoto.InitialImage = global::CirculationApp.Properties.Resources.nofoto;
            this.RPhoto.Location = new System.Drawing.Point(589, 16);
            this.RPhoto.Name = "RPhoto";
            this.RPhoto.Size = new System.Drawing.Size(93, 101);
            this.RPhoto.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.RPhoto.TabIndex = 6;
            this.RPhoto.TabStop = false;
            this.RPhoto.Click += new System.EventHandler(this.RPhoto_Click);
            // 
            // lReader
            // 
            this.lReader.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lReader.Location = new System.Drawing.Point(6, 19);
            this.lReader.Name = "lReader";
            this.lReader.Size = new System.Drawing.Size(565, 89);
            this.lReader.TabIndex = 5;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.lTitle);
            this.groupBox1.Controls.Add(this.lAuthor);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Location = new System.Drawing.Point(12, 336);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(367, 127);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Издание";
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label7.Location = new System.Drawing.Point(6, 72);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(115, 25);
            this.label7.TabIndex = 5;
            this.label7.Text = "Заглавие: ";
            // 
            // lTitle
            // 
            this.lTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lTitle.Location = new System.Drawing.Point(127, 72);
            this.lTitle.Name = "lTitle";
            this.lTitle.Size = new System.Drawing.Size(329, 36);
            this.lTitle.TabIndex = 5;
            // 
            // lAuthor
            // 
            this.lAuthor.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lAuthor.Location = new System.Drawing.Point(91, 32);
            this.lAuthor.Name = "lAuthor";
            this.lAuthor.Size = new System.Drawing.Size(365, 25);
            this.lAuthor.TabIndex = 5;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(6, 32);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(79, 25);
            this.label6.TabIndex = 5;
            this.label6.Text = "Автор:";
            // 
            // dgvLog
            // 
            this.dgvLog.AllowUserToAddRows = false;
            this.dgvLog.AllowUserToDeleteRows = false;
            this.dgvLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLog.Location = new System.Drawing.Point(12, 32);
            this.dgvLog.Margin = new System.Windows.Forms.Padding(4);
            this.dgvLog.MaximumSize = new System.Drawing.Size(2000, 270);
            this.dgvLog.MinimumSize = new System.Drawing.Size(0, 270);
            this.dgvLog.Name = "dgvLog";
            this.dgvLog.ReadOnly = true;
            this.dgvLog.RowHeadersVisible = false;
            this.dgvLog.Size = new System.Drawing.Size(1120, 270);
            this.dgvLog.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 15);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(124, 17);
            this.label4.TabIndex = 3;
            this.label4.Text = "Журнал событий:";
            // 
            // FormularTab
            // 
            this.FormularTab.Controls.Add(this.readerRightsView1);
            this.FormularTab.Controls.Add(this.bReaderRegistration);
            this.FormularTab.Controls.Add(this.bComment);
            this.FormularTab.Controls.Add(this.bRemoveResponsibility);
            this.FormularTab.Controls.Add(this.bFormularSendEmail);
            this.FormularTab.Controls.Add(this.bProlong);
            this.FormularTab.Controls.Add(this.bSearchReaderByFIO);
            this.FormularTab.Controls.Add(this.bReaderView);
            this.FormularTab.Controls.Add(this.bOrdersHistory);
            this.FormularTab.Controls.Add(this.bFormularFindById);
            this.FormularTab.Controls.Add(this.numericUpDown3);
            this.FormularTab.Controls.Add(this.label22);
            this.FormularTab.Controls.Add(this.lFromularNumber);
            this.FormularTab.Controls.Add(this.lFormularName);
            this.FormularTab.Controls.Add(this.label24);
            this.FormularTab.Controls.Add(this.label17);
            this.FormularTab.Controls.Add(this.dgvFormular);
            this.FormularTab.Controls.Add(this.pbFormular);
            this.FormularTab.Location = new System.Drawing.Point(4, 25);
            this.FormularTab.Name = "FormularTab";
            this.FormularTab.Padding = new System.Windows.Forms.Padding(3);
            this.FormularTab.Size = new System.Drawing.Size(1140, 504);
            this.FormularTab.TabIndex = 3;
            this.FormularTab.Text = "Формуляр читателя";
            this.FormularTab.UseVisualStyleBackColor = true;
            // 
            // readerRightsView1
            // 
            this.readerRightsView1.Location = new System.Drawing.Point(10, 69);
            this.readerRightsView1.Margin = new System.Windows.Forms.Padding(4);
            this.readerRightsView1.Name = "readerRightsView1";
            this.readerRightsView1.Size = new System.Drawing.Size(536, 97);
            this.readerRightsView1.TabIndex = 26;
            // 
            // bReaderRegistration
            // 
            this.bReaderRegistration.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bReaderRegistration.Location = new System.Drawing.Point(725, 102);
            this.bReaderRegistration.Name = "bReaderRegistration";
            this.bReaderRegistration.Size = new System.Drawing.Size(174, 64);
            this.bReaderRegistration.TabIndex = 25;
            this.bReaderRegistration.Text = "Выдать права бесплатного абонемента";
            this.bReaderRegistration.UseVisualStyleBackColor = true;
            this.bReaderRegistration.Click += new System.EventHandler(this.bReaderRegistration_Click);
            // 
            // bComment
            // 
            this.bComment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bComment.Enabled = false;
            this.bComment.Location = new System.Drawing.Point(905, 102);
            this.bComment.Name = "bComment";
            this.bComment.Size = new System.Drawing.Size(225, 31);
            this.bComment.TabIndex = 23;
            this.bComment.Text = "Комментарий о читателе";
            this.bComment.UseVisualStyleBackColor = true;
            this.bComment.Click += new System.EventHandler(this.bComment_Click);
            // 
            // bRemoveResponsibility
            // 
            this.bRemoveResponsibility.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bRemoveResponsibility.Location = new System.Drawing.Point(725, 69);
            this.bRemoveResponsibility.Name = "bRemoveResponsibility";
            this.bRemoveResponsibility.Size = new System.Drawing.Size(175, 27);
            this.bRemoveResponsibility.TabIndex = 22;
            this.bRemoveResponsibility.Text = "Снять ответственность";
            this.bRemoveResponsibility.UseVisualStyleBackColor = true;
            this.bRemoveResponsibility.Click += new System.EventHandler(this.bRemoveResponsibility_Click);
            // 
            // bFormularSendEmail
            // 
            this.bFormularSendEmail.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bFormularSendEmail.Enabled = false;
            this.bFormularSendEmail.Location = new System.Drawing.Point(726, 41);
            this.bFormularSendEmail.Name = "bFormularSendEmail";
            this.bFormularSendEmail.Size = new System.Drawing.Size(174, 26);
            this.bFormularSendEmail.TabIndex = 21;
            this.bFormularSendEmail.Text = "Отослать email";
            this.bFormularSendEmail.UseVisualStyleBackColor = true;
            this.bFormularSendEmail.Click += new System.EventHandler(this.bFormularSendEmail_Click);
            // 
            // bProlong
            // 
            this.bProlong.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bProlong.Location = new System.Drawing.Point(905, 69);
            this.bProlong.Name = "bProlong";
            this.bProlong.Size = new System.Drawing.Size(228, 27);
            this.bProlong.TabIndex = 19;
            this.bProlong.Text = "Продлить выделенное";
            this.bProlong.UseVisualStyleBackColor = true;
            this.bProlong.Click += new System.EventHandler(this.bProlong_Click);
            // 
            // bSearchReaderByFIO
            // 
            this.bSearchReaderByFIO.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bSearchReaderByFIO.Location = new System.Drawing.Point(905, 6);
            this.bSearchReaderByFIO.Name = "bSearchReaderByFIO";
            this.bSearchReaderByFIO.Size = new System.Drawing.Size(228, 33);
            this.bSearchReaderByFIO.TabIndex = 18;
            this.bSearchReaderByFIO.Text = "Поиск читателя по фамилии";
            this.bSearchReaderByFIO.UseVisualStyleBackColor = true;
            this.bSearchReaderByFIO.Click += new System.EventHandler(this.bSearchReaderByFIO_Click);
            // 
            // bReaderView
            // 
            this.bReaderView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bReaderView.Location = new System.Drawing.Point(905, 40);
            this.bReaderView.Name = "bReaderView";
            this.bReaderView.Size = new System.Drawing.Size(228, 27);
            this.bReaderView.TabIndex = 16;
            this.bReaderView.Text = "Просмотр сведений о читателе";
            this.bReaderView.UseVisualStyleBackColor = true;
            this.bReaderView.Click += new System.EventHandler(this.bReaderView_Click);
            // 
            // bOrdersHistory
            // 
            this.bOrdersHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bOrdersHistory.Enabled = false;
            this.bOrdersHistory.Location = new System.Drawing.Point(725, 6);
            this.bOrdersHistory.Name = "bOrdersHistory";
            this.bOrdersHistory.Size = new System.Drawing.Size(175, 33);
            this.bOrdersHistory.TabIndex = 15;
            this.bOrdersHistory.Text = "История выдач";
            this.bOrdersHistory.UseVisualStyleBackColor = true;
            this.bOrdersHistory.Click += new System.EventHandler(this.bOrdersHistory_Click);
            // 
            // bFormularFindById
            // 
            this.bFormularFindById.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bFormularFindById.Location = new System.Drawing.Point(749, 477);
            this.bFormularFindById.Name = "bFormularFindById";
            this.bFormularFindById.Size = new System.Drawing.Size(230, 23);
            this.bFormularFindById.TabIndex = 9;
            this.bFormularFindById.Text = "Найти";
            this.bFormularFindById.UseVisualStyleBackColor = true;
            this.bFormularFindById.Click += new System.EventHandler(this.bFormularFindById_Click);
            // 
            // numericUpDown3
            // 
            this.numericUpDown3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.numericUpDown3.Location = new System.Drawing.Point(593, 478);
            this.numericUpDown3.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numericUpDown3.Name = "numericUpDown3";
            this.numericUpDown3.Size = new System.Drawing.Size(150, 23);
            this.numericUpDown3.TabIndex = 8;
            // 
            // label22
            // 
            this.label22.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label22.Location = new System.Drawing.Point(6, 477);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(581, 24);
            this.label22.TabIndex = 5;
            this.label22.Text = "Считайте штрихкод или введите номер читательского билета:";
            // 
            // lFromularNumber
            // 
            this.lFromularNumber.AutoSize = true;
            this.lFromularNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lFromularNumber.Location = new System.Drawing.Point(289, 9);
            this.lFromularNumber.Name = "lFromularNumber";
            this.lFromularNumber.Size = new System.Drawing.Size(0, 17);
            this.lFromularNumber.TabIndex = 4;
            // 
            // lFormularName
            // 
            this.lFormularName.AutoSize = true;
            this.lFormularName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lFormularName.Location = new System.Drawing.Point(108, 47);
            this.lFormularName.Name = "lFormularName";
            this.lFormularName.Size = new System.Drawing.Size(0, 17);
            this.lFormularName.TabIndex = 4;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label24.Location = new System.Drawing.Point(3, 3);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(280, 24);
            this.label24.TabIndex = 3;
            this.label24.Text = "Номер читательского билета:";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label17.Location = new System.Drawing.Point(3, 40);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(99, 24);
            this.label17.TabIndex = 3;
            this.label17.Text = "Читатель:";
            // 
            // dgvFormular
            // 
            this.dgvFormular.AllowUserToAddRows = false;
            this.dgvFormular.AllowUserToDeleteRows = false;
            this.dgvFormular.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvFormular.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFormular.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.N,
            this.Avtor,
            this.Zaglavie,
            this.God,
            this.DateIssue,
            this.DateVozv,
            this.DateVozvFact,
            this.Penalt});
            this.dgvFormular.Location = new System.Drawing.Point(6, 173);
            this.dgvFormular.MultiSelect = false;
            this.dgvFormular.Name = "dgvFormular";
            this.dgvFormular.ReadOnly = true;
            this.dgvFormular.RowHeadersVisible = false;
            this.dgvFormular.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvFormular.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvFormular.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvFormular.Size = new System.Drawing.Size(1128, 298);
            this.dgvFormular.TabIndex = 2;
            // 
            // N
            // 
            this.N.HeaderText = "№";
            this.N.Name = "N";
            this.N.ReadOnly = true;
            this.N.Width = 30;
            // 
            // Avtor
            // 
            this.Avtor.HeaderText = "Автор";
            this.Avtor.Name = "Avtor";
            this.Avtor.ReadOnly = true;
            this.Avtor.Width = 150;
            // 
            // Zaglavie
            // 
            this.Zaglavie.HeaderText = "Заглавие";
            this.Zaglavie.Name = "Zaglavie";
            this.Zaglavie.ReadOnly = true;
            this.Zaglavie.Width = 250;
            // 
            // God
            // 
            this.God.HeaderText = "Год издания";
            this.God.Name = "God";
            this.God.ReadOnly = true;
            this.God.Width = 75;
            // 
            // DateIssue
            // 
            this.DateIssue.HeaderText = "Дата выдачи";
            this.DateIssue.Name = "DateIssue";
            this.DateIssue.ReadOnly = true;
            this.DateIssue.Width = 80;
            // 
            // DateVozv
            // 
            this.DateVozv.HeaderText = "Предпола гаемая дата возврата";
            this.DateVozv.Name = "DateVozv";
            this.DateVozv.ReadOnly = true;
            this.DateVozv.Width = 80;
            // 
            // DateVozvFact
            // 
            this.DateVozvFact.HeaderText = "Фактичес кая дата возврата";
            this.DateVozvFact.Name = "DateVozvFact";
            this.DateVozvFact.ReadOnly = true;
            this.DateVozvFact.Width = 80;
            // 
            // Penalt
            // 
            this.Penalt.HeaderText = "Нарушение";
            this.Penalt.Name = "Penalt";
            this.Penalt.ReadOnly = true;
            this.Penalt.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Penalt.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Penalt.Width = 85;
            // 
            // pbFormular
            // 
            this.pbFormular.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbFormular.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbFormular.ErrorImage = global::CirculationApp.Properties.Resources.nofoto;
            this.pbFormular.InitialImage = global::CirculationApp.Properties.Resources.nofoto;
            this.pbFormular.Location = new System.Drawing.Point(564, 6);
            this.pbFormular.Name = "pbFormular";
            this.pbFormular.Size = new System.Drawing.Size(146, 160);
            this.pbFormular.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbFormular.TabIndex = 20;
            this.pbFormular.TabStop = false;
            this.pbFormular.Click += new System.EventHandler(this.pbFormular_Click);
            // 
            // ReferenceTab
            // 
            this.ReferenceTab.Controls.Add(this.bSaveReferenceToFile);
            this.ReferenceTab.Controls.Add(this.flowLayoutPanel1);
            this.ReferenceTab.Controls.Add(this.Statistics);
            this.ReferenceTab.Controls.Add(this.bShowReference);
            this.ReferenceTab.Location = new System.Drawing.Point(4, 25);
            this.ReferenceTab.Margin = new System.Windows.Forms.Padding(4);
            this.ReferenceTab.Name = "ReferenceTab";
            this.ReferenceTab.Padding = new System.Windows.Forms.Padding(4);
            this.ReferenceTab.Size = new System.Drawing.Size(1140, 504);
            this.ReferenceTab.TabIndex = 1;
            this.ReferenceTab.Text = "Справка";
            this.ReferenceTab.UseVisualStyleBackColor = true;
            // 
            // bSaveReferenceToFile
            // 
            this.bSaveReferenceToFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bSaveReferenceToFile.Enabled = false;
            this.bSaveReferenceToFile.Location = new System.Drawing.Point(724, 459);
            this.bSaveReferenceToFile.Name = "bSaveReferenceToFile";
            this.bSaveReferenceToFile.Size = new System.Drawing.Size(234, 28);
            this.bSaveReferenceToFile.TabIndex = 5;
            this.bSaveReferenceToFile.Text = "Сохранить в файл";
            this.bSaveReferenceToFile.UseVisualStyleBackColor = true;
            this.bSaveReferenceToFile.Click += new System.EventHandler(this.bSaveReferenceToFile_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.Controls.Add(this.lReferenceName);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(7, 9);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1126, 28);
            this.flowLayoutPanel1.TabIndex = 4;
            // 
            // lReferenceName
            // 
            this.lReferenceName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lReferenceName.AutoSize = true;
            this.lReferenceName.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lReferenceName.Location = new System.Drawing.Point(3, 0);
            this.lReferenceName.Name = "lReferenceName";
            this.lReferenceName.Size = new System.Drawing.Size(0, 26);
            this.lReferenceName.TabIndex = 3;
            // 
            // Statistics
            // 
            this.Statistics.AllowUserToAddRows = false;
            this.Statistics.AllowUserToDeleteRows = false;
            this.Statistics.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Statistics.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Statistics.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.Statistics.Location = new System.Drawing.Point(7, 43);
            this.Statistics.Name = "Statistics";
            this.Statistics.ReadOnly = true;
            this.Statistics.RowHeadersVisible = false;
            this.Statistics.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.Statistics.Size = new System.Drawing.Size(1126, 409);
            this.Statistics.TabIndex = 1;
            this.Statistics.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.Statistics_CellMouseDoubleClick);
            this.Statistics.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.Statistics_ColumnHeaderMouseClick);
            // 
            // bShowReference
            // 
            this.bShowReference.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bShowReference.Enabled = false;
            this.bShowReference.Location = new System.Drawing.Point(964, 458);
            this.bShowReference.Name = "bShowReference";
            this.bShowReference.Size = new System.Drawing.Size(169, 29);
            this.bShowReference.TabIndex = 0;
            this.bShowReference.Text = "Вывести в таблицу";
            this.bShowReference.UseVisualStyleBackColor = true;
            this.bShowReference.Click += new System.EventHandler(this.bShowReference_Click);
            // 
            // AttendanceTab
            // 
            this.AttendanceTab.Controls.Add(this.bEmulation);
            this.AttendanceTab.Controls.Add(this.lInfoAttendance);
            this.AttendanceTab.Controls.Add(this.lAttendance);
            this.AttendanceTab.Location = new System.Drawing.Point(4, 25);
            this.AttendanceTab.Name = "AttendanceTab";
            this.AttendanceTab.Padding = new System.Windows.Forms.Padding(3);
            this.AttendanceTab.Size = new System.Drawing.Size(1140, 504);
            this.AttendanceTab.TabIndex = 4;
            this.AttendanceTab.Text = "Учёт посещаемости";
            this.AttendanceTab.UseVisualStyleBackColor = true;
            // 
            // bEmulation
            // 
            this.bEmulation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bEmulation.Location = new System.Drawing.Point(1041, 467);
            this.bEmulation.Name = "bEmulation";
            this.bEmulation.Size = new System.Drawing.Size(93, 31);
            this.bEmulation.TabIndex = 11;
            this.bEmulation.Text = "эмуляция";
            this.bEmulation.UseVisualStyleBackColor = true;
            this.bEmulation.Click += new System.EventHandler(this.bEmulation_Click);
            // 
            // lInfoAttendance
            // 
            this.lInfoAttendance.AutoSize = true;
            this.lInfoAttendance.Location = new System.Drawing.Point(346, 302);
            this.lInfoAttendance.Name = "lInfoAttendance";
            this.lInfoAttendance.Size = new System.Drawing.Size(425, 17);
            this.lInfoAttendance.TabIndex = 10;
            this.lInfoAttendance.Text = "Считайте штрихкод читателя, чтобы увеличить посещаемость";
            // 
            // lAttendance
            // 
            this.lAttendance.AutoSize = true;
            this.lAttendance.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lAttendance.Location = new System.Drawing.Point(240, 98);
            this.lAttendance.Name = "lAttendance";
            this.lAttendance.Size = new System.Drawing.Size(668, 31);
            this.lAttendance.TabIndex = 9;
            this.lAttendance.Text = "На сегодня посещаемость составляет: 0 человек(а)";
            // 
            // AcceptBooksTab
            // 
            this.AcceptBooksTab.Controls.Add(this.dgvTransfer);
            this.AcceptBooksTab.Controls.Add(this.label2);
            this.AcceptBooksTab.Controls.Add(this.bEmulationTransfer);
            this.AcceptBooksTab.Location = new System.Drawing.Point(4, 25);
            this.AcceptBooksTab.Name = "AcceptBooksTab";
            this.AcceptBooksTab.Padding = new System.Windows.Forms.Padding(3);
            this.AcceptBooksTab.Size = new System.Drawing.Size(1140, 504);
            this.AcceptBooksTab.TabIndex = 5;
            this.AcceptBooksTab.Text = "Приём книг на кафедру из хранения/в хранение с кафедры";
            this.AcceptBooksTab.UseVisualStyleBackColor = true;
            // 
            // dgvTransfer
            // 
            this.dgvTransfer.AllowUserToAddRows = false;
            this.dgvTransfer.AllowUserToDeleteRows = false;
            this.dgvTransfer.AllowUserToOrderColumns = true;
            this.dgvTransfer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvTransfer.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTransfer.Location = new System.Drawing.Point(6, 37);
            this.dgvTransfer.MultiSelect = false;
            this.dgvTransfer.Name = "dgvTransfer";
            this.dgvTransfer.RowHeadersVisible = false;
            this.dgvTransfer.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTransfer.Size = new System.Drawing.Size(1128, 428);
            this.dgvTransfer.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(202, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(689, 31);
            this.label2.TabIndex = 2;
            this.label2.Text = "Считайте штрихкод, чтобы принять книгу на кафедру";
            // 
            // bEmulationTransfer
            // 
            this.bEmulationTransfer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bEmulationTransfer.Location = new System.Drawing.Point(1031, 471);
            this.bEmulationTransfer.Name = "bEmulationTransfer";
            this.bEmulationTransfer.Size = new System.Drawing.Size(103, 23);
            this.bEmulationTransfer.TabIndex = 1;
            this.bEmulationTransfer.Text = "Эмуляция";
            this.bEmulationTransfer.UseVisualStyleBackColor = true;
            this.bEmulationTransfer.Click += new System.EventHandler(this.bEmulationTransfer_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label10.Location = new System.Drawing.Point(440, 28);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(384, 42);
            this.label10.TabIndex = 11;
            this.label10.Text = "Книговыдача ВГБИЛ";
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.выданныеКнигиНаТекущийМоментToolStripMenuItem,
            this.просроченныеКнигиToolStripMenuItem,
            this.списокДействийОператораЗаПериодToolStripMenuItem,
            this.отчётОтделаЗаПериодToolStripMenuItem,
            this.отчётТекущегоОператораЗаПериодToolStripMenuItem,
            this.всеКнигиЦентраАмериканскойКультурыToolStripMenuItem,
            this.обращаемостьКнигToolStripMenuItem,
            this.списокКнигСКоторыхСнятаОтветственностьToolStripMenuItem,
            this.списокНарушителейСроковПользованияToolStripMenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(331, 202);
            // 
            // выданныеКнигиНаТекущийМоментToolStripMenuItem
            // 
            this.выданныеКнигиНаТекущийМоментToolStripMenuItem.Name = "выданныеКнигиНаТекущийМоментToolStripMenuItem";
            this.выданныеКнигиНаТекущийМоментToolStripMenuItem.Size = new System.Drawing.Size(330, 22);
            this.выданныеКнигиНаТекущийМоментToolStripMenuItem.Text = "Выданные книги на текущий момент";
            this.выданныеКнигиНаТекущийМоментToolStripMenuItem.Click += new System.EventHandler(this.выданныеКнигиНаТекущийМоментToolStripMenuItem_Click);
            // 
            // просроченныеКнигиToolStripMenuItem
            // 
            this.просроченныеКнигиToolStripMenuItem.Name = "просроченныеКнигиToolStripMenuItem";
            this.просроченныеКнигиToolStripMenuItem.Size = new System.Drawing.Size(330, 22);
            this.просроченныеКнигиToolStripMenuItem.Text = "Просроченные книги";
            this.просроченныеКнигиToolStripMenuItem.Click += new System.EventHandler(this.просроченныеКнигиToolStripMenuItem_Click);
            // 
            // списокДействийОператораЗаПериодToolStripMenuItem
            // 
            this.списокДействийОператораЗаПериодToolStripMenuItem.Name = "списокДействийОператораЗаПериодToolStripMenuItem";
            this.списокДействийОператораЗаПериодToolStripMenuItem.Size = new System.Drawing.Size(330, 22);
            this.списокДействийОператораЗаПериодToolStripMenuItem.Text = "Список действий оператора за период";
            this.списокДействийОператораЗаПериодToolStripMenuItem.Click += new System.EventHandler(this.списокДействийОператораЗаПериодToolStripMenuItem_Click);
            // 
            // отчётОтделаЗаПериодToolStripMenuItem
            // 
            this.отчётОтделаЗаПериодToolStripMenuItem.Name = "отчётОтделаЗаПериодToolStripMenuItem";
            this.отчётОтделаЗаПериодToolStripMenuItem.Size = new System.Drawing.Size(330, 22);
            this.отчётОтделаЗаПериодToolStripMenuItem.Text = "Отчёт отдела за период";
            this.отчётОтделаЗаПериодToolStripMenuItem.Click += new System.EventHandler(this.отчётОтделаЗаПериодToolStripMenuItem_Click);
            // 
            // отчётТекущегоОператораЗаПериодToolStripMenuItem
            // 
            this.отчётТекущегоОператораЗаПериодToolStripMenuItem.Name = "отчётТекущегоОператораЗаПериодToolStripMenuItem";
            this.отчётТекущегоОператораЗаПериодToolStripMenuItem.Size = new System.Drawing.Size(330, 22);
            this.отчётТекущегоОператораЗаПериодToolStripMenuItem.Text = "Отчёт текущего оператора за период";
            this.отчётТекущегоОператораЗаПериодToolStripMenuItem.Click += new System.EventHandler(this.отчётТекущегоОператораЗаПериодToolStripMenuItem_Click);
            // 
            // всеКнигиЦентраАмериканскойКультурыToolStripMenuItem
            // 
            this.всеКнигиЦентраАмериканскойКультурыToolStripMenuItem.Name = "всеКнигиЦентраАмериканскойКультурыToolStripMenuItem";
            this.всеКнигиЦентраАмериканскойКультурыToolStripMenuItem.Size = new System.Drawing.Size(330, 22);
            this.всеКнигиЦентраАмериканскойКультурыToolStripMenuItem.Text = "Все книги ЦАК";
            this.всеКнигиЦентраАмериканскойКультурыToolStripMenuItem.Click += new System.EventHandler(this.всеКнигиЦентраАмериканскойКультурыToolStripMenuItem_Click);
            // 
            // обращаемостьКнигToolStripMenuItem
            // 
            this.обращаемостьКнигToolStripMenuItem.Name = "обращаемостьКнигToolStripMenuItem";
            this.обращаемостьКнигToolStripMenuItem.Size = new System.Drawing.Size(330, 22);
            this.обращаемостьКнигToolStripMenuItem.Text = "Обращаемость книг";
            this.обращаемостьКнигToolStripMenuItem.Click += new System.EventHandler(this.обращаемостьКнигToolStripMenuItem_Click);
            // 
            // списокКнигСКоторыхСнятаОтветственностьToolStripMenuItem
            // 
            this.списокКнигСКоторыхСнятаОтветственностьToolStripMenuItem.Name = "списокКнигСКоторыхСнятаОтветственностьToolStripMenuItem";
            this.списокКнигСКоторыхСнятаОтветственностьToolStripMenuItem.Size = new System.Drawing.Size(330, 22);
            this.списокКнигСКоторыхСнятаОтветственностьToolStripMenuItem.Text = "Список книг, с которых снята ответственность";
            this.списокКнигСКоторыхСнятаОтветственностьToolStripMenuItem.Click += new System.EventHandler(this.списокКнигСКоторыхСнятаОтветственностьToolStripMenuItem_Click);
            // 
            // списокНарушителейСроковПользованияToolStripMenuItem
            // 
            this.списокНарушителейСроковПользованияToolStripMenuItem.Name = "списокНарушителейСроковПользованияToolStripMenuItem";
            this.списокНарушителейСроковПользованияToolStripMenuItem.Size = new System.Drawing.Size(330, 22);
            this.списокНарушителейСроковПользованияToolStripMenuItem.Text = "Список нарушителей сроков пользования";
            this.списокНарушителейСроковПользованияToolStripMenuItem.Click += new System.EventHandler(this.списокНарушителейСроковПользованияToolStripMenuItem_Click);
            // 
            // MainMenu
            // 
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.заказToolStripMenuItem,
            this.справкаToolStripMenuItem});
            this.MainMenu.Location = new System.Drawing.Point(0, 0);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Size = new System.Drawing.Size(1176, 24);
            this.MainMenu.TabIndex = 12;
            this.MainMenu.Text = "menuStrip1";
            // 
            // заказToolStripMenuItem
            // 
            this.заказToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.историяЗаказовПоИнвентарномуНомеруToolStripMenuItem});
            this.заказToolStripMenuItem.Name = "заказToolStripMenuItem";
            this.заказToolStripMenuItem.Size = new System.Drawing.Size(49, 20);
            this.заказToolStripMenuItem.Text = "Заказ";
            // 
            // историяЗаказовПоИнвентарномуНомеруToolStripMenuItem
            // 
            this.историяЗаказовПоИнвентарномуНомеруToolStripMenuItem.Name = "историяЗаказовПоИнвентарномуНомеруToolStripMenuItem";
            this.историяЗаказовПоИнвентарномуНомеруToolStripMenuItem.Size = new System.Drawing.Size(310, 22);
            this.историяЗаказовПоИнвентарномуНомеруToolStripMenuItem.Text = "История заказов по инвентарному номеру";
            this.историяЗаказовПоИнвентарномуНомеруToolStripMenuItem.Click += new System.EventHandler(this.InvNumberOrderHistoryToolStripMenuItem_Click);
            // 
            // справкаToolStripMenuItem
            // 
            this.справкаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.HallServiceToolStripMenuItem,
            this.ActiveHallOrdersToolStripMenuItem,
            this.FinishedHallOrdersToolStripMenuItem});
            this.справкаToolStripMenuItem.Name = "справкаToolStripMenuItem";
            this.справкаToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.справкаToolStripMenuItem.Text = "Справка";
            // 
            // HallServiceToolStripMenuItem
            // 
            this.HallServiceToolStripMenuItem.Name = "HallServiceToolStripMenuItem";
            this.HallServiceToolStripMenuItem.Size = new System.Drawing.Size(274, 22);
            this.HallServiceToolStripMenuItem.Text = "Обслуживание в залах";
            this.HallServiceToolStripMenuItem.Click += new System.EventHandler(this.HallServiceToolStripMenuItem_Click);
            // 
            // ActiveHallOrdersToolStripMenuItem
            // 
            this.ActiveHallOrdersToolStripMenuItem.Name = "ActiveHallOrdersToolStripMenuItem";
            this.ActiveHallOrdersToolStripMenuItem.Size = new System.Drawing.Size(274, 22);
            this.ActiveHallOrdersToolStripMenuItem.Text = "Активные заказы текущего зала";
            this.ActiveHallOrdersToolStripMenuItem.Click += new System.EventHandler(this.ActiveHallOrdersToolStripMenuItem_Click);
            // 
            // FinishedHallOrdersToolStripMenuItem
            // 
            this.FinishedHallOrdersToolStripMenuItem.Name = "FinishedHallOrdersToolStripMenuItem";
            this.FinishedHallOrdersToolStripMenuItem.Size = new System.Drawing.Size(274, 22);
            this.FinishedHallOrdersToolStripMenuItem.Text = "Завершённые заказы текущего зала";
            this.FinishedHallOrdersToolStripMenuItem.Click += new System.EventHandler(this.FinishedHallOrdersToolStripMenuItem_Click);
            // 
            // pbLogo
            // 
            this.pbLogo.Image = global::CirculationApp.Properties.Resources.Artboard2;
            this.pbLogo.Location = new System.Drawing.Point(15, 27);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.Size = new System.Drawing.Size(203, 84);
            this.pbLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbLogo.TabIndex = 10;
            this.pbLogo.TabStop = false;
            // 
            // timerIssuedInHallCount
            // 
            this.timerIssuedInHallCount.Interval = 20000;
            this.timerIssuedInHallCount.Tick += new System.EventHandler(this.timerIssuedInHallCount_Tick);
            // 
            // dataGridViewButtonColumn1
            // 
            this.dataGridViewButtonColumn1.HeaderText = "Нарушение";
            this.dataGridViewButtonColumn1.Name = "dataGridViewButtonColumn1";
            this.dataGridViewButtonColumn1.ReadOnly = true;
            this.dataGridViewButtonColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewButtonColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dataGridViewButtonColumn1.Width = 85;
            // 
            // MainForm
            // 
            this.AcceptButton = this.bConfirm;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1176, 684);
            this.Controls.Add(this.MainMenu);
            this.Controls.Add(this.pbLogo);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.MainTabContainer);
            this.Controls.Add(this.tbCurrentEmployee);
            this.Controls.Add(this.bChangeAuthorization);
            this.Controls.Add(this.label3);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.MainMenu;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.Text = "Книговыдача ВГБИЛ";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.MainTabContainer.ResumeLayout(false);
            this.MainTab.ResumeLayout(false);
            this.MainTab.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.RPhoto)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLog)).EndInit();
            this.FormularTab.ResumeLayout(false);
            this.FormularTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFormular)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbFormular)).EndInit();
            this.ReferenceTab.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Statistics)).EndInit();
            this.AttendanceTab.ResumeLayout(false);
            this.AttendanceTab.PerformLayout();
            this.AcceptBooksTab.ResumeLayout(false);
            this.AcceptBooksTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTransfer)).EndInit();
            this.contextMenuStrip2.ResumeLayout(false);
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button bChangeAuthorization;
        public System.Windows.Forms.TextBox tbCurrentEmployee;
        private System.Windows.Forms.TabControl MainTabContainer;
        private System.Windows.Forms.TabPage MainTab;
        private System.Windows.Forms.TabPage ReferenceTab;
        private System.Windows.Forms.DataGridView dgvTransfer;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.Button bConfirm;
        private System.Windows.Forms.Label lReader;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lTitle;
        private System.Windows.Forms.Label lAuthor;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DataGridView dgvLog;
        private System.Windows.Forms.PictureBox pbLogo;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button bShowReference;
        private System.Windows.Forms.DataGridView Statistics;
        private System.Windows.Forms.Label lReferenceName;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.TabPage ttabPage4;
        private System.Windows.Forms.Label lFormularName;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Button bFormularFindById;
        private System.Windows.Forms.NumericUpDown numericUpDown3;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label lFromularNumber;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.DataGridViewTextBoxColumn N;
        private System.Windows.Forms.DataGridViewTextBoxColumn Avtor;
        private System.Windows.Forms.DataGridViewTextBoxColumn Zaglavie;
        private System.Windows.Forms.DataGridViewTextBoxColumn God;
        private System.Windows.Forms.DataGridViewTextBoxColumn DateIssue;
        private System.Windows.Forms.DataGridViewTextBoxColumn DateVozv;
        private System.Windows.Forms.DataGridViewTextBoxColumn DateVozvFact;
        private System.Windows.Forms.DataGridViewButtonColumn Penalt;
        private System.Windows.Forms.Button bSaveReferenceToFile;
        private System.Windows.Forms.Button bMainEmulation;
        private System.Windows.Forms.Button bOrdersHistory;
        private System.Windows.Forms.Button bReaderView;
        private System.Windows.Forms.Button bSearchReaderByFIO;
        public System.Windows.Forms.DataGridView dgvFormular;
        private System.Windows.Forms.DataGridViewButtonColumn dataGridViewButtonColumn1;
        private System.Windows.Forms.Button bProlong;
        private System.Windows.Forms.PictureBox RPhoto;
        private System.Windows.Forms.PictureBox pbFormular;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem выданныеКнигиНаТекущийМоментToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem просроченныеКнигиToolStripMenuItem;
        private System.Windows.Forms.Button bFormularSendEmail;
        private System.Windows.Forms.ToolStripMenuItem списокДействийОператораЗаПериодToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem отчётОтделаЗаПериодToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem всеКнигиЦентраАмериканскойКультурыToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem отчётТекущегоОператораЗаПериодToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem обращаемостьКнигToolStripMenuItem;
        private System.Windows.Forms.Button bRemoveResponsibility;
        private System.Windows.Forms.ToolStripMenuItem списокКнигСКоторыхСнятаОтветственностьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem списокНарушителейСроковПользованияToolStripMenuItem;
        private System.Windows.Forms.TabPage AttendanceTab;
        private System.Windows.Forms.TabPage FormularTab;
        private System.Windows.Forms.Button bEmulation;
        private System.Windows.Forms.Label lInfoAttendance;
        private System.Windows.Forms.Label lAttendance;
        private System.Windows.Forms.Button bComment;
        private System.Windows.Forms.Button bReaderRegistration;
        private LibflClassLibrary.Controls.Readers.ReaderRightsView readerRightsView1;
        private System.Windows.Forms.TabPage AcceptBooksTab;
        private System.Windows.Forms.Button bEmulationTransfer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lBooksCountInHall;
        private System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.ToolStripMenuItem заказToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem историяЗаказовПоИнвентарномуНомеруToolStripMenuItem;
        private System.Windows.Forms.Timer timerIssuedInHallCount;
        private System.Windows.Forms.ToolStripMenuItem справкаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem HallServiceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ActiveHallOrdersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem FinishedHallOrdersToolStripMenuItem;
        //private System.Windows.Forms.DataGridView dgvTransfer;
        //private Circulation.BRIT_SOVETDataSetTableAdapters.ZAKAZTableAdapter zAKAZTableAdapter;
        //private CrystalDecisions.Windows.Forms.CrystalReportViewer crystalReportViewer1;
        //private CrystalReport1 CrystalReport11;
        //private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        //private CrystalDecisions.Windows.Forms.CrystalReportViewer crystalReportViewer1;
        //private CrystalDecisions.CrystalReports.Engine.ReportDocument reportDocument1;
        //private CrystalReport1 CrystalReport11;
        //private CrystalReport2 CrystalReport21;
        //private CrystalReport3 CrystalReport31;
    }
}

