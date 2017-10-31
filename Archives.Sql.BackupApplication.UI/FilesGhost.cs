using System;
using System.Collections.Generic;

namespace Archives.Sql.BackupApplication.UI
{
    [Serializable]
    public class FilesGhost
    {
        public DateTime PubDate { get; set; }//最后一次操作的日期
        public List<string> DeffFiles { get; set; }//差异快照
        public List<string> FilesName { get; set; }//全部快照
    }
}
