using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using Microsoft.Office.Interop.Excel;

namespace ExportFullDatabaseToExcel
{
    class Program
    {
        static void Main(string[] args)
        {
            Excel.Workbook wb;
            Excel.Worksheet ws;
            Excel.Application excel = new Excel.Application();
            excel.Visible = true;
            wb = excel.Workbooks.Add(Excel.XlWBATemplate.xlWBATWorksheet);
            ws = (Excel.Worksheet)wb.Worksheets[1];

            SqlConnection connection = new SqlConnection("Data Source=192.168.4.25,1443;Initial Catalog=MKO;Persist Security Info=True;User ID=Sasha;Password=Corpse536");
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            da.SelectCommand = new SqlCommand();
            da.SelectCommand.Connection = connection;
            da.SelectCommand.CommandText = "SELECT Distinct TABLE_NAME FROM information_schema.TABLES";
            da.Fill(ds, "t");
            foreach (DataRow r in ds.Tables["t"].Rows)
            {
                ws = (Worksheet)wb.Sheets.Add(Missing.Value, wb.Sheets[wb.Sheets.Count], Missing.Value, Missing.Value);
                ws.Name = r["TABLE_NAME"].ToString();
                da.SelectCommand.CommandText = "SELECT COLUMN_NAME FROM MKO.INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = '" + ws.Name + "'";
                if (ds.Tables["f"] != null)
                {
                    ds.Tables.Remove("f");
                }
                da.Fill(ds, "f");
                //foreach (DataRow rf in ds.Tables["f"].Rows)
                {
                    da.SelectCommand.CommandText = "SELECT * from MKO.."+ws.Name;
                    if (ds.Tables["d"] != null)
                    {
                        ds.Tables.Remove("d");
                    }
                    da.Fill(ds, "d");
                    int j = 1, i = 2;
                    foreach (DataRow rd in ds.Tables["d"].Rows)
                    {
                        j = 1;
                        foreach (DataColumn dc in ds.Tables["d"].Columns)
                        {
                            if (i == 2)
                            {
                                ws.Cells[1, j] = dc.ColumnName;
                            }
                            //j++;
                            ws.Cells[i, j++] = rd[dc.ColumnName].ToString();
                        }
                        i++;
                    }
                }
            }


            ws = (Excel.Worksheet)wb.Worksheets[1];
            ws.Delete();
            
            
            
            
            //ws.Name = "План-график";




            //ws.Cells[currentRow, 1] = row["NN"];
            //ws.Cells[currentRow, 2] = row["CNAME"];
            //ws.Cells[currentRow, 3] = row["IDS"];
            //ws.Cells[currentRow, 4] = row["product"];
            //ws.Cells[currentRow, 5] = row["QUANTITY"];


//            insert into 
//OPENROWSET('Microsoft.ACE.OLEDB.12.0', 'Excel 12.0;Database=F:\network\mko.xlsx','select * from [1$]')
//select * from MKO..AFORGS

//use MKO
//go
//SELECT Distinct TABLE_NAME FROM information_schema.TABLES


//SELECT COLUMN_NAME FROM MKO.INFORMATION_SCHEMA.COLUMNS
//where TABLE_NAME = 'AFORGS'


        }
    }
}
