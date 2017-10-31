using CCWin.SkinControl;
using System.Windows.Forms;

namespace Archives.Sql.BackupApplication.UI
{
    public static class ListChange
    {
        public static int AddToButtom(this ListBox t,object item)
        {
            int i = t.Items.Add(item);
            t.TopIndex = t.Items.Count - (int)(t.Height / t.ItemHeight);
            return i;
        }

        public static int AddToButtom(this SkinListBox sbic,string item)
        {
            //SkinListBoxItem slbi = new SkinListBoxItem(item);
            //return sbic.Add(slbi);
            int i = sbic.Items.Add(new SkinListBoxItem(item));
            //t.TopIndex = t.Items.Count - (int)(t.Height / t.ItemHeight);
            sbic.TopIndex = sbic.Items.Count - (int)(sbic.Height / sbic.ItemHeight);
            return i;
        }
    }
}
