using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
namespace DocList
{
    public partial class Form1 : Form
    {
        BookBLL bookbll;
        bool Error                       = false;
        List<BookForRep> bl       = new List<BookForRep>();
        SerialPort port;
        public Form1()
        {
            InitializeComponent();
            try
            {
                bookbll = new BookBLL();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                Error = true;
            }

            //BarScanner bs = new BarScanner(this);
            CreateColumns();
            Docs.Rows.Clear();
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

        }
        private void proccessData(string data)
        {
            string FromPort = data;
            FromPort = FromPort.Trim();

            //if (!BarScanner.CheckScanData(FromPort))
            //{
            //    MessageBox.Show("Считанный штрихкод не является штрихкодом книги! Если вы уверены, что штрихкод правильный, попробуйте считать его еще раз");
            //    return;
            //}
            Book bookg = new Book();
            try
            {
                switch (bookbll.ISBJVVV(FromPort))
                {
                    case 1:
                        BooksView book = new BooksView();
                        book = bookbll.GetBookByBar(FromPort);
                        bookg = new Book(book);
                        FillGrid(bookg);
                        break;
                    case 2:
                        BooksViewRED bookred = new BooksViewRED();
                        bookred = bookbll.GetBookByBarRED(FromPort);
                        bookg = new Book(bookred);
                        FillGrid(bookg);
                        break;
                    case 3:
                        BooksViewFCC bookFCC = new BooksViewFCC();
                        bookFCC = bookbll.GetBookByBarFCC(FromPort);
                        bookg = new Book(bookFCC);
                        FillGrid(bookg);
                        break;
                    case -1:
                        MessageBox.Show("Книга не найдена в базе!");
                        return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            bookg.idm = bl.Count + 1;
            bl.Add(new BookForRep(bookg));

        }
        void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            proccessData(port.ReadLine());
        }

        internal void FireScan(string scanData)
        {

        }

        private void FillGrid(Book book)
        {
            string title =  ((book.author != null) ? book.author : "");
            title += (title != "") ? ((book.title != null) ? ", " + book.title : book.title) : book.title;
            string size =   ((book.volume != null) ? book.volume : "");
            size += (size != "") ? ((book.illustrs != null) ? ", " + book.illustrs : book.illustrs) : book.illustrs;
            size += (size != "") ? ((book.size != null) ? ", " + book.size : book.size) : book.size;
            string pub =    ((book.placepub != null) ? book.placepub : "");
            pub += (pub != "") ? ((book.pubhouse != null) ? ": " + book.pubhouse : book.pubhouse) : book.pubhouse;
            string note =   ((book.note != null) ? book.note : "");
            note += (note != "") ? ((book.notesp != null) ? ". " + book.notesp : book.notesp) : book.notesp;
            Docs.Invoke(
                (ThreadStart)delegate {
                    Docs.Rows.Insert
                            (
                                Docs.Rows.Count,
                                new string[] 
                                {
                                    "",
                                    title,
                                    pub,
                                    book.dpublish,
                                    size,
                                    note,
                                    book.inv,
                                    book.cdc
                                }
                            );
                        });
            autoinc(Docs);
        }

        private void CreateColumns()
        {
            Docs.Columns.Clear();
            Docs.AutoGenerateColumns = false;

            Docs.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            Docs.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
            col.HeaderText = "№№";
            col.Width = 50;
            col.ReadOnly = true;
            Docs.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "Автор, Заглавие";
            col.Width = 270;
            col.ReadOnly = true;
            Docs.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "М.изд: изд-во";
            col.Width = 150;
            col.ReadOnly = true;
            Docs.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "Дата изд";
            col.Width = 40;
            col.ReadOnly = true;
            Docs.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "Пагинация (объём, ил., размер";
            col.Width = 100;
            col.ReadOnly = true;
            Docs.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "Примечания (Общие. об особ.экз.)";
            col.Width = 200;
            col.ReadOnly = true;
            Docs.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "Инв. №";
            col.Width = 70;
            col.ReadOnly = true;
            Docs.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "Шифр";
            col.Width = 70;
            col.ReadOnly = true;
            Docs.Columns.Add(col);


        }
        public void autoinc(DataGridView dgv)
        {
            int i = 0;
            foreach (DataGridViewRow row in dgv.Rows)
            {
                row.Cells[0].Value = ++i;
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            if (Error)
                this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DocList dl = new DocList();
            dl.SetDataSource(bl);
            ViewRep v = new ViewRep(dl,textBox1.Text,bl.Count.ToString());
            v.ShowDialog();
            
        }
        private string emulation = "";
        private void btnEmulation_Click(object sender, EventArgs e)
        {
            Emulation em = new Emulation();
            em.ShowDialog();
            proccessData(em.emul);
        }
    }
}
