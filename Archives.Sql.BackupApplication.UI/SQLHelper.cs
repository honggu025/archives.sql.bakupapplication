using System;
using System.Data;
using System.Data.SqlClient;

namespace Archives.Sql.BackupApplication.UI
{
    public class SQLHelper
    {
        //public SQLHelper()
        //{
        //    //
        //    // TODO: 在此处添加构造函数逻辑
        //    //
        //}

        public static string strConn { get; set; }
        /// <summary>
        /// 增删改
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="type"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static int ExecutNonQuery(string sql, CommandType type, params SqlParameter[] param)
        {
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = type;
                    if (param != null)
                    {
                        cmd.Parameters.AddRange(param);
                    }
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }
        public static int ExecutNonQuery(string sql, CommandType type, bool timout, params SqlParameter[] param)
        {
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = type;
                    cmd.CommandTimeout = 1800;
                    if (param != null)
                    {
                        cmd.Parameters.AddRange(param);
                    }
                    conn.Open();
                    int i = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    return i;
                }
            }
        }
        /// <summary>
        /// 返回第一行第一列
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="type"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static Object ExcuteScalar(string sql, CommandType type, params SqlParameter[] param)
        {
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = type;
                    if (param != null)
                    {
                        cmd.Parameters.AddRange(param);
                    }
                    conn.Open();
                    return cmd.ExecuteScalar();
                }
            }
        }

        public static SqlDataReader ExcuteDataReader(string sql, CommandType type, params SqlParameter[] param)
        {
            SqlConnection conn = new SqlConnection(strConn);
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandType = type;
                if (param != null)
                {
                    cmd.Parameters.AddRange(param);
                }
                try
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    return cmd.ExecuteReader(CommandBehavior.CloseConnection);
                }
                catch (SqlException)
                {
                    conn.Close();
                    conn.Dispose();
                    return null;
                }
                catch (Exception)
                {
                    conn.Close();
                    conn.Dispose();
                    throw;
                }

            }
        }

        public static DataTable ExecuteDataTable(string sql, CommandType cmdType, params SqlParameter[] pms)
        {
            DataTable dt = new DataTable();
            using (SqlDataAdapter adapter = new SqlDataAdapter(sql, strConn))
            {
                adapter.SelectCommand.CommandType = cmdType;
                if (pms != null)
                {
                    adapter.SelectCommand.Parameters.AddRange(pms);
                }
                adapter.Fill(dt);
            }

            return dt;
        }
    }
}