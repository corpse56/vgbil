using LibflClassLibrary.Books.BJBooks.BJExemplars;
using LibflClassLibrary.Books.BJBooks.DB;
using LibflClassLibrary.ExportToVufind.Vufind;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LibflClassLibrary.Books.BJBooks.Loaders
{
    public class BJBookLoader
    {
        private string Fund { get; set; }
        private BJDatabaseWrapper dbWrapper;
        public BJBookLoader(string Fund)
        {
            this.Fund = Fund;
            dbWrapper = new BJDatabaseWrapper(Fund);
        }

        public int GetMaxIDMAIN()
        {
            DataTable table = dbWrapper.GetMaxIDMAIN();
            return int.Parse(table.Rows[0][0].ToString());

        }

        public DataTable GetBJRecord(int IDRecord)
        {
            DataTable table = dbWrapper.GetBJRecord(IDRecord);
            return table;
        }

        public int GetParentIDMAIN(int ParentPIN)
        {
            DataTable table = dbWrapper.GetParentIDMAIN(ParentPIN);
            if (table.Rows.Count == 0)
            {
                return ParentPIN;
            }
            ParentPIN = Convert.ToInt32(table.Rows[0]["SORT"]);
            return GetParentIDMAIN(ParentPIN);
        }

        internal AuthoritativeFile GetAFAll(int AFLinkId, string AFTable)
        {
            AuthoritativeFile result = new AuthoritativeFile();
            DataTable table = dbWrapper.GetAFAllValues(AFTable, AFLinkId);
            foreach (DataRow r in table.Rows)
            {
                result.Add(r["PLAIN"].ToString());
            }
            return result;
        }

        internal BJBookInfo GetBookInfoByPIN(int pin)
        {
            DataTable table = dbWrapper.GetBJRecord(pin);
            BJBookInfo result = new BJBookInfo();
            result.Id = $"{Fund}_{pin}";
            result.ID = pin;
            result.Fund = Fund;
            //BJExemplarInfo exemplar = new BJExemplarInfo(0);
            int CurrentIdData = 0;
            foreach (DataRow row in table.Rows)
            {
                if (row["IDBLOCK"] == DBNull.Value) continue;
                if ((int)row["IDBLOCK"] != 260)
                {
                    if ((int)row["IDBLOCK"] == 270)//если есть гиперссылка
                    {
                        result.DigitalCopy = new BJElectronicExemplarInfo(pin, Fund);
                    }
                    else
                    {
                        result.Fields.AddField(row["PLAIN"].ToString(), (int)row["MNFIELD"], row["MSFIELD"].ToString());
                        if ((int)row["MNFIELD"] == 200 && row["MSFIELD"].ToString() == "$a")
                        {
                            result.Title = row["PLAIN"].ToString();
                        }
                        if ((int)row["MNFIELD"] == 700 && row["MSFIELD"].ToString() == "$a" && string.IsNullOrWhiteSpace(result.Author))
                        {
                            result.Author = row["PLAIN"].ToString();
                        }
                    }
                }
                else
                {
                    if (CurrentIdData != (int)row["IDDATA"])
                    {
                        CurrentIdData = (int)row["IDDATA"];
                        result.Exemplars.Add(BJExemplarInfo.GetExemplarByIdData(CurrentIdData, Fund));
                        //exemplar = new BJExemplarInfo((int)row["IDDATA"]);
                        //exemplar.Fields.AddField(row["PLAIN"].ToString(), (int)row["MNFIELD"], row["MSFIELD"].ToString());
                    }
                    else
                    {
                        //exemplar.Fields.AddField(row["PLAIN"].ToString(), (int)row["MNFIELD"], row["MSFIELD"].ToString());
                    }
                }
            }
            table = dbWrapper.GetRTF(pin);
            if (table.Rows.Count != 0)
            {
                RichTextBox rtb = new RichTextBox();
                rtb.Rtf = table.Rows[0][0].ToString();
                result.RTF = rtb.Text;
                rtb.Dispose();
            }
            return result;
        }

        internal BJBookInfo GetBookByInventoryNumber(string inv)
        {
            BJDatabaseWrapper dbw = new BJDatabaseWrapper("");
            DataTable table = dbw.GetExemplarIdByInventoryNumber(inv);
            if (table.Rows.Count == 0) return null;
            string bookId = table.Rows[0]["exemplarId"].ToString();
            string fund = table.Rows[0]["fund"].ToString().ToUpper();
            BJBookInfo result = BJBookInfo.GetBookInfoByPIN($"{fund}_{bookId}");
            return result;
        }

        internal string Clarify_10a(int IDData, string PLAIN)
        {
            DataTable clarify = dbWrapper.Clarify_10a(IDData);
            string add;
            if (clarify.Rows.Count != 0)
            {
                add = $"{PLAIN}({clarify.Rows[0]["PLAIN"].ToString()})";
            }
            else
            {
                add = PLAIN;
            }
            return add;
        }

        internal string Clarify_101a(int IDINLIST, string PLAIN)
        {
            DataTable clarify = dbWrapper.Clarify_101a(IDINLIST);
            if (clarify.Rows.Count == 0)
            {
                return PLAIN;
            }
            else
            {
                return clarify.Rows[0]["NAME"].ToString();
            }
        }

        internal string Clarify_517a(int IDDATA, string PLAIN)
        {
            DataTable clarify = dbWrapper.Clarify_517a(IDDATA);
            string fieldValue;
            fieldValue = (clarify.Rows.Count != 0) ?
                $"({clarify.Rows[0]["PLAIN"].ToString()}){PLAIN}" :
                PLAIN;
            return fieldValue;
            //нужно специальным образом обрабатывать
        }

        internal string Clarify_205a(int IDDATA, string PLAIN)
        {
            string PublicationInfo = PLAIN;

            // 205$b
            DataTable clarify = dbWrapper.Clarify_205a_1(IDDATA);
            foreach (DataRow rr in clarify.Rows)
            {
                PublicationInfo += $";{rr["PLAIN"].ToString()}";
            }
            // 205$f
            clarify = dbWrapper.Clarify_205a_2(IDDATA);
            foreach (DataRow rr in clarify.Rows)
            {
                PublicationInfo += $" /{rr["PLAIN"].ToString()}";
            }
            // 205$g
            clarify = dbWrapper.Clarify_205a_3(IDDATA);
            foreach (DataRow rr in clarify.Rows)
            {
                PublicationInfo += $"; {rr["PLAIN"].ToString()}";
            }
            return PublicationInfo;
        }

        internal string Clarify_606a(int IDChain)
        {
            DataTable clarify = dbWrapper.Clarify_606a(IDChain);
            if (clarify.Rows.Count == 0)
            {
                return null;
            }
            string TPR = "";
            foreach (DataRow rr in clarify.Rows)
            {
                TPR += $"{rr["VALUE"].ToString()} • ";
            }
            TPR = TPR.Substring(0, TPR.Length - 2);
            return TPR;
        }

        internal string GetFieldValue(int iDMAIN, int mNFIELD, string mSFIELD)
        {
            DataTable table = dbWrapper.GetFieldValue(iDMAIN, mNFIELD, mSFIELD);
            return (table.Rows.Count == 0) ?  string.Empty : table.Rows[0][0].ToString();
        }

        internal DataTable GetAllIdmainWithImages()
        {
            return dbWrapper.GetAllIdmainWithImages();
        }


        internal DataTable GetIdDataOfAllExemplars(int idmain)
        {
            return dbWrapper.GetIdDataOfAllExemplars(idmain);
        }

        internal bool IsElectronicCopyExists(int idmain)
        {
            DataTable table = dbWrapper.GetHyperLink(idmain);
            DataTable table1 = dbWrapper.GetBookScanInfo(idmain);
            return (table.Rows.Count == 1 && table1.Rows.Count == 1) ? true : false;
        }

        internal DataTable GetImage(int IDMAIN)
        {
            return dbWrapper.GetImage(IDMAIN);
        }

        internal DataTable GetExemplar(int IDDATA)
        {
            return dbWrapper.GetExemplar(IDDATA);
        }

        internal DataTable GetHyperLink(int IDMAIN)
        {
            return dbWrapper.GetHyperLink(IDMAIN);
        }

        internal DataTable GetBookScanInfo(int IDMAIN)
        {
            return dbWrapper.GetBookScanInfo(IDMAIN);
        }

        internal string GetTitle(int IDMAIN)
        {
            DataTable table = dbWrapper.GetTitle(IDMAIN);
            return (table.Rows.Count != 0) ? table.Rows[0]["PLAIN"].ToString() : null;
        }

        public BJElectronicExemplarAvailabilityCodes GetElectronicExemplarAccessLevel(int IDMAIN, int IDProject)
        {
            DataTable table = dbWrapper.GetElectronicExemplarAccessLevel(IDMAIN, IDProject);
            if (table.Rows.Count == 0)
            {
                throw new Exception("В таблице BookProject нет такой записи!");
            }
            switch (table.Rows[0]["CodeTypeProject"].ToString())
            {
                case "v-stop":
                    return BJElectronicExemplarAvailabilityCodes.vstop;
                case "v-free-view":
                    return BJElectronicExemplarAvailabilityCodes.vfreeview;
                case "v-login-view":
                    return BJElectronicExemplarAvailabilityCodes.vloginview;
                case "dlstop":
                    return BJElectronicExemplarAvailabilityCodes.dlstop;
                case "dlopen":
                    return BJElectronicExemplarAvailabilityCodes.dlopen;
                case "dlview":
                    return BJElectronicExemplarAvailabilityCodes.dlview;
                case "dllimit":
                    return BJElectronicExemplarAvailabilityCodes.dllimit;

            }
            return BJElectronicExemplarAvailabilityCodes.vstop;
        }


        internal BJBookInfo GetBJBookByBar(string data)
        {
            //здесь ищем по всем базам. просто передаём BJVVV, но запрашивать будем все базы по очереди.

            DataTable table = dbWrapper.GetBJBookByBar(data);
            if (table.Rows.Count > 1) throw new Exception("Штрихкодов в базах найдено более одного.");
            if (table.Rows.Count == 0) return null;
            BJBookInfo book = BJBookInfo.GetBookInfoByPIN(Convert.ToInt32(table.Rows[0]["IDMAIN"]), table.Rows[0]["fund"].ToString());
            return book;
        }



    }
}
