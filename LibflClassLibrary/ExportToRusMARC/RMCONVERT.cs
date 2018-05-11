using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.IO;

/// <summary>
/// Сводное описание для RMCONVERT
/// </summary>

public class RMCONVERT
{
    String str, P001, P102, P200z, R, Inst, rs, IDSession = "RUSMforLIBNET";
    DataSet ds;
    SqlConnection conbase03,conbase01;
    SqlDataAdapter da;
    SqlCommand command;

    Int32 lzap = 0;
    Int32 ibase_adr;
    Int32 nomzap = 0;
    String marker, GP, pol001 = "", T = "False";
    String dt1, dt2;
    String FNin, FS, Date_Base, blok80 = "";
    public string BAZA;
    string FNout;
    Int32 l, DIDDATA;
    Int16 IDLEVEL;
    int posbrr;
    String PREFIX001;
    Byte[] mrc;

    public RMCONVERT(string baza)
    {
        this.BAZA = baza;
        //conbase03 = new SqlConnection(XmlConnections.GetConnection("/Connections/base03"));
        //conbase01 = new SqlConnection(XmlConnections.GetConnection("/Connections/base01"));
        conbase03 = new SqlConnection("Data Source=192.168.4.25,1443;Initial Catalog=EXPORTNEB;Persist Security Info=True;User ID=OAIUSER;Password=User_OAI;Connection Timeout = 1200");
        conbase01 = new SqlConnection("Data Source=192.168.4.7;Initial Catalog=TECHNOLOG_VVV;Persist Security Info=True;User ID=OAIUSER;Password=User_OAI;Connection Timeout = 1200");
        da = new SqlDataAdapter();
    }
    void Main(string[] args)
    {
        try
        {
            conbase03 = new SqlConnection("Data Source=192.168.4.25,1443;Initial Catalog=EXPORTNEB;Persist Security Info=True;User ID=OAIUSER;Password=User_OAI;Connection Timeout = 1200");
            conbase01 = new SqlConnection("Data Source=192.168.4.7;Initial Catalog=TECHNOLOG_VVV;Persist Security Info=True;User ID=OAIUSER;Password=User_OAI;Connection Timeout = 1200");
        }
        catch (Exception ex)
        {
            Console.Write(ex.Message + "Программа аварийно завершает работу.");
            throw;
        }
        da = new SqlDataAdapter();
        byte[] bt;
        bt = new byte[9999];

        FS = System.DateTime.Now.ToString("ddMMyyyyHHmmss");
        FNout = "VGBIL" + FS + ".iso";
        Console.Write("Начато формирование файла " + FNout);

        BAZA = "BJVVV";
        PREFIX001 = "V";

        //R = " SELECT TOP 1 CONVERT(VARCHAR(50),restore_date,121) FROM msdb..restorehistory WHERE destination_database_name = N'BJVVV'"
        //+ " ORDER BY restore_date DESC ";
        //da.SelectCommand = new SqlCommand();
        //da.SelectCommand.CommandText = R;
        //da.SelectCommand.Connection = con;
        //da.SelectCommand.CommandTimeout = 1200;
        //DataSet dsD = new DataSet();
        //da.Fill(dsD);
        //Date_Base = dsD.Tables[0].Rows[0][0].ToString();
        //dsD.Dispose();


        ////SELECT [ID],[p001],[DateBase],[notload],[del],[IDtrans],[ImyaF],[ImyaFNEB],[Timestamp]
        ////  FROM [BJRUSMARC].[dbo].[LIBNET_NEB] // Дата последней выгруженной базы
        //R = " SELECT DISTINCT TOP 1 CONVERT(VARCHAR(50),DateBase,121) DATBASE FROM [BJRUSMARC].[dbo].[LIBNETNEB]"
        //    + " WHERE BaZa = '" + PREFIX001 + "' ORDER BY DATBASE DESC ";
        //da.SelectCommand = new SqlCommand();
        //da.SelectCommand.CommandText = R;
        //da.SelectCommand.Connection = con;
        //da.SelectCommand.CommandTimeout = 1200;
        //dsD = new DataSet();
        //da.Fill(dsD);
        //dt1 = dsD.Tables[0].Rows[0][0].ToString();
        //dsD.Dispose();

        //FormFileN();//новые

        //FormFileC();//изменённые

        //FormFileD();//удалённые

        //BAZA = "REDKOSTJ";
        //PREFIX001 = "R";
        // R = " SELECT TOP 1 restore_date FROM msdb..restorehistory WHERE destination_database_name = N'REDKOSTJ'
        //+ " ORDER BY restore_date DESC ";
        // da.SelectCommand = new SqlCommand();
        // da.SelectCommand.CommandText = R;
        // da.SelectCommand.Connection = con;
        // da.SelectCommand.CommandTimeout = 1200;
        // dsD = new DataSet();
        // da.Fill(dsD);
        // Date_Base = dsD.Tables[0].Rows[0][0].ToString();

        // FormFileN();

        // //FormFileC();

        // //FormFileD();

        //Console.Write("Работа завершена");
        return;
    } // start_convert
    // -----------------------------------------------
    //-- Description:	Конвертирование BJVVV в RUSMARC
    //-- =============================================
    public void PBJ2RUSM(String IDSession, Int32 IDM, String STATUS)
    {
        Int32 DMNF, KK, KK2, VID_OBR;
        String SIDEXT, DMSF, DPPLAIN, IND1, IND2, IDENT;
        Int16 MET;
        Int32 DIDDATA;

        //da = new SqlDataAdapter();
        //da.SelectCommand = new SqlCommand();
        //T = "False";
        //if (PREFIX001.Equals("V"))
        //{
        //    da.SelectCommand.CommandText = " SELECT ForAllReader FROM BookAddInf..ScanInfo WHERE "
        //        + " IDBook=" + IDM.ToString() + " and IDBase=1"; //BJVVV
        //}
        //else
        //{
        //    da.SelectCommand.CommandText = " SELECT ForAllReader FROM BookAddInf..ScanInfo WHERE "
        //        + " IDBook=" + IDM.ToString() + " and IDBase=2"; //REDKOSTJ
        //}
        //da.SelectCommand.CommandTimeout = 1200;
        //da.SelectCommand.Connection = con;
        //ds = new DataSet();
        //Int32 K = da.Fill(ds);
        //if (K > 0)
        //{
        //    T = ds.Tables[0].Rows[0][0].ToString(); // ForAllReader
        //}
        //ds.Dispose();

        //da = new SqlDataAdapter();
        //da.SelectCommand = new SqlCommand();
        //da.SelectCommand.CommandText = " SELECT notload FROM BJRUSMARC..LIBNET_NEB WHERE "
        //        + " p001='" + PREFIX001+IDM.ToString() + "' and ImyaF='" + d3 + "' AND LEN(notload)>0";
        //da.SelectCommand.CommandTimeout = 1200;
        //da.SelectCommand.Connection = con;
        //ds = new DataSet();
        //K = da.Fill(ds);
        //if (K > 0)
        //{
        //    ds.Dispose();
        //    goto M0; // Запись не прошла контроль LIBNET и не загружена в LIBNET
        //}
        //ds.Dispose();

        P102 = "";
        P200z = ""; // Языки параллельных заглавий
        //-- Формирование обязательных полей
        INITRUSM(IDSession, IDM, STATUS);
        //MessageBox.Show(IDM.ToString());
        R = " SELECT ID,IDDATA,MNFIELD,MSFIELD,SORT "
        + " FROM " + BAZA + "..DATAEXT WHERE IDMAIN=" + IDM.ToString()
        + " ORDER BY IDDATA,MNFIELD,MSFIELD ";
        da.SelectCommand = new SqlCommand();
        da.SelectCommand.CommandText = R;
        da.SelectCommand.Connection = conbase03;
        da.SelectCommand.CommandTimeout = 1200;
        DataSet dsS = new DataSet();
        KK = da.Fill(dsS, "listS");
        if (KK > 0)
        {
            for (int countS = 0; countS < KK; countS++)
            {
                SIDEXT = dsS.Tables["listS"].Rows[countS][0].ToString();
                DIDDATA = Int32.Parse(dsS.Tables["listS"].Rows[countS]["IDDATA"].ToString());
                DMNF = Int32.Parse(dsS.Tables["listS"].Rows[countS]["MNFIELD"].ToString());
                DMSF = dsS.Tables["listS"].Rows[countS]["MSFIELD"].ToString();
                DPPLAIN = dsS.Tables["listS"].Rows[countS]["SORT"].ToString();
                if (DMNF != 606)
                {
                    R = " SELECT PLAIN  FROM " + BAZA + "..DATAEXTPLAIN "
                        + " WHERE IDDATAEXT=" + SIDEXT;
                    da.SelectCommand = new SqlCommand();
                    da.SelectCommand.CommandText = R;
                    da.SelectCommand.Connection = conbase03;
                    da.SelectCommand.CommandTimeout = 1200;
                    DataSet dsP = new DataSet();
                    Int32 KKP = da.Fill(dsP, "listSP");
                    if (KKP > 0) DPPLAIN = dsP.Tables["listSP"].Rows[0][0].ToString();
                    DPPLAIN = DPPLAIN.Replace("'", "~").Trim();
                    DPPLAIN = DPPLAIN.Replace("(char)13", "").Replace("(char)10", "");
                    if (DPPLAIN.Length > 3400) DPPLAIN = DPPLAIN.Substring(1, 3400) + "...";
                    dsP.Dispose();
                }
                VID_OBR = -1;
                R = " SELECT  VID_OBR, MET1, IND1, IND2, IDEN1 "
                   + " FROM [BJRUSMARC]..VVV2LIBNET "
                   + " WHERE MNF= " + DMNF + " AND MSF= '" + DMSF + "'";
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.CommandText = R;
                da.SelectCommand.Connection = conbase03;
                da.SelectCommand.CommandTimeout = 1200;
                ds = new DataSet();
                KK2 = da.Fill(ds, "list0");
                if (KK2 == 0) // Неопознанное поле
                {
                    ds.Dispose();
                    if (DMNF == 482) goto M5; // Приплетено к
                    R = " SELECT  [NAME] FROM " + BAZA + "..FIELDS "
                        + " WHERE MNFIELD= " + DMNF.ToString() + " AND MSFIELD= '" + DMSF + "'";
                    da.SelectCommand = new SqlCommand();
                    da.SelectCommand.CommandText = R;
                    da.SelectCommand.Connection = conbase03;
                    da.SelectCommand.CommandTimeout = 1200;
                    ds = new DataSet();
                    da.Fill(ds, "list1");
                    Inst = ds.Tables["list1"].Rows[0][0].ToString();
                    ds.Dispose();
                    // 830a -- Общее примечание, составленное каталогизатором
                    //R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
                    //   + " VALUES ('" + IDSession + "'," + IDM.ToString() + "," + DIDDATA.ToString() + ",830,' ',' ','a','"
                    //   + Inst + "'+': '+ N'" + DPPLAIN + "')";
                    //command = new SqlCommand(R, con);
                    //con.Open();
                    //command.CommandTimeout = 1200;
                    //command.ExecuteNonQuery();
                    //con.Close();
                    goto M5;
                } // Неопознанное поле

                VID_OBR = Int16.Parse(ds.Tables["list0"].Rows[0][0].ToString());
                MET = Int16.Parse(ds.Tables["list0"].Rows[0][1].ToString());
                IND1 = ds.Tables["list0"].Rows[0][2].ToString();
                IND2 = ds.Tables["list0"].Rows[0][3].ToString();
                IDENT = ds.Tables["list0"].Rows[0][4].ToString();
                ds.Dispose();

                if (VID_OBR == 0) // Копирование в выходную запись
                {
                    //10	$b	Уточнения	0	010	b
                    //10	$z	Ошибочный	0	010	z
                    //11	$9	Тираж	0	010	9
                    //11	$a	Номер ISSN	0	011	a
                    //11	$y	Отмененный	1	011	y
                    //200	$a	Основное заглавие документа	0	200	a
                    //200	$b	Основное заглавие произведения того же автора	1	200	a
                    //200	$e	Сведения, относящиеся к заглавию	1	200	e
                    //200	$f	Сведения об ответственности	0	200	f
                    //200	$h	Номер части	1	200	h
                    //200	$i	Наименование части	1	200	i
                    //205	$a	Сведения об издании	0	205	a
                    //205	$b	Дополнительные сведения об издании	1	205	b
                    //205	$f	Первые сведения об ответственности, относящиеся к изданию	1	205	f
                    //205	$g	Последующие сведения об ответственности, относящиеся к изданию	1	205	g
                    //215	$a	Объём	1	215	a
                    //215	$c	Размер	1	215	d
                    //215	$d	Сопроводительный материал	1	215	e
                    //300	$a	Общие примечания	0	300	a
                    //320	$a	Примечание о библиографии и указателе	0	320	a
                    //327	$a	Примечание о содержании	1	327	a
                    //330	$a	Текст аннотации	0	330	a
                    //500	$3	АФ	0	0  	NULL
                    //500	$a	Унифицированное заглавие	0	500	a
                    //501	$3	АФ	0	0  	NULL
                    //501	$a	Унифицированное общее заглавие	0	501	a
                    //503	$3	АФ	0	0  	NULL
                    //503	$a	Унифицированный заголовок	0	503	a
                    //830	$a	Примечание каталогизатора	0	830	a
                    //921	$a	Носитель	1	200	b
                    //922	$b	Трофей	0	300	a
                    //2100	$d	Дата издания	0	210	d
                    //2111	$e	Географическое название	1	620	d
                    //MessageBox.Show(MET.ToString());
                    R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
                     + " VALUES ('" + IDSession + "'," + IDM.ToString() + "," + DIDDATA.ToString() + "," + MET.ToString()
                     + ",'" + IND1 + "','" + IND2 + "','" + IDENT + "',N'" + DPPLAIN + "')";
                    command = new SqlCommand(R, conbase01);
                    conbase01.Open();
                    command.CommandTimeout = 1200;
                    command.ExecuteNonQuery();
                    conbase01.Close();
                    goto M5;
                } // VID_OBR = 0 Копирование в выходную запись

                if (VID_OBR == 1) // Вид издания
                {
                    OBR1(IDSession, IDM, DIDDATA, MET, IND1, IND2, IDENT, DPPLAIN);
                    goto M5;
                }   // VID_OBR == 1) // Вид издания

                if (VID_OBR == 3) // Иллюстрации
                {
                    OBR3(IDSession, IDM, DIDDATA, MET, IND1, IND2, IDENT, DPPLAIN);
                    goto M5;
                }  // Иллюстрации

                if (VID_OBR == 6) // 700,701,702
                {
                    OBR6(IDSession, IDM, DIDDATA, MET, IND1, IND2, IDENT, DPPLAIN);
                    goto M5;
                } // VID_OBR == 6  -- 700,701,702

                if (VID_OBR == 9) // Сводный уровень - Заглавие, номер в серии
                {
                    OBR9(IDSession, IDM, DIDDATA, MET, IND1, IND2, IDENT, DPPLAIN);
                    goto M5;
                }  // Сводный уровень - Заглавие, номер в серии

                if (VID_OBR == 10) // ISBNa
                {
                    OBR10a(IDSession, IDM, DIDDATA, MET, IND1, IND2, IDENT, DPPLAIN);
                    goto M5;
                }  // Сводный уровень - Заглавие, номер в серии

                if (VID_OBR == 101) // Язык публикации
                {
                    OBR101(IDSession, IDM, DIDDATA, MET, IND1, IND2, IDENT, DPPLAIN);
                    goto M5;
                }  // VID_OBR == 101 --- Язык публикации

                if (VID_OBR == 102) // Место издания
                {
                    OBR102(IDSession, IDM, DIDDATA, MET, IND1, IND2, IDENT, DPPLAIN);
                    goto M5;
                }  // VID_OBR == 102  ----- Место издания

                if (VID_OBR == 200) // Сведения об ответственности
                {
                    OBR200(IDSession, IDM, DIDDATA, MET, IND1, IND2, IDENT, DPPLAIN);
                    goto M5;
                }  // VID_OBR == 200  ----- Сведения об ответственности

                if (VID_OBR == 299) // Параллельное основное заглавие 200d + 200z
                {
                    OBR200dz(IDSession, IDM, DIDDATA, MET, IND1, IND2, IDENT, DPPLAIN);
                    goto M5;
                }  // VID_OBR == 200   Параллельное основное заглавие 200d + 200z

                if (VID_OBR == 298) // Язык Параллельного основного заглавия 200z
                {
                    OBR200z(IDSession, IDM, DIDDATA, MET, IND1, IND2, IDENT, DPPLAIN);
                    goto M5;
                }  // VID_OBR == 200  Язык Параллельного основного заглавия 200z

                if (VID_OBR == 326) //326	$a	Примечание о периодичности	0	326	a
                {
                    OBR326(IDSession, IDM, DPPLAIN);
                    goto M5;
                }  // VID_OBR == 200  ----- Сведения об ответственности

                if (VID_OBR == 400) // Связи
                {
                    OBR400(IDSession, IDM, DIDDATA, MET, IND1, IND2, IDENT, DPPLAIN);
                    goto M5;
                }  // VID_OBR == 400   Связи

                if (VID_OBR == 606) // ПР
                {
                    OBR606(IDSession, IDM, DIDDATA, MET, IND1, IND2, IDENT, DPPLAIN);
                    goto M5;
                }  // VID_OBR == 606 ----- ПР

                if (VID_OBR == 517) // Другое заглавие
                {
                    OBR517(IDSession, IDM, DIDDATA, MET, IND1, IND2, IDENT, DPPLAIN);
                    goto M5;
                } // VID_OBR == 517 --- Другое заглавие
                if (VID_OBR == 710) // Наименование организации  - 710,711,712
                {
                    OBR710(IDSession, IDM, DIDDATA, MET, IND1, IND2, IDENT, DPPLAIN);
                    goto M5;
                }  // VID_OBR == 710   --- Наименование организации  - 710,711,712


                //if (VID_OBR == 852) // Наименование фонда
            //{
            //    OBR852(IDSession,IDM, DIDDATA, MET, IND1, IND2, IDENT, DPPLAIN);
            //    goto M5;
            //} // VID_OBR == 852 --- Наименование фонда

            M5: ;
            } // FOR 
        } // KK>0 - Есть поля
    }
    //
    //-- =============================================
    //-- Description:	Инициализация РУСМАРК - записи
    //-- =============================================
    public void INITRUSM(String IDSession, Int32 IDM2, String MARKER_5)
    // 2011.04.11
    {
        Int32 IDM;
        String P005;
        Int16 IDLEVEL;
        IDM = IDM2;
        INITMARKER(IDSession, IDM, MARKER_5, out IDLEVEL);
        //-- Идентификатор записи 
        //P001 = PREFIX001 + IDM.ToString().PadLeft(8, '0');
        P001 = PREFIX001 + IDM.ToString();

        R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
         + " VALUES ('" + IDSession + "'," + IDM.ToString() + ",0,001,0,0,0,'" + P001 + "')";
        command = new SqlCommand(R, conbase01);
        conbase01.Open();
        command.CommandTimeout = 1200;
        command.ExecuteNonQuery();
        conbase01.Close();
        //-- Идентификатор версии/
        //            Console.WriteLine(date1.ToString("s"));
        //            Displays 2008-04-10T06:30:00                       
        P005 = DateTime.Now.ToString("s");
        P005 = P005.Replace("-", "");
        P005 = P005.Replace("T", "");
        P005 = P005.Replace(":", "");
        P005 = P005 += ".0";
        R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
         + " VALUES ('" + IDSession + "'," + IDM.ToString() + ",0,005,0,0,0,N'" + P005 + "')";
        command = new SqlCommand(R, conbase01);
        conbase01.Open();
        command.CommandTimeout = 1200;
        command.ExecuteNonQuery();
        conbase01.Close();
        //-- Инициализация поля 100
        //-- 100  ДАННЫЕ ОБЩЕЙ ОБРАБОТКИ
        //--   Поле содержит кодированные данные фиксированной длины, применимые к записям о документах, представленных на любых носителях.
        //--   Обязательное.  Не повторяется.
        //--   Индикатор 1: # (не определен)
        //--   Индикатор 2: # (не определен)
        INITP100(IDSession, IDM, IDLEVEL);
        //1: Монография
        //-100: Коллекция
        //-2: Сводный уровень многотомного издания
        //2: Том (выпуск) многотомного издания
        //-3: Сводный уровень сериального издания
        //-33: Сводный уровень подсерии, входящей в серию
        //3: Выпуск сериального издания
        //-4: Сводный уровень сборника
        //4: Часть сборника
        //-5: Сводный уровень продолжающегося издания
        //5: Выпуск продолжающегося издания
        if (IDLEVEL > 0 || IDLEVEL == -100 || IDLEVEL == -4 || IDLEVEL == -2)
        {
            //---- Инициализация поля 105
            //---- 105  ПОЛЕ КОДИРОВАННЫХ ДАННЫХ: ТЕКСТОВЫЕ МАТЕРИАЛЫ, МОНОГРАФИЧЕСКИЕ 
            //----		Поле содержит кодированные данные, относящиеся к монографическим текстовым изданиям.
            //----		Примечание: для описания монографических серий используется поле 110. 
            //----	Обязательное для печатных монографических текстовых документов (например, для книг).
            //----	Не повторяется. 
            //----	Индикатор 1: # (не определен)
            //----	Индикатор 2: # (не определен) 
            INITP105(IDSession, IDM);
            //-- end -- IDLEVEL  - Монография
        }
        else //-- Сводный уровень Серия, Подсерия,Продолжающиеся издания
        {
            //---- Инициализация поля 110
            //--  --EXECUTE INITP110 @IDM, @IDLEVEL - Удалено по требованию Селицкой 20091030
            INITP110(IDSession, IDM, IDLEVEL);
            //---- 110 ПОЛЕ КОДИРОВАННЫХ ДАННЫХ: ПРОДОЛЖАЮЩИЕСЯ РЕСУРСЫ 
            //----		Поле содержит кодированные данные, относящиеся к продолжающимся ресурсам, включая монографические серии, каталогизируемые как сериальные документы, а не как самостоятельные монографии. 
            //----		Обязательное в записи высшего уровня, составляемой для описания продолжающегося ресурса в целом.
            //----		Не повторяется. 
            //----		Примечание: Указание на содержание отдельных выпусков продолжающегося ресурса может (но не является обязательным) размещаться в позициях 110$a/4-6 записи уровня физической единицы, описывающей отдельный выпуск. Допускается также использование в записи уровня физической единицы позиции 110$a/7, если отдельный/отдельные выпуски содержат материалы конференции, однако, отражение этих материалов - не главная функция продолжающегося ресурса в целом (позиция $a/7 записи высшего уровня содержит 0). 
            //----	Индикатор 1: # (не определен)
            //----	Индикатор 2: # (не определен) 
            //-- END -- IDLEVEL <0  -Сериальное издание или сериальное издание
        }
        //-- Формирование обязательных 801-ых полей
        //-- 801  ИСТОЧНИК ЗАПИСИ		
        //--		Поле содержит указание на источники записи, к котор	ым от	носятся:
        //--			организация, создающая данные; организация, преобразующая данные в
        //--			машиночитаемую форму; организация, модифицирующая первоначальные записи /
        //--			данные; организация, распространяющая записи.		
        //--		Обязательное.		
        //--		Повторяется для каждой из перечисленных функций, вы	полня	емых той или иной
        //--			организацией.		
        //--		Индикатор 1: # (не определен)		
        //--		Индикатор 2: Индикатор функции		
        //--			Этот индикатор определяет функцию, выполняемую ор	ганиз	ацией, название
        //--			которой помещено в подполе $b.		
        //--			0 - Агентство, производящее первоначальную катало	гизац	ию
        //--				Организация, подготовившая данные для записи.		
        //--			1 - Агентство, преобразующее данные		
        //--				Организация, конвертировавшая данные в машиночита	емую	форму
        //--			2 - Агентство, вносящее изменения в запись		
        //--				Организация, модифицировавшая содержание записи,	либо	ее структуру.
        //--			3 - Агентство, распространяющее запись		
        //--				Организация, распространявшая запись		
        //--
        //--		В машиночитаемой записи должно быть не менее двух вхождений поля 801, т.к.
        //--			для того чтобы данные привести в машиночитаемую форму, их надо
        //--			подготовить. Т.о. у первого вхождения поля 801 индикатор	= 0, а у второго = 1.		
        P102 = "";  // Коды страны и места издания
        blok80 = "";
        return;
    }
    //
    //-- =============================================
    //-- Description:	Инициализвция МАРКЕРа
    //-- =============================================
    public void INITMARKER(String IDSession, Int32 IDM, String POZ5, out Int16 IDLEVEL)
    {
        Int32 KK, KK2;
        String C, MARKER;
        //-- МАРКЕР записи
        //--0-4 Длина записи
        //-- Пять десятичных цифр, при необходимости выравниваемых вправо начальными
        //--  нулями, указывают количество символов в записи, включая маркер записи,
        //--  справочник и переменные поля. Вычисляется автоматически, когда запись
        //--  окончательно сформирована для обмена.
        //--5 Статус записи - poz5
        //--  n = новая запись 
        //--  c = изменённая запись 
        //--  d = удалённая запись 
        //--6 Тип записи
        //--  а = текстовые материалы, кроме рукописных. В том числе печатные
        //--    текстовые материалы, микроформы печатных текстовых материалов, а
        //--    также электронные текстовые материалы. 
        //--  b = текстовые материалы, рукописные. В том числе микроформы рукописных
        //--    текстовых материалов и электронные рукописные текстовые материалы. 
        //--  с = музыкальные партитуры, кроме рукописных
        //--  d = музыкальные партитуры, рукописные
        //--  е = картографические материалы, кроме рукописных
        //--  f = картографические материалы, рукописные
        //--  g = проекционные и видеоматериалы (кинофильмы, диафильмы, слайды,
        //--    пленочные материалы, видеозаписи). Включает цифровые видеоматериалы
        //--   (не используется для не-проекционной двухмерной графики: см. ниже
        //--    код "k"). 
        //--  i = звукозаписи, немузыкальные 
        //--  j = звукозаписи, музыкальные 
        //--  k = двухмерная графика (иллюстрации, чертежи и т. п.)
        //--  l = электронный ресурс
        //--  m = информация на нескольких носителях (например, книга с приложением
        //--    программ на дискете, CD и т. п.)
        //--  r = трехмерные искусственные и естественные объекты
        //--7 Библиографический уровень (позиция символа 7) 
        //--  а = аналитический - документ, является частью физической единицы
        //--    (составная часть)
        //--  i = интегрируемый ресурс - ресурс, изменяющийся посредством обновлений (изъятия, вставки или замещения отдельных его частей), которые не публикуются отдельно, а интегрируются в новое единое целое
        //--  m = монографический - документ, представляет собой физически единое
        //--    целое или издается в заранее определенном количестве частей
        //--    Например: отдельная книга, многотомное издание в целом, том
        //--     многотомного издания, выпуск сериального издания. 
        //--  s = сериальный - продолжающийся ресурс, выпускаемый последовательными
        //--     частями (как правило, нумерованными и (или) датированными выпусками)
        //--     и рассчитанный на издание в течение времени, продолжительность
        //--     которого заранее не установлена
        //--  с = подборка - библиографическая единица, скомплектованная из отдельных
        //--    физических единиц
        //--8 Код иерархического уровня (позиция символа 8) 
        //--   Код определяет иерархическую связь записи с другими записями в том же
        //--    файле и показывает ее относительное положение в иерархии. 
        //--  # = иерархическая связь не определена
        //--  0 = иерархическая связь отсутствует
        //--  1 = запись высшего уровня
        //--  2 = запись ниже высшего уровня (любая запись ниже высшего уровня) 
        //--9 Не определено. Содержит символ пробела: #. 
        //--10 Длина индикатора. Содержит цифру "2". 
        //--11 Длина идентификатора подполя. Содержит цифру "2". 
        //--12-16 Базовый адрес данных (позиции символов 12-16) 
        //--  Пять десятичных цифр, выровненных вправо начальными нулями, указывающие
        //--   на начальную символьную позицию первого поля данных относительно
        //--   начала записи. Это число будет равно общему количеству символов в
        //--   маркере и справочнике, включая разделитель подполя в конце справочника.
        //--   В справочнике начальная позиция символов для каждого поля задается
        //--   относительно первого символа первого поля данных, которое является
        //--   полем 001, а не от начала записи. Базовый адрес, таким образом,
        //--   является основой, с помощью которой рассчитывается позиция каждого поля.
        //--17 Уровень кодирования
        //--  # = полный уровень. Корректно составленная запись о полностью
        //--    каталогизированном документе, подготовленная для использования в
        //--    Электронном каталоге или для обмена.
        //--  1 = подуровень 1. Запись, составленная на основе каталожной карточки
        //--   (ретроконверсия) или импортированная из другого формата, не
        //--   предоставляющего достаточно данных для корректного заполнения всех
        //--   обязательных (в т. ч. условно обязательных) элементов формата, и
        //--   неоткорректированная по документу. 
        //--18 Форма каталогизационного описания
        //--  # = запись составлена по правилам ISBD.
        //--  i = запись составлена не полностью по правилам ISBD (отдельные поля
        //--    соответствуют положениям ISBD).
        //--19 Не определено. Содержит символ пробела: #. 
        //--20 Длина элемента "длина поля данных". Содержит символ: 4
        //--21 Длина элемента "позиция начального символа". Содержит символ: 5
        //--22 Длина элемента "часть, определяемая при применении". Содержит символ: 0
        //--23 Не определено. Содержит символ пробела: #. 
        //-- Элементы данных, входящие в МАРКЕР ЗАПИСИ, больше нигде в формате не
        //--  используются. Хотя некоторые из значений кодов применения (например,
        //--  "тип записи" и "библиографический уровень") могут частично совпадать
        //--  с другими кодированными данными, в действительности, коды в МАРКЕРЕ
        //--  ЗАПИСИ относятся к характеристикам записей, а не к характеристикам
        //--  самой библиографической единицы. 
        R = " SELECT IDLEVEL FROM " + BAZA + "..MAIN WHERE ID= " + IDM.ToString();
        da.SelectCommand = new SqlCommand();
        da.SelectCommand.CommandText = R;
        da.SelectCommand.Connection = conbase03;
        da.SelectCommand.CommandTimeout = 1200;
        ds = new DataSet();
        int KKl = da.Fill(ds, "list0");
        IDLEVEL = Int16.Parse(ds.Tables["list0"].Rows[0][0].ToString());
        switch (IDLEVEL)
        {
            //                    1: Монография
            //-100: Коллекция
            //-2: Сводный уровень многотомного издания
            //2: Том (выпуск) многотомного издания
            //-3: Сводный уровень сериального издания
            //-33: Сводный уровень подсерии, входящей в серию
            //3: Выпуск сериального издания
            //-4: Сводный уровень сборника
            //4: Часть сборника
            //-5: Сводный уровень продолжающегося издания
            //5: Выпуск продолжающегося издания
            case -2: //   Сводный уровень многотомного издания
                C = "m";
                break;
            case 2: //   Том (выпуск) многотомного издания
                C = "m";
                break;
            case -3: //   Серия. Сводный уровень
                C = "s";
                break;
            case -33: //  Подсерия. Сводный уровень
                C = "s";
                break;
            case -5: //  Продолжающееся издание. Сводный уровень
                C = "s";
                break;
            case -100: //  Подборка. Сводный уровень
                C = "c";
                break;
            case -4:  //  Сборник. Часть - аналитика
                C = "a";
                break;
            default:
                C = "m";
                break;
        }
        MARKER = "     " + POZ5 + "a" + C + "  2200000 i 450 ";
        R = " SELECT P.ID  "
    + " FROM " + BAZA + "..DATAEXT DE LEFT JOIN " + BAZA + "..DATAEXTPLAIN P ON P.IDDATAEXT=DE.ID "
          + " WHERE MNFIELD=899 AND MSFIELD='$b' AND P.IDMAIN=" + IDM.ToString()
          + " AND CHARINDEX('ретро-конверсии карт',PLAIN)>0 ";
        da.SelectCommand = new SqlCommand();
        da.SelectCommand.CommandText = R;
        da.SelectCommand.Connection = conbase03;
        da.SelectCommand.CommandTimeout = 1200;
        ds = new DataSet();
        KK2 = da.Fill(ds, "list0");
        if (KK2 > 0)
        {
            MARKER = MARKER.ToString().PadLeft(17) + '1' + MARKER.Substring(19, 30);
        }   //-- Запись МАРКЕРА
        R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
          + " VALUES ('" + IDSession + "'," + IDM.ToString() + ",0,0,0,0,0,'" + MARKER + "')";
        command = new SqlCommand(R, conbase01);
        conbase01.Open();
        command.CommandTimeout = 1200;
        command.ExecuteNonQuery();
        conbase01.Close();

    }
    //
    //-- =============================================
    //-- Description:	Инициализвция Поля 100
    //-- =============================================
    private void INITP100(String IDSession, Int32 IDM, Int16 IDLEVEL)
    {
        Int32 KK, L;
        String P100, C1, T;
        //-- Поле 100
        //-- Данные общейй обработки
        //--Дата ввода записи в файл * 8 0-7 
        //-- Восемь цифровых символов в стандартной форме по ISO 8601-2004 для даты:
        //-- ГГГГMMДД, где ГГГГ соответствует году, ММ - месяцу, при необходимости с
        //-- начальным нулем, ДД - дню месяца, также с начальным нулем, если необходимо.
        //--Тип даты публикации * 1 8 
        //--  а = текущий продолжающийся ресурс 
        //--    Дата публикации 1 содержит год начала публикации. Если дата начала
        //--     публикации точно не известна, вместо любой неизвестной цифры
        //--     проставляется символ пробела: '#'.
        //--    Дата публикации 2 всегда содержит 9999. 
        //--  b = продолжающийся ресурс, публикация которого прекращена 
        //--    Дата публикации 1 содержит год начала публикации продолжающегося
        //--     ресурса. Если дата начала публикации точно не известна, вместо
        //--     любой неизвестной цифры проставляется символ пробела: '#'.
        //--    Дата публикации 2 содержит год прекращения издания. Для ресурсов,
        //--     о которых известно, что они больше не издаются, но последняя дата не
        //--     определена, вместо любой неизвестной цифры проставляется символ пробела: '#'. 
        //--  с = продолжающийся ресурс с неизвестным статусом 
        //--   Продолжающийся ресурс, о котором точно не известно, издается ли он
        //--   сейчас, или его издание прекращено. Дата публикации 1 содержит год
        //--   начала публикации продолжающегося ресурса. Если дата начала публикации
        //--   точно не известна, вместо любой неизвестной цифры проставляется знак '#'.
        //--   Дата публикации 2 содержит четыре символа пробела: ####. 
        //--  d = монография, изданная в одном томе или изданная в течение одного
        //--     календарного года 
        //--    Монография, изданная в одном томе, либо в нескольких томах, изданных
        //--     в одно время или с одинаковой датой издания, т.е. изданная в течение
        //--     одного календарного года. Если дата точно не известна, используется
        //--     код 'f'. Однако, если дата точно не известна, но указана в области
        //--     выходных данных как единичный год, например, [1769?], или [около 1769],
        //--     то используется код 'd'.
        //--    Описание выпуска продолжающегося ресурса или тома многотомника
        //--     содержит в позиции 100$a/8 код d. 
        //--    Код 'd' используется также, если известна дата copyright'a, но
        //--     неизвестна дата издания, иначе говоря, в этом случае дата
        //--     copyright'a используется вместо даты издания. 
        //--    Если монография издавалась с интервалом по времени, используется код 'g'.
        //--   Дата публикации 1 содержит год издания.
        //--   Дата публикации 2 содержит четыре символа пробела: ####. 
        //--  e = репродуцированный документ 
        //--   Каталогизируемый документ является репринтом, перепечаткой,
        //--    факсимильной копией, и т.д., но не новым изданием. Для новых
        //--    изданий используются коды: 'd', 'f', 'g', или 'h', в соответствии
        //--    с правилами их применения.
        //--   Если это продолжающийся ресурс, то указывается начальный год
        //--    переиздания и начальный год издания.
        //--   Дата публикации 1 содержит год издания репродукции.
        //--   Дата публикации 2 содержит год издания оригинала.
        //--   Если одна из дат точно не известна, вместо любой неизвестной цифры
        //--    указывается символ пробела: '#'. 
        //--  f = монография, дата публикации которой точно не известна 
        //--    Дата публикации 1 содержит наиболее раннюю из предполагаемых дат издания.
        //--    Дата публикации 2 содержит наиболее позднюю из возможных дат издания.
        //--  g = монография, публикация которой продолжается более года 
        //--   Дата публикации 1 содержит год начала издания. Если начальная дата издания точно не известна, вместо любой неизвестной цифры проставляется пробел: '#'.
        //--   Дата публикации 2 содержит дату окончания издания или 9999, если издание все еще продолжается. Если дата окончания точно не известна, вместо любой неизвестной цифры проставляется пробел: '#'.
        //--   В редких случаях на однотомном документе (или выпуске сериального документа)
        //--    указаны два года издания. В этих случаях используется код 'g'
        //--    с соответствующими датами. 
        //--  h = монография с фактической датой публикации и датой присвоения
        //--     авторского права / привилегии 
        //--    Дата публикации отличается от даты присвоения авторского
        //--     права / привилегии, указанного в документе. Если дата публикации
        //--     неизвестна, используется код 'd'. Привилегия определяется как
        //--     монопольное право, предоставляемое государственным органом автору
        //--     или книготорговой организации на издание в течение установленного
        //--     периода времени.
        //--   Дата публикации 1 содержит дату публикации.
        //--   Дата публикации 2 содержит дату присвоения авторского права / привилегии. 
        //--  i = монографии, имеющие как дату производства, так и дату реализации 
        //--   Используется для фильмов, аудиовизуальных материалов и т.д., когда
        //--    есть различия между датой производства документа и датой его реализации.
        //--   Дата публикации 1 содержит дату реализации.
        //--   Дата публикации 2 содержит дату производства.
        //--  j = документ с точной датой публикации 
        //--    используется в случае, когда важно записать месяц (и, возможно,
        //--    день) публикации.
        //--   Дата публикации 1 содержит год публикации.
        //--   Дата публикации 2 содержит дату (месяц и день) в формате ММДД,
        //--    где ММ - обозначение месяца, ДД - дня, при необходимости с
        //--    дополнительными нулями. Если день не известен, или не имеет
        //--    существенного значения, позиции 15 и 16 заполняются символами
        //--    пробела: '#'. 
        //--  u = дата(ы) публикации неизвестна(ы) 
        //--     Используется, если дату издания определить невозможно, и никакая
        //--      дата не может быть присвоена документу.
        //--   Дата публикации 1 содержит четыре символа пробела: ####.
        //--   Дата публикации 2 содержит четыре символа пробела: ####. 
        //--
        //--Дата публикации 1 * 4 9-12 
        //--Дата публикации 2 * 4 13-16 
        //--
        //--Код целевого назначения *** 3 17-19 
        //-- а = для юношества, общего характера (используется вместо кодов b, c, d или е, когда эти коды не используются или не могут быть использованы).
        //-- b = для детей дошкольного возраста, 0-5 лет
        //-- с = для детей младшего возраста, 5-10 лет
        //-- d = для детей среднего возраста, 9-14 лет
        //-- е = для юношества, возраст 14-20 лет
        //-- k = для взрослых, научная
        //-- m = для взрослых, общего характера
        //-- u = неизвестно 

        //--Код правительственной публикации ** 1 20 
        //--  а = федеральный/национальный 
        //--  b = республика, штат/провинция 
        //--  с = край, область, округ, графство/департамент 
        //--  d = местный (муниципальный, городской и т.д.) 
        //--  е = межтерриториальный (включающий разные департаменты и правительства ниже национального уровня) 
        //--  f = межправительственный 
        //--  g = нелегальное правительство или правительство в изгнании 
        //--  h = уровень не определен 
        //--  u = не известно 
        //--  y = неправительственная публикация 
        //--  z = другой административный уровень 

        //--Код модифицированной записи ** 1 21 
        //--  Заполнение обязательно, если производится модификация записи.
        //--  Этот односимвольный код показывает, достаточно ли оказалось имеющегося
        //--   набора символов для передачи данных в той форме, как они представлены в
        //--   документе. Если нет, а это может получиться, например, из-за
        //--   использования в документе специфических шрифтов или математических
        //--   формул, то для представления этих данных может использоваться
        //--   транслитерация, греческие буквы или какая-либо другая нотация.
        //--   Если при этом данных, введенных в запись, достаточно для воспроизведения
        //--   оригинального написания, то запись считается модифицированной,
        //--   и в позиции 100$a/21 проставляется 1. Если имеющихся наборов символов
        //--   оказывается достаточно для полноценного их представления в записи,
        //--   что как правило и происходит, или данные в записи не полностью отражают
        //--   исходные данные, - такая запись считается немодифицированной, и в
        //--   позиции 100$a/21 проставляется 0. 
        //--
        //--Язык каталогизации * 3 22-24 
        //--  Всегда RUS
        //-- 
        //--Код транслитерации ** 1 25 
        //--  a = правила транслитерации ISO
        //--  b = другие правила
        //--  c = несколько схем транслитерации - ISO или другие правила
        //--  y = транслитерация не используется 
        //--
        //--Наборы символов * 4 26-29 
        //--Дополнительные наборы символов ** 4 30-33 
        //--Два двухсимвольных кода, определяющие основные наборы графических символов, используемых при обмене записями. Позиции 26-27 определяют набор G0, позиции 28-29 - набор G1. Если набор G1 не нужен, позиции 28-29 содержат символы пробела: '##'. 
        //--Используются следующие двухсимвольные коды (при необходимости перечень их может быть расширен): 
        //--01 = ISO 646, версия IRV (основной латинский набор)
        //--02 = ISO регистрация #37 (основной кириллический набор)
        //--03 = ISO 5426 (расширенный латинский набор)
        //--04 = ISO DIS 5427 (расширенный кириллический набор)
        //--05 = ISO 5428 (греческий набор)
        //--06 = ISO 6438 (набор кодированных африканских символов)
        //--07 = ISO 10586 (набор символов грузинского алфавита)
        //--08 = ISO 8957 (набор символов иврита) таблица 1
        //--09 = ISO 8957 (набор символов иврита) таблица 2
        //--10 [Зарезервировано]
        //--11 = ISO 5426-2 (латинские символы, используемые в редких европейских языках и устаревших типографиях)
        //--50 = ISO 10646 (Unicode, UTF-8)
        //--79 = Code Page 866
        //--89 = WIN 1251
        //--99 = KOI-8
        //--Следует заметить, что ISO 10646, будучи 16-битным набором символов,
        //-- содержит все необходимые символы. Если позиции 26-27 содержат код "50",
        //-- то он используется для наборов C0, C1 и G0-G3. Позиции 28-33 содержат
        //-- символы пробелов. 
        //--
        //--Графика заглавия ** 2 34-35 
        //-- Заполнение обязательно, если графика заглавия отличается от обычной для
        //--   языка, указанного в подполе 101$g (или 101$a, если нет $g).
        //-- Двухсимвольный код, определяющий графику заглавия, используемого как
        //--   основное заглавие (в случае сериальных документов - ключевое заглавие).
        //---Относится к алфавиту, в котором заглавие приводится на источнике описания,
        //--   а не к набору символов записи (см. пример 28). 
        //-- При отсутствии международных стандартов наборов кодов, рекомендуются
        //--   следующие коды: 
        //--ba = латинская
        //--са = кириллическая
        //--da = японская - неопределенная графика
        //--db = японская - канджи
        //--dc = японская - кана
        //--еа = китайская
        //--fa = арабская
        //--ga = греческая
        //--ha = иврит
        //--ia = тайская
        //--ja = деванагари
        //--ка = корейская
        //--la = тамильская
        //--ma = грузинская
        //--mb = армянская
        //--zz = другая 
        //--
        if (IDLEVEL > 0) C1 = "u        ";
        else
        {
            if (IDLEVEL == -3 || IDLEVEL == -33 || IDLEVEL == -5) C1 = "a    9999";
            else C1 = "b        ";
        }
        // ДАТА ИЗДАНИЯ
        R = " SELECT  PLAIN  "
        + " FROM " + BAZA + "..DATAEXT DE LEFT JOIN " + BAZA + "..DATAEXTPLAIN P ON P.IDDATAEXT=DE.ID "
          + " WHERE P.IDMAIN= " + IDM.ToString() + " AND MNFIELD=2100 ";
        da.SelectCommand = new SqlCommand();
        da.SelectCommand.CommandText = R;
        da.SelectCommand.Connection = conbase03;
        da.SelectCommand.CommandTimeout = 1200;
        ds = new DataSet();
        int KK100 = da.Fill(ds, "list0");
        if (KK100 > 0)
        {
            T = ds.Tables["list0"].Rows[0][0].ToString().Trim();
            T = T.Replace("?", "");
            T = T.Replace("(", "[").Trim();
            T = T.Replace(")", "]").Trim();
            //T = T.Replace("[", "").TrimStart();
            //T = T.Replace("]", "").TrimStart();
            T = T.Replace("/ ", "[").Trim();
            T = T.Replace(" /", "]").Trim();
            //if (IDM == 1396395)
            //{
            //    MessageBox.Show(T);
            //}
            T = T.Replace("cop.", "").Trim();
            T = T.Replace("Cop.", "").Trim();
            T = T.Replace("pref.", "").Trim();
            T = T.Replace("ante ", "").Trim();
            T = T.Replace("post ", "").Trim();
            T = T.Replace("vorw.", "").Trim();
            T = T.Replace("Vorw.", "").Trim();
            T = T.Replace("[]", "").Trim();

            if (T.StartsWith("[") && T.EndsWith("]"))
            {
                T = T.Substring(1, T.Length - 2).Trim(); ;
                C1 = "f";
            }

            if (T.Length > 3)
            {
                if ((T.Length == 8) && (Int32.TryParse(T, out L) == true)) //ISNUMERIC(T)
                {
                    if (C1 == "f")
                    {
                        C1 += T;
                    }
                    else
                    {
                        C1 = "b" + T;
                    }
                }
                else
                {
                    String TT = T.ToUpper().Replace(" ", "");
                    if (TT.StartsWith("Б.Г.") || TT.StartsWith("S.A.") || TT.StartsWith("O.J.")
                        || TT.StartsWith("Б.Г") || TT.StartsWith("S.A") || TT.StartsWith("O.J")
                        || TT.StartsWith("N.D.") || TT.StartsWith("S.L") || TT.StartsWith("O.J"))
                    {
                        C1 = "u        ";
                        goto M1;
                    }

                    if (T.StartsWith("ca "))
                    {
                        T = T.Substring(3).TrimStart();
                    }
                    if (T.StartsWith("An "))
                    {
                        if (T.Substring(T.Length - 1, 1) != "]")
                        {
                            T.Substring(T.Length - 4, 4);
                        }
                        if (Int32.TryParse(T, out L) == true)
                        {
                            C1 = "d" + T + "    ";
                            goto M1;
                        }
                    }
                    if (T.StartsWith("-"))
                    {
                        if (T.Length >= 5)
                        {
                            C1 = "f" + "0001" + T.Substring(1, 4);
                        }
                    }
                    if (T.Length == 4)
                    {
                        if (T.EndsWith("---"))
                        {
                            C1 = "f" + T.Substring(0, 1) + "000" + T.Substring(0, 1) + "999";
                        }
                        else
                        {
                            if (T.EndsWith("--"))
                            {
                                C1 = "f" + T.Substring(0, 2) + "00" + T.Substring(0, 2) + "99";
                            }
                            else
                            {
                                if (T.EndsWith("-"))
                                {
                                    C1 = "f" + T.Substring(0, 3) + "0" + T.Substring(0, 3) + "9";
                                }
                                else
                                {
                                    C1 = "d" + T + "    ";
                                }
                            }
                        }
                        goto M1;
                    }
                    if (T.Length > 4)
                    {
                        String S = T.Substring(0, 4);
                        if (Int32.TryParse(S, out L) == true)
                        {
                            C1 = "d" + S + "    ";
                            goto M1;
                        }
                    }
                }
            } // Длина Т > 3
            else
            {// Длина Т <= 3
                KK100 = T.IndexOf('[');
                if (KK100 > 0)
                {
                    T = T.Substring(KK100 + 1, T.Length - KK100);
                    T = T.Remove(T.Length - 1); // Удаление]
                }
                T = T.Replace("/", "");
                if (T.EndsWith("--") == true)
                {
                    C1 = "f0" + T.Substring(0, 1) + "000" + T.Substring(0, 1) + "99";
                }
                else
                {
                    if (T.Length == 2)
                    {
                        if (T.EndsWith("-") == true)
                        {
                            C1 = "f00" + T.Substring(0, 1) + "000" + T.Substring(0, 1) + "9";
                        }
                        else
                        {
                            C1 = "d  " + T + "    ";
                        }
                    }
                    else
                    {
                        if (T.Length == 3)
                        {
                            if (T.EndsWith("-") == true)
                            {
                                C1 = "f0" + T.Substring(0, 2) + "00" + T.Substring(0, 2) + "9";
                            }
                            else
                            {
                                C1 = "d0" + T + "    ";
                            }
                        }
                        else
                        { // Длина Т = 1
                            C1 = "d000" + T + "    ";
                        }
                    }
                }
                //if (Int32.TryParse(T, out L) == true)
                //{
                //    C1 = 'f' + T;
                //}
                goto M1;
            }// Длина Т <= 3
            if (T.IndexOf("Vor") != -1) ////   ??????????????????  == "Vor"
            {
                T = T.Substring(T.Length - 4, 4); // Последние 4 цифры
                if (Int32.TryParse(T, out L) == true)
                {
                    C1 = 'f' + T + T;
                }
                goto M1;
            }
            T.Replace("XII", "12");
            T.Replace("XI", "11");
            T.Replace("IX", "09");
            T.Replace("X", "10");
            T.Replace("VIII", "08");
            T.Replace("VII", "07");
            T.Replace("VI", "06");
            T.Replace("IV", "04");
            T.Replace("V", "05");
            T.Replace("III", "03");
            T.Replace("II", "02");
            T.Replace("I", "01");
            T.Replace(".", "");
            if (T.IndexOf('=') != -1)
            {
                KK = T.IndexOf('=');
                T = T.Substring(KK + 1).TrimStart();
            }
        M2: ;
            if (T.Length == 4 && Int32.TryParse(T, out L) == true)
            {
                C1 = "d" + T + "    ";
                goto M1;
            }
            if (T.Substring(0, 1) == "[")
            {
                T = T.Remove(0, 1);
                L = T.IndexOf(']');
                if (L != -1)
                {
                    T = T.Remove(L).Trim();
                }
                goto M2;
            }
            T = T.Trim();
            if (T.Length == 9)
            {
                T = T.Replace("/", "");
                if (Int32.TryParse(T, out L) == true)
                {
                    C1 = "f" + T;
                    goto M1;
                }
            } //T.Length == 9

            if (Int32.TryParse(T, out L) == true)
            {
                if (T.Length > 4)
                {
                    if (T.Length == 7) T = "0" + T;
                    if (T.Length == 6) T = "  " + T;
                    C1 = "j" + T.Substring(T.Length - 4, 4) + T.Substring(0, 4);
                    goto M1;
                }
            }
            if (T.Length > 4)
            {
                T = T.Substring(T.Length - 4, 4);
            }
            if (Int32.TryParse(T, out L) == true)
            {
                C1 = "d" + T + "    ";
            }
            else
            {
                if (T.EndsWith("---") == true)
                {
                    C1 = "f" + T.Substring(0, 1) + "0" + T.Substring(0, 1) + "9";
                }
                else
                {
                    if (T.EndsWith("--") == true)
                    {
                        C1 = "f" + T.Substring(0, 2) + "00" + T.Substring(0, 2) + "99";
                    }
                    else
                    {
                        if (T.EndsWith("-") == true)
                        {
                            C1 = "f" + T.Substring(0, 3) + "0" + T.Substring(0, 3) + "9";
                        }
                    }
                }
            }
        }
    M1: ;
        if (C1.StartsWith("f"))
        {
            if (C1.Length == 5)
            {
                C1 = "d" + C1.Substring(1, 4);
            }
            else
            {
                if (C1.Length == 9)
                {
                    if (C1.EndsWith("    "))
                    {
                        C1 = "d" + C1.Substring(1, 8);
                    }
                }
            }
        }
        // 2010-11-30T12:12:48f00012000u  y0rusy50      ca
        P100 = DateTime.Now.ToString("s").Replace("-", "").Substring(0, 8) + C1 + "u  y0rusy50      ca";
        R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
         + " VALUES ('" + IDSession + "'," + IDM.ToString() + ",0,100,' ',' ','a',N'" + P100 + "')";
        command = new SqlCommand(R, conbase01);
        conbase01.Open();
        command.CommandTimeout = 1200;
        command.ExecuteNonQuery();
        conbase01.Close();
    } // INITP100
    //
    //*********************************************************
    private void INITP105(String IDSession, Int32 IDM)
    {
        //-- Description:	Инициализвция Поля 105
        //-- Description: ПОЛЕ КОДИРОВАННЫХ ДАННЫХ: ТЕКСТОВЫЕ МАТЕРИАЛЫ, МОНОГРАФИЧЕСКИЕ 
        String P105, R;
        P105 = "y   z   000|y";
        R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
+ " VALUES ('" + IDSession + "'," + IDM.ToString() + ",0,105,' ',' ','a',N'" + P105 + "') ";
        command = new SqlCommand(R, conbase01);
        conbase01.Open();
        command.CommandTimeout = 1200;
        command.ExecuteNonQuery();
        conbase01.Close();
    } // INITP105

    //-- =============================================
    private void INITP110(String IDSession, Int32 IDM, Int16 IDLEVEL)
    // 2011.04.12 13:00
    {
        //-- Description:	Инициализвция Поля 110
        //--110 ПОЛЕ КОДИРОВАННЫХ ДАННЫХ: ПРОДОЛЖАЮЩИЕСЯ РЕСУРСЫ 
        //$a / (позиция символа 0) Определитель вида продолжающегося ресурса
        //   Односимвольный код, указывающий на вид продолжающегося ресурса. Заполнение обязательно, если поле 110 приводится в записи.
        //   a = периодическое издание
        //       Сериальное издание, выходящее через определенные промежутки времени, как правило, постоянным для каждого года числом номеров (выпусков), не повторяющимися по содержанию, однотипно оформленными нумерованными и (или) датированными выпусками, имеющими одинаковое название (ГОСТ 7.60-2003). 
        //   b = монографическая серия
        //       Совокупность томов, объединенных общностью замысла, тематики, целевым или читательским назначением. 
        //   с = газета
        //       Периодическое газетное издание, выходящее через краткие промежутки времени, содержащее официальные материалы, оперативную информацию и статьи по актуальным общественно-политическим, научным, производственным и другим вопросам, а также литературные произведения и рекламу (ГОСТ 7.60-2003) 
        //   е = интегрируемое издание со сменными листами
        //       Библиографический ресурс, состоящий из основной части, обновляемой за счет отдельных листов, которые вставляются, удаляются и/или заменяются 
        //   f = база данных
        //       Коллекция логически взаимосвязанных данных, собранных вместе в виде одного или нескольких компьютерных файлов, как правило, создаваемая и управляемая с помощью системы управления базой данных 
        //   g = обновляемый веб-сайт
        //       Код используется для веб-сайтов, которые обновляются, но не могут быть отнесены ни к одной из указанных выше категорий 
        //   z = другие 
        String C, P110;
        switch (IDLEVEL)
        {
            case -2: // Многотомное издание
                C = "b";
                break;
            default:
                C = "a"; // Серии, подсерии и продолжающиеся издания
                break;
        }
        P110 = C + "uuz||||uu0";
        R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
       + " VALUES ('" + IDSession + "'," + IDM.ToString() + ",0,110,' ',' ','a',N'" + P110 + "') ";
        command = new SqlCommand(R, conbase01);
        conbase01.Open();
        command.CommandTimeout = 1200;
        command.ExecuteNonQuery();
        conbase01.Close();
        return;
    } // INITP110
    //*********************************************************
    //-- Description:	ФОРМИРОВАНИЕ ПОЛЯ 801 - Источник записи
    //-- =============================================
    private void OBR98(String IDSession, Int32 IDM, String STATUS)
    {
        String D, S;
        //--801  ИСТОЧНИК ЗАПИСИ 
        //--Поле содержит указание на источники записи, к которым относятся:
        //-- организация, создающая данные; организация, преобразующая данные
        //-- в машиночитаемую форму; организация, модифицирующая первоначальные
        //-- записи / данные; организация, распространяющая записи. 
        //--Обязательное. Повторяется для каждой из перечисленных функций,
        //-- выполняемых той или иной организацией. 
        //--Индикатор 1: # (не определен) 
        //--Индикатор 2: Индикатор функции 
        //-- 0 - Агентство, производящее первоначальную каталогизацию 
        //-- 1 - Агентство, преобразующее данные 
        //-- 2 - Агентство, вносящее изменения в запись 
        //-- 3 - Агентство, распространяющее запись 
        //--  $a Страна
        //--    Двухсимвольный код страны, в которой находится организация,
        //-- указанная в подполе $b. См. Приложение B. 
        //--   Обязательное. Не повторяется.
        //--  $b Наименование (в кодированной или полной форме) организации.
        //--    В связи с отсутствием общепринятых международных кодов,
        //--     рекомендуется использовать коды из MARC Code list for Organizations
        //--     - cписка кодов для организаций, который содержит коды для многих
        //--     библиографирующих организаций как внутри, так и вне США. Также
        //--     может быть использовано полное наименование организации или
        //--     национальный код. 
        //--   Обязательное. Не повторяется.
        //--  $с Дата выполнения той операции с записью, на которую указывает
        //--      индикатор 2. Дату следует записывать в соответствии с международным
        //--      стандартом ISO 8601-2004, указывая цифры года, месяца и дня без
        //--      разделителей между ними, т.е. ГГГГММДД. 
        //--    Обязательное при наличии данных. Не повторяется.
        //--  $g Правила каталогизации
        //--    Подполе содержит сокращенное наименование правил каталогизации,
        //--     которые использовались для подготовки библиографической записи.
        //--     Коды некоторых Правил приведены в Приложении Н. Подполе следует
        //--     использовать в том случае, если второй индикатор имеет значение 0
        //--     (для организации, выполняющей первоначальную каталогизацию)
        //--     или 2 (для организации, модифицирующей запись). 
        //--    Факультативное. Повторяется (при использовании в одной записи
        //--     различных правил).
        //--    RCR - код Российских правил каталогизации (Москва, 2005)
        //--  $2 Код формата
        //--    Подполе содержит наименование формата машиночитаемой записи.
        //--     Коды машиночитаемых форматов приведены в Приложении H. 
        //--   Факультативное. Не повторяется.
        //--   rusmarc - код РУСМАРКа
        //--   unimarc - UNIMARC Manual (London : IFLA UBCIM Programme)
        //--   unimrur - Руководство по UNIMARC = UNIMARC Manual. (Москва : 
        //--               Государственная публичная научно-техническая библиотека
        //--               России) 
        //--   marc21  - MARC 21 format for bibliographic data (Washington : Library
        //--              of Congress) 
        //--   usmarc  - USMARC Bibliographic Format. (Washington : Libray of Congress) 
        //--
        //-- Во многих случаях одна и та же организация выполняет все или отдельные
        //--  выше перечисленные функции. Поле следует повторять для каждой функции
        //--  т. к. даты обработки могут быть различными.
        //-- В машиночитаемой записи должно быть не менее двух вхождений поля 801,
        //--  т.к. для того чтобы данные привести в машиночитаемую форму, их надо
        //--  подготовить. Т.о. у первого вхождения поля 801 индикатор = 0, а у
        //--  второго = 1.
        //-- Если организация сразу каталогизирует документ в машиночитаемой форме,
        //--  то должно быть приведены два вхождения поля 801 с одинаковыми подполями
        //--  $c - дата составления. 
        //--
        //-- Взаимосвязанные поля 
        //--  МАРКЕР ЗАПИСИ, позиция символа 18 и позиция символа 5. 
        //--  035  ДРУГИЕ СИСТЕМНЫЕ НОМЕРА 
        //--   Идентификатор и наименование организации, создавшей или
        //--    модифицировавшей запись, полученную из другой системы, могут также
        //--    быть введены в поле 035. 
        //--  100  ДАННЫЕ ОБЩЕЙ ОБРАБОТКИ, Дата ввода записи в файл ($a, позиции
        //--   символов 0-7). Дата в поле 100 может иметь такое же значение, как
        //--   и дата преобразования записи в машиночитаемую форму, однако ее
        //--   необходимо повторить в поле 801.
        //-- 801 0 -- КАТАЛОГИЗАЦИЯ


        //From: Ирина Николаевна Мустафина [mailto:market@ditm.ru] 
        //Sent: Friday, January 23, 2015 11:49 AM
        //To: 'Светлана Балакерская'
        //Subject: RE: ЛИБНЕТ

        //Здравствуйте, Светлана Борисовна!

        //Сведения об экземплярах не нужны. Достаточно, чтобы в записи было само поле местонахождение с сиглой Вашей библиотеки: 899##$aВГБИЛ

        R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
         + " VALUES ('" + IDSession + "'," + IDM.ToString() + ",-4,801,' ','0','a',N'RU')";
        command = new SqlCommand(R, conbase01);
        conbase01.Open();
        command.CommandTimeout = 1200;
        command.ExecuteNonQuery();
        conbase01.Close();
        R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
         + " VALUES ('" + IDSession + "'," + IDM.ToString() + ",-4,801,' ','0','b',N'RuMoBil')";
        command = new SqlCommand(R, conbase01);
        conbase01.Open();
        command.CommandTimeout = 1200;
        command.ExecuteNonQuery();
        conbase01.Close();
        //  -- Дата каталогизации  - поле DateCreate из таблицы MAIN
        R = " SELECT REPLACE(CONVERT(VARCHAR(100),DateCreate,102),'.','')"
        + " FROM " + BAZA + "..MAIN WHERE ID = " + IDM.ToString();
        da.SelectCommand = new SqlCommand();
        da.SelectCommand.CommandText = R;
        da.SelectCommand.Connection = conbase03;
        da.SelectCommand.CommandTimeout = 1200;
        ds = new DataSet();
        da.Fill(ds, "list0");
        D = ds.Tables["list0"].Rows[0][0].ToString();
        R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
        + " VALUES ('" + IDSession + "'," + IDM.ToString() + ",-4,801,' ','0','c',N'" + D + "')";
        command = new SqlCommand(R, conbase01);
        conbase01.Open();
        command.CommandTimeout = 1200;
        command.ExecuteNonQuery();
        conbase01.Close();
        // -- Код правил каталогизации
        R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
         + " VALUES ('" + IDSession + "'," + IDM.ToString() + ",-4,801,' ','0','g',N'RCR')";
        command = new SqlCommand(R, conbase01);
        conbase01.Open();
        command.CommandTimeout = 1200;
        command.ExecuteNonQuery();
        conbase01.Close();

        //-- 801 1 -- КОНВЕРТИРОВАНИЕ
        R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
         + " VALUES ('" + IDSession + "'," + IDM.ToString() + ",-3,801,' ','1','a',N'RU')";
        command = new SqlCommand(R, conbase01);
        conbase01.Open();
        command.CommandTimeout = 1200;
        command.ExecuteNonQuery();
        conbase01.Close();

        R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
        + " VALUES ('" + IDSession + "'," + IDM.ToString() + ",-3,801,' ','1','b',N'RuMoBil')";
        //+ "N'Федеральное государственное учреждение культуры ' + CHAR(34) + N'"
        //+ "Всероссийская государственная библиотека иностранной литературы имени  М.И.Рудомино' + CHAR(34))";
        command = new SqlCommand(R, conbase01);
        conbase01.Open();
        command.CommandTimeout = 1200;
        command.ExecuteNonQuery();
        conbase01.Close();

        if (STATUS == "c") // -- Редактирование
        {
            //-- 801 2 -- Редактирование
            R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
             + " VALUES ('" + IDSession + "'," + IDM.ToString() + ",-2,801,' ','2','a',N'RU')";
            command = new SqlCommand(R, conbase01);
            conbase01.Open();
            command.CommandTimeout = 1200;
            command.ExecuteNonQuery();
            conbase01.Close();

            R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
            + " VALUES ('" + IDSession + "'," + IDM.ToString() + ",-2,801,' ','2','b',N'RuMoBil')";
            //+ "N'Федеральное государственное учреждение культуры ' + CHAR(34) + N'"
            //+ "Всероссийская государственная библиотека иностранной литературы имени  М.И.Рудомино' + CHAR(34))";
            command = new SqlCommand(R, conbase01);
            conbase01.Open();
            command.CommandTimeout = 1200;
            command.ExecuteNonQuery();
            conbase01.Close();
        }

        //-- 801 3 - РАСПРОСТРАНЕНИЕ
        R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
         + " VALUES ('" + IDSession + "'," + IDM.ToString() + ",-1,801,' ','3','a',N'RU') ";
        command = new SqlCommand(R, conbase01);
        conbase01.Open();
        command.CommandTimeout = 1200;
        command.ExecuteNonQuery();
        conbase01.Close();

        R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
         + " VALUES ('" + IDSession + "'," + IDM.ToString() + ",-1,801,' ','3','b',N'RuMoBil')";
        //+ "N'Федеральное государственное учреждение культуры ' + CHAR(34) + N'"
        //+ "Всероссийская государственная библиотека иностранной литературы имени  М.И.Рудомино' + CHAR(34))";
        command = new SqlCommand(R, conbase01);
        conbase01.Open();
        command.CommandTimeout = 1200;
        command.ExecuteNonQuery();
        conbase01.Close();
        //  -- Дата создания записи - первые 8 символов из поля 100
        R = "SELECT LEFT(POL,8) FROM TECHNOLOG_VVV..RUSM WHERE MET=100 and IDMAIN=" + IDM.ToString()
            + " and session='" + IDSession + "'";
        da.SelectCommand = new SqlCommand();
        da.SelectCommand.CommandText = R;
        da.SelectCommand.Connection = conbase01;
        da.SelectCommand.CommandTimeout = 1200;
        ds = new DataSet();
        da.Fill(ds, "list0");
        S = ds.Tables["list0"].Rows[0][0].ToString();
        R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
        + " VALUES ('" + IDSession + "'," + IDM.ToString() + ",-1,801,' ','3','c',N'" + S + "')";
        command = new SqlCommand(R, conbase01);
        conbase01.Open();
        command.CommandTimeout = 1200;
        command.ExecuteNonQuery();
        conbase01.Close();
        //  -- Формат записи
        R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
        + " VALUES ('" + IDSession + "'," + IDM.ToString() + ",-1,801,' ','3','Э',N'rusmarc') ";
        command = new SqlCommand(R, conbase01);
        conbase01.Open();
        command.CommandTimeout = 1200;
        command.ExecuteNonQuery();
        conbase01.Close();
        //Сведения об экземплярах не нужны. Достаточно, чтобы в записи было само поле местонахождение с сиглой Вашей библиотеки: 899##$aВГБИЛ
        // Мустафина
        R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
+ " VALUES ('" + IDSession + "'," + IDM.ToString() + ",-1,899,' ',' ','a',N'ВГБИЛ') ";
        command = new SqlCommand(R, conbase01);
        conbase01.Open();
        command.CommandTimeout = 1200;
        command.ExecuteNonQuery();
        conbase01.Close();

    } // OBR98 - ФОРМИРОВАНИЕ ПОЛЯ 801 - Источник записи
    //
    //-- =============================================
    //-- Description:	Вид обработки = 1 - Вид издания Поле BJ921b
    //-- =============================================
    private void OBR1(String IDSession, Int32 IDM, Int32 DIDDATA, Int16 MET, String IND1, String IND2
        , String IDENT, String DSORT)
    // 2011.05.11
    {
        String MARKER, POL, P4;
        Int16 METU;
        Int32 IDL, KK2;
        //-- Значения поля 
        //--		Альбом
        //--		Атлас
        //--		Брошюра
        //--		Издание картографическое
        //--		Издание листовое
        //--		Издание музыкальное
        //--		Издание научное
        //--		Издание официальное
        //--		Издание справочное
        //--		Издание учебное
        //--		Каталог
        //--		Книжка памятная
        //--		Пособие учебно-методическое
        //--		Проспект
        //--		Путеводитель
        //--		Словарь
        //--	Трофей 2008
        //--		Учебник
        //--		Энциклопедия
        //-- 
        if (DSORT == "Издание картографическое")
        {
            //-- Модификация МАРКЕРА - поз. 6
            R = " SELECT POL FROM TECHNOLOG_VVV..RUSM  WHERE MET=0 AND IDMAIN=" + IDM.ToString()
                + " and session='" + IDSession + "'";
            da.SelectCommand = new SqlCommand();
            da.SelectCommand.CommandText = R;
            da.SelectCommand.Connection = conbase01;
            da.SelectCommand.CommandTimeout = 1200;
            ds = new DataSet();
            da.Fill(ds, "list0");
            MARKER = ds.Tables["list0"].Rows[0][0].ToString().Insert(6, "e");
            R = "UPDATE TECHNOLOG_VVV..RUSM set POL='" + MARKER + "' WHERE MET=0 AND IDMAIN="
     + IDM.ToString() + " and session='" + IDSession + "'";
            command = new SqlCommand(R, conbase01);
            conbase01.Open();
            command.CommandTimeout = 1200;
            command.ExecuteNonQuery();
            conbase01.Close();
        } // Издание картографическое
        else
        {
            if (DSORT == "Издание музыкальное")
            {
                //-- Модификация МАРКЕРА
                R = " SELECT POL FROM TECHNOLOG_VVV..RUSM  WHERE MET=0 AND IDMAIN=" + IDM.ToString()
        + " and session='" + IDSession + "'";
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.CommandText = R;
                da.SelectCommand.Connection = conbase01;
                da.SelectCommand.CommandTimeout = 1200;
                ds = new DataSet();
                da.Fill(ds, "list0");
                MARKER = ds.Tables["list0"].Rows[0][0].ToString().Insert(6, "c");
                R = "UPDATE TECHNOLOG_VVV..RUSM set POL=N'" + MARKER + "' WHERE MET=0 AND IDMAIN=" + IDM.ToString()
        + " and session='" + IDSession + "'";
                command = new SqlCommand(R, conbase01);
                conbase01.Open();
                command.CommandTimeout = 1200;
                command.ExecuteNonQuery();
                conbase01.Close();
            } // Издание музыкальное
        } // Модификация по. 6 МАРКЕРА
        //M0: ;
        R = " SELECT IDLEVEL FROM " + BAZA + "..MAIN WHERE ID = " + IDM.ToString();
        da.SelectCommand = new SqlCommand();
        da.SelectCommand.CommandText = R;
        da.SelectCommand.Connection = conbase03;
        da.SelectCommand.CommandTimeout = 1200;
        ds = new DataSet();
        KK2 = da.Fill(ds, "list0");
        IDL = Int32.Parse(ds.Tables["list0"].Rows[0][0].ToString());
        //-- MAIN.IDLEVEL =
        //--   1  - МОНОГРАФИЯ
        //-- 100  - ПОДБОРКА. СВОДНЫЙ УРОВЕНЬ
        //-- - 2  - МНОГОТОМНОЕ ИЗДАНИЕ. СВОДНЫЙ УРОВЕНЬ
        //--   2  - ТОМ МНОГОТОМНОГО ИЗДАНИЯ
        //-- - 3  - СЕРИЯ. СВОДНЫЙ УРОВЕНЬ
        //-- -33  - ПОДСЕРИЯ. СВОДНЫЙ УРОВЕНЬ
        //--   3  - ВЫПУСК СЕРИИ
        //-- - 4  - СБОРНИК. СВОДНЫЙ УРОВЕНЬ
        //--   4  - ЧАСТЬ СБОРНИКА
        //-- - 5  - ПРОДОЛЖАЮЩЕЕСЯ ИЗДАНИЕ. СВОДНЫЙ УРОВЕНЬ
        //--   5  - ВЫПУСК (НОМЕР) ПРОДОЛЖАЮЩЕГОСЯ ИЗДАНИЯ
        if (IDL > 0 || IDL == -100 || IDL == -4 || IDL == -2)
        {
            METU = 105;
            //-- Модификация поля 105
            //--105  ПОЛЕ КОДИРОВАННЫХ ДАННЫХ: ТЕКСТОВЫЕ МАТЕРИАЛЫ, МОНОГРАФИЧЕСКИЕ 
            //-- Поле содержит кодированные данные, относящиеся к монографическим текстовым изданиям.
            //-- Примечание: для описания монографических серий используется поле 110. 
            //--
            //--$a Кодированные данные о монографическом текстовом документе 
            //--    Все данные, записываемые в подполе $a, идентифицируются позицией символа
            //-- в подполе.
            //--    Позиции символов нумеруются от 0 до 12. Все позиции символов должны
            //-- быть представлены в подполе. Если элемент данных не кодируется,
            //-- соответствующие позиции заполняются символами-заполнителями: ' | '.
            //-- Если используется лишь часть возможных позиций, отведенных для данного
            //-- элемента, то неиспользуемые позиции содержат символ пробела: '#'. 
            //--    Элементы фиксированной длины подполя $a: 
            //--Наименование элемента данных Количество символов Позиции символов 
            //-- Коды иллюстраций               4                   0-3 
            //-- Коды формы содержания          4                   4-7 
            //-- Код конференции или совещания  1                    8 
            //-- Индикатор юбилейного издания   1                    9 
            //-- Индикатор указателя            1                   10 
            //-- Код литературного жанра        1                   11 
            //-- Код биографии                  1                   12  
            //--
            //--   Коды иллюстраций 
            //--    Для кодов иллюстраций отводится четыре позиции символов, которые
            //--     заполняются слева направо. Неиспользуемые позиции заполняются
            //--     пробелами (#). Если для описания документа могут быть применены
            //--     более четырех кодов (т.е. присутствуют более четырех типов
            //--     иллюстраций), следует выбрать первые четыре в том порядке, в
            //--     котором они приведены в списке ниже.
            //--    Если элемент данных не используется, позиции символов 0-3 заполняются
            //--     символами-заполнителями ' | '. Если элемент данных используется, но
            //--     присутствует менее четырех типов иллюстраций, оставшиеся позиции
            //--     заполняются символами пробела: '#'. 
            //--  a = иллюстрации 
            //--        Используется для типов иллюстраций, которые не включены в перечень,
            //--         приведенный ниже, например диаграммы, графики или типы иллюстраций,
            //--         не кодируемые отдельно. 
            //--  b = карты 
            //--  с = портреты. Индивидуальные и групповые. 
            //--  d = морские карты, предназначенные для использования в навигации. 
            //--  е = планы, например, планы территорий со зданиями. 
            //--  f = вкладные листы, содержащие иллюстративные материалы с текстовыми
            //--       пояснениями или без них и не являющиеся частью страниц или листов
            //--       основного текста. 
            //--  g = музыкальные произведения 
            //--     Только в текстовой форме. Для звукозаписей, являющихся
            //--       сопроводительным материалом, используется код 'm'. 
            //--  h = факсимиле 
            //--     Воспроизведение части или целого документа средствами репродуцирования
            //--      или печати.
            //--  i = гербы 
            //--  j = генеалогические таблицы, схемы 
            //--  k = формы (бланки, анкеты и т.д. )
            //--  l = образцы (шаблоны и т. п. )
            //--  m = звукозаписи. Например, пластинка или диск с звукозаписью, вложенные
            //--       в кармашек книги 
            //--  n = прозрачные пленочные материалы (transparencies) 
            //--     Например, набор transparencies, в кармашке, прикрепленном к книге. 
            //--  o = украшения и орнаменты, раскрашенные буквы в рукописи 
            //--  y = без иллюстраций 
            //--     Допускается единственная форма использования: y###. 
            //--
            //-- Коды формы содержания 
            //--  а = библиографическое издание 
            //--  b = каталог 
            //--  c = указатель 
            //--    Алфавитный перечень имен или предметов с информацией о них в этой же
            //--     работе или где-либо еще.
            //--    Если документ содержит указатель к собственному тексту, позиция 
            //--     105$a/10 (Индикатор указателя) должна содержать 1. 
            //--  d = реферат или резюме, включая описательные, справочные и информационные
            //--     рефераты 
            //--  e = словарь 
            //--  f = энциклопедия 
            //--  g = справочное издание 
            //--  h = описание проекта 
            //--  i = статистические данные 
            //--  j = учебник 
            //--  k = патентный документ 
            //--  l = стандарт 
            //--  m = диссертация (оригинал) 
            //--  n = законы и законодательные акты 
            //--  o = цифровые таблицы 
            //--  p = технический отчет 
            //--  q = экзаменационный лист 
            //--  r = литературный обзор/рецензия 
            //--  s = договоры 
            //--  t = карикатуры или комиксы 
            //--  v = диссертация (переработанная) 
            //--  w = религиозные тексты 
            //--  z = другой тип содержания 
            //--
            //-- Код конференции 
            //--  0 = не является изданием, публикуемым от имени конференции
            //--  1 = является изданием, публикуемым от имени конференции 
            //--
            //-- Индикатор юбилейного издания 
            //--  0 = не является юбилейным изданием
            //--  1 = является юбилейным изданием 
            //--
            //-- Индикатор указателя 
            //--  0 = указатель отсутствует
            //--  1 = указатель имеется 
            //-- Код литературного жанра 
            //--  a = художественная литература 
            //--  b = драма. Включая телевизионные пьесы, сценарии и т.д. 
            //--  с = очерки, эссе 
            //--  d = юмор, сатира Но не карикатура и т.д. 
            //--  е = письма как литературная форма 
            //--  f = короткие рассказы 
            //--  g = поэтические произведения, включая нелитературные работы в стихах 
            //--  h = речи и другие риторические формы 
            //--  i = либретто 
            //--  y = нелитературный текст 
            //--  z = смешанные и другие литературные формы 
            //--
            //-- Код биографии 
            //--  a = автобиография, в том числе письма, переписка. 
            //--  b = биография отдельного лица 
            //--  с = коллективная биография, например, биография семьи. 
            //--  d = сборник биографической информации 
            //--     Например, справочник <Кто есть кто>. 
            //--  y = не биография 
            //--
            //--$9 Код ступени высшего профессионального образования 
            //--  Двухсимвольный код ступени высшего профессионального образования
            //--   (ступени профессионального образования приведены в соответствии с
            //--   законом о высшем образовании РФ).
            //--  Подполе используется при описании специфических видов документов
            //--   (диссертации, авторефераты, дипломные работы, работы в форме научного
            //--   доклада), для указания научной квалификации, на соискание которой
            //--   выдвинута описываемая работа. 
            //--   aa = высшая школа - неполное высшее образование
            //--   ab = высшая школа - бакалавр
            //--   ac = высшая школа - специалист
            //--   ad = высшая школа - магистр
            //--   au = высшая школа - неизвестно
            //--   ba = аспирантура - кандидат наук
            //--   ca = докторантура - доктор наук
            //--   zz = другое 
            R = " SELECT  POL FROM TECHNOLOG_VVV..RUSM "
               + " WHERE IDMAIN = " + IDM.ToString() + " AND MET = " + METU.ToString()
        + " and session='" + IDSession + "'";
            da.SelectCommand = new SqlCommand();
            da.SelectCommand.CommandText = R;
            da.SelectCommand.Connection = conbase01;
            da.SelectCommand.CommandTimeout = 1200;
            ds = new DataSet();
            da.Fill(ds, "list0");
            POL = ds.Tables["list0"].Rows[0][0].ToString();
            //-- Модификация поля 105
            P4 = POL.Substring(0, 4);
            switch (DSORT)
            {
                case "Издание картографическое":
                    P4 = OBR1_P4("b", P4);
                    break; // Издание картографическое
                case "Издание музыкальное":
                    P4 = OBR1_P4("g", P4);
                    break; // 
                case "Издание листовое":
                    P4 = OBR1_P4("f", P4);
                    break; // Издание листовое
            }
            P4.PadRight(4);
            if (P4 != POL.Substring(0, 4))
            { // поз. 0-3 поля POL изменились
                POL = P4 + POL.Substring(5);
                R = "UPDATE TECHNOLOG_VVV..RUSM set POL=N'" + POL + "' WHERE MET=105 AND IDMAIN=" + IDM.ToString()
        + " and session='" + IDSession + "'";
                command = new SqlCommand(R, conbase01);
                conbase01.Open();
                command.CommandTimeout = 1200;
                command.ExecuteNonQuery();
                conbase01.Close();
            }
            //-- Модификация поля 105 поз. 4-7
            P4 = POL.Substring(4, 4);
            switch (DSORT)
            {
                case "Издание официальное":
                    P4 = OBR1_P4("n", P4);
                    break;
                case "Издание справочное":
                    P4 = OBR1_P4("g", P4);
                    break;
                case "Каталог":
                    P4 = OBR1_P4("b", P4);
                    break;
                case "Словарь":
                    P4 = OBR1_P4("e", P4);
                    break;
                case "Издание учебное":
                    P4 = OBR1_P4("j", P4);
                    break;
                case "Учебник":
                    P4 = OBR1_P4("j", P4);
                    break;
                case "Пособие учебно-методическое":
                    P4 = OBR1_P4("j", P4);
                    break;
                case "Энциклопедия":
                    P4 = OBR1_P4("f", P4);
                    break;
            }
            P4.PadRight(4);
            if (P4 != POL.Substring(4, 4))
            { // поз. 4-7 поля POL изменились
                POL = POL.Substring(0, 4) + P4 + POL.Substring(8, POL.Length - 8);
                R = "UPDATE TECHNOLOG_VVV..RUSM set POL=N'" + POL + "' WHERE MET=105 AND IDMAIN=" + IDM.ToString()
        + " and session='" + IDSession + "'";
                command = new SqlCommand(R, conbase01);
                conbase01.Open();
                command.CommandTimeout = 1200;
                command.ExecuteNonQuery();
                conbase01.Close();
            }
        } // IDL > 0 || IDL == -100 || IDL == -2 || IDL == -4
        else
        {
            //-- - 3  - СЕРИЯ. СВОДНЫЙ УРОВЕНЬ
            //-- -33  - ПОДСЕРИЯ. СВОДНЫЙ УРОВЕНЬ
            //-- - 5  - ПРОДОЛЖАЮЩЕЕСЯ ИЗДАНИЕ. СВОДНЫЙ УРОВЕНЬ
            METU = 110;
            R = " SELECT POL FROM TECHNOLOG_VVV..RUSM "
             + " WHERE IDMAIN= " + IDM.ToString() + " AND MET= " + METU.ToString()
    + " and session = '" + IDSession + "'";
            da.SelectCommand = new SqlCommand();
            da.SelectCommand.CommandText = R;
            da.SelectCommand.Connection = conbase01;
            da.SelectCommand.CommandTimeout = 1200;
            ds = new DataSet();
            da.Fill(ds, "list0");
            POL = ds.Tables["list0"].Rows[0][0].ToString();
            //--110 ПОЛЕ КОДИРОВАННЫХ ДАННЫХ: ПРОДОЛЖАЮЩИЕСЯ РЕСУРСЫ 
            //-- Обязательное в записи высшего уровня, составляемой для описания
            //--   продолжающегося ресурса в целом.
            //-- Примечание: Указание на содержание отдельных выпусков продолжающегося
            //--  ресурса может (но не является обязательным) размещаться в позициях
            //--  110$a/4-6 записи уровня физической единицы, описывающей отдельный выпуск.
            //-- Допускается также использование в записи уровня физической единицы
            //--  позиции 110$a/7, если отдельный/отдельные выпуски содержат материалы
            //--  конференции, однако, отражение этих материалов - не главная функция
            //--  продолжающегося ресурса в целом (позиция $a/7 записи высшего уровня
            //--  содержит 0). 
            //--$a Кодированные данные продолжающегося ресурса 
            //-- Все данные, записываемые в подполе $a, идентифицируются позицией символа
            //--  в подполе.
            //-- Позиции символов нумеруются от 0 до 10. Все позиции символов должны
            //--  быть представлены в подполе.
            //-- Наименование элемента данных               Количество символов Позиции символов 
            //-- Определитель вида продолжающегося ресурса  1                     0 
            //-- Периодичность издания                      1                     1 
            //-- Регулярность                               1                     2 
            //-- Код вида материала                         1                     3 
            //-- Код характера содержания                   3                    4-6 
            //-- Индикатор материалов конференции           1                     7 
            //-- Код наличия титульного листа               1                     8 
            //-- Код наличия указателя                      1                     9 
            //-- Код наличия сводного указателя             1                     10 
            //--
            //--  Определитель вида продолжающегося ресурса 
            //--   Односимвольный код, указывающий на вид продолжающегося ресурса.
            //--    Заполнение обязательно, если поле 110 приводится в записи. 
            //--  a = периодическое издание 
            //--    Сериальное издание, выходящее через определенные промежутки времени,
            //--     как правило, постоянным для каждого года числом номеров (выпусков),
            //--     не повторяющимися по содержанию, однотипно оформленными нумерованными
            //--     и (или) датированными выпусками, имеющими одинаковое название 
            //--    (ГОСТ 7.60-2003). 
            //--  b = монографическая серия 
            //--    Совокупность томов, объединенных общностью замысла, тематики, целевым
            //--     или читательским назначением. 
            //--  с = газета 
            //--  е = интегрируемое издание со сменными листами 
            //--  f = база данных 
            //--    Коллекция логически взаимосвязанных данных, собранных вместе в виде
            //--     одного или нескольких компьютерных файлов, как правило, создаваемая и управляемая с помощью системы управления базой данных 
            //--  g = обновляемый веб-сайт 
            //--  z = другие 
            //--
            //-- Периодичность издания 
            //--  Заполнение обязательное для периодического издания. 
            //--   a = ежедневно
            //--   b = дважды в неделю
            //--   с = еженедельно
            //--   d = раз в две недели
            //--   е = дважды в месяц
            //--   f = ежемесячно
            //--   g = раз в два месяца
            //--   h = ежеквартально
            //--   i = три раза в год
            //--   j = дважды в год
            //--   k = ежегодно
            //--   l = раз в два года
            //--   m = раз в три года
            //--   n = три раза в неделю
            //--   o = три раза в месяц
            //--   p = обновляется постоянно
            //--   u = неизвестно
            //--   y = периодичность не определена (т.е. нерегулярно); см. также поз. симв. 2 
            //--   z = другая  
            //--
            //-- Регулярность 
            //--  a = регулярный 
            //--  b = нормализованный нерегулярный 
            //--   Продолжающийся ресурс не является полностью регулярным, но
            //--    нерегулярность его заранее определена. Например, ежемесячное издание,
            //--    за исключением июля-августа. 
            //--  u = не известно 
            //--  у = нерегулярный 
            //--
            //-- Код вида материала 
            //--  а = библиографическое издание 
            //--  b = каталог 
            //--  c = указатель 
            //--  d = реферат или резюме, включая описательные, справочные и информационные рефераты 
            //--  e = словарь 
            //--  f = энциклопедия 
            //--  g = справочное издание 
            //--  h = ежегодный отчет / ежегодный обзор 
            //--  i = статистический отчет / обзор 
            //--  j = учебник 
            //--  k = обзор 
            //--    Продолжающийся ресурс, содержащий обзор, например, книг, кинофильмов и т.д.
            //--     (но не обзоры событий и не статистические обзоры). 
            //--  l = законодательные акты 
            //--  m = судебные отчеты и сборники выдержек из решений судов 
            //--  n = юридические статьи 
            //--  o = случаи из судебной практики и их комментарии 
            //--  p = биографии. Например, справочник "Who is who". 
            //--  r = текущие периодические обзоры 
            //--  t = карикатуры или комиксы 
            //--  z = другой тип содержания 
            //--
            //-- Код характера содержания 
            //--  Трехсимвольный код, показывающий, содержит ли данный продолжающийся
            //--   ресурс материалы одного или более типов из тех, что перечислены для
            //--   позиции $a/3. Необходимо отличать от характеристики самого
            //--   продолжающегося ресурса, размещаемой в позиции $a/3.
            //--  Может быть указано до трех кодов (выравниваемых влево); неиспользуемые
            //--   позиции заполняются пробелами (#). Если для описания ресурса могут быть
            //--   применены более трех кодов (т.е. присутствуют более трех типов материалов),
            //--   следует выбрать первые три кода в том порядке, в котором они приведены
            //--   в списке.
            //--  Если элемент данных не используется, позиции символов 0-3 заполняются
            //--   символами-заполнителями ' | '. 

            //-- Индикатор материалов конференции 
            //--  Односимвольный код, указывающий, что продолжающийся ресурс содержит
            //--   труды, отчеты или краткое описание работы конференции, совещания,
            //--   семинара, или симпозиума. Например, труды ежегодной конференции. 
            //-- 0 = не является изданием, публикуемым от имени конференции
            //-- 1 = является изданием, публикуемым от имени конференции 
            //--
            //-- Код доступности титульного листа 
            //--  а = титульный лист в последнем выпуске тома - не прикреплен
            //--  b = титульный лист в последнем выпуске тома - прикреплен
            //--  c = титульный лист в первом выпуске следующего тома - не прикреплен
            //--  d = титульный лист в первом выпуске следующего тома - прикреплен
            //--  e = титульный лист публикуется отдельно - свободно по заказу
            //--  f = титульный лист публикуется отдельно - свободно, высылается автоматически
            //--  g = титульный лист публикуется отдельно - покупается - по заказу 
            //--  u = не известно ко времени составления записи
            //--  y = титульный лист не издается
            //--  z = другое 
            //--
            //-- Код доступности указателя 
            //--  a = каждый выпуск содержит указатель к содержанию - не прикреплен
            //--  b = указатель к содержанию - в последнем выпуске тома - не прикреплен - с самостоятельной нумерацией страниц
            //--  c = указатель к содержанию - в последнем выпуске тома - с ненумерованными страницами
            //--  d = указатель к содержанию - в последнем выпуске тома - прикреплен
            //--  е = указатель к содержанию - в первом выпуске следующего тома - не прикреплен - с самостоятельной нумерацией страниц
            //--  f = указатель к содержанию - в первом выпуске следующего тома - не прикреплен - с ненумерованными страницами
            //--  g = указатель к содержанию - в первом выпуске следующего тома - прикреплен
            //--  h = публикуется отдельно - свободно - рассылается автоматически
            //--  i = публикуется отдельно - свободно по запросу
            //--  j = публикуется отдельно - переплет от издателя - свободно - рассылается автоматически
            //--  k = публикуется отдельно - переплет от издателя - свободно по запросу
            //--  l = публикуется отдельно - переплет от издателя - продается по запросу
            //--  m = указатель является сериальным изданием и издается в качестве приложения или подсерии основного сериального издания.
            //--  u = не известно к моменту составления записи
            //--  y = указатель отсутствует
            //--  z = другое 
            //--
            //-- Код наличия сводного указателя 
            //--  0 = нет сводного указателя или оглавления
            //--  1 = имеется сводный указатель или оглавление 
            //--
            //-- Модификация поля 110
            P4 = POL.Substring(3, 4);
            switch (DSORT)
            {
                case "Энциклопедия":
                    P4 = OBR1_P4("f", P4);
                    break;
                //-- Код вида материала 
                //--  b = каталог 
                //--  e = словарь 
                //--  f = энциклопедия 
                //--  g = справочное издание 
                //--  j = учебник 
                case "Издание справочное":
                    P4 = OBR1_P4("g", P4);
                    break;
                case "Каталог":
                    P4 = OBR1_P4("b", P4);
                    break;
                case "Словарь":
                    P4 = OBR1_P4("e", P4);
                    break;
                case "Учебник":
                    P4 = OBR1_P4("j", P4);
                    break;
            } // switch
            P4.PadRight(4);
            //     if (P4.Equals(POL.Substring(4, 4)) == false) {
            // поз. 4-7 поля POL изменились
            if (P4 != POL.Substring(4, 4))
            {  //  поз. 4-7 поля POL изменились
                POL = POL.Substring(0, 4) + P4 + POL.Substring(8);
                R = "UPDATE TECHNOLOG_VVV..RUSM set POL=N'" + POL + "' WHERE MET=110 AND IDMAIN=" + IDM.ToString()
        + " and session='" + IDSession + "'";
                command = new SqlCommand(R, conbase01);
                conbase01.Open();
                command.CommandTimeout = 1200;
                command.ExecuteNonQuery();
                conbase01.Close();
            } // IDL == -3 || IDL == -33 || IDL == -5
        }
    }// OBR1 - Вид обработки = 1 - Вид издания Поле BJ921b
    //
    //-- =============================================
    //-- Description:	Обработка P4
    //-- =============================================
    private String OBR1_P4(String C, String P)
    {
        Int32 KK;
        String ppol4_7;
        //if (P.Substring(0, 1) == "y") P = C;
        //else
        //{
        //ppol4_7 = P.Substring(4, 4);
        ppol4_7 = P;
        KK = ppol4_7.IndexOf(' ');
        if (KK != -1)
        {
            switch (KK)
            {
                case 0: ppol4_7 = C + "   ";
                    break;
                case 1: ppol4_7 = ppol4_7.Substring(0, 1) + C + "  ";
                    break;
                case 2: ppol4_7 = ppol4_7.Substring(0, 2) + C + " ";
                    break;
                case 3: ppol4_7 = ppol4_7.Substring(0, 3) + C;
                    break;
            }
            //P = P.Substring(0, 4) + ppol4_7 + P.Substring(8);
            P = ppol4_7;
        }
        //}
        return P;
    }
    //
    //-- =============================================
    //-- Description:	Вид обработки = 3 - Иллюстрации
    //-- =============================================
    private void OBR3(String IDSession, Int32 IDM, Int32 DIDDATA, Int16 MET, String IND1,
                            String IND2, String IDENT, String DSORT)
    {
        String POL;
        Int32 KK;
        R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
        + " VALUES ('" + IDSession + "'," + IDM.ToString() + "," + DIDDATA.ToString() + "," + MET.ToString()
        + ",'" + IND1 + "','" + IND2 + "','" + IDENT + "',N'" + DSORT + "') ";
        command = new SqlCommand(R, conbase01);
        conbase01.Open();
        command.CommandTimeout = 1200;
        command.ExecuteNonQuery();
        conbase01.Close();
        //-- Если есть поле 105, ОНО КОРРЕКТИРУЕТСЯ
        R = " SELECT  POL FROM TECHNOLOG_VVV..RUSM "
          + " WHERE MET=105 AND IDBLOCK = " + DIDDATA.ToString()
    + " and session='" + IDSession + "'";
        da.SelectCommand = new SqlCommand();
        da.SelectCommand.CommandText = R;
        da.SelectCommand.Connection = conbase01;
        da.SelectCommand.CommandTimeout = 1200;
        ds = new DataSet();
        KK = da.Fill(ds, "list0");
        if (KK > 0)
        {
            POL = ds.Tables["list0"].Rows[0][0].ToString();
            //-- Коррекция ПОЛЯ 105a позиции 0-3
            //--$a / (позиции символов 0-3) Коды иллюстраций 
            //--Для кодов иллюстраций отводится четыре позиции символов, которые заполняются слева направо. Неиспользуемые позиции заполняются пробелами (#). Если для описания документа могут быть применены более четырех кодов (т.е. присутствуют более четырех типов иллюстраций), следует выбрать первые четыре в том порядке, в котором они приведены в списке ниже.
            //--Если элемент данных не используется, позиции символов 0-3 заполняются символами-заполнителями ' | '.
            //   Если элемент данных используется, но присутствует менее четырех типов иллюстраций, оставшиеся позиции заполняются символами пробела: '#'. 
            //--
            //--  a = иллюстрации 
            //--Используется для типов иллюстраций, которые не включены в перечень,
            //-- приведенный ниже, например диаграммы, графики или типы иллюстраций,
            //-- не кодируемые отдельно. 
            //--
            if (POL.Substring(0, 1) == "y")
            {
                POL = "a";
                R = " UPDATE TECHNOLOG_VVV..RUSM SET POL=N'" + POL.PadRight(4)
                    + "' WHERE MET=105 AND IDBLOCK=" + DIDDATA.ToString()
        + " and session='" + IDSession + "'";
                command = new SqlCommand(R, conbase01);
                conbase01.Open();
                command.CommandTimeout = 1200;
                command.ExecuteNonQuery();
                conbase01.Close();
            }
        }
    }
    //
    //-- =============================================
    //-- Description:	Вид обработки = 5 - поля описания старопечатных изданий
    //-- =============================================
    //private void OBR5(String IDSession,Int32 IDM, Int32 MNF, String MSF, Int32 DIDDATA, Int16 MET,
    //    String IND1, String IND2, String IDENT, String DSORT)
    //{
    //    String PREF;
    //    R = " SELECT  [NAME] FROM BJRUSMARC..VVV2LIBNET "
    //      + " WHERE MNF= " + MNF.ToString() + "AND MSF='" + MSF + "'";
    //    da.SelectCommand = new SqlCommand();
    //    da.SelectCommand.CommandText = R;
    //    da.SelectCommand.Connection = con;
    //    da.SelectCommand.CommandTimeout = 1200;
    //    ds = new DataSet();
    //    da.Fill(ds, "list0");
    //    PREF = ds.Tables["list0"].Rows[0][0].ToString();
    //    R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
    //    + " VALUES ('"+IDSession+"'," + IDM.ToString() + "," + DIDDATA.ToString() + "," + MET.ToString()
    //    + ",'" + IND1 + "',' " + IND2 + "','" + IDENT + "','" + PREF + "' +': '+ '" + DSORT + "') ";
    //    command = new SqlCommand(R, con);
    //    con.Open();
    //    command.CommandTimeout = 1200;
    //    command.ExecuteNonQuery();
    //    con.Close();
    //}
    //
    //-- =============================================
    //-- Description:	Вид обработки = 6 -  Фамилии
    //-- =============================================
    private void OBR6(String IDSession, Int32 IDM, Int32 DIDDATA, Int16 MET, String IND1, String IND2,
        String IDENT, String DPPLAIN)
    // 2011.04.12 11:20
    {
        Int32 K, KK, KK2, K1, K2;
        String POL, T1;
        //-- 700 a - Фамилия первого автора
        //  -- Формирование точки доступа
        // Anthony (Bloom, A.B.; 1914-2003)
        //Chukovskaya, L.K.
        POL = "";
        K = DPPLAIN.IndexOf(',');
        K2 = DPPLAIN.IndexOf('(');
        if (K != -1) // есть запятая
        {
            if (K2 != -1) // (
            {
                if (K2 < K) // До ( нет ,
                {
                    //    -- 700a - Начальный элемент ввода - до запято
                    POL = DPPLAIN.Substring(K2 + 1).TrimEnd(' '); // после ( 
                    T1 = "";
                    DPPLAIN = DPPLAIN.Substring(0, K2).TrimEnd(' ');
                }
                else
                { // , до (
                    T1 = DPPLAIN.Substring(K + 1, K2 - K - 1); // от , до (
                    POL = DPPLAIN.Substring(K2 + 1).TrimStart(' '); // после (
                    DPPLAIN = DPPLAIN.Substring(0, K).TrimEnd(' '); // до запятой
                }
            }
            else  // нет ( после запятой
            {
                T1 = DPPLAIN.Substring(K + 1); // после запятой
                DPPLAIN = DPPLAIN.Substring(0, K).TrimEnd(' '); // до запятой
                POL = "";
            }
            T1 = T1.TrimStart(' ');
            T1 = T1.TrimEnd(' ');
            if (T1.Length > 0)
            {
                //    -- 700b Часть имени, кроме начального элемента ввода 
                R = "INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
                + " VALUES ('" + IDSession + "'," + IDM.ToString() + "," + DIDDATA.ToString() + "," + MET.ToString()
                + ",' ','1','b',N'" + T1 + "') ";
                command = new SqlCommand(R, conbase01);
                conbase01.Open();
                command.CommandTimeout = 1200;
                command.ExecuteNonQuery();
                conbase01.Close();
            }
        }  // END -- Есть запятая
        else  // нет запятой
        {
            //    -- 700a - Начальный элемент ввода - до запятой ИЛИ до (
            if (K2 != -1) // есть (
            {
                POL = DPPLAIN.Substring(K2 + 1).TrimStart(' '); // после (
                DPPLAIN = DPPLAIN.Substring(0, K2).TrimEnd(' ');
            } // Нет , есть (
        } //  нет запятой
        R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
        + " VALUES ('" + IDSession + "'," + IDM.ToString() + "," + DIDDATA.ToString() + "," + MET.ToString()
        + ",' ','1','a',N'" + DPPLAIN + "') ";
        command = new SqlCommand(R, conbase01);
        conbase01.Open();
        command.CommandTimeout = 1200;
        command.ExecuteNonQuery();
        conbase01.Close();
        while (POL.Length > 0)
        {
            T1 = "";
            K1 = POL.IndexOf(")");
            if ((K1 != -1) && ((K1 + 1) < POL.Length))
            {
                T1 = POL.Substring(K1 + 1).TrimStart(' '); // Остаток после )
                POL = POL.Substring(0, K1).TrimEnd(' ');
            }
            POL = POL.TrimEnd(')');
            while ((K1 = POL.IndexOf(";")) != -1)
            {
                String T2;
                T2 = POL.Substring(0, K1).TrimEnd(' ');
                POL = POL.Substring(K1 + 1).TrimStart(' ');
                if (testAFNAME(T2) == 1) // ДАТЫ
                {
                    //    -- 700f Даты
                    R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
                    + " VALUES ('" + IDSession + "'," + IDM.ToString() + "," + DIDDATA + "," + MET + ",' ','1','f',N'" + T2 + "') ";
                    command = new SqlCommand(R, conbase01);
                    conbase01.Open();
                    command.CommandTimeout = 1200;
                    command.ExecuteNonQuery();
                    conbase01.Close();
                }
                else
                {
                    //    -- 700c Дополнение к именам, кроме дат
                    R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
                    + " VALUES ('" + IDSession + "'," + IDM.ToString() + "," + DIDDATA + "," + MET + ",' ','1','c',N'" + T2 + "') ";
                    command = new SqlCommand(R, conbase01);
                    conbase01.Open();
                    command.CommandTimeout = 1200;
                    command.ExecuteNonQuery();
                    conbase01.Close();
                }
            }
            POL = POL.TrimStart(' ');
            POL = POL.TrimEnd(' ');
            POL = POL.TrimEnd(')');
            if (testAFNAME(POL) == 1) // ДАТЫ
            {
                //    -- 700f Даты
                R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
                + " VALUES ('" + IDSession + "'," + IDM.ToString() + "," + DIDDATA + "," + MET + ",' ','1','f',N'" + POL + "') ";
                command = new SqlCommand(R, conbase01);
                conbase01.Open();
                command.CommandTimeout = 1200;
                command.ExecuteNonQuery();
                conbase01.Close();
            }
            else
            {
                //    -- 700c Дополнение к именам, кроме дат
                R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
                + " VALUES ('" + IDSession + "'," + IDM.ToString() + "," + DIDDATA + "," + MET + ",' ','1','c',N'" + POL + "') ";
                command = new SqlCommand(R, conbase01);
                conbase01.Open();
                command.CommandTimeout = 1200;
                command.ExecuteNonQuery();
                conbase01.Close();
            }
            POL = T1;
        } // while pol.Length > 0

        //    -- 700d Римские цифры
        //    -- 700g Расширение инициалов личного имени
        //-- РОЛЬ
        R = " SELECT  PLAIN  "
          + " FROM " + BAZA + "..DATAEXTPLAIN P "
        + " LEFT JOIN " + BAZA + "..DATAEXT DE ON P.IDDATAEXT=DE.ID "
          + " WHERE P.IDDATA= " + DIDDATA.ToString() + " AND MSFIELD = '$4' ";
        da.SelectCommand = new SqlCommand();
        da.SelectCommand.CommandText = R;
        da.SelectCommand.Connection = conbase03;
        da.SelectCommand.CommandTimeout = 1200;
        ds = new DataSet();
        KK = da.Fill(ds, "list0");
        if (KK == 0)
        {
            if (MET == 702)
            {
                R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
                + " VALUES ('" + IDSession + "'," + IDM.ToString() + "," + DIDDATA.ToString()
                + ",702,' ','1','Ю',N'340') "; // Редактор
                command = new SqlCommand(R, conbase01);
                conbase01.Open();
                command.CommandTimeout = 1200;
                command.ExecuteNonQuery();
                conbase01.Close();
            }
        }
        else
        {
            T1 = ds.Tables["list0"].Rows[0][0].ToString().Replace("\n", "").Replace("\r", "");
            R = " SELECT  KOD FROM BJRUSMARC..ROLI R "
                + " LEFT JOIN " + BAZA + "..LIST_3 L3 ON L3.NAME=R.NAME "
              + " WHERE SHORTNAME= '" + T1 + "'";
            da.SelectCommand = new SqlCommand();
            da.SelectCommand.CommandText = R;
            da.SelectCommand.Connection = conbase03;
            da.SelectCommand.CommandTimeout = 1200;
            //if (IDM == 1392592)
            //{
            //    MessageBox.Show(IDM.ToString());
            //}
            ds = new DataSet();
            KK2 = da.Fill(ds, "list7003");
            if (KK2 == 0)
            {
                String ROL = (MET != 700) ? "340" : "070";
                if (ROL.Length == 0)
                {
                    Console.Write(IDM.ToString());
                }
                R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
                + " VALUES ('" + IDSession + "'," + IDM.ToString() + "," + DIDDATA + "," + MET + ",' ','1','Ю',N'" + ROL + "') "; // Редактор
                command = new SqlCommand(R, conbase01);
                conbase01.Open();
                command.CommandTimeout = 1200;
                command.ExecuteNonQuery();
                conbase01.Close();
            }
            else
            {
                String TR = ds.Tables["list7003"].Rows[0][0].ToString();
                TR = TR.Replace("'", "~");
                if (TR.Length == 0)
                {
                    Console.Write(IDM.ToString());
                }

                R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
                + " VALUES ('" + IDSession + "'," + IDM.ToString() + "," + DIDDATA + "," + MET + ",' ','1','Ю',N'" + TR + "') ";
                command = new SqlCommand(R, conbase01);
                conbase01.Open();
                command.CommandTimeout = 1200;
                command.ExecuteNonQuery();
                conbase01.Close();
            }
            ds.Dispose();
        }
    } // OBR6 - Фамилии
    //
    //-- =============================================
    //-- Description:	Вид обработки = 9 - Сводный уровень
    //--     225 = Заглавие сводного уровня
    //-- =============================================
    private void OBR9(String IDSession, Int32 IDM, Int32 DIDDATA, Int16 MET, String IND1,
        String IND2, String IDENT, String DSORT)
    {
        Int32 KCV, KK, KK2;
        String Inst, ZAGL;
        KCV = Int32.Parse(DSORT);
        //-- Заглавие сводного уровня по ссылке=IDMAIN серии
        // Поле содержит заглавие серии вместе с любыми сведениями, относящимися к этому заглавию, и сведениями об ответственности,
        //     относящимися к заглавию серии, включая любые предшествующие повторения на других языках в той форме и последовательности,
        //     в которой они приведены в источнике информации. Соответствует области серии ISBD и ГОСТ 7.1-2003.
        //Обязательное, если данные имеются на издании. В противном случае - не используется.
        //Повторяется, если документ входит более чем одну серию.
        //Индикатор 1: Индикатор формы заглавия
        //    0 – Форма заглавия серии отличается от установленной
        //    1 – Нет установленной формы
        //    2 – Совпадает с установленной формой
        //Индикатор 2: # (не определен)

        //Подполя
        //$a	Основное заглавие серии
        //Заглавие серии в той форме, в которой оно представлено в источнике информации.
        //Обязательное, если поле 225 приводится в записи.
        //Не повторяется.

        //$d	Параллельное заглавие серии
        //Заглавие серии на другом языке и/или в другой графике, чем то, что приведено в подполе 225$a 
        //Обязательное, если параллельное заглавие серии представлено в источнике информации. Повторяется 
        //      для каждого параллельного заглавия серии.

        //$e	Сведения, относящиеся к заглавию
        //Сведения, относящиеся к заглавию серии в $a или в $d или к наименованию части ($i), в той форме,
        //      как они представлены в источнике информации.
        //Обязательное, если данные приведены в источнике информации.
        //Повторяется для каждых последующих сведений, относящихся к заглавию, а также для параллельных сведений, относящихся к заглавию.

        //$f	Сведения об ответственности
        //Сведения об ответственности для заглавия, представленного в $a (пример 3-4) или $d, а также для нумерованной
        //      или наименованной части внутри серии, представленной в $h или $i.
        //Обязательное, если данные приведены в источнике информации.
        //Повторяется для каждых последующих сведений об ответственности, для параллельных сведений об ответственности 
        //      и для сведений об ответственности, относящихся к подсериям.

        //$h	Обозначение или номер части
        //Содержит цифровое или буквенное обозначение раздела или части серии в $a.
        //Обязательное, если данные приведены в источнике информации.
        //Повторяется для каждого раздела или деления более низкого уровня или для номера параллельной части.

        //$i	Наименование части
        //Наименование раздела или части серии, когда серия, записанная в $a, делится на подсерии.
        //Обязательное, если данные приведены в источнике информации.
        //Повторяется при делении подсерий на более низкие уровни или для параллельных наименований подсерии.

        //$v	Обозначение тома
        //Номер каталогизируемого документа внутри серии, записанной в подполе 225$a.
        //Обязательное, если данные приведены в источнике информации.
        //Повторяется.

        //$x	ISSN серии
        //Обязательное, если данные приведены в источнике информации.
        //Повторяется.

        //$z	Язык параллельного заглавия
        //Кодированное обозначение языка параллельного заглавия, записанного в 225$d.
        //Обязательное, если приводится 225$d.
        //Повторяется, если повторяется 225$d.
        R = " SELECT PLAIN FROM " + BAZA + "..DATAEXTPLAIN P "
        + " LEFT JOIN " + BAZA + "..DATAEXT DE ON DE.ID = P.IDDATAEXT "
        + " WHERE MNFIELD=200 AND MSFIELD='$a' AND P.IDMAIN=" + DSORT;
        da.SelectCommand = new SqlCommand();
        da.SelectCommand.CommandText = R;
        da.SelectCommand.Connection = conbase03;
        da.SelectCommand.CommandTimeout = 1200;
        DataSet ds9 = new DataSet();
        Int32 K9 = da.Fill(ds9, "list9");
        if (K9 > 0)
        {
            ZAGL = ds9.Tables["list9"].Rows[0][0].ToString(); // PLAIN cv.urovenj
            ZAGL = ZAGL.Replace("'", "~").Replace("\n", "").Replace("\r", "");
            //-- Запись Заглавия сводного уровня в 225a
            R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
            + " VALUES ('" + IDSession + "'," + IDM.ToString() + "," + DIDDATA.ToString() + ",225,'1',' ','a',N'" + ZAGL + "') ";
            command = new SqlCommand(R, conbase01);
            conbase01.Open();
            command.CommandTimeout = 1200;
            command.ExecuteNonQuery();
            conbase01.Close();
            ////461  #0$1001RU\NLR\bibl\98765$12001#$vт. 6 
            //p461 = "001" + PREFIX001 + DSORT.PadLeft(8, '0')
            //    + (char)31 + "12001 " + (char)31 + "a" + ZAGL + (char)31 + "v" + Inst;
            String p461 = "2001 " + (char)31 + "a" + ZAGL;
            //R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
            //    + " VALUES ('" + IDSession + "'," + IDM.ToString() + "," + DIDDATA.ToString() + ",461,' ','0'"
            //    + ",'1',N'" + p461 + "') "; 
            ds9.Dispose();

            R = " SELECT PLAIN FROM " + BAZA + "..DATAEXTPLAIN P "
              + " LEFT JOIN " + BAZA + "..DATAEXT DE ON DE.ID = P.IDDATAEXT "
              + " WHERE MNFIELD=225 AND MSFIELD='$h' AND P.IDDATA=" + DIDDATA.ToString(); // Номер ТОМА
            da.SelectCommand = new SqlCommand();
            da.SelectCommand.CommandText = R;
            da.SelectCommand.Connection = conbase03;
            da.SelectCommand.CommandTimeout = 1200;
            ds9 = new DataSet();
            KK = da.Fill(ds9, "list90");
            if (KK > 0)
            {
                Inst = ds9.Tables["list90"].Rows[0][0].ToString();
                Inst = Inst.Replace("'", "~").Replace("\n", "").Replace("\r", "");

                // R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
                //+ " VALUES ('" + IDSession + "'," + IDM.ToString() + "," + DIDDATA.ToString() + ",225,'1',' ','h',N'" + Inst + "') ";
                // command = new SqlCommand(R, con);
                // con.Open();
                // command.CommandTimeout = 1200;
                // command.ExecuteNonQuery();
                // con.Close();
                p461 = p461 + (char)31 + "v" + Inst;
            }
            ds9.Dispose();

            R = " SELECT  PLAIN FROM " + BAZA + "..DATAEXTPLAIN P "
              + " LEFT JOIN " + BAZA + "..DATAEXT DE ON DE.ID = P.IDDATAEXT "
              + " WHERE MNFIELD=225 AND MSFIELD='$v' AND P.IDDATA=" + DIDDATA.ToString();
            da.SelectCommand = new SqlCommand();
            da.SelectCommand.CommandText = R;
            da.SelectCommand.Connection = conbase03;
            da.SelectCommand.CommandTimeout = 1200;
            ds9 = new DataSet();
            KK2 = da.Fill(ds9, "list92");
            if (KK2 > 0)
            {
                Inst = ds9.Tables["list92"].Rows[0][0].ToString();
                Inst = Inst.Replace("'", "~").Replace("\n", "").Replace("\r", "");
                //461  #0$1001RU\NLR\bibl\98765$12001#$vт. 6 
                p461 = p461 + (char)31 + "v" + Inst;
            }
            ds9.Dispose();

            R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
                + " VALUES ('" + IDSession + "'," + IDM.ToString() + "," + DIDDATA.ToString() + ",461,' ','0','1',N'" + p461 + "') ";
            command = new SqlCommand(R, conbase01);
            conbase01.Open();
            command.CommandTimeout = 1200;
            command.ExecuteNonQuery();
            conbase01.Close();

            R = " SELECT IDLEVEL FROM " + BAZA + "..MAIN WHERE ID=" + IDM;
            da.SelectCommand = new SqlCommand();
            da.SelectCommand.CommandText = R;
            da.SelectCommand.Connection = conbase03;
            da.SelectCommand.CommandTimeout = 1200;
            DataSet ds9level = new DataSet();
            da.Fill(ds9level);
            Int32 K9level = Int32.Parse(ds9level.Tables[0].Rows[0][0].ToString());
            ds9level.Dispose();
            if (K9level > 0)// IDLEVEL>0
            {
                R = " SELECT PLAIN FROM " + BAZA + "..DATAEXTPLAIN P "
              + " LEFT JOIN " + BAZA + "..DATAEXT DE ON DE.ID = P.IDDATAEXT "
              + " WHERE MNFIELD=11 AND MSFIELD='$a' AND P.IDMAIN=" + IDM;
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.CommandText = R;
                da.SelectCommand.Connection = conbase03;
                da.SelectCommand.CommandTimeout = 1200;
                DataSet ds9011a = new DataSet();
                Int32 K9011a = da.Fill(ds9011a);
                if (K9011a == 1) // есть один ISSN ? 011a
                {
                    String S11a = ds9011a.Tables[0].Rows[0][0].ToString().Replace("\n", "").Replace("\r", ""); // ISSN 011a
                    R = " SELECT IDDATA FROM " + BAZA + "..DATAEXT WHERE MNFIELD=225 AND MSFIELD='$a' AND IDMAIN=" + IDM;
                    da.SelectCommand = new SqlCommand();
                    da.SelectCommand.CommandText = R;
                    da.SelectCommand.Connection = conbase03;
                    da.SelectCommand.CommandTimeout = 1200;
                    DataSet ds225 = new DataSet();
                    Int32 K9225 = da.Fill(ds225);
                    if (K9225 == 1) // есть одно поле 225а
                    {
                        R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
                        + " VALUES ('" + IDSession + "'," + IDM.ToString() + "," + DIDDATA.ToString()
                        + ",225,'1',' ','x',N'" + S11a + "') ";
                        command = new SqlCommand(R, conbase01);
                        conbase01.Open();
                        command.CommandTimeout = 1200;
                        command.ExecuteNonQuery();
                        conbase01.Close();
                    } // есть одно поле 225а
                    ds225.Dispose();
                } // есть один ISSN 011a
                else
                {
                    R = " SELECT PLAIN FROM " + BAZA + "..DATAEXTPLAIN P "
                      + " LEFT JOIN " + BAZA + "..DATAEXT DE ON DE.ID = P.IDDATAEXT "
                      + " WHERE MNFIELD=11 AND MSFIELD='$z' AND P.IDMAIN=" + IDM;  // Есть Ошибочный ISSN?
                    da.SelectCommand = new SqlCommand();
                    da.SelectCommand.CommandText = R;
                    da.SelectCommand.Connection = conbase03;
                    da.SelectCommand.CommandTimeout = 1200;
                    DataSet ds9011z = new DataSet();
                    Int32 K911z = da.Fill(ds9011z);
                    if (K911z > 0)  // Есть Ошибочный ISSN
                    {
                        String S11z = "Ошибочный ISSN: " + ds9011z.Tables[0].Rows[0][0].ToString().Replace("\n", "").Replace("\r", ""); // ISSN 011z
                        R = " SELECT IDMAIN FROM TECHNOLOG_VVV..RUSM "
                      + " WHERE session = '" + IDSession + "' AND IDMAIN = " + IDM.ToString()
                          + " AND MET=301 AND POL=N'" + S11z + "'";  // Выведен Ошибочный ISSN?
                        da.SelectCommand = new SqlCommand();
                        da.SelectCommand.CommandText = R;
                        da.SelectCommand.Connection = conbase01;
                        da.SelectCommand.CommandTimeout = 1200;
                        DataSet ds9011z301 = new DataSet();
                        K911z = da.Fill(ds9011z301);
                        if (K911z == 0)  // Уже Есть Ошибочный ISSN
                        {
                            R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
                            + " VALUES ('" + IDSession + "'," + IDM.ToString() + "," + DIDDATA.ToString() + ",301,' ',' ','a'"
                            + ",N'" + S11z + "') "; // Примечания к идентификационным номерам
                            command = new SqlCommand(R, conbase01);
                            conbase01.Open();
                            command.CommandTimeout = 1200;
                            command.ExecuteNonQuery();
                            conbase01.Close();
                        } // Еще не выведено поле 301
                    } // Есть Ошибочный ISSN
                    ds9011z.Dispose();
                } // нет 011a
                ds9011a.Dispose();
            } // idlevel > 0
            ds9level.Dispose();
        } // Заглавие серии по ссылке в 225a
        ds9.Dispose();
    }
    //
    //-- =============================================
    //-- Description:	Вид обработки = OBR200 - Сведения об ответственности
    //--     Разделение поля на 2 - Первые и остальные          
    //-- =============================================
    private void OBR200(String IDSession, Int32 IDM, Int32 DIDDATA, Int16 MET, String IND1,
        String IND2, String IDENT, String DSORT)
    {
        // 2011.12.26
        //            200  ЗАГЛАВИЕ И СВЕДЕНИЯ ОБ ОТВЕТСТВЕННОСТИ 
        //Поле соответствует области заглавия и сведений об ответственности ГОСТ 7.1-2003.
        //Поле содержит в форме и последовательности, определяемой Правилами:
        //    основное заглавие; параллельные заглавия; сведения, относящиеся к
        //    заглавию; сведения об ответственности. 
        //Обязательное.
        //Не повторяется. 

        //    $f Первые сведения об ответственности
        //Содержит: Первые сведения об ответственности для заглавия, записанного в
        //    подполе $a или в подполе $i. 
        //Обязательное для непараллельных сведений, если таковые присутствуют.
        //Повторяется для параллельных сведений об ответственности и для сведений
        //    об ответственности, относящихся к зависимому заглавию, записанному в
        //    подполе $i.

        //    $g Последующие сведения об ответственности
        //Содержит: сведения об ответственности для заглавия, записанного в подполе $a,
        //      или для зависимого заглавия, записанного в подполе $i, следующие после
        //      первых сведений об ответственности. 
        //Факультативное.
        //Повторяется
        //
        // Правила каталогизации
        //     6.4. Первым сведениям об ответственности предшествует знак косая черта;
        //   последующие группы сведений отделяют друг от друга точкой с запятой.
        //   Однородные сведения внутри группы отделяют запятыми
        Int32 K200;
        String Inst;
        if (DSORT.StartsWith(";")) DSORT = DSORT.Substring(1);
        K200 = DSORT.IndexOf(';');
        if (K200 == DSORT.Length) K200 = -1;
        if (K200 != -1)
        {
            Inst = DSORT.Substring(K200 + 1);
            DSORT = DSORT.Remove(K200);
        }
        else Inst = "";
        R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
            + " VALUES ('" + IDSession + "'," + IDM.ToString() + "," + DIDDATA.ToString() + "," + MET.ToString()
            + ",'" + IND1 + "','" + IND2 + "','f',N'" + DSORT + "') ";
        command = new SqlCommand(R, conbase01);
        conbase01.Open();
        command.CommandTimeout = 1200;
        command.ExecuteNonQuery();
        conbase01.Close();
        if (Inst.Length != 0)
        {
            R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
                + " VALUES ('" + IDSession + "'," + IDM.ToString() + "," + DIDDATA.ToString() + "," + MET.ToString()
                + ",'" + IND1 + "','" + IND2 + "','g',N'" + Inst + "') ";
            command = new SqlCommand(R, conbase01);
            conbase01.Open();
            command.CommandTimeout = 1200;
            command.ExecuteNonQuery();
            conbase01.Close();
        }
    } // OBR200 - Сведения об ответственности
    //
    //-- =============================================
    //-- Description:	Вид обработки = OBR2050 - 205$b Дополнительные сведения об издании
    //--     Если в исходной записи нет 205a - замена на 205a        
    //-- =============================================
    private void OBR2050(String IDSession, Int32 IDM, Int32 DIDDATA, Int16 MET, String IND1,
        String IND2, String IDENT, String DSORT)
    {
        // 2015.04.09
        //            205  Сведения об издании
        //Содержит сведения об отличиях данного издания от других изданий того же произведения
        //- сведения об издании документа;
        //- дополнительные сведения об издании;
        //- сведения об ответственности, относящиеся к изданию  
        //Обязательное, если данные присутствуют.
        //Повторяется (например, в случае ошибочного указания издания) 
        //Индикатор 1 : # (не определен) Индикатор 2 : # (не определен) 
        //$b Дополнительные сведения об издании 
        // Содержит:
        //- сведения об издании, относящиеся к выпуску (если издание выходило отдельными выпусками)
        //- сведения, формально идентифицированные как сведения об издании, составляющем часть какого-либо издания
        //- дополнительные сведения об издании, которые имеют значительные отличия по содержанию от других выпусков данного произведения в целом
        //- дополнительные сведения об издании, которые являются альтернативным обозначением для сведений, приведенных в $a  
        //Обязательное, если перечисленные данные доступны.
        //Повторяется для каждых дополнительных сведений.

        R = " SELECT ID FROM " + BAZA + "..DATAEXT "
          + " WHERE MNFIELD=205 AND MSFIELD='$a' AND IDMAIN=" + IDM;  // Есть сведения об издании ?
        da.SelectCommand = new SqlCommand();
        da.SelectCommand.CommandText = R;
        da.SelectCommand.Connection = conbase03;
        da.SelectCommand.CommandTimeout = 1200;
        ds = new DataSet();
        int KK = da.Fill(ds);
        if (KK == 0)
        {
            IDENT = "a";
        }
        R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
            + " VALUES ('" + IDSession + "'," + IDM.ToString() + "," + DIDDATA.ToString() + "," + MET.ToString()
            + ",'" + IND1 + "','" + IND2 + "','" + IDENT + "',N'" + Inst + "') ";

        command = new SqlCommand(R, conbase01);
        conbase01.Open();
        command.CommandTimeout = 1200;
        command.ExecuteNonQuery();
        conbase01.Close();
    } // OBR2050 - Сведения об ответственности  205b
    //
    //-- =============================================
    //-- Description:	Вид обработки = OBR2051 - первые сведения об ответственности, относящиеся к изданию 205f
    //--     Если в исходной записи нет 200f - замена на 200f        
    //-- =============================================
    private void OBR205f(String IDSession, Int32 IDM, Int32 DIDDATA, Int16 MET, String IND1,
        String IND2, String IDENT, String DSORT)
    {
        // 2015.04.09
        //            205  Сведения об издании
        //Содержит сведения об отличиях данного издания от других изданий того же произведения
        //- сведения об издании документа;
        //- дополнительные сведения об издании;
        //- сведения об ответственности, относящиеся к изданию  
        //Обязательное, если данные присутствуют.
        //Повторяется (например, в случае ошибочного указания издания) 
        //Индикатор 1 : # (не определен) Индикатор 2 : # (не определен) 
        //$b Дополнительные сведения об издании 
        // Содержит:
        //- сведения об издании, относящиеся к выпуску (если издание выходило отдельными выпусками)
        //- сведения, формально идентифицированные как сведения об издании, составляющем часть какого-либо издания
        //- дополнительные сведения об издании, которые имеют значительные отличия по содержанию от других выпусков данного произведения в целом
        //- дополнительные сведения об издании, которые являются альтернативным обозначением для сведений, приведенных в $a  
        //Обязательное, если перечисленные данные доступны.
        //Повторяется для каждых дополнительных сведений.

        //$d Параллельные сведения об издании 
        // Сведения об издании на языке и / или в графике, отличных от языка / графики сведений об издании в подполе $a.  
        //Факультативное.
        //Повторяется для каждой группы сведений об издании на других языках.

        //$f Сведения об ответственности, относящиеся к изданию 
        // Содержит: любые первые сведения об ответственности, относящиеся к изданию  
        //Обязательное для непараллельных сведений, если таковые присутствуют.
        //Повторяется для сведений, относящихся к $b, и для параллельных сведений, относящихся к $d. 

        //Сведения об ответственности записываются непосредственно после сведений об издании 
        //    / дополнительных сведений, к которым они относятся. Если сведения об ответственности относятся к произведению
        //     в целом, а не к одному отдельному изданию или переизданию, они вводятся в подполя 200$f или 200$g, а не в поле 205.
        //     В некоторых случаях одно подполе 205$f может содержать более одного имени. 

        R = " SELECT ID FROM " + BAZA + "..DATAEXT "
          + " WHERE MNFIELD=200 AND MSFIELD='$f' AND IDMAIN=" + IDM;  // Есть Сведения об ответственности ?
        da.SelectCommand = new SqlCommand();
        da.SelectCommand.CommandText = R;
        da.SelectCommand.Connection = conbase03;
        da.SelectCommand.CommandTimeout = 1200;
        ds = new DataSet();
        int KK = da.Fill(ds);
        if (KK == 0)
        {
            MET = 200;
        }
        R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
            + " VALUES ('" + IDSession + "'," + IDM.ToString() + "," + DIDDATA.ToString() + "," + MET.ToString()
            + ",'" + IND1 + "','" + IND2 + "','f',N'" + Inst + "') ";

        command = new SqlCommand(R, conbase01);
        conbase01.Open();
        command.CommandTimeout = 1200;
        command.ExecuteNonQuery();
        conbase01.Close();
    } // OBR2051 - Сведения об ответственности  205f
    //
    //-- =============================================
    //-- Description:	Вид обработки = OBR2052 - Сведения об ответственности
    //--     Если в исходной записи нет 200f и 205f - замена на 200f        
    //--     Если в исходной записи есть 200f, но нет  205f - замена на 205f        
    //-- =============================================
    private void OBR205g(String IDSession, Int32 IDM, Int32 DIDDATA, Int16 MET, String IND1,
        String IND2, String IDENT, String DSORT)
    {
        // 2015.04.09
        //            205  Сведения об издании
        //Содержит сведения об отличиях данного издания от других изданий того же произведения
        //- сведения об издании документа;
        //- дополнительные сведения об издании;
        //- сведения об ответственности, относящиеся к изданию  
        //Обязательное, если данные присутствуют.

        //Повторяется (например, в случае ошибочного указания издания) 
        //Индикатор 1 : # (не определен) Индикатор 2 : # (не определен) 

        //$b Дополнительные сведения об издании 
        // Содержит:
        //- сведения об издании, относящиеся к выпуску (если издание выходило отдельными выпусками)
        //- сведения, формально идентифицированные как сведения об издании, составляющем часть какого-либо издания
        //- дополнительные сведения об издании, которые имеют значительные отличия по содержанию от других выпусков данного произведения в целом
        //- дополнительные сведения об издании, которые являются альтернативным обозначением для сведений, приведенных в $a  
        //Обязательное, если перечисленные данные доступны.
        //Повторяется для каждых дополнительных сведений.

        //$d Параллельные сведения об издании 
        // Сведения об издании на языке и / или в графике, отличных от языка / графики сведений об издании в подполе $a.  
        //Факультативное.
        //Повторяется для каждой группы сведений об издании на других языках.

        //$f Сведения об ответственности, относящиеся к изданию 
        //  Содержит: любые первые сведения об ответственности, относящиеся к изданию  
        //Обязательное для непараллельных сведений, если таковые присутствуют.
        //Повторяется для сведений, относящихся к $b, и для параллельных сведений, относящихся к $d. 

        // $g Последующие сведения об ответственности 
        //  Содержит: сведения об ответственности для заглавия, записанного в подполе $a, или для зависимого заглавия, записанного в подполе $i, следующие после первых сведений об ответственности (пример 1).  
        //Факультативное.
        //Повторяется. 

        //Сведения об ответственности записываются непосредственно после сведений об издании 
        //    / дополнительных сведений, к которым они относятся. Если сведения об ответственности относятся к произведению
        //     в целом, а не к одному отдельному изданию или переизданию, они вводятся в подполя 200$f или 200$g, а не в поле 205.
        R = " SELECT ID FROM " + BAZA + "..DATAEXT "
          + " WHERE MNFIELD=200 AND MSFIELD='$f' AND IDMAIN=" + IDM;  // Есть Сведения об ответственности ?
        da.SelectCommand = new SqlCommand();
        da.SelectCommand.CommandText = R;
        da.SelectCommand.Connection = conbase03;
        da.SelectCommand.CommandTimeout = 1200;
        ds = new DataSet();
        int KK = da.Fill(ds);
        if (KK == 0)
        {
            MET = 200;
        }
        R = " SELECT ID FROM " + BAZA + "..DATAEXT "
          + " WHERE MNFIELD=205 AND MSFIELD='$f' AND IDMAIN=" + IDM;  // Есть Первые Сведения об ответственности, относящиеся к изданию
        da.SelectCommand = new SqlCommand();
        da.SelectCommand.CommandText = R;
        da.SelectCommand.Connection = conbase03;
        da.SelectCommand.CommandTimeout = 1200;
        ds = new DataSet();
        KK = da.Fill(ds);
        if (KK == 0)
        {
            IDENT = "f";
        }
        R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
            + " VALUES ('" + IDSession + "'," + IDM.ToString() + "," + DIDDATA.ToString() + "," + MET.ToString()
            + ",'" + IND1 + "','" + IND2 + "','" + IDENT + "',N'" + Inst + "') ";

        command = new SqlCommand(R, conbase01);
        conbase01.Open();
        command.CommandTimeout = 1200;
        command.ExecuteNonQuery();
        conbase01.Close();
    } // OBR2052 - Сведения об ответственности  205g
    //
    //-- =============================================
    //-- Description:	Языки
    //-- =============================================
    private void OBR101(String IDSession, Int32 IDM, Int32 DIDDATA, Int16 MET, String IND1,
        String IND2, String IDENT, String DSORT)
    {
        // 2011.12.26
        Int32 KK, KK2;
        String S, KOD;
        //--101  ЯЗЫК ДОКУМЕНТА 
        //--	Поле содержит кодированную информацию о языке каталогизируемого
        //-- документа, его частей и заглавия, а также указывает на язык
        //-- оригинала, если документ является переводом. 
        //--	Обязательное для документов, содержащих текстовую информацию.
        //--	Не повторяется. 
        //--	Индикатор 1: Индикатор перевода 
        //--		0 - Документ на языке(ках) оригинала (в т. ч. параллельный
        //-- текст)
        //--		1 - Документ является переводом оригинала или промежуточного
        //-- перевода
        //--		2 - Документ содержит перевод (несколько переводов
        //--	Если нет возможности установить индикатор в записях,
        //-- конвертированных из исходного формата, вместо значений, описанных
        //-- выше, используется символ-заполнитель ' | '.
        //--	Значение 2 не используется, если переводами в документе являются
        //-- только резюме статей. 
        //--	Индикатор 2: # (не определен) 
        //-- $a Язык текста, звукозаписи и т.д. 
        //--	Обязательное для документов, имеющих текстовую основу (в том 
        //--числе для документов, имеющих не только текстовую основу, например,
        //-- песни, арии, фильмы и др.
        //--	Повторяется, когда текст написан более, чем на одном языке.
        //-- 
        //-- $b Язык промежуточного перевода 
        //--	Обязательное, если каталогизируемый документ переводится не с
        //-- языка оригинала.
        //--	Повторяется, когда перевод осуществлен через несколько
        //-- промежуточных переводов.
        //-- 
        //--$c Язык оригинала
        //--	Обязательное, если каталогизируемый документ является переводом.
        //--	Повторяется, когда оригинал издан более, чем на одном языке.
        //-- 
        //-- $d Язык резюме 
        //--	Язык резюме и рефератов каталогизируемого документа в целом или
        //-- его частей.
        //--	Обязательное, если хотя бы один из языков резюме и/или рефератов
        //-- отличается от языка(ов) текста (подполе 101$a).
        //--	Повторяется, если документ содержит резюме и/или рефераты на
        //-- разных языках.
        //-- 
        //-- $e Язык оглавления 
        //--	Обязательное, если хотя бы один из языков оглавления отличается
        //-- от языка(ов) текста (подполе 101$a).
        //--	Повторяется для каждого языка оглавления.
        //-- 
        //-- $f Язык титульного листа
        //--	Обязательное, если хотя бы один из языков титульного листа
        //-- отличается от языка(ов) текста (подполе 101$a).
        //--	Повторяется для каждого языка титульного листа.
        //-- 
        //-- $g Язык основного заглавия 
        //--	Обязательное, если язык основного заглавия отличается от первого
        //-- или единственного языка текста (подполе 101$a).
        //--	Не повторяется, так как по определению основное заглавие имеет
        //-- один язык. Повторения основного заглавия на других языках являются
        //-- параллельными заглавиями, и их языки указываются в подполе 200$z .
        //-- 
        //-- $h Язык либретто и т.п. 
        //--	Язык или языки текста, если описываемый документ включает текст
        //-- - либо сопроводительный материал, либо напечатанный непосредственно
        //-- в описываемом документе. Подполе не ограничивается либретто как
        //-- таковыми. 
        //--	Обязательное, если язык либретто и т.п. отличается от первого
        //-- или единственного языка текста (подполе 101$a). 
        //--	Повторяется.
        //-- 
        //-- $i Язык сопроводительного материала (кроме либретто, краткого
        //-- содержания и аннотаций) 
        //--	Содержит код языка сопроводительного материала, такого как
        //-- разъяснения к программе, вводные части, инструкции и т.д.
        //--	Обязательное, если хотя бы один из языков сопроводительного
        //-- материала отличается от языка(ов) текста (подполе 101$a).
        //--	Повторяется.
        //-- 
        //-- $j Язык субтитров 
        //--	Язык субтитров кинофильмов, если он отличается от языка
        //-- саундтрека.
        //--	Обязательное, если язык субтитров отличается от языка,
        //-- указанного в подполе 101$a. 
        //--	Повторяется.
        //-- 
        //--  Каждое подполе содержит трехсимвольный код языка (Приложение A).
        //-- Если подполе повторяется, порядок кодов языка должен отражать
        //-- последовательность и значение использования языка в каталогизируемом
        //-- документе. Если это невозможно, коды языков записываются в
        //-- алфавитном порядке. Код 'mul' может применяться, когда в каком-либо
        //-- подполе используется более трех языков. 
        //-- Код "und" (не определено) должен использоваться для документов, язык которых не может быть определен. 
        if (IDENT == "g") //  Язык текста, звукозаписи и т.д. 
        {
            R = " SELECT POL FROM TECHNOLOG_VVV..RUSM "
              + " WHERE IDMAIN= " + IDM.ToString() + " and session='" + IDSession + "'"
              + " AND MET= 101 AND (IDENT='$g' OR POL like '%" + (char)31 + "g%')"; // Уже есть  Язык основного заглавия 
            da.SelectCommand = new SqlCommand();
            da.SelectCommand.CommandText = R;
            da.SelectCommand.Connection = conbase01;
            da.SelectCommand.CommandTimeout = 1200;
            ds = new DataSet();
            KK = da.Fill(ds);
            if (KK > 0)
            {
                ds.Dispose();//  Язык основного заглавия не может повторяться
                return;
            }
        }

        R = " SELECT TOP(1) SMALL_KOD FROM BJRUSMARC..KODLANG "
          + " WHERE SHORTNAME='" + DSORT + "' OR SHORTNAME = REPLACE(N'" + DSORT + "','.','')";
        da.SelectCommand = new SqlCommand();
        da.SelectCommand.CommandText = R;
        da.SelectCommand.Connection = conbase03;
        da.SelectCommand.CommandTimeout = 1200;
        ds = new DataSet();
        KK = da.Fill(ds);
        if (KK != 0)
        {
            KOD = ds.Tables[0].Rows[0][0].ToString();
        }
        else
        {
            KOD = "und"; // Код "und" (не определено) должен использоваться для документов, язык которых не может быть определен. 
        }
        ds.Dispose();

        String KLANG = IDENT + KOD;
        R = " SELECT IDENT,POL FROM TECHNOLOG_VVV..RUSM "
          + " WHERE MET= 101 AND IDMAIN= " + IDM.ToString() + " and session='" + IDSession + "'";
        da.SelectCommand = new SqlCommand();
        da.SelectCommand.CommandText = R;
        da.SelectCommand.Connection = conbase01;
        da.SelectCommand.CommandTimeout = 1200;
        ds = new DataSet();
        KK2 = da.Fill(ds, "list1013");
        if (KK2 != 0) // Уже есть поле 101
        {
            String IDEN = ds.Tables["list1013"].Rows[0][0].ToString();
            S = ds.Tables["list1013"].Rows[0][1].ToString();
            ds.Dispose();
            if ((IDEN == IDENT && S.StartsWith(KOD) == false) || (IDEN != IDENT && S.IndexOf(KLANG) == -1))
            {
                R = " UPDATE TECHNOLOG_VVV..RUSM SET POL=N'" + S + (char)31 + KLANG + "'"
                         + " WHERE MET=101 AND IDMAIN=" + IDM.ToString()
                        + " and session='" + IDSession + "'";
                command = new SqlCommand(R, conbase01);
                conbase01.Open();
                command.CommandTimeout = 1200;
                command.ExecuteNonQuery();
                conbase01.Close();
            }
        }
        else // Первая запись поля 101
        {
            ds.Dispose();
            R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
            + " VALUES ('" + IDSession + "'," + IDM.ToString() + "," + DIDDATA.ToString() + "," + MET.ToString()
            + ",'" + IND1 + "','" + IND2 + "','" + IDENT + "',N'" + KOD + "') "; // Код Языка
            command = new SqlCommand(R, conbase01);
            conbase01.Open();
            command.CommandTimeout = 1200;
            command.ExecuteNonQuery();
            conbase01.Close();
        }
    } // OBR101 - Языки
    //
    //  **************************************************************
    //-- Create date: 2009.02.05
    //-- Description: Коды страны и места издания
    //-- =============================================
    private void OBR102(String IDSession, Int32 IDMQ, Int32 DIDDATAQ, Int16 METQ, String IND1Q,
                    String IND2Q, String IDENTQ, String DSORTQ)
    // 2011.05.10 11:49
    {
        String S, S2, SM, KOD, IND1, IND2, IDENT, MET;
        Int32 DIDDATA, KS;
        DataSet ds210a;
        MET = METQ.ToString();
        DIDDATA = DIDDATAQ;
        //--102  СТРАНА ПУБЛИКАЦИИ ИЛИ ПРОИЗВОДСТВА 
        //--  $a Страна публикации 
        //--     Используются коды из таблицы 2-х символьных кодов ISO 3166-1 и
        //--      ГОСТ 7.67-2003 (см. Приложение B). 
        //--  $b Место издания (не ISO) 
        //--    Код, указывающий на место издания или производства документа.
        //--     В подполе указываются коды, взятые из таблицы, отличной от таблицы
        //--     ISO 3166-2. Таблица, из которой взяты коды, идентифицируется в
        //--      подполе $2. 
        //--  $с Место издания (ISO) 
        //--    Код, указывающий на место издания или производства документа.
        //--    Повторяется, если в поле присутствует более одного подполя $a.
        //--     В подполе указываются коды, взятые из таблицы ISO 3166-2. 
        //--  $2 Код системы (источник кода, отличный от ISO) 
        //--    Источник, из которого взят код в подполе $b. Список кодов см. в
        //--     Приложении G. 
        //-- Код места издания должен указываться непосредственно после кода страны,
        //--  к которой он относится. В случае, если необходимо указать несколько мест
        //--  издания, относящихся к одной стране, рекомендуется повторять код страны,
        //--  так чтобы каждому подполю $b и $c предшествовало подполе $a.
        //--  Рекомендуется указывать соответствующий код для каждого места публикации
        //--  или производства, записанного в поле 210.
        //-- Место издания в принятой форме вводится в поле 620 МЕСТО КАК ТОЧКА ДОСТУПА.
        //--  Отвергнутая форма места издания, если именно она приведена на документе,
        //--  вводится в поле 210 ПУБЛИКАЦИЯ, РАСПРОСТРАНЕНИЕ и др. в том виде,
        //--  в каком она представлена на основном источнике описания. 
        //--Дополнительные коды: 
        //-- XX    Страна неизвестна (значение кода определено пользователем для
        //--        локального использования)
        //-- ZZ    Несколько стран (более трех) 
        //--
        //-- 102
        Int32 K102 = blok80.IndexOf("=" + DIDDATAQ.ToString() + ";"); // Блок обработан ?
        if (K102 != -1) return;  // Блок обработан 
        else blok80 += "=" + DIDDATAQ.ToString() + ";"; // Запоминание обработанного блока

        R = " SELECT MET1,IND1,IND2,IDEN1 FROM BJRUSMARC..VVV2LIBNET "
              + "  WHERE MNF=210 AND MSF='$a'";
        da.SelectCommand = new SqlCommand();
        da.SelectCommand.CommandText = R;
        da.SelectCommand.Connection = conbase03;
        da.SelectCommand.CommandTimeout = 1200;
        ds = new DataSet();
        KS = da.Fill(ds, "list210");

        MET = ds.Tables["list210"].Rows[0][0].ToString();
        IND1 = ds.Tables["list210"].Rows[0][1].ToString();
        IND2 = ds.Tables["list210"].Rows[0][2].ToString();
        IDENT = ds.Tables["list210"].Rows[0][3].ToString();

        R = " SELECT PLAIN FROM " + BAZA + "..DATAEXTPLAIN P"
          + " LEFT JOIN " + BAZA + "..DATAEXT DE ON DE.ID = P.IDDATAEXT "
          + " WHERE P.IDDATA=" + DIDDATA.ToString()
          + " AND MNFIELD=210 AND MSFIELD='$a'"; //  Место ИЗДАНИЯ
        da.SelectCommand = new SqlCommand();
        da.SelectCommand.CommandText = R;
        da.SelectCommand.Connection = conbase03;
        da.SelectCommand.CommandTimeout = 1200;
        ds = new DataSet();
        KS = da.Fill(ds, "listMect");
        if (KS > 0) // В блоке может быть несколько полей  Место ИЗДАНИЯ
        {
            if (KS == 1)
            { // Если в блоке 1 Место издания, то в этом блоке может быть Страна издания
                S = ds.Tables["listMect"].Rows[0][0].ToString().Replace("\n", "").Replace("\r", "").Replace("'", "~");  //  Место ИЗДАНИЯ
                S2 = ""; // Страна издания
                SM = ""; // Код Места издания
                KOD = ""; // Код страны издания
                R = " SELECT PLAIN FROM " + BAZA + "..DATAEXTPLAIN P"
                  + " LEFT JOIN " + BAZA + "..DATAEXT DE ON DE.ID = P.IDDATAEXT "
                  + " WHERE DE.IDDATA=" + DIDDATAQ.ToString()
                  + " AND MNFIELD=102 AND MSFIELD='$a'"; // Страна
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.CommandText = R;
                da.SelectCommand.Connection = conbase03;
                da.SelectCommand.CommandTimeout = 1200;
                Int32 KSa = da.Fill(ds, "list102");
                if (KSa > 0)
                {  // В этом блоке есть Страна
                    S2 = ds.Tables["list102"].Rows[0][0].ToString().Replace("\n", "").Replace("\r", "").Replace("'", "~");
                    S2 = S2.Replace("'", "~");
                    R = " SELECT KOD FROM BJRUSMARC..KODCTPAH WHERE [NAME]='" + S2 + "'"; // Страна
                    da.SelectCommand = new SqlCommand();
                    da.SelectCommand.CommandText = R;
                    da.SelectCommand.Connection = conbase03;
                    da.SelectCommand.CommandTimeout = 1200;
                    if (da.Fill(ds, "listkodc") > 0)
                    {
                        KOD = ds.Tables["listkodc"].Rows[0][0].ToString(); // Код Страны издания
                    }
                }
                S = S.Replace("'", "~");
                R = " SELECT KODCTPAHA,KODMECTO from BJRUSMARC..KODMECTO "// Есть Код Места
                    + " where NAIM='" + S + "' AND KODCTPAHA IS NOT NULL";
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.CommandText = R;
                da.SelectCommand.Connection = conbase03;
                da.SelectCommand.CommandTimeout = 1200;
                ds = new DataSet();
                if (da.Fill(ds, "listKM") > 0)
                { // Для этого места известен Код
                    S2 = ds.Tables["listKM"].Rows[0]["KODCTPAHA"].ToString(); // Код страны издания
                    if (KOD.Length == 0) KOD = S2;
                    SM = ds.Tables["listKM"].Rows[0]["KODMECTO"].ToString(); // Код Места
                }
                R = " SELECT POL from TECHNOLOG_VVV..RUSM "
                    + " where session='" + IDSession + "' AND IDMAIN=" + IDMQ.ToString() + " AND MET=102";
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.CommandText = R;
                da.SelectCommand.Connection = conbase01;
                da.SelectCommand.CommandTimeout = 1200;
                DataSet ds102 = new DataSet();
                K102 = da.Fill(ds102);
                if (K102 > 0)
                { // 
                    P102 = ds102.Tables[0].Rows[0][0].ToString(); // POL102
                }
                else
                {
                    P102 = "";
                }
                if (KOD.Length > 0)
                {
                    if (P102.Length > 0)
                    {
                        if (P102.StartsWith("a") == false)
                        {
                            P102 = KOD;
                        }
                        else
                        {
                            P102 += (char)31 + "a" + KOD;
                        }
                    }
                    else P102 = KOD;
                    if (SM != "-" && SM.Length > 0) P102 = P102 + (char)31 + "c" + SM; // Код Места ISO
                    else P102 = P102 + (char)31 + "b" + S; // Место издания  как Код Места не ISO
                    if (K102 > 0)
                    {
                        R = "UPDATE TECHNOLOG_VVV..RUSM SET POL='" + P102 + "' WHERE session ='" + IDSession
                            + "' AND IDMAIN=" + IDMQ.ToString() + " AND MET=102"; // Издательство
                    }
                    else
                    {
                        R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
                            + " VALUES ('" + IDSession + "'," + IDMQ.ToString() + "," + DIDDATA.ToString() + ",102"
                            + ",'" + IND1 + "','" + IND2 + "','" + IDENT + "',N'" + P102 + "')"; // Издательство
                    }
                    command = new SqlCommand(R, conbase01);
                    conbase01.Open();
                    command.CommandTimeout = 1200;
                    command.ExecuteNonQuery();
                    conbase01.Close();
                }
            } // В блоке 1 Место издания
            else
            { // В блоке KS Место издания
                for (int i = 0; i < KS; i++)
                {
                    S2 = ""; // Страна издания
                    SM = ""; // Код Места издания
                    KOD = ""; // Код страны издания
                    S = ds.Tables["listMect"].Rows[i][0].ToString();  //  Место ИЗДАНИЯ
                    S = S.Replace("'", "~");
                    R = " SELECT KODCTPAHA,KODMECTO from BJRUSMARC..KODMECTO "// Страна
                        + " where NAIM='" + S + "' AND KODCTPAHA IS NOT NULL ";
                    da.SelectCommand = new SqlCommand();
                    da.SelectCommand.CommandText = R;
                    da.SelectCommand.Connection = conbase03;
                    da.SelectCommand.CommandTimeout = 1200;
                    if (da.Fill(ds, "listKM2") > 0)
                    { // Для этого места известен Код
                        KOD = ds.Tables["listKM2"].Rows[0]["KODCTPAHA"].ToString(); // Код страны издания
                        SM = ds.Tables["listKM2"].Rows[0]["KODMECTO"].ToString(); // Код Места
                    }
                    R = " SELECT POL from TECHNOLOG_VVV..RUSM "
                        + " where session='" + IDSession + "' AND IDMAIN=" + IDMQ.ToString() + " AND MET=102";
                    da.SelectCommand = new SqlCommand();
                    da.SelectCommand.CommandText = R;
                    da.SelectCommand.Connection = conbase01;
                    da.SelectCommand.CommandTimeout = 1200;
                    DataSet ds102 = new DataSet();
                    K102 = da.Fill(ds102);
                    if (K102 > 0)
                    { // 
                        P102 = ds102.Tables[0].Rows[0][0].ToString(); // POL102
                    }
                    else
                    {
                        P102 = "";
                    }
                    if (KOD.Length > 0)
                    {
                        if (P102.Length > 0) P102 = (char)31 + "a" + KOD;
                        else P102 = KOD;
                        if (SM != "-" && SM.Length > 0) P102 = P102 + (char)31 + "c" + SM; // Код Места ISO
                        else P102 = P102 + (char)31 + "b" + S; // Место издания  как Код Места не ISO
                        if (K102 > 0)
                        {
                            R = "UPDATE TECHNOLOG_VVV..RUSM SET POL='" + P102 + "' WHERE session ='" + IDSession
                                + "' AND IDMAIN=" + IDMQ.ToString() + " AND MET=102"; // Издательство
                        }
                        else
                        {
                            R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
                                + " VALUES ('" + IDSession + "'," + IDMQ.ToString() + "," + DIDDATA.ToString() + ",102"
                                + ",'" + IND1 + "','" + IND2 + "','" + IDENT + "',N'" + P102 + "')"; // Издательство
                        }
                        command = new SqlCommand(R, conbase01);
                        conbase01.Open();
                        command.CommandTimeout = 1200;
                        command.ExecuteNonQuery();
                        conbase01.Close();
                    }
                } // Цикл по Местам издания текущего блока - для текущего Издательство
            }  // В блоке KS Место издания
        } //  Место ИЗДАНИЯ
        //--
        // 210  ПУБЛИКАЦИЯ, РАСПРОСТРАНЕНИЕ И ДР.
        //Соответствует области выходных данных, распространения и т.д. ISBD, а также области выходных данных ГОСТ 7.1-2003.
        //Содержит информацию о выходных данных, распространении и изготовлении документа. Для рукописных документов поле может содержать
        // информацию о месте и дате производства документа, а также имя переписчика или название мастерской.

        //Обязательное (за исключением записей на составную часть).
        //Повторяется для записи информации об изменении выходных данных в дополнение или вместо примечаний к области выходных данных в поле 300.
        //Индикатор 1: определяет, является ли имя издателя, распространителя и т.д., место и дата издания, записанные в поле, первым / наиболее ранним, промежуточным или текущим.
        //    # – Не применимо / Наиболее ранний из имеющихся издателей
        //    Для монографических документов, которые изначально являются завершенными, используется значение ‘#’. Для продолжающихся ресурсов значение ‘#’ используется при создании первой записи на продолжающийся ресурс; информация в поле с индикатором ‘#’ может быть изменена только в том случае, если она была введена ошибочно, либо в библиографирующее учреждение поступят более ранние выпуски продолжающегося ресурса с отличающейся информацией о выходных данных.
        //    0 – Промежуточный издатель
        //    В случае изменения имени издателя или места издания дополнительная информация о промежуточных выходных данных (т.е. о данных, которые в последствии также были изменены), может быть записана в отдельное вхождение поля 210 с индикатором 0.
        //    1 – Текущий или наиболее поздний издатель
        //    В случае изменения имени издателя или места издания текущая информация о выходных данных (т.е. имени, которое в последствии также было изменено), может быть записана в отдельное вхождение поля 210 с индикатором 1.
        //Индикатор 2: # (не определен)

        //$a	Место издания, распространения и т.д.
        //Содержит: наименование города или местности, где документ был опубликован (с необходимыми уточнениями и дополнениями).
        //  Обязательное (кроме записей ниже высшего уровня для отдельного тома, выпуска и т.п., а также неопубликованных материалов
        //      - рукописей, неизданных или неопубликованных видеоматериалов и фильмов, фотоснимков, необработанных звукозаписей,
        //      неопубликованных коллекций (фондов), авторских оттисков гравюр, литографий, плакатов, лубков, фотографий и т. п.).
        //  Повторяется для названия второго и последующих мест издания.
        //$b	Адрес издателя, распространителя и т. д.
        //  Факультативное.
        //  Повторяется.
        //$c	Имя издателя, распространителя и т. д.
        //  Содержит: наименование издательства, издающей организации или имя издателя.
        //  Обязательное, при наличии данных (кроме записей ниже высшего уровня для отдельного тома, выпуска и т.п., а также
        //      неопубликованных материалов - рукописей, неизданных или неопубликованных видеоматериалов и фильмов, фотоснимков,
        //      необработанных звукозаписей, неопубликованных коллекций (фондов), авторских оттисков гравюр, литографий, плакатов,
        //      лубков, фотографий и т. п.).
        //  Повторяется.
        //$d	Дата издания, распространения и т.д.
        //  Содержит дату или предполагаемую дату издания, дату написания рукописи, дату авторского права (копирайт), 
        //      дату изготовления (печатания), либо период времени, соответствующий той части документа, к которой относится поле.
        //      Допускается вводить открытую дату либо диапазон дат. При отсутствии года приводят приблизительный год в квадратных скобках.
        //  Обязательное: при отсутствии сведений о дате издания приводят предполагаемую дату издания с соответствующими пояснениями,
        //      если это необходимо. Обозначение [б. г.] – «без года» не приводят.
        //  Повторяется.
        //$e	Место изготовления
        //  Содержит: наименование города или местности, где был изготовлен документ либо скомплектована / переплетена составная рукопись
        //            (с необходимыми уточнениями и дополнениями).
        //  Факультативное.
        //  Повторяется.
        //$f	Адрес изготовителя
        //  Факультативное.
        //  Повторяется.
        //$g	Имя изготовителя
        //  Для печатных материалов содержит: имя печатника или наименование типографии.
        //  Факультативное.
        //  Повторяется.
        //$h	Дата изготовления
        //  Содержит дату изготовления документа (либо комплектования составной рукописи), если дата изготовления приводится наряду 
        //   с датой издания.
        //  Факультативное.
        //  Повторяется.
        R = " SELECT PLAIN FROM " + BAZA + "..DATAEXTPLAIN P"
          + " LEFT JOIN " + BAZA + "..DATAEXT DE ON DE.ID = P.IDDATAEXT "
          + " WHERE P.IDMAIN=" + IDMQ.ToString() + " AND DE.IDDATA=" + DIDDATA.ToString()
          + " AND MNFIELD=210 AND MSFIELD='$c'"; // Издательство
        da.SelectCommand = new SqlCommand();
        da.SelectCommand.CommandText = R;
        da.SelectCommand.Connection = conbase03;
        da.SelectCommand.CommandTimeout = 1200;
        ds = new DataSet();
        KS = da.Fill(ds, "list210");
        if (KS == 0)
        {
            R = " SELECT TOP(1) PLAIN FROM " + BAZA + "..DATAEXTPLAIN P"
             + " LEFT JOIN " + BAZA + "..DATAEXT DE ON DE.ID = P.IDDATAEXT "
             + " WHERE P.IDMAIN=" + IDMQ.ToString()
             + " AND MNFIELD=210 AND MSFIELD='$c'"; // Издательство
            da.SelectCommand = new SqlCommand();
            da.SelectCommand.CommandText = R;
            da.SelectCommand.Connection = conbase03;
            da.SelectCommand.CommandTimeout = 1200;
            ds = new DataSet();
            KS = da.Fill(ds, "list210");
        }
        if (KS > 0)
        {
            S = ds.Tables["list210"].Rows[0][0].ToString();
            S = S.Replace("'", "~").Replace("\n", "").Replace("\r", "");
            ds.Dispose();
            R = " SELECT MET1,IND1,IND2,IDEN1 FROM BJRUSMARC..VVV2LIBNET "
                  + "  WHERE MNF=210 AND MSF='$c'";
            da.SelectCommand = new SqlCommand();
            da.SelectCommand.CommandText = R;
            da.SelectCommand.Connection = conbase03;
            da.SelectCommand.CommandTimeout = 1200;
            ds = new DataSet();
            da.Fill(ds, "list210c");

            MET = ds.Tables["list210c"].Rows[0][0].ToString();
            IND1 = ds.Tables["list210c"].Rows[0][1].ToString();
            IND2 = ds.Tables["list210c"].Rows[0][2].ToString();
            IDENT = ds.Tables["list210c"].Rows[0][3].ToString();

            R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
                + " VALUES ('" + IDSession + "'," + IDMQ.ToString() + "," + DIDDATA.ToString() + ","
                + 210 + ",' ',' ','c',N'" + S + "')"; // Издательство
            command = new SqlCommand(R, conbase01);
            conbase01.Open();
            command.CommandTimeout = 1200;
            command.ExecuteNonQuery();
            conbase01.Close();
        }
        ds.Dispose();
        R = " SELECT PLAIN FROM " + BAZA + "..DATAEXTPLAIN P"
          + " LEFT JOIN " + BAZA + "..DATAEXT DE ON DE.ID = P.IDDATAEXT "
          + " WHERE P.IDMAIN=" + IDMQ.ToString() + " AND DE.IDDATA=" + DIDDATA.ToString()
          + " AND MNFIELD=210 AND MSFIELD='$a'"; // Место издания
        da.SelectCommand = new SqlCommand();
        da.SelectCommand.CommandText = R;
        da.SelectCommand.Connection = conbase03;
        da.SelectCommand.CommandTimeout = 1200;
        ds = new DataSet();
        KS = da.Fill(ds, "list210");
        if (KS == 0)
        {
            ds.Dispose();
            R = " SELECT TOP(1) PLAIN FROM " + BAZA + "..DATAEXTPLAIN P"
             + " LEFT JOIN " + BAZA + "..DATAEXT DE ON DE.ID = P.IDDATAEXT "
             + " WHERE P.IDMAIN=" + IDMQ.ToString()
             + " AND MNFIELD=210 AND MSFIELD='$a'"; // Место издания
            da.SelectCommand = new SqlCommand();
            da.SelectCommand.CommandText = R;
            da.SelectCommand.Connection = conbase03;
            da.SelectCommand.CommandTimeout = 1200;
            ds = new DataSet();
            KS = da.Fill(ds, "list210");
        }
        if (KS > 0)
        {
            S = ds.Tables["list210"].Rows[0][0].ToString();
            S = S.Replace("'", "~").Replace("\n", "").Replace("\r", "");

            R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
                + " VALUES ('" + IDSession + "'," + IDMQ.ToString() + "," + DIDDATA.ToString() + ",210"
                + ",' ',' ','a',N'" + S + "')"; // Место издания
            command = new SqlCommand(R, conbase01);
            conbase01.Open();
            command.CommandTimeout = 1200;
            command.ExecuteNonQuery();
            conbase01.Close();
        }
        ds.Dispose();
        R = " SELECT PLAIN FROM " + BAZA + "..DATAEXTPLAIN P"
          + " LEFT JOIN " + BAZA + "..DATAEXT DE ON DE.ID = P.IDDATAEXT "
          + " WHERE P.IDMAIN=" + IDMQ.ToString() + " AND DE.IDDATA=" + DIDDATA.ToString()
          + " AND MNFIELD=2100 AND MSFIELD='$d'"; // Дата издания
        da.SelectCommand = new SqlCommand();
        da.SelectCommand.CommandText = R;
        da.SelectCommand.Connection = conbase03;
        da.SelectCommand.CommandTimeout = 1200;
        ds = new DataSet();
        KS = da.Fill(ds, "list210");
        if (KS > 0)
        {
            S = ds.Tables["list210"].Rows[0][0].ToString();
            S = S.Replace("'", "~").Replace("\n", "").Replace("\r", "");
            R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
                + " VALUES ('" + IDSession + "'," + IDMQ.ToString() + "," + DIDDATA.ToString() + ",210"
                + ",' ',' ','d',N'" + S + "')"; // Дата издания
            command = new SqlCommand(R, conbase01);
            conbase01.Open();
            command.CommandTimeout = 1200;
            command.ExecuteNonQuery();
            conbase01.Close();
        }
        ds.Dispose();
        //-- Типография
        R = " SELECT PLAIN FROM " + BAZA + "..DATAEXTPLAIN P"
          + " LEFT JOIN " + BAZA + "..DATAEXT DE ON DE.ID = P.IDDATAEXT "
          + " WHERE P.IDMAIN=" + IDMQ.ToString() + " AND DE.IDDATA=" + DIDDATAQ.ToString()
          + " AND MNFIELD=2110 AND MSFIELD='$g'"; // Типография
        da.SelectCommand = new SqlCommand();
        da.SelectCommand.CommandText = R;
        da.SelectCommand.Connection = conbase03;
        da.SelectCommand.CommandTimeout = 1200;
        ds = new DataSet();
        KS = da.Fill(ds, "list210");
        if (KS > 0)
        {
            S = ds.Tables["list210"].Rows[0][0].ToString();
            S = S.Replace("'", "~").Replace("\n", "").Replace("\r", "");
            R = " SELECT MET1,IND1,IND2,IDEN1 FROM BJRUSMARC..VVV2LIBNET "
                 + "  WHERE MNF=2110 AND MSF='$g'";
            da.SelectCommand = new SqlCommand();
            da.SelectCommand.CommandText = R;
            da.SelectCommand.Connection = conbase03;
            da.SelectCommand.CommandTimeout = 1200;
            ds = new DataSet();
            da.Fill(ds, "list210g");

            MET = ds.Tables["list210g"].Rows[0][0].ToString();
            IND1 = ds.Tables["list210g"].Rows[0][1].ToString();
            IND2 = ds.Tables["list210g"].Rows[0][2].ToString();
            IDENT = ds.Tables["list210g"].Rows[0][3].ToString();

            R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
                + " VALUES ('" + IDSession + "'," + IDMQ.ToString() + "," + DIDDATAQ.ToString() + ","
                + MET + ",'" + IND1 + "','" + IND2 + "','" + IDENT + "',N'" + S + "')"; // Типография
            command = new SqlCommand(R, conbase01);
            conbase01.Open();
            command.CommandTimeout = 1200;
            command.ExecuteNonQuery();
            conbase01.Close();
        }
        ds.Dispose();
    } // OBR102 - Место издания
    //
    //-- =============================================
    //-- Description:	Вид обработки = 2103 - Издательство
    //-- =============================================
    private void OBR2103(String IDSession, Int32 IDM, Int32 MNF, String MSF, Int32 DIDDATA, Int16 MET,
        String IND1, String IND2, String IDENT, String DSORT)
    {
        //    String PREF;
        //    R = " SELECT  [NAME] FROM BJRUSMARC..VVV2LIBNET "
        //      + " WHERE MNF= " + MNF.ToString() + "AND MSF='" + MSF + "'";
        //    da.SelectCommand = new SqlCommand();
        //    da.SelectCommand.CommandText = R;
        //    da.SelectCommand.Connection = con;
        //    da.SelectCommand.CommandTimeout = 1200;
        //    ds = new DataSet();
        //    da.Fill(ds, "list0");
        //    PREF = ds.Tables["list0"].Rows[0][0].ToString();
        //    R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
        //    + " VALUES ('"+IDSession+"'," + IDM.ToString() + "," + DIDDATA.ToString() + "," + MET.ToString()
        //    + ",'" + IND1 + "',' " + IND2 + "','" + IDENT + "','" + PREF + "' +': '+ '" + DSORT + "') ";
        //    command = new SqlCommand(R, con);
        //    con.Open();
        //    command.CommandTimeout = 1200;
        //    command.ExecuteNonQuery();
        //    con.Close();
    }
    //
    // *******************************************************
    //-- Description:	Другое заглавие
    // *******************************************************
    private void OBR517(String IDSession, Int32 IDMQ, Int32 DIDDATAQ, Int16 METQ, String IND1Q,
                                             String IND2Q, String IDENTQ, String PLAINQ)
    {
        // 2011.12.26
        String S, T, LANG;
        Int32 MNF, KS, KL, KMNF, KKOD;
        //--
        R = " SELECT PLAIN FROM " + BAZA + "..DATAEXTPLAIN P "
          + " LEFT JOIN " + BAZA + "..DATAEXT DE ON DE.ID = P.IDDATAEXT "
          + " WHERE P.IDDATA=" + DIDDATAQ.ToString() + " AND MNFIELD=517 AND MSFIELD='$b'";
        da.SelectCommand = new SqlCommand();
        da.SelectCommand.CommandText = R;
        da.SelectCommand.Connection = conbase03;
        da.SelectCommand.CommandTimeout = 1200;
        ds = new DataSet();
        KS = da.Fill(ds, "listS");
        R = " SELECT PLAIN FROM " + BAZA + "..DATAEXTPLAIN P "
          + " LEFT JOIN " + BAZA + "..DATAEXT DE ON DE.ID = P.IDDATAEXT "
          + " WHERE P.IDDATA=" + DIDDATAQ.ToString() + " AND MNFIELD=517 AND MSFIELD='$z'";
        da.SelectCommand = new SqlCommand();
        da.SelectCommand.CommandText = R;
        da.SelectCommand.Connection = conbase03;
        da.SelectCommand.CommandTimeout = 1200;
        KL = da.Fill(ds, "listL");
        //MessageBox.Show(DIDDATAQ.ToString());
        if (KS == 0)
        { // В этом блоке нет Тип заглавия
            S = "Другое заглавие: " + PLAINQ;
            if (KL > 0)
            {
                String sr = ds.Tables["listL"].Rows[0][0].ToString();
                sr = sr.Replace("'", "~").Replace("\n", "").Replace("\r", "");
                S = S + "; Язык данного заглавия: " + sr;
            }
            R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
             + " VALUES ('" + IDSession + "'," + IDMQ.ToString() + "," + DIDDATAQ.ToString()
             + ",300,' ',' ','a',N'" + S + "')";
            command = new SqlCommand(R, conbase01);
            conbase01.Open();
            command.CommandTimeout = 1200;
            command.ExecuteNonQuery();
            conbase01.Close();
        }
        else
        {
            S = ds.Tables["listS"].Rows[0][0].ToString();
            S = S.Replace("'", "~").Replace("\n", "").Replace("\r", "");
            R = " SELECT MNF FROM BJRUSMARC..TIP_ZAGL  WHERE [NAME]='" + S + "'";
            da.SelectCommand = new SqlCommand();
            da.SelectCommand.CommandText = R;
            da.SelectCommand.Connection = conbase03;
            da.SelectCommand.CommandTimeout = 1200;
            KMNF = da.Fill(ds, "listMNF");
            if (KMNF > 0)
            {
                MNF = Int32.Parse(ds.Tables["listMNF"].Rows[0][0].ToString());
                if (MNF == 300)
                {
                    T = S + ": " + PLAINQ;
                    if (KL > 0)
                    {
                        String sr = ds.Tables["listL"].Rows[0][0].ToString();
                        sr = sr.Replace("'", "~").Replace("\n", "").Replace("\r", "");
                        T = T + "; Язык данного заглавия: " + sr;
                    }
                    R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
                     + " VALUES ('" + IDSession + "'," + IDMQ.ToString() + "," + DIDDATAQ.ToString()
                     + ",300,' ',' ','a',N'" + T + "')";
                    command = new SqlCommand(R, conbase01);
                    conbase01.Open();
                    command.CommandTimeout = 1200;
                    command.ExecuteNonQuery();
                    conbase01.Close();
                    return;
                }
                if (MNF > 500)
                {
                    if (MNF == 532) IND2Q = "3";
                    else IND2Q = " ";
                    R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
                     + " VALUES ('" + IDSession + "'," + IDMQ.ToString() + "," + DIDDATAQ.ToString()
                     + "," + MNF.ToString() + ",'0','" + IND2Q + "','a',N'" + PLAINQ + "')";
                    command = new SqlCommand(R, conbase01);
                    conbase01.Open();
                    command.CommandTimeout = 1200;
                    command.ExecuteNonQuery();
                    conbase01.Close();

                    if (KL > 0)
                    { // Язык данного заглавия
                        LANG = ds.Tables["listL"].Rows[0][0].ToString();
                        LANG = LANG.Replace("'", "~").Replace("\n", "").Replace("\r", "");
                        R = " SELECT SMALL_KOD FROM BJRUSMARC..KODLANG WHERE SHORTNAME='" + LANG + "'";
                        da.SelectCommand = new SqlCommand();
                        da.SelectCommand.CommandText = R;
                        da.SelectCommand.Connection = conbase03;
                        da.SelectCommand.CommandTimeout = 1200;
                        KKOD = da.Fill(ds, "listKOD");
                        if (KKOD == 0)
                        {
                            R = " SELECT SMALL_KOD FROM BJRUSMARC..KODLANG WHERE REPLACE(SHORTNAME,'.','')='" + LANG + "'";
                            da.SelectCommand = new SqlCommand();
                            da.SelectCommand.CommandText = R;
                            da.SelectCommand.Connection = conbase03;
                            da.SelectCommand.CommandTimeout = 1200;
                            KKOD = da.Fill(ds, "listKOD");
                        }
                        if (KKOD > 0)
                        {
                            S = ds.Tables["listKOD"].Rows[0][0].ToString();
                            R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
                             + " VALUES ('" + IDSession + "'," + IDMQ.ToString() + "," + DIDDATAQ.ToString()
                             + "," + MNF.ToString() + ",'0',' ','z',N'" + S + "')";
                            command = new SqlCommand(R, conbase01);
                            conbase01.Open();
                            command.CommandTimeout = 1200;
                            command.ExecuteNonQuery();
                            conbase01.Close();
                        }
                        return;
                    }
                }
            }
        }
    } // OBR517 - 	Другое заглавие
    //
    //-- =============================================
    //-- Description:	Вид обработки = 606 - Предметная рубрика
    //-- =============================================
    private void OBR606(String IDSession, Int32 IDM, Int32 DIDDATA, Int16 MET, String IND1,
        String IND2, String IDENT, String DSORT)
    {
        //          606  НАИМЕНОВАНИЕ ТЕМЫ КАК ПРЕДМЕТ 
        //Поле содержит слово или словосочетание, отражающее содержание документа, представленное в форме точки доступа. К наименованию темы, используемому в качестве предметной рубрики, факультативно могут быть добавлены тематические, географические, хронологические или формальные подзаголовки. 
        //Факультативное.
        //Повторяется. 
        //Индикатор 1: Уровень предметной единицы 
        //0 - Уровень значимости не может быть установлен
        //1 - Первичный термин
        //2 - Вторичный термин
        //# - Нет доступной информации для установления уровня значимости 
        //
        //Индикатор 2: # (не определен)
        //   $a Наименование темы
        //Тематический заголовок предметной рубрики / дескриптор в форме, определяемой используемой системой индексирования. 
        //Обязательное, если поле 606 присутствует в записи.
        //Не повторяется.
        // 
        //$j Формальный подзаголовок
        //Слово или словосочетание, добавляемое к предметной рубрике для отражения формы или вида документа, целевого и читательского назначения, в форме, определяемой системой предметизации. 
        //Обязательное, если имеются доступные данные.
        //Повторяется.
        // 
        //$x Тематический подзаголовок
        //Слово или словосочетание, добавляемое к предметной рубрике для отражения тематического аспекта рассмотрения предмета в документе, в форме, определяемой системой предметизации . 
        //Обязательное, если имеются доступные данные.
        //Повторяется.
        // 
        //$y Географический подзаголовок
        //Слово или словосочетание, добавляемое к предметной рубрике для отражения географического аспекта рассмотрения предмета в документе, в форме, определяемой системой предметизации. 
        //Обязательное, если имеются доступные данные.
        //Повторяется.
        // 
        //$z Хронологический подзаголовок
        //Слово или словосочетание, добавляемое к предметной рубрике для отражения хронологического аспекта рассмотрения предмета в документе, в форме, определяемой системой предметизации. 
        //Обязательное, если имеются доступные данные.
        //Повторяется.
        // 
        //$2 Код системы
        //Код системы предметных рубрик или тезауруса, по правилам которой построены ПР / дескриптор. Список кодов приведен в Приложении G. 
        //G.5. Локальный код 
        // local Используется не стандартный список кодов  
        //Обязательное, если поле 606 присутствует в записи.
        //Не повторяется.
        // 
        Int32 IDCH, NTES, KTES;
        String Inst, ST1, ST2, PR;
        char C1;
        IDCH = Int32.Parse(DSORT); //-- Предметная рубрика
        //            675  УНИВЕРСАЛЬНАЯ ДЕСЯТИЧНАЯ КЛАССИФИКАЦИЯ (UDC/УДК) 
        //Поле содержит классификационный индекс, присвоенный документу в соответствии с Универсальной десятичной классификацией, с указанием используемого издания УДК. 
        //Факультативное.
        //Повторяется. 
        //Индикатор 1: # (не определен)
        //Индикатор 2: # (не определен) 
        //Подполя $a Индекс
        //Классификационный индекс согласно таблицам УДК. 
        //Обязательное, если поле 675 присутствует в записи.
        //Не повторяется.
        //-- udc
        R = " SELECT UDC FROM " + BAZA + "..TPR_UDC "
                 + " WHERE IDCHAIN=" + DSORT;
        da.SelectCommand = new SqlCommand();
        da.SelectCommand.CommandText = R;
        da.SelectCommand.Connection = conbase03;
        da.SelectCommand.CommandTimeout = 1200;
        DataSet ds675 = new DataSet();
        Int32 K675 = da.Fill(ds675, "list675");
        if (K675 > 0)
        {
            Inst = ds675.Tables["list675"].Rows[0][0].ToString();
            Inst = Inst.Replace("'", "~");
            //             S675 = Inst.Replace("'", "'+char(39)+'");
            R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
                   + " VALUES ('" + IDSession + "'," + IDM.ToString() + "," + DIDDATA.ToString()
                    + ",675,' ',' ','a',N'" + Inst + "') ";
            command = new SqlCommand(R, conbase01);
            conbase01.Open();
            command.CommandTimeout = 1200;
            command.ExecuteNonQuery();
            conbase01.Close();
        }
        ds675.Dispose();
        R = " SELECT [VALUE], IDTYPE FROM " + BAZA + "..TPR_CHAIN C "
            + " LEFT JOIN " + BAZA + "..TPR_TES T ON T.ID= C.IDTES "
          + " WHERE C.IDCHAIN=" + DSORT + " ORDER BY IDORDER";
        da.SelectCommand = new SqlCommand();
        da.SelectCommand.CommandText = R;
        da.SelectCommand.Connection = conbase03;
        da.SelectCommand.CommandTimeout = 1200;
        DataSet ds606 = new DataSet();
        NTES = da.Fill(ds606, "list606");
        if (NTES > 0) // Число терминов в ПР
        {
            PR = "";
            for (int ITES = 0; ITES < NTES; ITES++)
            {
                Inst = ds606.Tables["list606"].Rows[ITES][0].ToString(); // 
                Inst = Inst.Replace("'", "~");
                KTES = Int32.Parse(ds606.Tables["list606"].Rows[ITES][1].ToString()); // IDTYPE 
                switch (KTES) // IDTYPE очередного термина
                {
                    case 1:  //1	Тематика
                        C1 = (ITES == 0) ? 'a' : 'x';
                        break;
                    case 2:  //2	Язык
                        C1 = 'x';  //$x Тематический подзаголовок
                        break;
                    case 3:  //3	Географическая подрубрика - Страна
                        C1 = 'y';  //$y Географический подзаголовок
                        break;
                    case 4:  //4	Хронологическая подрубрика
                        C1 = 'z';  //$z Хронологический подзаголовок
                        break;
                    case 7:  //7	Географическая подрубрика - Город
                        C1 = 'y';  //$y Географический подзаголовок
                        break;
                    case 8:  //8	Форма
                        C1 = 'j'; //$j Формальный подзаголовок
                        break;
                    case 9: //9	Жанр
                        C1 = 'j'; //$j Формальный подзаголовок
                        break;
                    default:
                        C1 = 'a';
                        break;
                }
                if ((KTES < 5) || (KTES > 7)) // Не Форма, не Жанр, не Персона
                {
                    if (ITES == 0) PR = Inst;
                    else PR = PR + (char)31 + C1 + Inst;
                }
                else
                {
                    if (KTES == 5) //	Персона
                    {
                        //5	Персона
                        //                   600  ИМЯ ЛИЦА КАК ПРЕДМЕТ 
                        //Поле содержит имя лица (как реального, так и вымышленного образа / персонажа), являющегося одним из объектов рассмотрения в документе, представленное в форме точки доступа. К имени лица, используемому в качестве предметной рубрики, могут быть добавлены тематические, географические, хронологические, формальные подзаголовки. 
                        //Факультативное.
                        //Повторяется. 
                        //Индикатор 1: # (не определен) 
                        //Индикатор 2: Индикатор формы представления имени 
                        //1 - Имя лица записано под фамилией (родовым именем, отчеством и т. д.) 

                        //$a Начальный элемент ввода
                        //Часть имени, используемая как начальный элемент ввода. Начальный элемент ввода определяет положение записи в упорядоченных списках. 
                        //Обязательное, если поле 600 присутствует в записи. Не повторяется.

                        //$b Часть имени, кроме начального элемента ввода
                        //Остаток имени от начального элемента ввода - фамилии или родового имени. Cодержит личные имена (не фамилии и не родовые имена) и другие присвоенные имена в инициальной форме. При использовании подполя индикатор формы представления имени должен быть установлен 1. Предназначенные для печати раскрытые инициалы записываются в подполе $g. 
                        //Обязательное, если имеются доступные данные.
                        //Не повторяется.

                        //$c Дополнения к именам, кроме дат
                        //Любые дополнения к именам (кроме дат), которые не являются неотъемлемой частью имени (титулы, звания, эпитеты, указание должности). 
                        //Обязательное, если имеются доступные данные.
                        //Повторяются при втором или последующих появлениях дополнений.

                        //$d Римские цифры
                        //Римские цифры, ассоциирующиеся с именами членов царствующих семей, князей, священнослужителей, римских пап. Если имеется эпитет (второе имя, прозвище и т.п.), связанный с нумерацией, эпитет также включается в подполе $d. При использовании подполя индикатор формы представления имени должен быть 0. 
                        //Обязательное, если имеются доступные данные.
                        //Не повторяется.

                        //$f Даты
                        //Даты, присоединяемые к именам лиц, включая слова, указывающие на смысл дат (например, жил, родился, умер). Указанные слова вводятся в подполе в полной или сокращенной форме. Все даты для лица, названного в поле, вводятся в одно подполе $f. 
                        //Обязательное, если имеются доступные данные.
                        //Не повторяется.

                        //$g Расширение инициалов личного имени
                        //Полная форма личного имени, когда наряду с инициалами, записанными в подполе $b, необходимо их раскрытие. 
                        //Обязательное, если имеются доступные данные.
                        //Не повторяется.

                        //$j Формальный подзаголовок
                        //Слово или словосочетание, добавляемое к предметной рубрике для отражения формы или вида документа, целевого и читательского назначения, в форме, определяемой системой предметизации. 
                        //Обязательное, если имеются доступные данные.
                        //Повторяется. 

                        //$p Наименование / адрес организации
                        //Подполе содержит наименование и/или адрес организации, в которой данное лицо работало в момент создания документа. 
                        //Факультативное.
                        //Не повторяется. 

                        //$x Тематический подзаголовок
                        //Слово или словосочетание, добавляемое к предметной рубрике для отражения тематического аспекта рассмотрения предмета в документе, в форме, определяемой системой предметизации. 
                        //Обязательное, если имеются доступные данные.
                        //Повторяется.

                        //$y Географический подзаголовок
                        //Слово или словосочетание, добавляемое к предметной рубрике для отражения географического аспекта рассмотрения предмета в документе, в форме, определяемой системой предметизации. 
                        //Обязательное, если имеются доступные данные.
                        //Повторяется.

                        //$z Хронологический подзаголовок
                        //Слово или словосочетание, добавляемое к предметной рубрике для отражения хронологического аспекта рассмотрения предмета в документе, в форме, определяемой системой предметизации. 
                        //Обязательное, если имеются доступные данные.
                        //Повторяется.

                        //$2 Код системы
                        //Код системы предметных рубрик или тезауруса, по правилам которой построены ПР / дескриптор. Список кодов приведен в Приложении G. 
                        //Обязательное, если поле 600 присутствует в записи.
                        //Не повторяется
                        l = Inst.IndexOf('(');
                        if (l != -1)
                        {
                            ST2 = Inst.Substring(l + 1); // года жизни
                            ST2 = ST2.Replace(")", "");
                            Inst = Inst.Remove(l);
                        }
                        else ST2 = "";
                        l = Inst.IndexOf(',');
                        if (l != -1)
                        {
                            ST1 = Inst.Substring(l + 1);  // Инициалы
                            ST1 = ST1.Replace(")", "");
                            Inst = Inst.Remove(l);  // Фамилия
                        }
                        else ST1 = "";
                        ST1 = ST1.TrimEnd(' ');
                        ST1 = ST1.TrimStart(' ');
                        if (ST1.Length > 0)
                        {
                            if (ST1.IndexOf('.') != -1)
                            { // Инициалы
                                Inst = Inst + (char)31 + "b" + ST1;
                            }
                            else
                            { // Расширение инициалов
                                Inst = Inst + (char)31 + "g" + ST1;
                            }
                        } // ST1
                        if (ST2.Length > 0)
                        { // Даты
                            Inst = Inst + (char)31 + "f" + ST2;
                        } // Даты
                        Inst += (char)31 + "2ВГБИЛ";
                        R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
                        + " VALUES ('" + IDSession + "'," + IDM.ToString() + "," + DIDDATA.ToString() + ",600,' ','1'"
                        + ",'a','" + Inst + "') ";
                        command = new SqlCommand(R, conbase01);
                        conbase01.Open();
                        command.CommandTimeout = 1200;
                        command.ExecuteNonQuery();
                        conbase01.Close();
                    } // KTES=5 Персона
                    else
                    {
                        if (KTES == 6)
                        {
                            //6	Организация
                            //                   601  НАИМЕНОВАНИЕ ОРГАНИЗАЦИИ КАК ПРЕДМЕТ 
                            //Поле содержит наименование организации, являющейся одним из объектов рассмотрения в документе, представленное в форме точки доступа. К наименованию организации, используемому в качестве предметной рубрики, могут быть добавлены тематические, географические, хронологические, формальные подзаголовки. 
                            //Факультативное.
                            //Повторяется. 
                            //Индикатор 1: Определяет постоянный или временный характер организации: 
                            //0 - Постоянная организация
                            //1 - Временная организация 
                            //Индикатор 2: Определяет способ ввода наименования: 
                            //0 - Наименование в инверсированной форме
                            //1 - Наименование, введенное под юрисдикцией
                            //2 - Наименование в прямой форме

                            // $a Начальный элемент ввода
                            //Часть наименования организации, используемая как начальный элемент ввода. Начальный элемент ввода определяет положение записи в упорядоченных списках. 
                            //Обязательное, если поле 601 присутствует в записи.
                            //Не повторяется.

                            //$b Структурное подразделение организации
                            //Часть наименования организации, содержащая наименование подведомственной организации, либо наименование структурного подразделения организации при иерархической структуре ее наименования. В это подполе заносится также часть наименования организации, следующая после названия территории в тех заголовках - наименованиях организации, начальным элементом ввода которых является юрисдикция (пример 7). 
                            //Обязательное, если имеются доступные данные.
                            //Повторяется (для каждого последующего иерархического уровня).

                            //$c Идентифицирующий признак
                            //Дополнение к наименованию или уточнение, за исключением порядкового номера, даты и места проведения временной организации. Может включать: географические названия, даты, номера. 
                            //Обязательное, если имеются доступные данные.
                            //Повторяется для каждого из перечисленных идентифицирующих признаков.

                            //$d Порядковый номер временной организации и / или порядковый номер ее части
                            //Указывается арабскими цифрами без наращения окончания. 
                            //Обязательное, если имеются доступные данные.
                            //Не повторяется.

                            //$e Место проведения временной организации
                            //Название города или любой другой местности, в которой проходила временная организация. 
                            //Обязательное, если имеются доступные данные.
                            //Не повторяется.

                            //$f Дата проведения временной организации
                            //Указывается арабскими цифрами без наращения окончания. 
                            //Обязательное, если имеются доступные данные.
                            //Не повторяется.

                            //$g Инверсированный элемент
                            //Часть наименования организации, записанного в инверсированной форме, перенесенная с начала наименования (имя или имя и отчество, записанные после фамилии). 
                            //Обязательное, если имеются доступные данные.
                            //Не повторяется.

                            //$h Часть наименования, отличная от начального элемента ввода и от инверсированного элемента
                            //В инверсированном заголовке - часть наименования, следующая за инверсией. 
                            //Обязательное, если имеются доступные данные.
                            //Не повторяется.

                            //$j Формальный подзаголовок
                            //Слово или словосочетание, добавляемое к предметной рубрике для отражения формы или вида издания, целевого и читательского назначения, в форме, определяемой системой предметизации. 
                            //Обязательное, если имеются доступные данные.
                            //Повторяется.

                            //$x Тематический подзаголовок
                            //Слово или словосочетание, добавляемое к предметной рубрике для отражения тематического аспекта рассмотрения предмета в документе, в форме, определяемой системой предметизации. 
                            //Обязательное, если имеются доступные данные.
                            //Повторяется.

                            //$y Географический подзаголовок
                            //Слово или словосочетание, добавляемое к предметной рубрике для отражения географического аспекта рассмотрения предмета в документе, в форме, определяемой системой предметизации. 
                            //Обязательное, если имеются доступные данные.
                            //Повторяется.

                            //$z Хронологический подзаголовок
                            //Слово или словосочетание, добавляемое к предметной рубрике для отражения хронологического аспекта рассмотрения предмета в документе, в форме, определяемой системой предметизации. 
                            //Обязательное, если имеются доступные данные.
                            //Повторяется.

                            //$2 Код системы
                            //Код системы предметных рубрик или тезауруса, по правилам которой построены ПР / дескриптор. Список кодов приведен в Приложении G. 
                            //Обязательное, если поле 601 присутствует в записи.
                            //Не повторяется.
                            Inst += (char)31 + "2ВГБИЛ";
                            R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
                              + " VALUES ('" + IDSession + "'," + IDM.ToString() + "," + DIDDATA.ToString() + ",601,'0','2'"
                              + ",'a',N'" + Inst + "') ";
                            command = new SqlCommand(R, conbase01);
                            conbase01.Open();
                            command.CommandTimeout = 1200;
                            command.ExecuteNonQuery();
                            conbase01.Close();
                        } // KTES = 6 Организация
                        else
                        {
                            if (KTES == 7)  //7	Географическая местность
                            {
                                //                   607  ГЕОГРАФИЧЕСКОЕ НАИМЕНОВАНИЕ КАК ПРЕДМЕТ 
                                //Поле содержит название географического объекта, являющегося одним из предметов рассмотрения в документе, представленное в форме точки доступа. К географическому наименованию, используемому в качестве предметной рубрики, факультативно могут быть добавлены тематические, географические, хронологические, формальные подзаголовки. 
                                //Факультативное.
                                //Повторяется. 
                                //Индикатор 1: # (не определен)
                                //Индикатор 2: # (не определен) 

                                //Подполя $a Географическое наименование
                                //Географическое наименование в форме, определяемой используемой системой индексирования. 
                                //Обязательное, если поле 607 используется в записи.
                                //Не повторяется.

                                //$j Формальный подзаголовок
                                //Слово или словосочетание, добавляемое к предметной рубрике для отражения формы или вида документа, целевого и читательского назначения, в форме, определяемой системой предметизации. 
                                //Обязательное, если имеются доступные данные.
                                //Повторяется.

                                //$x Тематический подзаголовок
                                //Слово или словосочетание, добавляемое к предметной рубрике для отражения тематического аспекта рассмотрения предмета в документе, в форме, определяемой системой предметизации. 
                                //Обязательное, если имеются доступные данные.
                                //Повторяется.

                                //$y Географический подзаголовок
                                //Слово или словосочетание, добавляемое к предметной рубрике для отражения географического аспекта рассмотрения предмета в документе, в форме, определяемой системой предметизации (пример 3). 
                                //Обязательное, если имеются доступные данные.
                                //Повторяется.

                                //$z Хронологический подзаголовок
                                //Слово или словосочетание, добавляемое к предметной рубрике для отражения хронологического аспекта рассмотрения предмета в документе, в форме, определяемой системой предметизации. 
                                //Обязательное, если имеются доступные данные.
                                //Повторяется.

                                //$2 Код системы
                                //Код системы предметных рубрик или тезауруса, по правилам которой построены ПР / дескриптор. Список кодов приведен в Приложении G. 
                                //Обязательное, если поле 607 приводится в записи.
                                //Не повторяется.
                                Inst += (char)31 + "2ВГБИЛ";
                                R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
                                  + " VALUES ('" + IDSession + "'," + IDM.ToString() + "," + DIDDATA.ToString() + ",607,' ',' '"
                                  + ",'a',N'" + Inst + "') ";
                                command = new SqlCommand(R, conbase01);
                                conbase01.Open();
                                command.CommandTimeout = 1200;
                                command.ExecuteNonQuery();
                                conbase01.Close();
                            } // KTES = 7 Местность
                        }
                    }
                }
            } // Цикл по терминам ПР
            if (PR.Length > 0)
            {
                PR += (char)31 + "2ВГБИЛ";
                R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
                + " VALUES ('" + IDSession + "'," + IDM.ToString() + "," + DIDDATA.ToString() + ",606"
                + ",'0',' ','a',N'" + PR + "') ";
                command = new SqlCommand(R, conbase01);
                conbase01.Open();
                command.CommandTimeout = 1200;
                command.ExecuteNonQuery();
                conbase01.Close();
            }
        }
    } // OBR606
    //
    //-- =============================================
    //-- Description:	Вид обработки = 7003 - Персона
    //-- =============================================
    private void OBR7003(String IDSession, Int32 IDM, Int32 MNF, String MSF, Int32 DIDDATA, Int16 MET,
        String IND1, String IND2, String IDENT, String DSORT)
    {
        //          700  ИМЯ ЛИЦА - ПЕРВИЧНАЯ ОТВЕТСТВЕННОСТЬ 
        //Поле содержит имя лица, для которого определен статус первичной ответственности по отношению к каталогизируемому документу, в форме точки доступа. Используется в том случае, если основной точкой доступа в записи является заголовок, содержащий имя лица. 
        //Обязательное, если должна быть создана основная точка доступа на имя лица, для которого определен статус первичной ответственности.
        //  Поле не может присутствовать в записи, где есть поле 710 НАИМЕНОВАНИЕ ОРГАНИЗАЦИИ - ПЕРВИЧНАЯ ОТВЕТСТВЕННОСТЬ или 720 РОДОВОЕ ИМЯ - ПЕРВИЧНАЯ ОТВЕТСТВЕННОСТЬ, так как запись может иметь только одну точку доступа с первичной ответственностью.
        //Не повторяется. 
        //Индикатор 1: # ( не определен) 
        //Индикатор 2: Индикатор формы представления имени 
        //  Индикатор определяет, записывается ли имя под первым приведенным именем (имеется в виду
        //            личное имя, а не фамилия) в прямом порядке или оно вводится под фамилией
        //            , родовым именем, отчеством в инверсированной форме. 
        //  0 - Имя лица вводится под личным именем или в прямом порядке
        //  1 - Имя лица записано под фамилией (родовым именем, отчеством и т. д.) 
        // $a Начальный элемент ввода
        //Часть имени, используемая как начальный элемент ввода. Если поле 700 приводится в записи, то по подполю 700$a определяется положение записи в упорядоченных списках. 
        //Обязательное, если поле 700 присутствует в записи.
        //Не повторяется.
        // $b Часть имени, кроме начального элемента ввода
        //Остаток имени от начального элемента ввода - фамилии или родового имени. Содержит личные имена (не фамилии) и другие присвоенные имена в инициальной форме. При использовании подполя индикатор формы представления имени должен быть 1. Предназначенные для печати раскрытые инициалы записываются в $g. 
        //Обязательное, если имеются доступные данные.
        //Не повторяется.
        // $c Дополнение к именам, кроме дат
        //Любые дополнения к именам (кроме дат), которые не формируют неотъемлемую часть самого имени, включая звания, эпитеты, определения или указания должности. 
        //Обязательное, если имеются доступные данные.
        //Повторяется для второго или последующих дополнений.
        // $d Римские цифры
        //Римские цифры, связанные с именами римских пап, членов королевских семей и священнослужителей. Если имеется эпитет (второе имя, прозвище и т.п.), связанный с нумерацией, эпитет также включается в подполе $d (примеры 11-12). При использовании подполя индикатор формы представления имени должен быть 0. 
        //Обязательное, если имеются доступные данные.
        //Не повторяется.
        // $f Даты
        //Даты, присоединяемые к именам лиц, включая слова, указывающие на смысл дат (т.е. жил, родился, умер), вводятся в подполе в полной или сокращенной форме . Все даты для лица, названного в поле, вводятся в одно подполе $f. 
        //Обязательное, если имеются доступные данные.
        //Не повторяется.
        // $g Расширение инициалов личного имени
        //Полная форма личного имени, когда наряду с инициалами, записанными в подполе $b, необходимо их раскрытие. 
        //Обязательное, если имеются доступные данные.
        //Не повторяется.
        // $p Наименование/адрес организации
        //Подполе содержит наименование и / или адрес организации, с которой данное лицо было связано в момент создания произведения. 
        //Факультативное.
        //Не повторяется.
        // $3 Номер авторитетной / нормативной записи
        //Контрольный номер авторитетной / нормативной записи для заголовка. Подполе предназначается для использования в рамках Формата авторитетных записей RUSMARC. До внедрения формата подполе может использоваться для номера в локальных файлах библиографических агентств. 
        //Обязательное, при условии существования авторитетной / нормативной записи.
        //Не повторяется.
        //      Примечание
        //Для связи полей, содержащих принятые формы имен в альтернативной графике, по-прежнему
        //            используется подполе $6. 
        //  Примечания о содержании поля
        // Форма 
        //Имя лица и все относящиеся к нему добавления формулируются в соответствии с Российскими правилами каталогизации. 
        // Выбор подполей 
        //Имена одного и того же лица, зафиксированные в файлах авторитетных / нормативных записей
        //     различных библиографирующих учреждений, не всегда будут одинаковым образом
        //     распределены по подполям. Единственным способом определения различий между
        //     начальным элементом ввода, частью имени, кроме начального элемента, и дополнениями к
        //     имени, кроме дат, является анализ их использования. Первый элемент ($a - начальный
        //     элемент ввода) - это слово, которое определяет положение записи в упорядоченных списках.
        //     Второй элемент ($b - часть имени, кроме начального элемента ввода) может использоваться
        //     как второй элемент сортировки. Третий элемент ($c - дополнения к имени, кроме дат) может
        //     использоваться как третий элемент сортировки, или может игнорироваться в процессе
        //     сортировки, особенно если он предшествует подполю $b.
        //Если имена, начинающиеся с неизменяемой частицы, упорядочиваются в списках под следующим
        //    после частицы элементом, то такие частицы рекомендуется записывать в конце подполя
        //    $b - часть имени, кроме начального элемента ввода. Кроме того, в этом случае возможно
        //    использование механизма символов границ сортировки - при этом неизменяемая частица
        //    располагается в начале подполя $a. Выбор одного из этих механизмов определяется
        //    требованиями, которые библиографирующее агентство предъявляет к выходным формам.
        //Титулы, адреса, эпитеты или уточняющие слова имен (кроме дат), добавленные каталогизатором,
        //    записываются как дополнения к именам в подполе $c. 
        //Пунктуация 
        //Рекомендуется включать в запись RUSMARC ту пунктуацию, которая предназначена для вывода
        //    на дисплей / печать. При отсутствии стандартов на пунктуацию получатели записей в
        //    формате RUSMARC должны иметь представление о практике, принятой в агентстве,
        //    подготавливающем запись, а учреждения, распространяющие записи, должны быть
        //    последовательными в своих решениях. В документацию, сопровождающую обменные файлы должны
        //    включаться разъяснения по использованию пунктуации. 
        //Индикатор 0 - имя записано в прямом порядке 
        //ИМЯ
        //ИМЯ ОТЧЕСТВО
        //ИМЯ ПРОЗВИЩЕ (ЭПИТЕТ, ОПРЕДЕЛЕНИЕ)
        //ИМЯ [ФАМИЛИЯ ИМЯ ОТЧЕСТВО] ДУХОВНОЕ ЗВАНИЕ
        //ИМЯ ИМЯ ИМЯ (китайские и другие восточные имена)
        //СОКРАЩЕННОЕ ИМЯ (.) ИНИЦИАЛ
        //ИНИЦИАЛ (.) СОКРАЩЕННОЕ ИЛИ ПОЛНОЕ ИМЯ 
        //
        //Индикатор 1 - имя записано под фамилией, родовым именем, отчеством 
        //ФАМИЛИЯ (,) ИМЯ ОТЧЕСТВО (то же в инициальной форме)
        //ФАМИЛИЯ (,) ИМЯ (то же инициалы)
        //ФАМИЛИЯ
        //ФАМИЛИЯ (-) ФАМИЛИЯ (,) ИМЯ ОТЧЕСТВО (то же в инициальной форме)
        //ФАМИЛИЯ ФАМИЛИЯ ФАМИЛИЯ (,) ИМЯ .(то же инициалы)
        //СОКРАЩЕННАЯ ФАМИЛИЯ (,) ИМЯ (то же инициалы)
        //
        //(во всех схемах могут также присутствовать идентифицирующие признаки)
        //
        String ST1, ST2, ST2_1, AFN_DAT, Inst;
        Int32 i;
        R = " SELECT AFLINKID FROM " + BAZA + "..DATAEXT "
         + " WHERE IDDATA=" + DIDDATA + " AND MNFIELD=" + MET.ToString();
        da.SelectCommand = new SqlCommand();
        da.SelectCommand.CommandText = R;
        da.SelectCommand.Connection = conbase03;
        da.SelectCommand.CommandTimeout = 1200;
        DataSet ds731 = new DataSet();
        Int32 K73 = da.Fill(ds731, "list731");
        if (K73 > 0) // Есть ссылка на AF
        {
            Int32 IDAF = Int32.Parse(ds731.Tables["list731"].Rows[0][0].ToString()); // IDAF
            AFN_DAT = ""; // Даты жизни
            R = " SELECT TOP(1) PLAIN FROM " + BAZA + "..AFNAMESVAR "
             + " WHERE IDAF=" + IDAF + " AND [STATUS]=1";
            da.SelectCommand = new SqlCommand();
            da.SelectCommand.CommandText = R;
            da.SelectCommand.Connection = conbase03;
            da.SelectCommand.CommandTimeout = 1200;
            DataSet ds732 = new DataSet();
            Int32 K732 = da.Fill(ds732, "list732");
            if (K732 > 0) // Есть предпочтительный вариант
            {
                Inst = ds732.Tables["list732"].Rows[0][0].ToString();
                Inst = Inst.Replace("'", "~").Replace("\n", "").Replace("\r", "");
                //  Обинье,Теодор Агриппа д' (1552-1630)
                //  Andrew,Robert (1926-    ) (подлинное имя)
                while ((l = Inst.IndexOf('(')) != -1)  //  Годы жизни,титул или род деятельности
                {
                    ST2 = Inst.Substring(l + 1); // годы жизни,титул или род деятельности
                    i = ST2.IndexOf(")");
                    ST2 = ST2.Substring(0, i); // Внутри ()
                    Inst = Inst.Remove(l, i + 1); // Удаление внутри () вместе с ()
                    while ((l = ST2.IndexOf(';')) != -1)
                    {
                        // Basil (Krivocheine; архиепископ)
                        // Василий (Кривошеин; 1900-1985)
                        ST2_1 = ST2.Substring(0, l - 1); // До ;
                        ST2 = ST2.Substring(l + 1);  // Остаток
                        if (ST2.StartsWith(" ") == true) ST2 = ST2.Substring(1);
                        i = testAFNAME(ST2_1);
                        if (i == 1)
                        {
                            AFN_DAT = ST2_1;
                            goto MAFN;
                        }
                    } // цикл по ;
                    // Losita,Angelo Maria (подлинное имя)
                    // Akihito (император Японии)
                    // Эшбери, Джон (1927-)
                    // Athenagoras (архиепископ; 1912-    )
                    // Эсхил (ок. 525-456 до н.э.)
                    // Bayley,John (    -1869)
                    i = testAFNAME(ST2);
                    if (i == 1)
                    {
                        AFN_DAT = ST2;
                        goto MAFN;
                    }
                } // цикл по () в AFNAMESVAR - Годы жизни,титул или род деятельности
                // В () нет дат жизни
                R = " SELECT PLAIN FROM " + BAZA + "..AFNAMESDATA "
                 + " WHERE IDAF=" + IDAF + " AND IDFIELD=6";     // Годы жизни
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.CommandText = R;
                da.SelectCommand.Connection = conbase03;
                da.SelectCommand.CommandTimeout = 1200;
                DataSet ds737 = new DataSet();
                Int32 K737 = da.Fill(ds737, "list737");
                if (K737 != 0) // 
                {
                    AFN_DAT = ds737.Tables["list737"].Rows[0][0].ToString().Replace("'", "~").Replace("\n", "").Replace("\r", "");
                }
            MAFN: ;
                l = Inst.IndexOf(',');
                if (l != -1)
                {
                    ST1 = Inst.Substring(l + 1);  // Инициалы
                    ST1 = ST1.Replace(")", "");
                    Inst = Inst.Remove(l);  // Фамилия
                }
                else ST1 = "";
                R = " SELECT ID FROM TECHNOLOG_VVV..RUSM "
                 + " WHERE IDBLOCK=" + DIDDATA.ToString() + " AND MET=" + MET.ToString()
                 + " AND IDENT='a' AND POL=N'" + Inst + "' and session='" + IDSession + "'";
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.CommandText = R;
                da.SelectCommand.Connection = conbase01;
                da.SelectCommand.CommandTimeout = 1200;
                DataSet ds733 = new DataSet();
                Int32 K733 = da.Fill(ds733);
                if (K733 == 0)
                { // Добавление фамилии
                    R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
                    + " VALUES ('" + IDSession + "'," + IDM.ToString() + "," + DIDDATA.ToString() + "," + MET.ToString()
                       + ",' ','1','a',N'" + Inst + "') ";
                    command = new SqlCommand(R, conbase01);
                    conbase01.Open();
                    command.CommandTimeout = 1200;
                    command.ExecuteNonQuery();
                    conbase01.Close();
                }
                if (ST1.Length > 0)
                {
                    if (ST1.IndexOf('.') != -1)
                    { // Инициалы
                        R = " SELECT ID FROM TECHNOLOG_VVV..RUSM "
                         + " WHERE IDBLOCK=" + DIDDATA.ToString() + " AND MET=" + MET.ToString()
                         + " AND IDENT='b' AND POL=N'" + ST1 + "' and session='" + IDSession + "'";
                        da.SelectCommand = new SqlCommand();
                        da.SelectCommand.CommandText = R;
                        da.SelectCommand.Connection = conbase01;
                        da.SelectCommand.CommandTimeout = 1200;
                        DataSet ds734 = new DataSet();
                        Int32 K734 = da.Fill(ds734);
                        if (K734 == 0)
                        {
                            R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
                             + " VALUES ('" + IDSession + "'," + IDM.ToString() + "," + DIDDATA.ToString() + ","
                             + MET.ToString() + ",' ','1','b',N'" + ST1 + "') ";
                            command = new SqlCommand(R, conbase01);
                            conbase01.Open();
                            command.CommandTimeout = 1200;
                            command.ExecuteNonQuery();
                            conbase01.Close();
                        }
                    }
                }
                else
                { // Расширение инициалов
                    R = " SELECT ID FROM TECHNOLOG_VVV..RUSM "
                     + " WHERE IDBLOCK=" + DIDDATA.ToString() + " AND MET=" + MET.ToString()
                     + " AND IDENT='g' AND POL=N'" + ST1 + "' and session='" + IDSession + "'";
                    da.SelectCommand = new SqlCommand();
                    da.SelectCommand.CommandText = R;
                    da.SelectCommand.Connection = conbase01;
                    da.SelectCommand.CommandTimeout = 1200;
                    DataSet ds735 = new DataSet();
                    Int32 K735 = da.Fill(ds735);
                    if (K735 == 0)
                    { // Имя лица как предмет
                        R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
                        + " VALUES ('" + IDSession + "'," + IDM.ToString() + "," + DIDDATA.ToString() + ",600,' ','1'"
                        + ",'g',N'" + ST1 + "') ";
                        command = new SqlCommand(R, conbase01);
                        conbase01.Open();
                        command.CommandTimeout = 1200;
                        command.ExecuteNonQuery();
                        conbase01.Close();
                    }
                }
            } // ST1  // Есть предпочтительный вариант
            if (AFN_DAT.Length > 0)
            { // Даты
                R = " SELECT ID FROM TECHNOLOG_VVV..RUSM "
                 + " WHERE IDBLOCK=" + DIDDATA.ToString() + " AND MET=" + MET.ToString()
                 + " AND IDENT='f' AND POL='" + AFN_DAT + "' and session='" + IDSession + "'";
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.CommandText = R;
                da.SelectCommand.Connection = conbase01;
                da.SelectCommand.CommandTimeout = 1200;
                DataSet ds736 = new DataSet();
                Int32 K736 = da.Fill(ds736);
                if (K736 == 0)
                {
                    R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
                    + " VALUES ('" + IDSession + "'," + IDM.ToString() + "," + DIDDATA.ToString() + ",600,' ','1'"
                    + ",'f','" + AFN_DAT + "') ";
                    command = new SqlCommand(R, conbase01);
                    conbase01.Open();
                    command.CommandTimeout = 1200;
                    command.ExecuteNonQuery();
                    conbase01.Close();
                    R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
                    + " VALUES ('" + IDSession + "'," + IDM.ToString() + "," + DIDDATA.ToString() + ",600,' ','1'"
                    + ",'Э','VGBIL') ";
                    command = new SqlCommand(R, conbase01);
                    conbase01.Open();
                    command.CommandTimeout = 1200;
                    command.ExecuteNonQuery();
                    conbase01.Close();
                }
            } // Даты
        }
    }
    //-- =============================================
    //-- Description:	testgodi - Проверка на годы жизхни
    //-- =============================================
    private Int32 testAFNAME(String ST)
    {
        String R;
        //  Athenagoras (1912-    )
        //  Эсхил (ок. 525-456 до н.э.)
        //  Bayley,John (    -1869)
        //  Евсеева,Л.М. (ред.-сост.)
        //  Reisner,Gavriel (Ben-Ephraim)
        //  Платон (428 или 427 до н.э. -348 или 347 до н.э.)
        //  Порфирий (232/233-ок.305)
        //  Алипий (Кастальский-Бороздин; архимандрит)
        //  Фрэг Самир (Ибн Аш-Шааты)
        if (ST.IndexOf("до н.э.") != -1) return 1;
        if (ST.StartsWith("ок.") == true) return 1;
        R = ST.Replace("ок.", "");
        if (R.IndexOfAny("0123456789IVXLM".ToCharArray()) != -1) return 1;
        //K = null;
        R = " SELECT PLAIN "
        + " FROM " + BAZA + "..DATAEXT DE LEFT JOIN " + BAZA + "..DATAEXTPLAIN P ON P.IDDATAEXT=DE.ID "
             + " WHERE P.IDDATA= " + DIDDATA.ToString() + " AND MNFIELD=710 AND MSFIELD='$9' ";
        da.SelectCommand = new SqlCommand();
        da.SelectCommand.CommandText = R;
        da.SelectCommand.Connection = conbase03;
        da.SelectCommand.CommandTimeout = 1200;
        Int32 KK = da.Fill(new DataSet()); // Характеристика организации - врем. или пост.
        if (KK > 0) return 2;

        return 0;
    }
    //
    //-- =============================================
    //-- Description:	Вид обработки = 710 - Организация
    //-- =============================================
    private void OBR710(String IDSession, Int32 IDM, Int32 DIDDATA, Int16 MET, String IND1,
        String IND2, String IDENT, String DSORT)
    {
        String NAME, C, POL, T1, VID, S;
        Int32 IDAF, KK, KK1, KK2, KK3;
        //--710  НАИМЕНОВАНИЕ ОРГАНИЗАЦИИ - ПЕРВИЧНАЯ ОТВЕТСТВЕННОСТЬ 
        //--Поле содержит наименование организации, для которой определен
        //--  статус первичной ответственности по отношению к каталогизируемому
        //--  документу, в форме точки доступа. Поле используется в случае,
        //--  если основной точкой доступа в записи является заголовок,
        //--  содержащий наименование организации. 
        //-- Обязательное, если должна быть создана основная точка доступа
        //--  на наименование организации, для которой определен статус
        //--  первичной ответственности.
        //-- Поле не может присутствовать в записи, где есть поле 700 ИМЯ
        //--  ЛИЦА - ПЕРВИЧНАЯ ОТВЕТСТВЕННОСТЬ или поле 720 - РОДОВОЕ ИМЯ
        //--  - ПЕРВИЧНАЯ ОТВЕТСТВЕННОСТЬ, так как запись может иметь только
        //--  одну точку доступа с первичной ответственностью.
        //-- Не повторяется. 
        //--
        //-- Индикатор 1: Определяет постоянный или временный характер
        //--  организации 
        //-- 0 - Постоянная организация
        //-- 1 - Временная организация
        //-- Если исходный формат не делает различия между постоянными
        //--  и временными организациями, то в позиции индикатора проставляется
        //--  символ-заполнитель.
        //-- Если наименование организации вводится как подразделение
        //--  вышестоящей организации, значение первого индикатора
        //--  определяется типом вышестоящей организации.
        //--
        //-- Индикатор 2: Определяет способ ввода наименования организации 
        //--  0 - Наименование в инверсированной форме
        //--   Инверсированная форма может использоваться, когда наименование
        //--    организации (постоянной или временной) начинается с инициала
        //--    или личного имени, относящихся к имени лица.
        //--  1 - Наименование, введенное под юрисдикцией
        //--   Используется для наименований организаций, относящихся к правительству или юрисдикции, записываемых под наименованием их местонахождения.
        //--  2 - Наименование в прямой форме
        //--   Используется для всех других видов наименований организаций. 
        //-- $a Начальный элемент ввода
        //--   Часть наименования, используемая как начальный элемент ввода.
        //--    Если поле 710 приводится в записи, то по подполю $a
        //--     определяется положение записи в упорядоченных списках. 
        //--   Обязательное, если поле 710 присутствует в записи.
        //--   Не повторяется.
        //-- $b Структурное подразделение
        //--   Часть наименования организации, содержащая наименование
        //--    подведомственной организации или структурного подразделения
        //--    при иерархической структуре наименования организации. А
        //--    также часть наименования организации, следующая после
        //--    названия юрисдикции, в заголовках под юрисдикцией. 
        //--   Обязательное, если имеются доступные данные.
        //--   Повторяется для каждого последующего иерархического уровня.
        //-- $c Идентифицирующий признак
        //--   Дополнение к наименованию или уточнение, за исключением
        //--    порядкового номера, даты и места проведения временной
        //--    организации. Может включать: географические названия, даты,
        //--    номера. 
        //--   Обязательное, если имеются доступные данные.
        //--   Повторяется.
        //-- $d Порядковый номер временной организации и / или порядковый
        //--     номер ее части
        //--   Указывается арабскими цифрами без наращения окончания. 
        //--    Обязательное, если имеются доступные данные.
        //--   Не повторяется.
        //-- $e Место проведения временной организации
        //--   Может представлять собой название города или любой др.
        //--    местности, где проходила временная организация, а также
        //--    название страны. 
        //--   Обязательное, если имеются доступные данные.
        //--   Не повторяется.
        //-- $f Дата проведения временной организации
        //--   Указывается арабскими цифрами без наращения окончания. 
        //--   Обязательное, если имеются доступные данные.
        //--   Не повторяется.
        //-- $g Инверсированный элемент
        //--   Часть наименования организации, записанного в инверсированной
        //--    форме, перенесенная с начала наименования (инициалы или имя
        //--    и отчество, записанные после фамилии).
        //--   Обязательное, если имеются доступные данные.
        //--   Не повторяется.
        //-- $h Часть наименования, отличная от начального элемента ввода
        //--     и инверсированного элемента
        //--   В инверсированном заголовке - часть наименования, следующая
        //--    за инверсией. 
        //--   Обязательное, если имеются доступные данные.
        //--   Не повторяется.
        //-- $p Местонахождение
        //--   Местонахождение или адрес организации. 
        //--   Факультативное.
        //--   Не повторяется.
        //-- $3 Номер авторитетной / нормативной записи
        //--   Контрольный номер авторитетной / нормативной записи для
        //--    заголовка. Подполе предназначается для использования в рамках
        //--    Российского коммуникативного формата представления авторитетных
        //--    / нормативных записей. 
        //--   Обязательное, при условии существования авторитетной / нормативной
        //--   записи.
        //--   Не повторяется.
        //-- $4 Код отношения
        //--   Код, используемый для указания взаимосвязи между организацией,
        //--    указанной в поле, и каталогизируемым документом.
        //--   Обязательное, если организация, указанная в поле, не является
        //--    автором документа (т.е. отсутствие подполя $4 определяет, что
        //--    организация, указанная в поле, является автором).
        //--   Повторяется (аналогично 70- полям).
        if (MET == 710)
        {
            R = " SELECT ID FROM TECHNOLOG_VVV..RUSM WHERE MET=710 AND session='" + IDSession
                + "' AND IDMAIN=" + IDM.ToString();
            da.SelectCommand = new SqlCommand();
            da.SelectCommand.CommandText = R;
            da.SelectCommand.Connection = conbase01;
            da.SelectCommand.CommandTimeout = 1200;
            ds = new DataSet();
            KK = da.Fill(ds);
            ds.Dispose();
            if (KK > 0) MET = 711;
        }
        VID = " ";
        R = " SELECT PLAIN "
        + " FROM " + BAZA + "..DATAEXT DE LEFT JOIN " + BAZA + "..DATAEXTPLAIN P ON P.IDDATAEXT=DE.ID "
             + " WHERE P.IDDATA= " + DIDDATA.ToString() + " AND MNFIELD=710 AND MSFIELD='$9' ";
        da.SelectCommand = new SqlCommand();
        da.SelectCommand.CommandText = R;
        da.SelectCommand.Connection = conbase03;
        da.SelectCommand.CommandTimeout = 1200;
        ds = new DataSet();
        KK = da.Fill(ds, "list0");
        if (KK == 0) VID = "Постоян";
        else
        {
            VID = ds.Tables["list0"].Rows[0][0].ToString().Replace("\n", "").Replace("\r", "");
        }
        ds.Dispose();

        if (VID.Substring(0, 7) == "Постоян")
        { VID = "0"; }
        else
        {
            if (VID.Substring(0, 6) == "Времен")
            { VID = "1"; }
        }
        R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
                   + " VALUES ('" + IDSession + "'," + IDM.ToString() + "," + DIDDATA.ToString() + ","
                   + MET.ToString() + ",'" + VID + "','1','a',N'" + DSORT + "')";
        command = new SqlCommand(R, conbase01);
        conbase01.Open();
        command.CommandTimeout = 1200;
        command.ExecuteNonQuery();
        conbase01.Close();
        R = " SELECT IDAF FROM " + BAZA + "..AFORGSVAR WHERE PLAIN='" + DSORT + "'";
        da.SelectCommand = new SqlCommand();
        da.SelectCommand.CommandText = R;
        da.SelectCommand.Connection = conbase03;
        da.SelectCommand.CommandTimeout = 1200;
        ds = new DataSet();
        KK = da.Fill(ds, "list0");
        if (KK > 0) // Есть авторитетная запись
        {
            IDAF = Int32.Parse(ds.Tables["list0"].Rows[0][0].ToString());
            R = " SELECT  IDFIELD,PLAIN FROM " + BAZA + "..AFORGSDATA "
                + " WHERE IDAF= " + IDAF.ToString();
            da.SelectCommand = new SqlCommand();
            da.SelectCommand.CommandText = R;
            da.SelectCommand.Connection = conbase03;
            da.SelectCommand.CommandTimeout = 1200;
            ds = new DataSet();
            KK2 = da.Fill(ds, "list0");
            if (KK2 > 0)
            {
                for (int count = 0; count < KK2; count++)
                {
                    S = ds.Tables["list0"].Rows[count][0].ToString();
                    POL = ds.Tables["list0"].Rows[count][1].ToString().Replace("\n", "").Replace("\r", "");
                    R = " SELECT  [NAME] FROM " + BAZA + "..AFORGSFIELDS "
                        + " WHERE ID= " + S + " AND LEFT([NAME],5) <> 'Дата ' ";
                    da.SelectCommand = new SqlCommand();
                    da.SelectCommand.CommandText = R;
                    da.SelectCommand.Connection = conbase03;
                    da.SelectCommand.CommandTimeout = 1200;
                    DataSet dsAF = new DataSet();
                    KK3 = da.Fill(dsAF, "list0");
                    if (KK3 > 0)
                    {
                        NAME = dsAF.Tables["list0"].Rows[0][0].ToString();
                        C = ((NAME == "Адрес") ? "p" : "c");
                    }
                    else C = "c";
                    dsAF.Dispose();
                    POL = POL.Replace("'", "~");
                    if (C.Equals("p"))
                    {
                        R = " SELECT IDMAIN FROM  TECHNOLOG_VVV..RUSM "
                            + " WHERE session='" + IDSession + "' AND IDMAIN=" + IDM.ToString() + " AND IDBLOCK=" + DIDDATA.ToString()
                            + " AND MET=" + MET.ToString() + " AND IDENT='p'";
                        da.SelectCommand = new SqlCommand();
                        da.SelectCommand.CommandText = R;
                        da.SelectCommand.Connection = conbase01;
                        da.SelectCommand.CommandTimeout = 1200;
                        dsAF = new DataSet();
                        KK3 = da.Fill(dsAF);
                        dsAF.Dispose();
                        if (KK3 == 0)
                        {
                            R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
                               + " VALUES ('" + IDSession + "'," + IDM.ToString() + "," + DIDDATA.ToString() + ","
                               + MET.ToString() + ",'" + VID + "','1','" + C + "',N'" + POL + "')";
                            command = new SqlCommand(R, conbase01);
                            conbase01.Open();
                            command.CommandTimeout = 1200;
                            command.ExecuteNonQuery();
                            conbase01.Close();
                        }
                    }
                }
            } // END -- WHILE
            //    -- 7003 Номер авторитетной / нормативной записи
            R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
              + " VALUES ('" + IDSession + "'," + IDM.ToString() + "," + DIDDATA.ToString() + "," + MET.ToString()
              + ", '" + VID + "','1','3','" + IDAF.ToString() + "')";
            command = new SqlCommand(R, conbase01);
            conbase01.Open();
            command.CommandTimeout = 1200;
            command.ExecuteNonQuery();
            conbase01.Close();
        } // // Есть авторитетная запись
        // Проверка наличия в BJ ссылки на код роли
        R = " SELECT PLAIN  "
        + " FROM " + BAZA + "..DATAEXT DE LEFT JOIN " + BAZA + "..DATAEXTPLAIN P ON P.IDDATAEXT=DE.ID "
          + " WHERE P.IDDATA= " + DIDDATA.ToString() + " AND MSFIELD= '$4' ";
        da.SelectCommand = new SqlCommand();
        da.SelectCommand.CommandText = R;
        da.SelectCommand.Connection = conbase03;
        da.SelectCommand.CommandTimeout = 1200;
        ds = new DataSet();
        KK2 = da.Fill(ds, "list0");
        if (KK2 == 0)
        {
            if (MET != 710)
            {
                R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
             + " VALUES ('" + IDSession + "'," + IDM.ToString() + "," + DIDDATA.ToString() + "," + MET.ToString()
             + ",'" + VID + "','1','Ю','340')"; // Редактор
                command = new SqlCommand(R, conbase01);
                conbase01.Open();
                command.CommandTimeout = 1200;
                command.ExecuteNonQuery();
                conbase01.Close();
            } // MET != 710 и роль не указана. Подразумевается Автор
        }
        else // есть $4 - роль
        {
            T1 = ds.Tables["list0"].Rows[0][0].ToString();
            R = " SELECT KOD FROM BJRUSMARC..ROLI R "
                + " LEFT JOIN " + BAZA + "..LIST_3 L3 ON L3.NAME=R.NAME "
            + " WHERE SHORTNAME= '" + T1 + "'";
            da.SelectCommand = new SqlCommand();
            da.SelectCommand.CommandText = R;
            da.SelectCommand.Connection = conbase03;
            da.SelectCommand.CommandTimeout = 1200;
            ds = new DataSet();
            KK1 = da.Fill(ds, "list0");
            if (KK1 == 0)
            {
                if (MET != 710)
                {
                    R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
                   + " VALUES ('" + IDSession + "'," + IDM.ToString() + "," + DIDDATA.ToString() + "," + MET.ToString()
                   + ",'" + VID + "','1','Ю','340')"; // Редактор
                    command = new SqlCommand(R, conbase01);
                    conbase01.Open();
                    command.CommandTimeout = 1200;
                    command.ExecuteNonQuery();
                    conbase01.Close();
                }
            }
            else // есть код роли
            {
                POL = ds.Tables["list0"].Rows[0][0].ToString();
                R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
                 + " VALUES ('" + IDSession + "'," + IDM.ToString() + "," + DIDDATA.ToString() + "," + MET.ToString()
                 + ",'" + VID + "','1','Ю',N'" + POL + "')";
                command = new SqlCommand(R, conbase01);
                conbase01.Open();
                command.CommandTimeout = 1200;
                command.ExecuteNonQuery();
                conbase01.Close();
            }
        }
    }  // OBR710 - Организация
    //
    //-- =============================================
    //-- Description:	Вид обработки = 011 - ISSN
    //-- =============================================
    private void OBR011(String IDSession, Int32 IDM, Int32 DIDDATA, Int16 MET, String IND1,
        String IND2, String IDENT, String DSORT)
    {
        String NAME, C, POL, T1, S;
        Int32 IDAF, KK, KK1, KK2, KK3, VID;
        // 011  МЕЖДУНАРОДНЫЙ СТАНДАРТНЫЙ НОМЕР СЕРИАЛЬНОГО ИЗДАНИЯ (ISSN)
        //Поле содержит Международный стандартный номер сериального издания, присвоенный документу центром ISSN, любой ISSN,
        //который был присвоен или аннулирован, а также сведения об условиях доступности и / или цене издания. 
        //Это поле (за исключением подполя $9) соответствует Области стандартного номера (или его альтернативы) и 
        //условий доступности стандартов ISBD(CR) и ГОСТ 7.1-2003. Подполе $9 относится к Области примечания ГОСТ 7.1-2003.
        //Обязательное при наличии ISSN.
        //Повторяется.
        //Индикатор 1: Индикатор значимости продолжающегося ресурса
        //    Показывает, представляет ли продолжающийся ресурс интерес для международных или национальных пользователей, либо только для локальных пользователей, как определено в Руководстве по ISSN (ISSN Manual).
        //    # - Уровень значимости не определен / Не применяется 
        //    0 - Продолжающийся ресурс представляет интерес для международных или национальных пользователей 
        //    1 - Продолжающийся ресурс представляет интерес для локальных пользователей
        //Индикатор 2: # (не определен)
        //$a 	Номер (ISSN)
        //  Содержит правильно составленный ISSN, включая дефис между четвертой и пятой цифрами, без аббревиатуры 'ISSN'.
        //  Обязательное при наличии ISSN.
        //  Не повторяется.
        //$b 	Уточнения
        //  Не используется.
        //$d 	Цена
        //  Содержит указание на цену издания.
        //  Факультативное.
        //  Повторяется.
        //$9 	Тираж
        //  Содержит сведения о тираже каталогизируемого документа.
        //  Факультативное.
        //  Повторяется.
        //$y 	Отмененный ISSN
        //  Содержит любые ISSN, которые являются формально правильными, но отменены центром ISSN.
        //  Факультативное.
        //  Повторяется.
        //$z 	Ошибочный ISSN
        //  Включает любые неправильные ISSN, отличающиеся от тех, что содержатся в подполе $y, чаще всего вследствие опечаток.
        //  Факультативное.
        //  Повторяется.
        //Примечания о содержании поля
        //      Поле 011 может присутствовать только в записях на издание сериального уровня (содержащих код "s" в позиции маркера 7).
        //      В поле 011 вводится ISSN серии, заглавие которой приведено в поле 200.
        //          Для вывода на карточку ISSN серии или подсерии в области серии используется подполе $x поля 225.
        //          Если заглавию в поле 200 соответствует более одного правильного ISSN, то поле 011 повторяется для каждого ISSN.
        //Если поле 200 содержит заглавие серии, включающей собираемую подсерию, то ISSN подсерии вводится в запись на подсерию.
        //Если серия включает несобираемую серию, то ISSN подсерии вводится в поле 011, встроенное в поле 462 УРОВЕНЬ ПОДНАБОРА.
        //Если подсерия включается в несобираемую серию, то ISSN серии вводится в поле 011, встроенное в поле 461 УРОВЕНЬ НАБОРА. 
        VID = 0;
        R = " SELECT IDLEVEL FROM " + BAZA + "..MAIN WHERE ID=" + IDM;
        da.SelectCommand = new SqlCommand();
        da.SelectCommand.CommandText = R;
        da.SelectCommand.Connection = conbase03;
        da.SelectCommand.CommandTimeout = 1200;
        ds = new DataSet();
        da.Fill(ds);
        VID = Int32.Parse(ds.Tables[0].Rows[0][0].ToString());
        ds.Dispose();
        if (VID > 0) return; //      Поле 011 может присутствовать только в записях на издание сериального уровня IDLEVEL<0
        R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
                   + " VALUES ('" + IDSession + "'," + IDM.ToString() + "," + DIDDATA.ToString() + ","
                   + MET.ToString() + ",'" + IND1 + "','" + IND2 + "','" + IDENT + "','" + DSORT + "')";
        command = new SqlCommand(R, conbase01);
        conbase01.Open();
        command.CommandTimeout = 1200;
        command.ExecuteNonQuery();
        conbase01.Close();
    }  // OBR011 - ISSN для сводного уровня
    //
    //-- =============================================
    //-- Description:	Вид обработки = 010a - ISBN
    //-- =============================================
    private void OBR10a(String IDSession, Int32 IDM, Int32 DIDDATA, Int16 MET, String IND1,
        String IND2, String IDENT, String DSORT)
    {
        String S;
        // 010  МЕЖДУНАРОДНЫЙ СТАНДАРТНЫЙ НОМЕР КНИГИ (ISBN)
        //Поле содержит Международный стандартный номер книги и уточнения, которые определяют различия между номерами ISBN, 
        //  если в записи содержится более одного номера. Поле (за исключением подполя $9) соответствует Области стандартного номера
        //  (или его альтернативы) и условий доступности стандартов ISBD и ГОСТ 7.1-2003. Подполе $9 относится к Области примечания
        //   ГОСТ 7.1-2003.
        //Обязательное при наличии ISBN.
        //Повторяется, если необходимо записать более одного правильного ISBN.
        //Индикатор 1: # (не определен)
        //Индикатор 2: # (не определен)
        //$a	Номер (ISBN)
        //  Содержит правильно составленный ISBN, включая дефисы, без аббревиатуры 'ISBN'.
        //  Обязательное при наличии ISBN.
        //  Не повторяется.
        //$b 	Уточнения
        //  Содержит уточнения, например: в пер.
        //  Факультативное.
        //  Не повторяется.
        //$d 	Цена
        //  Содержит сведения о цене каталогизируемого документа.
        //  Факультативное.
        //  Не повторяется.
        //$9 	Тираж
        //  Содержит сведения о тираже каталогизируемого документа.
        //  Факультативное.
        //  Повторяется.
        //$z 	Ошибочный ISBN
        //  Ошибочно присвоенный ISBN, аннулированный впоследствии, ISBN с неверной контрольной суммой, или неверно напечатанный.
        //  Факультативное.
        //  Повторяется. 
        S = "";
        DSORT = DSORT.Replace('Х', 'X').Replace('x', 'X').Replace('х', 'X'); // русск Х -> lat
        if (DSORT.IndexOf(' ') != -1)
        {
            S = DSORT;
            DSORT = DSORT.Replace("  ", " ");
            DSORT = DSORT.Replace(" ", "-");
            DSORT = DSORT.Replace("--", "-");
        }
        if (DSORT.Length >= 13)
        {
            R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
                       + " VALUES ('" + IDSession + "'," + IDM.ToString() + "," + DIDDATA.ToString()
                       + ",010,' ',' ','a',N'" + DSORT + "')";
            command = new SqlCommand(R, conbase01);
            conbase01.Open();
            command.CommandTimeout = 1200;
            command.ExecuteNonQuery();
            conbase01.Close();

            R = " SELECT PLAIN FROM " + BAZA + "..DATAEXTPLAIN DP "
   + " INNER JOIN " + BAZA + "..DATAEXT D ON D.ID=DP.IDDATAEXT "
   + " WHERE MSFIELD='$b' AND D.IDDATA=" + DIDDATA; // Уточнения
            da.SelectCommand = new SqlCommand();
            da.SelectCommand.CommandText = R;
            da.SelectCommand.Connection = conbase03;
            da.SelectCommand.CommandTimeout = 1200;
            ds = new DataSet();
            int KK = da.Fill(ds);
            if (KK != 0)
            {
                if (S.Length == 0)
                {
                    S = ds.Tables[0].Rows[0][0].ToString().Replace("\n", "").Replace("\r", "").Replace("'", "~");// Уточнение
                }
                else
                {
                    S = S + "; " + ds.Tables[0].Rows[0][0].ToString().Replace("'", "~").Replace("\n", "").Replace("\r", "");// Уточнение
                }
            }
            ds.Dispose();

            if (S.Length > 0)
            {
                R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
                           + " VALUES ('" + IDSession + "'," + IDM.ToString() + "," + DIDDATA.ToString()
                           + ",010,' ',' ','b',N'" + S.Replace("'", "~") + "')";  // Уточнение после ISBN в () + доп.поле 10b
                command = new SqlCommand(R, conbase01);
                conbase01.Open();
                command.CommandTimeout = 1200;
                command.ExecuteNonQuery();
                conbase01.Close();
            }

            R = " SELECT PLAIN FROM " + BAZA + "..DATAEXTPLAIN DP "
                + " INNER JOIN " + BAZA + "..DATAEXT D ON D.ID=DP.IDDATAEXT "
                + " WHERE MSFIELD='$z' AND D.IDDATA=" + DIDDATA; // Ошибочный ISBN
            da.SelectCommand = new SqlCommand();
            da.SelectCommand.CommandText = R;
            da.SelectCommand.Connection = conbase03;
            da.SelectCommand.CommandTimeout = 1200;
            ds = new DataSet();
            KK = da.Fill(ds);
            if (KK != 0)
            {
                for (int i = 0; i < KK; i++)
                {
                    S = ds.Tables[0].Rows[i][0].ToString().Replace("'", "~").Replace("\n", "").Replace("\r", ""); // Ошибочный ISBN
                    R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
                               + " VALUES ('" + IDSession + "'," + IDM.ToString() + "," + DIDDATA.ToString()
                               + ",010,' ',' ','z',N'" + S + "')";
                    command = new SqlCommand(R, conbase01);
                    conbase01.Open();
                    command.CommandTimeout = 1200;
                    command.ExecuteNonQuery();
                    conbase01.Close();
                }
            }
            ds.Dispose();
        } // LEN() >= 13
    }  // OBR10a - ISBN 
    //
    //*********************************************************
    private void OBR326(String IDSession, Int32 IDM, String DPPLAIN)
    {
        //-- Description: Примечание о периодичности 
        R = " SELECT IDLEVEL FROM " + BAZA + "..MAIN WHERE ID=" + IDM;
        da.SelectCommand = new SqlCommand();
        da.SelectCommand.CommandText = R;
        da.SelectCommand.Connection = conbase03;
        da.SelectCommand.CommandTimeout = 1200;
        ds = new DataSet();
        da.Fill(ds);
        int VID = int.Parse(ds.Tables[0].Rows[0][0].ToString());
        ds.Dispose();
        if (VID > 0) return; //      Поле 011 может присутствовать только в записях на издание сериального уровня IDLEVEL<0
        else
        {
            R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
+ " VALUES ('" + IDSession + "'," + IDM.ToString() + ",0,326,' ',' ','a',N'" + DPPLAIN + "') ";
            command = new SqlCommand(R, conbase01);
            conbase01.Open();
            command.CommandTimeout = 1200;
            command.ExecuteNonQuery();
            conbase01.Close();
        }
    } // OBR326
    //
    //*********************************************************
    private void OBR200dz(String IDSession, Int32 IDM, Int32 DIDDATA, Int16 MET, String IND1,
        String IND2, String IDENT, String DPPLAIN)
    {
        //-- Description: Параллельное основное заглавие 200d + язык параллельного заглавия 200z 
        R = " SELECT ID FROM " + BAZA + "..DATAEXT "
            + " WHERE MSFIELD='$z' AND IDDATA=" + DIDDATA;
        da.SelectCommand = new SqlCommand();
        da.SelectCommand.CommandText = R;
        da.SelectCommand.Connection = conbase03;
        da.SelectCommand.CommandTimeout = 1200;
        ds = new DataSet();
        int KK = da.Fill(ds);
        if (KK == 0)
        {
            ds.Dispose();
            return; // Параллельное заглавие ДОЛЖНО БЫТЬ с языком параллельного заглавия 
        }
        else
        {
            //  Параллельное заглавие
            //              for (int I = 0; I < KK; I++)
            //              {
            //                  string S = ds.Tables[0].Rows[I][0].ToString(); // Язык данного ПАРАЛЛЕЛЬНОГО ЗАГЛАВИЯ
            //                  R = " SELECT TOP(1) SMALL_KOD FROM BJRUSMARC..KODLANG "
            //+ " WHERE SHORTNAME='" + S + "' OR SHORTNAME = REPLACE(N'" + S + "','.','')";
            //                  da.SelectCommand = new SqlCommand();
            //                  da.SelectCommand.CommandText = R;
            //                  da.SelectCommand.Connection = con;
            //                  da.SelectCommand.CommandTimeout = 1200;
            //                  ds = new DataSet();
            //                  KK = da.Fill(ds);
            //                  if (KK != 0)
            //                  {
            //                      String KOD = ds.Tables[0].Rows[0][0].ToString();
            //                      ds.Dispose();
            //                  }
            //                  ds.Dispose();
            //              }
            R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
+ " VALUES ('" + IDSession + "'," + IDM.ToString() + ",0,200,'" + IND1 + "','" + IND2 + "','d',N'" + DPPLAIN + "') ";
            command = new SqlCommand(R, conbase01);
            conbase01.Open();
            command.CommandTimeout = 1200;
            command.ExecuteNonQuery();
            conbase01.Close();
        }
    } // OBR200dz
    //
    //*********************************************************
    private void OBR200z(String IDSession, Int32 IDM, Int32 DIDDATA, Int16 MET, String IND1,
        String IND2, String IDENT, String DPPLAIN)
    {
        //-- Description: язык параллельного заглавия 200z 
        R = " SELECT TOP(1) SMALL_KOD FROM BJRUSMARC..KODLANG "
+ " WHERE SHORTNAME=N'" + DPPLAIN + "' OR SHORTNAME = REPLACE(N'" + DPPLAIN + "','.','')";
        da.SelectCommand = new SqlCommand();
        da.SelectCommand.CommandText = R;
        da.SelectCommand.Connection = conbase03;
        da.SelectCommand.CommandTimeout = 1200;
        ds = new DataSet();
        int KK = da.Fill(ds);
        if (KK != 0)
        {
            String KOD = ds.Tables[0].Rows[0][0].ToString();
            //             R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
            //+ " VALUES ('" + IDSession + "'," + IDM.ToString() + ",0,200,'" + IND1 + "','" + IND2 + "','z',N'" + KOD + "') ";
            //             command = new SqlCommand(R, con);
            //             con.Open();
            //             command.CommandTimeout = 1200;
            //             command.ExecuteNonQuery();
            //             con.Close();
            if (P200z.Length == 0)
            {
                P200z = KOD;
            }
            else
            {
                P200z = P200z + (char)31 + "z" + KOD;
            }
        }
        ds.Dispose();
    } // OBR200z
    //
    //-- =============================================
    //-- Description:	Вид обработки = 400 - Связи
    //-- =============================================
    private void OBR400(String IDSession, Int32 IDM, Int32 DIDDATA, Int16 MET, String IND1,
        String IND2, String IDENT, String DSORT)
    {
        Int32 KCV, KK, KK2;
        String Inst, ZAGL;
        KCV = Int32.Parse(DSORT);
        //-- Заглавие связи по ссылке=IDMAIN 
        R = " SELECT PLAIN FROM " + BAZA + "..DATAEXTPLAIN P "
        + " LEFT JOIN " + BAZA + "..DATAEXT DE ON DE.ID = P.IDDATAEXT "
        + " WHERE MNFIELD=200 AND MSFIELD='$a' AND P.IDMAIN=" + DSORT;
        da.SelectCommand = new SqlCommand();
        da.SelectCommand.CommandText = R;
        da.SelectCommand.Connection = conbase03;
        da.SelectCommand.CommandTimeout = 1200;
        DataSet ds9 = new DataSet();
        Int32 K9 = da.Fill(ds9, "list9");
        if (K9 > 0)
        {
            ZAGL = ds9.Tables["list9"].Rows[0][0].ToString(); // PLAIN 
            ZAGL = ZAGL.Replace("'", "~").Replace("\n", "").Replace("\r", "");

            String p461 = "2001 " + (char)31 + "a" + ZAGL;
            ds9.Dispose();

            R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
                + " VALUES ('" + IDSession + "'," + IDM.ToString() + "," + DIDDATA.ToString()
                + "," + MET.ToString() + ",'" + IND1 + "','" + IND2 + "','" + IDENT + "',N'" + p461 + "') ";
            command = new SqlCommand(R, conbase01);
            conbase01.Open();
            command.CommandTimeout = 1200;
            command.ExecuteNonQuery();
            conbase01.Close();

        } // Заглавие по ссылке в 1000
        ds9.Dispose();
    } // OBR400
    //
    //-- =============================================
    //-- Description:	Контролирование поля 101 - Языки
    //-- =============================================
    private void CONTROL101(String IDSession, Int32 IDM)
    {
        R = " SELECT IDENT,POL  "
          + "  FROM TECHNOLOG_VVV..RUSM WHERE session='" + IDSession + "' AND IDMAIN=" + IDM.ToString()
          + " AND MET=101";
        da.SelectCommand = new SqlCommand();
        da.SelectCommand.CommandText = R;
        da.SelectCommand.Connection = conbase01;
        da.SelectCommand.CommandTimeout = 1200;
        DataSet ds101 = new DataSet();
        Int32 K101 = da.Fill(ds101);
        if (K101 > 0)
        {
            // Поле содержит кодированную информацию о языке каталогизируемого документа, его частей и заглавия, а также указывает на язык оригинала,
            //   если документ является переводом.
            //Обязательное для документов, содержащих текстовую информацию.
            //Не повторяется.

            //Индикатор 1: Индикатор перевода
            //    Индикатор определяет, составлен ли документ на языке оригинала, является ли переводом или содержит несколько переводов.
            //    0 - Документ на языке(ках) оригинала (в т. ч. параллельный текст)
            //    1 - Документ является переводом оригинала или промежуточного перевода
            //    2 - Документ содержит перевод (несколько переводов
            //    Если нет возможности установить индикатор в записях, конвертированных из исходного формата, вместо значений, описанных выше,
            //       используется символ-заполнитель ' | '.
            //    Значение 2 не используется, если переводами в документе являются только резюме статей.
            //Индикатор 2: # (не определен)
            //$a 	Язык текста, звукозаписи и т.д.
            //  Обязательное для документов, имеющих текстовую основу (в том числе для документов, имеющих не только текстовую основу,
            //       например, песни, арии, фильмы и др.
            //Повторяется, когда текст написан более, чем на одном языке.
            //$b 	Язык промежуточного перевода
            //  Обязательное, если каталогизируемый документ переводится не с языка оригинала.
            //  Повторяется, когда перевод осуществлен через несколько промежуточных переводов.
            //$c 	Язык оригинала
            //  Обязательное, если каталогизируемый документ является переводом.
            //  Повторяется, когда оригинал издан более, чем на одном языке.
            //$d 	Язык резюме
            //  Язык резюме и рефератов каталогизируемого документа в целом или его частей.
            //  Обязательное, если хотя бы один из языков резюме и/или рефератов отличается от языка(ов) текста (подполе 101$a).
            //  Повторяется, если документ содержит резюме и/или рефераты на разных языках.
            //$e 	Язык оглавления
            //  Обязательное, если хотя бы один из языков оглавления отличается от языка(ов) текста (подполе 101$a).
            //  Повторяется для каждого языка оглавления.
            //$f 	Язык титульного листа
            //  Обязательное, если хотя бы один из языков титульного листа отличается от языка(ов) текста (подполе 101$a).
            //  Повторяется для каждого языка титульного листа.
            //$g 	Язык основного заглавия
            //  Обязательное, если язык основного заглавия отличается от первого или единственного языка текста (подполе 101$a).
            //  Не повторяется, так как по определению основное заглавие имеет один язык. Повторения основного заглавия
            //   на других языках являются параллельными заглавиями, и их языки указываются в подполе 200$z .
            //$h 	Язык либретто и т.п.
            //  Язык или языки текста, если описываемый документ включает текст – либо сопроводительный материал, либо напечатанный
            //   непосредственно в описываемом документе. Подполе не ограничивается либретто как таковыми.
            //  Обязательное, если язык либретто и т.п. отличается от первого или единственного языка текста (подполе 101$a).
            //  Повторяется.
            //$i 	Язык сопроводительного материала (кроме либретто, краткого содержания и аннотаций)
            //  Содержит код языка сопроводительного материала, такого как разъяснения к программе, вводные части, инструкции и т.д.
            //  Обязательное, если хотя бы один из языков сопроводительного материала отличается от языка(ов) текста (подполе 101$a).
            //  Повторяется.
            //$j 	Язык субтитров
            //  Язык субтитров кинофильмов, если он отличается от языка саундтрека.
            //  Обязательное, если язык субтитров отличается от языка, указанного в подполе 101$a.
            //  Повторяется.

            //Примечания о содержании поля
            //  Каждое подполе содержит трехсимвольный код языка (Приложение A). Если подполе повторяется, порядок кодов языка должен
            //       отражать последовательность и значение использования языка в каталогизируемом документе. Если это невозможно,
            //       коды языков записываются в алфавитном порядке. Код 'mul' может применяться, когда в каком-либо подполе используется
            //       более трех языков.
            string IDEN = ds101.Tables[0].Rows[0][0].ToString(); // IDENT 
            String S = ds101.Tables[0].Rows[0][1].ToString(); // POLE
            //R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
            //    + " VALUES ('" + IDSession + "'," + IDM.ToString() + "," + DIDDATA.ToString()
            //    + "," + MET.ToString() + ",'" + IND1 + "','" + IND2 + "','" + IDENT + "',N'" + p461 + "') ";
            //command = new SqlCommand(R, con);
            //con.Open();
            //command.CommandTimeout = 1200;
            //command.ExecuteNonQuery();
            //con.Close();

        } // 
        ds101.Dispose();
    } // CONTROL101
    //
    //-- =============================================
    //-- Description:	Контролирование поля 101 - Языки
    //-- =============================================
    private int CONTROL_OBYAZ(String IDSession)
    {
        R = " SELECT IDENT,POL  "
          + "  FROM TECHNOLOG_VVV..RUSM2 WHERE session='" + IDSession + "' "
          + " AND MET=215";
        //and (IDENT='a' OR POL LIKE '%\u0031a%')";
        da.SelectCommand = new SqlCommand();
        da.SelectCommand.CommandText = R;
        da.SelectCommand.Connection = conbase01;
        da.SelectCommand.CommandTimeout = 1200;
        DataSet dss = new DataSet();
        Int32 Kc = da.Fill(dss);
        dss.Dispose();
        if (Kc == 0)
        {
            return -1;
        }
        else
        {
            R = " SELECT R21.ID FROM TECHNOLOG_VVV..RUSM2 R21 "
            + " inner join  TECHNOLOG_VVV..RUSM2 R22 ON R21.ID=R22.ID "
            + " WHERE R21.session='" + IDSession + "' AND R21.session=R22.session"
                + @" AND R21.MET=225 and R21.IDENT!='a' AND R21.POL NOT LIKE '%\u001Fa%'"
                + @" AND R22.MET=225 AND (R22.IDENT='i' OR R22.POL LIKE '%\u001Fi%')";
            da.SelectCommand = new SqlCommand();
            da.SelectCommand.CommandText = R;
            da.SelectCommand.Connection = conbase03;
            da.SelectCommand.CommandTimeout = 1200;
            dss = new DataSet();
            Kc = da.Fill(dss);
            dss.Dispose();
            if (Kc > 0)
            {
                return -1;
            }

            else
            {
                R = " SELECT ID FROM TECHNOLOG_VVV..RUSM2 WHERE session='" + IDSession + "' "
                  + " AND MET=101";
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.CommandText = R;
                da.SelectCommand.Connection = conbase01;
                da.SelectCommand.CommandTimeout = 1200;
                dss = new DataSet();
                Kc = da.Fill(dss);
                dss.Dispose();
                if (Kc == 0)
                {
                    return -1;
                }
                else
                {
                    return 0;// 
                }
            }
        }
    } // CONTROL_OBYAZ
    // 
    // =================================================================
    //-- Description:	Слияние подполей РУСМАРКа - RUSM -> RUSM2
    //-- =============================================
    private void MERGERUSM(String IDSession)
    {
        //-- Слияние подполей в поле
        String Inst, IND1, IND2, IDENT, POL;
        Int16 MET;
        Int32 IDB, IDM, KK, KK2, KK3;

        R = " SELECT IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL  "
          + "  FROM TECHNOLOG_VVV..RUSM WHERE session='" + IDSession + "'"
          + " ORDER BY IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT ";
        da.SelectCommand = new SqlCommand();
        da.SelectCommand.CommandText = R;
        da.SelectCommand.Connection = conbase01;
        da.SelectCommand.CommandTimeout = 1200;
        ds = new DataSet();
        KK = da.Fill(ds, "listMER");

        if (KK > 0)
        {
            for (int count = 0; count < KK; count++)
            {
                IDM = Int32.Parse(ds.Tables["listMER"].Rows[count][0].ToString());
                IDB = Int32.Parse(ds.Tables["listMER"].Rows[count][1].ToString());
                MET = Int16.Parse(ds.Tables["listMER"].Rows[count][2].ToString());
                IND1 = ds.Tables["listMER"].Rows[count][3].ToString();
                IND2 = ds.Tables["listMER"].Rows[count][4].ToString();
                IDENT = ds.Tables["listMER"].Rows[count][5].ToString();
                IDENT = IDENT.Replace('Э', '2').Replace('Ю', '4').Replace('Я', '5');
                POL = ds.Tables["listMER"].Rows[count][6].ToString();
                ds.Dispose();
                //if (IDM==1358687) MessageBox.Show(POL);
                POL = POL.Replace(@"''", "~"); // есть
                POL = POL.Replace(@"'", "~");
                //POL = POL.Replace("&", "&amp");
                if (((MET >= 300) && (MET <= 399)) || ((MET >= 600) && (MET <= 675)) || MET == 1000)
                {
                    //-- Подполя выводятся отдельными полями
                    Inst = IND1 + IND2 + (char)31 + IDENT + POL;
                    String S675 = Inst;
                    R = " SELECT ID FROM TECHNOLOG_VVV..RUSM2 "
                      + " WHERE IDMAIN = " + IDM.ToString() + " and session='" + IDSession
                      + "' AND MET= " + MET.ToString() + " AND IND1='" + IND1 + "' AND IND2='" + IND2 + "' AND IDENT='" + IDENT
                      + "' AND POL=N'" + Inst + "'";
                    da.SelectCommand = new SqlCommand();
                    da.SelectCommand.CommandText = R;
                    da.SelectCommand.Connection = conbase01;
                    da.SelectCommand.CommandTimeout = 1200;
                    DataSet ds2 = new DataSet();
                    KK2 = da.Fill(ds2);
                    if (KK2 == 0)
                    {
                        R = " INSERT INTO TECHNOLOG_VVV..RUSM2 (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
                          + " VALUES ('" + IDSession + "'," + IDM.ToString() + "," + IDB + " ," + MET + ",'"
                          + IND1 + "','" + IND2 + "','" + IDENT + "',N'" + S675 + "') ";
                        command = new SqlCommand(R, conbase01);
                        conbase01.Open();
                        command.CommandTimeout = 1200;
                        command.ExecuteNonQuery();
                        conbase01.Close();
                    }
                }
                else
                {
                    if (MET == 210 || MET == 200)
                    {
                        R = " SELECT  TOP(1) POL,ID FROM TECHNOLOG_VVV..RUSM2 "
                          + " WHERE IDMAIN = " + IDM.ToString() + "AND MET= " + MET.ToString()
            + " and session='" + IDSession + "'";
                        da.SelectCommand = new SqlCommand();
                        da.SelectCommand.CommandText = R;
                        da.SelectCommand.Connection = conbase01;
                        da.SelectCommand.CommandTimeout = 1200;
                        DataSet ds2 = new DataSet();
                        KK2 = da.Fill(ds2, "list");
                        if (KK2 == 0)
                        { // Добавление поля
                            Inst = IND1 + IND2 + (char)31 + IDENT + POL;
                            R = " INSERT INTO TECHNOLOG_VVV..RUSM2 "
                              + "(session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
                              + " VALUES ('" + IDSession + "'," + IDM.ToString() + "," + IDB.ToString()
                              + "," + MET.ToString() + ",'" + IND1 + "','" + IND2
                              + "','" + IDENT + "',N'" + Inst + "') ";
                            command = new SqlCommand(R, conbase01);
                            conbase01.Open();
                            command.CommandTimeout = 1200;
                            command.ExecuteNonQuery();
                            conbase01.Close();
                        }
                        else // ЕСТЬ УЖЕ
                        { // Добавление подполя
                            IDB = Int32.Parse(ds2.Tables["list"].Rows[0][1].ToString());
                            Inst = ds2.Tables["list"].Rows[0][0].ToString(); // POL
                            Inst += (char)31 + IDENT + POL;
                            R = " UPDATE TECHNOLOG_VVV..RUSM2 "
                             + " SET POL = N'" + Inst + "' "
                             + " WHERE ID= " + IDB.ToString() + " and session='" + IDSession + "'";
                            command = new SqlCommand(R, conbase01);
                            conbase01.Open();
                            command.CommandTimeout = 1200;
                            command.ExecuteNonQuery();
                            conbase01.Close();
                        }
                        ds2.Dispose();
                    } //MET==210 || MET==200
                    else
                    {
                        R = " SELECT  POL  "
                          + " FROM TECHNOLOG_VVV..RUSM2 "
                          + " WHERE IDMAIN=" + IDM.ToString() + " AND IDBLOCK=" + IDB.ToString()
                          + " AND MET=" + MET.ToString() + "  AND IND1='" + IND1 + "' AND IND2='" + IND2 + "'"
                          + " and session='" + IDSession + "'";
                        da.SelectCommand = new SqlCommand();
                        da.SelectCommand.CommandText = R;
                        da.SelectCommand.Connection = conbase01;
                        da.SelectCommand.CommandTimeout = 1200;
                        DataSet ds3 = new DataSet();
                        KK3 = da.Fill(ds3, "list3");
                        if (KK3 == 0)
                        { // Добавление поля
                            Inst = (MET < 10) ? POL : IND1 + IND2 + (char)31 + IDENT + POL;
                            R = " INSERT INTO TECHNOLOG_VVV..RUSM2 (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
                            + " VALUES ('" + IDSession + "'," + IDM.ToString() + "," + IDB.ToString() + ","
                            + MET.ToString() + ",'" + IND1 + "','" + IND2 + "','" + IDENT + "',N'" + Inst + "') ";
                            command = new SqlCommand(R, conbase01);
                            conbase01.Open();
                            command.CommandTimeout = 1200;
                            command.ExecuteNonQuery();
                            conbase01.Close();
                        }
                        else
                        { // Добавление подполя
                            Inst = ds3.Tables["list3"].Rows[0][0].ToString();
                            Inst += (char)31 + IDENT + POL;
                            R = " UPDATE TECHNOLOG_VVV..RUSM2 SET POL = N'" + Inst + "'"
                            + " WHERE IDMAIN= " + IDM.ToString() + "AND IDBLOCK=" + IDB.ToString()
                            + " AND MET=" + MET.ToString() + " AND IND1='" + IND1 + "' AND IND2='" + IND2
                            + "' and session='" + IDSession + "'";
                            command = new SqlCommand(R, conbase01);
                            conbase01.Open();
                            command.CommandTimeout = 1200;
                            command.ExecuteNonQuery();
                            conbase01.Close();
                        }
                        ds3.Dispose();
                    }
                } // не 300
            } // for
            conbase01.Close();
        } // RUSM не пуста
    }  // MERGE
    // ------------------------------------------------------
    private Byte[] SaveZap(String IDSession, Int32 IDM)
    //2011.05.10
    {
        String sprav, rsp, ind1, ind2, ident, MARKER;
        Int32 KSZ, met, AllbyteCount, byteCount;
        Byte[] br = new Byte[1] { 0x00 };
        MARKER = "nam  22";
        //CREATE TABLE [dbo].[RUSM2](
        //    [ID] [int] IDENTITY(1,1) NOT NULL,
        //    [IDMAIN] [int] NULL,
        //    [IDBLOCK] [int] NULL,
        //    [MET] [smallint] NULL,
        //    [IND1] [char](1) NULL,
        //    [IND2] [char](1) NULL,
        //    [IDENT] [char](1) NULL,
        //    [POL] [nvarchar](3500) NULL
        da.SelectCommand = new SqlCommand();
        da.SelectCommand.CommandText = " SELECT ID,IDBLOCK,MET,IND1,IND2,IDENT,POL "
            + " FROM TECHNOLOG_VVV..RUSM2 "
            + " WHERE IDMAIN=" + IDM.ToString() + " and session='" + IDSession + "'"
            + " ORDER BY IDBLOCK,MET,IND1,IND2,IDENT";
        da.SelectCommand.Connection = conbase01;
        da.SelectCommand.CommandTimeout = 1200;
        DataSet dsZ = new DataSet();
        KSZ = da.Fill(dsZ, "listKSZ");
        if (KSZ > 0)
        {
            rsp = ""; // Сумма длин всех полей
            for (Int32 count = 0; count < KSZ; count++)
            {
                met = Int32.Parse(dsZ.Tables["listKSZ"].Rows[count]["MET"].ToString());
                if (met != 0) rsp = rsp + dsZ.Tables["listKSZ"].Rows[count]["POL"].ToString() + (char)30;
            }
            rsp = rsp.Replace("~", "'");
            UTF8Encoding utf8 = new UTF8Encoding();
            AllbyteCount = utf8.GetByteCount(rsp); // Сумма длин всех полей UTF-8
            sprav = "";
            posbrr = 0;
            mrc = new Byte[AllbyteCount];
            String tr = "";
            for (Int32 count = 0; count < KSZ; count++)
            {
                //              GetTranslitStr(p980,0);
                tr = dsZ.Tables["listKSZ"].Rows[count]["POL"].ToString() + (char)30;
                met = Int32.Parse(dsZ.Tables["listKSZ"].Rows[count]["MET"].ToString());
                if (met == 0) MARKER = tr;
                else
                {
                    Int32 IDB = Int32.Parse(dsZ.Tables["listKSZ"].Rows[count]["IDBLOCK"].ToString());
                    Int32 IDR2 = Int32.Parse(dsZ.Tables["listKSZ"].Rows[count][0].ToString());
                    ind1 = dsZ.Tables["listKSZ"].Rows[count]["IND1"].ToString();
                    ind2 = dsZ.Tables["listKSZ"].Rows[count]["IND2"].ToString();
                    ident = dsZ.Tables["listKSZ"].Rows[count]["IDENT"].ToString();
                    tr = tr.Replace("~", "'");
                    //UTF8Encoding utf8 = new UTF8Encoding();
                    byteCount = utf8.GetBytes(tr, 0, tr.Length, mrc, posbrr);

                    sprav += met.ToString().PadLeft(3, '0') + byteCount.ToString().PadLeft(4, '0');
                    sprav += posbrr.ToString().PadLeft(5, '0');
                    posbrr += byteCount;
                }
            }
            sprav += (char)30; // Конец поля 
            Int32 ls = sprav.Length + 24; // Длина справочника и маркера = базовый адрес
            Int32 lz = ls + AllbyteCount + 1; // Длина записи

            rs = lz.ToString().PadLeft(5, '0') + MARKER.Substring(5, 7) + ls.ToString().PadLeft(5, '0');
            rs += "3  450 " + sprav;

            br = new Byte[lz]; // Результирующий массив
            // Encode the entire string.
            // public override int GetBytes(
            //    string s,
            //    int charIndex,
            //    int charCount,
            //    byte[] bytes,
            //    int byteIndex
            //)
            Int32 L = utf8.GetBytes(rs, 0, ls, br, 0); // Запись Маркера и Справочника
            utf8.GetBytes(rsp, 0, rsp.Length, br, ls); // Добавление полей
            br[lz - 1] = Convert.ToByte((char)29);
        }
        R = "DELETE TECHNOLOG_VVV..RUSM2 WHERE session='" + IDSession + "'";
        command = new SqlCommand(R, conbase01);
        conbase01.Open();
        command.CommandTimeout = 1200;
        command.ExecuteNonQuery();
        conbase01.Close();

        return br;
    }  // End of SaveZap().

    private void FormFileN()
    {
        //da.SelectCommand.CommandText = "SELECT ID FROM " + BAZA + "..MAIN WHERE "
        //+ "  DATEDIFF(dy,DateChange,'" + dt1.ToString("yyyyMMdd") + "')<=0 "
        //+ " AND DATEDIFF(dy,DateChange,'" + dt2.ToString("yyyyMMdd") + "')>=0 "
        // Новые
        //SELECT [ID],[p001],[DateBase],[notload],[del],[IDtrans],[ImyaF],[ImyaFNEB],[Timestamp]
        //  FROM [BJRUSMARC].[dbo].[LIBNET_NEB]
        da.SelectCommand = new SqlCommand();
        da.SelectCommand.CommandText =
           " SELECT DISTINCT IDMAIN FROM " + BAZA + "..DATAEXT"
            + " WHERE SORT = 'Длявыдачи' AND MNFIELD=921 AND MSFIELD='$c' "
            + " AND IDMAIN NOT IN "
            + " (SELECT IDMAIN FROM  " + BAZA + "..DATAEXT"
            + "    WHERE SORT = 'Учетнаязапись' AND MNFIELD=899 AND MSFIELD='$x')"
            + " AND IDMAIN NOT IN ("
            + "SELECT DISTINCT PIN as IDMAIN FROM BJRUSMARC..LIBNETNEB WHERE notload=''"
            + " AND del=0 AND BaZa='" + PREFIX001 + "')";
        da.SelectCommand.Connection = conbase03;
        da.SelectCommand.CommandTimeout = 1200;
        DataSet dst = new DataSet();
        Int32 K = da.Fill(dst);
        Byte[] str_out = new Byte[0];
        //K = 2400;
        //Int32 K = 1;
        for (int countM = 0; countM < K; countM++)
        {
            command = new SqlCommand("DELETE TECHNOLOG_VVV..RUSM; DELETE TECHNOLOG_VVV..RUSM2;", conbase01);
            conbase01.Open();
            command.CommandTimeout = 1200;
            command.ExecuteNonQuery();
            conbase01.Close();

            int IDMt = Int32.Parse(dst.Tables[0].Rows[countM][0].ToString());
            //int IDMt = 1202423;
            da.SelectCommand = new SqlCommand();
            da.SelectCommand.CommandText = " SELECT ID FROM  " + BAZA + "..DATAEXT "
            + " WHERE IDMAIN=" + IDMt.ToString() + " and MNFIELD=921 AND MSFIELD='$b' AND SORT='Изданиекартографическое'";
            da.SelectCommand.Connection = conbase03;
            da.SelectCommand.CommandTimeout = 1200;
            DataSet ds921 = new DataSet();
            int K921 = da.Fill(ds921);
            ds921.Dispose();
            if (K921 > 0)
            {
                continue; // Карты не обрабатываются
            }
            PBJ2RUSM(IDSession, IDMt, "n"); // Создание таблицы RUSM

            da.SelectCommand = new SqlCommand();
            da.SelectCommand.CommandText = " SELECT ID,POL "
            + " FROM TECHNOLOG_VVV..RUSM "
            + " WHERE IDMAIN=" + IDMt.ToString() + " and session='" + IDSession + "'"
            + " AND MET=102";
            da.SelectCommand.Connection = conbase01;
            da.SelectCommand.CommandTimeout = 1200;
            DataSet dsZ = new DataSet();
            int K102 = da.Fill(dsZ);
            if (K102 > 0)
            {
                K102 = Int32.Parse(dsZ.Tables[0].Rows[0]["ID"].ToString());
                P102 = dsZ.Tables[0].Rows[0]["POL"].ToString().Replace("'", "~") + (char)31 + "2VGBILGEO";
                R = " UPDATE TECHNOLOG_VVV..RUSM SET POL =N'" + P102 + "' WHERE ID=" + K102;
                command = new SqlCommand(R, conbase01);
                conbase01.Open();
                command.CommandTimeout = 1200;
                command.ExecuteNonQuery();
                conbase01.Close();
            }

            //CONTROL101(IDSession, IDMt); // Проверка поля Языки

            OBR98(IDSession, IDMt, "n"); // ФОРМИРОВАНИЕ ПОЛЯ 801 - Источник записи
            //=========================================================================================================Саша, здесь RUSM сформирована=======================================

            MERGERUSM(IDSession); // Создание таблицы RUSM2

            if (P200z.Length > 0) // Языки параллельных заглавий всегда в конце поля 200
            {
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.CommandText = " SELECT ID,POL "
                + " FROM TECHNOLOG_VVV..RUSM2 "
                + " WHERE IDMAIN=" + IDMt.ToString() + " and session='" + IDSession + "'"
                + " AND MET=200";
                da.SelectCommand.Connection = conbase01;
                da.SelectCommand.CommandTimeout = 1200;
                DataSet ds200 = new DataSet();
                int K200 = da.Fill(ds200);
                if (K200 > 0)
                {
                    string st200 = ds200.Tables[0].Rows[0][0].ToString();
                    string S200 = ds200.Tables[0].Rows[0][1].ToString().Replace("'", "~") + (char)31 + "z" + P200z;
                    R = " UPDATE TECHNOLOG_VVV..RUSM2 SET POL =N'" + S200 + "' WHERE ID=" + st200;
                    command = new SqlCommand(R, conbase01);
                    conbase01.Open();
                    command.CommandTimeout = 1200;
                    command.ExecuteNonQuery();
                    conbase01.Close();
                }
            }

            if (CONTROL_OBYAZ(IDSession) < 0) continue; ;

            Byte[] b_rez = SaveZap(IDSession, IDMt); // Формирование РУСМАРК-записи в виде массива байтов (UTF-8)
            using (BinaryWriter binWriter =
                     new BinaryWriter(File.Open(FNout, FileMode.Append, FileAccess.Write)))
            {
                binWriter.Write(b_rez);
            }
            //USE [BJRUSMARC]
            //****** Object:  Table [dbo].[LIBNETNEB]    Script Date: 10/27/2015 13:04:51 ******/
            //CREATE TABLE [dbo].[LIBNETNEB](
            //    [ID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
            //    [BaZa] [char](1) NULL,
            //    [PIN] [int] NULL,
            //    [DateBase] [datetime] NULL,
            //    [notload] [varchar](50) NULL,
            //    [del] [bit] NULL,
            //    [IDtrans] [int] NULL,
            //    [ImyaF] [varchar](50) NULL,
            //    [ImyaFNEB] [varchar](50) NULL,
            //    [Timestamp] [timestamp] NULL,
            //    [marker5] [char](1) NULL,
            // CONSTRAINT [PK_LIBNETNEB] PRIMARY KEY CLUSTERED 
            //(
            //    [ID] ASC
            //)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
            //) ON [PRIMARY]
            //ALTER TABLE [dbo].[LIBNETNEB] ADD  DEFAULT ('n') FOR [marker5]

            nomzap++;
            R = "INSERT INTO BJRUSMARC..LIBNETNEB (BaZa,PIN,DateBase,notload,del,IDtrans,ImyaF,marker5)"
            + " VALUES ('" + PREFIX001 + "'," + IDMt.ToString() + ",CONVERT(DateTime,'" + Date_Base + "',121),'',0,"
            + nomzap + ",'" + FS + "','n')";
            command = new SqlCommand(R, conbase03);
            conbase03.Open();
            command.CommandTimeout = 1200;
            command.ExecuteNonQuery();
            conbase03.Close();

        } // цикл по IDM
        dst.Dispose();
        if (PREFIX001.Equals("V")) Console.Write("Из BJVVV выгружено " + K.ToString() + " записей n");
        else Console.Write("Из REDKOSTJ выгружено " + K.ToString() + " записей n");
    }// FormFileN()

    private void FormFileC()
    {
        //Изменены после передачи
        da.SelectCommand = new SqlCommand();
        da.SelectCommand.CommandText =
            " DECLARE @DAT datetime; SET @DAT = CONVERT(datetime,'" + dt1 + "',121);" // Дата последней выгруженной базы
            + " SELECT DISTINCT DE.IDMAIN FROM "+BAZA+"..DATAEXT DE "
            + " INNER JOIN BJVVV..DATA D ON D.ID=DE.IDDATA "
            + " WHERE DE.IDMAIN IN (SELECT DISTINCT PIN as IDMAIN FROM BJRUSMARC..LIBNETNEB WHERE notload='' AND del=0 AND BaZa='V')"
            + " AND DE.Changed>=@DAT and DE.Changed is not NULL and DATEDIFF(day,DE.Changed,DE.Created) != 0"
            + " AND IDBLOCK<260 ";
        da.SelectCommand.Connection = conbase03;
        da.SelectCommand.CommandTimeout = 1200;
        DataSet dst = new DataSet();
        Int32 K = da.Fill(dst);
        Byte[] str_out = new Byte[0];
        //K = 2400;
        for (int countM = 0; countM < K; countM++)
        {
            command = new SqlCommand("DELETE TECHNOLOG_VVV..RUSM; DELETE TECHNOLOG_VVV..RUSM2;", conbase01);
            conbase01.Open();
            command.CommandTimeout = 1200;
            command.ExecuteNonQuery();
            conbase01.Close();

            int IDMt = Int32.Parse(dst.Tables[0].Rows[countM][0].ToString());
            //int IDMt = 1223123;
            PBJ2RUSM(IDSession, IDMt, "c"); // Создание таблицы RUSM

            da.SelectCommand = new SqlCommand();
            da.SelectCommand.CommandText = " SELECT ID,POL "
            + " FROM TECHNOLOG_VVV..RUSM "
            + " WHERE IDMAIN=" + IDMt.ToString() + " and session='" + IDSession + "'"
            + " AND MET=102";
            da.SelectCommand.Connection = conbase01;
            da.SelectCommand.CommandTimeout = 1200;
            DataSet dsZ = new DataSet();
            int K102 = da.Fill(dsZ);
            if (K102 > 0)
            {
                K102 = Int32.Parse(dsZ.Tables[0].Rows[0]["ID"].ToString());
                P102 = dsZ.Tables[0].Rows[0]["POL"].ToString().Replace("'", "~") + (char)31 + "2VGBILGEO";
                R = " UPDATE TECHNOLOG_VVV..RUSM SET POL =N'" + P102 + "' WHERE ID=" + K102;
                command = new SqlCommand(R, conbase01);
                conbase01.Open();
                command.CommandTimeout = 1200;
                command.ExecuteNonQuery();
                conbase01.Close();
            }
            OBR98(IDSession, IDMt, "c"); // ФОРМИРОВАНИЕ ПОЛЯ 801 - Источник записи
            MERGERUSM(IDSession); // Создание таблицы RUSM2

            if (P200z.Length > 0) // Языки параллельных заглавий всегда в конце поля 200
            {
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.CommandText = " SELECT ID,POL "
                + " FROM TECHNOLOG_VVV..RUSM2 "
                + " WHERE IDMAIN=" + IDMt.ToString() + " and session='" + IDSession + "'"
                + " AND MET=200";
                da.SelectCommand.Connection = conbase01;
                da.SelectCommand.CommandTimeout = 1200;
                DataSet ds200 = new DataSet();
                int K200 = da.Fill(ds200);
                if (K200 > 0)
                {
                    string st200 = ds200.Tables[0].Rows[0][0].ToString();
                    string S200 = ds200.Tables[0].Rows[0][1].ToString().Replace("'", "~") + (char)31 + "z" + P200z;
                    R = " UPDATE TECHNOLOG_VVV..RUSM2 SET POL =N'" + S200 + "' WHERE ID=" + st200;
                    command = new SqlCommand(R, conbase01);
                    conbase01.Open();
                    command.CommandTimeout = 1200;
                    command.ExecuteNonQuery();
                    conbase01.Close();
                }
            }

            if (CONTROL_OBYAZ(IDSession) < 0) continue; ;

            Byte[] b_rez = SaveZap(IDSession, IDMt); // Формирование РУСМАРК-записи в виде массива байтов (UTF-8)

            using (BinaryWriter binWriter =
                     new BinaryWriter(File.Open(FNout, FileMode.Append, FileAccess.Write)))
            {
                binWriter.Write(b_rez);
            }

            nomzap++;
            R = "INSERT INTO BJRUSMARC..LIBNETNEB (BaZa,PIN,DateBase,notload,del,IDtrans,ImyaF,marker5)"
           + " VALUES ('" + PREFIX001 + "'," + IDMt.ToString() + ",CONVERT(DateTime,'" + Date_Base + "',121),'',0,"
           + nomzap + ",'" + FS + "','c')";
            command = new SqlCommand(R, conbase03);
            conbase03.Open();
            command.CommandTimeout = 1200;
            command.ExecuteNonQuery();
            conbase03.Close();
        } // цикл по IDM
        dst.Dispose();
        if (PREFIX001.Equals("V")) Console.Write("Из BJVVV выгружено " + K.ToString() + " записей c");
        else Console.Write("Из REDKOSTJ выгружено " + K.ToString() + " записей c");
    }// FormFileC()

    private void FormFileD()
    {
        // Удалены
        da.SelectCommand = new SqlCommand();
        da.SelectCommand.CommandText =
             "SELECT DISTINCT IDMAIN FROM " + BAZA + "..DATAEXT WHERE "
              + " SORT = 'Списано'  AND MNFIELD=921 AND MSFIELD='$c' "
             + " AND IDMAIN NOT IN (SELECT DISTINCT IDMAIN FROM  " + BAZA + "..DATAEXT "
                + " WHERE SORT = 'Длявыдачи'  AND MNFIELD=921 AND MSFIELD='$c') "
             + " AND IDMAIN IN ( "
              + " SELECT DISTINCT PIN as IDMAIN FROM BJRUSMARC..LIBNETNEB WHERE notload='' "
               + " AND del=0 AND BaZa='" + PREFIX001 + "' AND marker5='n')";
        da.SelectCommand.Connection = conbase03;
        da.SelectCommand.CommandTimeout = 1200;
        DataSet dst = new DataSet();
        Int32 K = da.Fill(dst);
        Byte[] str_out = new Byte[0];
        //K = 2400;
        for (int countM = 0; countM < K; countM++)
        {
            command = new SqlCommand("DELETE TECHNOLOG_VVV..RUSM; DELETE TECHNOLOG_VVV..RUSM2;", conbase01);
            conbase01.Open();
            command.CommandTimeout = 1200;
            command.ExecuteNonQuery();
            conbase01.Close();

            int IDMt = Int32.Parse(dst.Tables[0].Rows[countM][0].ToString());

            INITMARKER(IDSession, IDMt, "d", out IDLEVEL);
            //-- Идентификатор записи 
            P001 = PREFIX001 + IDMt.ToString();

            R = " INSERT INTO TECHNOLOG_VVV..RUSM (session,IDMAIN,IDBLOCK,MET,IND1,IND2,IDENT,POL) "
             + " VALUES ('" + IDSession + "'," + IDMt.ToString() + ",0,001,0,0,0,'" + P001 + "')";
            command = new SqlCommand(R, conbase01);
            conbase01.Open();
            command.CommandTimeout = 1200;
            command.ExecuteNonQuery();
            conbase01.Close();

            MERGERUSM(IDSession); // Создание таблицы RUSM2

            Byte[] b_rez = SaveZap(IDSession, IDMt); // Формирование РУСМАРК-записи в виде массива байтов (UTF-8)
            using (BinaryWriter binWriter =
                     new BinaryWriter(File.Open(FNout, FileMode.Append, FileAccess.Write)))
            {
                binWriter.Write(b_rez);
            }

            R = "UPDATE BJRUSMARC..LIBNETNEB SET del=1 where BaZa='" + PREFIX001
                + "' AND PIN=" + IDMt.ToString();
            command = new SqlCommand(R, conbase03);
            conbase03.Open();
            command.CommandTimeout = 1200;
            command.ExecuteNonQuery();
            conbase03.Close();

            nomzap++;

            R = "INSERT INTO BJRUSMARC..LIBNETNEB (BaZa,PIN,DateBase,notload,IDtrans,ImyaF,marker5,del)"
+ " VALUES ('" + PREFIX001 + "'," + IDMt.ToString() + ",CONVERT(DateTime,'" + Date_Base + "',121),'',"
+ nomzap + ",'" + FS + "','d',1)";
            command = new SqlCommand(R, conbase03);
            conbase03.Open();
            command.CommandTimeout = 1200;
            command.ExecuteNonQuery();
            conbase03.Close();
        } // цикл по IDM
        dst.Dispose();
        if (PREFIX001.Equals("V")) Console.Write("Из BJVVV выгружено " + K.ToString() + " записей d");
        else Console.Write("Из REDKOSTJ выгружено " + K.ToString() + " записей d");
    }// FormFileD()




    public int FormRUSM(int IDMAIN)
    {

        command = new SqlCommand("DELETE TECHNOLOG_VVV..RUSM;", conbase01);
        conbase01.Open();
        command.CommandTimeout = 1200;
        command.ExecuteNonQuery();
        conbase01.Close();

        int IDMt = IDMAIN;
        //int IDMt = 1202423;
        da.SelectCommand = new SqlCommand();
        da.SelectCommand.CommandText = " SELECT ID FROM  " + BAZA + "..DATAEXT "
        + " WHERE IDMAIN=" + IDMt.ToString() + " and MNFIELD=921 AND MSFIELD='$b' AND SORT='Изданиекартографическое'";
        da.SelectCommand.Connection = conbase03;
        da.SelectCommand.CommandTimeout = 1200;
        DataSet ds921 = new DataSet();
        int K921 = da.Fill(ds921);
        ds921.Dispose();
        if (K921 > 0)
        {
            return 1; // Карты не обрабатываются
        }
        PBJ2RUSM(IDSession, IDMt, "n"); // Создание таблицы RUSM

        da.SelectCommand = new SqlCommand();
        da.SelectCommand.CommandText = " SELECT ID,POL "
        + " FROM TECHNOLOG_VVV..RUSM "
        + " WHERE IDMAIN=" + IDMt.ToString() + " and session='" + IDSession + "'"
        + " AND MET=102";
        da.SelectCommand.Connection = conbase01;
        da.SelectCommand.CommandTimeout = 1200;
        DataSet dsZ = new DataSet();
        int K102 = da.Fill(dsZ);
        if (K102 > 0)
        {
            K102 = Int32.Parse(dsZ.Tables[0].Rows[0]["ID"].ToString());
            P102 = dsZ.Tables[0].Rows[0]["POL"].ToString().Replace("'", "~") + (char)31 + "2VGBILGEO";
            R = " UPDATE TECHNOLOG_VVV..RUSM SET POL =N'" + P102 + "' WHERE ID=" + K102;
            command = new SqlCommand(R, conbase01);
            conbase01.Open();
            command.CommandTimeout = 1200;
            command.ExecuteNonQuery();
            conbase01.Close();
        }

        //CONTROL101(IDSession, IDMt); // Проверка поля Языки

        OBR98(IDSession, IDMt, "n"); // ФОРМИРОВАНИЕ ПОЛЯ 801 - Источник записи
        //=========================================================================================================Саша, здесь RUSM сформирована=======================================
        return 0;




        // цикл по IDM

    }// FormRUSM()

}

