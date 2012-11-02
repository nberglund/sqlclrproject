using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace DM.Build.Yukon.DeployProperties {
  public partial class NewDb : Form {
    #region fields
    string connString; //connection string
    internal string dbName;
    #endregion

    #region constructor
    public NewDb(string svrName, string connS) {
      connString = connS;
      InitializeComponent();
      SetEventHooks();
      btnCreate.Enabled = false;
      txtSvr.Text = svrName;
      txtSvr.Enabled = false;

    }
    #endregion

    #region methods
    /// <summary>
    /// Sets the event methods
    /// </summary>
    void SetEventHooks() {
      btnCreate.Click += new EventHandler(btnCreate_Click);
      btnCancel.Click += new EventHandler(btnCancel_Click);
      txtDbName.TextChanged += new EventHandler(txtDbName_TextChanged);

    }

    bool CreateDb() {
      SqlConnection conn = null;
      SqlCommand cmd = null;
      Cursor crs = Cursor.Current;
      Cursor.Current = Cursors.WaitCursor;
      try {
        conn = new SqlConnection(connString);
        cmd = conn.CreateCommand();
        cmd.CommandText = "create database " + txtDbName.Text;
        conn.Open();
        cmd.ExecuteNonQuery();
        dbName = txtDbName.Text; 
        MessageBox.Show("The creation of the databased succeeded.", "Database Created");
        return true;
      }

      catch {
        MessageBox.Show("The creation of the databased failed.", "Failure");
        return false;
      }
      finally {
        if(conn!=null)
          conn.Dispose();
        Cursor.Current = crs;
      }
     
    }

    #region event-methods

    void txtDbName_TextChanged(object sender, EventArgs e) {
      btnCreate.Enabled = txtDbName.TextLength > 0;  
    }

    void btnCancel_Click(object sender, EventArgs e) {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

    void btnCreate_Click(object sender, EventArgs e) {
      if (CreateDb()) {
        this.DialogResult = DialogResult.OK;
        this.Close();
      }
        
    }

    #endregion


    #endregion

  }
}