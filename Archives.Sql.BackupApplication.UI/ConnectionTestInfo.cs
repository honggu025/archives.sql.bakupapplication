using CCWin.SkinControl;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Archives.Sql.BackupApplication.UI
{
    public class ConnectionTestInfo
    {
        private static SqlConnection mySqlConnection;  //mySqlConnection   is   a   SqlConnection   object 
        private static string ConnectionString = "";
        private static bool IsCanConnectioned = false;

        /// <summary>
        /// 测试连接数据库是否成功
        /// </summary>
        /// <returns></returns>
        public static bool ConnectionTest(string connectionStr)
        {
            //获取数据库连接字符串
            ConnectionString = connectionStr;
            //创建连接对象
            mySqlConnection = new SqlConnection(ConnectionString);
            //ConnectionTimeout 在.net 1.x 可以设置 在.net 2.0后是只读属性，则需要在连接字符串设置
            //如：server=.;uid=sa;pwd=;database=PMIS;Integrated Security=SSPI; Connection Timeout=30
            //mySqlConnection.ConnectionTimeout = 1;//设置连接超时的时间
            try
            {
                //Open DataBase
                //打开数据库
                mySqlConnection.Open();
                IsCanConnectioned = true;
            }
            catch
            {
                //Can not Open DataBase
                //打开不成功 则连接不成功
                IsCanConnectioned = false;
            }
            finally
            {
                //Close DataBase
                //关闭数据库连接
                mySqlConnection.Close();
            }
            //mySqlConnection   is   a   SqlConnection   object 
            if (mySqlConnection.State == ConnectionState.Closed || mySqlConnection.State == ConnectionState.Broken)
            {
                //Connection   is   not   available  
                return IsCanConnectioned;
            }
            else
            {
                //Connection   is   available  
                return IsCanConnectioned;
            }
        }

        #region 判断数据库是否存在
        public static bool CheckExistsDataBase(UserInfo userinfo)
        {
            string sql = "select count(*) From master.dbo.sysdatabases where name = @baseName";
            SqlParameter spm = new SqlParameter("@baseName", userinfo.BaseName);
            int result = (int)SQLHelper.ExcuteScalar(sql, CommandType.Text, spm);
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 判断数据库表是否存在，通过指定专用的连接字符串，执行一个不需要返回值的SqlCommand命令。
        /// <summary>
        /// 判断数据库表是否存在，返回页头，通过指定专用的连接字符串，执行一个不需要返回值的SqlCommand命令。
        /// </summary>
        /// <param name="tablename">bhtsoft表</param>
        /// <returns></returns>
        public static bool CheckExistsTable(string[] tablenames, UserInfo userinfo)
        {
            //String tableNameStr;
            //SqlCommand cmd;
            bool allExists = true;
            //using (SqlConnection con = new SqlConnection(ConfigHelper.GetAppConfig("myConn1")))
            //{
            //    con.Open();
            //    for (int i = 0; i < tablenames.Length; i++)
            //    {
            //        tableNameStr = "select count(1) from dzcjda_sjz where name = '" + tablenames[i] + "'";
            //        cmd = new SqlCommand(tableNameStr, con);
            //        int result = Convert.ToInt32(cmd.ExecuteScalar());
            //        if (result == 0)
            //        {
            //            allExists = false;
            //        }
            //    }

            //}

            for (int i = 0; i < tablenames.Length; i++)
            {
                string sql = "select count(1) from @baseName where name = @tableName";
                SqlParameter[] spm = new SqlParameter[]{
                    new SqlParameter("@baseName",userinfo.BaseName),
                    new SqlParameter("@tableName",tablenames[i])
                };
                int result = Convert.ToInt32(SQLHelper.ExcuteScalar(sql, CommandType.Text, spm));
                if (result == 0)
                {
                    allExists = false;
                }
                
            }
            return allExists;
        }
        #endregion


        #region 检查库中表的完整性
        public static bool checkTables(SkinListBox listbox, UserInfo userinfo)
        {
            List<string> strList = Common.TablesList.ToList();
            string sql = "use "+ userinfo.BaseName +";select [name] from [sysobjects] where [type] = 'u' order by[name]";
            using (SqlDataReader reader = SQLHelper.ExcuteDataReader(sql,CommandType.Text))
            {
                if (reader == null)
                {
                    return false;
                }
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        foreach (var item in strList)
                        {
                            if (item == reader.GetString(0))
                            {
                                strList.Remove(item);
                                //Thread.Sleep(300);
                                listbox.AddToButtom(item + "...OK");
                                break;
                            }
                            listbox.AddToButtom(item + "...FALSE");
                        }
                    }
                }
            }
            if (strList.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion
    }


}
