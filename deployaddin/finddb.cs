using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Sql;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Microsoft.Win32;
using System.Data.SqlClient;
using System.Resources;


namespace DeployAddIn {
  public partial class finddb : Form {
    #region fields
    internal string cnString = ""; //the created connection-string
    ArrayList servers = new ArrayList(); //holds servers
    bool servChanged = true; //flag to indicate if we need to re-connect
    #endregion

    #region constructors
    public finddb() {
      InitializeComponent();
      SetEventHooks();
      servers.Add("");
      LoadServers(ServerLocation.Local);
      grpConnect.Enabled = false;
      rdWinAuth.Checked = true;
      pnlOK.Enabled = false;
    }
    #endregion

    #region methods
    /// <summary>
    /// Sets up the event methods for the various controls
    /// </summary>
    void SetEventHooks() {
      btnRefresh.Click += new EventHandler(btnRefresh_Click);
      cboServers.SelectedIndexChanged += new EventHandler(cboServers_SelectedIndexChanged);
      cboServers.KeyUp += new KeyEventHandler(cboServers_KeyUp);
      rdWinAuth.CheckedChanged += new EventHandler(rdWinAuth_CheckedChanged);
      cboDbs.DropDown += new EventHandler(cboDbs_DropDown);
      cboServers.TextChanged += new EventHandler(cboServers_TextChanged);
      cboDbs.KeyUp += new KeyEventHandler(cboDbs_KeyUp);
      cboDbs.TextChanged += new EventHandler(cboDbs_TextChanged);
      btnOK.Click += new EventHandler(btnOK_Click);
      btnCancel.Click += new EventHandler(btnCancel_Click);
      btnTest.Click += new EventHandler(btnTest_Click);
      btnNew.Click += new EventHandler(btnNew_Click);
    
    }

    

    
    
    /// <summary>
    /// Before creating the connection-string, makes sure the data is correct
    /// </summary>
    /// <returns>boolean</returns>
    bool ValidateData() {
      bool success = true;
      StringBuilder sb = new StringBuilder();
      sb.Append("Following data is invalid or missing:\n");
      if (cboServers.Text.Length < 1) {
        sb.Append("  *Server name - Need to enter a server name.\n");
        success = false;
      }
      if (rdSqlAuth.Checked && txtUserName.Text.Length < 1) {
        sb.Append("  *User Name - User name needed.\n");
        success = false;
      }

      if (cboDbs.Text.Length < 1) {
        sb.Append("  *Connect to database - Database name needed.\n");
        success = false;
      }

      if (!success)
        MessageBox.Show(sb.ToString(), "Missing or Invalid Data");

      return success;
    }

    /// <summary>
    /// Creates the connection string
    /// </summary>
    /// <returns>bool</returns>
    bool SetConnString() {
      if (ValidateData()) {
        if (rdWinAuth.Checked)
          cnString = string.Format("server={0}; database={1}; Integrated Security='SSPI';", cboServers.Text, cboDbs.Text);
        else
          cnString = string.Format("server={0}; database={3}; uid={1}; pwd={2};", cboServers.Text, txtUserName.Text, txtPassword.Text, cboDbs.Text);
        return true;
      }
      return false;
    }

    /// <summary>
    /// Loads servers from either the local machine or remotely
    /// </summary>
    /// <param name="loc">ServerLocation enum, indicates to load local or remote</param>
    void LoadServers(ServerLocation loc) {
      Cursor crs = Cursor.Current;
      Cursor.Current = Cursors.WaitCursor;
      cboServers.DataSource = null;
      if (loc == ServerLocation.Local)
        GetLocalServers(ref servers);
      else
        GetNetWorkedServers(ref servers);

      cboServers.DataSource = servers;
      Cursor.Current = crs;
    }

    /// <summary>
    /// The method loads servers from the network
    /// </summary>
    /// <param name="al">ArrayList holding servers</param>
    void GetNetWorkedServers(ref ArrayList al) {
      string servName;
      string instName;
      string verNo;
      SqlDataSourceEnumerator se = SqlDataSourceEnumerator.Instance;
      DataTable dt = se.GetDataSources();
      foreach (DataRow row in dt.Rows) {
        //get major version
        verNo = row["version"].ToString();
        if (CheckVersion(verNo)) {
          instName = row["InstanceName"].ToString();
          servName = row["ServerName"].ToString();
          if (instName != string.Empty)
            servName = servName + "\\" + instName;

          if (!al.Contains(servName))
            al.Add(servName);

        }
      }
    }

    /// <summary>
    /// Reads the registry and finds locally installed servers
    /// </summary>
    /// <param name="al">ArrayList holding servers</param>
    void GetLocalServers(ref ArrayList al) {
      string machine = System.Environment.MachineName;
      
      string sqlServer = "";
      string regRoot = @"SOFTWARE\Microsoft\Microsoft SQL Server";
      
      String[] instances = null;
      RegistryKey rk = null;
      rk = Registry.LocalMachine.OpenSubKey(regRoot);
      if (rk != null) {
        al.Add(machine);
        rk.Close();
      }
        //string mch = machine;
        //RegistryKey rkSql = rk.OpenSubKey("Instance Names\\SQL");
        //foreach (string inst in (string[])rkSql.GetValueNames()) {
        //  string mssqlval = (string)rkSql.GetValue(inst);
        //  if (mssqlval != string.Empty) {
        //    if (CheckRegForVersion(rk, mssqlval)) {
        //      if (inst == "MSSQLSERVER")
        //        sqlServer = machine;
        //      else
        //        sqlServer = machine + "\\" + inst;

        //      if (!al.Contains(sqlServer))
        //        al.Add(sqlServer);
        //    }
        //  }
        //}
        //rkSql.Close();


        //  instances = (String[])rk.GetValue("InstalledInstances");
        //  if (instances != null && instances.Length > 0) {

        //    //having gone through the entries in instance names
        //    //loop through the instances (to be on the safe side)
        //    foreach (String element in instances) {
        //      //"MSSQLSERVER" should have been picked up above
        //      if (element != "MSSQLSERVER") {
        //        sqlServer = machine + "\\" + element;
        //        if (!al.Contains(sqlServer)) {
        //          if (CheckRegForVersion(rk, element))
        //            al.Add(sqlServer);
        //        }
        //      }
        //    }
        //  }
        //}
      //}
    }

    /// <summary>
    /// Helper method to return a version number
    /// </summary>
    /// <param name="rk">Registry key to look up</param>
    /// <param name="element"></param>
    /// <returns></returns>
    bool CheckRegForVersion(RegistryKey rk, string element) {
      RegistryKey rkMSSql = rk.OpenSubKey(element + "\\MSSQLServer\\CurrentVersion");
      string verNo = (string)rkMSSql.GetValue("CurrentVersion");
      return CheckVersion(verNo);
    }

    /// <summary>
    /// Helper method to validate agains a version number
    /// </summary>
    /// <param name="verNo">The version number to validate</param>
    /// <returns>boolean</returns>
    bool CheckVersion(string verNo) {
      bool ret = false;
      int firstDot = verNo.IndexOf('.', 0);
      if (firstDot != -1) {
        verNo = verNo.Substring(0, firstDot);
        if (int.Parse(verNo) > 8)
          ret = true;
      }
      return ret;
    }

    /// <summary>
    /// Connects to a server and loads the databases from that server.
    /// </summary>
    void LoadDataBases() {
      string uid;
      string pwd;
      SqlConnection conn = null;
      cboDbs.Items.Clear();
      try {
        //male sure we have the right connection info
        if (CheckUserName(out uid, out pwd)) {
          conn = DoConnect();
          if (conn != null) {
            SqlCommand cmd = new SqlCommand("select name from sys.databases order by name", conn);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
              cboDbs.Items.Add(dr["name"]);

            servChanged = false;
          }
          else {
            MessageBox.Show("Could not connect to the server/database. Please check:\n  1. That the server is valid and started.\n  2. You have the rights to connect to the server,", "Connection Failed");
          }
        }
      }
      finally {
        if (conn != null) {
          conn.Dispose();
        }
        

      }

    }

    /// <summary>
    /// Method to check that necessary user info exists.
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="pwd"></param>
    /// <returns>boolean</returns>
    bool CheckUserName(out string uid, out string pwd) {
      uid = "";
      pwd = "";
      bool userOK = false;
      if (rdSqlAuth.Checked) {
        if (txtUserName.Text == string.Empty) {
          MessageBox.Show("User name needs to be supplied.", "Missing Username");
        }
        else {
          uid = txtUserName.Text;
          pwd = txtPassword.Text;
          userOK = true;
        }
      }
      else
        userOK = true;

      return userOK;

    }

    /// <summary>
    /// Tries to connect to a server/database
    /// </summary>
    /// <returns>A SqlConnection or null</returns>
    private SqlConnection DoConnect() {
      SqlConnection conn = null;
      string svrName = cboServers.Text;
      string connString = "";
      string uid = "";
      string pwd = "";
      bool intSec = true;
      bool cont = true; ;
      if (rdSqlAuth.Checked) {
        intSec = false;
        if (!CheckUserName(out uid, out pwd))
          cont = false;
      }
      if (intSec && cont)
        connString = string.Format("server={0};Integrated Security='SSPI';", svrName);
      else if (!intSec && cont)
        connString = string.Format("server={0};uid={1};pwd={2}", svrName, uid, pwd);

      if (cont) {
        try {
          conn = new SqlConnection(connString);
          conn.Open();
          return conn;
        }
        catch {
          return null;
        }
      }
      return null;
    }

    #region event-methods
    void btnRefresh_Click(object sender, EventArgs e) {
      LoadServers(ServerLocation.Networked);
    
    }

    void cboServers_SelectedIndexChanged(object sender, EventArgs e) {
      grpConnect.Enabled = cboServers.Text.Length > 0;  
    }

    void rdWinAuth_CheckedChanged(object sender, EventArgs e) {
      pnlLogin.Enabled = !rdWinAuth.Checked;  
    
    }

    void cboServers_KeyUp(object sender, KeyEventArgs e) {
      grpConnect.Enabled = cboServers.Text.Length > 0;

    }

    void btnTest_Click(object sender, EventArgs e) {
      //MessageBox.Show("The method or operation is not implemented.", "Not Implemented");
      string uid;
      string pwd;
      Cursor crs = Cursor.Current;
      if(CheckUserName(out uid, out pwd)) {
        Cursor.Current = Cursors.WaitCursor;
        SqlConnection conn = DoConnect();
        if (conn != null) {
          MessageBox.Show("Connection succeeded!", "Connection Test");
          conn.Close();
          conn.Dispose();
        }
        else
          MessageBox.Show("Connection test failed! Please check the following:\n  1. The server name is valid and the server is started.\n  2. The database you try to connect to exists.\n  3. You have the right credentials to connect to the server/database.", "Connection Test");
      }
      Cursor.Current = crs;
    }

    void btnCancel_Click(object sender, EventArgs e) {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

    void btnOK_Click(object sender, EventArgs e) {
      if (SetConnString()) {
        this.DialogResult = DialogResult.OK;
        this.Close();

      }
      
    }

    void btnNew_Click(object sender, EventArgs e) {
      string uid = "";
      string pwd = "";
      string connString = "";
      if (CheckUserName(out uid, out pwd)) {
        if (rdWinAuth.Checked)
          connString = string.Format("server={0};Integrated Security='SSPI';", cboServers.Text);
        else
          connString = string.Format("server={0};uid={1};pwd={2}", cboServers.Text, uid, pwd);

        NewDb nd = new NewDb(cboServers.Text, connString);
        nd.ShowDialog();
        if (nd.DialogResult == DialogResult.OK) {
          cboDbs.Items.Add(nd.dbName);
          cboDbs.SelectedText = nd.dbName;
        }
        nd.Dispose();

        
      }

    
    }

    void cboDbs_TextChanged(object sender, EventArgs e) {
      pnlOK.Enabled = cboDbs.Text.Length > 0;
    }

    void cboDbs_KeyUp(object sender, KeyEventArgs e) {
      pnlOK.Enabled = cboDbs.Text.Length > 0;
    }

    void cboServers_TextChanged(object sender, EventArgs e) {
      servChanged = true;
      cboDbs.Items.Clear();
      cboDbs.Text = "";
      rdWinAuth.Checked = true;

    }

    void cboDbs_DropDown(object sender, EventArgs e) {
      Cursor crs = Cursor.Current;
      Cursor.Current = Cursors.WaitCursor;
      if (servChanged)
        LoadDataBases();

      Cursor.Current = crs;




    }

    

    

    #endregion

    #endregion
  }

  public enum ServerLocation {
    Local,
    Networked,
  }
}