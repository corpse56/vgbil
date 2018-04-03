using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using XMLConnections;
using System.IO;
using System.Net;

namespace DgtPeriodicBackup
{
    class Program
    {
        static void Main(string[] args)
        {
            //DateTime backupdate = GetLastBackUpDate();
            string[] PINsToBackup;
            StreamWriter fs;
            Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory + "_log.txt");
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "_log.txt"))
            {
                fs = File.AppendText(AppDomain.CurrentDomain.BaseDirectory + "_log.txt");
            }
            else
            {
                fs = File.CreateText(AppDomain.CurrentDomain.BaseDirectory + "_log.txt");
            }
            fs.WriteLine("=======================================================================");
            fs.WriteLine("=======================================================================");
            fs.WriteLine("Начало резервного копирования периодики");
            fs.WriteLine(DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString());
            fs.WriteLine();

            string outside_ip;
            string inside_ip;
            DirectoryInfo TargetFolderOutside;
            DirectoryInfo TargetFolderInside;
            NetworkConnection nc1 = null;
            NetworkConnection nc2 = null;


            try
            {
                PINsToBackup = GetPINS();
                outside_ip = @"\\" + XmlConnections.GetConnection("/Connections/outside_ip") + @"\Backup\BookAddInf\PERIOD\";
                inside_ip = @"\\" + XmlConnections.GetConnection("/Connections/inside_ip") + @"\BookAddInf\PERIOD\";

                nc1 = new NetworkConnection(outside_ip, new NetworkCredential(@"bj\CopyPeriodAddInf", "Period_Copy"));
                //nc2 = new NetworkConnection(inside_ip, new NetworkCredential(@"libfl\PeriodicUploadUser", "PeriodicUploadUser_2015"));
                nc2 = new NetworkConnection(inside_ip, new NetworkCredential(@"libfl\CopyBookAddInf", "Book_Copy"));



                for (int i = 0; i < PINsToBackup.Length; i++)
                {
                    Console.WriteLine("ПИН "+(i+1).ToString()+" из "+PINsToBackup.Length);

                    TargetFolderOutside = new DirectoryInfo(outside_ip + PINsToBackup[i]);
                    TargetFolderInside = new DirectoryInfo(inside_ip + PINsToBackup[i]);

                    if (!Directory.Exists(TargetFolderOutside.FullName))
                    {
                        TargetFolderOutside.Create();
                    }
                    else
                    {
                        TargetFolderOutside.Delete(true);
                        TargetFolderOutside.Create();
                    }
                    if (!Directory.Exists(TargetFolderInside.FullName))
                    {
                        TargetFolderInside.Create();
                    }
                    else
                    {
                        TargetFolderInside.Delete(true);
                        TargetFolderInside.Create();
                    }

                    DirectoryInfo di = new DirectoryInfo(@"E:\BookAddInf\PERIOD\" + PINsToBackup[i]);

                    DirectoryInfo[] subdi = di.GetDirectories();
                    FileInfo[] infotxt = di.GetFiles();
                    if (infotxt.Length != 0)
                    {
                        fs.Write("Копирую: " + infotxt[0].FullName);
                        Console.Write("Копирую: " + infotxt[0].FullName);
                        //infotxt[0].CopyTo(TargetFolderOutside.FullName + infotxt[0].Name);
                        infotxt[0].CopyTo(TargetFolderInside.FullName + infotxt[0].Name);
                        fs.WriteLine(". Успех");
                        Console.WriteLine(". Успех");
                    }
                    foreach (DirectoryInfo d in subdi)
                    {
                        FileInfo[] fi = d.GetFiles();
                        foreach (FileInfo f in fi)
                        {
                            if (!Directory.Exists(TargetFolderOutside.FullName + f.Directory.Name))
                            {
                                Directory.CreateDirectory(TargetFolderOutside.FullName + f.Directory.Name);
                            }
                            if (!Directory.Exists(TargetFolderInside.FullName + f.Directory.Name))
                            {
                                Directory.CreateDirectory(TargetFolderInside.FullName + f.Directory.Name);
                            }
                            fs.Write("Копирую: " + f.FullName);
                            Console.Write("Копирую: " + f.FullName);
                            
                            //f.CopyTo(TargetFolderOutside.FullName + f.Directory.Name+"\\"+f.Name);
                            f.CopyTo(TargetFolderInside.FullName + f.Directory.Name + "\\" + f.Name);
                            fs.WriteLine(". Успех");
                            Console.WriteLine(". Успех");
                        }
                    }



                }
                fs.WriteLine();

                UpdateLastBackupDate();
                if (nc1 != null)
                    nc1.Dispose();
                if (nc2 != null)
                    nc2.Dispose();

            }
            catch (Exception ex)
            {
                fs.WriteLine();
                fs.WriteLine("Ошибка: " + ex.Message);
                fs.WriteLine();
                fs.WriteLine("Неуспешное окончание резервного копирования");
                fs.WriteLine(DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString());
                fs.WriteLine("=======================================================================");
                fs.WriteLine("=======================================================================");
                fs.Flush();
                fs.Close();
                return;
            }
            finally
            {
                if (nc1 != null)
                    nc1.Dispose();
                if (nc2 != null)
                    nc2.Dispose();
            }



            fs.WriteLine("Успешное окончание резервного копирования");
            fs.WriteLine(DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString());
            fs.WriteLine("=======================================================================");
            fs.WriteLine("=======================================================================");

            fs.Flush();
            fs.Close();
            if (nc1 != null)
                nc1.Dispose();
            if (nc2 != null)
                nc2.Dispose();
            //Console.ReadKey();
        }

        private static string[] GetPINS()
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            da.SelectCommand = new SqlCommand();
            da.SelectCommand.Connection = new SqlConnection();
            da.SelectCommand.Connection.ConnectionString = XmlConnections.GetConnection("/Connections/BJVVV");
            da.SelectCommand.CommandText = "  select * " +
                                                "FROM [BookAddInf].[dbo].[DgtPeriodic] A " +
                                                "where A.[DATEADD]>(select BACKUPLASTDATE from BookAddInf..[BackupPeriodic] where ID=1) " +
                                                "or A.CHANGED>(select BACKUPLASTDATE from BookAddInf..[BackupPeriodic] where ID=1)";
            if (da.Fill(ds, "t") == 0)
            {
                throw new Exception("Нечего бэкапить!");
            }
            string[] result = new string[ds.Tables["t"].Rows.Count];
            for (int i = 0; i < ds.Tables["t"].Rows.Count; i++)
            {
                result[i] = PINFormat(ds.Tables["t"].Rows[i]["PIN"].ToString()) + ds.Tables["t"].Rows[i]["YEAR"] + @"\";
            }
            /*result = new string[2];
            result[0] = PINFormat("191") + @"2013\";
            result[1] = PINFormat("192") + @"2013\";*/
            return result;
        }

        private static string PINFormat(string pin)
        {
            switch (pin.Length)
            {
                case 1:
                    pin = "000000" + pin;
                    break;
                case 2:
                    pin = "00000" + pin;
                    break;
                case 3:
                    pin = "0000" + pin;
                    break;
                case 4:
                    pin = "000" + pin;
                    break;
                case 5:
                    pin = "00" + pin;
                    break;
                case 6:
                    pin = "0" + pin;
                    break;
            }

            return pin.Substring(0, 1) + @"\" + pin.Substring(1, 3) + @"\" + pin.Substring(4, 3) + @"\";
        }

        private static DateTime GetLastBackUpDate()
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            da.SelectCommand = new SqlCommand();
            da.SelectCommand.Connection = new SqlConnection();
            da.SelectCommand.Connection.ConnectionString = XmlConnections.GetConnection("/Connections/BJVVV");
            da.SelectCommand.CommandText = "select * from BookAddInf..[BackupPeriodic] where ID = 1";
            if (da.Fill(ds, "t") == 0)
            {
                throw new Exception("Не найдена последняя дата бэкапа в базе!");
            }
            return (DateTime)ds.Tables["t"].Rows[0]["BACKUPLASTDATE"];
        }

        private static void UpdateLastBackupDate()
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            da.UpdateCommand = new SqlCommand();
            da.UpdateCommand.Connection = new SqlConnection();
            da.UpdateCommand.Connection.ConnectionString = XmlConnections.GetConnection("/Connections/BJVVV");
            da.UpdateCommand.CommandText = "update BookAddInf..[BackupPeriodic] set [BACKUPLASTDATE] = getdate() where ID = 1";
            da.UpdateCommand.Connection.Open();
            da.UpdateCommand.ExecuteNonQuery();
            da.UpdateCommand.Connection.Close();

        }
    }
}
