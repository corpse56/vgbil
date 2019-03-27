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
    class PreparingREDKOSTJ
    {
        string TARGET_BASE = "REDKOSTJ";
        //string ConnectionString = "Data Source=192.168.4.25,1443;Initial Catalog=BJVVV;Persist Security Info=True;User ID=pereezd;Password=pereezd_123;Connect Timeout=1200";
        string ConnectionString = "Data Source=127.0.0.1;Initial Catalog=BJVVV;Integrated Security=True;";

        List<FieldInfo> Fields = new List<FieldInfo>();
        List<BJUserInfo> Users = new List<BJUserInfo>();

        public enum Roles
        {
            RedkayaKniga = 2,
            Obsluzhivanie = 3,
            Komplektator = 4,
            Inventarizator = 5,
            Sistematizator = 6,
            Guest = 7
        };
        List<string> Komplektator = new List<string>();
        List<string> RedkayaKniga = new List<string>();
        List<string> Sistematizator = new List<string>();
        List<string> Inventarizator = new List<string>();
        List<string> Obsluzhivanie = new List<string>();

        public void Execute()
        {
            using (var reader = new StreamReader(@"e:\новые права библиоджет\Роли REDKOSTJ.csv", Encoding.Default))
            {


                while (!reader.EndOfStream)
                {

                    var line = reader.ReadLine();
                    var values = line.Split(';');
                    FieldInfo field = GetField(values[0]);
                    if (field == null) continue;
                    Debug.Assert(field.Id != -1, values[0]);

                    Fields.Add(field);

                    RedkayaKniga.Add(values[1]);
                    Obsluzhivanie.Add(values[2]);
                    Komplektator.Add(values[3]);
                    Inventarizator.Add(values[4]);
                    Sistematizator.Add(values[5]);

                    Debug.Assert(values[1].ToUpper() != "NULL" || values[1].ToUpper() != "1");
                    Debug.Assert(values[2].ToUpper() != "NULL" || values[2].ToUpper() != "1");
                    Debug.Assert(values[3].ToUpper() != "NULL" || values[3].ToUpper() != "1");
                    Debug.Assert(values[4].ToUpper() != "NULL" || values[4].ToUpper() != "1");
                    Debug.Assert(values[5].ToUpper() != "NULL" || values[5].ToUpper() != "1");

                }
            }
            using (var reader = new StreamReader(@"e:\новые права библиоджет\Пользователи REDKOSTJ.csv", Encoding.Default))
            {
                BJUserInfo user = new BJUserInfo();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');
                    UserStatus us = new UserStatus();
                    us.DepId = GetDepId(values[3]);
                    Debug.Assert(us.DepId != -1, values[3]);
                    us.RoleId = GetRoleId(values[2]);
                    Debug.Assert(us.RoleId != -1, values[2]);
                    us.DepName = values[3];
                    if (values[0] == "")
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
            InsertRoleRights((int)Roles.Sistematizator, Sistematizator);
            InsertRoleRights((int)Roles.Inventarizator, Inventarizator);
            InsertRoleRights((int)Roles.RedkayaKniga, RedkayaKniga);
            InsertRoleRights((int)Roles.Obsluzhivanie, Obsluzhivanie);

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
                                    " insert into " + TARGET_BASE + "..USERSROLE (ID, ROLE, OPERATION ) values (2,'Редкая книга','Редкая книга');" +
                                    " insert into " + TARGET_BASE + "..USERSROLE (ID, ROLE, OPERATION ) values (3,'Обслуживание читателей','Обслуживание читателей');" +
                                    " insert into " + TARGET_BASE + "..USERSROLE (ID, ROLE, OPERATION ) values (4,'Комплектатор','Комплектатор');" +
                                    " insert into " + TARGET_BASE + "..USERSROLE (ID, ROLE, OPERATION ) values (5,'Инвентаризатор','Инвентаризация');" +
                                    " insert into " + TARGET_BASE + "..USERSROLE (ID, ROLE, OPERATION ) values (6,'Систематизатор','Систематизация');" +
                                    " insert into " + TARGET_BASE + "..USERSROLE (ID, ROLE, OPERATION ) values (7,'Гость','Гость');" +
                                    "set IDENTITY_INSERT [" + TARGET_BASE + "].[dbo].[USERSROLE] off;";
                command.ExecuteNonQuery();

                command.CommandText = "update " + TARGET_BASE + "..USERSTATUS set IDROLE = 12 where IDUSER != 1;" +
                                      "update " + TARGET_BASE + "..USERS set LOGIN = substring('Z_'+LOGIN,1,25) where ID != 1;";
                command.ExecuteNonQuery();
            }
        }

        private FieldInfo GetField(string fieldName)
        {
            if (fieldName == "Источник финансирования" ||
                fieldName == "Направление временного хранения")
            //    fieldName == "Финансирующая организация" ||
            //    fieldName == "Примечание о финансировании")
            {
                return null;
            }
            //if (fieldName == "Примечание о финансировании")
            //{
            //    return null;
            //}

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
                case "Редкая книга":
                    return (int)Roles.RedkayaKniga;
                case "Систематизатор":
                    return (int)Roles.Sistematizator;
                case "Инвентаризатор":
                    return (int)Roles.Inventarizator;
                case "Обслуживание читателей":
                    return (int)Roles.Obsluzhivanie;
                case "Гость":
                    return (int)Roles.Guest;
                default:
                    return -1;
            }
        }


    }
}
