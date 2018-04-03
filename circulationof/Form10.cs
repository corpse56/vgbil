using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Circulation
{
    public partial class Form10 : Form
    {
        string IDOldReader;
        dbReader reader;
        public bool canShow;
        public Form10(dbReader reader_)
        {
            InitializeComponent();
            this.reader = reader_;
            if (!FindReaderInOldBase(reader))
            {
                MessageBox.Show("Был произведен поиск по фамилии читателя в старой базе для переноса его комментариев и долгов, но читателей с такими фамилиями не нашлось.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                SetChecked(reader);
                canShow = false;
            }
            else
            {
                canShow = true;
            }
            //FindReaderInOldBase(new DBWork.dbReader(22577));

        }
        private bool FindReaderInOldBase(dbReader reader)
        {
            Conn.SQLDA.SelectCommand.CommandText = " select * from AbonOld..Main " +
                                                   " where lower(FullName) like ltrim(rtrim( (lower('" +
                                                   reader.Surname + " " + reader.Name + " " + reader.SecondName + " " + "'))))+'%' ";

            DataSet DS = new DataSet();
            int cnt = Conn.SQLDA.Fill(DS, "old");
            if (cnt == 0)
            {
                return false;
            }
            dataGridView1.DataSource = DS.Tables["old"];
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].HeaderText = "Номер читателя";
            dataGridView1.Columns[2].HeaderText = "ФИО";
            dataGridView1.Columns[3].HeaderText = "Дата рождения";
            dataGridView1.Columns[4].HeaderText = "Документ";
            dataGridView1.Columns[5].HeaderText = "Вид абонемента";
            dataGridView1.Columns[6].HeaderText = "Телефон";
            dataGridView1.Columns[7].HeaderText = "Учебное заведение";
            dataGridView1.Columns[8].HeaderText = "Адрес";
            dataGridView1.Columns[8].Visible = false;
            dataGridView1.Columns[9].HeaderText = "Индекс";
            dataGridView1.Columns[10].HeaderText = "Страна";
            dataGridView1.Columns[11].HeaderText = "Область";
            dataGridView1.Columns[12].HeaderText = "Город";
            dataGridView1.Columns[13].HeaderText = "Улица";
            dataGridView1.Columns[14].HeaderText = "Дом";
            dataGridView1.Columns[15].HeaderText = "Индекс2";
            dataGridView1.Columns[16].HeaderText = "Страна2";
            dataGridView1.Columns[17].HeaderText = "Область2";
            dataGridView1.Columns[18].HeaderText = "Город2";
            dataGridView1.Columns[19].HeaderText = "Улица2";
            dataGridView1.Columns[20].HeaderText = "Дом2";
            dataGridView1.Columns[21].HeaderText = "Дата регистрации";
            dataGridView1.Columns[22].HeaderText = "Дата перерегистрации";
            dataGridView1.Columns[23].HeaderText = "Потерян";
            dataGridView1.Columns[24].HeaderText = "Научная степень";
            dataGridView1.Columns[25].HeaderText = "Отдел";
            dataGridView1.Columns[26].HeaderText = "Сектор";
            dataGridView1.Columns[27].HeaderText = "Номер комнаты";
            dataGridView1.Columns[28].HeaderText = "Местонахождение";
            dataGridView1.Columns[29].HeaderText = "Email";
            dataGridView1.Columns[30].HeaderText = "Отделение библиотеки";
            dataGridView1.Columns[31].HeaderText = "Коментарии";
            dataGridView1.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            MethodsForCurBase.FormTable(reader, dataGridView2);
            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            if (dataGridView1.Rows.Count == 0)
            {
                SetChecked(reader);
            }
            else
            {
                IDOldReader = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                Notes nt = IsHaveCommentsInOldBase(IDOldReader);
                if ((nt.Comment != "(нет комментариев)") || (nt.Note != "(нет примечаний)"))
                {
                    if (nt.Note == "(нет примечаний)") nt.Note = "";
                    if (nt.Comment == "(нет комментариев)") nt.Comment = "";

                    InsertComment(IDOldReader, reader, nt);
                    UpdateBooks(IDOldReader, reader);
                    SetChecked(reader);
                }
                else
                {
                    UpdateBooks(IDOldReader, reader);
                    SetChecked(reader);
                    MessageBox.Show("Данные из старой таблицы успешно перенесены!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                    return;
                }
                MessageBox.Show("Данные из старой таблицы успешно перенесены!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            Close();
        }

        private void SetChecked(dbReader reader)
        {
            SqlCommand cmd = new SqlCommand("[Readers]..[setchk]", Conn.ReadersCon);
            if (cmd.Connection.State == ConnectionState.Closed)
            {
                cmd.Connection.Open();
            }
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@IDR", SqlDbType.Int);
            cmd.Parameters["@IDR"].Value = reader.id;
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();

        }

        private void InsertComment(string idrold, dbReader reader, Notes nts)
        {
            SqlCommand cmd = new SqlCommand("[Readers]..[updcomment]", Conn.ReadersCon);
            cmd.CommandType = CommandType.StoredProcedure;
            if (cmd.Connection.State == ConnectionState.Closed)
            {
                cmd.Connection.Open();
            }
            cmd.Parameters.Add("@IDR", SqlDbType.Int);
            cmd.Parameters.Add("@COMMENT", SqlDbType.NVarChar);
            cmd.Parameters.Add("@NOTE", SqlDbType.NVarChar);
            cmd.Parameters["@IDR"].Value = reader.id;
            cmd.Parameters["@COMMENT"].Value = nts.Comment;
            cmd.Parameters["@NOTE"].Value = nts.Note;

            cmd.ExecuteNonQuery();
            cmd.Connection.Close();

        }
        private int UpdateBooks(string idrold, dbReader reader)
        {
            SqlCommand cmd = new SqlCommand("[Reservation_R]..[updbooks]", Conn.ZakazCon);
            if (Conn.ZakazCon.State != ConnectionState.Open) Conn.ZakazCon.Open();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@IDR", SqlDbType.Int);
            cmd.Parameters.Add("@IDROLD", SqlDbType.NVarChar);
            cmd.Parameters["@IDR"].Value = reader.id;
            cmd.Parameters["@IDROLD"].Value = idrold;

            int i = cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            return i;
        }

        private Notes IsHaveCommentsInOldBase(string id)
        {
            Conn.SQLDA.SelectCommand.CommandText = "select * from AbonOld..Main where IDReader = '" + id+"'";
            DataSet DS = new DataSet();
            int cnt = Conn.SQLDA.Fill(DS, "old");
            Notes n = new Notes();
            if (cnt == 0)
            {
                n.Comment = "(нет комментариев)";
                n.Note = "(нет примечаний)";
                return n;
            }
            if (DS.Tables["old"].Rows[0]["Komentarii"].ToString() != "")
            {
                n.Comment = DS.Tables["old"].Rows[0]["Komentarii"].ToString();
            }
            else
            {
                n.Comment = "(нет комментариев)";
            }
            if (DS.Tables["old"].Rows[0]["EducationalInstitution"].ToString() != "")
            {
                n.Note = DS.Tables["old"].Rows[0]["EducationalInstitution"].ToString();
            }
            else
            {
                n.Note = "(нет примечаний)";
            }
            return n;

        }
        private struct Notes
        {
            public string Comment;
            public string Note;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SetChecked(reader);
            Close();
        }



    }
}
