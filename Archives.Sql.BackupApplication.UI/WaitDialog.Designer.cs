namespace Archives.Sql.BackupApplication.UI
{
    partial class WaitDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.skinProgressIndicator1 = new CCWin.SkinControl.SkinProgressIndicator();
            this.SuspendLayout();
            // 
            // skinProgressIndicator1
            // 
            this.skinProgressIndicator1.AutoStart = true;
            this.skinProgressIndicator1.BackColor = System.Drawing.Color.Transparent;
            this.skinProgressIndicator1.CircleColor = System.Drawing.Color.White;
            this.skinProgressIndicator1.CircleSize = 0.8F;
            this.skinProgressIndicator1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.skinProgressIndicator1.Location = new System.Drawing.Point(130, 15);
            this.skinProgressIndicator1.Name = "skinProgressIndicator1";
            this.skinProgressIndicator1.Percentage = 0F;
            this.skinProgressIndicator1.ShowText = true;
            this.skinProgressIndicator1.Size = new System.Drawing.Size(146, 146);
            this.skinProgressIndicator1.TabIndex = 11;
            this.skinProgressIndicator1.Text = "等待还原";
            this.skinProgressIndicator1.TextDisplay = CCWin.SkinControl.TextDisplayModes.Text;
            // 
            // WaitDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(40)))), ((int)(((byte)(49)))));
            this.CanResize = false;
            this.CaptionHeight = 4;
            this.ClientSize = new System.Drawing.Size(406, 177);
            this.Controls.Add(this.skinProgressIndicator1);
            this.EffectCaption = CCWin.TitleType.None;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MdiAutoScroll = false;
            this.MdiBorderStyle = System.Windows.Forms.BorderStyle.None;
            this.MinimizeBox = false;
            this.Mobile = CCWin.MobileStyle.None;
            this.Name = "WaitDialog";
            this.ShowDrawIcon = false;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "WaitDialog";
            this.ResumeLayout(false);

        }

        #endregion

        private CCWin.SkinControl.SkinProgressIndicator skinProgressIndicator1;
    }
}