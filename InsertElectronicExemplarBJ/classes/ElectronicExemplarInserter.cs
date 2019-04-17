using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json.Linq;
using InsertElectronicExemplarBJ.LIBFL_API;
using Newtonsoft.Json;
using LibflClassLibrary.Books.BJBooks;

namespace InsertElectronicExemplarBJ
{

    //Free - эл. свободный доступ
    //Indoor - эл. внутри библиотеки
    //Order - эл. через личный кабинет
    public enum ElectronicExemplarType { Free, Indoor, Order }
    
    class ElectronicExemplarInserter : IDisposable
    {
        private int _pin;
        private string _baseName;
        private string _fund;
        private const int BJVVV_IDDATA_FREE = 17183938;
        private const int BJVVV_IDDATA_INDOOR = 17183940;
        private const int BJVVV_IDDATA_ORDER = 17183941;
        private const int REDKOSTJ_IDDATA_FREE = 673572;
        private const int REDKOSTJ_IDDATA_INDOOR = 673573;
        private const int REDKOSTJ_IDDATA_ORDER = 673574;

        private int IDDATA_FREE;
        private int IDDATA_INDOOR;
        private int IDDATA_ORDER;
        
        public ElectronicExemplarInserter(int pin, string baseName, string fund)
        {
            this._pin = pin;
            this._baseName = baseName;
            this._fund = fund;
            switch (this._baseName)//вставлять пока что можем только в эти фонды
            {
                case "BJVVV":
                    IDDATA_FREE = BJVVV_IDDATA_FREE;
                    IDDATA_INDOOR = BJVVV_IDDATA_INDOOR;
                    IDDATA_ORDER = BJVVV_IDDATA_ORDER;
                    break;
                case "BJVVV_Test":
                    IDDATA_FREE = BJVVV_IDDATA_FREE;
                    IDDATA_INDOOR = BJVVV_IDDATA_INDOOR;
                    IDDATA_ORDER = BJVVV_IDDATA_ORDER;
                    break;
                case "REDKOSTJ":
                    IDDATA_FREE = REDKOSTJ_IDDATA_FREE;
                    IDDATA_INDOOR = REDKOSTJ_IDDATA_INDOOR;
                    IDDATA_ORDER = REDKOSTJ_IDDATA_ORDER;
                    break;
                default:
                    throw new Exception("В этот фонд " + this._baseName + " временно не добавляются инвентари электронных копий.");
            }
        }
        private SqlConnection connection;
        SqlTransaction transaction;
        public void InsertElectronicExemplar(ElectronicExemplarType type)
        {
            switch (this._baseName)//вставлять пока что можем только в эти фонды
            {
                case "BJVVV":
                case "BJVVV_Test":
                case "REDKOSTJ":
                    break;
                default:
                    throw new Exception("В этот фонд " + this._baseName + " временно не добавляются инвентари электронных копий.");
            }

            //string connectionString = "Data Source=127.0.0.1\\SQL2008R2;Initial Catalog=BJVVV;Persist Security Info=True;User ID=test;Password=test";
            string connectionString = "Data Source=192.168.4.25,1443;Initial Catalog=BJVVV_Test;Persist Security Info=True;User ID=sasha;Password=Corpse536;Connect Timeout=1200";
            this.connection = new SqlConnection(connectionString);
            connection.Open();
            this.transaction = connection.BeginTransaction();
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.Transaction = transaction;


            command.Parameters.Add("pin", System.Data.SqlDbType.Int).Value = this._pin;
            command.CommandText = "insert into " + this._baseName + "..DATA (IDMAIN,IDBLOCK, Creator) values (@pin, 260, 2);select scope_identity()";
            int IdData = Convert.ToInt32(command.ExecuteScalar());


            //=====================================================================================================================================
            //899$a

            command.CommandText = "select max(COLOR) from " + this._baseName + "..DATAEXT where IDMAIN = @pin";
            int Color = Convert.ToInt32(command.ExecuteScalar());
            command.Parameters.Add("iddata", SqlDbType.Int).Value = IdData;
            command.Parameters.Add("color", SqlDbType.Int).Value = ++Color;
            command.Parameters.Add("sort", SqlDbType.NVarChar).Value = "ХранЦИИТСерверабиблиотеки";
            command.CommandText = "insert into " + this._baseName + "..DATAEXT (IDMAIN, IDDATA, MNFIELD, MSFIELD, AFLINKID, IDINLIST, COLOR, SORT, PREFIXLEN, Created, Creator )" +
                                                                " values (      @pin, @iddata,    899,     '$a',    0,      79,     @color, @sort, 0,       getdate(),   2 );select scope_identity()";
            //вставляем в DATAEXT
            int IdDataExt = Convert.ToInt32(command.ExecuteScalar());

            command.Parameters.Add("iddataext", SqlDbType.Int).Value = IdDataExt;
            command.Parameters.Add("plain", SqlDbType.NVarChar).Value = "…Хран… ЦИИТ Сервера библиотеки";
            command.CommandText = "insert into " + this._baseName + "..DATAEXTPLAIN (IDMAIN, IDDATA, IDDATAEXT, PLAIN)" +
                                                                    "values (@pin, @iddata, @iddataext, @plain)";

            command.ExecuteNonQuery();//вставляем в DATAEXTPLAIN

            // у всех одно и то же метонахождение
            command.Parameters.Add("IDDATA_FREE", SqlDbType.Int).Value = IDDATA_FREE;
            command.Parameters.Add("IDDATA_INDOOR", SqlDbType.Int).Value = IDDATA_INDOOR;
            command.Parameters.Add("IDDATA_ORDER", SqlDbType.Int).Value = IDDATA_ORDER;
            command.CommandText = "insert into " + this._baseName + "..UNIWORDSEXT_RU " +
                                    "(IDWORDS, IDMAIN, IDDATA, IDDATAEXT, MNFIELD, MSFIELD)"+
                                    "select IDWORDS, @pin IDMAIN, @iddata IDDATA, @iddataext IDDATAEXT, MNFIELD, MSFIELD from " + this._baseName + "..UNIWORDSEXT_RU " +
                                    "where IDDATA = @IDDATA_FREE and MNFIELD = 899 and MSFIELD = '$a'";
            command.ExecuteNonQuery();


            //=====================================================================================================================================
            //899$b
            command.CommandText = "select max(COLOR) from " + this._baseName + "..DATAEXT where IDMAIN = @pin";
            Color = Convert.ToInt32(command.ExecuteScalar());
            command.Parameters["color"].Value = ++Color;
            command.Parameters["sort"].Value = "ФЭД";
            command.CommandText = "insert into " + this._baseName + "..DATAEXT (IDMAIN, IDDATA, MNFIELD, MSFIELD, AFLINKID, IDINLIST, COLOR, SORT, PREFIXLEN, Created, Creator )" +
                                                                " values (      @pin, @iddata,    899,     '$b',    0,       5,     @color, @sort, 0,       getdate(),   2 );select scope_identity()";
            //вставляем в DATAEXT
            IdDataExt = Convert.ToInt32(command.ExecuteScalar());

            command.Parameters["iddataext"].Value = IdDataExt;
            command.Parameters["plain"].Value = "ФЭД";
            command.CommandText = "insert into " + this._baseName + "..DATAEXTPLAIN (IDMAIN, IDDATA, IDDATAEXT, PLAIN)" +
                                                                    "values (@pin, @iddata, @iddataext, @plain)";

            command.ExecuteNonQuery();//вставляем в DATAEXTPLAIN

            // у всех одно и то же наименование фонда или коллекции
            command.CommandText = "insert into " + this._baseName + "..UNIWORDSEXT_RU " +
                                    "(IDWORDS, IDMAIN, IDDATA, IDDATAEXT, MNFIELD, MSFIELD)" +
                                    "select IDWORDS, @pin IDMAIN, @iddata IDDATA, @iddataext IDDATAEXT, MNFIELD, MSFIELD from " + this._baseName + "..UNIWORDSEXT_RU " +
                                    "where IDDATA = @IDDATA_FREE and MNFIELD = 899 and MSFIELD = '$b'";
            command.ExecuteNonQuery();

            //=====================================================================================================================================
            //921$a
            command.CommandText = "select max(COLOR) from " + this._baseName + "..DATAEXT where IDMAIN = @pin";
            Color = Convert.ToInt32(command.ExecuteScalar());
            command.Parameters["color"].Value = ++Color;
            command.Parameters["sort"].Value = "Электроннаякопия";
            command.CommandText = "insert into " + this._baseName + "..DATAEXT (IDMAIN, IDDATA, MNFIELD, MSFIELD, AFLINKID, IDINLIST, COLOR, SORT, PREFIXLEN, Created, Creator )" +
                                                                " values (      @pin, @iddata,    921,     '$a',    0,       13,     @color, @sort, 0,       getdate(),   2 );select scope_identity()";
            //вставляем в DATAEXT
            IdDataExt = Convert.ToInt32(command.ExecuteScalar());

            command.Parameters["iddataext"].Value = IdDataExt;
            command.Parameters["plain"].Value = "Электронная копия";
            command.CommandText = "insert into " + this._baseName + "..DATAEXTPLAIN (IDMAIN, IDDATA, IDDATAEXT, PLAIN)" +
                                                                    "values (@pin, @iddata, @iddataext, @plain)";

            command.ExecuteNonQuery();//вставляем в DATAEXTPLAIN

            // у всех один и тот же носитель
            command.CommandText = "insert into " + this._baseName + "..UNIWORDSEXT_RU " +
                                    "(IDWORDS, IDMAIN, IDDATA, IDDATAEXT, MNFIELD, MSFIELD)" +
                                    "select IDWORDS, @pin IDMAIN, @iddata IDDATA, @iddataext IDDATAEXT, MNFIELD, MSFIELD from " + this._baseName + "..UNIWORDSEXT_RU " +
                                    "where IDDATA = @IDDATA_FREE and MNFIELD = 921 and MSFIELD = '$a'";
            command.ExecuteNonQuery();

            //=====================================================================================================================================
            //921$c
            command.CommandText = "select max(COLOR) from " + this._baseName + "..DATAEXT where IDMAIN = @pin";
            Color = Convert.ToInt32(command.ExecuteScalar());
            command.Parameters["color"].Value = ++Color;
            command.Parameters["sort"].Value = "Длявыдачи";
            command.CommandText = "insert into " + this._baseName + "..DATAEXT (IDMAIN, IDDATA, MNFIELD, MSFIELD, AFLINKID, IDINLIST, COLOR, SORT, PREFIXLEN, Created, Creator )" +
                                                                " values (      @pin, @iddata,    921,     '$c',    0,       4,     @color, @sort, 0,       getdate(),   2 );select scope_identity()";
            //вставляем в DATAEXT
            IdDataExt = Convert.ToInt32(command.ExecuteScalar());

            command.Parameters["iddataext"].Value = IdDataExt;
            command.Parameters["plain"].Value = "Для выдачи";
            command.CommandText = "insert into " + this._baseName + "..DATAEXTPLAIN (IDMAIN, IDDATA, IDDATAEXT, PLAIN)" +
                                                                    "values (@pin, @iddata, @iddataext, @plain)";

            command.ExecuteNonQuery();//вставляем в DATAEXTPLAIN

            // у всех один и тот же класс издания
            command.CommandText = "insert into " + this._baseName + "..UNIWORDSEXT_RU " +
                                    "(IDWORDS, IDMAIN, IDDATA, IDDATAEXT, MNFIELD, MSFIELD) " +
                                    "select IDWORDS, @pin IDMAIN, @iddata IDDATA, @iddataext IDDATAEXT, MNFIELD, MSFIELD from " + this._baseName + "..UNIWORDSEXT_RU " +
                                    "where IDDATA = @IDDATA_FREE and MNFIELD = 921 and MSFIELD = '$c'";
            command.ExecuteNonQuery();

            //=====================================================================================================================================
            //922$a
            command.CommandText = "select max(COLOR) from " + this._baseName + "..DATAEXT where IDMAIN = @pin";
            Color = Convert.ToInt32(command.ExecuteScalar());
            command.Parameters["color"].Value = ++Color;
            command.Parameters["sort"].Value = "ОцифровкаВГБИЛ";
            command.CommandText = "insert into " + this._baseName + "..DATAEXT (IDMAIN, IDDATA, MNFIELD, MSFIELD, AFLINKID, IDINLIST, COLOR, SORT, PREFIXLEN, Created, Creator )" +
                                                                " values (      @pin, @iddata,    922,     '$a',    0,       0,     @color, @sort, 0,       getdate(),   2 );select scope_identity()";
            //вставляем в DATAEXT
            IdDataExt = Convert.ToInt32(command.ExecuteScalar());

            command.Parameters["iddataext"].Value = IdDataExt;
            command.Parameters["plain"].Value = "Оцифровка ВГБИЛ";
            command.CommandText = "insert into " + this._baseName + "..DATAEXTPLAIN (IDMAIN, IDDATA, IDDATAEXT, PLAIN)" +
                                                                    "values (@pin, @iddata, @iddataext, @plain)";

            command.ExecuteNonQuery();//вставляем в DATAEXTPLAIN

            // у всех один и тот же источник поступления
            command.CommandText = "insert into " + this._baseName + "..UNIWORDSEXT_RU " +
                                    "(IDWORDS, IDMAIN, IDDATA, IDDATAEXT, MNFIELD, MSFIELD)" +
                                    "select IDWORDS, @pin IDMAIN, @iddata IDDATA, @iddataext IDDATAEXT, MNFIELD, MSFIELD from " + this._baseName + "..UNIWORDSEXT_RU " +
                      "where IDDATA = @IDDATA_FREE and MNFIELD = 922 and MSFIELD = '$a'";
            command.ExecuteNonQuery();

            //=====================================================================================================================================
            //922$d - наименование валюты


            command.CommandText = "select max(COLOR) from " + this._baseName + "..DATAEXT where IDMAIN = @pin";
            Color = Convert.ToInt32(command.ExecuteScalar());
            command.Parameters["color"].Value = ++Color;
            command.Parameters["sort"].Value = "RUB";
            command.CommandText = "insert into " + this._baseName + "..DATAEXT (IDMAIN, IDDATA, MNFIELD, MSFIELD, AFLINKID, IDINLIST, COLOR, SORT, PREFIXLEN, Created, Creator )" +
                                                                " values (      @pin, @iddata,    922,     '$d',    0,       30,     @color, @sort, 0,       getdate(),   2 );select scope_identity()";
            //вставляем в DATAEXT
            IdDataExt = Convert.ToInt32(command.ExecuteScalar());

            command.Parameters["iddataext"].Value = IdDataExt;
            command.Parameters["plain"].Value = "RUB";
            command.CommandText = "insert into " + this._baseName + "..DATAEXTPLAIN (IDMAIN, IDDATA, IDDATAEXT, PLAIN)" +
                                                                    "values (@pin, @iddata, @iddataext, @plain)";

            command.ExecuteNonQuery();//вставляем в DATAEXTPLAIN

            // у всех одна и та же валюта
            command.CommandText = "insert into " + this._baseName + "..UNIWORDSEXT_AZ " +
                                    "(IDWORDS, IDMAIN, IDDATA, IDDATAEXT, MNFIELD, MSFIELD)" +
                                    "select IDWORDS, @pin IDMAIN, @iddata IDDATA, @iddataext IDDATAEXT, MNFIELD, MSFIELD from " + this._baseName + "..UNIWORDSEXT_AZ " +
                      "where IDDATA = @IDDATA_FREE and MNFIELD = 922 and MSFIELD = '$d'";
            command.ExecuteNonQuery();

            //=====================================================================================================================================
            //922$c - цена
            
            //узнаем цену. Для этого восопльзуемся функцией из API, которая вернёт всю информацию о заданном пине + сведения об файлах электронной копии
            //API возвращает информацию в виде JSON, поэтому нам нужно подключить ещё библиотеку для работы с этим форматом данных. Newtonsoft.JSON.dll
            //string bookId = this._baseName+"_"+this._pin;//приведём пин к такому виду, который воспринимает API

            ////создадим клиента API
            //LIBFL_API.ServiceSoapClient api = new ServiceSoapClient();
            
            //string book = api.GetBookInfoByID(bookId);//Вызовем функцию из API
            //JToken jsonBook = (JToken)JsonConvert.DeserializeObject(book);
            //JToken exemplars = jsonBook["Exemplars"];//получим все экземпляры
            //int PageCount = 0;
            //foreach (JToken token in exemplars)
            //{
            //    if (token["IsElectronicCopy"].ToString().ToLower() == "true")
            //    {
            //        PageCount = int.Parse(token["CountJPG"].ToString());
            //        break;
            //    }
            //}

            BJBookInfo b = BJBookInfo.GetBookInfoByPIN(_pin,_fund);
            int PageCount = b.DigitalCopy.CountJPG;


            const double PricePerPage = 8.27;
            double Cost = Math.Round(PricePerPage * PageCount, 2);
            command.CommandText = "select max(COLOR) from " + this._baseName + "..DATAEXT where IDMAIN = @pin";
            Color = Convert.ToInt32(command.ExecuteScalar());
            command.Parameters["color"].Value = ++Color;
            command.Parameters["sort"].Value = (int)(Cost * 100);
            command.CommandText = "insert into " + this._baseName + "..DATAEXT (IDMAIN, IDDATA, MNFIELD, MSFIELD, AFLINKID, IDINLIST, COLOR, SORT, PREFIXLEN, Created, Creator )" +
                                                                " values (      @pin, @iddata,    922,     '$c',    0,       0,     @color, @sort, 0,       getdate(),   2 );select scope_identity()";
            //вставляем в DATAEXT
            IdDataExt = Convert.ToInt32(command.ExecuteScalar());

            string CostStr = Cost.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
            command.Parameters["iddataext"].Value = IdDataExt;
            command.Parameters["plain"].Value = CostStr;
            command.CommandText = "insert into " + this._baseName + "..DATAEXTPLAIN (IDMAIN, IDDATA, IDDATAEXT, PLAIN)" +
                                                                    "values (@pin, @iddata, @iddataext, @plain)";

            command.ExecuteNonQuery();//вставляем в DATAEXTPLAIN

          

            //=====================================================================================================================================
            //921$d

            // у всех разный доступ для читателя
            string SORT = "";
            string PLAIN = "";
            int IDINLIST = 0;
            switch (type)//берем примеры из существующих
            {
                case ElectronicExemplarType.Free:
                    command.CommandText = "insert into " + this._baseName + "..UNIWORDSEXT_RU " +
                                            "(IDWORDS, IDMAIN, IDDATA, IDDATAEXT, MNFIELD, MSFIELD)" +
                                            "select IDWORDS, @pin IDMAIN, @iddata IDDATA, @iddataext IDDATAEXT, MNFIELD, MSFIELD from " + this._baseName + "..UNIWORDSEXT_RU " +
                                            "where IDDATA = @IDDATA_FREE and MNFIELD = 921 and MSFIELD = '$d'";
                    SORT = "Элсвободныйдоступ";
                    PLAIN = "Эл. свободный доступ";
                    IDINLIST = 5;
                    break;
                case ElectronicExemplarType.Indoor:
                    command.CommandText = "insert into " + this._baseName + "..UNIWORDSEXT_RU " +
                                            "(IDWORDS, IDMAIN, IDDATA, IDDATAEXT, MNFIELD, MSFIELD)" +
                                            "select IDWORDS, @pin IDMAIN, @iddata IDDATA, @iddataext IDDATAEXT, MNFIELD, MSFIELD from " + this._baseName + "..UNIWORDSEXT_RU " +
                                            "where IDDATA = @IDDATA_INDOOR and MNFIELD = 921 and MSFIELD = '$d'";
                    SORT = "Элтольковбиблиотеке";
                    PLAIN = "Эл. только в библиотеке";
                    IDINLIST = 6;
                    break;
                case ElectronicExemplarType.Order:
                    command.CommandText = "insert into " + this._baseName + "..UNIWORDSEXT_RU " +
                                            "(IDWORDS, IDMAIN, IDDATA, IDDATAEXT, MNFIELD, MSFIELD)" +
                                            "select IDWORDS, @pin IDMAIN, @iddata IDDATA, @iddataext IDDATAEXT, MNFIELD, MSFIELD from " + this._baseName + "..UNIWORDSEXT_RU " +
                                        "where IDDATA = @IDDATA_ORDER and MNFIELD = 921 and MSFIELD = '$d'";
                    SORT = "Элчерезличныйкабинет";
                    PLAIN = "Эл. через личный кабинет";
                    IDINLIST = 7;
                    break;
            }
            command.ExecuteNonQuery();



            command.CommandText = "select max(COLOR) from " + this._baseName + "..DATAEXT where IDMAIN = @pin";
            Color = Convert.ToInt32(command.ExecuteScalar());
            command.Parameters["color"].Value = ++Color;
            command.Parameters["sort"].Value = SORT;
            command.Parameters.Add("idinlist", SqlDbType.Int).Value = IDINLIST;
            command.CommandText = "insert into " + this._baseName + "..DATAEXT (IDMAIN, IDDATA, MNFIELD, MSFIELD, AFLINKID, IDINLIST, COLOR, SORT, PREFIXLEN, Created, Creator )" +
                                                                " values (      @pin, @iddata,    921,     '$d',    0,      @idinlist,     @color, @sort, 0,       getdate(),   2 );select scope_identity()";
            //вставляем в DATAEXT
            IdDataExt = Convert.ToInt32(command.ExecuteScalar());

            command.Parameters["iddataext"].Value = IdDataExt;
            command.Parameters["plain"].Value = PLAIN;
            command.CommandText = "insert into " + this._baseName + "..DATAEXTPLAIN (IDMAIN, IDDATA, IDDATAEXT, PLAIN)" +
                                                                    "values (@pin, @iddata, @iddataext, @plain)";
            command.ExecuteNonQuery();//вставляем в DATAEXTPLAIN

            //=====================================================================================================================================
            //899$p

            //узнаем какой инвентарь был последним
            //запросим все
            command.CommandText = "select A.SORT from " + this._baseName + "..DATAEXT A "+
                                  "left join " + this._baseName + "..DATAEXT B on A.IDDATA = B.IDDATA and B.MNFIELD = 921 and B.MSFIELD = '$a' " +
                                  "where A.MNFIELD = 899 and A.MSFIELD = '$p' and B.IDINLIST = 13 ";//носитель электронная копия
            DataTable invsText = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(command);
            int cnt = da.Fill(invsText);
            List<int> invNumbers = new List<int>();
            string NextInv = "";
            int NextInvINT32 = 0;
            if (cnt > 0)
            {
                foreach (DataRow row in invsText.Rows)
                {
                    string textNum = row["SORT"].ToString();
                    int num = 0;
                    try
                    {
                        num = int.Parse(textNum.Remove(textNum.Length - 1));
                    }
                    catch
                    {
                        continue;
                    }
                    invNumbers.Add(num);
                }
                int lastInv = invNumbers.Max();
                NextInvINT32 = ++lastInv;
                NextInv = NextInvINT32.ToString() + "э";
            }
            else // если нет электронных инвентарей, то делаем первый
            {
                //найдём минимальный первый для текущей базы
                command.CommandText = "select MIN from " + this._baseName + "..INV_GATES where ID = " +
                                    "(select GATE from " + this._baseName + "..LIST_9 where SHORTNAME = 'ФЭД')";
                int MinInGate = Convert.ToInt32(command.ExecuteScalar());
                NextInv = MinInGate.ToString() + "э";
                NextInvINT32 = MinInGate;

            }
            //проверим, не выходит ли за ворота полученное значение инвентаря

            command.CommandText = "select MAX from "+this._baseName+"..INV_GATES where ID = "+ 
                                  "(select GATE from "+this._baseName+"..LIST_9 where SHORTNAME = 'ФЭД')";
            int MaxInGate = Convert.ToInt32(command.ExecuteScalar());
            if (NextInvINT32 > MaxInGate)
            {
                throw new Exception("Инвентарный номер вышел за инвентарные ворота! база: "+this._baseName+", пин: "+this._pin);
            }



            //вставляем в DATAEXT
            command.CommandText = "select max(COLOR) from " + this._baseName + "..DATAEXT where IDMAIN = @pin";
            Color = Convert.ToInt32(command.ExecuteScalar());
            command.Parameters["color"].Value = ++Color;
            command.Parameters["sort"].Value = NextInv;
            command.CommandText = "insert into " + this._baseName + "..DATAEXT (IDMAIN, IDDATA, MNFIELD, MSFIELD, AFLINKID, IDINLIST, COLOR, SORT, PREFIXLEN, Created, Creator )" +
                                                                " values (      @pin, @iddata,    899,     '$p',    0,       0,     @color, @sort, 0,       getdate(),   2 );select scope_identity()";
            //вставляем в DATAEXT
            IdDataExt = Convert.ToInt32(command.ExecuteScalar());

            command.Parameters["iddataext"].Value = IdDataExt;
            command.Parameters["plain"].Value = NextInv;
            command.CommandText = "insert into " + this._baseName + "..DATAEXTPLAIN (IDMAIN, IDDATA, IDDATAEXT, PLAIN)" +
                                                                    "values (@pin, @iddata, @iddataext, @plain)";
            //вставляем в DATAEXTPLAIN
            command.ExecuteNonQuery();//вставляем в DATAEXTPLAIN


            command.Parameters.Add("NextInv", SqlDbType.NVarChar).Value = NextInv;
            command.CommandText = "insert into " + this._baseName + "..UNIWORDS_1 (VALUE) values (@NextInv) ; select scope_identity()";
            //вставляем в UNIWORDS_1
            int IdUniWords_1 = 0;
            try
            {
                //пробуем вставить
                IdUniWords_1 = Convert.ToInt32(command.ExecuteScalar());
            }
            catch
            {
                //если вставка не удалась, то значит уже такое значение есть в таблице. осталось узнать его ID
                //выяснилось, что BiblioJet не подчищает слова, на которые не осталось ссылок автоматически. наверное это есть в функциях сервисного обслуживания
                command.CommandText = "select ID from " + this._baseName + "..UNIWORDS_1 where VALUE = @plain";
                IdUniWords_1 = (int)command.ExecuteScalar();
            }

            command.Parameters.Add("IdUniWords_1", SqlDbType.Int).Value = IdUniWords_1;
            command.Parameters["iddataext"].Value = IdDataExt;

            command.CommandText = "insert into " + this._baseName + "..UNIWORDSEXT_1 (IDWORDS, IDMAIN, IDDATA, IDDATAEXT, MNFIELD, MSFIELD) values "+
                                                                                     " (@IdUniWords_1, @pin, @iddata, @iddataext, 899, '$p') ";
            command.ExecuteNonQuery();//вставляем в UNIWORDSEXT_1


            //узнаем ID в INV_ACT. если нет, то создадим
            command.CommandText = "select ID from "+this._baseName+"..INV_ACT where SOURCE = 5 and NUMBER ='01011901'";
            DataTable dt = new DataTable();
            cnt = da.Fill(dt);
            int IdINV_ACT;
            if (cnt == 0)
            {
                command.CommandText = "insert into " + this._baseName + "..INV_ACT (SOURCE, NUMBER,     Date,     Creator, STATUS, INVOICE, CONTROL_COUNT, YEAR)" +
                                                                        "values    ( 5 ,   '01011901', getdate(),    2    ,   0,     '',         1000000,  2018 );select scope_identity();";
                IdINV_ACT = Convert.ToInt32(command.ExecuteScalar());
            }
            else
            {
                IdINV_ACT = Convert.ToInt32(dt.Rows[0][0]);
            }

            //узнаем GATE для текущей базы
            command.CommandText = "select GATE from " + this._baseName + "..LIST_9 where SHORTNAME = 'ФЭД'";
            int Gate = (int)command.ExecuteScalar();
            


            //теперь вставим в INV_NO
            command.Parameters.Add("gate", SqlDbType.Int).Value = Gate;
            command.Parameters.Add("int32INV", SqlDbType.Int).Value = NextInvINT32;
            command.Parameters.Add("idACT", SqlDbType.Int).Value = IdINV_ACT;

            command.CommandText = "insert into " + this._baseName + "..INV_NO (IDMAIN,     IDDATA,     IDFOUND,    INVENTNO,    IDACT,    GATE,  Operator,    Date)"+
                                                                     " values  (@pin,       @iddata,     5,          @int32INV,  @idACT,    @gate,   2 ,       getdate())";
                                                                                                     //код фонла из LIST_9  
            command.ExecuteNonQuery();
            
            //=====================================================================================================================================
            //899$w
            //узнаем какой штрихкод был последним
            //так как штрихкоды сквозные, а не по фондам, то объединим запрос с редкой книгой
            command.CommandText = " select A.SORT from BJVVV..DATAEXT A " +
                                  " left join BJVVV..DATAEXT B on A.IDDATA = B.IDDATA and B.MNFIELD = 921 and B.MSFIELD = '$a' " +
                                  " where A.MNFIELD = 899 and A.MSFIELD = '$w' and B.IDINLIST = 13 " +  //носитель электронная копия
                                  " union all " +
                                  " select A.SORT from REDKOSTJ..DATAEXT A " +
                                  " left join REDKOSTJ..DATAEXT B on A.IDDATA = B.IDDATA and B.MNFIELD = 921 and B.MSFIELD = '$a' " +
                                  " where A.MNFIELD = 899 and A.MSFIELD = '$w' and B.IDINLIST = 13 " +  //носитель электронная копия;
                                    " union all " +
                                    " select A.SORT from BJVVV_Test..DATAEXT A " +
                                    " left join BJVVV_Test..DATAEXT B on A.IDDATA = B.IDDATA and B.MNFIELD = 921 and B.MSFIELD = '$a' " +
                                    " where A.MNFIELD = 899 and A.MSFIELD = '$w' and B.IDINLIST = 13 ";  //носитель электронная копия;
            DataTable BarText = new DataTable();
            da = new SqlDataAdapter(command);
            cnt = da.Fill(BarText);
            List<int> Bars = new List<int>();
            string NextBar = "";
            int NextBarINT32 = 0;
            if (cnt > 0)
            {
                foreach (DataRow row in BarText.Rows)
                {
                    string textNum = row["SORT"].ToString();
                    int num = int.Parse(textNum.Substring(1));
                    Bars.Add(num);
                }
                int lastBar = Bars.Max();
                NextBarINT32 = ++lastBar;
                NextBar = "E" + NextBarINT32.ToString();
            }
            else // если нет электронных инвентарей, то делаем первый
            {
                NextBar = "E100000001";
            }

            //вставляем в DATAEXT
            command.CommandText = "select max(COLOR) from " + this._baseName + "..DATAEXT where IDMAIN = @pin";
            Color = Convert.ToInt32(command.ExecuteScalar());
            command.Parameters["color"].Value = ++Color;
            command.Parameters["sort"].Value = NextBar;
            command.CommandText = "insert into " + this._baseName + "..DATAEXT (IDMAIN, IDDATA, MNFIELD, MSFIELD, AFLINKID, IDINLIST, COLOR, SORT, PREFIXLEN, Created, Creator )" +
                                                                " values (      @pin, @iddata,    899,     '$w',    0,       0,     @color, @sort, 0,       getdate(),   2 );select scope_identity()";
            //вставляем в DATAEXT
            IdDataExt = Convert.ToInt32(command.ExecuteScalar());

            command.Parameters["iddataext"].Value = IdDataExt;
            command.Parameters["plain"].Value = NextBar;
            command.CommandText = "insert into " + this._baseName + "..DATAEXTPLAIN (IDMAIN, IDDATA, IDDATAEXT, PLAIN)" +
                                                                    "values (@pin, @iddata, @iddataext, @plain)";
            //вставляем в DATAEXTPLAIN
            command.ExecuteNonQuery();//вставляем в DATAEXTPLAIN

            command.Parameters["iddataext"].Value = IdDataExt;
            command.CommandText = "insert into " + this._baseName + "..BARCODE_UNITS (IDMAIN, IDDATA, IDDATAEXT, BARCODE, Creator, DateCreate )" +
                                                                " values (      @pin,        @iddata, @iddataext, @plain,    2,  getdate() )";

            command.ExecuteNonQuery();//вставляем в BARCODE_UNITS

            //на этом всё
            //transaction.Rollback();
            transaction.Commit();


            transaction.Dispose();
            connection.Dispose();
            command.Dispose();
            da.Dispose();

        }

        #region Члены IDisposable

        public void Dispose()
        {
            connection.Dispose();
            transaction.Dispose();
        }

        #endregion
    }
}
