using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.IO;

namespace AddLitresAccounts
{
    class Program
    {
        static void Main(string[] args)
        {
            #region вставить новые пароли литрес

            SqlDataAdapter da = new SqlDataAdapter();
            //da.SelectCommand = new SqlCommand();
            //da.SelectCommand.Connection = new SqlConnection();
            //da.SelectCommand.Connection.ConnectionString = "Data Source=192.168.4.25,1443;Initial Catalog=Reservation_R;Persist Security Info=True;User ID=sasha;Password=Corpse536";
            //da.SelectCommand.CommandText = "select * from BJVVV..DATAEXT where MNFIELD = 230";
            //DataSet ds = new DataSet();
            //int i = da.Fill(ds, "t");
            //da.SelectCommand.CommandText = "select * from BJVVV..DATAEXTPLAIN where ID = 3";
            //i = da.Fill(ds, "t");

            da = new SqlDataAdapter();
            da.InsertCommand = new SqlCommand();
            da.InsertCommand.Connection = new SqlConnection();
            da.InsertCommand.Connection.ConnectionString = "Data Source=192.168.4.25,1443;Initial Catalog=Reservation_R;Persist Security Info=True;User ID=sasha;Password=Corpse536;Connect Timeout=1200";
            da.InsertCommand.Connection.Open();
            StreamReader sr = new StreamReader(@"f:\network\Lib_100S36522.txt");
            string account;
            while (sr.Peek() >= 0)
            {
                account = sr.ReadLine();
                da.InsertCommand.Parameters.Clear();
                da.InsertCommand.Parameters.AddWithValue("login", account.Split(',')[0]);
                da.InsertCommand.Parameters.AddWithValue("pwd", account.Split(',')[1]);
                da.InsertCommand.CommandText = "insert into LITRES..ACCOUNTS (LRLOGIN,LRPWD,CREATED) values (@login, @pwd, getdate())";
                //da.InsertCommand.ExecuteNonQuery();
            }
            da.InsertCommand.Connection.Close();


            #endregion

        }
    }
}
