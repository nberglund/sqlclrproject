namespace DeployAddIn {
  partial class finddb {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      this.textBox1 = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.cboServers = new System.Windows.Forms.ComboBox();
      this.btnRefresh = new System.Windows.Forms.Button();
      this.grpLogon = new System.Windows.Forms.GroupBox();
      this.pnlLogin = new System.Windows.Forms.Panel();
      this.txtPassword = new System.Windows.Forms.TextBox();
      this.txtUserName = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.rdSqlAuth = new System.Windows.Forms.RadioButton();
      this.rdWinAuth = new System.Windows.Forms.RadioButton();
      this.grpConnect = new System.Windows.Forms.GroupBox();
      this.btnNew = new System.Windows.Forms.Button();
      this.cboDbs = new System.Windows.Forms.ComboBox();
      this.label4 = new System.Windows.Forms.Label();
      this.btnTest = new System.Windows.Forms.Button();
      this.btnOK = new System.Windows.Forms.Button();
      this.btnCancel = new System.Windows.Forms.Button();
      this.pnlOK = new System.Windows.Forms.Panel();
      this.hlpProv = new System.Windows.Forms.HelpProvider();
      this.grpLogon.SuspendLayout();
      this.pnlLogin.SuspendLayout();
      this.grpConnect.SuspendLayout();
      this.pnlOK.SuspendLayout();
      this.SuspendLayout();
      // 
      // textBox1
      // 
      this.textBox1.BackColor = System.Drawing.SystemColors.Menu;
      this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.textBox1.Location = new System.Drawing.Point(12, 12);
      this.textBox1.Multiline = true;
      this.textBox1.Name = "textBox1";
      this.textBox1.Size = new System.Drawing.Size(329, 31);
      this.textBox1.TabIndex = 100;
      this.textBox1.Text = "Enter information to connect to the Microsoft SQL Server 2005, (or later), you in" +
          "tend to deploy your project to.";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(11, 46);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(66, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "Server name:";
      // 
      // cboServers
      // 
      this.cboServers.FormattingEnabled = true;
      this.cboServers.Location = new System.Drawing.Point(12, 62);
      this.cboServers.Name = "cboServers";
      this.cboServers.Size = new System.Drawing.Size(248, 21);
      this.cboServers.TabIndex = 0;
      // 
      // btnRefresh
      // 
      this.btnRefresh.Location = new System.Drawing.Point(266, 62);
      this.btnRefresh.Name = "btnRefresh";
      this.btnRefresh.Size = new System.Drawing.Size(75, 23);
      this.btnRefresh.TabIndex = 3;
      this.btnRefresh.Text = "Refresh";
      // 
      // grpLogon
      // 
      this.grpLogon.Controls.Add(this.pnlLogin);
      this.grpLogon.Controls.Add(this.rdSqlAuth);
      this.grpLogon.Controls.Add(this.rdWinAuth);
      this.grpLogon.Location = new System.Drawing.Point(12, 91);
      this.grpLogon.Name = "grpLogon";
      this.grpLogon.Size = new System.Drawing.Size(329, 131);
      this.grpLogon.TabIndex = 4;
      this.grpLogon.TabStop = false;
      this.grpLogon.Text = "Logon to the server";
      // 
      // pnlLogin
      // 
      this.pnlLogin.Controls.Add(this.txtPassword);
      this.pnlLogin.Controls.Add(this.txtUserName);
      this.pnlLogin.Controls.Add(this.label3);
      this.pnlLogin.Controls.Add(this.label2);
      this.pnlLogin.Location = new System.Drawing.Point(17, 65);
      this.pnlLogin.Name = "pnlLogin";
      this.pnlLogin.Size = new System.Drawing.Size(306, 62);
      this.pnlLogin.TabIndex = 2;
      // 
      // txtPassword
      // 
      this.txtPassword.Location = new System.Drawing.Point(84, 35);
      this.txtPassword.Name = "txtPassword";
      this.txtPassword.Size = new System.Drawing.Size(219, 20);
      this.txtPassword.TabIndex = 3;
      // 
      // txtUserName
      // 
      this.txtUserName.Location = new System.Drawing.Point(84, 9);
      this.txtUserName.Name = "txtUserName";
      this.txtUserName.Size = new System.Drawing.Size(219, 20);
      this.txtUserName.TabIndex = 2;
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(21, 38);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(52, 13);
      this.label3.TabIndex = 1;
      this.label3.Text = "Password:";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(19, 12);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(59, 13);
      this.label2.TabIndex = 0;
      this.label2.Text = "User Name:";
      // 
      // rdSqlAuth
      // 
      this.rdSqlAuth.AutoSize = true;
      this.rdSqlAuth.Location = new System.Drawing.Point(17, 42);
      this.rdSqlAuth.Name = "rdSqlAuth";
      this.rdSqlAuth.Size = new System.Drawing.Size(169, 17);
      this.rdSqlAuth.TabIndex = 1;
      this.rdSqlAuth.Text = "Use SQL Server Authentication";
      // 
      // rdWinAuth
      // 
      this.rdWinAuth.AutoSize = true;
      this.rdWinAuth.Location = new System.Drawing.Point(17, 19);
      this.rdWinAuth.Name = "rdWinAuth";
      this.rdWinAuth.Size = new System.Drawing.Size(158, 17);
      this.rdWinAuth.TabIndex = 0;
      this.rdWinAuth.Text = "Use Windows Authentication";
      // 
      // grpConnect
      // 
      this.grpConnect.Controls.Add(this.btnNew);
      this.grpConnect.Controls.Add(this.cboDbs);
      this.grpConnect.Controls.Add(this.label4);
      this.grpConnect.Location = new System.Drawing.Point(12, 228);
      this.grpConnect.Name = "grpConnect";
      this.grpConnect.Size = new System.Drawing.Size(329, 64);
      this.grpConnect.TabIndex = 5;
      this.grpConnect.TabStop = false;
      this.grpConnect.Text = "Connect to a database";
      // 
      // btnNew
      // 
      this.btnNew.Location = new System.Drawing.Point(255, 29);
      this.btnNew.Name = "btnNew";
      this.btnNew.Size = new System.Drawing.Size(65, 23);
      this.btnNew.TabIndex = 4;
      this.btnNew.Text = "New";
      // 
      // cboDbs
      // 
      this.cboDbs.FormattingEnabled = true;
      this.cboDbs.Location = new System.Drawing.Point(17, 32);
      this.cboDbs.Name = "cboDbs";
      this.cboDbs.Size = new System.Drawing.Size(231, 21);
      this.cboDbs.TabIndex = 3;
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(16, 16);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(160, 13);
      this.label4.TabIndex = 2;
      this.label4.Text = "Select or enter a database name:";
      // 
      // btnTest
      // 
      this.btnTest.Location = new System.Drawing.Point(22, 9);
      this.btnTest.Name = "btnTest";
      this.btnTest.Size = new System.Drawing.Size(106, 23);
      this.btnTest.TabIndex = 6;
      this.btnTest.Text = "Test Connection";
      // 
      // btnOK
      // 
      this.btnOK.Location = new System.Drawing.Point(181, 9);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new System.Drawing.Size(75, 23);
      this.btnOK.TabIndex = 7;
      this.btnOK.Text = "OK";
      // 
      // btnCancel
      // 
      this.btnCancel.Location = new System.Drawing.Point(266, 307);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 8;
      this.btnCancel.Text = "Cancel";
      // 
      // pnlOK
      // 
      this.pnlOK.Controls.Add(this.btnOK);
      this.pnlOK.Controls.Add(this.btnTest);
      this.pnlOK.Location = new System.Drawing.Point(1, 299);
      this.pnlOK.Name = "pnlOK";
      this.pnlOK.Size = new System.Drawing.Size(259, 38);
      this.pnlOK.TabIndex = 101;
      // 
      // finddb
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(349, 344);
      this.Controls.Add(this.pnlOK);
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.grpConnect);
      this.Controls.Add(this.grpLogon);
      this.Controls.Add(this.btnRefresh);
      this.Controls.Add(this.cboServers);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.textBox1);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.HelpButton = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "finddb";
      this.Text = "New Database Reference";
      this.grpLogon.ResumeLayout(false);
      this.grpLogon.PerformLayout();
      this.pnlLogin.ResumeLayout(false);
      this.pnlLogin.PerformLayout();
      this.grpConnect.ResumeLayout(false);
      this.grpConnect.PerformLayout();
      this.pnlOK.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TextBox textBox1;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ComboBox cboServers;
    private System.Windows.Forms.Button btnRefresh;
    private System.Windows.Forms.GroupBox grpLogon;
    private System.Windows.Forms.RadioButton rdSqlAuth;
    private System.Windows.Forms.RadioButton rdWinAuth;
    private System.Windows.Forms.Panel pnlLogin;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox txtUserName;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox txtPassword;
    private System.Windows.Forms.GroupBox grpConnect;
    private System.Windows.Forms.Button btnNew;
    private System.Windows.Forms.ComboBox cboDbs;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Button btnTest;
    private System.Windows.Forms.Button btnOK;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Panel pnlOK;
    private System.Windows.Forms.HelpProvider hlpProv;

  }
}