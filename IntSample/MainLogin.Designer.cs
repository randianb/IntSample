namespace SampleApp
{
    partial class MainLogin
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
            this.radLabel3 = new Telerik.WinControls.UI.RadLabel();
            this.txtPassword = new Telerik.WinControls.UI.RadTextBox();
            this.txtUsername = new Telerik.WinControls.UI.RadTextBox();
            this.radLabel2 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.btnLostPassword = new Telerik.WinControls.UI.RadButton();
            this.btnLogin = new Telerik.WinControls.UI.RadButton();
            this.chkSave = new Telerik.WinControls.UI.RadCheckBox();
            this.directorySearcher1 = new System.DirectoryServices.DirectorySearcher();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUsername)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnLostPassword)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnLogin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkSave)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radLabel3
            // 
            this.radLabel3.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radLabel3.Location = new System.Drawing.Point(12, 11);
            this.radLabel3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.radLabel3.Name = "radLabel3";
            this.radLabel3.Size = new System.Drawing.Size(149, 18);
            this.radLabel3.TabIndex = 18;
            this.radLabel3.Text = "Login to Development ERP";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(91, 72);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.NullText = "password";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(197, 20);
            this.txtPassword.TabIndex = 2;
            this.txtPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPassword_KeyDown);
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(91, 42);
            this.txtUsername.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.NullText = "username";
            this.txtUsername.Size = new System.Drawing.Size(197, 20);
            this.txtUsername.TabIndex = 1;
            // 
            // radLabel2
            // 
            this.radLabel2.Location = new System.Drawing.Point(12, 76);
            this.radLabel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.radLabel2.Name = "radLabel2";
            this.radLabel2.Size = new System.Drawing.Size(56, 18);
            this.radLabel2.TabIndex = 14;
            this.radLabel2.Text = "Password:";
            // 
            // radLabel1
            // 
            this.radLabel1.Location = new System.Drawing.Point(12, 48);
            this.radLabel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(59, 18);
            this.radLabel1.TabIndex = 13;
            this.radLabel1.Text = "Username:";
            // 
            // btnLostPassword
            // 
            this.btnLostPassword.Location = new System.Drawing.Point(179, 134);
            this.btnLostPassword.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnLostPassword.Name = "btnLostPassword";
            this.btnLostPassword.Size = new System.Drawing.Size(108, 24);
            this.btnLostPassword.TabIndex = 5;
            this.btnLostPassword.Text = "Lost Password";
            this.btnLostPassword.Click += new System.EventHandler(this.btnLostPassword_Click);
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(91, 134);
            this.btnLogin.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(83, 24);
            this.btnLogin.TabIndex = 4;
            this.btnLogin.Text = "Login";
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // chkSave
            // 
            this.chkSave.Location = new System.Drawing.Point(91, 104);
            this.chkSave.Name = "chkSave";
            this.chkSave.Size = new System.Drawing.Size(169, 18);
            this.chkSave.TabIndex = 3;
            this.chkSave.Text = "Save Username and Password";
            this.chkSave.Visible = false;
            // 
            // directorySearcher1
            // 
            this.directorySearcher1.ClientTimeout = System.TimeSpan.Parse("-00:00:01");
            this.directorySearcher1.ServerPageTimeLimit = System.TimeSpan.Parse("-00:00:01");
            this.directorySearcher1.ServerTimeLimit = System.TimeSpan.Parse("-00:00:01");
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Location = new System.Drawing.Point(263, 11);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(24, 24);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 17;
            this.pictureBox1.TabStop = false;
            // 
            // MainLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 171);
            this.Controls.Add(this.chkSave);
            this.Controls.Add(this.radLabel3);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.radLabel2);
            this.Controls.Add(this.radLabel1);
            this.Controls.Add(this.btnLostPassword);
            this.Controls.Add(this.btnLogin);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainLogin";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = " INT, S. A.";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainLogin_FormClosed);
            this.Load += new System.EventHandler(this.MainLogin_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUsername)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnLostPassword)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnLogin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkSave)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadLabel radLabel3;
        private System.Windows.Forms.PictureBox pictureBox1;
        private Telerik.WinControls.UI.RadTextBox txtPassword;
        private Telerik.WinControls.UI.RadTextBox txtUsername;
        private Telerik.WinControls.UI.RadLabel radLabel2;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadButton btnLostPassword;
        private Telerik.WinControls.UI.RadButton btnLogin;
        private Telerik.WinControls.UI.RadCheckBox chkSave;
        private System.DirectoryServices.DirectorySearcher directorySearcher1;
    }
}