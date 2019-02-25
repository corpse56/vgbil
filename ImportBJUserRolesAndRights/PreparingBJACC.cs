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
    class PreparingBJACC
    {
        enum Roles
        {
            AKC = 2,
            Obsluzhivanie = 3,
        }
        string TARGET_BASE = "BJACC";
        string ConnectionString = "Data Source=127.0.0.1;Initial Catalog=BJVVV;Integrated Security=True;Connect Timeout=1200";
        List<FieldInfo> Fields = new List<FieldInfo>();
        List<string> AKC = new List<string>();
        List<string> Obsluzhivanie = new List<string>();

        List<BJUserInfo> Users = new List<BJUserInfo>();
        public void Execute()
        {
            using (var reader = new StreamReader(@"e:\новые права библиоджет\Роли BJACC.csv", Encoding.Default))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');
                    FieldInfo field = GetField(values[0]);
                    if (field == null) continue;
                    Debug.Assert(field.Id != -1);

                    Fields.Add(field);
                    AKC.Add(values[1]);
                    Obsluzhivanie.Add(values[2]);
                    Debug.Assert(values[1].ToUpper() != "NULL" || values[1].ToUpper() != "1");
                    Debug.Assert(values[2].ToUpper() != "NULL" || values[2].ToUpper() != "1");
                }
            }

            using (var reader = new StreamReader(@"e:\новые права библиоджет\Пользователи BJACC.csv", Encoding.Default))
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
                Users[0] = user;
            }
            InsertRoles(TARGET_BASE);
            InsertRoleRights(TARGET_BASE, (int)Roles.AKC, AKC);
            InsertRoleRights(TARGET_BASE, (int)Roles.Obsluzhivanie, Obsluzhivanie);
            //InsertUsers(Users);


        }

        void InsertRoleRights(string BaseName, int IdRole, List<string> Rights)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                int i = 0;
                foreach (string right in Rights)
                {
                    if (right == "NULL")
                    {
                        i++;
                        continue;
                    }
                    command.CommandText = " insert into " + BaseName + "..USERSROLERIGHTS (IDROLE, IDFIELD, MNFIELD) values " +
                        "                   (" + IdRole + ", " + Fields[i].Id.ToString() + "," + Fields[i].MNFIELD.ToString() + ")";
                    command.ExecuteNonQuery();
                    i++;
                }
            }
        }

        void InsertRoles(string BaseName)
        {
            DataTable result = new DataTable();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;

                command.CommandText = " delete from " + BaseName + "..ROLES where ID != 1 ";
                command.ExecuteNonQuery();

                command.CommandText = "set IDENTITY_INSERT [" + BaseName + "].[dbo].[USERSROLE] on;" +
                                    " insert into " + BaseName + "..ROLES (ID, ROLE, OPERATION ) values (2,'АКЦ','АКЦ');" +
                                    " insert into " + BaseName + "..ROLES (ID, ROLE, OPERATION ) values (3,'Обслуживание','Обслуживание');" +
                                    " insert into " + BaseName + "..ROLES (ID, ROLE, OPERATION ) values (4,'Гость','Гость');" +
                                    "set IDENTITY_INSERT [" + BaseName + "].[dbo].[USERSROLE] off;";
                command.ExecuteNonQuery();

                command.CommandText = "update " + BaseName + "..USERSTATUS set IDROLE = 4 where IDUSER != 1";
                command.ExecuteNonQuery();
            }
        }

        private FieldInfo GetField(string fieldName)
        {
            if (fieldName == "Источник финансирования" ||
                fieldName == "Наименование коллекции" ||
                fieldName == "Финансирующая организация" ||
                fieldName == "Примечание о финансировании")
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
            DataTable result = new DataTable();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                command.Parameters.AddWithValue("DepName", SqlDbType.NVarChar).Value = DepName;
                command.CommandText = "select * from " + TARGET_BASE + "..LIST_8 where NAME = @DepName ";
                int i = adapter.Fill(result);
                if (i == 0) return -1;
            }

            return Convert.ToInt32(result.Rows[0]["ID"]);

        }

        private int GetRoleId(string RoleName)
        {
            switch (RoleName)
            {
                case "АКЦ":
                    return 2;
                case "Обслуживание читателей":
                    return 3;
                case "Администратор":
                    return 1;
                default:
                    return -1;
            }
        }
    }
}
