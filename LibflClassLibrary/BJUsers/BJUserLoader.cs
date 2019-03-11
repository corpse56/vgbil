using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibflClassLibrary.BJUsers
{
    class BJUserLoader
    {
        BJUserDBWrapper wrapper = new BJUserDBWrapper();
        public BJUserInfo GetUserByLogin(string login)
        {
            DataTable table = wrapper.GetUserByLogin(login);
            if (table.Rows.Count == 0) return null;
            BJUserInfo result = new BJUserInfo();
            result.Login = login;
            result.UserStatus = new List<UserStatus>();
            foreach (DataRow row in table.Rows)
            {
                UserStatus us = new UserStatus();
                us.DepId = Convert.ToInt32(row["DepId"]);
                us.DepName = row["DepName"].ToString();
                us.RoleId = Convert.ToInt32(row["IDROLE"]);
                us.RoleName = row["ROLE"].ToString();
                result.UserStatus.Add(us);
            }
            return result;
        }
    }
}
