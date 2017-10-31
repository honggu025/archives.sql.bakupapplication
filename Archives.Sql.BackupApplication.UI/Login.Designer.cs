namespace Archives.Sql.BackupApplication.UI
{
    partial class Login
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            this.skTxtUserName = new CCWin.SkinControl.SkinTextBox();
            this.skTxtPassword = new CCWin.SkinControl.SkinTextBox();
            this.skChekLoginType = new CCWin.SkinControl.SkinCheckBox();
            this.skinLabel1 = new CCWin.SkinControl.SkinLabel();
            this.skinLabel2 = new CCWin.SkinControl.SkinLabel();
            this.skinLabel3 = new CCWin.SkinControl.SkinLabel();
            this.skinButton1 = new CCWin.SkinControl.SkinButton();
            this.SuspendLayout();
            // 
            // skTxtUserName
            // 
            this.skTxtUserName.BackColor = System.Drawing.Color.Transparent;
            this.skTxtUserName.BackgroundImage = global::Archives.Sql.BackupApplication.UI.Properties.Resources.userBg;
            this.skTxtUserName.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.skTxtUserName.DownBack = null;
            this.skTxtUserName.Icon = null;
            this.skTxtUserName.IconIsButton = false;
            this.skTxtUserName.IconMouseState = CCWin.SkinClass.ControlState.Normal;
            this.skTxtUserName.IsPasswordChat = '\0';
            this.skTxtUserName.IsSystemPasswordChar = false;
            this.skTxtUserName.Lines = new string[0];
            this.skTxtUserName.Location = new System.Drawing.Point(34, 151);
            this.skTxtUserName.Margin = new System.Windows.Forms.Padding(0);
            this.skTxtUserName.MaximumSize = new System.Drawing.Size(540, 47);
            this.skTxtUserName.MaxLength = 32767;
            this.skTxtUserName.MinimumSize = new System.Drawing.Size(540, 47);
            this.skTxtUserName.MouseBack = global::Archives.Sql.BackupApplication.UI.Properties.Resources.userHoverBg;
            this.skTxtUserName.MouseState = CCWin.SkinClass.ControlState.Normal;
            this.skTxtUserName.Multiline = false;
            this.skTxtUserName.Name = "skTxtUserName";
            this.skTxtUserName.NormlBack = global::Archives.Sql.BackupApplication.UI.Properties.Resources.userBg;
            this.skTxtUserName.Padding = new System.Windows.Forms.Padding(50, 5, 5, 5);
            this.skTxtUserName.ReadOnly = false;
            this.skTxtUserName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.skTxtUserName.Size = new System.Drawing.Size(540, 47);
            // 
            // 
            // 
            this.skTxtUserName.SkinTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.skTxtUserName.SkinTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.skTxtUserName.SkinTxt.Font = new System.Drawing.Font("微软雅黑", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skTxtUserName.SkinTxt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(3)))), ((int)(((byte)(51)))));
            this.skTxtUserName.SkinTxt.Location = new System.Drawing.Point(50, 5);
            this.skTxtUserName.SkinTxt.Name = "BaseText";
            this.skTxtUserName.SkinTxt.Size = new System.Drawing.Size(485, 36);
            this.skTxtUserName.SkinTxt.TabIndex = 0;
            this.skTxtUserName.SkinTxt.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.skTxtUserName.SkinTxt.WaterText = "";
            this.skTxtUserName.TabIndex = 5;
            this.skTxtUserName.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.skTxtUserName.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.skTxtUserName.WaterText = "";
            this.skTxtUserName.WordWrap = true;
            // 
            // skTxtPassword
            // 
            this.skTxtPassword.BackColor = System.Drawing.Color.Transparent;
            this.skTxtPassword.BackgroundImage = global::Archives.Sql.BackupApplication.UI.Properties.Resources.passHoverBg;
            this.skTxtPassword.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.skTxtPassword.DownBack = null;
            this.skTxtPassword.Icon = null;
            this.skTxtPassword.IconIsButton = false;
            this.skTxtPassword.IconMouseState = CCWin.SkinClass.ControlState.Normal;
            this.skTxtPassword.IsPasswordChat = '*';
            this.skTxtPassword.IsSystemPasswordChar = false;
            this.skTxtPassword.Lines = new string[0];
            this.skTxtPassword.Location = new System.Drawing.Point(34, 236);
            this.skTxtPassword.Margin = new System.Windows.Forms.Padding(0);
            this.skTxtPassword.MaximumSize = new System.Drawing.Size(540, 47);
            this.skTxtPassword.MaxLength = 32767;
            this.skTxtPassword.MinimumSize = new System.Drawing.Size(540, 47);
            this.skTxtPassword.MouseBack = global::Archives.Sql.BackupApplication.UI.Properties.Resources.passHoverBg;
            this.skTxtPassword.MouseState = CCWin.SkinClass.ControlState.Normal;
            this.skTxtPassword.Multiline = false;
            this.skTxtPassword.Name = "skTxtPassword";
            this.skTxtPassword.NormlBack = global::Archives.Sql.BackupApplication.UI.Properties.Resources.passBg;
            this.skTxtPassword.Padding = new System.Windows.Forms.Padding(50, 5, 5, 5);
            this.skTxtPassword.ReadOnly = false;
            this.skTxtPassword.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.skTxtPassword.Size = new System.Drawing.Size(540, 47);
            // 
            // 
            // 
            this.skTxtPassword.SkinTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.skTxtPassword.SkinTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.skTxtPassword.SkinTxt.Font = new System.Drawing.Font("微软雅黑", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skTxtPassword.SkinTxt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(3)))), ((int)(((byte)(51)))));
            this.skTxtPassword.SkinTxt.Location = new System.Drawing.Point(50, 5);
            this.skTxtPassword.SkinTxt.Name = "BaseText";
            this.skTxtPassword.SkinTxt.PasswordChar = '*';
            this.skTxtPassword.SkinTxt.Size = new System.Drawing.Size(485, 36);
            this.skTxtPassword.SkinTxt.TabIndex = 0;
            this.skTxtPassword.SkinTxt.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.skTxtPassword.SkinTxt.WaterText = "";
            this.skTxtPassword.TabIndex = 6;
            this.skTxtPassword.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.skTxtPassword.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.skTxtPassword.WaterText = "";
            this.skTxtPassword.WordWrap = true;
            // 
            // skChekLoginType
            // 
            this.skChekLoginType.AutoSize = true;
            this.skChekLoginType.BackColor = System.Drawing.Color.Transparent;
            this.skChekLoginType.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.skChekLoginType.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skChekLoginType.DefaultCheckButtonWidth = 37;
            this.skChekLoginType.DownBack = null;
            this.skChekLoginType.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skChekLoginType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.skChekLoginType.Location = new System.Drawing.Point(34, 319);
            this.skChekLoginType.MaximumSize = new System.Drawing.Size(40, 50);
            this.skChekLoginType.MinimumSize = new System.Drawing.Size(40, 40);
            this.skChekLoginType.MouseBack = null;
            this.skChekLoginType.Name = "skChekLoginType";
            this.skChekLoginType.NormlBack = null;
            this.skChekLoginType.SelectedDownBack = null;
            this.skChekLoginType.SelectedMouseBack = null;
            this.skChekLoginType.SelectedNormlBack = null;
            this.skChekLoginType.Size = new System.Drawing.Size(40, 40);
            this.skChekLoginType.TabIndex = 7;
            this.skChekLoginType.UseVisualStyleBackColor = false;
            this.skChekLoginType.CheckedChanged += new System.EventHandler(this.skChekLoginType_CheckedChanged);
            // 
            // skinLabel1
            // 
            this.skinLabel1.AutoSize = true;
            this.skinLabel1.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel1.BorderColor = System.Drawing.Color.White;
            this.skinLabel1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.skinLabel1.Location = new System.Drawing.Point(77, 327);
            this.skinLabel1.Name = "skinLabel1";
            this.skinLabel1.Size = new System.Drawing.Size(138, 21);
            this.skinLabel1.TabIndex = 8;
            this.skinLabel1.Text = "使用系统账户登陆";
            // 
            // skinLabel2
            // 
            this.skinLabel2.AutoSize = true;
            this.skinLabel2.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel2.BorderColor = System.Drawing.Color.White;
            this.skinLabel2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.skinLabel2.Location = new System.Drawing.Point(30, 121);
            this.skinLabel2.Name = "skinLabel2";
            this.skinLabel2.Size = new System.Drawing.Size(58, 21);
            this.skinLabel2.TabIndex = 9;
            this.skinLabel2.Text = "用户名";
            // 
            // skinLabel3
            // 
            this.skinLabel3.AutoSize = true;
            this.skinLabel3.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel3.BorderColor = System.Drawing.Color.White;
            this.skinLabel3.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.skinLabel3.Location = new System.Drawing.Point(30, 206);
            this.skinLabel3.Name = "skinLabel3";
            this.skinLabel3.Size = new System.Drawing.Size(42, 21);
            this.skinLabel3.TabIndex = 10;
            this.skinLabel3.Text = "密码";
            // 
            // skinButton1
            // 
            this.skinButton1.BackColor = System.Drawing.Color.Transparent;
            this.skinButton1.BaseColor = System.Drawing.Color.Aqua;
            this.skinButton1.BorderColor = System.Drawing.Color.Transparent;
            this.skinButton1.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinButton1.DownBack = ((System.Drawing.Image)(resources.GetObject("skinButton1.DownBack")));
            this.skinButton1.DownBaseColor = System.Drawing.Color.Aqua;
            this.skinButton1.DrawType = CCWin.SkinControl.DrawStyle.Img;
            this.skinButton1.ForeColorSuit = true;
            this.skinButton1.InnerBorderColor = System.Drawing.Color.Transparent;
            this.skinButton1.Location = new System.Drawing.Point(366, 310);
            this.skinButton1.MouseBack = ((System.Drawing.Image)(resources.GetObject("skinButton1.MouseBack")));
            this.skinButton1.Name = "skinButton1";
            this.skinButton1.NormlBack = ((System.Drawing.Image)(resources.GetObject("skinButton1.NormlBack")));
            this.skinButton1.Size = new System.Drawing.Size(208, 58);
            this.skinButton1.TabIndex = 11;
            this.skinButton1.UseVisualStyleBackColor = false;
            this.skinButton1.Click += new System.EventHandler(this.skinButton1_Click);
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(40)))), ((int)(((byte)(49)))));
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.CanResize = false;
            this.CaptionHeight = 20;
            this.ClientSize = new System.Drawing.Size(602, 403);
            this.Controls.Add(this.skinButton1);
            this.Controls.Add(this.skinLabel3);
            this.Controls.Add(this.skinLabel2);
            this.Controls.Add(this.skinLabel1);
            this.Controls.Add(this.skChekLoginType);
            this.Controls.Add(this.skTxtPassword);
            this.Controls.Add(this.skTxtUserName);
            this.EffectCaption = CCWin.TitleType.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Login";
            this.ShowDrawIcon = false;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Login";
            this.Load += new System.EventHandler(this.Login_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private CCWin.SkinControl.SkinTextBox skTxtUserName;
        private CCWin.SkinControl.SkinTextBox skTxtPassword;
        private CCWin.SkinControl.SkinCheckBox skChekLoginType;
        private CCWin.SkinControl.SkinLabel skinLabel1;
        private CCWin.SkinControl.SkinLabel skinLabel2;
        private CCWin.SkinControl.SkinLabel skinLabel3;
        private CCWin.SkinControl.SkinButton skinButton1;
    }
}