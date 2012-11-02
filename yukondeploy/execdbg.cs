using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using DM.Build.Yukon.Attributes.Service;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System.IO;
using System.Reflection;
using System.Data.Sql;
using System.Diagnostics;
using Microsoft.SqlServer.Server;
using System.Text;

namespace DM.Build.Yukon.Tasks {
  public class ExecDebug : Task {

    #region fields
    Task t;
    string connString;
    string cmdText = "";
    #endregion
    #region properties
    /// <summary>
    /// Connection string to the database.
    /// [Optional]
    /// </summary>
    public string ConnectionString {
      get { return connString; }
      set {
        if (value != null && value != string.Empty) {
          connString = value;
        }
      }
    }

    public string CmdText {
      get { return cmdText; }
      set {
        if (value != null && value != string.Empty) {
          cmdText = value;
        }
      }
    }
    

    #endregion

    #region methods

    /// <summary>
    /// Validates the input. makes sure the necessary info exists.
    /// </summary>
    void Validate() {
      bool val = true;
      StringBuilder valError = new StringBuilder("Can not execute statement due to following:\n");

      if (cmdText == "") {
        val = false;
        valError.Append("* No CommandText.\n");
      }
      
      if (!val) {
        throw new ApplicationException(valError.ToString());
      }
    }



    public override bool Execute() {
      SqlConnection conn = null;
      //Debugger.Launch();
      try {
        //check that we have the necessary info
        Validate();
        SqlDataReader dr = null;
        StringBuilder sb = new StringBuilder();
        int x = 0;
        t = this;
        conn = new SqlConnection(ConnectionString);
        conn.Open();
        SqlCommand cmd = new SqlCommand(CmdText);
        cmd.Connection = conn;
        Utility.LogMyComment(this, "About to execute statement: '" + cmdText + "'");

        if (CmdText.ToUpper().Contains("SELECT")) {
          dr = cmd.ExecuteReader();
        }
        else {
          x = cmd.ExecuteNonQuery();
        }
        Utility.LogMyComment(this, "Succeeded executing statement!");
        if (dr != null) {
          for (int i = 0; i < dr.FieldCount; i++) {
            string colName = dr.GetName(i);
            if (colName == string.Empty || colName == null)
              colName = "No Name";
            sb.Append(colName);
            if (i < dr.FieldCount - 1)
              sb.Append("\t");
            else if (i == dr.FieldCount - 1)
              sb.Append("\n");
          }
          string u = "";
          for (int i = 0; i < dr.FieldCount; i++) {
            string colName = dr.GetName(i);
            if(colName == string.Empty || colName == null)
              colName = "No Name";
            sb.Append(u.PadRight(colName.Length, '.'));
            if (i < dr.FieldCount - 1)
              sb.Append("\t");
            else if (i == dr.FieldCount - 1)
              sb.Append("\n");
          }
          while (dr.Read()) {
            for (int i = 0; i < dr.FieldCount; i++) {
              sb.Append(dr[i].ToString());
              if (i < dr.FieldCount - 1)
                sb.Append(",");
              
             }
             sb.Append("\n");
          }
          Utility.LogMyComment(this, sb.ToString());

        }
        else if(dr==null && !CmdText.ToUpper().Contains("SELECT"))
          Utility.LogMyComment(this, x.ToString());
        
       
        return true;
      }
      catch (Exception e) {
        Utility.LogMyComment(this, "Error(s) Occured");
        Utility.LogMyComment(this, "Could not execute statement.");
        Utility.LogMyErrorFromException(this, e);
        return false;

      }
      finally {
        if (conn != null && conn.State != ConnectionState.Closed)
            conn.Close();
        
      }
    }

    #endregion



  }
}
