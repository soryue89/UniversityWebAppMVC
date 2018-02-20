using Park_University_MVC.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Park_University_MVC.BusinessLayer
{
    public class BusinessAuth
    {
        public static bool VerifyLogin(string username, string password)
        {
            return RepositoryAuth.VerifyLogin(username, password);
        }

        public static string GetRolesForUser(string username)
        {
            return RepositoryAuth.GetRolesForUser(username);
        }
    }
}
