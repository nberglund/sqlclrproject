using System;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Text;
using System.Reflection;
using System.IO;
using System.Diagnostics;
using System.Collections;
using DM.Build.Yukon.Attributes.Service;
using Microsoft.SqlServer.Server;


namespace DM.Build.Yukon.Tasks {
  public class DeployTypes : Task {
    string asmName = ""; //name of the assembly to deploy
    string asmPath = ""; //path to the assembly
    string connString = "";
    string scriptFile = ""; //indicates the path and name to script the operations
    bool toScript = false; //indicates whether to create a script or not. If false toConnect has to be true.
    bool toConnect = false; //indicates whether to connect or just create a script. If false, toScripts has to be true/
    bool toDropTable = false; //indicates whether to drop the whole table or just columns
    internal int castType = 0; //type to cast the UDT to when creating a temp column, values:0=varchar(max);1=varbinary(max)
    bool alterAsm = false; //flag which indicates whether to run ALTER instead of CREATE ASSEMBLY
    string runFile = ""; //indicates the path and name to the file for test
    StreamWriter sw;
    StreamWriter swRun;
    SqlConnection conn = null;
    SqlTransaction tx = null;

    ArrayList alTypes = null;

    /// <summary>
    /// Indicates whether to create an Assembly or Alter an existing. [Optional]
    /// </summary>
    /// <value>boolean</value>
    public bool AlterAssembly {
      get { return alterAsm; }
      set { alterAsm = value; }
    }

    /// <summary>
    /// The name of the assembly without file-extension. Required.
    /// </summary>
    public string AssemblyName {
      get { return asmName; }
      set { asmName = value; }
    }

    /// <summary>
    /// The full path to the assembly.Required
    /// </summary>
    public string AssemblyPath {
      get { return asmPath; }
      set { asmPath = value; }
    }

    /// <summary>
    /// Connection string to the database. Optional if ScriptFilePath exists
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

    /// <summary>
    /// The full path to the script file. Optional if ConnectionString exists.
    /// [Optional]
    /// </summary>
    public string ScriptFilePath {
      get { return scriptFile; }
      set {
        if (value != string.Empty && value != null) {
          string fullFilePath = value;
          string rfPath = Path.GetDirectoryName(fullFilePath);
          runFile = Path.Combine(rfPath, "test_types.sql");
          scriptFile = Utility.AddToFileName(fullFilePath, "types");
          toScript = true;
        }
      }
    }

    /// <summary>
    /// Indicates whether to drop a whole table dependent on a UDT/UDF or just the dependent column(s). Default is to drop just the columns. [Optional]
    /// </summary>
    /// <value>boolean</value>
    public bool IsTableDrop {
      get { return toDropTable; }
      set { toDropTable = value; }
    }

    /// <summary>
    /// Integer which indicates what datatype to cast a UDT to when re-deploying
    /// </summary>
    /// <value>Integer with values: 0=varchar(max), 1=varbinary(max)</value>
    public int TypeToCastUDTTo
    {
      get { return castType; }
      set { castType = value; }
    }

    /// <summary>
    /// Boolean flag which indicates whether to connect to the database or not
    /// </summary>
    /// <value></value>
    public bool ToConnect {
      get { return toConnect; }
      set { toConnect = value; }
    }

    /// <summary>
    /// Validates the input. makes sure the necessary info exists.
    /// </summary>
    void Validate() {
      bool val = true;
      StringBuilder valError = new StringBuilder("Can not deploy due to following:\n");

      if (asmName == "") {
        val = false;
        valError.Append("* AssemblyName is missing.\n");
      }

      if (asmPath == "") {
        val = false;
        valError.Append("* AssemblyPath is missing.\n");
      }

      if (toConnect) {
        if (ConnectionString == null || ConnectionString == string.Empty) {
          val = false;
          valError.Append("* ConnectionString is missing.\n");
        }
      }
      else {
        if (alterAsm) {
          val = false;
          valError.Append("* You need to run connected in order to deploy types in ALTER mode.\n");
        }
      }


      if (toScript) {
        if (scriptFile == null || scriptFile == string.Empty) {
          val = false;
          valError.Append("* ScriptFilePath is missing.\n");
        }
      }

      if (!val) {
        throw new ApplicationException(valError.ToString());
      }
    }

    bool CheckTypeExists(string realName, string aliasName, ArrayList alMeths) {
      string checkName = realName + ", " + aliasName;
      return alMeths.Contains(checkName);
    }

    public override bool Execute() {
      SqlConnection conn2 = null;
      //Debugger.Launch();
      bool typeExist = false;
      string stringName = "";
      string sysName = "";
      try {
        //check that we have the necessary info
        Validate();
        if (toConnect) {
          //open the connection
          conn = new SqlConnection(connString);
          conn.Open();

          //start the tx
          tx = conn.BeginTransaction();
        }

        //create the file
        if (toScript) {
          string deploymentString = "--Deployment script for UDT's in assembly: " + asmName;
          string testString = "--Testing statements script for UDT's in assembly: " + asmName;

          if (alterAsm) {
            deploymentString = "--Deployment script for new UDT's in altered assembly: " + asmName;
            testString = "--Testing statements script for new UDT's in altered assembly: " + asmName;
          }
          sw = Utility.OpenFile(scriptFile, !alterAsm);
          swRun = Utility.OpenFile(runFile, !alterAsm);

          Utility.WriteToFile(sw, deploymentString, false, false);
          Utility.WriteToFile(sw, "--Autogenerated at: " + DateTime.Now, false, true);
          Utility.WriteToFile(swRun, testString, false, false);
          Utility.WriteToFile(swRun, "--Autogenerated at: " + DateTime.Now, false, true);

        }

        if (!alterAsm) {
          //drop types
          DropAssembly d = new DropAssembly(sw, conn, tx, asmName, toDropTable, castType, toScript, this, Assembly.LoadFile(asmPath));
          //drop types and dependent stuff
          d.DropTypes(toConnect);
        }
        else if (alterAsm) {
          conn2 = new SqlConnection(connString);
          conn2.Open();
          SqlCommand cmd = conn2.CreateCommand();
          cmd.CommandText = @"select t.assembly_class realname, 
                              t.name alias
                              from sys.assembly_types t with (nolock)
                              join sys.assemblies a with (nolock)
                              on t.assembly_id = a.assembly_id
                              where a.name = '" + asmName + "'";
          SqlDataReader dr = cmd.ExecuteReader();
          if (dr != null && dr.HasRows) {
            alTypes = new ArrayList();
            while (dr.Read())
              alTypes.Add(dr.GetString(0) + ", " + dr.GetString(1));

            dr.Close();
            conn2.Close();
          }

        }

        Type attr = typeof(SqlUserDefinedTypeAttribute);

        //get the assembly
        Assembly asm = Assembly.LoadFile(asmPath);

        if (toScript) {
          Utility.WriteToFile(sw, "--About to create UDT's", false, false);
        }

        foreach (Type t in asm.GetTypes()) {
          string typeName = t.Name;
          //check for the UDT attribute
          if (t.IsDefined(attr, false)) {
            
            object[] attrs = t.GetCustomAttributes(attr,false);
            SqlUserDefinedTypeAttribute udt = (SqlUserDefinedTypeAttribute)attrs[0];
            //string typeName = udt.Name;
            if(udt.Name !=string.Empty && udt.Name != null )
              typeName = udt.Name;

            sysName = Utility.GetSysName(typeName, out stringName);
            
            string fullName = t.FullName;
            if (alterAsm && CheckTypeExists(t.Name, stringName, alTypes))
              continue;
            
            typeExist = true;
            string createText = "CREATE TYPE " + sysName + "\nEXTERNAL NAME [" + asmName + "].[" + fullName + "]";

            if (toScript) {
              Utility.WriteToFile(sw, createText, true, true);
              if (createText.Contains("CREATE"))
                Utility.WriteToFile(swRun, Utility.GetExecString(createText), false, false);

            }


            if (toConnect) {
              Utility.LogMyComment(this, "About to execute:\n" + createText + "\n");
              Utility.WriteToDb(createText, conn, tx);
              Utility.LogMyComment(this, "Executed with success\n");
            }
      
            
          }
        }
        if (typeExist) {
          if (toConnect) {
            Utility.LogMyComment(this, "Comitting Transaction");
            tx.Commit();
          }
          Utility.LogMyComment(this, "Deployment Succeeded!\n");
          return true;
        }


      }

      catch (Exception e) {
        Utility.LogMyComment(this, "Error(s) Occured");
        Utility.LogMyComment(this, "Creating Type(s) Failed");
        Utility.LogMyErrorFromException(this, e);
        if (toConnect) {
          if (tx != null){
            Utility.LogMyComment(this, "Rolling Back Transaction\n");
            if(tx.Connection!=null)
              tx.Rollback();
          }
        }

        
        return false;
      }
      finally {
        if (toConnect) {
          if (tx != null) {
            if (tx.Connection != null)
              tx.Rollback();
          }
          if (conn != null && conn.State != ConnectionState.Closed)
            conn.Close();
        }
          
        if (toScript && sw != null) {
          sw.Flush();
          sw.Close();
        }

        if (toScript && swRun != null) {
          swRun.Flush();
          swRun.Close();
        }
      }
      return true;
    }
  }
}
