using Park_University_MVC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Park_University_MVC.DataLayer
{
    public class RepositoryAuth
    {
        public static bool VerifyLogin(string username, string password)
        {
            bool res = false;
            try
            {
                string sql = "select username from Users where Username=@Username and Password=@Password";
                List<SqlParameter> plist = new List<SqlParameter>();
                DBHelper.addSqlParam(plist, "@Username", SqlDbType.VarChar, username, 50);
                DBHelper.addSqlParam(plist, "@Password", SqlDbType.VarChar,password, 50);
                object obj = DataAccess.GetSingleAnswer(sql, plist);
                if (obj != null)
                    res = true;
            }
            catch (Exception)
            {
                throw;
            }
            return res;
        }

        public static string GetRolesForUser(string username)
        {
            string roles = "";
            try
            {
                string sql = "select r.RoleName from Roles r "
                    + "inner join UserRoles ur on ur.RoleId=r.RoleId "
                    + "inner join Users u on ur.UserName=u.UserName "
                    + "where u.UserName=@UserName";
                List<SqlParameter> plist = new List<SqlParameter>();
                DBHelper.addSqlParam(plist,"@UserName", SqlDbType.VarChar, username, 50);
                DataTable dt = DataAccess.GetManyRowsCols(sql, plist);
                foreach (DataRow dr in dt.Rows)
                    roles += dr["RoleName"].ToString() + "|";
                if (roles.Length > 0)
                    roles = roles.Substring(0, roles.Length-1);
            }
            catch(Exception)
            {
                throw;
            }
            return roles;
        }

        public static long VerifyUser(string username)
        {
            long res = 0;
            try
            {
                string sql = "select studentId from Users where UserName=@Username";
                List<SqlParameter> plist = new List<SqlParameter>();
                DBHelper.addSqlParam(plist, "@Username", SqlDbType.VarChar, username, 50);
                object obj = DataAccess.GetSingleAnswer(sql, plist);
                if (obj != null)
                {
                    res = (long)obj;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return res;
        }
    }
}