using CCWin;
using CCWin.SkinControl;
using System;
using System.Windows.Forms;

namespace Archives.Sql.BackupApplication.UI
{
    public partial class Login : CCSkinMain
    {
        public Login()
        {
            InitializeComponent();

            //DataTable dataSources = SqlClientFactory.Instance.CreateDataSourceEnumerator().GetDataSources();
            //DataColumn column2 = dataSources.Columns["ServerName"];
            //DataColumn column = dataSources.Columns["InstanceName"];
            //DataRowCollection rows = dataSources.Rows;
            //string[] array = new string[rows.Count];
            //for (int i = 0; i < array.Length; i++)
            //{
            //    string str2 = rows[i][column2] as string;
            //    string str = rows[i][column] as string;
            //    if (((str == null) || (str.Length == 0)) || ("MSSQLSERVER" == str))
            //    {
            //        array[i] = str2;
            //    }
            //    else
            //    {
            //        array[i] = str2 + @"\" + str;
            //    }
            //}
            //Array.Sort<string>(array);
        }
        private string userName;
        public string UserName
        {
            get
            {
                return skTxtUserName.Text;
               // return txtUserName.Text;
            }
            set
            {
                //txtUserName.Text = value;
                skTxtUserName.Text = value;
                userName = value;
            }
        }
        private string passWorld;
        public string PassWorld
        {
            get
            {
                //return txtPassWorld.Text;
                return skTxtPassword.Text;
            }
            set
            {
                //txtPassWorld.Text = value;
                skTxtPassword.Text = value;
                passWorld = value;
            }
        }
        private LoginType loginType;
        
        public LoginType LoginType
        {
            get { return loginType; }
            set
            {
                loginType = value;
                //cbLoginType.SelectedIndex = (int)loginType;
                skChekLoginType.Checked = loginType == LoginType.FromSysTem;
                if (loginType == LoginType.FromIdAndPsw)
                {
                    //txtUserName.Enabled = true;
                    //txtPassWorld.Enabled = true;
                    skTxtUserName.Enabled = true;
                    skTxtPassword.Enabled = true;
                }
                else
                {
                    //txtUserName.Enabled = false;
                    //txtPassWorld.Enabled = false;
                    skTxtUserName.Enabled = false;
                    skTxtPassword.Enabled = false;
                }
            }
        }
        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void cbLoginType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.LoginType = ((ComboBox)sender).SelectedIndex == 0 ? LoginType.FromSysTem : LoginType.FromIdAndPsw;
        }

        

        private void skChekLoginType_CheckedChanged(object sender, EventArgs e)
        {
            this.LoginType = ((SkinCheckBox)sender).Checked ? LoginType.FromSysTem : LoginType.FromIdAndPsw;
            //MessageBox.Show(((SkinCheckBox)sender).Checked.ToString());
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            //this.Hide();

        }
    }
}
