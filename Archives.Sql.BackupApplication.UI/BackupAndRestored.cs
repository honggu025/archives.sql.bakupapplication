using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Collections;
using System.Windows.Forms;
using System.Threading;
using CCWin.SkinControl;

namespace Archives.Sql.BackupApplication.UI
{
    public class BackupAndRestored
    {
        static string connectionString = ConfigHelper.GetAppConfig("myConn1");
        SqlConnection conn = new SqlConnection(connectionString);
        /// <summary>
        /// 备份指定的数据库文件
        /// </summary>
        /// <param name="databasefile">备份文件要存放到的路径</param>
        /// <returns></returns>
        public static bool BackUpDataBase(string databasefile, UserInfo userinfo,SkinListBox listbox, bool isFullBackup)
        {
            ///调用时,由调用函数先确认是否存在完整备份的文件,有则先进行处理,确保执行备份时,文件已被删除或改名.
            ///这里的逻辑,不再校验是否有备份文件.
            ///只做单一的完整备份或差异备份

            ///RESTORE DATABASE [dzcjda_sjz] FROM  DISK = N'H:\xuz\dif_20170516.bak' WITH  FILE = 1,  MOVE N'dzcjda_sjz' TO N'F:\sql2008shili\MSSQL10_50.SQLEXPRESS\MSSQL\DATA\dzcjda_sjz.mdf',  MOVE N'dzcjda_sjz_log' TO N'F:\sql2008shili\MSSQL10_50.SQLEXPRESS\MSSQL\DATA\dzcjda_sjz_1.ldf',  NOUNLOAD,  STATS = 10
            ///if (File.Exists(databasefile))
            ///{
            ///    return false;
            ///}
            WaitThreadControl wtc = new WaitThreadControl { RecordedDatabase = false };
            Thread thread = new Thread((q) =>
            {
                //还原的数据库MyDataBase
                string sql;
                if (isFullBackup)
                {
                    //sql = "BACKUP DATABASE @baseName TO DISK = @backFilePath ";
                    sql = "BACKUP DATABASE @baseName TO  DISK = @backFilePath WITH NOFORMAT, INIT,  NAME = N'dzcjda_sjz-完整 数据库 备份', SKIP, NOREWIND, NOUNLOAD,  STATS = 10";
                }
                else
                {
                    sql = "BACKUP DATABASE @baseName TO  DISK = @backFilePath WITH  DIFFERENTIAL , NOFORMAT, INIT,  NAME = N'dzcjda_sjz-差异 数据库 备份', SKIP, NOREWIND, NOUNLOAD,  STATS = 10";
                }
                //string sql1 = @"BACKUP DATABASE [MZLWebApp] TO  DISK = '" + databasefile + ".bak' WITH NOFORMAT, NOINIT,  NAME = '"+ DateTime.Now.ToString("yyyyMMdd") +"', SKIP, NOREWIND, NOUNLOAD,  STATS = 10";
                SqlParameter[] spms = new SqlParameter[] {
                    new SqlParameter("@baseName" , userinfo.BaseName),
                    new SqlParameter("@backFilePath" , databasefile)
                };
                try
                {
                    SQLHelper.ExecutNonQuery(sql, CommandType.Text, spms);
                }
                catch (Exception)
                {
                    listbox.Invoke(new Action<string>(n => listbox.AddToButtom(n)), "备份失败...False");
                    ((WaitThreadControl)q).RecordedDatabase = true;
                    return;
                }
                listbox.Invoke(new Action<string>(n => listbox.AddToButtom(n)), "备份成功...OK");
                ((WaitThreadControl)q).RecordedDatabase = true;
                
            });
            thread.IsBackground = true;
            WaitDialog wd = new WaitDialog(wtc);
            wd.chengeLoadingText("正在备份数据库");
            wd.StartPosition = FormStartPosition.CenterParent;
            thread.Start(wtc);//要先执行线程再谈对话框,不然会阻塞.
            wd.ShowDialog();

            return true;

        }

        #region 网上的数据库还原原型,没有使用到
        //以下是还原数据库，稍微麻烦些，要关闭所有与当前数据库相连的连接------------------------------------

        //--------------------------------------------------------------------------------------------------------------------------
        public string RestoreDatabase(string backfile)
        {
            ///杀死原来所有的数据库连接进程
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = "Data Source=.;Initial Catalog=master;User ID=sa;pwd =";
            conn.Open();
            string sql = "SELECT spid FROM sysprocesses ,sysdatabases WHERE sysprocesses.dbid=sysdatabases.dbid AND sysdatabases.Name='" +
                          "MyDataBase" + "'";
            SqlCommand cmd1 = new SqlCommand(sql, conn);
            SqlDataReader dr;
            ArrayList list = new ArrayList();
            try
            {
                dr = cmd1.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(dr.GetInt16(0));
                }
                dr.Close();
            }
            catch (SqlException eee)
            {
                MessageBox.Show(eee.ToString());
            }
            finally
            {
                conn.Close();
            }
            //MessageBox.Show(list.Count.ToString());
            for (int i = 0; i < list.Count; i++)
            {
                conn.Open();
                cmd1 = new SqlCommand(string.Format("KILL {0}", list[i].ToString()), conn);
                cmd1.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("系统已经清除的数据库线程： " + list[i].ToString() + "\r\n正在还原数据库！");
            }
            //这里一定要是master数据库，而不能是要还原的数据库，因为这样便变成了有其它进程
            //占用了数据库。
            string constr = @"Data Source=.;Initial Catalog=master;User ID=sa;pwd =";
            string database = "";//MyDataBase;
            string path = backfile;
            string BACKUP = String.Format("RESTORE DATABASE {0} FROM DISK = '{1}'", database, path);
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(BACKUP, con);
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("还原成功,点击退出系统！");
                Application.Exit();
            }
            catch (SqlException ee)
            {
                //throw(ee);

                //MessageBox.Show("还原失败");

                MessageBox.Show(ee.ToString());

            }
            finally
            {
                con.Close();
            }
            return "成功与否字符串";
        }
        #endregion


        #region 还原数据库
        //以下是一键还原数据库(通过最后一个参数指定是完整还原还是差异还原,完整还原,第一个参数只要一个文件名,差异还原则要两个)------------------------------------
        /// <summary>
        /// 一键还原数据库
        /// </summary>
        /// <param name="backfile">一个或两个文件,完全备份和差异备份</param>
        /// <param name="userinfo"></param>
        /// <param name="listbox"></param>
        /// <param name="type">是完整还原还是差异还原</param>
        /// <returns></returns>
        public static bool FullRestoreDatabase(string[] backfile, UserInfo userinfo, SkinListBox listbox, BacOrResType type)
        {
            ///杀死原来所有的数据库连接进程
            ArrayList list = new ArrayList();
            string sql = "SELECT spid FROM sysprocesses ,sysdatabases WHERE sysprocesses.dbid=sysdatabases.dbid AND sysdatabases.Name=@baseName";
            SqlParameter spm = new SqlParameter("@baseName", userinfo.BaseName);
            using (SqlDataReader reader = SQLHelper.ExcuteDataReader(sql, CommandType.Text, spm))
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        list.Add(reader.GetInt16(0));
                    }
                }
            }
            for (int i = 0; i < list.Count; i++)
            {
                sql = string.Format("KILL {0}", list[i].ToString());
                SQLHelper.ExecutNonQuery(sql, CommandType.Text, null);
                //MessageBox.Show("系统已经清除的数据库线程： " + list[i].ToString() + "\r\n正在还原数据库！");
                listbox.AddToButtom("清除数据库线程： " + list[i].ToString());
            }
            listbox.AddToButtom("所有线程清理完成...OK");
            //listbox.AddToButtom("开始执行完整还原操作...");

            listbox.AddToButtom(type == BacOrResType.Full ? "开始完整还原数据库..." : "开始差异还原数据库...");
            WaitThreadControl wtc = new WaitThreadControl { RecordedDatabase = false };
            #region 通过斜线旋转执行等待
            //Thread waitThread = new Thread((canStop) =>
            //    {
            //        string[] strArray = new string[] { "/", "—", "\\" };
            //        int i = 0;
            //        WaitDialog wd = new WaitDialog();
            //        wd.StartPosition = FormStartPosition.CenterParent;
            //        listbox.Invoke(new Action<WaitDialog>(wd1 =>
            //        {
            //            ((WaitDialog)wd1).ShowDialog();
            //        }), wd);
            //        while (!((WaitThreadControl)canStop).RecordedDatabase)
            //        {
            //            int b;
            //            Math.DivRem(i, 3, out b);
            //            listbox.Invoke(new Action(() =>
            //            {
            //                listbox.Items.RemoveAt(listbox.Items.Count - 1);
            //                listbox.AddToButtom("正在还原数据库，禁止所有操作..." + strArray[b]);
            //            }));
            //            i++;
            //            Thread.Sleep(300);
            //        }

            //        listbox.Invoke(new Action<string>(u =>
            //        {
            //            listbox.Items.RemoveAt(listbox.Items.Count - 1);
            //            listbox.AddToButtom(u);
            //        }), "还原数据库执行完成...OK");
            //    }); 
            #endregion
            Thread thread = new Thread((q) =>
            {
                bool hasBase = ConnectionTestInfo.CheckExistsDataBase(userinfo);
                //数据库已经存在时,不需要加move关键字,但需要覆盖.
                //数据库不存在时,需要加move,但不需要覆盖
                string movPath = hasBase ? "" : @"MOVE @mdfFileName TO @mdfFilePath ,  MOVE @logFileName TO @logFilePath ,";//数据库是否存在
                string defaultPath = string.Empty;
                if (!hasBase)
                {
                    #region 获取数据库默认路径代码
                    listbox.Invoke(new Action<string>(n => { listbox.AddToButtom(n); }), "获取系统默认路径...");//.AddToButtom("获取系统默认路径...");
                    sql = "select filename from master..sysfiles";
                    using (SqlDataReader reader = SQLHelper.ExcuteDataReader(sql, CommandType.Text))
                    {
                        if (reader.HasRows)
                        {
                            if (reader.Read())
                            {
                                defaultPath = Path.GetDirectoryName(reader.GetString(0));
                            }
                        }
                    }
                    if (string.IsNullOrEmpty(defaultPath))
                    {
                        listbox.Invoke(new Action<string>(n => { listbox.AddToButtom(n); }), "获取系统默认路径失败，需指定数据库存放路径...");
                        //listbox.AddToButtom("获取系统默认路径失败，需指定数据库存放路径");
                        //todo:弹出对话框
                    }
                    else
                    {
                        listbox.Invoke(new Action<string, string>((n, m) => { listbox.AddToButtom(n); listbox.AddToButtom(m); }), "获取默认路径成功...OK", defaultPath);
                        //listbox.AddToButtom("获取默认路径成功");
                        //listbox.Invoke(new Action<string>(n => { listbox.AddToButtom(n); }), defaultPath);
                        //listbox.AddToButtom(defaultPath);
                    }
                    #endregion
                }
                if (type == BacOrResType.Full)
                {

                    sql = @"RESTORE DATABASE @baseName FROM  DISK = @backFileName WITH  FILE = 1, " + movPath + " NOUNLOAD, REPLACE, STATS = 10";//NORECOVERY,

                }
                else
                {
                    sql = "RESTORE DATABASE @baseName FROM  DISK = @backFileName WITH  FILE = 1, " + movPath + "  NORECOVERY, REPLACE,  NOUNLOAD,  STATS = 10";

                }
                SqlParameter[] spms = new SqlParameter[]
                {
                    new SqlParameter("@baseName" , userinfo.BaseName),
                    new SqlParameter("@backFileName", backfile[0]),
                    new SqlParameter("@mdfFileName", userinfo.BaseName),
                    new SqlParameter("@mdfFilePath", defaultPath + "\\" + userinfo.BaseName + ".mdf"),
                    new SqlParameter("@logFileName", userinfo.BaseName + "_log"),
                    new SqlParameter("@logFilePath", defaultPath + "\\" + userinfo.BaseName + ".ldf")
                };
                SQLHelper.ExecutNonQuery(sql, CommandType.Text, true, spms);
                //listbox.Invoke(new Action<string> ( u => listbox.AddToButtom(u) ),"执行完成");
                if (type == BacOrResType.Def)
                {
                    spms[1] = new SqlParameter("@backFileName", backfile[1]);
                    sql = @"RESTORE DATABASE @baseName FROM  DISK = @backFileName WITH  FILE = 1,  NOUNLOAD,  STATS = 10";
                    SQLHelper.ExecutNonQuery(sql, CommandType.Text, true, spms);
                }
                ((WaitThreadControl)q).RecordedDatabase = true;
                listbox.Invoke(new Action<string>(n => listbox.AddToButtom(n)), "还原完成...OK");
            });
            thread.IsBackground = true;
            thread.Start(wtc);//要先执行线程再谈对话框,不然会阻塞.
            WaitDialog wd = new WaitDialog(wtc);
            wd.chengeLoadingText("正在还原数据库");
            wd.StartPosition = FormStartPosition.CenterParent;
            wd.ShowDialog();
            //waitThread.IsBackground = true;
            //waitThread.Start(wtc);

            return true;
            #region 原来的代码
            //这里一定要是master数据库，而不能是要还原的数据库，因为这样便变成了有其它进程
            //占用了数据库。

            //string constr = userinfo.ConnectionString;
            //string database = userinfo.BaseName;//MyDataBase;
            //string path = backfile;
            //string BACKUP = String.Format("RESTORE DATABASE {0} FROM DISK = '{1}'", database, path);
            //SqlConnection con = new SqlConnection(constr);
            //SqlCommand cmd = new SqlCommand(BACKUP, con);
            //con.Open();
            //try
            //{
            //    cmd.ExecuteNonQuery();
            //    MessageBox.Show("还原成功,点击退出系统！");
            //    Application.Exit();
            //}
            //catch (SqlException ee)
            //{
            //    //throw(ee);

            //    //MessageBox.Show("还原失败");

            //    MessageBox.Show(ee.ToString());

            //}
            //finally
            //{
            //    con.Close();
            //}
            //return "成功与否字符串"; 
            #endregion
        }

        #endregion

        #region 还原资料文件夹
        ///还原资料文件夹
        ///经过主窗体的逻辑,可以认为,传过来的快照对象实例包含所有的需要还原的文件
        ///而且,快照记录的所有文件在文件夹中都一一对应
        ///不方便改名,这个方法其实是所有拷贝文件的方法,将第二个参数里所有内容拷贝到第三个参数的文件夹中,二/三参数在拷贝完后是平级的.
        public static void RestoredFiles(List<string> fileGhost, string backupFilesPath, string restoredFilesPath, SkinListBox listbox)
        {
            listbox.AddToButtom("开始拷贝文件...");
            WaitThreadControl wtc = new WaitThreadControl { RecordedDatabase = false };

            Thread thread = new Thread((q) =>
            {
                //todo:拷贝文件的逻辑,需要考虑如果目标磁盘满了,或者其他错误导致的还原中断,如何处理.
                //可以使用try块来捕获异常
                //调用递归,参数为两个路径,表示为将第一个路径所代表的文件夹拷贝到第二个路径下面
                //路径格式为第一个参数为快照中每一个文件夹名字完整路径,第二个参数,为目标文件夹完整路径
                foreach (var item in fileGhost)
                {
                    CopyDirectory(backupFilesPath + "\\" + item, restoredFilesPath);//todo:将等待窗体放进参数里,在每次拷贝一个文件后,改变等待窗体显示的内容,用invoke
                }

                ((WaitThreadControl)q).RecordedDatabase = true;
                listbox.Invoke(new Action<string>(n => listbox.AddToButtom(n)), "拷贝完成...OK");
            });
            thread.IsBackground = true;
            thread.Start(wtc);//要先执行线程再谈对话框,不然会阻塞.
            WaitDialog wd = new WaitDialog(wtc);
            wd.StartPosition = FormStartPosition.CenterParent;
            wd.chengeLoadingText("正在拷贝数据文件");
            wd.ShowDialog();
        }

        #region 拷贝文件夹递归模块,被上面调用
        /// <summary>
        /// 拷贝文件夹
        /// </summary>
        /// <param name="srcdir"></param>
        /// <param name="desdir"></param>
        private static void CopyDirectory(string srcdir, string desdir)
        {
            string folderName = srcdir.Substring(srcdir.LastIndexOf("\\") + 1);

            string desfolderdir = desdir + "\\" + folderName;

            if (desdir.LastIndexOf("\\") == (desdir.Length - 1))
            {
                desfolderdir = desdir + folderName;
            }
            string[] filenames = Directory.GetFileSystemEntries(srcdir);

            foreach (string file in filenames)// 遍历所有的文件和目录
            {
                if (Directory.Exists(file))// 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
                {

                    string currentdir = desfolderdir + "\\" + file.Substring(file.LastIndexOf("\\") + 1);
                    if (!Directory.Exists(currentdir))
                    {
                        Directory.CreateDirectory(currentdir);
                    }

                    CopyDirectory(file, desfolderdir);
                }

                else // 否则直接copy文件
                {
                    string srcfileName = file.Substring(file.LastIndexOf("\\") + 1);

                    srcfileName = desfolderdir + "\\" + srcfileName;


                    if (!Directory.Exists(desfolderdir))
                    {
                        Directory.CreateDirectory(desfolderdir);
                    }


                    File.Copy(file, srcfileName);
                }
            }//foreach 
        }//function end 
        #endregion 
        #endregion
    }

    public enum BacOrResType
    {
        Full, Def
    }
}
