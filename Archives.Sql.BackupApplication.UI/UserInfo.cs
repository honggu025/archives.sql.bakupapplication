namespace Archives.Sql.BackupApplication.UI
{
    public  class UserInfo
    {
        public  string UserName { get; set; }
        public  string PassWorld { get; set; }
        public  LoginType LoginType { get; set; }
        public string ConnectionString { get; set; }
        public string BaseName { get; set; }
        public string AppPath { get; set; }
    }
    public enum LoginType
    {
        FromSysTem = 0,
        FromIdAndPsw = 1
    }
}
