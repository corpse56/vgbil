using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportBJUserRolesAndRights
{
    class Program
    {
        public enum Roles { Komplektator = 2,
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
            OperatorBD = 14, };

        static void Main(string[] args)
        {
            using (var reader = new StreamReader(@"e:\Роли и права Bibliojet - Новые права.csv"))
            {
                List<string> Fields = new List<string>();
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

                List<string> Users = new List<string>();

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    Fields.Add(values[0]);
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

                }
                string ConnectionString = "Data Source=127.0.0.1;Initial Catalog=BJVVV;Integrated Security=True;Connect Timeout=1200";
                DataTable BJFields = GetBJFields(ConnectionString);

                InsertRoles("BJVVV", ConnectionString);

                InsertRoleRights("BJVVV", (int)Roles.Komplektator, Komplektator, ConnectionString);
                InsertRoleRights("BJVVV", (int)Roles.Katalogizator, Katalogizator, ConnectionString);
                InsertRoleRights("BJVVV", (int)Roles.ExtKatalogizator, ExtKatalogizator, ConnectionString);
                InsertRoleRights("BJVVV", (int)Roles.Sistematizator, Sistematizator, ConnectionString);
                InsertRoleRights("BJVVV", (int)Roles.Inventarizator, Inventarizator, ConnectionString);
                InsertRoleRights("BJVVV", (int)Roles.ExtInventarizator, ExtInventarizator, ConnectionString);
                InsertRoleRights("BJVVV", (int)Roles.Knigohranenie, Knigohranenie, ConnectionString);
                InsertRoleRights("BJVVV", (int)Roles.ExtKnigohranenie, ExtKnigohranenie, ConnectionString);
                InsertRoleRights("BJVVV", (int)Roles.Obsluzhivanie, Obsluzhivanie, ConnectionString);
                InsertRoleRights("BJVVV", (int)Roles.ExtObsluzhivanie, ExtObsluzhivanie, ConnectionString);
                //ниже которые они без прав. можно не выполнять
                //InsertRoleRights("BJVVV", (int)Roles.Guest, Guest);
                //InsertRoleRights("BJVVV", (int)Roles.Registrator, Registrator);
                //InsertRoleRights("BJVVV", (int)Roles.OperatorBD, OperatorBD);

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    Users.Add(values[0],);
                    Fields.Add(values[0]);
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

                }


                InsertUsers("BJVVV",);

            }
        }

        private static void InsertRoleRights(string BaseName, int IdRole, List<string> Rights, string ConnectionString)
        {
            DataTable Fields = GetBJFields(ConnectionString);
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                foreach (string right in Rights)
                {
                    DataRow field = Fields.AsEnumerable().First(x => x["NAME"].ToString() == right);
                    command.CommandText = " insert into " + BaseName + "..USERSROLERIGHTS (IDROLE, IDFIELD, MNFIELD) values ("+IdRole+", "+field["ID"].ToString()+","+ field["MNFIELD"].ToString() + ")";
                    command.ExecuteNonQuery();
                }
            }
        }
        private static void InsertRoles(string BaseName, string ConnectionString)
        {
            DataTable result = new DataTable();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;

                command.CommandText = " delete from " + BaseName + "..ROLES where ID != 1 ";
                command.ExecuteNonQuery();

                command.CommandText = "set IDENTITY_INSERT [" + BaseName + "].[dbo].[USERSROLE] on;" +
                                    " insert into " + BaseName + "..ROLES (ID, ROLE, OPERATION ) values (2,'Комплектатор','Комплектование');" +
                                    " insert into " + BaseName + "..ROLES (ID, ROLE, OPERATION ) values (3,'Каталогизатор','Каталогизация');" +
                                    " insert into " + BaseName + "..ROLES (ID, ROLE, OPERATION ) values (4,'Опытный каталогизатор','Каталогизация');" +
                                    " insert into " + BaseName + "..ROLES (ID, ROLE, OPERATION ) values (5,'Систематизатор','Систематизация');" +
                                    " insert into " + BaseName + "..ROLES (ID, ROLE, OPERATION ) values (6,'Инвентаризатор','Инвентаризация');" +
                                    " insert into " + BaseName + "..ROLES (ID, ROLE, OPERATION ) values (7,'Опытный инвентаризатор','Инвентаризация');" +
                                    " insert into " + BaseName + "..ROLES (ID, ROLE, OPERATION ) values (8,'Книгохранение','Хранение');" +
                                    " insert into " + BaseName + "..ROLES (ID, ROLE, OPERATION ) values (9,'Книгохранение расширенное','Хранение');" +
                                    " insert into " + BaseName + "..ROLES (ID, ROLE, OPERATION ) values (10,'Обслуживание читателей','Обслуживание читателей');" +
                                    " insert into " + BaseName + "..ROLES (ID, ROLE, OPERATION ) values (11,'Обслуживание читателей расширенное','Обслуживание читателей расширенное');" +
                                    " insert into " + BaseName + "..ROLES (ID, ROLE, OPERATION ) values (12,'Гость','Гость');" +
                                    " insert into " + BaseName + "..ROLES (ID, ROLE, OPERATION ) values (13,'Регистратор','Регистратор');" +
                                    " insert into " + BaseName + "..ROLES (ID, ROLE, OPERATION ) values (14,'Оператор базы данных','Оператор базы данных');" +
                                    "set IDENTITY_INSERT [" + BaseName + "].[dbo].[USERSROLE] off;";
                command.ExecuteNonQuery();

                command.CommandText = "update " + BaseName + "..USERSTATUS set IDROLE = 12 where IDUSER != 1";
                command.ExecuteNonQuery();
            }
        }

        public static DataTable GetBJFields(string ConnectionString)
        {
            DataTable result = new DataTable();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                command.CommandText = "select ID, MNFIELD, MSFIELD, NAME from BJVVV..FIELDS where MSFIELD != -1 ";
                adapter.Fill(result);
            }
            return result;

        }
    }
}
