using System;
using System.Windows;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Globalization;
using System.Xml;
using System.Windows.Forms.VisualStyles;
using System.Threading;
using System.Net.Mail;
using System.Net;
using System.Text.RegularExpressions;
using System.IO.Ports;
using System.IO;
using LibflClassLibrary.Readers.ReadersRight;
using LibflClassLibrary.Controls.Readers;
using LibflClassLibrary.Controls;
using LibflClassLibrary.BJUsers;
using CirculationACC;
using LibflClassLibrary.Circulation;
using LibflClassLibrary.Books.BJBooks.BJExemplars;
using LibflClassLibrary.Books.BJBooks;
using Utilities;
using LibflClassLibrary.Readers;
using ALISAPI.Errors;
using LibflClassLibrary.ALISAPI.Errors;
using LibflClassLibrary.ExportToVufind;
using LibflClassLibrary.Books;

namespace CirculationApp
{
    public delegate void HeaderClick(object sender, DataGridViewCellMouseEventArgs ev);

    public partial class MainForm : Form
    {
        Department department;
        CirculationInfo ci = new CirculationInfo();

        //public int EmpID;
        SerialPort port;
        private BJUserInfo bjUser;
        public ExtGui.RoundProgress RndPrg;
        public MainForm()
        {

            InitializeComponent();

            this.StartPosition = FormStartPosition.CenterScreen;

            fBJAuthorization au = new fBJAuthorization("BJVVV");
            au.ShowDialog();
            if (au.User != null)
            {
                bjUser = au.User;
                this.tbCurrentEmployee.Text = $"{bjUser.FIO}; {bjUser.SelectedUserStatus.DepName}";
                ShowLog();
            }
            department = new Department(bjUser);
            //Form1.Scanned += new ScannedEventHandler(Form1_Scanned);
            this.bConfirm.Enabled = false;
            this.bCancel.Enabled = false;
            label4.Text = "������ ������� " + DateTime.Now.ToShortDateString() + ":";

            port = new SerialPort("COM1", 9600, Parity.None, 8, StopBits.One);
            port.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
            port.Handshake = Handshake.RequestToSend;
            port.NewLine = Convert.ToChar(13).ToString();

            try
            {
                port.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            ShowIssuedInHallCount();

        }
        public delegate void ScanFuncDelegate(string data);
        

        void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string FromPort = "";
            FromPort = port.ReadLine();
            FromPort = FromPort.Trim();
            port.DiscardInBuffer();

            //������ ���������-��������, ����� ����� ����������� ����������� ���� ����������
            ScanFuncDelegate ScanDelegate;
            ScanDelegate = new ScanFuncDelegate(Form1_Scanned);
            this.Invoke(ScanDelegate, new object[] { FromPort });
        }


        void Form1_Scanned(string fromport)
        {
            string g = MainTabContainer.SelectedTab.ToString();
            switch (MainTabContainer.SelectedTab.Text)
            {
                case "�������� ��������":
                    #region formular
                    ReaderInfo reader = ReaderInfo.GetReaderByBar(fromport);
                    FillFormular(reader);

                    #endregion
                    break;

                case "����/������ �������":
                    #region priem

                    Circulate(fromport);
                    break;
                    #endregion

                

                case "���� ������������":
                    #region ���� ������������
                    try
                    {
                        department.AttendanceScan(fromport);
                    }
                    catch (Exception ex)
                    {
                        ALISError error = ALISErrorList._list.Find(x => x.Code == ex.Message);
                        MessageBox.Show(error.Message);
                        return;
                    }
                    ShowAttendance();
                    break;
                case "���� ���� �� ������� �� ��������/� �������� � �������":
                    RecieveBookFromInBookKeeping(fromport);
                    ShowBookTransfer(bjUser);
                    break;
                #endregion

            }
        }

        private void ShowAttendance()
        {
            lAttendance.Text = "�� ������� ������������ ����������: " + department.GetAttendance() + " �������(�)";
        }

        private void RecieveBookFromInBookKeeping(string fromport)
        {
            
            //BJExemplarInfo exemplar = BJExemplarInfo.GetExemplarByBar(fromport);
            ExemplarBase exemplar = ExemplarFactory.CreateExemplar(fromport);
            if (exemplar == null)
            {
                MessageBox.Show("����� �� �������.");
                return;
            }
            OrderInfo order = ci.FindOrderByExemplar(exemplar);
            try
            { 
                if (bjUser.SelectedUserStatus.DepName.ToLower().Contains("�����������"))
                {
                    ci.RecieveBookInBookkeeping(order, bjUser);
                    MessageBox.Show("����� ������� ������� � ��������!");
                    return;
                }
                else
                {
                    ci.RecieveBookFromBookkeeping(order, bjUser);
                    MessageBox.Show("����� ������� ������� �� �������!");
                    return;
                }
            }
            catch (Exception ex)
            {
                ALISError error = ALISErrorList._list.Find(x => x.Code == ex.Message);
                MessageBox.Show(error.Message);
                return;
            }
        }

        private void Circulate(string fromPort)
        {

            switch (department.Circulate(fromPort))
            {
                case 0://����� ���� ������. ����� ������� � � �����
                    try
                    {
                        department.RecieveBook(fromPort);
                    }
                    catch (Exception ex)
                    {
                        ALISError error = ALISErrorList._list.Find(x => x.Code == ex.Message);
                        MessageBox.Show(error.Message);
                        return;
                    }
                    CancelIssue();
                    break;
                case 1:
                    MessageBox.Show("�������� �� ������ �� � ���� ��������� �� � ���� ����!");
                    break;
                case 2:
                    MessageBox.Show("�������� �������� ��������, � ������ �������� �������!");
                    break;
                case 3:
                    MessageBox.Show("�������� �������� �������, � ������ �������� ��������!");
                    break;
                case 4:
                    lAuthor.Text = department.ScannedExemplar.Author;
                    lTitle.Text = department.ScannedExemplar.Title;
                    bCancel.Enabled = true;
                    label1.Text = "�������� �������� ��������";
                    break;
                case 5:
                    lReader.Text = department.ScannedReader.FamilyName + " " + department.ScannedReader.Name + " " + department.ScannedReader.FatherName;
                    RPhoto.Image = department.ScannedReader.Photo;
                    bConfirm.Enabled = true;
                    this.AcceptButton = bConfirm;
                    bConfirm.Focus();
                    label1.Text = "����������� ��������";
                    break;

            }
            ShowLog();
            ShowIssuedInHallCount();
        }

        private void ShowIssuedInHallCount()
        {
            lBooksCountInHall.Text = $"������ � ���: {department.GetIssuedInHallBooksCount()} ����";
        }

        private void AttendanceScan(string fromport)
        {
            if (!ReaderVO.IsReader(fromport))
            {
                MessageBox.Show("�������� �������� ��������!", "��������!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            ReaderVO reader = new ReaderVO(fromport);

            if (!reader.IsAlreadyMarked())
            {
                //department.AddAttendance(reader);
                //lAttendance.Text = "�� ������� ������������ ����������: " + department.GetAttendance() + " �������(�)";
            }
            else
            {
                MessageBox.Show("���� �������� ��� ������� ������� ��� �������!");
                return;
            }
        }

        public void FillFormular(ReaderInfo reader)
        {
            if (reader == null)
            {
                MessageBox.Show("�������� �� ������!");
                return;
            }
            FillFormularGrid(reader);

            readerRightsView1.Init(reader.NumberReader);

        }
        public void FillFormularGrid(ReaderInfo reader)
        {
            lFormularName.Text = reader.FamilyName + " " + reader.Name + " " + reader.FatherName;
            lFromularNumber.Text = reader.NumberReader.ToString();
            List<OrderInfo> formular = ci.GetOrders(reader.NumberReader);
            formular = formular.FindAll(x => x.StatusName.In(new[] {    CirculationStatuses.InReserve.Value,
                                                                        CirculationStatuses.IssuedAtHome.Value,
                                                                        CirculationStatuses.IssuedInHall.Value,
                                                                        }));

            KeyValuePair<string, string>[] columns =
{
                    new KeyValuePair<string, string> ( "id", "id"),
                    new KeyValuePair<string, string> ( "bar", "��������"),
                    new KeyValuePair<string, string> ( "inv", "���. �����"),
                    new KeyValuePair<string, string> ( "author", "�����"),
                    new KeyValuePair<string, string> ( "title", "��������"),
                    new KeyValuePair<string, string> ( "issueDate", "���� ������"),
                    new KeyValuePair<string, string> ( "returnDate", "�������������� ���� ��������"),
                    new KeyValuePair<string, string> ( "issDep", "��� ������"),
                    new KeyValuePair<string, string> ( "retDep", "��� ��������"),
                    new KeyValuePair<string, string> ( "cipher", "�������������� ����"),
                    new KeyValuePair<string, string> ( "baseName", "����"),
                    //new KeyValuePair<string, string> ( "prolongedTimes", "�������� ���"),
                    new KeyValuePair<string, string> ( "status", "������"),
                    new KeyValuePair<string, string> ( "rack", "�������"),
                    //new KeyValuePair<string, string> ( "issueType", "��� ������"),
                };
            dgvFormular.Columns.Clear();
            foreach (var c in columns)
                dgvFormular.Columns.Add(c.Key, c.Value);

            dgvFormular.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvFormular.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;

            dgvFormular.Columns["id"].Visible = false;

            dgvFormular.Columns["bar"].Width = 100;
            dgvFormular.Columns["inv"].Width = 100;
            dgvFormular.Columns["author"].Width = 150;
            dgvFormular.Columns["title"].Width = 250;
            dgvFormular.Columns["issueDate"].DefaultCellStyle.Format = "dd.MM.yyyy";
            dgvFormular.Columns["issueDate"].Width = 80;
            dgvFormular.Columns["returnDate"].DefaultCellStyle.Format = "dd.MM.yyyy";
            dgvFormular.Columns["returnDate"].Width = 80;
            dgvFormular.Columns["cipher"].Width = 70;
            dgvFormular.Columns["baseName"].Width = 70;
            dgvFormular.Columns["status"].Width = 100;
            dgvFormular.Columns["rack"].Width = 100;
            dgvFormular.Columns["issDep"].Width = 100;
            dgvFormular.Columns["retDep"].Width = 100;

            foreach (OrderInfo order in formular)
            {
                dgvFormular.Rows.Add();
                var row = dgvFormular.Rows[dgvFormular.Rows.Count - 1];
                //BJExemplarInfo exemplar = BJExemplarInfo.GetExemplarByIdData(order.ExemplarId, order.Fund);
                ExemplarBase exemplar = ExemplarFactory.CreateExemplar(order.ExemplarId, order.Fund);
                //BJBookInfo book = BJBookInfo.GetBookInfoByPIN(exemplar.IDMAIN, exemplar.Fund);
                row.Cells["id"].Value = order.OrderId;
                row.Cells["bar"].Value = exemplar.Bar;//.Fields["899$w"].ToString();
                row.Cells["inv"].Value = exemplar.InventoryNumber;//.Fields["899$p"].ToString();
                row.Cells["author"].Value = exemplar.Author;//book.Fields["700$a"].ToString();
                row.Cells["title"].Value = exemplar.Title;//book.Fields["200$a"].ToString();
                row.Cells["issueDate"].Value = order.IssueDate;
                row.Cells["returnDate"].Value = order.ReturnDate;
                row.Cells["cipher"].Value = exemplar.Cipher;
                row.Cells["baseName"].Value = BookBase.GetRusFundName(exemplar.Fund);
                row.Cells["status"].Value = order.StatusName;
                row.Cells["rack"].Value = (exemplar is BJExemplarInfo) ? ((BJExemplarInfo)exemplar).Fields["899$c"].ToString() : "";
                row.Cells["issDep"].Value = string.IsNullOrEmpty(order.IssueDep) ?  "" : KeyValueMapping.LocationCodeToName[int.Parse(order.IssueDep)];
                row.Cells["retDep"].Value = string.IsNullOrEmpty(order.ReturnDep) ? "" : KeyValueMapping.LocationCodeToName[int.Parse(order.ReturnDep)];
            }

            //Formular.DataSource = reader.GetFormular();

            //dgvFormular.Columns["num"].HeaderText = "��";
            //dgvFormular.Columns["num"].Width = 40;
            //dgvFormular.Columns["bar"].HeaderText = "��������";
            //dgvFormular.Columns["bar"].Width = 80;
            //dgvFormular.Columns["avt"].HeaderText = "�����";
            //dgvFormular.Columns["avt"].Width = 200;
            //dgvFormular.Columns["tit"].HeaderText = "��������";
            //dgvFormular.Columns["tit"].Width = 400;
            //dgvFormular.Columns["iss"].HeaderText = "���� ������";
            //dgvFormular.Columns["iss"].Width = 80;
            //dgvFormular.Columns["ret"].HeaderText = "�������������� ���� ��������";
            //dgvFormular.Columns["ret"].Width = 110;
            //dgvFormular.Columns["shifr"].HeaderText = "�������������� ����";
            //dgvFormular.Columns["shifr"].Width = 90;
            //dgvFormular.Columns["idiss"].Visible = false;
            //dgvFormular.Columns["idr"].Visible = false;
            //dgvFormular.Columns["DATE_ISSUE"].Visible = false;
            //dgvFormular.Columns["fund"].HeaderText = "����";
            //dgvFormular.Columns["fund"].Width = 50;
            //dgvFormular.Columns["prolonged"].HeaderText = "��������, ���";
            //dgvFormular.Columns["prolonged"].Width = 80;
            //dgvFormular.Columns["IsAtHome"].HeaderText = "��� ������";
            //dgvFormular.Columns["IsAtHome"].Width = 80;
            //dgvFormular.Columns["rack"].HeaderText = "�������";
            //dgvFormular.Columns["rack"].Width = 80;
            pbFormular.Image = reader.Photo;
            foreach (DataGridViewRow r in dgvFormular.Rows)
            {
                DateTime ret = (DateTime)r.Cells["returnDate"].Value;
                if (ret < DateTime.Now)
                {
                    r.DefaultCellStyle.BackColor = Color.Tomato;
                }
            }
        }
        private void bConfirm_Click(object sender, EventArgs e)
        {

            //����� �������� ��������, ��� ����� 10 ���� ��� �� ��� ����. �������� ��� ���?
            //� ��� ��������, ���� �� ����� ��� ���� � ���.



            department.IssueBookToReader();
            CancelIssue();
            ShowLog();
        }
        private void bCancel_Click(object sender, EventArgs e)
        {
            CancelIssue();
        }
        private void CancelIssue()
        {
            this.lAuthor.Text = "";
            this.lTitle.Text = "";
            this.lReader.Text = "";
            label1.Text = "�������� �������� �������";
            bConfirm.Enabled = false;
            bCancel.Enabled = false;
            RPhoto.Image = null;
            department.ExpectedBar = ExpectingAction.WaitingBook;
        }
        private void ShowLog()
        {
            List<OrderFlowInfo> flow = ci.GetOrdersFlow(bjUser.SelectedUserStatus.UnifiedLocationCode);

            KeyValuePair<string, string>[] columns =
            {
                new KeyValuePair<string, string> ( "time", "�����"),
                new KeyValuePair<string, string> ( "bar", "��������"),
                new KeyValuePair<string, string> ( "title", "�������"),
                new KeyValuePair<string, string> ( "reader", "��������"),
                new KeyValuePair<string, string> ( "status", "��������"),
                new KeyValuePair<string, string> ( "baseName", "����"),
                //new KeyValuePair<string, string> ( "issueType", "��� ������"),
            };
            dgvLog.Columns.Clear();
            foreach (var c in columns)
                dgvLog.Columns.Add(c.Key, c.Value);

            dgvLog.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvLog.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;

            dgvLog.Columns["time"].DefaultCellStyle.Format = "HH:mm";
            dgvLog.Columns["bar"].Width = 100;
            dgvLog.Columns["title"].Width = 300;
            dgvLog.Columns["reader"].Width = 80;
            dgvLog.Columns["status"].Width = 180;
            dgvLog.Columns["baseName"].Width = 70;
            //dgvLog.Columns["issueType"].Width = 80;
            int i = 0;
            foreach(OrderFlowInfo fi in flow)
            {

                dgvLog.Rows.Add();
                var row = dgvLog.Rows[dgvLog.Rows.Count - 1];
                OrderInfo oi = ci.GetOrder(fi.OrderId);
                //BJExemplarInfo exemplar = BJExemplarInfo.GetExemplarByIdData(oi.ExemplarId, oi.Fund);
                ExemplarBase exemplar = ExemplarFactory.CreateExemplar(oi.ExemplarId, oi.Fund);
                BJBookInfo book = BJBookInfo.GetBookInfoByPIN(exemplar.BookId);

                row.Cells["time"].Value = fi.Changed;
                row.Cells["bar"].Value = exemplar.Bar;
                //string title = string.IsNullOrEmpty(book.Fields["700$a"].ToString()) ? "<���>" : book.Fields["700$a"].ToString();
                row.Cells["title"].Value = exemplar.AuthorTitle;// $"{title}; {book.Fields["200$a"].ToString()}";
                row.Cells["reader"].Value = oi.ReaderId;
                row.Cells["status"].Value = fi.StatusName;
                row.Cells["baseName"].Value = BookBase.GetRusFundName(oi.Fund);
                //row.Cells["issueType"].Value = exemplar.ExemplarAccess.Access.In(new [] {1000,1006}) ? "�� ���" : "� ���";
                if (i++ == 30) break;
            }

            foreach (DataGridViewColumn c in dgvLog.Columns)
            {
                c.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            
        }


        private void bFormularFindById_Click(object sender, EventArgs e)
        {
            ReaderInfo reader = ReaderInfo.GetReader("a@a.a");
            try
            {
                reader = ReaderInfo.GetReader((int)numericUpDown3.Value);//new ReaderVO((int)numericUpDown3.Value);
            }
            catch
            {
                MessageBox.Show("�������� �� ������");
                return;
            }
            if (reader == null)
            {
                MessageBox.Show("�������� �� ������!");
                return;
            }
            FillFormular(reader);

        }
        private void bProlong_Click(object sender, EventArgs e)
        {
            if (dgvFormular.SelectedRows.Count == 0)
            {
                MessageBox.Show("�������� ������!");
                return;
            }
            try
            {
                ci.ProlongOrder((int)dgvFormular.SelectedRows[0].Cells["id"].Value);
            }
            catch (Exception ex)
            {
                ALISError error = ALISErrorList._list.Find(x => x.Code == ex.Message);
                MessageBox.Show(error.Message);
                return;
            }
            FillFormular(ReaderInfo.GetReader(int.Parse(lFromularNumber.Text)));
        }

        private void bChangeAuthorization_Click(object sender, EventArgs e)
        {
            fBJAuthorization au = new fBJAuthorization("BJVVV");
            au.ShowDialog();
            if (au.User != null)
            {
                bjUser = au.User;
                this.tbCurrentEmployee.Text = $"{bjUser.FIO}; {bjUser.SelectedUserStatus.DepName}";
            }
            department = new Department(bjUser);
            tabControl1_SelectedIndexChanged(sender, e);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (MainTabContainer.SelectedTab.Text)
            {
                case "����/������ �������":
                    ShowLog();
                    ShowIssuedInHallCount();
                    //CancelIssue();
                    label1.Enabled = true;
                    
                    //label1.Text = "�������� �������� �������";
                    break;
                case "�������":
                    label1.Enabled = false;
                    break;
                case "�������� ��������":
                    lFromularNumber.Text = "";
                    lFormularName.Text = "";
                    dgvFormular.Columns.Clear();
                    AcceptButton = this.bFormularFindById;
                    pbFormular.Image = null;
                    readerRightsView1.Clear();
                    break;
                case "���� ������������":
                    ShowAttendance();
                    break;
                case "���� ���� �� ������� �� ��������/� �������� � �������":
                    ShowBookTransfer(bjUser);
                    break;

            }
        }

        private void ShowBookTransfer(BJUserInfo bjUser)
        {
            List<OrderInfo> transferOrders;// = ci.GetOrders(bjUser.SelectedUserStatus.UnifiedLocationCode);

            if (bjUser.SelectedUserStatus.DepName.ToLower().Contains("�����������"))
            {
                transferOrders = ci.GetOrders(CirculationStatuses.ForReturnToBookStorage.Value);
                label2.Text = "�������� ��������, ����� ������� ����� � �������������";
            }   
            else
            {
                transferOrders = ci.GetOrders(CirculationStatuses.EmployeeLookingForBook.Value);
                label2.Text = "�������� ��������, ����� ������� ����� �� �������";
            }

            KeyValuePair<string, string>[] columns =
            {
                new KeyValuePair<string, string> ( "pin", "���"),
                new KeyValuePair<string, string> ( "title", "�������"),
                new KeyValuePair<string, string> ( "bar", "��������"),
                new KeyValuePair<string, string> ( "reader", "��������"),
                new KeyValuePair<string, string> ( "location", "���������������"),
                new KeyValuePair<string, string> ( "issDep", "����� ������"),
                new KeyValuePair<string, string> ( "retDep", "����� ��������"),
                new KeyValuePair<string, string> ( "inv", "���. �����"),
                new KeyValuePair<string, string> ( "status", "������ ������"),
            };
            dgvTransfer.Columns.Clear();
            foreach (var c in columns)
                dgvTransfer.Columns.Add(c.Key, c.Value);

            dgvTransfer.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvTransfer.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;

            dgvTransfer.Columns["pin"].Width = 120;
            dgvTransfer.Columns["title"].Width = 200;
            dgvTransfer.Columns["bar"].Width = 100;
            dgvTransfer.Columns["reader"].Width = 80;
            dgvTransfer.Columns["location"].Width = 150;
            dgvTransfer.Columns["issDep"].Width = 150;
            dgvTransfer.Columns["retDep"].Width = 150;
            dgvTransfer.Columns["inv"].Width = 80;
            dgvTransfer.Columns["status"].Width = 80;

            foreach (OrderInfo oi in transferOrders)
            {
                dgvTransfer.Rows.Add();
                var row = dgvTransfer.Rows[dgvTransfer.Rows.Count - 1];
                ExemplarBase exemplar = ExemplarFactory.CreateExemplar(oi.ExemplarId, oi.Fund);
                if (exemplar == null)
                {
                    row.Cells["pin"].Value = oi.BookId;
                    row.Cells["title"].Value = "��������� ����������� � ����";
                    row.Cells["reader"].Value = oi.ReaderId;
                    row.Cells["status"].Value = oi.StatusName;
                    continue;
                }

                BookBase book = BookFactory.CreateBookByPin(exemplar.BookId);
                row.Cells["pin"].Value = oi.BookId;
                row.Cells["bar"].Value = exemplar.Bar;
                row.Cells["title"].Value = exemplar.AuthorTitle;
                row.Cells["reader"].Value = oi.ReaderId;
                row.Cells["location"].Value = exemplar.Location;
                row.Cells["issDep"].Value = KeyValueMapping.LocationCodeToName[oi.IssuingDepartmentId];
                row.Cells["retDep"].Value = string.IsNullOrEmpty(oi.ReturnDep) ? "" : KeyValueMapping.LocationCodeToName[int.Parse(oi.ReturnDep)];
                row.Cells["inv"].Value = exemplar.InventoryNumber;
                row.Cells["status"].Value = oi.StatusName;
                //row.Cells["issueType"].Value = exemplar.ExemplarAccess.Access.In(new [] {1000,1006}) ? "�� ���" : "� ���";
            }

            if (bjUser.SelectedUserStatus.DepName.ToLower().Contains("�����������"))
            {
                dgvTransfer.Columns["issDep"].Visible = false;
            }
            else
            {
                dgvTransfer.Columns["retDep"].Visible = false;

            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (this.bjUser == null)//��� �� �������� ����������
            {
                MessageBox.Show("�� �� ������������! ��������� ����������� ���� ������", "��������!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }


        private void bShowReference_Click(object sender, EventArgs e)
        {
            bSaveReferenceToFile.Enabled = false;
            int x = this.Left + bShowReference.Left;
            int y = this.Top + bShowReference.Top + MainTabContainer.Top + 60;
            contextMenuStrip2.Show(x, y);
        }


       

        
        public void autoinc(DataGridView dgv)
        {
            int i = 0;
            foreach (DataGridViewRow row in dgv.Rows)
            {
                row.Cells[0].Value = ++i;
            }
        }
        
        private void Statistics_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

            if (lReferenceName.Text.Contains("�������"))
            {
                foreach (DataGridViewRow r in Statistics.Rows)
                {
                    if (r.Cells[10].Value.ToString() == "true")
                    {
                        r.DefaultCellStyle.BackColor = Color.Yellow;
                    }
                }
            }
            if (lReferenceName.Text.Contains("�������"))
            {
                //foreach (DataGridViewRow r in Statistics.Rows)
                //{
                //    object value = r.Cells[5].Value;
                //    if (Convert.ToBoolean(value) == true)
                //    {
                //        r.DefaultCellStyle.BackColor = Color.Yellow;
                //    }
                //}
            }
            autoinc(Statistics);
            
        }

        System.Drawing.Printing.PrintDocument pd;
        DataGridViewPrinter prin;
        DataGridView dgw2;
        private bool SetupThePrinting()
        {
            PrintDialog MyPrintDialog = new PrintDialog();
            MyPrintDialog.AllowCurrentPage = false;
            MyPrintDialog.AllowPrintToFile = false;
            MyPrintDialog.AllowSelection = false;
            MyPrintDialog.AllowSomePages = false;
            MyPrintDialog.PrintToFile = false;
            MyPrintDialog.ShowHelp = false;
            MyPrintDialog.ShowNetwork = false;

            if (MyPrintDialog.ShowDialog() != DialogResult.OK)
                return false;
            pd = new System.Drawing.Printing.PrintDocument();
            pd.DocumentName = "������ �����";
            //pd.PrinterSettings = MyPrintDialog.PrinterSettings;
            pd.DefaultPageSettings = pd.PrinterSettings.DefaultPageSettings;
            pd.DefaultPageSettings.Margins = new System.Drawing.Printing.Margins(20, 20, 20, 20);
            pd.DefaultPageSettings.Landscape = true;
            pd.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(pd_PrintPage);
            prin = new DataGridViewPrinter(dgw2, pd, true, false, string.Empty, new Font("Tahoma", 18), Color.Black, false);
            

            return true;
        }

        void pd_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            bool more = prin.DrawDataGridView(e.Graphics);
            if (more == true)
                e.HasMorePages = true;
        }
        class Span
        {
            public DateTime start;
            public DateTime end;
        }
        Span MyDateSpan;

        private void bSaveReferenceToFile_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)Statistics.DataSource;

            StringBuilder fileContent = new StringBuilder();

            foreach (DataGridViewColumn dc in Statistics.Columns)
            {
                fileContent.Append(dc.HeaderText + ";");
            }

            fileContent.Replace(";", System.Environment.NewLine, fileContent.Length - 1, 1);



            foreach (DataRow dr in dt.Rows)
            {

                foreach (var column in dr.ItemArray)
                {
                    fileContent.Append("\"" + column.ToString() + "\";");
                }

                fileContent.Replace(";", System.Environment.NewLine, fileContent.Length - 1, 1);
            }

            string tmp = lReferenceName.Text + "_" + DateTime.Now.ToString("hh:mm:ss.nnn") + ".csv";
            tmp = lReferenceName.Text + "_" + DateTime.Now.Ticks.ToString() + ".csv";
            SaveFileDialog sd = new SaveFileDialog();
            sd.Title = "��������� � ����";
            sd.Filter = "csv files (*.csv)|*.csv";
            sd.FilterIndex = 1;
            sd.FileName = tmp;
            if (sd.ShowDialog() == DialogResult.OK)
            {
                System.IO.File.WriteAllText(sd.FileName, fileContent.ToString(), Encoding.UTF8);
            }
        }

        public string emul;
        public string pass;
        private void bMainEmulation_Click(object sender, EventArgs e)
        {
            //ReaderInfo reader = ReaderInfo.GetReader(189245);
            //MessageBox.Show(reader.IsEnteredThroughAccessControlSystem().ToString());
            //reader = ReaderInfo.GetReader(194405);
            //MessageBox.Show(reader.IsEnteredThroughAccessControlSystem().ToString());

            ParolEmulation f20 = new ParolEmulation(this);
            f20.ShowDialog();
            if (pass == "aa")
            {
                pass = "";
                Emulation f19 = new Emulation(this);
                f19.ShowDialog();
                Form1_Scanned(f19.emul);
            }
        }

        private void bOrdersHistory_Click(object sender, EventArgs e)
        {
            if (lFromularNumber.Text == "")
            {
                MessageBox.Show("������� ����� ��� �������� �������� ��������!");
                return;
            }
            ReaderVO reader = new ReaderVO(int.Parse(lFromularNumber.Text));
            History f7 = new History(reader);
            f7.ShowDialog();
        }

        private void bReaderView_Click(object sender, EventArgs e)
        {
            if (lFromularNumber.Text == "")
            {
                MessageBox.Show("������� ����� ��� �������� �������� ��������!");
                return;
            }
            ReaderVO reader = new ReaderVO(int.Parse(lFromularNumber.Text));
            ReaderInformation f9 = new ReaderInformation(reader,this);
            f9.ShowDialog();
        }

        private void bSearchReaderByFIO_Click(object sender, EventArgs e)
        {
            //����� �������� �� �������
            FindReaderBySurname f16 = new FindReaderBySurname(this);
            f16.ShowDialog();
        }
        
        
       
       

        private void Statistics_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex == -1) return;
            if ((lReferenceName.Text.IndexOf("������ ������������ ���������� �� ������� ������") != -1) )
            {
                MainTabContainer.SelectedIndex = 1;
                numericUpDown3.Value = int.Parse(Statistics.Rows[e.RowIndex].Cells[3].Value.ToString());
                bFormularFindById_Click(sender, new EventArgs());
            }
            if (lReferenceName.Text.Contains("�������"))
            {
                MainTabContainer.SelectedIndex = 1;
                numericUpDown3.Value = int.Parse(Statistics.Rows[e.RowIndex].Cells[2].Value.ToString());
                bFormularFindById_Click(sender, new EventArgs());
            }
        }


        private void RPhoto_Click(object sender, EventArgs e)
        {
            ViewFullSizePhoto fullsize = new ViewFullSizePhoto(RPhoto.Image);
            fullsize.ShowDialog();
        }

        private void pbFormular_Click(object sender, EventArgs e)
        {
            ViewFullSizePhoto fullsize = new ViewFullSizePhoto(pbFormular.Image);
            fullsize.ShowDialog();

        }

        private void ����������������������������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Statistics.Columns.Clear();
            //Statistics.Columns.Add("NN", "� �/�");
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            Statistics.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            //DatePeriod f3 = new DatePeriod();
            //f3.ShowDialog();
            lReferenceName.Text = "������ �������� ���������� �� ������� ������ ";
            DBReference dbref = new DBReference();
            Statistics.DataSource = dbref.GetAllIssuedBook();
            if (this.Statistics.Rows.Count == 0)
            {
                this.Statistics.Columns.Clear();
                MessageBox.Show("��� �������� ����!");
                return;
            }

            autoinc(Statistics);
            Statistics.Columns[0].Width = 40;
            Statistics.Columns[0].HeaderText = "��";
            Statistics.Columns[1].HeaderText = "��������";
            Statistics.Columns[1].Width = 270;
            Statistics.Columns[2].HeaderText = "�����";
            Statistics.Columns[2].Width = 140;
            Statistics.Columns[3].HeaderText = "����� ������ ������� ������";
            Statistics.Columns[3].Width = 70;
            Statistics.Columns[4].HeaderText = "�������";
            Statistics.Columns[4].Width = 100;
            Statistics.Columns[5].HeaderText = "���";
            Statistics.Columns[5].Width = 90;
            Statistics.Columns[6].HeaderText = "��������";
            Statistics.Columns[6].Width = 100;
            Statistics.Columns[7].HeaderText = "��������";
            Statistics.Columns[7].Width = 80;
            Statistics.Columns[8].HeaderText = "���� ������";
            Statistics.Columns[8].ValueType = typeof(DateTime);
            Statistics.Columns[8].DefaultCellStyle.Format = "dd.MM.yyyy";
            Statistics.Columns[8].Width = 85;
            Statistics.Columns[9].HeaderText = "������ �������� ���� ��������";
            Statistics.Columns[9].DefaultCellStyle.Format = "dd.MM.yyyy";
            Statistics.Columns[9].Width = 85;
            Statistics.Columns[10].Visible = false;
            Statistics.Columns[11].HeaderText = "�������������� ����";
            Statistics.Columns[11].Width = 100;
            Statistics.Columns[12].HeaderText = "����";
            Statistics.Columns[12].Width = 50;
            Statistics.Columns[13].HeaderText = "��� ������";
            Statistics.Columns[13].Width = 50;

            bSaveReferenceToFile.Enabled = true;
        }

        private void �����������������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Statistics.Columns.Clear();
            //Statistics.Columns.Add("NN", "� �/�");
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            Statistics.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            //DatePeriod f3 = new DatePeriod();
            //f3.ShowDialog();
            lReferenceName.Text = "������ ������������ ���������� �� ������� ������";
            DBReference dbref = new DBReference();
            Statistics.DataSource = dbref.GetAllOverdueBook();
            if (this.Statistics.Rows.Count == 0)
            {
                this.Statistics.Columns.Clear();
                MessageBox.Show("��� �������� ����!");
                return;
            }

            autoinc(Statistics);
            Statistics.Columns[0].HeaderText = "��";
            Statistics.Columns[0].Width = 40;
            Statistics.Columns[1].HeaderText = "��������";
            Statistics.Columns[1].Width = 240;
            Statistics.Columns[2].HeaderText = "�����";
            Statistics.Columns[2].Width = 120;
            Statistics.Columns[3].HeaderText = "����� ������ ������� ������";
            Statistics.Columns[3].Width = 70;
            Statistics.Columns[4].HeaderText = "�������";
            Statistics.Columns[4].Width = 100;
            Statistics.Columns[5].HeaderText = "���";
            Statistics.Columns[5].Width = 80;
            Statistics.Columns[6].HeaderText = "��������";
            Statistics.Columns[6].Width = 80;
            Statistics.Columns[7].HeaderText = "��������";
            Statistics.Columns[7].Width = 75;
            Statistics.Columns[8].HeaderText = "���� ������";
            Statistics.Columns[8].ValueType = typeof(DateTime);
            Statistics.Columns[8].DefaultCellStyle.Format = "dd.MM.yyyy";
            Statistics.Columns[8].Width = 85;
            Statistics.Columns[9].HeaderText = "������ �������� ���� ��������";
            Statistics.Columns[9].DefaultCellStyle.Format = "dd.MM.yyyy";
            Statistics.Columns[9].Width = 85;
            Statistics.Columns[10].Visible = false;
            Statistics.Columns[10].ValueType = typeof(bool);
            Statistics.Columns[11].HeaderText = "���� ��������� �������� email";
            Statistics.Columns[11].DefaultCellStyle.Format = "dd.MM.yyyy";
            Statistics.Columns[11].Width = 85;
            Statistics.Columns[12].HeaderText = "�������������� ����";
            Statistics.Columns[12].Width = 85;
            Statistics.Columns[13].HeaderText = "����";
            Statistics.Columns[13].Width = 50;
            Statistics.Columns[14].HeaderText = "�������";
            Statistics.Columns[14].Width = 70;
            foreach (DataGridViewRow r in Statistics.Rows)
            {
                object value = r.Cells[10].Value;
                if (Convert.ToBoolean(value) == true)
                {
                    r.DefaultCellStyle.BackColor = Color.Yellow;
                }
            }
            bSaveReferenceToFile.Enabled = true;
        }

        private void bFormularSendEmail_Click(object sender, EventArgs e)
        {
            if (lFromularNumber.Text == "")
            {
                MessageBox.Show("������� ����� ��� �������� �������� ��������!");
                return;
            }
            ReaderInfo reader = ReaderInfo.GetReader(int.Parse(lFromularNumber.Text)); 
            EmailSending es = new EmailSending(reader, bjUser);
            es.ShowDialog();
        }

        private void �������������������������������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Statistics.Columns != null)
                Statistics.Columns.Clear();
            fDatePeriod f3 = new fDatePeriod();
            f3.ShowDialog();
            lReferenceName.Text = "";
            lReferenceName.Text = "������ �������� ��������� �� ������ � " + f3.StartDate.ToString("dd.MM.yyyy") + " �� " + f3.EndDate.ToString("dd.MM.yyyy") + ": ";
            DBGeneral dbg = new DBGeneral();
            
            try
            {
                Statistics.DataSource = dbg.GetOperatorActions(f3.StartDate, f3.EndDate, bjUser.Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Statistics.Columns.Clear();
                return;
            }
            autoinc(Statistics);
            Statistics.Columns[0].HeaderText = "��";
            Statistics.Columns[1].Width = 250;
            Statistics.Columns[1].HeaderText = "��������";
            Statistics.Columns[2].HeaderText = "����";
            Statistics.Columns[2].Width = 200;
            autoinc(Statistics);
        }

        private void �������������������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Statistics.Columns != null)
                Statistics.Columns.Clear();
            fDatePeriod f3 = new fDatePeriod();
            f3.ShowDialog();
            lReferenceName.Text = "����� ������ �� ������ � " + f3.StartDate.ToString("dd.MM.yyyy") + " �� " + f3.EndDate.ToString("dd.MM.yyyy") + ": ";
            DBGeneral dbg = new DBGeneral();

            try
            {
                Statistics.DataSource = dbg.GetDepReport(f3.StartDate, f3.EndDate);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Statistics.Columns.Clear();
                return;
            }
            autoinc(Statistics);
            Statistics.Columns[0].HeaderText = "��";
            Statistics.Columns[1].Width = 250;
            Statistics.Columns[1].HeaderText = "������������";
            Statistics.Columns[2].HeaderText = "����������";
            Statistics.Columns[2].Width = 200;
            autoinc(Statistics);
        }
        private void ������������������������������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Statistics.Columns != null)
                Statistics.Columns.Clear();
            fDatePeriod f3 = new fDatePeriod();
            f3.ShowDialog();
            lReferenceName.Text = "����� �������� ��������� �� ������ � " + f3.StartDate.ToString("dd.MM.yyyy") + " �� " + f3.EndDate.ToString("dd.MM.yyyy") + ": ";
            DBGeneral dbg = new DBGeneral();

            try
            {
                Statistics.DataSource = dbg.GetOprReport(f3.StartDate, f3.EndDate, bjUser.Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Statistics.Columns.Clear();
                return;
            }
            autoinc(Statistics);
            Statistics.Columns[0].HeaderText = "��";
            Statistics.Columns[1].Width = 250;
            Statistics.Columns[1].HeaderText = "������������";
            Statistics.Columns[2].HeaderText = "����������";
            Statistics.Columns[2].Width = 200;
            autoinc(Statistics);
        }
        private void ����������������������������������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Statistics.Columns.Clear();
            //Statistics.Columns.Add("NN", "� �/�");
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            Statistics.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            //DatePeriod f3 = new DatePeriod();
            //f3.ShowDialog();
            lReferenceName.Text = "������ ���� ���������� ��� + �� ";
            DBReference dbref = new DBReference();
            Statistics.DataSource = dbref.GetAllBooks();
            if (this.Statistics.Rows.Count == 0)
            {
                this.Statistics.Columns.Clear();
                MessageBox.Show("��� �������� ����!");
                return;
            }

            autoinc(Statistics);
            Statistics.Columns[0].Width = 70;
            Statistics.Columns[0].HeaderText = "��";
            Statistics.Columns[1].HeaderText = "��������";
            Statistics.Columns[1].Width = 500;
            Statistics.Columns[2].HeaderText = "�����";
            Statistics.Columns[2].Width = 200;
            Statistics.Columns[3].HeaderText = "��������";
            Statistics.Columns[3].Width = 100;
            Statistics.Columns[4].HeaderText = "����";
            Statistics.Columns[4].Width = 150;
            Statistics.Columns[5].HeaderText = "��������";
            Statistics.Columns[5].Width = 150;
            Statistics.Columns[6].HeaderText = "�������";
            Statistics.Columns[6].Width = 150;

            bSaveReferenceToFile.Enabled = true;
        }

        private void ����������������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Statistics.Columns.Clear();
            //Statistics.Columns.Add("NN", "� �/�");
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            Statistics.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            //DatePeriod f3 = new DatePeriod();
            //f3.ShowDialog();
            lReferenceName.Text = "������������ ���������� ��� ";
            DBReference dbref = new DBReference();
            Statistics.DataSource = dbref.GetBookNegotiability();
            if (this.Statistics.Rows.Count == 0)
            {
                this.Statistics.Columns.Clear();
                MessageBox.Show("��� �������� ����!");
                return;
            }

            autoinc(Statistics);
            Statistics.Columns[0].Width = 70;
            Statistics.Columns[0].HeaderText = "��";
            Statistics.Columns[1].HeaderText = "��������";
            Statistics.Columns[1].Width = 500;
            Statistics.Columns[2].HeaderText = "�����";
            Statistics.Columns[2].Width = 200;
            Statistics.Columns[3].HeaderText = "��������";
            Statistics.Columns[3].Width = 100;
            Statistics.Columns[4].HeaderText = "������������";
            Statistics.Columns[4].Width = 100;
            Statistics.Columns[5].HeaderText = "����";
            Statistics.Columns[5].Width = 70;

            bSaveReferenceToFile.Enabled = true;
        }

        private void bRemoveResponsibility_Click(object sender, EventArgs e)
        {
            if (dgvFormular.SelectedRows.Count == 0)
            {
                MessageBox.Show("�������� ������!");
                return;
            }


            DialogResult dr = MessageBox.Show("�� ������������� ������ ����� ��������������� �� ���������� �����?", "��������!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

            if (dr == DialogResult.No) return;
            try
            {
                ci.RemoveResponsibility(bjUser, (int)dgvFormular.SelectedRows[0].Cells["id"].Value, CirculationStatuses.Finished.Value);
            }
            catch (Exception ex)
            {
                ALISError error = ALISErrorList._list.Find(x => x.Code == ex.Message);
                MessageBox.Show(error.Message);
                return;
            }
            FillFormular(ReaderInfo.GetReader(int.Parse(lFromularNumber.Text)));

        }

        private void ��������������������������������������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Statistics.Columns.Clear();
            //Statistics.Columns.Add("NN", "� �/�");
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            Statistics.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            //DatePeriod f3 = new DatePeriod();
            //f3.ShowDialog();
            lReferenceName.Text = "������������ ���������� ��� ";
            DBReference dbref = new DBReference();
            Statistics.DataSource = dbref.GetBooksWithRemovedResponsibility();
            if (this.Statistics.Rows.Count == 0)
            {
                this.Statistics.Columns.Clear();
                MessageBox.Show("��� �������� ����!");
                return;
            }

            autoinc(Statistics);
            Statistics.Columns[0].HeaderText = "��";
            Statistics.Columns[0].Width = 40;
            Statistics.Columns[1].HeaderText = "��������";
            Statistics.Columns[1].Width = 250;
            Statistics.Columns[2].HeaderText = "�����";
            Statistics.Columns[2].Width = 130;
            Statistics.Columns[3].HeaderText = "����� ������ ������� ������";
            Statistics.Columns[3].Width = 70;
            Statistics.Columns[4].HeaderText = "�������";
            Statistics.Columns[4].Width = 100;
            Statistics.Columns[5].HeaderText = "���";
            Statistics.Columns[5].Width = 80;
            Statistics.Columns[6].HeaderText = "��������";
            Statistics.Columns[6].Width = 80;
            Statistics.Columns[7].HeaderText = "��������";
            Statistics.Columns[7].Width = 80;
            Statistics.Columns[8].HeaderText = "���� ������";
            Statistics.Columns[8].ValueType = typeof(DateTime);
            Statistics.Columns[8].DefaultCellStyle.Format = "dd.MM.yyyy";
            Statistics.Columns[8].Width = 85;
            Statistics.Columns[9].HeaderText = "���� ������ ���������������";
            Statistics.Columns[9].DefaultCellStyle.Format = "dd.MM.yyyy";
            Statistics.Columns[9].Width = 85;
            Statistics.Columns[10].HeaderText = "����";
            Statistics.Columns[10].Width = 80;
            bSaveReferenceToFile.Enabled = true;
        }

        private void ����������������������������������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Statistics.Columns.Clear();
            //Statistics.Columns.Add("NN", "� �/�");
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            Statistics.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            //DatePeriod f3 = new DatePeriod();
            //f3.ShowDialog();
            lReferenceName.Text = "������ ����������� ������ ����������� ";
            DBReference dbref = new DBReference();
            Statistics.DataSource = dbref.GetViolators();
            if (this.Statistics.Rows.Count == 0)
            {
                this.Statistics.Columns.Clear();
                MessageBox.Show("��� �������� ����!");
                return;
            }
            //���
            //����� ������
            //����� ��������
            //����������� �����
            //�������
            //��.�����
            //�����
            //���� ������
            //���� �����
            //���� ����������

            autoinc(Statistics);
            Statistics.Columns[0].HeaderText = "��";
            Statistics.Columns[0].Width = 30;
            Statistics.Columns[1].HeaderText = "���";
            Statistics.Columns[1].Width = 100;
            Statistics.Columns[2].HeaderText = "����� ��������";
            Statistics.Columns[2].Width = 100;
            Statistics.Columns[3].HeaderText = "����� ��������";
            Statistics.Columns[3].Width = 100;
            Statistics.Columns[4].HeaderText = "��� �����/ ����";
            Statistics.Columns[4].Width = 100;
            Statistics.Columns[5].HeaderText = "�������";
            Statistics.Columns[5].Width = 100;
            Statistics.Columns[6].HeaderText = "Email";
            Statistics.Columns[6].Width = 100;
            Statistics.Columns[7].HeaderText = "�����";
            Statistics.Columns[7].Width = 100;

            Statistics.Columns[8].HeaderText = "���� ������";
            Statistics.Columns[8].Width = 90;
            Statistics.Columns[9].HeaderText = "���� ��������";
            Statistics.Columns[9].Width = 90;
            Statistics.Columns[10].HeaderText = "���� ����������";
            Statistics.Columns[10].Width = 90;
            bSaveReferenceToFile.Enabled = true;
            //foreach (DataGridViewRow r in Statistics.Rows)
            //{
            //    object value = r.Cells[6].Value;
            //    if (Convert.ToBoolean(value) == true)
            //    {
            //        r.DefaultCellStyle.BackColor = Color.Yellow;
            //    }
            //}
        }

        private void bComment_Click(object sender, EventArgs e)
        {
            if (lFromularNumber.Text == "")
            {
                MessageBox.Show("������� ����� ��� �������� �������� ��������!");
                return;
            }
            ReaderVO reader = new ReaderVO(int.Parse(lFromularNumber.Text));

            ChangeComment cc = new ChangeComment(reader);
            cc.ShowDialog();

        }


        private void bReaderRegistration_Click(object sender, EventArgs e)
        {
            if (lFromularNumber.Text == "")
            {
                MessageBox.Show("������� ����� ��� �������� �������� ��������!");
                return;
            }
            ReaderInfo reader = ReaderInfo.GetReader(int.Parse(lFromularNumber.Text));

            fReaderRegistrationAndRights frr = new fReaderRegistrationAndRights();
            frr.Init(reader.NumberReader);
            frr.ShowDialog();

            FillFormular(reader);
        }

        private void bEmulation_Click(object sender, EventArgs e)
        {
            bMainEmulation_Click(sender, e);
        }

        private void bEmulationTransfer_Click(object sender, EventArgs e)
        {
            bMainEmulation_Click(sender, e);
        }

        private void InvNumberOrderHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fInvNumberOrdersHistory f = new fInvNumberOrdersHistory();
            f.Show();
        }

        private void timerIssuedInHallCount_Tick(object sender, EventArgs e)
        {
            ShowIssuedInHallCount();
        }

        private void HallServiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fDatePeriod dp = new fDatePeriod();
            dp.ShowDialog();
            if (dp.Cancel == true)
            {
                //return;
            }
            TableDataVisualizer tbv = new TableDataVisualizer(dp, bjUser, ReferenceType.HallService);
            tbv.ShowDialog();
        }

        private void ActiveHallOrdersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TableDataVisualizer tbv = new TableDataVisualizer(null, bjUser, ReferenceType.ActiveHallOrders);
            tbv.ShowDialog();

        }

        private void FinishedHallOrdersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TableDataVisualizer tbv = new TableDataVisualizer(null, bjUser, ReferenceType.FinishedHallOrders);
            tbv.ShowDialog();

        }

        private void AllBooksInHallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("�������� ����� ������ �� 15 �����. ������� �� ��� �����������, ��� - ��� ������", "��������!", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                TableDataVisualizer tbv = new TableDataVisualizer(null, bjUser, ReferenceType.AllBooksInHall);
                tbv.ShowDialog();
            }

        }

        private void DebtoListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TableDataVisualizer tbv = new TableDataVisualizer(null, bjUser, ReferenceType.DebtorList);
            tbv.ShowDialog();
        }

        private void ReaderRegistrationAndLitresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fDatePeriod dp = new fDatePeriod();
            dp.ShowDialog();

            TableDataVisualizer tbv = new TableDataVisualizer(dp, bjUser, ReferenceType.ReaderRegistration);
            tbv.ShowDialog();

        }

        private void OrdersBySubjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fDatePeriod dp = new fDatePeriod();
            dp.ShowDialog();

            TableDataVisualizer tbv = new TableDataVisualizer(dp, bjUser, ReferenceType.OrdersCountBySubject);
            tbv.ShowDialog();

        }
    }
  
}
