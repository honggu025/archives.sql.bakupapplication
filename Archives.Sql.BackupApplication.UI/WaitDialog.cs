using CCWin;
using System;
using System.Threading;
using System.Windows.Forms;

namespace Archives.Sql.BackupApplication.UI
{
    public partial class WaitDialog :  CCSkinMain
    {
        public WaitDialog()
        {
            InitializeComponent();
            this.ControlBox = false;
        }
        public WaitDialog(WaitThreadControl wtc):this()
        {
            Thread thread = new Thread(n => {
                while (!((WaitThreadControl)n).RecordedDatabase)
                {
                    //skinTextBox1.Invoke(new Action<string>(m => { skinTextBox1.Text = m; }),wtc.RecordedDatabase.ToString());
                    Application.DoEvents();
                }
                //this.BeginInvoke()
                this.Invoke(new Action(closeForm));
            });
            thread.IsBackground = true;
            thread.Start(wtc);
        }

        public void closeForm()
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public void chengeLoadingText(string message)
        {
            this.skinProgressIndicator1.Text = message;
        }
    }
}
