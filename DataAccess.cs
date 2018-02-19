using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace Park_University_MVC.DataLayer
{
    public class DataAccess
    {
        static string connstr = ConfigurationManager.ConnectionStrings["StudentDB2017"].ConnectionString;
        public static object GetSingleAnswer(string sql, List<SqlParameter> plist, 
            SqlConnection connc = null, SqlTransaction sqltrans=null, bool isStoredProc = false)
        {
            object res = null;
            SqlConnection conn = null;
            if (sqltrans == null)
                conn = new SqlConnection(connstr);
            else
                conn = connc;
            try
            {
                if (sqltrans == null)
                    conn.Open();
                SqlCommand sqlcomm = new SqlCommand(sql, conn);
                if (sqltrans != null)
                    sqlcomm.Transaction = sqltrans;
                if (isStoredProc == true)
                    sqlcomm.CommandType = System.Data.CommandType.StoredProcedure;
                if(plist != null)
                {
                    foreach (SqlParameter p in plist)
                        sqlcomm.Parameters.Add(p);
                }
                res = sqlcomm.ExecuteScalar();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (sqltrans == null)
                    conn.Close();
            }
            return res;
        }

        public static DataTable GetManyRowsCols (string sql, List<SqlParameter> plist,
            SqlConnection connc = null, SqlTransaction sqltrans=null, bool isStoredProcedure = false)
        {
            DataTable res = new DataTable();
            SqlConnection conn = null;
            if (sqltrans == null)
                conn = new SqlConnection(connstr);
            else
                conn = connc;
            try
            {
                if (sqltrans == null)
                    conn.Open();
                SqlCommand sqlcomm = new SqlCommand(sql, conn);
                if (sqltrans != null)
                    sqlcomm.Transaction = sqltrans;
                if (isStoredProcedure)
                    sqlcomm.CommandType = CommandType.StoredProcedure;
                if (plist != null)
                {
                    foreach (SqlParameter p in plist)
                        sqlcomm.Parameters.Add(p);
                }
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlcomm;
                da.Fill(res);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (sqltrans == null)
                    conn.Close();
            }
            return res;
        }
        public static int InsertUpdateDelete(string sql, List<SqlParameter> plist,
            SqlConnection connc=null,SqlTransaction sqltrans = null, bool isStoredProcedure= false)
        {
            int res = 0;
            SqlConnection conn = null;
            if (sqltrans == null)
                conn = new SqlConnection(connstr);
            else
                conn = connc;
            try
            {
                if (sqltrans == null)
                    conn.Open();
                SqlCommand sqlcomm = new SqlCommand(sql, conn);
                if (sqltrans != null)
                    sqlcomm.Transaction = sqltrans;
                if (isStoredProcedure)
                    sqlcomm.CommandType = CommandType.StoredProcedure;
                if (plist != null)
                {
                    foreach (SqlParameter p in plist)
                        sqlcomm.Parameters.Add(p);
                }
                res = sqlcomm.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (sqltrans == null)
                    conn.Close();
            }
            return res;
        }
    }
}
