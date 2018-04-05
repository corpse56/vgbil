using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Data;

/// <summary>
/// Сводное описание для ReaderLib
/// </summary>
public class ReaderLib
{
    public ReaderLib(string login, string sess)
    {
        SqlDataAdapter DA = new SqlDataAdapter();
        DA.SelectCommand = new SqlCommand();
        DA.SelectCommand.Connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["BJVVV"].ConnectionString);
        DA.SelectCommand.Parameters.Clear();
        DA.SelectCommand.Parameters.AddWithValue("LOGIN", login.ToLower());
        DA.SelectCommand.CommandText = "select USERS.ID id,USERS.NAME name,USERS.LOGIN login,dpt.NAME dname from BJVVV..USERS join BJVVV..LIST_8 dpt on USERS.DEPT = dpt.ID where lower([LOGIN]) = @LOGIN";

        DataSet DS = new DataSet();
        int recc = DA.Fill(DS, "Employee");
        this.ID = DS.Tables["Employee"].Rows[0]["id"].ToString();
        this.Login = DS.Tables["Employee"].Rows[0]["login"].ToString();
        this.Name = DS.Tables["Employee"].Rows[0]["name"].ToString();
        this.Dep = DS.Tables["Employee"].Rows[0]["dname"].ToString();
        this.Session = sess;
        //DA.SelectCommand.Connection.Close();


    }
    public string Name;
    public string Dep;
    public string ID;
    public string Login;
    public string Session;
}
