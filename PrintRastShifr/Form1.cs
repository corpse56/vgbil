using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
//using OposScanner_CCO;
using System.Drawing.Printing;
using System.Data.SqlClient;
using Test1;
using System.IO.Ports;

namespace RastShifrRestored
{
    public partial class Form1 : Form
    {
        // Fields
        private Button button1;
        private Button button2;
        public string IDDATAEXT;
        
        private Form2 f2;
        private Label label1;
        private static PrintDocument pd;
        private Font printFont;
        private string printString;
        private RadioButton radioButton1;
        private RadioButton radioButton2;
        private PrintDocument ShifrLabelPD;
        private string connstr;

        SerialPort port;

        // Methods
        
        //private void button1_Click(object sender, EventArgs e);
        //private void button2_Click(object sender, EventArgs e);
        
        //private void Form1_Load(object sender, EventArgs e);
        
        //private void pd_PrintPage(object sender, PrintPageEventArgs e);
        //private void Scanner_DataEvent(int Status);
        //private void Scanner_ErrorEvent(int ResultCode, int ResultCodeExtended, int ErrorLocus, ref int pErrorResponse);

        public Form1()
        {
            InitializeComponent();
            this.f2 = new Form2();
            this.f2.ShowDialog();
            /*try
            {
                //MessageBox.Show("before new scanner");
                this.Scanner = new OPOSScannerClass();
                //MessageBox.Show("before error event");
                this.Scanner.ErrorEvent += new _IOPOSScannerEvents_ErrorEventEventHandler(Scanner_ErrorEvent);
                //MessageBox.Show("before data event");
                //this.Scanner.DataEvent += new _IOPOSScannerEvents_DataEventEventHandler(Scanner_DataEvent);
                //MessageBox.Show();
                this.Scanner.DataEvent += new _IOPOSScannerEvents_DataEventEventHandler(Scanner_DataEvent);

                this.Scanner.Open("STI_SCANNER");
                ResultCodeH.Check(this.Scanner.ClaimDevice(7000));
                this.Scanner.DeviceEnabled = true;
                ResultCodeH.Check(this.Scanner.ResultCode);
                this.Scanner.AutoDisable = true;
                ResultCodeH.Check(this.Scanner.ResultCode);
                this.Scanner.DataEventEnabled = true;
                ResultCodeH.Check(this.Scanner.ResultCode);
                MessageBox.Show("dataeventenabled = "+this.Scanner.DataEventEnabled.ToString());
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message.ToString());
            }*/



            pd = new PrintDocument();
            pd.PrinterSettings.PrinterName = this.f2.PrinterName;
            this.printFont = new Font("Arial", 21f);
            //num = this.printFont.Height;
            pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
            //MessageBox.Show("after printer setup");
            try
            {
                connstr = XmlConnections.GetConnection("/Connections/BJVVV");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "��������� �������� ��������� ������.");
                throw;
            }
            //MessageBox.Show("end ctor");
            string COMPORT = XmlConnections.GetConnection("/Connections/COMPORT");

            port = new SerialPort(COMPORT, 9600, Parity.None, 8, StopBits.One);
            //bool op = port.IsOpen;

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
        string emu = "";
        void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string str;
                IDDATAEXT = "";
                SqlDataAdapter adapter;
                SqlConnection connection;
                DataSet set;
                DataSet set2;
                str = port.ReadLine();
                str = str.Trim();

                port.DiscardInBuffer();
                port.DiscardOutBuffer();
                adapter = new SqlDataAdapter();
                connection = new SqlConnection(connstr);
                adapter.SelectCommand = new SqlCommand();
                adapter.SelectCommand.Connection = connection;
                adapter.SelectCommand.CommandText = "select BARCODE_UNITS.IDMAIN,IDDATA from BJVVV..BARCODE_UNITS where BARCODE_UNITS.BARCODE = '" + str + "'";
                set = new DataSet();
                if (adapter.Fill(set) == 0)
                {
                    MessageBox.Show("����� ����� ��� � ����, ���� �� �� �������� ��������.", "��������!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //this.Scanner.DataEventEnabled = true;
                    //this.Scanner.DeviceEnabled = true;
                    return;
                }
                adapter.SelectCommand.CommandText = "select DATAEXT.SORT, DATAEXT.ID from BJVVV..DATAEXT " +
                                                    "where DATAEXT.IDMAIN = " + set.Tables[0].Rows[0][0].ToString() +
                                                    " and DATAEXT.MNFIELD=899 and DATAEXT.MSFIELD='$j' " +
                                                    "and DATAEXT.IDDATA = " + set.Tables[0].Rows[0][1].ToString();
                set2 = new DataSet();
                set2.Clear();
                if (adapter.Fill(set2) == 0)// 0 - � ���� ����� ��� ��������������� �����. 1 - � ���� ����� ���� �������������� ��� � ��������
                {
                    adapter.SelectCommand.CommandText = "select SORT from BJVVV..DATAEXT where IDDATA = " + set.Tables[0].Rows[0][1].ToString()
                                                        + " and DATAEXT.MNFIELD=899 and DATAEXT.MSFIELD='$b'";
                    set2 = new DataSet();
                    int countshifr = adapter.Fill(set2);
                    if (countshifr == 0)
                    {
                        MessageBox.Show("� ����� �������� ���. ������ ����������� ���� ����. ���������� ���������� ����! ����� ���������� ����, �������� ���� ��� ������� ���. ������!");
                        //this.Scanner.DataEventEnabled = true;
                        //this.Scanner.DeviceEnabled = true;
                        return;
                    }
                    string mhr = set2.Tables[0].Rows[0][0].ToString();
                    //���� ��� ��������� � �������������� � ���
                    if (mhr.Contains("���������")) //�������������� ���
                    {
                        adapter.SelectCommand.CommandText = "select A.SORT as inv, A.ID as invid, A.IDDATA as inviddata, " +
                                                            "B.SORT as shif, B.ID as shifid, B.IDDATA as shifiddata, C.SORT " +
                                                            "from BJVVV..DATAEXT A " +
                                                            "left join BJVVV..DATAEXT B on A.IDDATA=B.IDDATA " +
                                                            "left join BJVVV..DATAEXT C on B.IDDATA=C.IDDATA " +
                                                            "where A.IDMAIN = " + set.Tables[0].Rows[0][0].ToString() +
                                                            " and A.MNFIELD=899 and A.MSFIELD='$p' " +
                                                            "and B.MNFIELD=899 and B.MSFIELD='$j' " +
                                                            "and C.MNFIELD=899 and C.MSFIELD='$b' " +
                                                            "and C.SORT like '%���������%' ";
                    }
                    else //�������������� ��
                    {
                        adapter.SelectCommand.CommandText = "select A.SORT as inv, A.ID as invid, A.IDDATA as inviddata, " +
                                                            "B.SORT as shif, B.ID as shifid, B.IDDATA as shifiddata, C.SORT " +
                                                            "from BJVVV..DATAEXT A " +
                                                            "left join BJVVV..DATAEXT B on A.IDDATA=B.IDDATA " +
                                                            "left join BJVVV..DATAEXT C on B.IDDATA=C.IDDATA " +
                                                            "where A.IDMAIN = " + set.Tables[0].Rows[0][0].ToString() +
                                                            "and A.MNFIELD=899 and A.MSFIELD='$p' " +
                                                            "and B.MNFIELD=899 and B.MSFIELD='$j' " +
                                                            "and C.MNFIELD=899 and C.MSFIELD='$b' " +
                                                            "and C.SORT not like '%���������%' ";
                    }

                    set2 = new DataSet();
                    set2.Clear();
                    countshifr = adapter.Fill(set2);
                    if (countshifr == 0)
                    {
                        MessageBox.Show("��� ����� " + mhr + " ����������� �������������� ����!");
                        //MessageBox.Show("������������ ������ � 0�0001. ���������� � ������������"); //��� ���������! ������ ���� �� �����
                        //Close();
                        return;
                    }
                    if (countshifr == 1)//������������ �������������� ���� �� ��� ���������.
                    {
                        IDDATAEXT = set2.Tables[0].Rows[0][4].ToString();
                    }
                    if (countshifr >= 2) //���� �������������� ��� � ������, �� ���� ������� ����� �������������� ��������
                    {

                        Form3 f3 = new Form3(set2.Tables[0], this);
                        f3.ShowDialog();
                        if (IDDATAEXT == "Cancel")
                        {
                            //this.Scanner.DataEventEnabled = true;
                            //this.Scanner.DeviceEnabled = true;
                            return;
                        }
                    }
                }
                else //1 - � ���� ����� ���� �������������� ��� � ��������
                {
                    IDDATAEXT = set2.Tables[0].Rows[0][1].ToString();
                }
                adapter.SelectCommand.CommandText = "select DATAEXTPLAIN.PLAIN from BJVVV..DATAEXTPLAIN where DATAEXTPLAIN.IDDATAEXT = " + IDDATAEXT;
                set2 = new DataSet();
                set2.Clear();
                if (adapter.Fill(set2) == 0)//������ ����
                {
                    MessageBox.Show("����������� ������." + IDDATAEXT, "��������!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //this.Scanner.DataEventEnabled = true;
                    //this.Scanner.DeviceEnabled = true;
                    return;
                }
                this.printString = set2.Tables[0].Rows[0][0].ToString();
                pd = new PrintDocument();
                pd.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);
                pd.PrinterSettings.PrinterName = this.f2.PrinterName;



                pd.Print();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //Form4 f4 = new Form4(this.printString);
            //f4.ShowDialog();

        }

   /*     void Scanner_DataEvent(int Status)
        {
            //MessageBox.Show("start data event");
            this.Scanner.DataEventEnabled = true;
            this.Scanner.DeviceEnabled = true;
            try
            {
                string str;
                IDDATAEXT = "";
                SqlDataAdapter adapter;
                SqlConnection connection;
                DataSet set;
                DataSet set2;
                bool flag;
                str = this.Scanner.ScanData.Remove(this.Scanner.ScanData.Length - 1);
                adapter = new SqlDataAdapter();
                connection = new SqlConnection(connstr);
                adapter.SelectCommand = new SqlCommand();
                adapter.SelectCommand.Connection = connection;
                adapter.SelectCommand.CommandText = "select BARCODE_UNITS.IDMAIN,IDDATA from BJVVV..BARCODE_UNITS where BARCODE_UNITS.BARCODE = '" + str + "'";
                set = new DataSet();
                if (adapter.Fill(set) == 0)
                {
                    MessageBox.Show("����� ����� ��� � ����, ���� �� �� �������� ��������.", "��������!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Scanner.DataEventEnabled = true;
                    this.Scanner.DeviceEnabled = true;
                    return;
                }
                adapter.SelectCommand.CommandText = "select DATAEXT.SORT, DATAEXT.ID from BJVVV..DATAEXT " +
                                                    "where DATAEXT.IDMAIN = " + set.Tables[0].Rows[0][0].ToString() +
                                                    " and DATAEXT.MNFIELD=899 and DATAEXT.MSFIELD='$j' " +
                                                    "and DATAEXT.IDDATA = " + set.Tables[0].Rows[0][1].ToString();
                set2 = new DataSet();
                set2.Clear();
                if (adapter.Fill(set2) == 0)// 0 - � ���� ����� ��� ��������������� �����. 1 - � ���� ����� ���� �������������� ��� � ��������
                {
                    adapter.SelectCommand.CommandText = "select SORT from DATAEXT where IDDATA = " + set.Tables[0].Rows[0][1].ToString()
                                                        + " and DATAEXT.MNFIELD=899 and DATAEXT.MSFIELD='$b'";
                    set2 = new DataSet();
                    int countshifr = adapter.Fill(set2);
                    if (countshifr == 0)
                    {
                        MessageBox.Show("� ����� �������� ���. ������ ����������� ���� ����. ���������� ���������� ����! ����� ���������� ����, �������� ���� ��� ������� ���. ������!");
                        this.Scanner.DataEventEnabled = true;
                        this.Scanner.DeviceEnabled = true;
                        return;
                    }
                    string mhr = set2.Tables[0].Rows[0][0].ToString();
                    //���� ��� ��������� � �������������� � ���
                    if (mhr.Contains("���������")) //�������������� ���
                    {
                        adapter.SelectCommand.CommandText = "select A.SORT as inv, A.ID as invid, A.IDDATA as inviddata, " +
                                                            "B.SORT as shif, B.ID as shifid, B.IDDATA as shifiddata, C.SORT " +
                                                            "from BJVVV..DATAEXT A " +
                                                            "left join BJVVV..DATAEXT B on A.IDDATA=B.IDDATA " +
                                                            "left join BJVVV..DATAEXT C on B.IDDATA=C.IDDATA " +
                                                            "where A.IDMAIN = " + set.Tables[0].Rows[0][0].ToString() +
                                                            " and A.MNFIELD=899 and A.MSFIELD='$p' " +
                                                            "and B.MNFIELD=899 and B.MSFIELD='$j' " +
                                                            "and C.MNFIELD=899 and C.MSFIELD='$b' " +
                                                            "and C.SORT like '%���������%' ";
                    }
                    else //�������������� ��
                    {
                        adapter.SelectCommand.CommandText = "select A.SORT as inv, A.ID as invid, A.IDDATA as inviddata, " +
                                                            "B.SORT as shif, B.ID as shifid, B.IDDATA as shifiddata, C.SORT " +
                                                            "from BJVVV..DATAEXT A " +
                                                            "left join BJVVV..DATAEXT B on A.IDDATA=B.IDDATA " +
                                                            "left join BJVVV..DATAEXT C on B.IDDATA=C.IDDATA " +
                                                            "where A.IDMAIN = " + set.Tables[0].Rows[0][0].ToString() +
                                                            "and A.MNFIELD=899 and A.MSFIELD='$p' " +
                                                            "and B.MNFIELD=899 and B.MSFIELD='$j' " +
                                                            "and C.MNFIELD=899 and C.MSFIELD='$b' " +
                                                            "and C.SORT not like '%���������%' ";
                    }

                    set2 = new DataSet();
                    set2.Clear();
                    countshifr = adapter.Fill(set2);
                    if (countshifr == 0)
                    {
                        MessageBox.Show("��� ����� " + mhr + " ����������� �������������� ����!");
                        this.Scanner.DataEventEnabled = true;
                        this.Scanner.DeviceEnabled = true;
                        //MessageBox.Show("������������ ������ � 0�0001. ���������� � ������������"); //��� ���������! ������ ���� �� �����
                        //Close();
                        return;
                    }
                    if (countshifr == 1)//������������ �������������� ���� �� ��� ���������.
                    {
                        IDDATAEXT = set2.Tables[0].Rows[0][4].ToString();
                    }
                    if (countshifr >= 2) //���� �������������� ��� � ������, �� ���� ������� ����� �������������� ��������
                    {

                        Form3 f3 = new Form3(set2.Tables[0], this);
                        f3.ShowDialog();
                        if (IDDATAEXT == "Cancel")
                        {
                            this.Scanner.DataEventEnabled = true;
                            this.Scanner.DeviceEnabled = true;
                            return;
                        }
                    }
                }
                else //1 - � ���� ����� ���� �������������� ��� � ��������
                {
                    IDDATAEXT = set2.Tables[0].Rows[0][1].ToString();
                }
                adapter.SelectCommand.CommandText = "select DATAEXTPLAIN.PLAIN from BJVVV..DATAEXTPLAIN where DATAEXTPLAIN.IDDATAEXT = " + IDDATAEXT;
                set2 = new DataSet();
                set2.Clear();
                if (adapter.Fill(set2) == 0)//������ ����
                {
                    MessageBox.Show("����������� ������." + IDDATAEXT, "��������!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Scanner.DataEventEnabled = true;
                    this.Scanner.DeviceEnabled = true;
                    return;
                }
                this.printString = set2.Tables[0].Rows[0][0].ToString();
                pd = new PrintDocument();
                pd.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);
                pd.PrinterSettings.PrinterName = this.f2.PrinterName;
                pd.Print();
                this.Scanner.DataEventEnabled = true;
                this.Scanner.DeviceEnabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //MessageBox.Show("end data event");
        }*/

        void pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            PaperSize size;
            Rectangle rectangle;
            int num;
            string str;
            StringFormat format;
            num = 0;
            str = this.printString;
            List<int> Semicolon = new List<int>();
            
            for (int i = 0; i < printString.Length; i++)
            {
                if (str.IndexOf(';', i, 1) != -1)
                {
                    Semicolon.Add(i);
                    num++;
                }
            }
            if (num > 2)
            {
                MessageBox.Show("�������������� ���� ����� ������������ ������!");
                return;
            }
            if (num == 2)
            {
                this.printString = this.printString.Remove(Semicolon[0], Semicolon[1] - Semicolon[0]);
            }
            if (num > 0)
            {
                this.printString = this.printString.Insert(Semicolon[0] + 1, Environment.NewLine);
            }
            /*if (this.printString.Length > 9)
            {
                this.printString = this.printString.Insert(8, " ");
            }*/
            /*if (this.printString.IndexOf(";") >= 0)
            {
                this.printString = this.printString.Remove(this.printString.IndexOf(";") + 1, 1);                
            }*/
            Font printFont;

            printFont = new Font("Arial", 19f);

            size = new PaperSize("bar", 0x9d, 0x2f);
            rectangle = new Rectangle(new Point(0, 0), new Size(0xad, 0x37));
            pd.DefaultPageSettings.PaperSize = size;
            pd.DefaultPageSettings.Margins.Left = 0;
            pd.DefaultPageSettings.Margins.Bottom = 0;
            pd.DefaultPageSettings.Margins.Top = 0;
            pd.DefaultPageSettings.Margins.Right = 0;
            format = new StringFormat();// (0x4000);
            format.LineAlignment = StringAlignment.Near;
            format.Alignment = StringAlignment.Near;
            format.FormatFlags = StringFormatFlags.NoClip;
            e.Graphics.DrawRectangle(Pens.White, rectangle);
            e.Graphics.DrawString(this.printString, printFont, Brushes.Black, rectangle, format);
            
        }


        void Form1_Load(object sender, EventArgs e)
        {
            if (this.f2.PrinterName == "close")
            {
                this.Close();
            }
        }

        void button2_Click(object sender, EventArgs e)
        {
            emu = "U100168031";
            string str;
            IDDATAEXT = "";
            SqlDataAdapter adapter;
            SqlConnection connection;
            DataSet set;
            DataSet set2;
            str = emu;
            adapter = new SqlDataAdapter();
            connection = new SqlConnection(connstr);
            adapter.SelectCommand = new SqlCommand();
            adapter.SelectCommand.Connection = connection;
            adapter.SelectCommand.CommandText = "select BARCODE_UNITS.IDMAIN,IDDATA from BJVVV..BARCODE_UNITS where BARCODE_UNITS.BARCODE = '" + str + "'";
            set = new DataSet();
            if (adapter.Fill(set) == 0)
            {
                MessageBox.Show("����� ����� ��� � ����, ���� �� �� �������� ��������.", "��������!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //this.Scanner.DataEventEnabled = true;
                //this.Scanner.DeviceEnabled = true;
                return;
            }
            adapter.SelectCommand.CommandText = "select DATAEXT.SORT, DATAEXT.ID from BJVVV..DATAEXT " +
                                                "where DATAEXT.IDMAIN = " + set.Tables[0].Rows[0][0].ToString() +
                                                " and DATAEXT.MNFIELD=899 and DATAEXT.MSFIELD='$j' " +
                                                "and DATAEXT.IDDATA = " + set.Tables[0].Rows[0][1].ToString();
            set2 = new DataSet();
            set2.Clear();
            if (adapter.Fill(set2) == 0)// 0 - � ���� ����� ��� ��������������� �����. 1 - � ���� ����� ���� �������������� ��� � ��������
            {
                adapter.SelectCommand.CommandText = "select SORT from DATAEXT where IDDATA = " + set.Tables[0].Rows[0][1].ToString()
                                                    + " and DATAEXT.MNFIELD=899 and DATAEXT.MSFIELD='$b'";
                set2 = new DataSet();
                int countshifr = adapter.Fill(set2);
                if (countshifr == 0)
                {
                    MessageBox.Show("� ����� �������� ���. ������ ����������� ���� ����. ���������� ���������� ����! ����� ���������� ����, �������� ���� ��� ������� ���. ������!");
                    //this.Scanner.DataEventEnabled = true;
                    //this.Scanner.DeviceEnabled = true;
                    return;
                }
                string mhr = set2.Tables[0].Rows[0][0].ToString();
                //���� ��� ��������� � �������������� � ���
                if (mhr.Contains("���������")) //�������������� ���
                {
                    adapter.SelectCommand.CommandText = "select A.SORT as inv, A.ID as invid, A.IDDATA as inviddata, " +
                                                        "B.SORT as shif, B.ID as shifid, B.IDDATA as shifiddata, C.SORT " +
                                                        "from BJVVV..DATAEXT A " +
                                                        "left join BJVVV..DATAEXT B on A.IDDATA=B.IDDATA " +
                                                        "left join BJVVV..DATAEXT C on B.IDDATA=C.IDDATA " +
                                                        "where A.IDMAIN = " + set.Tables[0].Rows[0][0].ToString() +
                                                        " and A.MNFIELD=899 and A.MSFIELD='$p' " +
                                                        "and B.MNFIELD=899 and B.MSFIELD='$j' " +
                                                        "and C.MNFIELD=899 and C.MSFIELD='$b' " +
                                                        "and C.SORT like '%���������%' ";
                }
                else //�������������� ��
                {
                    adapter.SelectCommand.CommandText = "select A.SORT as inv, A.ID as invid, A.IDDATA as inviddata, " +
                                                        "B.SORT as shif, B.ID as shifid, B.IDDATA as shifiddata, C.SORT " +
                                                        "from BJVVV..DATAEXT A " +
                                                        "left join BJVVV..DATAEXT B on A.IDDATA=B.IDDATA " +
                                                        "left join BJVVV..DATAEXT C on B.IDDATA=C.IDDATA " +
                                                        "where A.IDMAIN = " + set.Tables[0].Rows[0][0].ToString() +
                                                        "and A.MNFIELD=899 and A.MSFIELD='$p' " +
                                                        "and B.MNFIELD=899 and B.MSFIELD='$j' " +
                                                        "and C.MNFIELD=899 and C.MSFIELD='$b' " +
                                                        "and C.SORT not like '%���������%' ";
                }

                set2 = new DataSet();
                set2.Clear();
                countshifr = adapter.Fill(set2);
                if (countshifr == 0)
                {
                    MessageBox.Show("��� ����� " + mhr + " ����������� �������������� ����!");
                    //MessageBox.Show("������������ ������ � 0�0001. ���������� � ������������"); //��� ���������! ������ ���� �� �����
                    //Close();
                    return;
                }
                if (countshifr == 1)//������������ �������������� ���� �� ��� ���������.
                {
                    IDDATAEXT = set2.Tables[0].Rows[0][4].ToString();
                }
                if (countshifr >= 2) //���� �������������� ��� � ������, �� ���� ������� ����� �������������� ��������
                {

                    Form3 f3 = new Form3(set2.Tables[0], this);
                    f3.ShowDialog();
                    if (IDDATAEXT == "Cancel")
                    {
                        //this.Scanner.DataEventEnabled = true;
                        //this.Scanner.DeviceEnabled = true;
                        return;
                    }
                }
            }
            else //1 - � ���� ����� ���� �������������� ��� � ��������
            {
                IDDATAEXT = set2.Tables[0].Rows[0][1].ToString();
            }
            adapter.SelectCommand.CommandText = "select DATAEXTPLAIN.PLAIN from BJVVV..DATAEXTPLAIN where DATAEXTPLAIN.IDDATAEXT = " + IDDATAEXT;
            set2 = new DataSet();
            set2.Clear();
            if (adapter.Fill(set2) == 0)//������ ����
            {
                MessageBox.Show("����������� ������." + IDDATAEXT, "��������!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //this.Scanner.DataEventEnabled = true;
                //this.Scanner.DeviceEnabled = true;
                return;
            }
            this.printString = set2.Tables[0].Rows[0][0].ToString();
            pd = new PrintDocument();
            pd.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);
            pd.PrinterSettings.PrinterName = this.f2.PrinterName;
            pd.Print();
            //this.Scanner.DataEventEnabled = true;
            //this.Scanner.DeviceEnabled = true;        }
        }
        void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}