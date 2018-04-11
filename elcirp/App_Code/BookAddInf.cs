using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Web.Extensions;

/// <summary>
/// Сводное описание для BookAddInf
/// </summary>
public abstract class BookAddInf
{
    public BookAddInf() 
    {
        da = new SqlDataAdapter();
        da.SelectCommand = new SqlCommand();
        da.SelectCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/BJVVV"));
        ds = new DataSet();
    }
    public BookAddInf(int idmain,string vkey,string idr)
    {
        this.IDMAIN = idmain;
        this.ViewKey = vkey;
        this.IDReader = idr;
        da = new SqlDataAdapter();
        da.SelectCommand = new SqlCommand();
        da.SelectCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/BJVVV"));
        ds = new DataSet();
    }
    public bool GetAccess()
    {
        da.SelectCommand.Parameters.Clear();
        da.SelectCommand.Parameters.Add("idbase", SqlDbType.Int);
        da.SelectCommand.Parameters.Add("idbook", SqlDbType.Int);
        da.SelectCommand.Parameters["idbase"].Value = 1;
        da.SelectCommand.Parameters["idbook"].Value = this.IDMAIN;
        da.SelectCommand.CommandText = "select * from BookAddInf..ScanInfo where IDBook = @idbook and IDBase = @idbase";
        ds = new DataSet();
        int i = da.Fill(ds, "t");
        return (bool)ds.Tables["t"].Rows[0]["ForAllReader"];
    }
    public bool GetEBook()
    {
        da.SelectCommand.Parameters.Clear();
        da.SelectCommand.Parameters.Add("idbase", SqlDbType.Int);
        da.SelectCommand.Parameters.Add("idbook", SqlDbType.Int);
        da.SelectCommand.Parameters["idbase"].Value = 1;
        da.SelectCommand.Parameters["idbook"].Value = this.IDMAIN;
        da.SelectCommand.CommandText = "select * from BookAddInf..ScanInfo where IDBook = @idbook and IDBase = @idbase";
        ds = new DataSet();
        int i = da.Fill(ds, "t");
        return (bool)ds.Tables["t"].Rows[0]["EBook"];
    }
    public bool GetOldBook()
    {
        da.SelectCommand.Parameters.Clear();
        da.SelectCommand.Parameters.Add("idbase", SqlDbType.Int);
        da.SelectCommand.Parameters.Add("idbook", SqlDbType.Int);
        da.SelectCommand.Parameters["idbase"].Value = 1;
        da.SelectCommand.Parameters["idbook"].Value = this.IDMAIN;
        da.SelectCommand.CommandText = "select * from BookAddInf..ScanInfo where IDBook = @idbook and IDBase = @idbase";
        ds = new DataSet();
        int i = da.Fill(ds, "t");
        return (bool)ds.Tables["t"].Rows[0]["OldBook"];
    }
    public string GetReaderNameByID()
    {
        da.SelectCommand.Parameters.Clear();
        da.SelectCommand.Parameters.Add("idr", SqlDbType.Int);
        try
        {
            da.SelectCommand.Parameters["idr"].Value = int.Parse(this.IDReader);
        }
        catch
        {
            return "";
        }
        da.SelectCommand.CommandText = "select * from Readers..Main where NumberReader = @idr";
        ds = new DataSet();
        int i = da.Fill(ds, "t");
        return 
            ds.Tables["t"].Rows[0]["FamilyName"].ToString() + " " +
            ds.Tables["t"].Rows[0]["Name"].ToString() + " " +
            ds.Tables["t"].Rows[0]["FatherName"].ToString();
    }


    public abstract string GetTitle();
    public abstract string GetAuthor();
    public abstract string GetPath();


    public int IDMAIN;
    public BaseName Baza;
    public bool ForAllReaders;
    public bool EBook;
    public bool OldBook;
    public string ViewKey;
    public string IDReader;
    public enum BaseName { BJVVV = 1, REDKOSTJ = 2 };
    protected SqlDataAdapter da;
    protected DataSet ds;




    public bool UserHaveOrder()
    {
        da.SelectCommand.Parameters.Clear();
        da.SelectCommand.Parameters.Add("idbase", SqlDbType.Int);
        da.SelectCommand.Parameters.Add("idbook", SqlDbType.Int);
        da.SelectCommand.Parameters.Add("idr", SqlDbType.Int);
        da.SelectCommand.Parameters.Add("vkey", SqlDbType.NVarChar);
        da.SelectCommand.Parameters["idbase"].Value = this.Baza;
        da.SelectCommand.Parameters["idbook"].Value = this.IDMAIN;
        da.SelectCommand.Parameters["idr"].Value = this.IDReader;
        da.SelectCommand.Parameters["vkey"].Value = this.ViewKey;

        da.SelectCommand.CommandText = "select * from Reservation_R..ELISSUED where IDMAIN = @idbook and IDREADER = @idr and VIEWKEY = @vkey and BASE = @idbase"+
                        " and cast(cast(getdate() as nvarchar(11)) as datetime) between DATEISSUE and DATERETURN";
        ds = new DataSet();
        int i = da.Fill(ds, "t");
        if (i == 0) return false; else return true;
//        DateTime start = (DateTime)ds.Tables["t"].Rows[0]["DATEISSUE"];
  //      DateTime end = (DateTime)ds.Tables["t"].Rows[0]["DATERETURN"];
        
    }


    public bool GetAgreement()
    {
        da.SelectCommand.Parameters.Clear();
        da.SelectCommand.Parameters.Add("idbase", SqlDbType.Int);
        da.SelectCommand.Parameters.Add("idbook", SqlDbType.Int);
        da.SelectCommand.Parameters.Add("idr", SqlDbType.Int);
        da.SelectCommand.Parameters["idbase"].Value = 1;
        da.SelectCommand.Parameters["idbook"].Value = this.IDMAIN;
        da.SelectCommand.Parameters["idr"].Value = (this.IDReader == null) ? "0" : this.IDReader;
        da.SelectCommand.CommandText = "select * from Reservation_R..AGREEMENT "+
        "where IDMAIN = @idbook and IDREADER = @idr and BASE = @idbase";
        ds = new DataSet();
        int i = da.Fill(ds, "t");
        if (i == 0) return false; else return true;
    }
    public void InsertAgreement()
    {
        da.InsertCommand = new SqlCommand();
        da.InsertCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/BJVVV"));
        da.InsertCommand.Connection.Open();
        da.InsertCommand.Parameters.Clear();
        da.InsertCommand.Parameters.Add("idbase", SqlDbType.Int);
        da.InsertCommand.Parameters.Add("idbook", SqlDbType.Int);
        da.InsertCommand.Parameters.Add("idr", SqlDbType.Int);
        da.InsertCommand.Parameters.Add("dateagr", SqlDbType.DateTime);
        da.InsertCommand.Parameters["idbase"].Value = this.Baza;
        da.InsertCommand.Parameters["idbook"].Value = this.IDMAIN;
        da.InsertCommand.Parameters["idr"].Value = this.IDReader;
        da.InsertCommand.Parameters["dateagr"].Value = DateTime.Now;
        da.InsertCommand.CommandText = "insert into Reservation_R..AGREEMENT " +
        "(IDMAIN,IDREADER,BASE,DATEAGR) values (@idbook,@idr,@idbase,@dateagr)";
        da.InsertCommand.ExecuteNonQuery();
        da.InsertCommand.Connection.Close();
    }
    public void DeleteAgreement()
    {
        da.DeleteCommand = new SqlCommand();
        da.DeleteCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/BJVVV"));
        da.DeleteCommand.Connection.Open();
        da.DeleteCommand.Parameters.Clear();
        da.DeleteCommand.Parameters.Add("idbase", SqlDbType.Int);
        da.DeleteCommand.Parameters.Add("idbook", SqlDbType.Int);
        da.DeleteCommand.Parameters.Add("idr", SqlDbType.Int);
        da.DeleteCommand.Parameters.Add("dateagr", SqlDbType.DateTime);
        da.DeleteCommand.Parameters["idbase"].Value = this.Baza;
        da.DeleteCommand.Parameters["idbook"].Value = this.IDMAIN;
        da.DeleteCommand.Parameters["idr"].Value = this.IDReader;
        da.DeleteCommand.Parameters["dateagr"].Value = DateTime.Now;
        da.DeleteCommand.CommandText = "Delete from Reservation_R..AGREEMENT " +
        " where IDMAIN =@idbook and IDREADER = @idr and BASE = @idbase";
        da.DeleteCommand.ExecuteNonQuery();
        da.DeleteCommand.Connection.Close();
    }

    public void InsertELOPENEDWAR(string sid)
    {
        da.SelectCommand.Parameters.Clear();
        da.SelectCommand.Parameters.Add("idbase", SqlDbType.Int);
        da.SelectCommand.Parameters.Add("idbook", SqlDbType.Int);
        da.SelectCommand.Parameters.Add("sid", SqlDbType.NVarChar);
        da.SelectCommand.Parameters["idbase"].Value = 1;
        da.SelectCommand.Parameters["idbook"].Value = this.IDMAIN;
        da.SelectCommand.Parameters["sid"].Value = sid;
        da.SelectCommand.CommandText = "select top 1 * from Reservation_R..ELOPEN_WITHOUTAUTHRGHT " +
        " where IDMAIN = @idbook and BASE = @idbase and SESSION = @sid order by DATEOPENING desc";
        ds = new DataSet();
        int i = da.Fill(ds, "t");
        if (i > 0) return;
        //DataRow r = ds.Tables["t"].Rows[0];
        
        //if (((DateTime)r["DATEOPENING"]).Date == DateTime.Now.Date)
        //return;
        da.InsertCommand = new SqlCommand();
        da.InsertCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/BJVVV"));
        da.InsertCommand.Connection.Open();
        da.InsertCommand.Parameters.Clear();
        da.InsertCommand.Parameters.Add("idbase", SqlDbType.Int);
        da.InsertCommand.Parameters.Add("idbook", SqlDbType.Int);
        da.InsertCommand.Parameters.Add("idr", SqlDbType.Int);
        da.InsertCommand.Parameters.Add("dateopn", SqlDbType.DateTime);
        da.InsertCommand.Parameters.Add("sid", SqlDbType.NVarChar);
        da.InsertCommand.Parameters["idbase"].Value = this.Baza;
        da.InsertCommand.Parameters["idbook"].Value = this.IDMAIN;
        if (this.IDReader == null) 
            da.InsertCommand.Parameters["idr"].Value = "0";
        else
            da.InsertCommand.Parameters["idr"].Value = this.IDReader;

        da.InsertCommand.Parameters["dateopn"].Value = DateTime.Now;
        da.InsertCommand.Parameters["sid"].Value = sid;
        da.InsertCommand.CommandText = "insert into Reservation_R..ELOPEN_WITHOUTAUTHRGHT " +
        "(IDMAIN,IDREADER,BASE,DATEOPENING,SESSION) values (@idbook,@idr,@idbase,@dateopn,@sid)";
        da.InsertCommand.ExecuteNonQuery();
        da.InsertCommand.Connection.Close();
        
    }

    public void InsertELOPENEDPERIOD(string sessionId)
    {
        throw new NotImplementedException();
    }
}
public class BookAddInfBJ : BookAddInf
{
    public BookAddInfBJ() : base() { }
    public BookAddInfBJ(int idmain,string vkey,string idr)
        : base(idmain,vkey,idr)
    {
        this.Baza = BaseName.BJVVV;
        da.SelectCommand.Parameters.Clear();
        da.SelectCommand.Parameters.Add("idbook", SqlDbType.Int);
        da.SelectCommand.Parameters["idbook"].Value = this.IDMAIN;
        da.SelectCommand.Parameters.Add("idbase", SqlDbType.Int);
        da.SelectCommand.Parameters["idbase"].Value = (int)this.Baza;

        da.SelectCommand.CommandText = "select * from BookAddInf..ScanInfo where IDBook = @idbook and IDBase = @idbase";
        ds = new DataSet();
        int i = da.Fill(ds, "t");
        if (i == 0)
        {
            throw new Exception("Не найдено в базе");
        }
        this.ForAllReaders = (bool)ds.Tables["t"].Rows[0]["ForAllReader"];
        this.EBook = (bool)ds.Tables["t"].Rows[0]["EBook"];
        this.OldBook = (bool)ds.Tables["t"].Rows[0]["OldBook"];

    }

    public override string GetTitle()
    {
        da.SelectCommand.Parameters.Clear();
        da.SelectCommand.Parameters.Add("idbook", SqlDbType.Int);
        da.SelectCommand.Parameters["idbook"].Value = this.IDMAIN;

        da.SelectCommand.CommandText =
            " select PA.PLAIN tit from BJVVV..DATAEXT A " +
            " left join BJVVV..DATAEXTPLAIN PA on PA.IDDATAEXT = A.ID " +
            " where A.IDMAIN = @idbook and A.MNFIELD = 200 and A.MSFIELD = '$a'";
        ds = new DataSet();
        int i = da.Fill(ds, "t");
        return ds.Tables["t"].Rows[0]["tit"].ToString();
    }
    public override string GetAuthor()
    {
        da.SelectCommand.Parameters.Clear();
        da.SelectCommand.Parameters.Add("idbook", SqlDbType.Int);
        da.SelectCommand.Parameters["idbook"].Value = this.IDMAIN;

        da.SelectCommand.CommandText =
            " select PA.PLAIN tit from BJVVV..DATAEXT A " +
            " left join BJVVV..DATAEXTPLAIN PA on PA.IDDATAEXT = A.ID " +
            " where A.IDMAIN = @idbook and A.MNFIELD = 700 and A.MSFIELD = '$a'";
        ds = new DataSet();
        int i = da.Fill(ds, "t");
        if (i == 0)
            return "";
        else
            return ds.Tables["t"].Rows[0]["tit"].ToString();
    }
    public override string GetPath()
    {
        string idmain = this.IDMAIN.ToString();
        string result = "";
        
        switch (idmain.Length)
        {
            case 1:
                result = "000000" + idmain;
                break;
            case 2:
                result = "00000" + idmain;
                break;
            case 3:
                result = "0000" + idmain;
                break;
            case 4:
                result = "000" + idmain;
                break;
            case 5:
                result = "00" + idmain;
                break;
            case 6:
                result = "0" + idmain;
                break;
            case 7:
                result = idmain;
                break;
        }
        return @"BJVVV\"+@result[0] + @"\" + result[1] + result[2] + result[3] + @"\" + result[4] + result[5] + result[6] + @"\JPEG_HQ\";
    }
}
public class BookAddInfRED : BookAddInf
{
    public BookAddInfRED(int idmain,string vkey,string idr)
        : base(idmain,vkey,idr)
    {
        this.Baza = BaseName.REDKOSTJ;
        da.SelectCommand.Parameters.Clear();
        da.SelectCommand.Parameters.Add("idbook", SqlDbType.Int);
        da.SelectCommand.Parameters["idbook"].Value = this.IDMAIN;
        da.SelectCommand.Parameters.Add("idbase", SqlDbType.Int);
        da.SelectCommand.Parameters["idbase"].Value = (int)this.Baza;

        da.SelectCommand.CommandText = "select * from BookAddInf..ScanInfo where IDBook = @idbook and IDBase = @idbase";
        ds = new DataSet();
        int i = da.Fill(ds, "t");
        if (i == 0)
        {
            throw new Exception("Не найдено в базе");
        }
        this.ForAllReaders = (bool)ds.Tables["t"].Rows[0]["ForAllReader"];
        this.EBook = (bool)ds.Tables["t"].Rows[0]["EBook"];
        this.OldBook = (bool)ds.Tables["t"].Rows[0]["OldBook"];

    }
    public override string GetTitle()
    {
        da.SelectCommand.Parameters.Clear();
        da.SelectCommand.Parameters.Add("idbook", SqlDbType.Int);
        da.SelectCommand.Parameters["idbook"].Value = this.IDMAIN;

        da.SelectCommand.CommandText =
            " select PA.PLAIN tit from REDKOSTJ..DATAEXT A " +
            " left join REDKOSTJ..DATAEXTPLAIN PA on PA.IDDATAEXT = A.ID " +
            " where A.IDMAIN = @idbook and A.MNFIELD = 200 and A.MSFIELD = '$a'";
        ds = new DataSet();
        int i = da.Fill(ds, "t");
        return ds.Tables["t"].Rows[0]["tit"].ToString();
    }
    public override string GetAuthor()
    {
        da.SelectCommand.Parameters.Clear();
        da.SelectCommand.Parameters.Add("idbook", SqlDbType.Int);
        da.SelectCommand.Parameters["idbook"].Value = this.IDMAIN;

        da.SelectCommand.CommandText =
            " select PA.PLAIN tit from BJVVV..DATAEXT A " +
            " left join REDKOSTJ..DATAEXTPLAIN PA on PA.IDDATAEXT = A.ID " +
            " where A.IDMAIN = @idbook and A.MNFIELD = 700 and A.MSFIELD = '$a'";
        ds = new DataSet();
        int i = da.Fill(ds, "t");
        if (i == 0) 
            return "";
        else
            return ds.Tables["t"].Rows[0]["tit"].ToString();
    }
    public override string GetPath()
    {
        string idmain = this.IDMAIN.ToString();
        string result = "";

        switch (idmain.Length)
        {
            case 1:
                result = "000000" + idmain;
                break;
            case 2:
                result = "00000" + idmain;
                break;
            case 3:
                result = "0000" + idmain;
                break;
            case 4:
                result = "000" + idmain;
                break;
            case 5:
                result = "00" + idmain;
                break;
            case 6:
                result = "0" + idmain;
                break;
            case 7:
                result = idmain;
                break;
        }
        return @"REDKOSTJ\"+@result[0] + @"\" + result[1] + result[2] + result[3] + @"\" + result[4] + result[5] + result[6] + @"\";
    }
}