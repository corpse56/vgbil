using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;

namespace Circulation.Classes
{
    public static class Utils
    {
        public static string DataTableToCSV(this DataTable datatable, char seperator, ProgressBar pb)
        {
            pb.Visible = true;
            pb.Maximum = datatable.Rows.Count;
            pb.Value = 0;

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < datatable.Columns.Count; i++)
            {
                sb.Append(datatable.Columns[i]);
                sb.Append(seperator);
                //if (i < datatable.Columns.Count - 1)
                //    sb.Append(seperator);
            }
            sb.AppendLine();
            foreach (DataRow dr in datatable.Rows)
            {
                pb.Value++;
                Application.DoEvents();
                for (int i = 0; i < datatable.Columns.Count; i++)
                {
                    sb.Append(dr[i].ToString().Replace(";", " "));
                    sb.Append(seperator);
                    //if (i < datatable.Columns.Count - 1)
                    //    sb.Append(seperator);
                }
                sb.AppendLine();
            }
            pb.Visible = false;
            return sb.ToString();
        }
    }
}
