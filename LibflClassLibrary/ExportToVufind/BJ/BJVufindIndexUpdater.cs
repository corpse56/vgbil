using LibflClassLibrary.Books.BJBooks.DB;
using LibflClassLibrary.ExportToVufind.Vufind;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Utilities;

namespace LibflClassLibrary.ExportToVufind.BJ
{
    public class BJVufindIndexUpdater : VufindIndexUpdater
    {
        private string Fund { get; set; }

        public BJVufindIndexUpdater(string Host, string Fund) : base(Host)
        {
            this.Fund = Fund;
        }
        public override object GetCurrentIncrement()
        {
            BJDatabaseWrapper dbWrapper = new BJDatabaseWrapper(this.Fund);
            List<IncrementStruct> Increment = new List<IncrementStruct>();

            StringBuilder sb = new StringBuilder();

            DataTable table = dbWrapper.GetIncrementUpdate();
            foreach (DataRow row in table.Rows)
            {
                sb.Clear();
                sb.AppendFormat("{0}_{1}", this.Fund, row["IDMAIN"].ToString());
                Increment.Add(new IncrementStruct("updated", sb.ToString()));
                //Debug.Assert(sb.Length < 7, row["IDMAIN"].ToString());
            }

            table = dbWrapper.GetIncrementDeleted();
            foreach (DataRow row in table.Rows)
            {
                sb.Clear();
                sb.AppendFormat("{0}_{1}", this.Fund, row["IDMAIN"].ToString());
                Increment.Add(new IncrementStruct("deleted", sb.ToString()));
            }

            table = dbWrapper.GetIncrementCovers();
            foreach (DataRow row in table.Rows)
            {
                sb.Clear();
                sb.AppendFormat("{0}_{1}", this.Fund, row["IDMAIN"].ToString());
                Increment.Add(new IncrementStruct("cover", sb.ToString()));
            }



            return Increment;
        }

        public override object GetCurrentIncrementDeleted()
        {
            throw new NotImplementedException();
        }
    }
    public class IncrementStruct
    {
        public IncrementStruct(string Flag, string Id)
        {
            this.Flag = Flag;
            this.Id = Id;
        }
        public string Flag;//updated или deleted
        public string Id;
    }
}