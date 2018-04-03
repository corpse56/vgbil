using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Text;
using System.Security.Cryptography;
using System.Globalization;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Drawing;
using DataProviderAPI.ValueObjects;
using DataProviderAPI.Loaders;



[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]


// Чтобы разрешить вызывать веб-службу из сценария с помощью ASP.NET AJAX, раскомментируйте следующую строку. 
// [System.Web.Script.Services.ScriptService]
public class Service : System.Web.Services.WebService
{

    public Service () {

        //Раскомментируйте следующую строку в случае использования сконструированных компонентов 
        //InitializeComponent(); 
    }

    


    //[WebMethod]
    public DataSet GetReaderInfoDataSet(int NumberReader)
    {
        SqlDataAdapter da = new SqlDataAdapter();
        da.SelectCommand = new SqlCommand();
        da.SelectCommand.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ReadersConnection_OnlyRead"].ConnectionString);
        da.SelectCommand.Parameters.AddWithValue("NumberReader", NumberReader);
        da.SelectCommand.CommandText = "select FamilyName, Name, FatherName, DateBirth from Readers..Main where NumberReader = @NumberReader";
        DataSet ds = new DataSet();
        int cnt = da.Fill(ds);
        if (cnt == 0) throw new Exception("Читатель не найден!");
        return ds;
    }

    [WebMethod(Description = "Возвращает информацию о пользователе. Если пользователь не найден или входной параметр имеет неправильный формат, генерируется исключение.")]
    public ReaderInfo GetReaderInfo(string NumberReader)
    {
        //int NumReader = 0;
        //if (!int.TryParse(NumberReader, out NumReader))
        //{
        //    throw new Exception("\""+NumberReader+"\"  не является корректным номером читателя");
        //}

        //SqlDataAdapter da = new SqlDataAdapter();
        //da.SelectCommand = new SqlCommand();
        //da.SelectCommand.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ReadersConnection_OnlyRead"].ConnectionString);
        //da.SelectCommand.Parameters.AddWithValue("NumberReader", NumberReader);
        //da.SelectCommand.CommandText = "select FamilyName, Name, FatherName, DateBirth, TypeReader from Readers..Main where NumberReader = @NumberReader";
        //DataSet ds = new DataSet();
        //int cnt = da.Fill(ds);
        //if (cnt == 0) throw new Exception("Читатель не найден!");
        //ReaderInfo ri = new ReaderInfo();
        //ri.FamilyName = ds.Tables[0].Rows[0]["FamilyName"].ToString();
        //ri.Name = ds.Tables[0].Rows[0]["Name"].ToString();
        //ri.FatherName = ds.Tables[0].Rows[0]["FatherName"].ToString();
        //ri.DateBirth = (DateTime)ds.Tables[0].Rows[0]["DateBirth"];
        //ri.IsRemoteReader = (bool)ds.Tables[0].Rows[0]["TypeReader"];
        //return ri;
        return this.GetUser(NumberReader);
    }




    [WebMethod(Description = "Возвращает true при успешной авторизации. Во всех остальных случаях генерируется исключение. Если пользователь не найден или входной параметр имеет неправильный формат, генерируется исключение. Возможные исключения: "+
                             " \n1. \"Читатель не найден.\" Означает, что читатель не найден в базе ни по номеру читательского билета, ни по номеру социальной карты, ни по email." +
                             " \n2. \"Таких Email найдено больше одного! Введите номер читателя!\"  В базе имеет 250 повторяющихся email. Введен email из этого списка. "+
                             " Невозможно идентифицировать. В этом случае читателя придётся попросить указать номер читательского билета в качестве логина." +
                             " \n3. \"Неверный пароль.\" Означает, что читатель найден, но введён неверный пароль.")]

    public string Authorize(string login, string password)
    {
        string CommandText = "";
        int NumberReader = 0;
        bool Check = false;
        if (int.TryParse(login, out NumberReader))
        {
            CommandText = "select * from Readers..Main where NumberReader = @Login";
        } else
            if (login.Length == 19)
            {
                for (int i = 0; i < 19; i++)
                {
                    if (!int.TryParse(login[i].ToString(), out NumberReader))
                    {
                        Check = true;
                        break;
                    }
                }
                if (!Check)//значит 19 цифр. типа номер социалки вбил
                {
                    CommandText = "select * from Readers..Main where NumberSC = @Login";
                }
            }
            else //не номер, и не социалка, значит email
            {
                CommandText = "select * from Readers..Main where Email = @Login";
            }


        SqlDataAdapter da = new SqlDataAdapter();
        da.SelectCommand = new SqlCommand();
        da.SelectCommand.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ReadersConnection_OnlyRead"].ConnectionString);
        da.SelectCommand.Parameters.AddWithValue("Login", login);
        da.SelectCommand.CommandText = CommandText;
        DataSet ds = new DataSet();
        int cnt = da.Fill(ds);
        if (cnt == 0) throw new Exception("Читатель не найден!");
        if (cnt > 1) throw new Exception("Таких Email найдено больше одного! Введите номер читателя!");

        string Salt = ds.Tables[0].Rows[0]["WordReg"].ToString();

        string HashedPwd = HashPass(password, Salt);

        string result = ds.Tables[0].Rows[0]["NumberReader"].ToString();

        da.SelectCommand.Parameters.Clear();
        da.SelectCommand.Parameters.AddWithValue("NumberReader", result);
        da.SelectCommand.Parameters.AddWithValue("Password", HashedPwd);
        da.SelectCommand.CommandText = "select * from Readers..Main where NumberReader = @NumberReader and Password = @Password ";
        ds = new DataSet();
        cnt = da.Fill(ds);

        if (cnt == 0) throw new Exception("Неверный пароль.");

        return result;
    }
    [WebMethod(Description = "Принимает пароль и соль. Возвращает хэш пароля")]
    public String HashPass(String strPassword, String strSol)
    {
        String strHashPass = String.Empty;
        byte[] bytes = Encoding.Unicode.GetBytes(strSol + strPassword);
        //создаем объект для получения средст шифрования 
        SHA256CryptoServiceProvider CSP = new SHA256CryptoServiceProvider();
        //вычисляем хеш-представление в байтах 
        byte[] byteHash = CSP.ComputeHash(bytes);
        //формируем одну цельную строку из массива 
        foreach (byte b in byteHash)
        {
            strHashPass += string.Format("{0:x2}", b);
        }
        return strHashPass;
    }

    //[WebMethod(Description = "Восстановление пароля. Возвращает true при успешном восстановлении пароля. Во всех остальных случаях генерируется исключение.")]
    public bool PasswordRecovery(string login, string familyName, string name, string fatherName, string dateBirth, string pwd1, string pwd2)
    {

        if (pwd1 != pwd2) throw new Exception("Пароль и его подтверждение не совпадают!");
        string CommandText = "";
        int NumberReader = 0;
        bool Check = false;
        CultureInfo provider = CultureInfo.InvariantCulture;
        DateTime DateBirth;
        if (!DateTime.TryParseExact(dateBirth, "dd.MM.yyyy", provider, DateTimeStyles.None, out DateBirth)) throw new Exception("Неверный формат даты.");

        if (login.Length == 6 && int.TryParse(login, out NumberReader))
        {
            CommandText = "select * from Readers..Main where NumberReader = @Login";
        }
        else
            if (login.Length == 19)
            {
                for (int i = 0; i < 19; i++)
                {
                    if (!int.TryParse(login[i].ToString(), out NumberReader))
                    {
                        Check = true;
                        break;
                    }
                }
                if (!Check)//значит 18 цифр. типа номер социалки вбил
                {
                    CommandText = "select * from Readers..Main where NumberSC = @Login";
                }
            }
            else //не номер, и не социалка, значит email
            {
                CommandText = "select * from Readers..Main where Email = @Login";
            }

        SqlDataAdapter da = new SqlDataAdapter();
        da.SelectCommand = new SqlCommand();
        da.SelectCommand.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ReadersConnection_OnlyRead"].ConnectionString);
        da.SelectCommand.Parameters.AddWithValue("Login", login);
        da.SelectCommand.CommandText = CommandText;
        DataSet ds = new DataSet();
        int cnt = da.Fill(ds);
        if (cnt == 0) throw new Exception("Читатель не найден!");
        if (cnt > 1) throw new Exception("Таких Email найдено больше одного! Необходимо указать номер читателя номер читателя!");


        NumberReader = Convert.ToInt32(ds.Tables[0].Rows[0]["NumberReader"]);


        da.SelectCommand.Parameters.Clear();
        da.SelectCommand.Parameters.AddWithValue("NumerReader", NumberReader);
        da.SelectCommand.Parameters.AddWithValue("FamilyName", familyName);
        da.SelectCommand.Parameters.AddWithValue("Name", name);
        da.SelectCommand.Parameters.AddWithValue("DateBirth", DateBirth);

        if (fatherName == "")
        {
            da.SelectCommand.CommandText = "select * from Readers.dbo.Main where FamilyName = @FamilyName and [Name] = @Name and FatherName is null and  DateBirth = @DateBirth and NumberReader = @NumberReader";
        }
        else
        {
            da.SelectCommand.Parameters.AddWithValue("FatherName", fatherName);
            da.SelectCommand.CommandText = "select * from Readers.dbo.Main where FamilyName = @FamilyName and [Name] = @Name and FatherName = @FatherName and  DateBirth = @DateBirth and NumberReader = @NumberReader";
        }

        ds = new DataSet();
        int hit = da.Fill(ds, "hit");
        if (hit == 0)
        {
            throw new Exception("Введённые данные не найдены!");
        }

        da.UpdateCommand = new SqlCommand();
        da.UpdateCommand.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ReadersConnection_OnlyReadWrite"].ConnectionString);
        da.UpdateCommand.Connection.Open();

        da.UpdateCommand.Parameters.Add("pass", SqlDbType.NVarChar);
        da.UpdateCommand.Parameters.Add("wordreg", SqlDbType.NVarChar);
        string wordreg = RndWordReg(32);
        string passw = HashPass(pwd1, wordreg);
        da.UpdateCommand.Parameters["pass"].Value = passw;
        da.UpdateCommand.Parameters["wordreg"].Value = wordreg;
        da.UpdateCommand.Parameters.AddWithValue("NumberReader", NumberReader);
        da.UpdateCommand.CommandText = "update Readers..Main set Password = @pass,WordReg = @wordreg where NumberReader = @NumberReader";
        int y = da.UpdateCommand.ExecuteNonQuery();
        da.UpdateCommand.Connection.Close();
        return true;
    }
    [WebMethod(Description = "Получить тип логина. Возвращает NumberReader, если введён номер читательского билета. "+
                             " Возвращает SocialCardNumber если введён номер социальной карты. Возвращает Email если введён Email. "+
                             " В остальных случаях возвращает NotDefined.")]
    public string GetLoginType(string login)
    {
        int NumberReader = 0;
        bool Check = false;
        if (login.Length == 6 && int.TryParse(login, out NumberReader))
        {
            return "NumberReader";
        }
        else
            if (login.Length == 19)
            {
                for (int i = 0; i < 19; i++)
                {
                    if (!int.TryParse(login[i].ToString(), out NumberReader))
                    {
                        Check = true;
                        break;
                    }
                }
                if (!Check)//значит 18 цифр. типа номер социалки вбил
                {
                    return "SocialCardNumber";
                }
            }
            else
                if (Regex.IsMatch(login,
                   @"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))" +
                   @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$"))//не номер, и не социалка, значит email. проверяем формат
                {
                    return "Email";
                }
                else
                {
                    return "NotDefined";
                }
        return "NotDefined";
    }
    public String RndWordReg(int nLength)
    {
        String WordReg = "";
        byte[] bRandom = new byte[1];
        RNGCryptoServiceProvider Gen = new RNGCryptoServiceProvider();
        int i = 0;
        while (i < nLength)
        {
            Gen.GetBytes(bRandom);
            if (((bRandom[0] >= 48) && (bRandom[0] <= 57)) || ((bRandom[0] >= 65) && (bRandom[0] <= 90)) ||
                ((bRandom[0] >= 97) && (bRandom[0] <= 122)))
            {
                WordReg += Convert.ToChar(bRandom[0]);
                i++;
            }
        }
        return WordReg;
    }

    [WebMethod(Description = "Возвращает информацию о пользователе. Если пользователь не найден или входной параметр имеет неправильный формат, генерируется исключение. ")]
    public ReaderInfo GetUser(string login)
    {
        string CommandText = "";
        int NumberReader = 0;
        bool Check = false;
        if (int.TryParse(login, out NumberReader))
        {
            CommandText = "select * from Readers..Main where NumberReader = @Login";
        }
        else
            if (login.Length == 19)
            {
                for (int i = 0; i < 19; i++)
                {
                    if (!int.TryParse(login[i].ToString(), out NumberReader))
                    {
                        Check = true;
                        break;
                    }
                }
                if (!Check)//значит 19 цифр. типа номер социалки вбил
                {
                    CommandText = "select * from Readers..Main where NumberSC = @Login";
                }
            }
            else //не номер, и не социалка, значит email
            {
                CommandText = "select * from Readers..Main where Email = @Login";
            }
        SqlDataAdapter da = new SqlDataAdapter();
        da.SelectCommand = new SqlCommand();
        da.SelectCommand.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ReadersConnection_OnlyRead"].ConnectionString);
        da.SelectCommand.Parameters.AddWithValue("Login", login);
        da.SelectCommand.CommandText = CommandText;
        DataSet ds = new DataSet();
        int cnt = da.Fill(ds);
        if (cnt == 0) throw new Exception("Читатель не найден!");
        if (cnt > 1) throw new Exception("Таких Email найдено больше одного! Введите номер читателя!");

        string Salt = ds.Tables[0].Rows[0]["WordReg"].ToString();

        string HashedPwd = HashPass(ds.Tables[0].Rows[0]["Password"].ToString(), Salt);


        ReaderInfo ri = new ReaderInfo();
        ri.NumberReader = ds.Tables[0].Rows[0]["NumberReader"].ToString();
        ri.FamilyName = ds.Tables[0].Rows[0]["FamilyName"].ToString();
        ri.Name = ds.Tables[0].Rows[0]["Name"].ToString();
        ri.FatherName = ds.Tables[0].Rows[0]["FatherName"].ToString();
        ri.DateBirth = (DateTime)ds.Tables[0].Rows[0]["DateBirth"];
        ri.IsRemoteReader = (bool)ds.Tables[0].Rows[0]["TypeReader"];
        ri.BarCode = ds.Tables[0].Rows[0]["BarCode"].ToString();
        ri.DateRegistration = (DateTime)ds.Tables[0].Rows[0]["DateRegistration"];
        ri.DateReRegistration = (DateTime)ds.Tables[0].Rows[0]["DateReRegistration"];
        ri.Email = ds.Tables[0].Rows[0]["Email"].ToString();
        ri.HashedPassword = ds.Tables[0].Rows[0]["Password"].ToString();
        ri.MobileTelephone = ds.Tables[0].Rows[0]["MobileTelephone"].ToString();
        ri.Salt = ds.Tables[0].Rows[0]["WordReg"].ToString();
        
        try
        {
            ri.WorkDepartment = Convert.ToInt32(ds.Tables[0].Rows[0]["WorkDepartment"]);
        }
        catch
        {
            ri.WorkDepartment = 0;
        }
            
        return ri;
    }

    
    [WebMethod(Description = "Вставляет пин BJVVV в корзину личного кабинета. Генерирует исключение, если есть ошибки подключения к БД." +
                             " Если PIN пустой - генерируется исключение. Если IDSession пустой - генерируется исключение. Возвращает true если операция прошла успешно.  ")]
    public bool InsertIntoBasket(int PIN, string IDSession)
    {
        if ((PIN == null) || (PIN == 0))
        {
            throw new Exception("PIN не может быть пустым или равняться нулю!");
        }
        if ((IDSession == null) || (IDSession == ""))
        {
            throw new Exception("IDSession не может быть пустым!");
        }
        SqlDataAdapter da = new SqlDataAdapter();
        da.InsertCommand = new SqlCommand();
        da.InsertCommand.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ReadersConnection_Basket"].ConnectionString);
        da.InsertCommand.Parameters.AddWithValue("PIN", PIN);
        da.InsertCommand.Parameters.AddWithValue("IDSession", IDSession);
        da.InsertCommand.Connection.Open();
        da.InsertCommand.CommandText = "insert into TECHNOLOG_VVV..USERLIST (session, idbook, dt) values (@IDSession, @PIN, getdate())";
        try
        {
            int cnt = da.InsertCommand.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw new Exception("Ничего не вставлено! " + ex.Message);
        }
        da.InsertCommand.Connection.Close();
        return true;
    }

    [WebMethod(Description = "Вставляет массив пинов BJVVV в корзину личного кабинета. Генерирует исключение, если есть ошибки подключения к БД." +
                             " Если PIN пустой - генерируется исключение. Если IDSession пустой - генерируется исключение. Возвращает true если операция прошла успешно.  ")]
    public bool InsertArrayIntoBasket(int[] PINs, string IDSession)
    {

        if ((PINs == null) || (PINs.Length == 0))
        {
            throw new Exception("Массив PINs не может быть пустым или равняться нулю!");
        }
        if ((IDSession == null) || (IDSession == ""))
        {
            throw new Exception("IDSession не может быть пустым!");
        }
        SqlDataAdapter da = new SqlDataAdapter();
        da.InsertCommand = new SqlCommand();
        da.InsertCommand.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ReadersConnection_Basket"].ConnectionString);
        da.InsertCommand.Parameters.Add( "PIN", SqlDbType.Int);
        da.InsertCommand.Parameters.Add( "IDSession", SqlDbType.NVarChar);
        da.InsertCommand.Parameters["IDSession"].Value = IDSession;

        da.InsertCommand.Connection.Open();

        foreach (int pin in PINs)
        {
            da.InsertCommand.Parameters["PIN"].Value = pin;

            da.InsertCommand.CommandText = "insert into TECHNOLOG_VVV..USERLIST (session, idbook, dt) values (@IDSession, @PIN, getdate())";
            try
            {
                int cnt = da.InsertCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Запись "+pin+" не вставлена! " + ex.Message);
            }
        }
        da.InsertCommand.Connection.Close();
        return true;
    }
    [WebMethod(Description="Получает статус экземпляра книги по инвентарному номеру. Принимает IDDATA и идентификатор базы. BJVVV - основной фонд, BRIT_SOVET - фонд британского совета,"+
                           " BJACC - Амекриканский культурный центр, BJFCC - французский культурный центр, BJSCC - Центр славянской культуры" )]
    public string GetExemplarStatus(string IDDATA, string BaseName)
    {
        SearchResultSet res = new SearchResultSet();
        string result = "unknown";
        if ((BaseName.ToLower() == "litres") || (BaseName.ToLower() == "pearson") || (IDDATA.ToLower() == "ebook"))
        {
            res.id = IDDATA;
            res.availability = "available";
            result = JsonConvert.SerializeObject(res, Newtonsoft.Json.Formatting.Indented);
            return result;
        }
        if (IDDATA == string.Empty)
        {
            res.id = IDDATA;
            res.availability = "unknown";
            result = JsonConvert.SerializeObject(res, Newtonsoft.Json.Formatting.Indented);
            return result;
        }

        SqlDataAdapter da = new SqlDataAdapter();
        da.SelectCommand = new SqlCommand();
        da.SelectCommand.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["BookStatusConnection"].ConnectionString);
        da.SelectCommand.Parameters.Add("IDDATA", SqlDbType.Int).Value = IDDATA;
        //da.SelectCommand.Parameters["IDDATA"].Value = IDDATA;
        da.SelectCommand.Parameters.Add("RET", SqlDbType.NVarChar, 200).Direction = ParameterDirection.Output;
        da.SelectCommand.CommandType = CommandType.StoredProcedure;
        switch (BaseName)
        {
            case "BJVVV":
                da.SelectCommand.Parameters.Add("INV", SqlDbType.NVarChar).Value = "";
                da.SelectCommand.CommandText = "Reservation_R.dbo.ForOPAC_MF";
                break;
            case "BJACC":
                da.SelectCommand.CommandText = "Reservation_R.dbo.ForOPAC_ACC";
                break;
            case "BJFCC":
                da.SelectCommand.CommandText = "Reservation_R.dbo.ForOPAC_FCC";
                break;
            case "BJSCC":
                da.SelectCommand.CommandText = "Reservation_R.dbo.ForOPAC_SCC";
                break;
            case "BRIT_SOVET":
                da.SelectCommand.CommandText = "Reservation_R.dbo.ForOPAC_BRIT_SOVET";
                break;
            case "REDKOSTJ":
                res.id = IDDATA;
                res.availability = "available";
                result = JsonConvert.SerializeObject(res, Newtonsoft.Json.Formatting.Indented);
                return result;
            default:
                throw new Exception("неверное имя базы");
        }

        da.SelectCommand.Connection.Open();
        da.SelectCommand.ExecuteNonQuery();
        da.SelectCommand.Connection.Close();


        result = da.SelectCommand.Parameters["RET"].Value.ToString().ToLower();

        if (result.Contains("списано")) result = "unavailable";
        else if (result.Contains("занято")) result = "unavailable";
        else if (result.Contains("свободно")) result = "available";
        else if (result.Contains("заказано")) result = "unavailable";
        else if (result.Contains("бронеполка")) result = "unavailable";
        else if (result.Contains("принят")) result = "unavailable";
        else if (result.Contains("подготовлен")) result = "available";
        else result = "available";

        res = new SearchResultSet();
        res.id = IDDATA.ToString();
        res.availability = result;
        result = JsonConvert.SerializeObject(res, Newtonsoft.Json.Formatting.Indented);
        return result;

    }
    [WebMethod(Description = "Получает статус книги по id. Принимает ID книги из VuFind.")]
    public string GetBookStatus(string id)
    {

        SqlDataAdapter da = new SqlDataAdapter();
        da.SelectCommand = new SqlCommand();
        da.SelectCommand.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["BookStatusConnection"].ConnectionString);

        string result = "";

        string fund = id.Substring(0, id.LastIndexOf("_")).ToUpper();
        string ID = id.Substring(id.LastIndexOf("_") + 1);
        SearchResultSet res;
        switch (fund)
        {
            case "BJVVV":
            case "BJACC":
            case "BJFCC":
            case "BJSCC":
            case "BRIT_SOVET":
                break;
            case "REDKOSTJ":
            case "PEARSON":
            case "LITRES":
                result = "available";
                res = new SearchResultSet();
                res.id = id;
                res.availability = result;
                result = JsonConvert.SerializeObject(res, Newtonsoft.Json.Formatting.Indented);
                return result;
            case "PERIOD":
                result = "unknown";
                res = new SearchResultSet();
                res.id = id;
                res.availability = result;
                result = JsonConvert.SerializeObject(res, Newtonsoft.Json.Formatting.Indented);
                return result;
            default:
                result = "unknown";
                res = new SearchResultSet();
                res.id = id;
                res.availability = result;
                result = JsonConvert.SerializeObject(res, Newtonsoft.Json.Formatting.Indented);
                return result;
                
        }
        string[] exemplars = GetExemplars(fund, ID);
        int booked = 0;
        int busy = 0;
        int available = 0;
        int unknown = 0;
        foreach (string exemplar in exemplars)
        {
            string status = GetExemplarStatus(exemplar, fund);
            if (status.Contains( "available" ))
            {
                available++;
                continue;
            }
            if (status.Contains( "busy"))
            {
                busy++;
                continue;
            }
            if (status.Contains("booked"))//booked считаем как unavailiable, чтобы в каталоге только два статуса было доступен/недоступен
            {
                busy++;
                continue;
            }
            if (status.Contains( "unknown"))
            {
                unknown++;
                continue;
            }
        }
        if (exemplars.Length == 0)
        {
            result = "unkonown";
        }
        else if (available > 0) result = "available";
            else if (busy == exemplars.Length) result = "unavailable";
                else if (unknown == exemplars.Length) result = "unkonown";
                    else if (booked > 0) result = "unavailable";
                        else result = "unknown";

        res = new SearchResultSet();
        res.id = id;
        res.availability = result;
        result = JsonConvert.SerializeObject(res, Newtonsoft.Json.Formatting.Indented);
        return result;

    }

    public class SearchResultSet
    {
        public string id;
        public string availability;
        //public string availability_message;
    }

    private string[] GetExemplars(string fund, string id)
    {
        SqlDataAdapter da = new SqlDataAdapter();
        da.SelectCommand = new SqlCommand();
        da.SelectCommand.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["BookStatusConnection"].ConnectionString);
        da.SelectCommand.CommandText =  "select distinct IDDATA from "+fund+"..DATAEXT "+
                                        " where ( (MNFIELD = 899 and MSFIELD = '$p') or (MNFIELD = 899 and MSFIELD = '$a') or (MNFIELD = 899 and MSFIELD = '$w') ) and IDMAIN = " + id;
        DataSet ds = new DataSet();
        int i = da.Fill(ds);
        string[] result = new string[ds.Tables[0].Rows.Count];
        i = 0;
        foreach (DataRow row in ds.Tables[0].Rows)
        {
            result[i++] = row["IDDATA"].ToString();
        }
        return result;
    }


    [WebMethod(Description = "Информация о книге по id книги. Принимает id книги из VuFind. (например BJVVV_123456)")]
    public string GetBookInfoByID(string id)
    {
        string baseName = id.Substring(0, id.LastIndexOf("_")).ToUpper();
        int ID = int.Parse(id.Substring(id.LastIndexOf("_") + 1));

        BJBookLoader loader = new BJBookLoader(baseName);
        BookInfo bi = loader.GetBJBookByID(ID);
        return JsonConvert.SerializeObject(bi, Newtonsoft.Json.Formatting.Indented); ;
    }

    [WebMethod(Description = "Информация о книге по инвентарному номеру. Принимает строку с инвентарным номером (например 123456фр) и имя базы (например BJVVV или REDKOSTJ)")]
    public string GetBookInfoByInvNumber(string invNumber, string baseName)
    {
        BJBookLoader loader = new BJBookLoader(baseName);
        BookInfo bi = loader.GetBJBookByINV(invNumber);
        return JsonConvert.SerializeObject(bi, Newtonsoft.Json.Formatting.Indented); ;
    }


    



    //================================================================================================================================================
    //================================================================================================================================================
    //================================================================================================================================================
    //================================================================================================================================================
    //================================================================================================================================================
    //================================================================================================================================================
    //================================================================================================================================================

    [WebMethod(Description = "Меняет пароль читателю.")]
    public void ChangePWD(int number, string pwd)
    {
        //DateTime.UtcNow 

        if (1 == 1)
        {
            Exception ex = new Exception("Неверные данные");
            throw ex;
        }

    }



}
