using System;
using System.IO;
using System.Windows.Forms;
using CCWin;
using System.Collections.Generic;

namespace Archives.Sql.BackupApplication.UI
{
    public partial class Form1 : CCSkinMain
    {
        public UserInfo UserInfo { get; set; }
        public Form1()
        {
            InitializeComponent();
        }


        #region 窗体初始化
        /// <summary>
        /// 初始化代码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Shown(object sender, EventArgs e)
        {
            listLog2.AddToButtom("开始加文件夹载备份和还原快照...");
            int fileGhostType = Common.getFileGhost(listLog2, Application.StartupPath);
            if (fileGhostType == 0)
            {
                listLog2.AddToButtom("备份和还原快照加载完成...OK");

            }
            else if (fileGhostType == 1)
            {
                listLog2.AddToButtom("找不到备份快照文件,请先进行首次备份...");
            }
            else if (fileGhostType == 2 || fileGhostType == 3)
            {
                //todo:禁用还原按钮
                listLog2.AddToButtom("找不到还原快照文件,不可进行还原操作,请先进行备份...");
            }
            //Login loginForm = new Login();
            //string idAndPsw = ConfigHelper.GetAppConfig("idAndPsw");
            //if (hasLoginConfig(idAndPsw))
            //{
            //    //配置有效
            //    string[] idpsw = idAndPsw.Split(',');
            //    loginForm.UserName = idpsw[0];
            //    loginForm.PassWorld = idpsw[1];
            //    loginForm.LoginType = idpsw[2] == "FromSysTem" ? LoginType.FromSysTem : LoginType.FromIdAndPsw;
            //}
            //else
            //{
            //    //配置无效
            //    if (this.UserInfo == null)
            //    {
            //        //无Info
            //        loginForm.LoginType = LoginType.FromSysTem;
            //    }
            //    else
            //    {
            //        //有Info
            //        loginForm.UserName = UserInfo.UserName;
            //        loginForm.PassWorld = UserInfo.PassWorld;
            //        loginForm.LoginType = UserInfo.LoginType;
            //    }
            //}
            
            while (!loginSeccess(createNewLoginForm()))
            {
                listLog2.AddToButtom("数据库登陆失败...");
                MessageBox.Show("数据库登陆信息验证失败！请重新输入！");
            }
            listLog2.AddToButtom("数据库登陆成功...OK");
            listLog2.AddToButtom("正在验证数据库完整性");
            if (ConnectionTestInfo.CheckExistsDataBase(UserInfo))
            {

                listLog2.AddToButtom("数据库完整性验证成功...OK");
                listLog2.AddToButtom("开始验证表单数据...");
                if (ConnectionTestInfo.checkTables(listLog2, UserInfo))
                {
                    listLog2.AddToButtom("表单校验成功...OK");
                }
                else
                {
                    listLog2.AddToButtom("表单校验失败...缺少核心表单数据");
                    listLog2.AddToButtom("开始执行初始还原操作...");
                    skBtnRestored_Click("", new EventArgs());
                }
            }
            else
            {
                listLog2.AddToButtom("数据库完整性验证失败...不存在");
                listLog2.AddToButtom("开始执行初始还原操作...");
                skBtnRestored_Click("", new EventArgs());
            }


        }

        private Login createNewLoginForm()
        {
            Login loginForm = new Login();
            string idAndPsw = ConfigHelper.GetAppConfig("idAndPsw");
            if (hasLoginConfig(idAndPsw))
            {
                //配置有效
                string[] idpsw = idAndPsw.Split(',');
                loginForm.UserName = idpsw[0];
                loginForm.PassWorld = idpsw[1];
                loginForm.LoginType = idpsw[2] == "FromSysTem" ? LoginType.FromSysTem : LoginType.FromIdAndPsw;
            }
            else
            {
                //配置无效
                if (this.UserInfo == null)
                {
                    //无Info
                    loginForm.LoginType = LoginType.FromSysTem;
                }
                else
                {
                    //有Info
                    loginForm.UserName = UserInfo.UserName;
                    loginForm.PassWorld = UserInfo.PassWorld;
                    loginForm.LoginType = UserInfo.LoginType;
                }
            }
            return loginForm;
        }

        public bool loginSeccess(Login loginForm)
        {
            DialogResult result = DialogResult.OK;
            try
            {
                result = loginForm.ShowDialog(this);
            }
            catch (Exception)
            {

            }
            if (result == DialogResult.OK)
            {
                UserInfo userinfo = new UserInfo { UserName = loginForm.UserName, PassWorld = loginForm.PassWorld, LoginType = loginForm.LoginType };
                string connectionString;
                string shili = ConfigHelper.GetAppConfig("shiLiName");
                if (loginForm.LoginType == LoginType.FromIdAndPsw)
                {
                    connectionString = @"Data Source=" + shili + ";Initial Catalog=master;User ID=" + loginForm.UserName + ";Password=" + loginForm.PassWorld;
                }
                else
                {
                    connectionString = @"Data Source=" + shili + ";Initial Catalog=master;Integrated Security=True";
                }
                if (ConnectionTestInfo.ConnectionTest(connectionString))
                {
                    userinfo.ConnectionString = connectionString;
                    userinfo.BaseName = ConfigHelper.GetAppConfig("baseName");
                    userinfo.AppPath = Application.StartupPath;
                    SQLHelper.strConn = connectionString;
                    this.UserInfo = userinfo;
                    ConfigHelper.UpdateAppConfig("idAndPsw", UserInfo.UserName + "," + UserInfo.PassWorld + "," + UserInfo.LoginType);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                System.Environment.Exit(0);
                return false;
            }
        }

        /// <summary>
        /// 是否具备有效的配置文件记录帐号密码
        /// </summary>
        /// <param name="idAndPsw"></param>
        /// <returns></returns>
        public bool hasLoginConfig(string idAndPsw)
        {
            if (string.IsNullOrEmpty(idAndPsw))
            {
                //没有配置
                return false;
            }
            else
            {
                //有配置
                string[] idpsw = idAndPsw.Split(',');
                if (idpsw.Length > 2)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        #endregion

        #region 一键还原
        /// <summary>
        /// 一键还原按钮,首先弹出选择还原文件夹对话框
        /// 接着检查文件夹里文件个数和文件名(只有一个且为Full代表完整还原,有两个且一个Full一个Def为差异还原)
        /// 然后执行还原数据库命令,弹出模态对话框,阻止用户操作.完成后关闭.弹出继续对话框
        /// 然后检查目标资料文件夹目录,与快照对比(程序中保存三份快照,一份为主机备份后生成的快照,一份为上次还原后生成的快照,一份为主机备份后生成的增量快照)
        /// 若计算后,与增量快照匹配,则进行还原,否则,拷贝完后,弹出缺少的文件列表.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void skBtnRestored_Click(object sender, EventArgs e)
        {

            listLog2.AddToButtom("开始执行还原...");
            //调用下面的数据库还原方法
            restoredDataBase();
            //调用下面的文件还原方法
            restoredFill();
        }
        //数据库还原逻辑,仅被一键还原调用.
        public void restoredDataBase()
        {
            #region 数据库还原逻辑
            //todo:是否添加跳过数据库还原对话框.
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                string sqlBackupFileForderPath = ConfigHelper.GetAppConfig("sqlBackupFileForderPath");
                if (string.IsNullOrEmpty(sqlBackupFileForderPath))
                {
                    sqlBackupFileForderPath = this.UserInfo.AppPath + "\\SQLBackup";
                }
                DirectoryInfo TheFolder = new DirectoryInfo(sqlBackupFileForderPath);

                if (!TheFolder.Exists || TheFolder.GetFiles("*.bak").Length == 0)
                {
                    dialog.ShowNewFolderButton = true;
                    dialog.Description = "数据库备份文件所在的文件夹";
                    //dialog.RootFolder = Environment.SpecialFolder.Recent;
                    if (dialog.ShowDialog(this) != DialogResult.OK)
                    {
                        listLog2.AddToButtom("未能选择文件夹,还原终止...");
                        return;
                    }
                    TheFolder = new DirectoryInfo(dialog.SelectedPath);
                }

                FileInfo[] fullFileInfo = TheFolder.GetFiles("backupFull.bak");
                FileInfo[] defFileInfo = TheFolder.GetFiles("backupDef.bak");
                if (defFileInfo.Length > 0)
                {
                    //差异还原
                    if (fullFileInfo.Length == 0)
                    {
                        listLog2.AddToButtom("找不到备份文件,还原终止...");
                        return;
                    }
                    string[] backFiles = new string[] { TheFolder.FullName + "\\" + fullFileInfo[0].Name, TheFolder.FullName + "\\" + defFileInfo[0].Name };
                    BackupAndRestored.FullRestoreDatabase(backFiles, UserInfo, listLog2, BacOrResType.Def);
                }
                else
                {
                    //完整还原
                    //检查文件个数
                    if (fullFileInfo.Length == 0)
                    {
                        listLog2.AddToButtom("找不到备份文件,还原终止...");
                        return;
                    }
                    string[] backFiles = new string[] { TheFolder.FullName + "\\" + fullFileInfo[0].Name };
                    BackupAndRestored.FullRestoreDatabase(backFiles, UserInfo, listLog2, BacOrResType.Full);
                }
            }
            #endregion
        }
        //资料文件还原逻辑,仅被一键还原调用和增量还原调用.
        public void restoredFill()
        {
            #region 文件还原逻辑
            ///先判断快照文件是否存在,还原时,所有快照都要使用.
            ///如果快照都存在,比较目标文件夹和目标快照是否相符,不符则生成差异快照(RestoredDef).(按逻辑,在没有外界因素干扰下,目标文件夹不会比目标快照多,多出来的也是无用文件,不用管)
            ///(本行逻辑有误,跳过)如果相符,比较原快照和原文件夹是否相符(一般都是程序生成,一般是相符的),不符则生成差异快照(BackupDef)
            ///如果相符,比较原快照和目标快照,生成差异快照(fillDefNow)
            ///比较fillDefNow和原文件夹,看原文件夹里是否包含fillDefNow所有文件
            ///如果不包含,列出列表\
            ///如果包含,执行还原

            #region 检查是否加载快照
            //判断Common里面,是否有备份快照,如果没有,弹出对话框提示选择,没有选择,则直接return
            if (Common.BackupGhost == null)
            {
                using (FolderBrowserDialog dialog = new FolderBrowserDialog())
                {
                    listLog2.AddToButtom("选择包含备份快照文件的文件夹");
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        int fileGhostResult = Common.getFileGhost(listLog2, dialog.SelectedPath);
                        while (fileGhostResult == 1 || fileGhostResult == 3)
                        {
                            listLog2.AddToButtom("请选择包含备份快照文件的文件夹");
                            if (dialog.ShowDialog() != DialogResult.OK)
                            {
                                listLog2.AddToButtom("没有找到备份快照文件,还原无法继续...False");
                                return;
                            }
                            fileGhostResult = Common.getFileGhost(listLog2, dialog.SelectedPath);
                        }
                    }
                    else
                    {
                        listLog2.AddToButtom("没有找到备份快照文件,还原无法继续...False");
                        return;
                    }
                }
            }
            #endregion
            //运行到这里,说明快照加载至少有备份快照,开始比较快照
            //Common.BackupGhost是主机备份后生成的原快照.Common.RestorGhost是目标还原后生成的目标快照
            #region 首先对比原快照与目标快照的差异,生成差异快照.然后对比差异快照和原备份文件夹,判断原备份文件夹内容是否齐全.

            #region 获取原备份文件夹路径
            //一般情况下,由程序一键备份生成的备份文件,路径为"程序所在目录\BackupFiles",存放在backupFilesFolderPath变量中
            //这里应至少包含最后一次增量的所有文件.先获取,等比较完备份快照和还原快照后再和其比较
            //todo:添加目录不存在的选择文件夹的逻辑
            string backupFilesFolderPath = UserInfo.AppPath + "\\BackupFiles";
            if (!Directory.Exists(backupFilesFolderPath))
            {
                listLog2.AddToButtom("资料备份文件不存在...还原结束");
                return;
            }
            #endregion

            #region 获取原备份文件夹实例,并生成临时快照backupFilesGhost,备用
            DirectoryInfo TheFolder = new DirectoryInfo(backupFilesFolderPath);//todo:备份文件如果一个硬盘放不下,则添加选择路径逻辑
            DirectoryInfo[] backupFilesInfo = TheFolder.GetDirectories();
            //List<string> backupFileGhostClong = new List<string>(Common.BackupGhost.FilesName);//将原快照进行复制,以保留原快照内容
            List<string> backupFilesGhost = new List<string>(); // 原备份文件夹快照
            foreach (var item in backupFilesInfo)//生成原备份文件夹快照
            {
                backupFilesGhost.Add(item.Name);
            }
            #region 逻辑错误,不进行比较
            //compareStrList(backupFileGhostClong, backupFilesGhost);
            //if (backupFileGhostClong.Count > 0)
            //{
            //    listLog2.AddToButtom("备份文件数量不符...丢失");
            //    foreach (var item in backupFileGhostClong)
            //    {
            //        listLog2.AddToButtom(item);
            //    }
            //    listLog2.AddToButtom("请查找以上文件,并拷贝到程序目录\\BackupFiles文件夹中");
            //    return;
            //} 
            #endregion
            #endregion

            #region 获取目标文件夹路径,保存在restoredFilesFolderPath变量中.
            //查找配置文件里是否有目标文件夹的配置,如果有,加载,如果没有,则弹出对话框选择目标文件夹,并且记录到配置文件.配置文件保存目标文件夹目录的Name为restoredFilesFolderPath
            //将获取到的路径保存到配置文件和内存restoredFilesFolderPath变量中.
            //-----------------------------------获取目标文件夹路径(开始)---------------------------------------
            string restoredFilesFolderPath = ConfigHelper.GetAppConfig("restoredFilesFolderPath");
            if (string.IsNullOrEmpty(restoredFilesFolderPath))
            {
                if (MessageBox.Show("确定将开始选择还原资料文件的目标路径\r\n取消将终止还原", "没有找到还原路径", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK)
                {
                    listLog2.AddToButtom("还原终止,请重新运行还原程序...");
                    return;
                }
                //listLog2.AddToButtom("没有找到目标文件夹,请先选择要还原的目标路径");
                using (FolderBrowserDialog dialog = new FolderBrowserDialog())
                {
                    dialog.ShowNewFolderButton = true;
                    if (dialog.ShowDialog() != DialogResult.OK)
                    {
                        listLog2.AddToButtom("还原终止,请重新运行还原程序...");
                        return;
                    }
                    //当用户选择了一个文件夹后,进行进一步询问,防止选错.
                    if (MessageBox.Show("当前选择路径\r\n" + dialog.SelectedPath + "\r\n取消将终止还原\r\n可在程序设置里重新配置此路径", "确定要在此位置进行还原吗?", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK)
                    {
                        listLog2.AddToButtom("还原终止,请重新运行还原程序...");
                        return;
                    }
                    //运行到这里,说明选择了目标路径,将路径写入配置文件
                    restoredFilesFolderPath = dialog.SelectedPath;
                    ConfigHelper.UpdateAppConfig("restoredFilesFolderPath", restoredFilesFolderPath);
                }

            }
            //todo:(重要,必须添加,否则还原无限终止)添加第一次运行还原时,如果读取到配置文件已经设置了路径,弹出对话框,用户确认后继续,不确认重新弹出选择路径对话框,有三个按钮:确认/重选/终止.
            //-----------------------------------获取目标文件夹路径(结束)--------------------------------------- 
            #endregion

            #region 生成目标文件夹临时快照restoredFileGhost,与原快照进行比较,比较后,原快照保留的内容即本次还原的增量
            //---------------对比原快照和目标文件夹(开始)-------------------------
            //获取目标文件夹内子文件夹的集合
            if (!Directory.Exists(restoredFilesFolderPath))
            {
                listLog2.AddToButtom("配置文件中目标文件夹路径有误,请在程序设置中重新配置...");
                return;
            }
            TheFolder = new DirectoryInfo(restoredFilesFolderPath);//使用原快照和原文件夹对比时创建的变量.
            //todo:添加比较原快照和目标目录Size大小功能
            DirectoryInfo[] restoredFilesInfo = TheFolder.GetDirectories();
            List<string> restoredFileGhost = new List<string>();
            List<string> backupFileGhostClong = new List<string>(Common.BackupGhost.FilesName);//将原快照进行复制,以保留原快照内容
            foreach (var item in restoredFilesInfo)
            {
                restoredFileGhost.Add(item.Name);
            }
            compareStrList(backupFileGhostClong, restoredFileGhost);
            if (backupFileGhostClong.Count < 1)
            {
                listLog2.AddToButtom("目标文件夹已包含所有内容...");
                return;
            }
            //正常情况下,比较完成后,backupFileGhostClong里面会有剩余,内容为备份快照与目标文件夹多出来的部分.即增量
            //todo:是否加入对目标快照也有剩余,并进行删除的逻辑.
            //---------------对比原快照和目标文件夹(结束)-------------------------  
            #endregion

            #region 将增量与原备份文件夹进行比较,如果文件夹缺少内容,提示补全,否则开始还原.
            //----------------检查原备份文件夹下是否包含所有增量文件(开始)-------------
            List<string> backupFileGhostClongClong = new List<string>(backupFileGhostClong);//将差异快照再次备份一份,作用只是进行比较,不参与还原.
            compareStrList(backupFileGhostClongClong, backupFilesGhost);//进行比较
            if (backupFileGhostClongClong.Count > 1)
            {
                listLog2.AddToButtom("备份文件数量不符...丢失");
                foreach (var item in backupFileGhostClongClong)
                {
                    listLog2.AddToButtom("缺少文件夹..." + item);
                    backupFileGhostClong.Remove(item);
                }
                listLog2.AddToButtom("请查找以上文件,并拷贝到程序目录\\BackupFiles文件夹中");//todo:考虑做成弹出窗体利用listview形式进行展示.
                return;
            }
            //----------------检查原备份文件夹下是否包含所有增量文件(结束)------------- 
            #endregion
            ///执行到这里,说明
            ///1.备份快照与目标文件夹已经比较.
            ///2.备份快照与目标文件夹的增量已经与备份文件夹进行比较.
            ///3.备份文件夹文件齐全.
            ///开始进行还原.
            //----------------------开始执行还原(开始)-------------------
            //(废弃)调用一个还原的函数,通过参数指定是全部还原还是增量还原.
            //上面一行注释逻辑不清晰,还原文件不用指定全部还原和增量还原,传给函数一个快照对象,按照快照进行复制即可.
            //backupFileGhostClong里面所包含的内容,是全部的增量,且所有的增量都有一个文件夹对应
            if (backupFileGhostClong.Count < 1)
            {
                listLog2.AddToButtom("没有进行任何复制,还原退出...");
                return;
            }
            BackupAndRestored.RestoredFiles(backupFileGhostClong, backupFilesFolderPath, restoredFilesFolderPath, listLog2);
            //----------------------开始执行还原(结束)-------------------
            #endregion
            #endregion

            #region 保存目标快照
            TheFolder = new DirectoryInfo(restoredFilesFolderPath);//使用原快照和原文件夹对比时创建的变量.
            //todo:添加比较原快照和目标目录Size大小功能
            restoredFilesInfo = TheFolder.GetDirectories();
            foreach (var item in restoredFilesInfo)
            {
                restoredFileGhost.Add(item.Name);
            }
            Common.SaveFileGhost(restoredFileGhost, UserInfo.AppPath, false);
            #endregion
        }
        #endregion

        #region 数据库单独还原
        /// <summary>
        /// 数据库完整还原按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fullRestoreDatabase_Click(object sender, EventArgs e)//完整还原按钮
        {
            using (OpenFileDialog fileDialog = new OpenFileDialog())
            {
                fileDialog.Filter = "(*.bak)|*.bak";
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    //获取用户选择文件的后缀名
                    //string extension = Path.GetExtension(fileDialog.FileName);
                    //获取用户选择文件的文件名
                    string fileName = Path.GetFileNameWithoutExtension(fileDialog.FileName);
                    if (fileName.Contains("Full"))
                    {
                        string[] strList = new string[] { fileDialog.FileName };
                        BackupAndRestored.FullRestoreDatabase(strList, this.UserInfo, listLog2, BacOrResType.Full);
                    }
                    else
                    {
                        MessageBox.Show("请选择完整的备份文件（文件名中包含“Full”字样）");
                        listLog2.AddToButtom("数据库还原失败...文件格式错误");
                        listLog2.AddToButtom("请使用“完整还原”按钮重新选择备份文件");
                    }
                }
            }
        }
        /// <summary>
        /// 数据库差异还原按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void skinButton1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog fileDialog = new OpenFileDialog())
            {
                fileDialog.Filter = "(*.bak)|*.bak";
                fileDialog.Multiselect = true;
                fileDialog.Title = "同时选择完整备份和差异备份文件";
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    if (fileDialog.FileNames.Length != 2)
                    {
                        listLog2.AddToButtom("需同时选中完整备份和差异备份两个文件...");
                        listLog2.AddToButtom("还原终止...False");
                        return;
                    }
                    //int fullFile = 0;
                    //int defFile = 0;
                    string[] strList = new string[2];
                    foreach (var item in fileDialog.FileNames)
                    {
                        if (Path.GetFileNameWithoutExtension(item).Contains("Full"))
                        {
                            strList[0] = item;
                            //fullFile += 1;
                        }
                        else if (Path.GetFileNameWithoutExtension(item).Contains("Def"))
                        {
                            strList[1] = item;
                            //defFile += 1;
                        }
                    }
                    if (string.IsNullOrEmpty(strList[0]) || string.IsNullOrEmpty(strList[1]))
                    {
                        listLog2.AddToButtom("所选文件名称不正确(需包含\"full\"或\"def\")");
                        listLog2.AddToButtom("还原终止...False");
                        return;
                    }
                    BackupAndRestored.FullRestoreDatabase(strList, this.UserInfo, listLog2, BacOrResType.Def);

                }
            }
        }
        #endregion

        #region 文件单独还原

        /// <summary>
        /// 对比两个字符串集合,将两个集合重复的部分remove掉
        /// 不会对集合进行克隆,将直接对传入的参数进行改动,如需要,自行克隆.
        /// </summary>
        public void compareStrList(List<string> strList1, List<string> strList2)
        {
            #region 测试字符串数组比较差异,最终版.
            //int result = 0;
            int i2Len;
            //i1Len = strList1.Count;
            i2Len = strList2.Count;
            for (int i = 0; i < strList1.Count; i++)
            {
                for (int j = 0; j < i2Len; j++)
                {
                    if (strList1[i] == strList2[j])
                    {
                        strList1.RemoveAt(i);
                        strList2.RemoveAt(j);
                        j = -1;
                        i2Len--;
                    }
                    if (i == strList1.Count)
                    {
                        break;
                    }
                }
            }
            #endregion
        }

        /// <summary>
        /// 文件还原按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void skBtnFullFileRestor_Click(object sender, EventArgs e)
        {
            //todo:(已完成)等写完一键还原方法后,再补充
            ///完整还原的逻辑
            ///就是要把原文件夹所有的文件都复制到目标去,不做任何比较
            ///唯一需要校验的时原快照和原文件夹是否相符
            ///但无论是否相符,都会复制所有文件,最后要提示缺少哪些文件.

            //获取原快照
            #region 检查是否加载快照
            //判断Common里面,是否有备份快照,如果没有,弹出对话框提示选择,没有选择,则直接return
            if (Common.BackupGhost == null)
            {
                using (FolderBrowserDialog dialog = new FolderBrowserDialog())
                {

                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        int fileGhostResult = Common.getFileGhost(listLog2, dialog.SelectedPath);
                        while (fileGhostResult == 1 || fileGhostResult == 3)
                        {
                            listLog2.AddToButtom("请选择包含备份快照文件的文件夹");
                            if (dialog.ShowDialog() != DialogResult.OK)
                            {
                                return;
                            }
                            fileGhostResult = Common.getFileGhost(listLog2, dialog.SelectedPath);
                        }
                    }
                    else
                    {
                        listLog2.AddToButtom("没有找到备份快照文件,还原无法继续...False");
                        return;
                    }
                }
            }
            #endregion

            //获取原文件夹路径,生成快照
            #region 获取原备份文件夹路径
            //一般情况下,由程序一键备份生成的备份文件,路径为"程序所在目录\BackupFiles",存放在backupFilesFolderPath变量中

            //todo:添加目录不存在的选择文件夹的逻辑
            string backupFilesFolderPath = UserInfo.AppPath + "\\BackupFiles";
            if (!Directory.Exists(backupFilesFolderPath))
            {
                listLog2.AddToButtom("资料备份文件不存在...还原结束");
                return;
            }
            #endregion
            #region 获取原备份文件夹实例,并生成临时快照backupFilesGhost,备用
            DirectoryInfo TheFolder = new DirectoryInfo(backupFilesFolderPath);//todo:备份文件如果一个硬盘放不下,则添加选择路径逻辑
            DirectoryInfo[] backupFilesInfo = TheFolder.GetDirectories();
            //List<string> backupFileGhostClong = new List<string>(Common.BackupGhost.FilesName);//将原快照进行复制,以保留原快照内容
            List<string> backupFilesGhost = new List<string>(); // 原备份文件夹快照
            foreach (var item in backupFilesInfo)//生成原备份文件夹快照
            {
                backupFilesGhost.Add(item.Name);
            }
            #endregion

            //比较,生成差异快照
            List<string> backupFileGhostClong = new List<string>(Common.BackupGhost.FilesName);//将原快照进行复制,以保留原快照内容
            compareStrList(backupFileGhostClong, backupFilesGhost);

            //获得目标文件夹路径
            #region 获取目标文件夹路径,保存在restoredFilesFolderPath变量中.
            //查找配置文件里是否有目标文件夹的配置,如果有,加载,如果没有,则弹出对话框选择目标文件夹,并且记录到配置文件.配置文件保存目标文件夹目录的Name为restoredFilesFolderPath
            //将获取到的路径保存到配置文件和内存restoredFilesFolderPath变量中.
            //-----------------------------------获取目标文件夹路径(开始)---------------------------------------
            string restoredFilesFolderPath = ConfigHelper.GetAppConfig("restoredFilesFolderPath");
            if (string.IsNullOrEmpty(restoredFilesFolderPath))
            {
                if (MessageBox.Show("确定将开始选择还原资料文件的目标路径\r\n取消将终止还原", "没有找到还原路径", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK)
                {
                    listLog2.AddToButtom("还原终止,请重新运行还原程序...");
                    return;
                }
                //listLog2.AddToButtom("没有找到目标文件夹,请先选择要还原的目标路径");
                using (FolderBrowserDialog dialog = new FolderBrowserDialog())
                {
                    dialog.ShowNewFolderButton = true;
                    if (dialog.ShowDialog() != DialogResult.OK)
                    {
                        listLog2.AddToButtom("还原终止,请重新运行还原程序...");
                        return;
                    }
                    //当用户选择了一个文件夹后,进行进一步询问,防止选错.
                    if (MessageBox.Show("当前选择路径\r\n" + dialog.SelectedPath + "\r\n取消将终止还原\r\n可在程序设置里重新配置此路径", "确定要在此位置进行还原吗?", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK)
                    {
                        listLog2.AddToButtom("还原终止,请重新运行还原程序...");
                        return;
                    }
                    //运行到这里,说明选择了目标路径,将路径写入配置文件
                    restoredFilesFolderPath = dialog.SelectedPath;
                    ConfigHelper.UpdateAppConfig("restoredFilesFolderPath", restoredFilesFolderPath);
                }

            }
            //todo:(重要,必须添加,否则还原无限终止)添加第一次运行还原时,如果读取到配置文件已经设置了路径,弹出对话框,用户确认后继续,不确认重新弹出选择路径对话框,有三个按钮:确认/重选/终止.
            //-----------------------------------获取目标文件夹路径(结束)--------------------------------------- 
            #endregion
            //拷贝文件
            BackupAndRestored.RestoredFiles(backupFilesGhost, backupFilesFolderPath, restoredFilesFolderPath, listLog2);
            //提示缺少
            //如果文件有缺少(即原快照有剩余)提示缺少
            if (backupFileGhostClong.Count > 0)
            {
                foreach (var item in backupFileGhostClong)
                {
                    listLog2.AddToButtom("缺少文件夹..." + item);
                }
                listLog2.AddToButtom("请查找以上文件,并拷贝到程序目录\\BackupFiles文件夹中");//todo:考虑做成弹出窗体利用listview形式进行展示.
            }
            //保存快照到文件
            #region 保存目标快照
            TheFolder = new DirectoryInfo(restoredFilesFolderPath);//使用原快照和原文件夹对比时创建的变量.
            //todo:添加比较原快照和目标目录Size大小功能
            DirectoryInfo[] restoredFilesInfo = TheFolder.GetDirectories();
            List<string> restoredFileGhost = new List<string>();
            foreach (var item in restoredFilesInfo)
            {
                restoredFileGhost.Add(item.Name);
            }
            Common.SaveFileGhost(restoredFileGhost, UserInfo.AppPath, false);
            #endregion
        }
        /// <summary>
        /// 增量还原文件按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void skBtnDefFillRestor_Click(object sender, EventArgs e)
        {
            //todo:(已完成)等写完一键还原方法后再补充
            ///增量还原的逻辑
            ///就是一键还原的逻辑,直接调用即可
            restoredFill();
        }
        #endregion

        #region 一键备份
        private void skBtnBackup_Click(object sender, EventArgs e)
        {
            listLog2.AddToButtom("开始一键备份");
            //备份数据库
            sqlBackup();
            //文件备份
            filesBackup();
        }

        
        /// <summary>
        /// 数据库备份逻辑,仅被一键备份调用
        /// </summary>
        private void sqlBackup()
        {
            string backupFolderPath = UserInfo.AppPath + "\\SQLBackup";
            if (!Directory.Exists(backupFolderPath))
            {
                Directory.CreateDirectory(backupFolderPath);
            }
            string backupFullPath = backupFolderPath + "\\backupFull.bak";
            string backupDefPath = backupFolderPath + "\\backupDef.bak";
            string backupDefLastPath = backupFolderPath + "\\backupDef_last.bak";
            ///完整备份可以在两个地方进行,一个时一键智能备份,一个时完整备份按钮,这两个无论怎样,
            ///都会覆盖完整备份的文件,所以,这里只要检测到完整备份的文件,就只做差异备份
            ///没有完整备份的文件,就做完整备份.
            ///每次完整备份之前,也要做完整备份的备份.
            ///检查完整备份文件是否存在,若存在,检查上次备份文件是否存在
            if (File.Exists(backupFullPath))
            {
                //有完整备份的文件,只做差异备份
                //如果有差异备份文件,则改名,再进行差异备份
                if (File.Exists(backupDefPath))
                {
                    File.Copy(backupDefPath, backupDefLastPath, true);
                }
                BackupAndRestored.BackUpDataBase(backupDefPath, UserInfo, listLog2, false);
            }
            else
            {
                //没有完整备份的文件,做完整备份
                BackupAndRestored.BackUpDataBase(backupFullPath, UserInfo, listLog2, true);
            }
        }


        /// <summary>
        /// 文件备份逻辑
        /// </summary>
        private void filesBackup()
        {
            ///弹出文件备份选择要备份文件夹的对话框
            ///todo:做分散备份的工作,防止一个硬盘放不下
            ///先看有没有目标快照,有的话,比较目标快照和原文件夹,做差异备份,没有,做完整备份.
            ///完整备份,先看有没有完整备份快照,有的话,比较备份快照和原文件夹,进行差异备份,没有,完整备份全部文件
            ///备份完后,将原文件夹生成快照.
            ///
            //按照上面逻辑,所有的备份都要用到原文件夹和原备份文件夹,所以,先将原文件夹生成临时快照,生成的临时快照集合为filesGhost
            #region 获取原文件夹路径
            //todo:在程序界面加入配置选项按钮,可以设置各种路径
            //首先在配置文件中找原文件夹的路径配置,如果没有找到,则提示配置,保存在filesFolderPath变量中.
            string filesFolderPath;
            if (!getFilesFolderPath(out filesFolderPath))
            {
                listLog2.AddToButtom("没有找到要备份的文件路径,请先在设置里配置路径");
                listLog2.AddToButtom("备份结束...");
                return;
                //todo:添加设置按钮.
            }
            #endregion
            #region 生成原文件夹临时快照集合为filesGhost
            //DirectoryInfo TheFolder = new DirectoryInfo(filesFolderPath);
            //DirectoryInfo[] filesInfo = TheFolder.GetDirectories();
            //List<string> filesGhost = new List<string>();//原文件夹临时快照集合
            //foreach (var item in filesInfo)
            //{
            //    filesGhost.Add(item.Name);
            //}
            List<string> filesGhost = createGhost(filesFolderPath);
            #endregion
            #region 获取原备份文件夹路径,保存变量名backupFilesFolderPath
            //原备份文件夹路径为
            string backupFilesFolderPath = UserInfo.AppPath + "\\BackupFiles";
            if (!Directory.Exists(backupFilesFolderPath))
            {
                Directory.CreateDirectory(backupFilesFolderPath);
            }
            #endregion
            #region 判断是完整备份还是差异备份的逻辑
            if (Common.RestorGhost == null)
            {
                //做完整备份
                if (Common.BackupGhost == null)
                {
                    //备份所有文件
                    BackupAndRestored.RestoredFiles(filesGhost, filesFolderPath, backupFilesFolderPath, listLog2);
                }
                else
                {
                    //备份原快照和原文件夹的差异
                    List<string> backupFileGhostClong = new List<string>(Common.BackupGhost.FilesName);
                    List<string> filesGhostClong = new List<string>(filesGhost);
                    compareStrList(backupFileGhostClong, filesGhostClong);
                    if (filesGhostClong.Count < 1)
                    {
                        listLog2.AddToButtom("备份文件夹已包含所有内容...");
                        return;
                    }
                    else
                    {
                        BackupAndRestored.RestoredFiles(filesGhostClong, filesFolderPath, backupFilesFolderPath, listLog2);
                    }
                }
            }
            else
            {
                //做目标快照和原文件夹的差异快照
                //差异快照再和原备份文件夹快照比较,生成最终要拷贝的文件快照
                List<string> restoredFileGhostClong = new List<string>(Common.RestorGhost.FilesName);
                List<string> filesGhostClong = new List<string>(filesGhost);
                List<string> backupFileGhostClong = new List<string>(Common.BackupGhost.FilesName);
                compareStrList(restoredFileGhostClong, filesGhostClong);
                if (filesGhostClong.Count < 1)
                {
                    listLog2.AddToButtom("本期没有增加任何文件");
                    return;
                }
                compareStrList(filesGhostClong, backupFileGhostClong);
                if (filesGhostClong.Count < 1)
                {
                    listLog2.AddToButtom("目标文件夹已包含所有内容...");
                    return;
                }
                BackupAndRestored.RestoredFiles(filesGhostClong, filesFolderPath, backupFilesFolderPath, listLog2);
            }
            #endregion

            //生成快照
            #region 保存快照
            List<string> backupFileGhost = createGhost(backupFilesFolderPath);
            Common.SaveFileGhost(backupFileGhost, UserInfo.AppPath, true);
            #endregion
        }

        public bool getFilesFolderPath(out string filesPath)
        {
            #region 获取原文件夹路径
            //todo:在程序界面加入配置选项按钮,可以设置各种路径
            //首先在配置文件中找原文件夹的路径配置,如果没有找到,则提示配置,保存在filesFolderPath变量中.
            string filesFolderPath = ConfigHelper.GetAppConfig("filesFolderPath");
            if (string.IsNullOrEmpty(filesFolderPath))
            {
                filesPath = "";
                return false;
                //todo:添加设置按钮.
            }
            #endregion
            filesPath = filesFolderPath;
            return true;
        }
        public List<string> createGhost(string filesFolderPath)  //为某个路径生成快照集合.
        {
            #region 生成原文件夹临时快照集合为filesGhost
            DirectoryInfo TheFolder = new DirectoryInfo(filesFolderPath);
            DirectoryInfo[] filesInfo = TheFolder.GetDirectories();
            List<string> filesGhost = new List<string>();//原文件夹临时快照集合
            foreach (var item in filesInfo)
            {
                filesGhost.Add(item.Name);
            }
            #endregion
            return filesGhost;
        }
        #endregion

        #region 数据库备份
        //数据库单独完整备份
        private void skBtnFullSqlBackup_Click(object sender, EventArgs e)
        {
            string backupFolderPath = UserInfo.AppPath + "\\SQLBackup";
            if (!Directory.Exists(backupFolderPath))
            {
                Directory.CreateDirectory(backupFolderPath);
            }
            string backupFullPath = backupFolderPath + "\\backupFull.bak";
            string backupFullLastPath = backupFolderPath + "\\backupFull_last.bak";
            if (File.Exists(backupFullPath))
            {
                File.Copy(backupFullPath, backupFullLastPath, true);
            }
            BackupAndRestored.BackUpDataBase(backupFullPath, UserInfo, listLog2, true);
        }
        //数据库单独差异备份
        private void skBtnDefSqlBackup_Click(object sender, EventArgs e)
        {
            sqlBackup();
        }
        #endregion

        #region 文件单独备份
        /// <summary>
        /// 文件完整备份
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void skBtnFullFileBackup_Click(object sender, EventArgs e)
        {
            //完整备份就是不管目标快照,只将原文件夹和原备份文件夹做差异比较,拷贝多出的内容.
            #region 获取原文件夹路径
            string filesFolderPath;
            if (!getFilesFolderPath(out filesFolderPath))
            {
                listLog2.AddToButtom("没有找到要备份的文件路径,请先在设置里配置路径");
                listLog2.AddToButtom("备份结束...");
                return;
            }
            #endregion
            #region 将原文件夹生成临时快照
            List<string> filesGhost = createGhost(filesFolderPath);
            #endregion
            #region 比较临时快照和原备份快照
            if (Common.BackupGhost == null)
            {
                Common.BackupGhost = new FilesGhost() { FilesName = new List<string>() };
            }
            compareStrList(filesGhost, Common.BackupGhost.FilesName);
            if (filesGhost.Count < 1)
            {
                listLog2.AddToButtom("目标已包含所有文件,备份结束");
                return;
            }
            #endregion
            #region 获取原备份文件夹路径,保存变量名backupFilesFolderPath
            //原备份文件夹路径为
            string backupFilesFolderPath = UserInfo.AppPath + "\\BackupFiles";
            if (!Directory.Exists(backupFilesFolderPath))
            {
                Directory.CreateDirectory(backupFilesFolderPath);
            }
            #endregion
            #region 拷贝文件
            BackupAndRestored.RestoredFiles(filesGhost, filesFolderPath, backupFilesFolderPath, listLog2);
            #endregion
            #region 保存快照
            List<string> backupFileGhost = createGhost(backupFilesFolderPath);
            Common.SaveFileGhost(backupFileGhost, UserInfo.AppPath, true);
            #endregion
        }

        /// <summary>
        /// 增量备份文件夹,就是一键备份的逻辑.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void skBtnDefFileBackup_Click(object sender, EventArgs e)
        {
            filesBackup();
        }
        #endregion

        /// <summary>
        /// (暂时无用)获取VS.NET 自带的数据库连接对话框的数据库连接信息
        /// </summary>
        /// <param name="conn">初始化连接字符串</param>
        /// <returns>数据库连接</returns>
        //public string GetDatabaseConnectionString(String conn)
        //{
        //    string connString = String.Empty;
        //    Microsoft.Data.ConnectionUI.DataConnectionDialog connDialog = new Microsoft.Data.ConnectionUI.DataConnectionDialog();

        //    // 添加数据源列表，可以向窗口中添加自己程序所需要的数据源类型 必须增加以下几项中任一一项
        //    connDialog.DataSources.Add(Microsoft.Data.ConnectionUI.DataSource.AccessDataSource); // Access 
        //    connDialog.DataSources.Add(Microsoft.Data.ConnectionUI.DataSource.OdbcDataSource); // ODBC
        //    connDialog.DataSources.Add(Microsoft.Data.ConnectionUI.DataSource.OracleDataSource); // Oracle 
        //    connDialog.DataSources.Add(Microsoft.Data.ConnectionUI.DataSource.SqlDataSource); // Sql Server
        //    connDialog.DataSources.Add(Microsoft.Data.ConnectionUI.DataSource.SqlFileDataSource); // Sql Server File

        //    // 初始化
        //    connDialog.SelectedDataSource = Microsoft.Data.ConnectionUI.DataSource.SqlDataSource;
        //    connDialog.SelectedDataProvider = Microsoft.Data.ConnectionUI.DataProvider.SqlDataProvider;
        //    //也可以提前设计好连接字符串。

        //    connDialog.ConnectionString = conn; //"Data Source=.;Initial Catalog=XJGasBottles_test;User ID=sa;Password=123456";
        //    //只能够通过DataConnectionDialog类的静态方法Show出对话框
        //    //不同使用dialog.Show()或dialog.ShowDialog()来呈现对话框
        //    DialogResult result = Microsoft.Data.ConnectionUI.DataConnectionDialog.Show(connDialog);
        //    if (result == DialogResult.OK)
        //    {
        //        connString = connDialog.ConnectionString;
        //    }
        //    else if (result == DialogResult.Cancel || result == DialogResult.Abort)
        //    {
        //        System.Environment.Exit(0);
        //    }
        //    else
        //    {
        //        connString = conn;
        //    }
        //    return connString;
        //}




        private void skinButton6_Click(object sender, EventArgs e)
        {
            #region 测试等待窗体
            //WaitThreadControl wtc = new WaitThreadControl() { RecordedDatabase = false };
            //WaitDialog wd = new WaitDialog(wtc);
            //wd.StartPosition = FormStartPosition.CenterParent;
            //Thread th = new Thread(n =>
            //{
            //    //wd.ShowDialog(this);
            //    //this.Invoke(new Action(() => { wd.ShowDialog(this); }));
            //    //((WaitDialog)n).ShowDialog(this);
            //    Thread.Sleep(5000);
            //    ((WaitThreadControl)n).RecordedDatabase = false;

            //});
            //th.IsBackground = true;
            //th.Start(wtc);
            //wd.ShowDialog(); 
            //wd.Hide();
            #endregion

            #region 测试字符串数组比较差异,最终版.
            //List<string> strList1 = new List<string>();
            //List<string> strList2 = new List<string>();
            //for (int i = 0; i < 2500; i++)
            //{
            //    strList1.Add("sssss" + i.ToString());
            //    strList2.Add("sssss" + i.ToString());
            //}
            //Random a = new Random();
            //for (int i = 0; i < 300; i++)
            //{
            //    strList1.RemoveAt(a.Next(1000));
            //}
            ////List<string> strDef = new List<string>();
            //Stopwatch st = new Stopwatch();
            //st.Start();
            //int i1Len, i2Len;
            //i1Len = strList1.Count;
            //i2Len = strList2.Count;
            //for (int i = 0; i < strList1.Count; i++)
            //{
            //    int cc = -1;
            //    for (int j = 0; j < i2Len; j++)
            //    {
            //        if (strList1[i] == strList2[j])
            //        {
            //            strList1.RemoveAt(i);
            //            strList2.RemoveAt(j);
            //            j = -1;
            //            i2Len--;
            //        }
            //        if (i == strList1.Count)
            //        {
            //            st.Stop();
            //            long long1 = st.ElapsedTicks;
            //            return;
            //        }
            //    }
            //}
            //st.Stop();
            //long long2 = st.ElapsedTicks;
            #endregion
            #region 获取目标文件夹路径
            //对比原快照和目标文件夹.
            //查找配置文件里是否有目标文件夹的配置,如果有,加载,如果没有,则弹出对话框选择目标文件夹,并且记录到配置文件.配置文件保存目标文件夹目录的Name为restoredFilesFolderPath
            //-----------------------------------获取目标文件夹路径(开始)---------------------------------------
            string restoredFilesFolderPath = ConfigHelper.GetAppConfig("restoredFilesFolderPath");
            if (string.IsNullOrEmpty(restoredFilesFolderPath))
            {
                if (MessageBox.Show("确定将开始选择还原资料文件的目标路径\r\n取消将终止还原", "没有找到还原路径", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK)
                {
                    listLog2.AddToButtom("还原终止,请重新运行还原程序...");
                    return;
                }
                //listLog2.AddToButtom("没有找到目标文件夹,请先选择要还原的目标路径");
                using (FolderBrowserDialog dialog = new FolderBrowserDialog())
                {
                    dialog.ShowNewFolderButton = true;
                    if (dialog.ShowDialog() != DialogResult.OK)
                    {
                        listLog2.AddToButtom("还原终止,请重新运行还原程序...");
                        return;
                    }
                    if (MessageBox.Show("当前选择路径\r\n" + dialog.SelectedPath + "\r\n取消将终止还原\r\n可在程序设置里重新配置此路径", "确定要在此位置进行还原吗?", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK)
                    {
                        listLog2.AddToButtom("还原终止,请重新运行还原程序...");
                        return;
                    }
                    //运行到这里,说明选择了目标路径,将路径写入配置文件
                    restoredFilesFolderPath = dialog.SelectedPath;
                    ConfigHelper.UpdateAppConfig("restoredFilesFolderPath", restoredFilesFolderPath);
                }

            }
            //-----------------------------------获取目标文件夹路径(结束)--------------------------------------- 
            #endregion


        }

        private void button1_Click(object sender, EventArgs e)
        {
            #region 测试代码
            //string s = Application.StartupPath;

            //Thread t = new Thread((q) => {
            //    string[] strArray = new string[] { "/", "—", "\\" };
            //    int i = 0;
            //    bool qq = (bool)q;
            //    while((bool)q)
            //    {
            //        int b;
            //        Math.DivRem(i, 3, out b);
            //        this.listLog.Invoke(new Action(() =>
            //        {
            //            listLog.Items.RemoveAt(listLog.Items.Count - 1);
            //            listLog.AddToButtom("开始还原完整数据库..." + strArray[b]);
            //        }));
            //        i++;
            //        Thread.Sleep(500);
            //    }
            //    //((Action<string>)q)("1000");
            //});
            //t.IsBackground = true;
            //t.Start(new Action<string>(m=> { MessageBox.Show(m); })); 
            #endregion

            #region 写文件范例
            //序列化
            //FileStream fs = new FileStream(@"D:\Program\CSharp\NGramTest\NGramTest\serializePeople.dat", FileMode.Create);
            //People p = new People() { Name = "Haocheng Wu", Age = 24 };

            //BinaryFormatter bf = new BinaryFormatter();
            //bf.Serialize(fs, p);
            //fs.Close();
            #endregion

            #region 读文件范例
            //fs = new FileStream(@"D:\Program\CSharp\NGramTest\NGramTest\serializePeople.dat", FileMode.Open);
            //BinaryFormatter bf = new BinaryFormatter();
            //People p = bf.Deserialize(fs) as People;
            #endregion
        }

        private void skinButton5_Click(object sender, EventArgs e)
        {

        }

        private void skinButton1_Click_1(object sender, EventArgs e)
        {

        }

        private void skinButton2_Click(object sender, EventArgs e)
        {

        }
    }
}
