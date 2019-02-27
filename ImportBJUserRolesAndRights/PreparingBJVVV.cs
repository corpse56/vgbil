using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportBJUserRolesAndRights
{
    class PreparingBJVVV
    {
        string TARGET_BASE = "BJVVV";
        string ConnectionString = "Data Source=127.0.0.1;Initial Catalog=BJVVV;Integrated Security=True;Connect Timeout=1200;";
        List<FieldInfo> Fields = new List<FieldInfo>();
        List<BJUserInfo> Users = new List<BJUserInfo>();

        public enum Roles
        {
            Komplektator = 2,
            Katalogizator = 3,
            ExtKatalogizator = 4,
            Sistematizator = 5,
            Inventarizator = 6,
            ExtInventarizator = 7,
            Knigohranenie = 8,
            ExtKnigohranenie = 9,
            Obsluzhivanie = 10,
            ExtObsluzhivanie = 11,
            Guest = 12,
            Registrator = 13,
            OperatorBD = 14,
        };
        List<string> Komplektator = new List<string>();
        List<string> Katalogizator = new List<string>();
        List<string> ExtKatalogizator = new List<string>();
        List<string> Sistematizator = new List<string>();
        List<string> Inventarizator = new List<string>();
        List<string> ExtInventarizator = new List<string>();
        List<string> Knigohranenie = new List<string>();
        List<string> ExtKnigohranenie = new List<string>();
        List<string> Obsluzhivanie = new List<string>();
        List<string> ExtObsluzhivanie = new List<string>();
        List<string> Guest = new List<string>();
        List<string> Registrator = new List<string>();
        List<string> OperatorBD = new List<string>();

        public void Execute()
        {
            using (var reader = new StreamReader(@"e:\новые права библиоджет\Роли BJVVV.csv"))
            {


                while (!reader.EndOfStream)
                {

                    var line = reader.ReadLine();
                    var values = line.Split(';');
                    FieldInfo field = GetField(values[0]);
                    if (field == null) continue;
                    Debug.Assert(field.Id != -1);

                    Fields.Add(field);

                    Komplektator.Add(values[1]);
                    Katalogizator.Add(values[2]);
                    ExtKatalogizator.Add(values[3]);
                    Sistematizator.Add(values[4]);
                    Inventarizator.Add(values[5]);
                    ExtInventarizator.Add(values[6]);
                    Knigohranenie.Add(values[7]);
                    ExtKnigohranenie.Add(values[8]);
                    Obsluzhivanie.Add(values[9]);
                    ExtObsluzhivanie.Add(values[10]);
                    Guest.Add(values[11]);
                    Registrator.Add(values[12]);
                    OperatorBD.Add(values[13]);

                    Debug.Assert(values[1].ToUpper() != "NULL" || values[1].ToUpper() != "1");
                    Debug.Assert(values[2].ToUpper() != "NULL" || values[2].ToUpper() != "1");
                    Debug.Assert(values[3].ToUpper() != "NULL" || values[3].ToUpper() != "1");
                    Debug.Assert(values[4].ToUpper() != "NULL" || values[4].ToUpper() != "1");
                    Debug.Assert(values[5].ToUpper() != "NULL" || values[5].ToUpper() != "1");
                    Debug.Assert(values[6].ToUpper() != "NULL" || values[6].ToUpper() != "1");
                    Debug.Assert(values[7].ToUpper() != "NULL" || values[7].ToUpper() != "1");
                    Debug.Assert(values[8].ToUpper() != "NULL" || values[8].ToUpper() != "1");
                    Debug.Assert(values[9].ToUpper() != "NULL" || values[9].ToUpper() != "1");
                    Debug.Assert(values[10].ToUpper() != "NULL" || values[10].ToUpper() != "1");
                    Debug.Assert(values[11].ToUpper() != "NULL" || values[11].ToUpper() != "1");
                    Debug.Assert(values[12].ToUpper() != "NULL" || values[12].ToUpper() != "1");
                    Debug.Assert(values[13].ToUpper() != "NULL" || values[13].ToUpper() != "1");

                }
            }
            using (var reader = new StreamReader(@"e:\новые права библиоджет\пользователи BJVVV.csv", Encoding.Default))
            {
                BJUserInfo user = new BJUserInfo();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');
                    UserStatus us = new UserStatus();
                    us.DepId = GetDepId(values[3]);
                    Debug.Assert(us.DepId != -1);
                    us.RoleId = GetRoleId(values[2]);
                    Debug.Assert(us.RoleId != -1);
                    us.DepName = values[3];
                    if (values[0] == "")//дописать обработку
                    {
                        user.UserStatus.Add(us);
                        continue;
                    }
                    else
                    {
                        Users.Add(user);
                        user = new BJUserInfo();
                        user.login = values[0];
                        user.password = values[1];
                        user.HashPwd = BJUserInfo.HashPassword(user.password);
                        user.UserStatus.Add(us);
                    }
                }
                Users.RemoveAt(0);//удаляем пустой
                Users.Add(user);
            }

            InsertRoles();

            InsertRoleRights((int)Roles.Komplektator, Komplektator);
            InsertRoleRights((int)Roles.Katalogizator, Katalogizator);
            InsertRoleRights((int)Roles.ExtKatalogizator, ExtKatalogizator);
            InsertRoleRights((int)Roles.Sistematizator, Sistematizator);
            InsertRoleRights((int)Roles.Inventarizator, Inventarizator);
            InsertRoleRights((int)Roles.ExtInventarizator, ExtInventarizator);
            InsertRoleRights((int)Roles.Knigohranenie, Knigohranenie);
            InsertRoleRights((int)Roles.ExtKnigohranenie, ExtKnigohranenie);
            InsertRoleRights((int)Roles.Obsluzhivanie, Obsluzhivanie);
            InsertRoleRights((int)Roles.ExtObsluzhivanie, ExtObsluzhivanie);

            //ниже которые они без прав. можно не выполнять
            //InsertRoleRights(TARGET_BASE, (int)Roles.Guest, Guest);
            //InsertRoleRights(TARGET_BASE, (int)Roles.Registrator, Registrator);
            //InsertRoleRights(TARGET_BASE, (int)Roles.OperatorBD, OperatorBD);


            InsertUsers(Users);

        }

        private void InsertUsers(List<BJUserInfo> users)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.Connection.Open();
                command.Parameters.Add("Login", SqlDbType.NVarChar);
                command.Parameters.Add("Password", SqlDbType.VarChar);
                command.Parameters.Add("Name", SqlDbType.NVarChar);
                command.Parameters.Add("NewId", SqlDbType.Int);
                command.Parameters.Add("RoleId", SqlDbType.Int);
                command.Parameters.Add("DeptId", SqlDbType.Int);
                command.Parameters.Add("UChar", SqlDbType.Int);


                foreach (var user in users)
                {
                    command.Parameters["Login"].Value = user.login;

                    byte[] bytes = Encoding.Unicode.GetBytes(user.HashPwd);
                    char[] chars = new char[user.password.Length];
                    string newpass = Encoding.Unicode.GetString(bytes);
                    command.Parameters["Password"].Value = "";//Encoding.Default.GetString(bytes);
                    //char c = char.Parse(newpass[0].ToString());
                    command.Parameters["Name"].Value = user.login;
                    command.Parameters["NewId"].Value = DBNull.Value;
                    command.Parameters["RoleId"].Value = DBNull.Value;
                    command.Parameters["DeptId"].Value = DBNull.Value;
                    command.Parameters["UChar"].Value = DBNull.Value;

                    command.CommandText = " insert into " + TARGET_BASE + "..USERS (LOGIN, HASH, NAME  ) " +
                                                            " values               (upper(@Login), @Password, @Name);" +
                                                            "select scope_identity();";
                    int idUser = Convert.ToInt32(command.ExecuteScalar());
                    foreach (char c in newpass)
                    {
                        command.Parameters["UChar"].Value = (int)c;
                        command.Parameters["NewId"].Value = idUser;
                        command.CommandText = " update " + TARGET_BASE + "..USERS set HASH = HASH+char(@UChar) where ID = @NewId";
                        command.ExecuteNonQuery();
                    }

                    foreach (var status in user.UserStatus)
                    {
                        command.Parameters["NewId"].Value = idUser;
                        command.Parameters["RoleId"].Value = status.RoleId;
                        command.Parameters["DeptId"].Value = status.DepId;
                        command.CommandText = "insert into " + TARGET_BASE + "..USERSTATUS (IDUSER, IDROLE, IDDEPT) " +
                                                                                " values    (@NewId,@RoleId, @DeptId)";
                        command.ExecuteNonQuery();
                    }

                }
            }
        }

        void InsertRoleRights(int IdRole, List<string> Rights)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                connection.Open();
                int i = 0;
                foreach (string right in Rights)
                {
                    if (right == "NULL")
                    {
                        i++;
                        continue;
                    }
                    command.CommandText = " insert into " + TARGET_BASE + "..USERSROLERIGHTS (IDROLE, IDFIELD, MNFIELD) values " +
                        "                   (" + IdRole + ", " + Fields[i].Id.ToString() + "," + Fields[i].MNFIELD.ToString() + ")";
                    command.ExecuteNonQuery();
                    i++;
                }
            }
        }
        void InsertRoles()
        {
            DataTable result = new DataTable();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                connection.Open();
                command.CommandText = " delete from " + TARGET_BASE + "..USERSROLE where ID != 1 ";
                command.ExecuteNonQuery();

                command.CommandText = "set IDENTITY_INSERT [" + TARGET_BASE + "].[dbo].[USERSROLE] on;" +
                                    " insert into " + TARGET_BASE + "..USERSROLE (ID, ROLE, OPERATION ) values (2,'Комплектатор','Комплектование');" +
                                    " insert into " + TARGET_BASE + "..USERSROLE (ID, ROLE, OPERATION ) values (3,'Каталогизатор','Каталогизация');" +
                                    " insert into " + TARGET_BASE + "..USERSROLE (ID, ROLE, OPERATION ) values (4,'Опытный каталогизатор','Каталогизация');" +
                                    " insert into " + TARGET_BASE + "..USERSROLE (ID, ROLE, OPERATION ) values (5,'Систематизатор','Систематизация');" +
                                    " insert into " + TARGET_BASE + "..USERSROLE (ID, ROLE, OPERATION ) values (6,'Инвентаризатор','Инвентаризация');" +
                                    " insert into " + TARGET_BASE + "..USERSROLE (ID, ROLE, OPERATION ) values (7,'Опытный инвентаризатор','Инвентаризация');" +
                                    " insert into " + TARGET_BASE + "..USERSROLE (ID, ROLE, OPERATION ) values (8,'Книгохранение','Хранение');" +
                                    " insert into " + TARGET_BASE + "..USERSROLE (ID, ROLE, OPERATION ) values (9,'Книгохранение расширенное','Хранение');" +
                                    " insert into " + TARGET_BASE + "..USERSROLE (ID, ROLE, OPERATION ) values (10,'Обслуживание читателей','Обслуживание читателей');" +
                                    " insert into " + TARGET_BASE + "..USERSROLE (ID, ROLE, OPERATION ) values (11,'Обслуживание читателей расширенное','Обслуживание читателей расширенное');" +
                                    " insert into " + TARGET_BASE + "..USERSROLE (ID, ROLE, OPERATION ) values (12,'Гость','Гость');" +
                                    " insert into " + TARGET_BASE + "..USERSROLE (ID, ROLE, OPERATION ) values (13,'Регистратор','Регистратор');" +
                                    " insert into " + TARGET_BASE + "..USERSROLE (ID, ROLE, OPERATION ) values (14,'Оператор базы данных','Оператор базы данных');" +
                                    "set IDENTITY_INSERT [" + TARGET_BASE + "].[dbo].[USERSROLE] off;";
                command.ExecuteNonQuery();

                command.CommandText = "update " + TARGET_BASE + "..USERSTATUS set IDROLE = 12 where IDUSER != 1;" +
                                      "update " + TARGET_BASE + "..USERS set LOGIN = substring(LOGIN+'_',1,25) where ID != 1;";
                command.ExecuteNonQuery();
            }
        }

        private FieldInfo GetField(string fieldName)
        {
            //if (fieldName == "Источник финансирования" ||
            //    fieldName == "Наименование коллекции" ||
            //    fieldName == "Финансирующая организация" ||
            //    fieldName == "Примечание о финансировании")
            //{
            //    return null;
            //}
            if (fieldName == "Примечание о финансировании")
            {
                return null;
            }

            DataTable result = new DataTable();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                command.CommandText = "select ID, MNFIELD, MSFIELD, NAME from " + TARGET_BASE + "..FIELDS where NAME = @FieldName";
                command.Parameters.AddWithValue("FieldName", SqlDbType.NVarChar).Value = fieldName;
                adapter.Fill(result);
            }
            FieldInfo field = new FieldInfo();
            if (result.Rows.Count == 0)
            {
                field.Id = -1;//не нашлось поле
                return field;
            }
            if ((result.Rows[0]["MSFIELD"].ToString() == "-1"))
            {
                field = null;//такое не нужно
                return field;
            }
            field.Id = (int)result.Rows[0]["ID"];
            field.Name = fieldName;
            field.MNFIELD = (int)result.Rows[0]["MNFIELD"];
            return field;
        }
        private int GetDepId(string DepName)
        {
            DepName = DepName.Replace(".", "").Replace("…", "");
            DataTable result = new DataTable();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                command.Parameters.AddWithValue("DepName", SqlDbType.NVarChar).Value = DepName;
                command.CommandText = "select * from " + TARGET_BASE + "..LIST_8 where REPLACE(NAME,'…','') = @DepName ";
                int i = adapter.Fill(result);
                if (i == 0) return -1;
            }

            return Convert.ToInt32(result.Rows[0]["ID"]);

        }
        private int GetRoleId(string RoleName)
        {
            switch (RoleName)
            {
                case "Администратор":
                    return 1;
                case "Комплектатор":
                case "Компектатор":
                    return (int)Roles.Komplektator;
                case "Каталогизатор":
                    return (int)Roles.Katalogizator;
                case "Опытный каталогизатор":
                    return (int)Roles.ExtKatalogizator;
                case "Систематизатор":
                    return (int)Roles.Sistematizator;
                case "Инвентаризатор":
                    return (int)Roles.Inventarizator;
                case "Опытный инвентаризатор":
                    return (int)Roles.ExtInventarizator;
                case "Книгохранение":
                    return (int)Roles.Knigohranenie;
                case "Книгохранение расширенное":
                case "Книгохранение (+)":
                    return (int)Roles.ExtKnigohranenie;
                case "Обслуживание читателей":
                    return (int)Roles.Obsluzhivanie;
                case "Обслуживание читателей (+)":
                case "Обслуживание читателей расширенное":
                    return (int)Roles.ExtObsluzhivanie;
                case "Гость":
                    return (int)Roles.Guest;
                case "Регистратор":
                    return (int)Roles.Registrator;
                case "Оператор базы данных":
                    return (int)Roles.OperatorBD;

                default:
                    return -1;
            }
        }


    }
}
