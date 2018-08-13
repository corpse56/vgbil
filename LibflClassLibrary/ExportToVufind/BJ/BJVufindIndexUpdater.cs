using LibflClassLibrary.Books.BJBooks.DB;
using LibflClassLibrary.ExportToVufind.Vufind;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

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
                sb.AppendFormat("{0}_{1}", this.Fund, row["IDMAIN"].ToString());
                Increment.Add(new IncrementStruct("updated", sb.ToString()));
            }

            table = dbWrapper.GetIncrementDeleted();
            foreach (DataRow row in table.Rows)
            {
                sb.AppendFormat("{0}_{1}", this.Fund, row["IDMAIN"].ToString());
                Increment.Add(new IncrementStruct("deleted", sb.ToString()));
            }


            return new object();
        }

        public override object GetCurrentIncrementDeleted()
        {
            throw new NotImplementedException();
        }
    }
    class IncrementStruct
    {
        public IncrementStruct(string Flag, string Id)
        {
            this.Flag = Flag;
            this.Id = Id;
        }
        string Flag;
        string Id;
    }
}