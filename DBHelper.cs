using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace Park_University_MVC.DataLayer
{
    class DBHelper
    {
        public static void addSqlParam(List<SqlParameter> plist, string paramName,
            SqlDbType paramType, object paramValue, int size = 0)
        {
            SqlParameter p = null;
            if (size == 0)
                p = new SqlParameter(paramName, paramType);
            else
                p = new SqlParameter(paramName, paramType, size);
            p.Value = paramValue;
            plist.Add(p);
        }
    }
}
