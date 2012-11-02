using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Resources;
using System.Data.Common;
using System.Xml;
using System.IO;
using System.Reflection;
using Microsoft.Build.BuildEngine;
using System.Diagnostics;

namespace DM.Build.Yukon.DeployProperties {
  public partial class PropertiesForm : Form {

    #region fields
    bool isDirty = false; //indicates whether any changes of the data has taken place
    XmlDataDocument doc; 
    DataSet dsSqlProj = new DataSet();
    string sqlProjFile = "";
    string vsProjPath = "";
    string vsProjectFile = "";
    string appPath = "";
    bool firstFind = true;
    viewlog vForm = null;

    string binPath = ""; //variable which says where the runtime is loaded from.

    DeployTypeEnum dpType = DeployTypeEnum.Project;

    OpenFileDialog fd;

    const string sqlFile = "The deployment settings file, (sql.proj), can not be found. You will now be presented with a dialog to 'Browse' for it.\n\nIf this is a project without a settings file, use the default 'sql.proj' file. This can be found in the 'DeploymentProperties' directory.";
    const string projFile = "The project file for your project, (*.csproj|*.vbproj), can not be found. You will now be presented with a dialog to 'Browse' for it.";
    const string typeErrFile = "The type conversion file for your project, (typeconversion.xml), can not be found. You will now be presented with a dialog to 'Browse' for it.";

    ToolStripMenuItem mnuTopFile;
    ToolStripMenuItem mnuTopDeploy;

    ToolStripMenuItem mnuSubSave;
    ToolStripMenuItem mnuSubExit;
    ToolStripMenuItem mnuSubDeployAll;
    ToolStripMenuItem mnuSubDeployAsm;
    ToolStripMenuItem mnuSubDeployUDT;
    ToolStripMenuItem mnuSubDeployMeth;
    ToolStripMenuItem mnuSubDropAsm;
    
    ToolStripSeparator mnuSubSep;
    ToolStripSeparator mnuSubSep2;


#endregion

    #region constructor
    public PropertiesForm(string projPath, string execPath) {

      if (projPath == "") {
        dpType = DeployTypeEnum.Assembly;
        
      }

      else {
        sqlProjFile = Path.Combine(projPath, "sql.proj");
        vsProjPath = projPath;
      }
      appPath = execPath;
      InitializeComponent();
                    
      SetEventHooks();
      SetUpHelp();
      SetControlState();
      LoadDocs();
      if (dpType == DeployTypeEnum.Assembly) {
        menuStrip1.Visible = true;
        SetupMenus();
        SetUpMenuEvents();
        
      }
      else
        menuStrip1.Visible = false;
        
      //make sure we are set to normal
      this.AutoScroll = true;
      
      
      binPath = System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory();
      isDirty = false;

    }
    #endregion

    #region methods

    void SetupMenus() {
      mnuTopFile = new ToolStripMenuItem();
      mnuTopFile.Name = "mnuTopFile";
      mnuTopFile.Text = "&File";

      mnuTopDeploy = new ToolStripMenuItem();
      mnuTopDeploy.Name = "mnuTopDeploy";
      mnuTopDeploy.Text = "&Deploy";
            
      mnuSubSave = new ToolStripMenuItem();
      mnuSubSave.Name = "mnuSubSave";
      mnuSubSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
      mnuSubSave.Text = "Save";

      mnuSubExit = new ToolStripMenuItem();
      mnuSubExit.Name = "mnuSubExit";
      mnuSubExit.Text = "E&xit";

      mnuSubDeployAll = new ToolStripMenuItem();
      mnuSubDeployAll.Name = "mnuSubDeployAll";
      mnuSubDeployAll.Text = "Deploy All";

      mnuSubDeployAsm = new ToolStripMenuItem();
      mnuSubDeployAsm.Name = "mnuSubDeployAsm";
      mnuSubDeployAsm.Text = "Deploy Assembly";

      mnuSubDeployUDT = new ToolStripMenuItem();
      mnuSubDeployUDT.Name = "mnuSubDeployUDT";
      mnuSubDeployUDT.Text = "Deploy UDT's";

      mnuSubDeployMeth = new ToolStripMenuItem();
      mnuSubDeployMeth.Name = "mnuSubDeployMeth";
      mnuSubDeployMeth.Text = "Deploy Methods";

      mnuSubDropAsm = new ToolStripMenuItem();
      mnuSubDropAsm.Name = "mnuSubDeployMeth";
      mnuSubDropAsm.Text = "Drop Assembly";

      
      mnuSubSep = new ToolStripSeparator();
      mnuSubSep2 = new ToolStripSeparator();

      mnuTopFile.DropDownItems.AddRange(new ToolStripItem[] { mnuSubSave, mnuSubSep, mnuSubExit });
      mnuTopDeploy.DropDownItems.AddRange(new ToolStripItem[] { mnuSubDeployAll, mnuSubDeployAsm, mnuSubDeployUDT, mnuSubDeployMeth, mnuSubSep2, mnuSubDropAsm });

      menuStrip1.Items.AddRange(new ToolStripItem[] { mnuTopFile, mnuTopDeploy});

      
    }

    void SetUpMenuEvents() {
      mnuSubSave.Click += new EventHandler(mnuSubSave_Click);
      mnuSubExit.Click += new EventHandler(mnuSubExit_Click);
      mnuSubDeployAll.Click += new EventHandler(mnuSubDeployAll_Click);
      mnuSubDeployAsm.Click += new EventHandler(mnuSubDeployAsm_Click);
      mnuSubDeployUDT.Click += new EventHandler(mnuSubDeployUDT_Click);
      mnuSubDeployMeth.Click += new EventHandler(mnuSubDeployMeth_Click);
      mnuSubDropAsm.Click += new EventHandler(mnuSubDropAsm_Click);

    }

    

    void SetControlState() {
      
      grpProjFiles.Enabled = false;
      chkAlterAsm.Checked = false;
      chkDbConn.Checked = false;
      chkDepAttr.Checked = true;
      txtSqlProjFile.Text = "";
      txtProjFile.Text = "";
      txtTypeFile.Text = "typeconversion.xml";
      grpAsmSettings.Enabled = false;
      grpDb.Enabled = false;
      grpMisc.Enabled = false;
      grpProjFiles.Enabled = false;


    }

    void SetUpDataBinding() {
      Binding b = new Binding("Text", dsSqlProj, "PropertyGroup.Assemblypath");

      txtAsmName.DataBindings.Add(new Binding("Text", dsSqlProj, "PropertyGroup.Assemblyname"));
      txtAsmName.DataBindings[0].DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;

      chkAlterAsm.DataBindings.Add(new Binding("Checked", dsSqlProj, "PropertyGroup.Alterassembly"));
      chkAlterAsm.DataBindings[0].DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;

      chkUnchecked.DataBindings.Add(new Binding("Checked", dsSqlProj, "PropertyGroup.Uncheckeddata"));
      chkUnchecked.DataBindings[0].DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;

      cboPermSet.DataBindings.Add(new Binding("SelectedIndex", dsSqlProj, "PropertyGroup.Permissionset"));
      cboPermSet.DataBindings[0].DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;

      chkDbConn.DataBindings.Add(new Binding("Checked", dsSqlProj,"PropertyGroup.ConnectDatabase"));
      chkDbConn.DataBindings[0].DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;

      txtConnString.DataBindings.Add(new Binding("Text", dsSqlProj, "PropertyGroup.Connectionstring"));
      txtConnString.DataBindings[0].DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;

      chkInfer.DataBindings.Add(new Binding("Checked", dsSqlProj, "PropertyGroup.Infermethods"));
      chkInfer.DataBindings[0].DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;

      chkDropTable.DataBindings.Add(new Binding("Checked", dsSqlProj, "PropertyGroup.DropTable"));
      chkDropTable.DataBindings[0].DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;

      cboUDTCast.DataBindings.Add(new Binding("SelectedIndex", dsSqlProj, "PropertyGroup.Castudtcolto"));
      cboUDTCast.DataBindings[0].DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;

      chkSource.DataBindings.Add(new Binding("Checked", dsSqlProj, "PropertyGroup.Uploadsource"));
      chkSource.DataBindings[0].DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;

      txtSourceExt.DataBindings.Add(new Binding("Text", dsSqlProj, "PropertyGroup.Sourceextension"));
      txtSourceExt.DataBindings[0].DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;

      txtSourcePath.DataBindings.Add(new Binding("Text", dsSqlProj, "PropertyGroup.Sourcepath"));
      txtSourcePath.DataBindings[0].DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;

      chkDepAttr.DataBindings.Add(new Binding("Checked", dsSqlProj, "PropertyGroup.Usedeployattributes"));
      chkDepAttr.DataBindings[0].DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;

      chkDbgSymbols.DataBindings.Add(new Binding("Checked", dsSqlProj, "PropertyGroup.Deploydbgsymbols"));
      chkDbgSymbols.DataBindings[0].DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;

      txtDbgSymbols.DataBindings.Add(new Binding("Text", dsSqlProj, "PropertyGroup.Debugpath"));
      txtDbgSymbols.DataBindings[0].DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;

      cboVersion.DataBindings.Add(new Binding("SelectedIndex", dsSqlProj, "PropertyGroup.Serverversion"));
      cboVersion.DataBindings[0].DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
      
      
      if (Path.GetExtension(vsProjectFile) == ".dll") {
        txtProjFile.DataBindings.Add(b);
        txtProjFile.DataBindings[0].DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;

      }
      
     
      isDirty = false;
    }

    /// <summary>
    /// Reads in the SQL Project settings file, and hooks the data to the control.
    /// </summary>
    /// <returns>boolean</returns>
    bool LoadDocs() {
           
      isDirty = false;
      try {

        grpProjFiles.Enabled = true;

        //if dpType = Project only enable the SQL Proj file panel
        //ptherwise enable both proj as well as assembly
        if (dpType == DeployTypeEnum.Project) {
          //check for the sql project file and try to load it
          if (LoadSql()) {
            pnlSql.Enabled = false;
            pnlProj.Enabled = true;
            vsProjectFile = dsSqlProj.Tables["Import"].Rows[0]["Project"].ToString();
            if (LoadProj()) {
              pnlProj.Enabled = false;
              pnlType.Enabled = true;
              if (dsSqlProj.Tables["Import"].Rows[0]["Project"].ToString().Contains("$safeprojectname$")) {
                dsSqlProj.Tables["Import"].Rows[0]["Project"] = vsProjectFile;
              }
              SetProjectNameAndAsm();
              LoadTypeFile();
            }
            //hook up the data
            SetUpDataBinding();

            ////finally set control state
            grpAsmSettings.Enabled = true;
            grpDb.Enabled = true;
            grpMisc.Enabled = true;
            pnlSrcPath.Enabled = chkSource.Checked;
            pnlDbg.Enabled = chkDbgSymbols.Checked;

          }
        }
        else if (dpType == DeployTypeEnum.Assembly) {
          MessageBox.Show("It seems you are running this application in stand-alone mode.\n\nIf this is the case, 'Browse' for the compiled assembly (dll), or a project-file (*.csproj|*.vbproj) that you want to deploy.","Stand Alone");
          pnlSql.Enabled = false;
          pnlProj.Enabled = true;
          pnlType.Enabled = false;
                  
        }

          
      }
      
      catch (Exception e) {
        MessageBox.Show("An unexpected error happened. The error is: " + e.Message + ".", "Unexpected Error");
        grpProjFiles.Enabled = true;
        isDirty = false;
        return false;
      }

      return true;

    }

    private void SetProjectNameAndAsm() {
      string asmName = "";
      //change the Assemblyname property to from the project file
      //if it is the $(AssemblyName)
      asmName = dsSqlProj.Tables["PropertyGroup"].Rows[0]["Assemblyname"].ToString();
      if (asmName == "$(AssemblyName)" || asmName == "") {
        asmName = "";
        if (Path.GetExtension(vsProjectFile) != ".dll") {
          DataSet projDs = new DataSet();
          projDs.ReadXml(vsProjectFile, XmlReadMode.InferSchema);
          asmName = projDs.Tables["PropertyGroup"].Rows[0]["AssemblyName"].ToString();
        }
        //we're going against a compiled dll - read off the metadata
        else {
          Assembly a = Assembly.LoadFile(vsProjectFile);
          AssemblyName am = a.GetName();
          asmName = am.Name;
        }
        dsSqlProj.Tables["PropertyGroup"].Rows[0]["Assemblyname"] = asmName;
      }
    }

    private void LoadDocsForAsm(FileTypeEnum fte) {
      bool success = false;
      isDirty = false;
      try {

        // so we are checking for assembly first
        if (dpType == DeployTypeEnum.Assembly) {
          //we have started with an assembly or project file
          if (fte == FileTypeEnum.Project) {
            firstFind = false;
            pnlSql.Enabled = true;
            if (LoadSql()) {
              success = true;
              //if we're pointing to a dll (instead of a project file)

              if (Path.GetExtension(vsProjectFile) == ".dll") {
                if (dsSqlProj.Tables.Contains("Import"))
                  dsSqlProj.Tables.Remove(dsSqlProj.Tables["Import"]);
                dsSqlProj.Tables["PropertyGroup"].Rows[0]["Assemblypath"] = vsProjectFile;
                dsSqlProj.Tables["PropertyGroup"].Rows[0]["Debugpath"] = Path.ChangeExtension(vsProjectFile, ".pdb");
              }
              else {//we're pointing to a project file
                if (dsSqlProj.Tables["Import"].Rows[0]["Project"].ToString().Contains("$safeprojectname$")) {
                  dsSqlProj.Tables["Import"].Rows[0]["Project"] = vsProjectFile;
                }
              }
            }
            
          }
            //we have loaded an already existing sql.proj file
          else if (fte == FileTypeEnum.Sql) {
            ReadIntoDatasetSqlFile();
            if (dsSqlProj.Tables.Contains("Import")) {
              vsProjectFile = dsSqlProj.Tables["Import"].Rows[0]["Project"].ToString();
            }
            else
              vsProjectFile = dsSqlProj.Tables["PropertyGroup"].Rows[0]["Assemblypath"].ToString();

            if (LoadProj()) {
              success = true;
            }

          }
        }

        if (success) {
          SetProjectNameAndAsm();
          LoadTypeFile();
          pnlType.Enabled = true;

          //hook up the data
          SetUpDataBinding();

          ////finally set control state
          grpAsmSettings.Enabled = true;
          grpDb.Enabled = true;
          grpMisc.Enabled = true;
          pnlSrcPath.Enabled = chkSource.Checked;
          pnlDbg.Enabled = chkDbgSymbols.Checked;

        }

       


      }

      catch(Exception e) {
        MessageBox.Show("An unexpected error happened. The error is: " + e.Message + ".", "Unexpected Error");
        grpProjFiles.Enabled = true;
        isDirty = false;
      
      }

    }

    private void LoadTypeFile() {
      if (FindFile(FileTypeEnum.Xml)) {
        pnlType.Enabled = false;
      }
      
    }

    private bool LoadProj() {
      try {
        if (FindFile(FileTypeEnum.Project)) {
          
          return true;
        }

        return false;
      }

      catch { return false; }
      
      
    }

    private bool LoadSql() {
      try {
        if (FindFile(FileTypeEnum.Sql)) {
          //load the sql.proj file
          ReadIntoDatasetSqlFile();
          return true;
        }
        return false;
      }
      catch { return false; }
    }

    private bool FindFile(FileTypeEnum fte) {
      bool exists = false;
      bool ret = false;
      string searchFile = "";
      string errMsg = "";

      if (fte == FileTypeEnum.Sql) {
        //MessageBox.Show(vsProjPath);
        errMsg = sqlFile;
        if (dpType == DeployTypeEnum.Assembly && sqlProjFile == "") {
          if (firstFind)
            sqlProjFile = "";
          else
            sqlProjFile = Path.Combine(vsProjPath, "sql.proj");
        }

        searchFile =sqlProjFile;
      }
      else if (fte == FileTypeEnum.Project) {
        errMsg = projFile;
        if (vsProjectFile != "" && !vsProjectFile.Contains("$safeprojectname$"))
          searchFile = Path.Combine(vsProjPath, vsProjectFile);
        else if (vsProjectFile.Contains("$safeprojectname$"))
          searchFile = "";
        else
          searchFile = vsProjectFile;

      }

      else if (fte == FileTypeEnum.Xml) {
        errMsg = typeErrFile;
        if (vsProjectFile != "")
          searchFile = Path.Combine(vsProjPath, "typeconversion.xml");
        else
          searchFile = "";

      }

      //check for the file and if not there browse for it
      if (searchFile == "" || !File.Exists(searchFile)) {
        MessageBox.Show(errMsg, "File Not Found");
        searchFile = "";
        exists = SelectFile(fte, out searchFile);

      }
      else
        exists = true;
      
      //if the file is there, set the properties
      if (exists) {
        
        if (fte == FileTypeEnum.Sql) {
          sqlProjFile = searchFile;
          txtSqlProjFile.Text = sqlProjFile;
          if (vsProjPath == "")
            vsProjPath = Path.GetDirectoryName(sqlProjFile);
        }

        else if (fte == FileTypeEnum.Project) {
          vsProjectFile = searchFile;
          txtProjFile.Text = vsProjectFile;
        }

        else if (fte == FileTypeEnum.Xml) {
          dsSqlProj.Tables["PropertyGroup"].Rows[0]["TypeConversionFilePath"] = searchFile;
          txtTypeFile.Text = searchFile;
        }
        ret = true;

      }

     
      return ret;
    }

    void ReadIntoDatasetSqlFile()
    {
      dsSqlProj.ReadXml(sqlProjFile);

      //check that the sql.proj file is the latest
      bool toReload = Utility.VerifySqlProjFile(dsSqlProj, sqlProjFile);

      if (toReload)
      {
        dsSqlProj.Clear();
        dsSqlProj.Dispose();
        dsSqlProj = new DataSet();
        dsSqlProj.ReadXml(sqlProjFile);
      }

    }

    /// <summary>
    /// Setting up the "tool-tip" help
    /// </summary>
    void SetUpHelp() {
      ResourceManager rm = new ResourceManager("DM.Build.Yukon.DeployProperties.StringResources", typeof(PropertiesForm).Assembly);
      hlpProv.SetShowHelp(txtSqlProjFile, true);
      hlpProv.SetHelpString(txtSqlProjFile, rm.GetString("proj_Sql_Txt"));

      hlpProv.SetShowHelp(txtProjFile, true);
      hlpProv.SetHelpString(txtProjFile, rm.GetString("proj_Project_Txt"));
      
      hlpProv.SetShowHelp(txtAsmName, true);
      hlpProv.SetHelpString(txtAsmName, rm.GetString("asm_Name_Txt"));

      hlpProv.SetShowHelp(chkAlterAsm, true);
      hlpProv.SetHelpString(chkAlterAsm, rm.GetString("asm_Alter_Txt"));

      hlpProv.SetShowHelp(chkUnchecked, true);
      hlpProv.SetHelpString(chkUnchecked, rm.GetString("asm_Unchecked_Txt"));

      hlpProv.SetShowHelp(cboPermSet, true);
      hlpProv.SetHelpString(cboPermSet, rm.GetString("asm_Perm_Txt"));

      hlpProv.SetShowHelp(chkDropTable, true);
      hlpProv.SetHelpString(chkDropTable, rm.GetString("asm_Drop_Txt"));

      hlpProv.SetShowHelp(chkDbConn, true);
      hlpProv.SetHelpString(chkDbConn, rm.GetString("db_Connect_Txt"));

      hlpProv.SetShowHelp(txtConnString, true);
      hlpProv.SetHelpString(txtConnString, rm.GetString("db_ConnString_Txt"));

      hlpProv.SetShowHelp(chkInfer, true);
      hlpProv.SetHelpString(chkInfer, rm.GetString("meth_Infer_Txt"));

      hlpProv.SetShowHelp(chkSource, true);
      hlpProv.SetHelpString(chkSource, rm.GetString("src_Upload_Txt"));

      hlpProv.SetShowHelp(txtSourceExt, true);
      hlpProv.SetHelpString(txtSourceExt, rm.GetString("src_Ext_Txt"));

      hlpProv.SetShowHelp(txtSourcePath, true);
      hlpProv.SetHelpString(txtSourcePath, rm.GetString("src_Path_txt"));

    }
    
    /// <summary>
    /// Sets event methods for controls
    /// </summary>
    void SetEventHooks() {
      this.Load += new EventHandler(PropertiesForm_Load);
      this.FormClosing += new FormClosingEventHandler(PropertiesForm_FormClosing);
      txtProjFile.TextChanged += new EventHandler(SetDirty);
      txtAsmName.TextChanged += new EventHandler(SetDirty);
      txtConnString.TextChanged += new EventHandler(SetDirty);
      txtSourceExt.TextChanged += new EventHandler(SetDirty);
      txtSourcePath.TextChanged += new EventHandler(SetDirty);

      chkUnchecked.CheckedChanged += new EventHandler(SetDirty);
      chkInfer.CheckedChanged += new EventHandler(SetDirty);
      chkDropTable.CheckedChanged += new EventHandler(chkDropTable_CheckedChanged);
      chkAlterAsm.CheckedChanged += new EventHandler(chkAlterAsm_CheckedChanged);
      chkDbConn.CheckedChanged += new EventHandler(chkDbConn_CheckedChanged);
      chkSource.CheckedChanged += new EventHandler(chkSource_CheckedChanged);
      chkDbgSymbols.CheckedChanged +=new EventHandler(chkDbgSymbols_CheckedChanged);
      chkDepAttr.CheckedChanged +=new EventHandler(SetDirty);
      cboUDTCast.SelectedIndexChanged += new EventHandler(SetDirty);

      cboPermSet.SelectedIndexChanged += new EventHandler(SetDirty);
      cboVersion.SelectedIndexChanged += new EventHandler(SetDirty);

      

      btnBrowseSql.Click += new EventHandler(btnBrowseSql_Click);
      btnBrowseProj.Click += new EventHandler(btnBrowseProj_Click);
      btnDbBrowse.Click += new EventHandler(btnDbBrowse_Click);
      btnBrowseType.Click += new EventHandler(btnBrowseType_Click);

      chkSource.CheckedChanged += new EventHandler(chkSource_CheckedChanged);

     

    
    }

    /// <summary>
    /// Makes sure that the data is valid.
    /// </summary>
    /// <returns>boolean</returns>
    bool ValidateData() {
      bool validData = true;
      StringBuilder errBld = new StringBuilder();
      errBld.Append("Following data is missing or invalid:\n");
      if (txtAsmName.Text == string.Empty) {
        validData = false;
        errBld.Append("  *Assembly Name - A name of the assembly to create must be supplied.\n");
      }

      if (chkDbConn.Checked && txtConnString.Text == string.Empty) {
        validData = false;
        errBld.Append("  *Connection String - A connection string must be supplied.\n");
      }

      if (chkSource.Checked && txtSourceExt.Text == string.Empty) {
        validData = false;
        errBld.Append("  *Source File Extension - An extension for source files must be supplied.\n");
      }

      if (chkSource.Checked && txtSourcePath.Text == string.Empty) {
        validData = false;
        errBld.Append("  *Source File Path - A path to the source files must be supplied.\n");
      }

      if (!validData)
        MessageBox.Show(errBld.ToString(), "Missing or Invalid Data");

      return validData;
    }

    /// <summary>
    /// Writes the changed data to the sql.proj file.
    /// </summary>
    /// <returns>boolean</returns>
    bool UpdateData() {
      DataSet dsUpdate = null;
      bool res = false;
      if (ValidateData()) {
        doc = null;
        dsSqlProj.AcceptChanges();
        dsUpdate = dsSqlProj.Copy();
        try {
          doc = new XmlDataDocument(dsUpdate);
          doc.Save(sqlProjFile);
          isDirty = false;
          res = true;
        }
        catch (Exception e) {
          MessageBox.Show("An unexpected error happened. The error is: " + e.Message + ". The application is shutting down.", "Unexpected Error");
          res = false;
          return false;
        }
      }
      return res;
    }

    /// <summary>
    /// Allows the user to select the various project files
    /// </summary>
    /// <param name="fte">FileTypeEnum defining whether we're looking for the *.proj file (0), the VS project file, the Xml type conversion file or the compiled assembly.</param>
    /// <param name="projFile">The path to the file</param>
    /// <returns>boolean</returns>
    private bool SelectFile(FileTypeEnum fte, out string projFile) {
      projFile = "";
      string combineFile = "sql.proj";
      string filePath = appPath;
      string fileFilter = "sql deployment proj files (*.proj)|*.proj|All files (*.*)|*.*";
      if (fte == FileTypeEnum.Sql) {
        if (dpType == DeployTypeEnum.Assembly && vsProjPath != "")
          filePath = vsProjPath;

      }

      else if (fte == FileTypeEnum.Project) {
        fileFilter = "C# Project files (*.csproj)|*.csproj|VB Project files (*.vbproj)|*.vbproj |All files (*.*)|*.*";
        if (dpType == DeployTypeEnum.Assembly)
          fileFilter = "Dll file (*.dll)|*.dll|C# Project files (*.csproj)|*.csproj|VB Project files (*.vbproj)|*.vbproj |All files (*.*)|*.*";
        filePath = vsProjPath;
      }
      else if (fte == FileTypeEnum.Assembly) {
        fileFilter = "Dll file (*.dll)|*.dll|All files (*.*)|*.*";
        filePath = vsProjPath;
      }

      else if (fte == FileTypeEnum.Xml) {
        fileFilter = "Type conversion file (*.xml)|*.xml|All files (*.*)|*.*";
        filePath = appPath;
      }
      
      fd = new OpenFileDialog();
      fd.InitialDirectory = filePath;
      fd.Filter = fileFilter;
      fd.FilterIndex = 1;
      fd.RestoreDirectory = false;

      if (fd.ShowDialog() == DialogResult.OK) {
        projFile = fd.FileName;
        if (Path.GetDirectoryName(projFile) != vsProjPath) {
          if (fte  == FileTypeEnum.Xml)
            combineFile = "typeconversion.xml";
          if(fte == FileTypeEnum.Sql || fte == FileTypeEnum.Xml) {
            File.Copy(projFile, Path.Combine(vsProjPath, combineFile));
            projFile = Path.Combine(vsProjPath, combineFile);
          }
        }
        return true;
      }
      else
        return false;
    }

    void DoDeploy(string target) {
      this.Cursor = Cursors.WaitCursor;
      vForm = new viewlog();
      bool res = true;

      res = UpdateData();

      if(res) {
      
      try {

        string buildPath = System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory();
        buildPath = Path.Combine(buildPath, "MSBUILD.exe");

        System.Diagnostics.Process p = new System.Diagnostics.Process();
        p.StartInfo = new ProcessStartInfo(buildPath, "\"" + sqlProjFile + "\" /t:" + target);
        p.StartInfo.UseShellExecute = false;
        p.StartInfo.CreateNoWindow = true;
        p.StartInfo.RedirectStandardOutput = true;
        p.OutputDataReceived += new DataReceivedEventHandler(p_OutputDataReceived);
        p.Start();
        p.BeginOutputReadLine();
        p.WaitForExit();
        string exitMsg = "succeeded";
        if (p.ExitCode != 0)
          exitMsg = "failed";

        this.Cursor = Cursors.Default;
        string viewMsg = string.Format("The deployment/drop {0}. Do you want to view the log file?", exitMsg);

        if (MessageBox.Show(viewMsg, "View Logfile", MessageBoxButtons.YesNo) == DialogResult.Yes) {
          vForm.ShowDialog();
        }
      }
      catch (Exception ex) {
        MessageBox.Show("An unexpected error happened. The error is: " + ex.Message + ".", "Unexpected Error");

      }

      finally {
        vForm.Dispose();
      }
      }
    }

    #region event-methods

    void p_OutputDataReceived(object sender, DataReceivedEventArgs e) {
      // Collect the sort command output.
      
      if (!String.IsNullOrEmpty(e.Data)) {
        vForm.AppendText(e.Data + Environment.NewLine);
       
      }
    
    }

    
    void chkAlterAsm_CheckedChanged(object sender, EventArgs e) {
      chkUnchecked.Enabled = chkAlterAsm.Checked;
      isDirty = true;
    }

    void chkDbConn_CheckedChanged(object sender, EventArgs e) {
      pnlConnString.Enabled = chkDbConn.Checked;
      isDirty = true;
    }

    void chkDbgSymbols_CheckedChanged(object sender, EventArgs e)
    {
      pnlDbg.Enabled = chkDbgSymbols.Checked;
      isDirty = true;

    }

    void btnOK_Click(object sender, EventArgs e) {
      if(UpdateData()) {
        this.Close();
        
      }
    }

    void btnApply_Click(object sender, EventArgs e) {
      UpdateData();
    }

    void btnCancel_Click(object sender, EventArgs e) {
      this.Close();
      
    }

    void btnBrowseSql_Click(object sender, EventArgs e) {
      string sqlFile;
      if (SelectFile(FileTypeEnum.Sql, out sqlFile)) {
        sqlProjFile = sqlFile;
        txtSqlProjFile.Text = sqlFile;
        if (firstFind && dpType == DeployTypeEnum.Assembly) {
          vsProjPath = Path.GetDirectoryName(sqlProjFile);
          LoadDocsForAsm(FileTypeEnum.Sql);
        }
        
      }
     
    }

    void btnBrowseProj_Click(object sender, EventArgs e) {
      string sqlFile;
      if (SelectFile(FileTypeEnum.Project, out sqlFile)) {
        vsProjectFile = sqlFile;
        txtProjFile.Text = vsProjectFile;
        vsProjPath = Path.GetDirectoryName(vsProjectFile);
        if (firstFind && dpType == DeployTypeEnum.Assembly) {
          LoadDocsForAsm(FileTypeEnum.Project);
        }
      }
    }

    

    void btnBrowseType_Click(object sender, EventArgs e) {
      string sqlFile;
      if (SelectFile(FileTypeEnum.Xml, out sqlFile)) {
        txtTypeFile.Text = sqlFile;
        //LoadDocs();

      }
    }

    void btnDbBrowse_Click(object sender, EventArgs e) {
      finddb f = new finddb();
      if (f.ShowDialog() == DialogResult.OK)
        txtConnString.Text = f.cnString;
      
      f.Dispose();
    
    }


    private void btnBrowseSource_Click(object sender, EventArgs e)
    {
      FolderBrowserDialog fd = new FolderBrowserDialog();
      fd.ShowNewFolderButton = false;
      fd.Description = "Choose the directory where your source files resides.";
      if (fd.ShowDialog() == DialogResult.OK)
        txtSourcePath.Text = fd.SelectedPath.ToString();
        
 
    }
    

    void chkSource_CheckedChanged(object sender, EventArgs e) {
      grpSource.Enabled = chkSource.Checked;
      string ext = "";
      if (chkSource.Checked) {
        ext = Path.GetExtension(txtProjFile.Text);
        if (ext.Contains("cs"))
          txtSourceExt.Text = "cs";
        else if (ext.Contains("vb"))
          txtSourceExt.Text = "cs";
        else
          txtSourceExt.Text = "cs";

          

      }
      
    }

    void chkDropTable_CheckedChanged(object sender, EventArgs e)
    {
      pnlUDTCast.Enabled = !chkDropTable.Checked;
      isDirty = true;
    }

    void txtConnString_TextChanged(object sender, EventArgs e) {
      dsSqlProj.Tables["PropertyGroup"].Rows[0]["Connectionstring"] = txtConnString.Text;
      isDirty = true;  
    }

    void SetDirty(object sender, EventArgs e) {
      isDirty = true;
    }

    void PropertiesForm_Load(object sender, EventArgs e) {
      isDirty = false;
    }

    void PropertiesForm_FormClosing(object sender, FormClosingEventArgs e) {
      DialogResult res = DialogResult.No;
      if (isDirty) {
        res = MessageBox.Show("Do you want to save your work before exiting?", "Save Work", MessageBoxButtons.YesNoCancel);
        if (res == DialogResult.Yes) {
          //if the update doesn't succeed - give the user another chance
          if (!UpdateData()) {
            res = MessageBox.Show("There seems to be some problems with saving your data. Do you want to exit the application without saving the data?", "Exit Application", MessageBoxButtons.YesNo);
            if (res == DialogResult.No)
              e.Cancel = true;
          }//else just fall through
        }
        else if(res == DialogResult.Cancel)
          e.Cancel = true;
      }

      if (!e.Cancel)
        this.Dispose();
    
    }

    void mnuSubDropAsm_Click(object sender, EventArgs e) {
      DoDeploy("DropAssembly");
    }

    void mnuSubDeployMeth_Click(object sender, EventArgs e) {
      DoDeploy("DeployMeth");
    }

    void mnuSubDeployUDT_Click(object sender, EventArgs e) {
      DoDeploy("DeployUdt");
    }

    void mnuSubDeployAsm_Click(object sender, EventArgs e) {
      DoDeploy("DeployAsm");
    
    }

    void mnuSubDeployAll_Click(object sender, EventArgs e) {
      DoDeploy("DeployAll");
    }

    void mnuSubExit_Click(object sender, EventArgs e) {
      btnCancel_Click(this, EventArgs.Empty);
    }

    void mnuSubSave_Click(object sender, EventArgs e) {
      btnApply_Click(this, EventArgs.Empty);
    }


    #endregion

    

    
    #endregion
  }

  internal enum DeployTypeEnum {
    Project = 4,
    Assembly  = 8

  }

  internal enum FileTypeEnum {
    Sql = 0,
    Xml = 2,
    Project = 4,
    Assembly = 8
  }
}
