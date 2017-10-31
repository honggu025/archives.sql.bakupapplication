using CCWin.SkinControl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Archives.Sql.BackupApplication.UI
{
    public class Common
    {
        public static List<string> TablesList { get; set; }
        public static FilesGhost BackupGhost { get; set; }//本机备份快照
        public static FilesGhost RestorGhost { get; set; }//目标机快照


        static Common()
        {
            TablesList = new List<string> { "cj_code_bigclass", "cj_code_class", "cj_code_dbbackup", "cj_code_dept", "cj_code_funclass", "cj_code_log", "cj_code_midclass", "cj_code_secret_level", "cj_code_term", "cj_code_type", "cj_code_ultraiso", "cj_code_unitclass", "cj_t_dalytjb", "cj_t_flsltjb", "cj_t_gctj", "cj_user_qx", "cj_user_yh", "cj_user_yhz", "cj_user_zqx", "cj_user_zyh", "cj_y_daglr", "cj_y_execlaw", "cj_y_hy_information_1", "cj_y_hy_information_2", "cj_y_hyjl", "cj_y_project", "cj_y_search", "cj_y_workrecord", "cj_y_ywzd", "cj_y_zd_project", "cj_y_zrs", "cj_z_archive", "cj_z_dzda", "cj_z_file", "cj_z_project", "cj_z_sxda" };
            BackupGhost = new FilesGhost() { FilesName = new List<string>() };
            RestorGhost = new FilesGhost() { FilesName = new List<string>() };
        }

        public static int getFileGhost(SkinListBox listBox, string Path)
        {
            #region 读文件范例
            string appPath = Path;
            FileStream fs = null;
            BinaryFormatter bf = new BinaryFormatter();

            int result = 0;
            try
            {
                fs = new FileStream(appPath + @"\backupGhost.FileGhost", FileMode.Open);
                BackupGhost = bf.Deserialize(fs) as FilesGhost;
                listBox.AddToButtom("备份文件夹快照加载成功...OK");
            }
            catch (FileNotFoundException)
            {
                result += 1;
                BackupGhost = null;
                listBox.AddToButtom("备份文件夹快照加载失败...找不到备份快照文件");
            }
            try
            {
                fs = new FileStream(appPath + @"\restorGhost.FileGhost", FileMode.Open);
                RestorGhost = bf.Deserialize(fs) as FilesGhost;
                listBox.AddToButtom("还原文件夹快照加载成功...OK");
            }
            catch (FileNotFoundException)
            {
                result += 2;
                RestorGhost = null;
                listBox.AddToButtom("还原文件夹快照加载失败...找不到还原快照文件");
            }
            return result;
            #endregion
        }

        //递归获取文件夹大小
        public static long GetDirectoryLength(string dirPath)
        {
            //判断给定的路径是否存在,如果不存在则退出    
            if (!Directory.Exists(dirPath)) return 0;
            long len = 0;
            //定义一个DirectoryInfo对象    
            DirectoryInfo di = new DirectoryInfo(dirPath);
            //通过GetFiles方法,获取di目录中的所有文件的大小    
            foreach (FileInfo fi in di.GetFiles())
            { len += fi.Length; }
            //获取di中所有的文件夹,并存到一个新的对象数组中,以进行递归    
            DirectoryInfo[] dis = di.GetDirectories();
            if (dis.Length > 0)
            {
                for (int i = 0; i < dis.Length; i++)
                {
                    len += GetDirectoryLength(dis[i].FullName);
                }
            }
            return len;
        }
        /// <summary>
        /// 保存快照文件到硬盘
        /// </summary>
        /// <param name="fg">快照对象,调用之前不必设置保存时间,方法内部加时间.\r\n如有必要,需设置DeffFiles属性</param>
        /// <param name="savePath">完整文件夹路径</param>
        /// <param name="isBackupGhost">是否是备份快照,备份文件为backupGhost.FileGhost,还原文件为restorGhost.FileGhost</param>
        public static void SaveFileGhost(FilesGhost fg, string savePath, bool isBackupGhost)
        {
            string desfolderdir;
            string fileName = isBackupGhost ? "backupGhost.FileGhost" : "restorGhost.FileGhost";

            if (savePath.LastIndexOf("\\") == (savePath.Length - 1))
            {
                desfolderdir = savePath + fileName;
            }
            else
            {
                desfolderdir = savePath + "\\" + fileName;
            }
            #region 写文件范例
            //序列化
            FileStream fs = new FileStream(desfolderdir, FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();
            fg.PubDate = DateTime.Now;
            if (isBackupGhost)
            {
                Common.BackupGhost = fg;
            }
            else
            {
                Common.RestorGhost = fg;
            }
            bf.Serialize(fs, fg);
            fs.Close();
            #endregion
        }
        /// <summary>
        /// 保存快照文件到硬盘
        /// </summary>
        /// <param name="strList">字符串数组,保存了所有文件夹名称</param>
        /// <param name="savePath">完整文件夹路径</param>
        /// <param name="isBackupGhost">是否是备份快照,备份文件为backupGhost.FileGhost,还原文件为restorGhost.FileGhost</param>
        public static void SaveFileGhost(List<string> strList, string savePath, bool isBackupGhost)
        {
            FilesGhost fg = new FilesGhost() { FilesName = strList };
            SaveFileGhost(fg, savePath, isBackupGhost);
        }
    }
}
