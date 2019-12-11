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
            label4.Text = "Журнал событий " + DateTime.Now.ToShortDateString() + ":";

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

            //вводим прослойку-делегата, чтобы иметь возможность эмулировать ввод штрихкодов
            ScanFuncDelegate ScanDelegate;
            ScanDelegate = new ScanFuncDelegate(Form1_Scanned);
            this.Invoke(ScanDelegate, new object[] { FromPort });
        }


        void Form1_Scanned(string fromport)
        {
            string g = MainTabContainer.SelectedTab.ToString();
            switch (MainTabContainer.SelectedTab.Text)
            {
                case "Формуляр читателя":
                    #region formular
                    ReaderInfo reader = ReaderInfo.GetReaderByBar(fromport);
                    FillFormular(reader);

                    #endregion
                    break;

                case "Приём/выдача изданий":
                    #region priem

                    Circulate(fromport);
                    break;
                    #endregion

                

                case "Учёт посещаемости":
                    #region Учёт посещаемости
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
                case "Приём книг на кафедру из хранения/в хранение с кафедры":
                    RecieveBookFromInBookKeeping(fromport);
                    ShowBookTransfer(bjUser);
                    break;
                #endregion

            }
        }

        private void ShowAttendance()
        {
            lAttendance.Text = "На сегодня посещаемость составляет: " + department.GetAttendance() + " человек(а)";
        }

        private void RecieveBookFromInBookKeeping(string fromport)
        {
            
            //BJExemplarInfo exemplar = BJExemplarInfo.GetExemplarByBar(fromport);
            ExemplarBase exemplar = ExemplarFactory.CreateExemplar(fromport);
            if (exemplar == null)
            {
                MessageBox.Show("Книга не найдена.");
                return;
            }
            OrderInfo order = ci.FindOrderByExemplar(exemplar);
            try
            { 
                if (bjUser.SelectedUserStatus.DepName.ToLower().Contains("книгохранен"))
                {
                    ci.RecieveBookInBookkeeping(order, bjUser);
                    MessageBox.Show("Книга успешно принята в хранение!");
                    return;
                }
                else
                {
                    ci.RecieveBookFromBookkeeping(order, bjUser);
                    MessageBox.Show("Книга успешно принята на кафедру!");
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
                case 0://книга была выдана. нужно принять её в отдел
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
                    MessageBox.Show("Штрихкод не найден ни в базе читателей ни в базе книг!");
                    break;
                case 2:
                    MessageBox.Show("Ожидался штрихкод читателя, а считан штрихкод издания!");
                    break;
                case 3:
                    MessageBox.Show("Ожидался штрихкод издания, а считан штрихкод читателя!");
                    break;
                case 4:
                    lAuthor.Text = department.ScannedExemplar.Author;
                    lTitle.Text = department.ScannedExemplar.Title;
                    bCancel.Enabled = true;
                    label1.Text = "Считайте штрихкод читателя";
                    break;
                case 5:
                    lReader.Text = department.ScannedReader.FamilyName + " " + department.ScannedReader.Name + " " + department.ScannedReader.FatherName;
                    RPhoto.Image = department.ScannedReader.Photo;
                    bConfirm.Enabled = true;
                    this.AcceptButton = bConfirm;
                    bConfirm.Focus();
                    label1.Text = "Подтвердите операцию";
                    break;

            }
            ShowLog();
            ShowIssuedInHallCount();
        }

        private void ShowIssuedInHallCount()
        {
            lBooksCountInHall.Text = $"Выдано в зал: {department.GetIssuedInHallBooksCount()} книг";
        }

        private void AttendanceScan(string fromport)
        {
            if (!ReaderVO.IsReader(fromport))
            {
                MessageBox.Show("Неверный штрихкод читателя!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            ReaderVO reader = new ReaderVO(fromport);

            if (!reader.IsAlreadyMarked())
            {
                //department.AddAttendance(reader);
                //lAttendance.Text = "На сегодня посещаемость составляет: " + department.GetAttendance() + " человек(а)";
            }
            else
            {
                MessageBox.Show("Этот читатель уже посетил текущий зал сегодня!");
                return;
            }
        }

        public void FillFormular(ReaderInfo reader)
        {
            if (reader == null)
            {
                MessageBox.Show("Читатель не найден!");
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
                    new KeyValuePair<string, string> ( "bar", "Штрихкод"),
                    new KeyValuePair<string, string> ( "inv", "Инв. номер"),
                    new KeyValuePair<string, string> ( "author", "Автор"),
                    new KeyValuePair<string, string> ( "title", "Заглавие"),
                    new KeyValuePair<string, string> ( "issueDate", "Дата выдачи"),
                    new KeyValuePair<string, string> ( "returnDate", "Предполагаемая дата возврата"),
                    new KeyValuePair<string, string> ( "issDep", "Зал выдачи"),
                    new KeyValuePair<string, string> ( "retDep", "Зал возврата"),
                    new KeyValuePair<string, string> ( "cipher", "Расстановочный шифр"),
                    new KeyValuePair<string, string> ( "baseName", "Фонд"),
                    //new KeyValuePair<string, string> ( "prolongedTimes", "Продлено раз"),
                    new KeyValuePair<string, string> ( "status", "Статус"),
                    new KeyValuePair<string, string> ( "rack", "Стеллаж"),
                    //new KeyValuePair<string, string> ( "issueType", "Тип выдачи"),
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

            //dgvFormular.Columns["num"].HeaderText = "№№";
            //dgvFormular.Columns["num"].Width = 40;
            //dgvFormular.Columns["bar"].HeaderText = "Штрихкод";
            //dgvFormular.Columns["bar"].Width = 80;
            //dgvFormular.Columns["avt"].HeaderText = "Автор";
            //dgvFormular.Columns["avt"].Width = 200;
            //dgvFormular.Columns["tit"].HeaderText = "Заглавие";
            //dgvFormular.Columns["tit"].Width = 400;
            //dgvFormular.Columns["iss"].HeaderText = "Дата выдачи";
            //dgvFormular.Columns["iss"].Width = 80;
            //dgvFormular.Columns["ret"].HeaderText = "Предполагаемая дата возврата";
            //dgvFormular.Columns["ret"].Width = 110;
            //dgvFormular.Columns["shifr"].HeaderText = "Расстановочный шифр";
            //dgvFormular.Columns["shifr"].Width = 90;
            //dgvFormular.Columns["idiss"].Visible = false;
            //dgvFormular.Columns["idr"].Visible = false;
            //dgvFormular.Columns["DATE_ISSUE"].Visible = false;
            //dgvFormular.Columns["fund"].HeaderText = "Фонд";
            //dgvFormular.Columns["fund"].Width = 50;
            //dgvFormular.Columns["prolonged"].HeaderText = "Продлено, раз";
            //dgvFormular.Columns["prolonged"].Width = 80;
            //dgvFormular.Columns["IsAtHome"].HeaderText = "Тип выдачи";
            //dgvFormular.Columns["IsAtHome"].Width = 80;
            //dgvFormular.Columns["rack"].HeaderText = "Стеллаж";
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

            //здесь вставить проверку, что более 10 книг уже на дом берёт. выдавать или нет?
            //и ещё проверку, чтоб не более ста книг в зал.



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
            label1.Text = "Считайте штрихкод издания";
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
                new KeyValuePair<string, string> ( "time", "Время"),
                new KeyValuePair<string, string> ( "bar", "Штрихкод"),
                new KeyValuePair<string, string> ( "title", "Издание"),
                new KeyValuePair<string, string> ( "reader", "Читатель"),
                new KeyValuePair<string, string> ( "status", "Действие"),
                new KeyValuePair<string, string> ( "baseName", "Фонд"),
                //new KeyValuePair<string, string> ( "issueType", "Тип выдачи"),
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
                //string title = string.IsNullOrEmpty(book.Fields["700$a"].ToString()) ? "<нет>" : book.Fields["700$a"].ToString();
                row.Cells["title"].Value = exemplar.AuthorTitle;// $"{title}; {book.Fields["200$a"].ToString()}";
                row.Cells["reader"].Value = oi.ReaderId;
                row.Cells["status"].Value = fi.StatusName;
                row.Cells["baseName"].Value = BookBase.GetRusFundName(oi.Fund);
                //row.Cells["issueType"].Value = exemplar.ExemplarAccess.Access.In(new [] {1000,1006}) ? "на дом" : "в зал";
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
                MessageBox.Show("Читатель не найден");
                return;
            }
            if (reader == null)
            {
                MessageBox.Show("Читатель не найден!");
                return;
            }
            FillFormular(reader);

        }
        private void bProlong_Click(object sender, EventArgs e)
        {
            if (dgvFormular.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выделите строку!");
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
                case "Приём/выдача изданий":
                    ShowLog();
                    ShowIssuedInHallCount();
                    //CancelIssue();
                    label1.Enabled = true;
                    
                    //label1.Text = "Считайте штрихкод издания";
                    break;
                case "Справка":
                    label1.Enabled = false;
                    break;
                case "Формуляр читателя":
                    lFromularNumber.Text = "";
                    lFormularName.Text = "";
                    dgvFormular.Columns.Clear();
                    AcceptButton = this.bFormularFindById;
                    pbFormular.Image = null;
                    readerRightsView1.Clear();
                    break;
                case "Учёт посещаемости":
                    ShowAttendance();
                    break;
                case "Приём книг на кафедру из хранения/в хранение с кафедры":
                    ShowBookTransfer(bjUser);
                    break;

            }
        }

        private void ShowBookTransfer(BJUserInfo bjUser)
        {
            List<OrderInfo> transferOrders;// = ci.GetOrders(bjUser.SelectedUserStatus.UnifiedLocationCode);

            if (bjUser.SelectedUserStatus.DepName.ToLower().Contains("книгохранен"))
            {
                transferOrders = ci.GetOrders(CirculationStatuses.ForReturnToBookStorage.Value);
                label2.Text = "Считайте штрихкод, чтобы принять книгу в книгохранение";
            }   
            else
            {
                transferOrders = ci.GetOrders(CirculationStatuses.EmployeeLookingForBook.Value);
                label2.Text = "Считайте штрихкод, чтобы принять книгу на кафедру";
            }

            KeyValuePair<string, string>[] columns =
            {
                new KeyValuePair<string, string> ( "pin", "Пин"),
                new KeyValuePair<string, string> ( "title", "Издание"),
                new KeyValuePair<string, string> ( "bar", "Штрихкод"),
                new KeyValuePair<string, string> ( "reader", "Читатель"),
                new KeyValuePair<string, string> ( "location", "Местонахождение"),
                new KeyValuePair<string, string> ( "issDep", "Отдел выдачи"),
                new KeyValuePair<string, string> ( "retDep", "Отдел возврата"),
                new KeyValuePair<string, string> ( "inv", "Инв. номер"),
                new KeyValuePair<string, string> ( "status", "Статус заказа"),
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
                    row.Cells["title"].Value = "Экземпляр отсутствует в базе";
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
                //row.Cells["issueType"].Value = exemplar.ExemplarAccess.Access.In(new [] {1000,1006}) ? "на дом" : "в зал";
            }

            if (bjUser.SelectedUserStatus.DepName.ToLower().Contains("книгохранен"))
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
            if (this.bjUser == null)//это не забывать исправлять
            {
                MessageBox.Show("Вы не авторизованы! Программа заканчивает свою работу", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            if (lReferenceName.Text.Contains("просроч"))
            {
                foreach (DataGridViewRow r in Statistics.Rows)
                {
                    if (r.Cells[10].Value.ToString() == "true")
                    {
                        r.DefaultCellStyle.BackColor = Color.Yellow;
                    }
                }
            }
            if (lReferenceName.Text.Contains("нарушит"))
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
            pd.DocumentName = "Сверка фонда";
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
            sd.Title = "Сохранить в файл";
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
                MessageBox.Show("Введите номер или считайте штрихкод читателя!");
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
                MessageBox.Show("Введите номер или считайте штрихкод читателя!");
                return;
            }
            ReaderVO reader = new ReaderVO(int.Parse(lFromularNumber.Text));
            ReaderInformation f9 = new ReaderInformation(reader,this);
            f9.ShowDialog();
        }

        private void bSearchReaderByFIO_Click(object sender, EventArgs e)
        {
            //поиск читателя по фамилии
            FindReaderBySurname f16 = new FindReaderBySurname(this);
            f16.ShowDialog();
        }
        
        
       
       

        private void Statistics_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex == -1) return;
            if ((lReferenceName.Text.IndexOf("Список просроченных документов на текущий момент") != -1) )
            {
                MainTabContainer.SelectedIndex = 1;
                numericUpDown3.Value = int.Parse(Statistics.Rows[e.RowIndex].Cells[3].Value.ToString());
                bFormularFindById_Click(sender, new EventArgs());
            }
            if (lReferenceName.Text.Contains("нарушит"))
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

        private void выданныеКнигиНаТекущийМоментToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Statistics.Columns.Clear();
            //Statistics.Columns.Add("NN", "№ п/п");
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            Statistics.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            //DatePeriod f3 = new DatePeriod();
            //f3.ShowDialog();
            lReferenceName.Text = "Список выданных документов на текущий момент ";
            DBReference dbref = new DBReference();
            Statistics.DataSource = dbref.GetAllIssuedBook();
            if (this.Statistics.Rows.Count == 0)
            {
                this.Statistics.Columns.Clear();
                MessageBox.Show("Нет выданных книг!");
                return;
            }

            autoinc(Statistics);
            Statistics.Columns[0].Width = 40;
            Statistics.Columns[0].HeaderText = "№№";
            Statistics.Columns[1].HeaderText = "Заглавие";
            Statistics.Columns[1].Width = 270;
            Statistics.Columns[2].HeaderText = "Автор";
            Statistics.Columns[2].Width = 140;
            Statistics.Columns[3].HeaderText = "Номер читате льского билета";
            Statistics.Columns[3].Width = 70;
            Statistics.Columns[4].HeaderText = "Фамилия";
            Statistics.Columns[4].Width = 100;
            Statistics.Columns[5].HeaderText = "Имя";
            Statistics.Columns[5].Width = 90;
            Statistics.Columns[6].HeaderText = "Отчество";
            Statistics.Columns[6].Width = 100;
            Statistics.Columns[7].HeaderText = "Штрихкод";
            Statistics.Columns[7].Width = 80;
            Statistics.Columns[8].HeaderText = "Дата выдачи";
            Statistics.Columns[8].ValueType = typeof(DateTime);
            Statistics.Columns[8].DefaultCellStyle.Format = "dd.MM.yyyy";
            Statistics.Columns[8].Width = 85;
            Statistics.Columns[9].HeaderText = "Предпо лагаемая дата возврата";
            Statistics.Columns[9].DefaultCellStyle.Format = "dd.MM.yyyy";
            Statistics.Columns[9].Width = 85;
            Statistics.Columns[10].Visible = false;
            Statistics.Columns[11].HeaderText = "Расстановочный шифр";
            Statistics.Columns[11].Width = 100;
            Statistics.Columns[12].HeaderText = "Фонд";
            Statistics.Columns[12].Width = 50;
            Statistics.Columns[13].HeaderText = "Тип выдачи";
            Statistics.Columns[13].Width = 50;

            bSaveReferenceToFile.Enabled = true;
        }

        private void просроченныеКнигиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Statistics.Columns.Clear();
            //Statistics.Columns.Add("NN", "№ п/п");
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            Statistics.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            //DatePeriod f3 = new DatePeriod();
            //f3.ShowDialog();
            lReferenceName.Text = "Список просроченных документов на текущий момент";
            DBReference dbref = new DBReference();
            Statistics.DataSource = dbref.GetAllOverdueBook();
            if (this.Statistics.Rows.Count == 0)
            {
                this.Statistics.Columns.Clear();
                MessageBox.Show("Нет выданных книг!");
                return;
            }

            autoinc(Statistics);
            Statistics.Columns[0].HeaderText = "№№";
            Statistics.Columns[0].Width = 40;
            Statistics.Columns[1].HeaderText = "Заглавие";
            Statistics.Columns[1].Width = 240;
            Statistics.Columns[2].HeaderText = "Автор";
            Statistics.Columns[2].Width = 120;
            Statistics.Columns[3].HeaderText = "Номер читате льского билета";
            Statistics.Columns[3].Width = 70;
            Statistics.Columns[4].HeaderText = "Фамилия";
            Statistics.Columns[4].Width = 100;
            Statistics.Columns[5].HeaderText = "Имя";
            Statistics.Columns[5].Width = 80;
            Statistics.Columns[6].HeaderText = "Отчество";
            Statistics.Columns[6].Width = 80;
            Statistics.Columns[7].HeaderText = "Штрихкод";
            Statistics.Columns[7].Width = 75;
            Statistics.Columns[8].HeaderText = "Дата выдачи";
            Statistics.Columns[8].ValueType = typeof(DateTime);
            Statistics.Columns[8].DefaultCellStyle.Format = "dd.MM.yyyy";
            Statistics.Columns[8].Width = 85;
            Statistics.Columns[9].HeaderText = "Предпо лагаемая дата возврата";
            Statistics.Columns[9].DefaultCellStyle.Format = "dd.MM.yyyy";
            Statistics.Columns[9].Width = 85;
            Statistics.Columns[10].Visible = false;
            Statistics.Columns[10].ValueType = typeof(bool);
            Statistics.Columns[11].HeaderText = "Дата последней отправки email";
            Statistics.Columns[11].DefaultCellStyle.Format = "dd.MM.yyyy";
            Statistics.Columns[11].Width = 85;
            Statistics.Columns[12].HeaderText = "Расстановочный шифр";
            Statistics.Columns[12].Width = 85;
            Statistics.Columns[13].HeaderText = "Фонд";
            Statistics.Columns[13].Width = 50;
            Statistics.Columns[14].HeaderText = "Стеллаж";
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
                MessageBox.Show("Введите номер или считайте штрихкод читателя!");
                return;
            }
            ReaderInfo reader = ReaderInfo.GetReader(int.Parse(lFromularNumber.Text)); 
            EmailSending es = new EmailSending(reader, bjUser);
            es.ShowDialog();
        }

        private void списокДействийОператораЗаПериодToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Statistics.Columns != null)
                Statistics.Columns.Clear();
            fDatePeriod f3 = new fDatePeriod();
            f3.ShowDialog();
            lReferenceName.Text = "";
            lReferenceName.Text = "Список действий оператора за период с " + f3.StartDate.ToString("dd.MM.yyyy") + " по " + f3.EndDate.ToString("dd.MM.yyyy") + ": ";
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
            Statistics.Columns[0].HeaderText = "№№";
            Statistics.Columns[1].Width = 250;
            Statistics.Columns[1].HeaderText = "Действие";
            Statistics.Columns[2].HeaderText = "Дата";
            Statistics.Columns[2].Width = 200;
            autoinc(Statistics);
        }

        private void отчётОтделаЗаПериодToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Statistics.Columns != null)
                Statistics.Columns.Clear();
            fDatePeriod f3 = new fDatePeriod();
            f3.ShowDialog();
            lReferenceName.Text = "Отчёт отдела за период с " + f3.StartDate.ToString("dd.MM.yyyy") + " по " + f3.EndDate.ToString("dd.MM.yyyy") + ": ";
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
            Statistics.Columns[0].HeaderText = "№№";
            Statistics.Columns[1].Width = 250;
            Statistics.Columns[1].HeaderText = "Наименование";
            Statistics.Columns[2].HeaderText = "Количество";
            Statistics.Columns[2].Width = 200;
            autoinc(Statistics);
        }
        private void отчётТекущегоОператораЗаПериодToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Statistics.Columns != null)
                Statistics.Columns.Clear();
            fDatePeriod f3 = new fDatePeriod();
            f3.ShowDialog();
            lReferenceName.Text = "Отчёт текущего оператора за период с " + f3.StartDate.ToString("dd.MM.yyyy") + " по " + f3.EndDate.ToString("dd.MM.yyyy") + ": ";
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
            Statistics.Columns[0].HeaderText = "№№";
            Statistics.Columns[1].Width = 250;
            Statistics.Columns[1].HeaderText = "Наименование";
            Statistics.Columns[2].HeaderText = "Количество";
            Statistics.Columns[2].Width = 200;
            autoinc(Statistics);
        }
        private void всеКнигиЦентраАмериканскойКультурыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Statistics.Columns.Clear();
            //Statistics.Columns.Add("NN", "№ п/п");
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            Statistics.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            //DatePeriod f3 = new DatePeriod();
            //f3.ShowDialog();
            lReferenceName.Text = "Список всех документов ЦАК + ОФ ";
            DBReference dbref = new DBReference();
            Statistics.DataSource = dbref.GetAllBooks();
            if (this.Statistics.Rows.Count == 0)
            {
                this.Statistics.Columns.Clear();
                MessageBox.Show("Нет выданных книг!");
                return;
            }

            autoinc(Statistics);
            Statistics.Columns[0].Width = 70;
            Statistics.Columns[0].HeaderText = "№№";
            Statistics.Columns[1].HeaderText = "Заглавие";
            Statistics.Columns[1].Width = 500;
            Statistics.Columns[2].HeaderText = "Автор";
            Statistics.Columns[2].Width = 200;
            Statistics.Columns[3].HeaderText = "Штрихкод";
            Statistics.Columns[3].Width = 100;
            Statistics.Columns[4].HeaderText = "Фонд";
            Statistics.Columns[4].Width = 150;
            Statistics.Columns[5].HeaderText = "Тематика";
            Statistics.Columns[5].Width = 150;
            Statistics.Columns[6].HeaderText = "Стеллаж";
            Statistics.Columns[6].Width = 150;

            bSaveReferenceToFile.Enabled = true;
        }

        private void обращаемостьКнигToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Statistics.Columns.Clear();
            //Statistics.Columns.Add("NN", "№ п/п");
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            Statistics.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            //DatePeriod f3 = new DatePeriod();
            //f3.ShowDialog();
            lReferenceName.Text = "Обращаемость документов ЦАК ";
            DBReference dbref = new DBReference();
            Statistics.DataSource = dbref.GetBookNegotiability();
            if (this.Statistics.Rows.Count == 0)
            {
                this.Statistics.Columns.Clear();
                MessageBox.Show("Нет выданных книг!");
                return;
            }

            autoinc(Statistics);
            Statistics.Columns[0].Width = 70;
            Statistics.Columns[0].HeaderText = "№№";
            Statistics.Columns[1].HeaderText = "Заглавие";
            Statistics.Columns[1].Width = 500;
            Statistics.Columns[2].HeaderText = "Автор";
            Statistics.Columns[2].Width = 200;
            Statistics.Columns[3].HeaderText = "Штрихкод";
            Statistics.Columns[3].Width = 100;
            Statistics.Columns[4].HeaderText = "Обращаемость";
            Statistics.Columns[4].Width = 100;
            Statistics.Columns[5].HeaderText = "Фонд";
            Statistics.Columns[5].Width = 70;

            bSaveReferenceToFile.Enabled = true;
        }

        private void bRemoveResponsibility_Click(object sender, EventArgs e)
        {
            if (dgvFormular.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выделите строку!");
                return;
            }


            DialogResult dr = MessageBox.Show("Вы действительно хотите снять ответственность за выделенную книгу?", "Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

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

        private void списокКнигСКоторыхСнятаОтветственностьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Statistics.Columns.Clear();
            //Statistics.Columns.Add("NN", "№ п/п");
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            Statistics.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            //DatePeriod f3 = new DatePeriod();
            //f3.ShowDialog();
            lReferenceName.Text = "Обращаемость документов ЦАК ";
            DBReference dbref = new DBReference();
            Statistics.DataSource = dbref.GetBooksWithRemovedResponsibility();
            if (this.Statistics.Rows.Count == 0)
            {
                this.Statistics.Columns.Clear();
                MessageBox.Show("Нет выданных книг!");
                return;
            }

            autoinc(Statistics);
            Statistics.Columns[0].HeaderText = "№№";
            Statistics.Columns[0].Width = 40;
            Statistics.Columns[1].HeaderText = "Заглавие";
            Statistics.Columns[1].Width = 250;
            Statistics.Columns[2].HeaderText = "Автор";
            Statistics.Columns[2].Width = 130;
            Statistics.Columns[3].HeaderText = "Номер читате льского билета";
            Statistics.Columns[3].Width = 70;
            Statistics.Columns[4].HeaderText = "Фамилия";
            Statistics.Columns[4].Width = 100;
            Statistics.Columns[5].HeaderText = "Имя";
            Statistics.Columns[5].Width = 80;
            Statistics.Columns[6].HeaderText = "Отчество";
            Statistics.Columns[6].Width = 80;
            Statistics.Columns[7].HeaderText = "Штрихкод";
            Statistics.Columns[7].Width = 80;
            Statistics.Columns[8].HeaderText = "Дата выдачи";
            Statistics.Columns[8].ValueType = typeof(DateTime);
            Statistics.Columns[8].DefaultCellStyle.Format = "dd.MM.yyyy";
            Statistics.Columns[8].Width = 85;
            Statistics.Columns[9].HeaderText = "Дата снятия ответственности";
            Statistics.Columns[9].DefaultCellStyle.Format = "dd.MM.yyyy";
            Statistics.Columns[9].Width = 85;
            Statistics.Columns[10].HeaderText = "Фонд";
            Statistics.Columns[10].Width = 80;
            bSaveReferenceToFile.Enabled = true;
        }

        private void списокНарушителейСроковПользованияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Statistics.Columns.Clear();
            //Statistics.Columns.Add("NN", "№ п/п");
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            Statistics.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            //DatePeriod f3 = new DatePeriod();
            //f3.ShowDialog();
            lReferenceName.Text = "Список нарушителей сроков пользования ";
            DBReference dbref = new DBReference();
            Statistics.DataSource = dbref.GetViolators();
            if (this.Statistics.Rows.Count == 0)
            {
                this.Statistics.Columns.Clear();
                MessageBox.Show("Нет выданных книг!");
                return;
            }
            //ФИО
            //Номер билета
            //Права читателя
            //Инвентарный номер
            //телефон
            //Эл.почта
            //Адрес
            //Дата выдачи
            //Срок сдачи
            //Дней просрочено

            autoinc(Statistics);
            Statistics.Columns[0].HeaderText = "№№";
            Statistics.Columns[0].Width = 30;
            Statistics.Columns[1].HeaderText = "ФИО";
            Statistics.Columns[1].Width = 100;
            Statistics.Columns[2].HeaderText = "Номер читателя";
            Statistics.Columns[2].Width = 100;
            Statistics.Columns[3].HeaderText = "Права читателя";
            Statistics.Columns[3].Width = 100;
            Statistics.Columns[4].HeaderText = "Инв номер/ шкод";
            Statistics.Columns[4].Width = 100;
            Statistics.Columns[5].HeaderText = "Телефон";
            Statistics.Columns[5].Width = 100;
            Statistics.Columns[6].HeaderText = "Email";
            Statistics.Columns[6].Width = 100;
            Statistics.Columns[7].HeaderText = "Адрес";
            Statistics.Columns[7].Width = 100;

            Statistics.Columns[8].HeaderText = "Дата выдачи";
            Statistics.Columns[8].Width = 90;
            Statistics.Columns[9].HeaderText = "Дата возврата";
            Statistics.Columns[9].Width = 90;
            Statistics.Columns[10].HeaderText = "Дней просрочено";
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
                MessageBox.Show("Введите номер или считайте штрихкод читателя!");
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
                MessageBox.Show("Введите номер или считайте штрихкод читателя!");
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
            DialogResult dr = MessageBox.Show("Операция может занять до 15 минут. Нажмите Да для продолжения, Нет - для отмены", "Внимание!", MessageBoxButtons.YesNo);
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
