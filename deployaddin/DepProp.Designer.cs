namespace DeployAddIn {
  partial class DepProp {
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

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      this.lblAsmName = new System.Windows.Forms.Label();
      this.txtAsmName = new System.Windows.Forms.TextBox();
      this.lblAlterAsm = new System.Windows.Forms.Label();
      this.chkAlterAsm = new System.Windows.Forms.CheckBox();
      this.chkDbConn = new System.Windows.Forms.CheckBox();
      this.lblConndb = new System.Windows.Forms.Label();
      this.lblConnstring = new System.Windows.Forms.Label();
      this.txtConnString = new System.Windows.Forms.TextBox();
      this.lblPermSet = new System.Windows.Forms.Label();
      this.cboPermSet = new System.Windows.Forms.ComboBox();
      this.chkUnchecked = new System.Windows.Forms.CheckBox();
      this.label1 = new System.Windows.Forms.Label();
      this.chkDropTable = new System.Windows.Forms.CheckBox();
      this.label2 = new System.Windows.Forms.Label();
      this.chkInfer = new System.Windows.Forms.CheckBox();
      this.label3 = new System.Windows.Forms.Label();
      this.hlpProv = new System.Windows.Forms.HelpProvider();
      this.txtProjFile = new System.Windows.Forms.TextBox();
      this.txtSqlProjFile = new System.Windows.Forms.TextBox();
      this.txtTypeFile = new System.Windows.Forms.TextBox();
      this.txtSourceExt = new System.Windows.Forms.TextBox();
      this.txtSourcePath = new System.Windows.Forms.TextBox();
      this.txtDbgSymbols = new System.Windows.Forms.TextBox();
      this.grpAsmSettings = new System.Windows.Forms.GroupBox();
      this.grpUDT = new System.Windows.Forms.GroupBox();
      this.pnlUDTCast = new System.Windows.Forms.Panel();
      this.cboUDTCast = new System.Windows.Forms.ComboBox();
      this.label10 = new System.Windows.Forms.Label();
      this.grpDb = new System.Windows.Forms.GroupBox();
      this.pnlConnString = new System.Windows.Forms.Panel();
      this.cboVersion = new System.Windows.Forms.ComboBox();
      this.label6 = new System.Windows.Forms.Label();
      this.btnDbBrowse = new System.Windows.Forms.Button();
      this.grpMisc = new System.Windows.Forms.GroupBox();
      this.pnlDbg = new System.Windows.Forms.Panel();
      this.btnBrowseDbg = new System.Windows.Forms.Button();
      this.label9 = new System.Windows.Forms.Label();
      this.label8 = new System.Windows.Forms.Label();
      this.chkDbgSymbols = new System.Windows.Forms.CheckBox();
      this.lblDepAttr = new System.Windows.Forms.Label();
      this.chkDepAttr = new System.Windows.Forms.CheckBox();
      this.grpSource = new System.Windows.Forms.GroupBox();
      this.pnlSrcPath = new System.Windows.Forms.Panel();
      this.btnBrowseSource = new System.Windows.Forms.Button();
      this.label7 = new System.Windows.Forms.Label();
      this.label11 = new System.Windows.Forms.Label();
      this.label5 = new System.Windows.Forms.Label();
      this.chkSource = new System.Windows.Forms.CheckBox();
      this.grpProjFiles = new System.Windows.Forms.GroupBox();
      this.pnlType = new System.Windows.Forms.Panel();
      this.label4 = new System.Windows.Forms.Label();
      this.btnBrowseType = new System.Windows.Forms.Button();
      this.pnlProj = new System.Windows.Forms.Panel();
      this.lblProjFile = new System.Windows.Forms.Label();
      this.btnBrowseProj = new System.Windows.Forms.Button();
      this.pnlSql = new System.Windows.Forms.Panel();
      this.btnBrowseSql = new System.Windows.Forms.Button();
      this.lblSqlProj = new System.Windows.Forms.Label();
      this.grpAsmSettings.SuspendLayout();
      this.grpUDT.SuspendLayout();
      this.pnlUDTCast.SuspendLayout();
      this.grpDb.SuspendLayout();
      this.pnlConnString.SuspendLayout();
      this.grpMisc.SuspendLayout();
      this.pnlDbg.SuspendLayout();
      this.grpSource.SuspendLayout();
      this.pnlSrcPath.SuspendLayout();
      this.grpProjFiles.SuspendLayout();
      this.pnlType.SuspendLayout();
      this.pnlProj.SuspendLayout();
      this.pnlSql.SuspendLayout();
      this.SuspendLayout();
      // 
      // lblAsmName
      // 
      this.lblAsmName.AutoSize = true;
      this.lblAsmName.Location = new System.Drawing.Point(56, 27);
      this.lblAsmName.Name = "lblAsmName";
      this.lblAsmName.Size = new System.Drawing.Size(82, 13);
      this.lblAsmName.TabIndex = 66;
      this.lblAsmName.Text = "Assembly Name";
      this.lblAsmName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // txtAsmName
      // 
      this.hlpProv.SetHelpString(this.txtAsmName, "This is help for assembly name");
      this.txtAsmName.Location = new System.Drawing.Point(159, 24);
      this.txtAsmName.Name = "txtAsmName";
      this.hlpProv.SetShowHelp(this.txtAsmName, true);
      this.txtAsmName.Size = new System.Drawing.Size(157, 20);
      this.txtAsmName.TabIndex = 8;
      // 
      // lblAlterAsm
      // 
      this.lblAlterAsm.AutoSize = true;
      this.lblAlterAsm.Location = new System.Drawing.Point(63, 51);
      this.lblAlterAsm.Name = "lblAlterAsm";
      this.lblAlterAsm.Size = new System.Drawing.Size(75, 13);
      this.lblAlterAsm.TabIndex = 890;
      this.lblAlterAsm.Text = "Alter Assembly";
      this.lblAlterAsm.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // chkAlterAsm
      // 
      this.chkAlterAsm.AutoSize = true;
      this.chkAlterAsm.Location = new System.Drawing.Point(158, 51);
      this.chkAlterAsm.Name = "chkAlterAsm";
      this.chkAlterAsm.Size = new System.Drawing.Size(15, 14);
      this.chkAlterAsm.TabIndex = 9;
      // 
      // chkDbConn
      // 
      this.chkDbConn.AutoSize = true;
      this.chkDbConn.Location = new System.Drawing.Point(158, 26);
      this.chkDbConn.Name = "chkDbConn";
      this.chkDbConn.Size = new System.Drawing.Size(15, 14);
      this.chkDbConn.TabIndex = 12;
      // 
      // lblConndb
      // 
      this.lblConndb.AutoSize = true;
      this.lblConndb.Location = new System.Drawing.Point(27, 28);
      this.lblConndb.Name = "lblConndb";
      this.lblConndb.Size = new System.Drawing.Size(112, 13);
      this.lblConndb.TabIndex = 78;
      this.lblConndb.Text = "Connect To Database";
      this.lblConndb.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // lblConnstring
      // 
      this.lblConnstring.AutoSize = true;
      this.lblConnstring.Location = new System.Drawing.Point(19, 40);
      this.lblConnstring.Name = "lblConnstring";
      this.lblConnstring.Size = new System.Drawing.Size(91, 13);
      this.lblConnstring.TabIndex = 777;
      this.lblConnstring.Text = "Connection String";
      this.lblConnstring.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // txtConnString
      // 
      this.txtConnString.Location = new System.Drawing.Point(130, 37);
      this.txtConnString.Name = "txtConnString";
      this.txtConnString.Size = new System.Drawing.Size(400, 20);
      this.txtConnString.TabIndex = 13;
      // 
      // lblPermSet
      // 
      this.lblPermSet.AutoSize = true;
      this.lblPermSet.Location = new System.Drawing.Point(67, 74);
      this.lblPermSet.Name = "lblPermSet";
      this.lblPermSet.Size = new System.Drawing.Size(71, 13);
      this.lblPermSet.TabIndex = 878;
      this.lblPermSet.Text = "Permissionset";
      this.lblPermSet.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // cboPermSet
      // 
      this.cboPermSet.FormattingEnabled = true;
      this.hlpProv.SetHelpString(this.cboPermSet, "This is help for combo");
      this.cboPermSet.Items.AddRange(new object[] {
            "SAFE",
            "EXTERNAL ACCESS",
            "UNSAFE"});
      this.cboPermSet.Location = new System.Drawing.Point(159, 71);
      this.cboPermSet.Name = "cboPermSet";
      this.hlpProv.SetShowHelp(this.cboPermSet, true);
      this.cboPermSet.Size = new System.Drawing.Size(157, 21);
      this.cboPermSet.TabIndex = 11;
      // 
      // chkUnchecked
      // 
      this.chkUnchecked.AutoSize = true;
      this.chkUnchecked.Location = new System.Drawing.Point(270, 51);
      this.chkUnchecked.Name = "chkUnchecked";
      this.chkUnchecked.Size = new System.Drawing.Size(15, 14);
      this.chkUnchecked.TabIndex = 10;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(179, 51);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(89, 13);
      this.label1.TabIndex = 106;
      this.label1.Text = "Unchecked Data";
      this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // chkDropTable
      // 
      this.chkDropTable.AutoSize = true;
      this.chkDropTable.Location = new System.Drawing.Point(140, 16);
      this.chkDropTable.Name = "chkDropTable";
      this.chkDropTable.Size = new System.Drawing.Size(15, 14);
      this.chkDropTable.TabIndex = 17;
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(59, 16);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(60, 13);
      this.label2.TabIndex = 1478;
      this.label2.Text = "Drop Table";
      this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // chkInfer
      // 
      this.chkInfer.AutoSize = true;
      this.chkInfer.Location = new System.Drawing.Point(158, 15);
      this.chkInfer.Name = "chkInfer";
      this.chkInfer.Size = new System.Drawing.Size(15, 14);
      this.chkInfer.TabIndex = 15;
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(66, 16);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(72, 13);
      this.label3.TabIndex = 125;
      this.label3.Text = "Infer Methods";
      this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // txtProjFile
      // 
      this.hlpProv.SetHelpString(this.txtProjFile, "This is help for assembly name");
      this.txtProjFile.Location = new System.Drawing.Point(149, 3);
      this.txtProjFile.Name = "txtProjFile";
      this.hlpProv.SetShowHelp(this.txtProjFile, true);
      this.txtProjFile.Size = new System.Drawing.Size(399, 20);
      this.txtProjFile.TabIndex = 4;
      // 
      // txtSqlProjFile
      // 
      this.hlpProv.SetHelpString(this.txtSqlProjFile, "This is help for assembly name");
      this.txtSqlProjFile.Location = new System.Drawing.Point(148, 7);
      this.txtSqlProjFile.Name = "txtSqlProjFile";
      this.hlpProv.SetShowHelp(this.txtSqlProjFile, true);
      this.txtSqlProjFile.Size = new System.Drawing.Size(400, 20);
      this.txtSqlProjFile.TabIndex = 2;
      // 
      // txtTypeFile
      // 
      this.hlpProv.SetHelpString(this.txtTypeFile, "This is help for assembly name");
      this.txtTypeFile.Location = new System.Drawing.Point(148, 3);
      this.txtTypeFile.Name = "txtTypeFile";
      this.hlpProv.SetShowHelp(this.txtTypeFile, true);
      this.txtTypeFile.Size = new System.Drawing.Size(400, 20);
      this.txtTypeFile.TabIndex = 6;
      // 
      // txtSourceExt
      // 
      this.hlpProv.SetHelpString(this.txtSourceExt, "This is help for assembly name");
      this.txtSourceExt.Location = new System.Drawing.Point(133, 4);
      this.txtSourceExt.Name = "txtSourceExt";
      this.hlpProv.SetShowHelp(this.txtSourceExt, true);
      this.txtSourceExt.Size = new System.Drawing.Size(29, 20);
      this.txtSourceExt.TabIndex = 20;
      // 
      // txtSourcePath
      // 
      this.hlpProv.SetHelpString(this.txtSourcePath, "This is help for assembly name");
      this.txtSourcePath.Location = new System.Drawing.Point(280, 4);
      this.txtSourcePath.Name = "txtSourcePath";
      this.hlpProv.SetShowHelp(this.txtSourcePath, true);
      this.txtSourcePath.Size = new System.Drawing.Size(256, 20);
      this.txtSourcePath.TabIndex = 21;
      // 
      // txtDbgSymbols
      // 
      this.hlpProv.SetHelpString(this.txtDbgSymbols, "This is help for assembly name");
      this.txtDbgSymbols.Location = new System.Drawing.Point(123, 3);
      this.txtDbgSymbols.Name = "txtDbgSymbols";
      this.hlpProv.SetShowHelp(this.txtDbgSymbols, true);
      this.txtDbgSymbols.Size = new System.Drawing.Size(256, 20);
      this.txtDbgSymbols.TabIndex = 24;
      // 
      // grpAsmSettings
      // 
      this.grpAsmSettings.Controls.Add(this.txtAsmName);
      this.grpAsmSettings.Controls.Add(this.lblAsmName);
      this.grpAsmSettings.Controls.Add(this.lblAlterAsm);
      this.grpAsmSettings.Controls.Add(this.chkAlterAsm);
      this.grpAsmSettings.Controls.Add(this.lblPermSet);
      this.grpAsmSettings.Controls.Add(this.chkUnchecked);
      this.grpAsmSettings.Controls.Add(this.cboPermSet);
      this.grpAsmSettings.Controls.Add(this.label1);
      this.grpAsmSettings.Location = new System.Drawing.Point(12, 143);
      this.grpAsmSettings.Name = "grpAsmSettings";
      this.grpAsmSettings.Size = new System.Drawing.Size(617, 98);
      this.grpAsmSettings.TabIndex = 124;
      this.grpAsmSettings.TabStop = false;
      this.grpAsmSettings.Text = "Assembly Settings";
      // 
      // grpUDT
      // 
      this.grpUDT.Controls.Add(this.pnlUDTCast);
      this.grpUDT.Controls.Add(this.label2);
      this.grpUDT.Controls.Add(this.chkDropTable);
      this.grpUDT.Location = new System.Drawing.Point(18, 31);
      this.grpUDT.Name = "grpUDT";
      this.grpUDT.Size = new System.Drawing.Size(593, 40);
      this.grpUDT.TabIndex = 105;
      this.grpUDT.TabStop = false;
      this.grpUDT.Text = "UDT\'s";
      // 
      // pnlUDTCast
      // 
      this.pnlUDTCast.Controls.Add(this.cboUDTCast);
      this.pnlUDTCast.Controls.Add(this.label10);
      this.pnlUDTCast.Location = new System.Drawing.Point(199, 8);
      this.pnlUDTCast.Name = "pnlUDTCast";
      this.pnlUDTCast.Size = new System.Drawing.Size(287, 28);
      this.pnlUDTCast.TabIndex = 17;
      // 
      // cboUDTCast
      // 
      this.cboUDTCast.FormattingEnabled = true;
      this.cboUDTCast.Items.AddRange(new object[] {
            "varchar (max)",
            "varbinary (max)"});
      this.cboUDTCast.Location = new System.Drawing.Point(88, 5);
      this.cboUDTCast.Name = "cboUDTCast";
      this.cboUDTCast.Size = new System.Drawing.Size(121, 21);
      this.cboUDTCast.TabIndex = 18;
      // 
      // label10
      // 
      this.label10.AutoSize = true;
      this.label10.Location = new System.Drawing.Point(10, 8);
      this.label10.Name = "label10";
      this.label10.Size = new System.Drawing.Size(70, 13);
      this.label10.TabIndex = 1049;
      this.label10.Text = "Cast UDT To";
      this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // grpDb
      // 
      this.grpDb.Controls.Add(this.pnlConnString);
      this.grpDb.Controls.Add(this.lblConndb);
      this.grpDb.Controls.Add(this.chkDbConn);
      this.grpDb.Location = new System.Drawing.Point(12, 247);
      this.grpDb.Name = "grpDb";
      this.grpDb.Size = new System.Drawing.Size(617, 112);
      this.grpDb.TabIndex = 98;
      this.grpDb.TabStop = false;
      this.grpDb.Text = "Database Settings";
      // 
      // pnlConnString
      // 
      this.pnlConnString.Controls.Add(this.cboVersion);
      this.pnlConnString.Controls.Add(this.label6);
      this.pnlConnString.Controls.Add(this.txtConnString);
      this.pnlConnString.Controls.Add(this.btnDbBrowse);
      this.pnlConnString.Controls.Add(this.lblConnstring);
      this.pnlConnString.Location = new System.Drawing.Point(28, 46);
      this.pnlConnString.Name = "pnlConnString";
      this.pnlConnString.Size = new System.Drawing.Size(583, 60);
      this.pnlConnString.TabIndex = 8888;
      // 
      // cboVersion
      // 
      this.cboVersion.FormattingEnabled = true;
      this.cboVersion.Items.AddRange(new object[] {
            2005,
            2008});
      this.cboVersion.Location = new System.Drawing.Point(131, 9);
      this.cboVersion.Name = "cboVersion";
      this.cboVersion.Size = new System.Drawing.Size(157, 21);
      this.cboVersion.TabIndex = 13;
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(9, 12);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(100, 13);
      this.label6.TabIndex = 778;
      this.label6.Text = "SQL Server Version";
      this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // btnDbBrowse
      // 
      this.btnDbBrowse.Location = new System.Drawing.Point(541, 35);
      this.btnDbBrowse.Name = "btnDbBrowse";
      this.btnDbBrowse.Size = new System.Drawing.Size(35, 23);
      this.btnDbBrowse.TabIndex = 14;
      this.btnDbBrowse.Text = "...";
      // 
      // grpMisc
      // 
      this.grpMisc.Controls.Add(this.grpUDT);
      this.grpMisc.Controls.Add(this.pnlDbg);
      this.grpMisc.Controls.Add(this.label8);
      this.grpMisc.Controls.Add(this.chkDbgSymbols);
      this.grpMisc.Controls.Add(this.lblDepAttr);
      this.grpMisc.Controls.Add(this.chkDepAttr);
      this.grpMisc.Controls.Add(this.grpSource);
      this.grpMisc.Controls.Add(this.label3);
      this.grpMisc.Controls.Add(this.chkInfer);
      this.grpMisc.Location = new System.Drawing.Point(12, 365);
      this.grpMisc.Name = "grpMisc";
      this.grpMisc.Size = new System.Drawing.Size(617, 176);
      this.grpMisc.TabIndex = 1890;
      this.grpMisc.TabStop = false;
      this.grpMisc.Text = "Misc. Settings";
      // 
      // pnlDbg
      // 
      this.pnlDbg.Controls.Add(this.btnBrowseDbg);
      this.pnlDbg.Controls.Add(this.txtDbgSymbols);
      this.pnlDbg.Controls.Add(this.label9);
      this.pnlDbg.Location = new System.Drawing.Point(182, 138);
      this.pnlDbg.Name = "pnlDbg";
      this.pnlDbg.Size = new System.Drawing.Size(429, 30);
      this.pnlDbg.TabIndex = 102;
      // 
      // btnBrowseDbg
      // 
      this.btnBrowseDbg.Location = new System.Drawing.Point(383, 3);
      this.btnBrowseDbg.Name = "btnBrowseDbg";
      this.btnBrowseDbg.Size = new System.Drawing.Size(36, 23);
      this.btnBrowseDbg.TabIndex = 25;
      this.btnBrowseDbg.Text = "...";
      // 
      // label9
      // 
      this.label9.AutoSize = true;
      this.label9.Location = new System.Drawing.Point(32, 7);
      this.label9.Name = "label9";
      this.label9.Size = new System.Drawing.Size(83, 13);
      this.label9.TabIndex = 2356;
      this.label9.Text = "Debug File Path";
      this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // label8
      // 
      this.label8.AutoSize = true;
      this.label8.Location = new System.Drawing.Point(22, 145);
      this.label8.Name = "label8";
      this.label8.Size = new System.Drawing.Size(118, 13);
      this.label8.TabIndex = 100;
      this.label8.Text = "Upload Debug Symbols";
      this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // chkDbgSymbols
      // 
      this.chkDbgSymbols.AutoSize = true;
      this.chkDbgSymbols.Location = new System.Drawing.Point(158, 145);
      this.chkDbgSymbols.Name = "chkDbgSymbols";
      this.chkDbgSymbols.Size = new System.Drawing.Size(15, 14);
      this.chkDbgSymbols.TabIndex = 23;
      // 
      // lblDepAttr
      // 
      this.lblDepAttr.AutoSize = true;
      this.lblDepAttr.Location = new System.Drawing.Point(184, 15);
      this.lblDepAttr.Name = "lblDepAttr";
      this.lblDepAttr.Size = new System.Drawing.Size(113, 13);
      this.lblDepAttr.TabIndex = 99;
      this.lblDepAttr.Text = "Upload deployattribute";
      this.lblDepAttr.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // chkDepAttr
      // 
      this.chkDepAttr.AutoSize = true;
      this.chkDepAttr.Location = new System.Drawing.Point(305, 16);
      this.chkDepAttr.Name = "chkDepAttr";
      this.chkDepAttr.Size = new System.Drawing.Size(15, 14);
      this.chkDepAttr.TabIndex = 16;
      // 
      // grpSource
      // 
      this.grpSource.Controls.Add(this.pnlSrcPath);
      this.grpSource.Controls.Add(this.label5);
      this.grpSource.Controls.Add(this.chkSource);
      this.grpSource.Location = new System.Drawing.Point(18, 73);
      this.grpSource.Name = "grpSource";
      this.grpSource.Size = new System.Drawing.Size(593, 63);
      this.grpSource.TabIndex = 1867;
      this.grpSource.TabStop = false;
      this.grpSource.Text = "Source Files";
      // 
      // pnlSrcPath
      // 
      this.pnlSrcPath.Controls.Add(this.btnBrowseSource);
      this.pnlSrcPath.Controls.Add(this.txtSourceExt);
      this.pnlSrcPath.Controls.Add(this.txtSourcePath);
      this.pnlSrcPath.Controls.Add(this.label7);
      this.pnlSrcPath.Controls.Add(this.label11);
      this.pnlSrcPath.Location = new System.Drawing.Point(7, 31);
      this.pnlSrcPath.Name = "pnlSrcPath";
      this.pnlSrcPath.Size = new System.Drawing.Size(582, 28);
      this.pnlSrcPath.TabIndex = 2389;
      // 
      // btnBrowseSource
      // 
      this.btnBrowseSource.Location = new System.Drawing.Point(541, 1);
      this.btnBrowseSource.Name = "btnBrowseSource";
      this.btnBrowseSource.Size = new System.Drawing.Size(36, 23);
      this.btnBrowseSource.TabIndex = 22;
      this.btnBrowseSource.Text = "...";
      this.btnBrowseSource.Click += new System.EventHandler(this.btnBrowseSource_Click);
      // 
      // label7
      // 
      this.label7.AutoSize = true;
      this.label7.Location = new System.Drawing.Point(169, 6);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(105, 13);
      this.label7.TabIndex = 235;
      this.label7.Text = "Source File Directory";
      this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // label11
      // 
      this.label11.AutoSize = true;
      this.label11.Location = new System.Drawing.Point(34, 6);
      this.label11.Name = "label11";
      this.label11.Size = new System.Drawing.Size(81, 13);
      this.label11.TabIndex = 186;
      this.label11.Text = "Source File Ext.";
      this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(17, 15);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(102, 13);
      this.label5.TabIndex = 166;
      this.label5.Text = "Upload Source Files";
      this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // chkSource
      // 
      this.chkSource.AutoSize = true;
      this.chkSource.Location = new System.Drawing.Point(140, 14);
      this.chkSource.Name = "chkSource";
      this.chkSource.Size = new System.Drawing.Size(15, 14);
      this.chkSource.TabIndex = 19;
      // 
      // grpProjFiles
      // 
      this.grpProjFiles.Controls.Add(this.pnlType);
      this.grpProjFiles.Controls.Add(this.pnlProj);
      this.grpProjFiles.Controls.Add(this.pnlSql);
      this.grpProjFiles.Location = new System.Drawing.Point(12, 13);
      this.grpProjFiles.Name = "grpProjFiles";
      this.grpProjFiles.Size = new System.Drawing.Size(617, 124);
      this.grpProjFiles.TabIndex = 922;
      this.grpProjFiles.TabStop = false;
      this.grpProjFiles.Text = "Project/Assembly Files";
      // 
      // pnlType
      // 
      this.pnlType.Controls.Add(this.txtTypeFile);
      this.pnlType.Controls.Add(this.label4);
      this.pnlType.Controls.Add(this.btnBrowseType);
      this.pnlType.Location = new System.Drawing.Point(10, 91);
      this.pnlType.Name = "pnlType";
      this.pnlType.Size = new System.Drawing.Size(594, 27);
      this.pnlType.TabIndex = 107;
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(21, 6);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(106, 13);
      this.label4.TabIndex = 113;
      this.label4.Text = "Type Conversion File";
      this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // btnBrowseType
      // 
      this.btnBrowseType.Location = new System.Drawing.Point(554, 3);
      this.btnBrowseType.Name = "btnBrowseType";
      this.btnBrowseType.Size = new System.Drawing.Size(36, 23);
      this.btnBrowseType.TabIndex = 7;
      this.btnBrowseType.Text = "...";
      // 
      // pnlProj
      // 
      this.pnlProj.Controls.Add(this.txtProjFile);
      this.pnlProj.Controls.Add(this.lblProjFile);
      this.pnlProj.Controls.Add(this.btnBrowseProj);
      this.pnlProj.Location = new System.Drawing.Point(10, 58);
      this.pnlProj.Name = "pnlProj";
      this.pnlProj.Size = new System.Drawing.Size(594, 27);
      this.pnlProj.TabIndex = 909;
      // 
      // lblProjFile
      // 
      this.lblProjFile.AutoSize = true;
      this.lblProjFile.Location = new System.Drawing.Point(21, 6);
      this.lblProjFile.Name = "lblProjFile";
      this.lblProjFile.Size = new System.Drawing.Size(108, 13);
      this.lblProjFile.TabIndex = 488;
      this.lblProjFile.Text = "Assembly/Project File";
      this.lblProjFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // btnBrowseProj
      // 
      this.btnBrowseProj.Location = new System.Drawing.Point(554, 4);
      this.btnBrowseProj.Name = "btnBrowseProj";
      this.btnBrowseProj.Size = new System.Drawing.Size(36, 23);
      this.btnBrowseProj.TabIndex = 5;
      this.btnBrowseProj.Text = "...";
      // 
      // pnlSql
      // 
      this.pnlSql.Controls.Add(this.btnBrowseSql);
      this.pnlSql.Controls.Add(this.txtSqlProjFile);
      this.pnlSql.Controls.Add(this.lblSqlProj);
      this.pnlSql.Location = new System.Drawing.Point(10, 19);
      this.pnlSql.Name = "pnlSql";
      this.pnlSql.Size = new System.Drawing.Size(594, 33);
      this.pnlSql.TabIndex = 888;
      // 
      // btnBrowseSql
      // 
      this.btnBrowseSql.Location = new System.Drawing.Point(554, 7);
      this.btnBrowseSql.Name = "btnBrowseSql";
      this.btnBrowseSql.Size = new System.Drawing.Size(36, 23);
      this.btnBrowseSql.TabIndex = 3;
      this.btnBrowseSql.Text = "...";
      // 
      // lblSqlProj
      // 
      this.lblSqlProj.AutoSize = true;
      this.lblSqlProj.Location = new System.Drawing.Point(5, 10);
      this.lblSqlProj.Name = "lblSqlProj";
      this.lblSqlProj.Size = new System.Drawing.Size(123, 13);
      this.lblSqlProj.TabIndex = 887;
      this.lblSqlProj.Text = "Deployment Settings File";
      this.lblSqlProj.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // DepProp
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.grpProjFiles);
      this.Controls.Add(this.grpMisc);
      this.Controls.Add(this.grpDb);
      this.Controls.Add(this.grpAsmSettings);
      this.Name = "DepProp";
      this.Size = new System.Drawing.Size(637, 545);
      this.grpAsmSettings.ResumeLayout(false);
      this.grpAsmSettings.PerformLayout();
      this.grpUDT.ResumeLayout(false);
      this.grpUDT.PerformLayout();
      this.pnlUDTCast.ResumeLayout(false);
      this.pnlUDTCast.PerformLayout();
      this.grpDb.ResumeLayout(false);
      this.grpDb.PerformLayout();
      this.pnlConnString.ResumeLayout(false);
      this.pnlConnString.PerformLayout();
      this.grpMisc.ResumeLayout(false);
      this.grpMisc.PerformLayout();
      this.pnlDbg.ResumeLayout(false);
      this.pnlDbg.PerformLayout();
      this.grpSource.ResumeLayout(false);
      this.grpSource.PerformLayout();
      this.pnlSrcPath.ResumeLayout(false);
      this.pnlSrcPath.PerformLayout();
      this.grpProjFiles.ResumeLayout(false);
      this.pnlType.ResumeLayout(false);
      this.pnlType.PerformLayout();
      this.pnlProj.ResumeLayout(false);
      this.pnlProj.PerformLayout();
      this.pnlSql.ResumeLayout(false);
      this.pnlSql.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Label lblAsmName;
    private System.Windows.Forms.TextBox txtAsmName;
    private System.Windows.Forms.Label lblAlterAsm;
    private System.Windows.Forms.CheckBox chkAlterAsm;
    private System.Windows.Forms.CheckBox chkDbConn;
    private System.Windows.Forms.Label lblConndb;
    private System.Windows.Forms.Label lblConnstring;
    private System.Windows.Forms.TextBox txtConnString;
    private System.Windows.Forms.Label lblPermSet;
    private System.Windows.Forms.ComboBox cboPermSet;
    private System.Windows.Forms.CheckBox chkUnchecked;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.CheckBox chkDropTable;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.CheckBox chkInfer;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.HelpProvider hlpProv;
    private System.Windows.Forms.GroupBox grpAsmSettings;
    private System.Windows.Forms.GroupBox grpDb;
    private System.Windows.Forms.GroupBox grpMisc;
    private System.Windows.Forms.GroupBox grpProjFiles;
    private System.Windows.Forms.TextBox txtProjFile;
    private System.Windows.Forms.Label lblProjFile;
    private System.Windows.Forms.Button btnBrowseProj;
    private System.Windows.Forms.Button btnDbBrowse;
    private System.Windows.Forms.Panel pnlConnString;
    private System.Windows.Forms.Panel pnlProj;
    private System.Windows.Forms.Panel pnlSql;
    private System.Windows.Forms.Button btnBrowseSql;
    private System.Windows.Forms.TextBox txtSqlProjFile;
    private System.Windows.Forms.Label lblSqlProj;
    private System.Windows.Forms.Panel pnlType;
    private System.Windows.Forms.TextBox txtTypeFile;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Button btnBrowseType;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.CheckBox chkSource;
    private System.Windows.Forms.GroupBox grpSource;
    private System.Windows.Forms.TextBox txtSourceExt;
    private System.Windows.Forms.Panel pnlSrcPath;
    private System.Windows.Forms.Button btnBrowseSource;
    private System.Windows.Forms.TextBox txtSourcePath;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.Label lblDepAttr;
    private System.Windows.Forms.CheckBox chkDepAttr;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.CheckBox chkDbgSymbols;
    private System.Windows.Forms.Panel pnlDbg;
    private System.Windows.Forms.Button btnBrowseDbg;
    private System.Windows.Forms.TextBox txtDbgSymbols;
    private System.Windows.Forms.Label label9;
    private System.Windows.Forms.Label label10;
    private System.Windows.Forms.ComboBox cboUDTCast;
    private System.Windows.Forms.GroupBox grpUDT;
    private System.Windows.Forms.Panel pnlUDTCast;
    private System.Windows.Forms.Label label11;
    private System.Windows.Forms.ComboBox cboVersion;
    private System.Windows.Forms.Label label6;
    
  }
}
