using System;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Reflection;
using System.IO;
using DM.Build.Yukon.Attributes.Service;
using System.Diagnostics;
using System.Collections;

namespace DM.Build.Yukon.Tasks {
  public class DeployMethods : Task {
    string asmName = ""; //name of the assembly to deploy
    string asmPath = ""; //path to the assembly
    bool inferMeth = false; //should we infer proc and UDF's from the assembly
    string typeConvPath = "";//path to the typeconversion file
    string connString = "";
    string scriptFile = ""; //indicates the path and name to script the operations
    bool toScript = false; //indicates whether to create a script or not. If false toConnect has to be true.
    bool toConnect = false; //indicates whether to connect or just create a script. If false, toScripts has to be true/
    bool toDropTable = false; //indicates whether to drop the whole table or just columns
    bool alterAsm = false; //flag which indicates whether to run ALTER instead of CREATE ASSEMBLY
    string runFile = ""; //indicates the path and name to the file for test
    StreamWriter sw;
    StreamWriter swRun;
    SqlConnection conn = null;
    SqlTransaction tx = null;
    ArrayList alMeths = null;

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
    /// When creating the syntax for deploying methods to Yukon we need to 
    /// convert from CLR/SQLTypes to T-SQL Types. This is a file which holds the 
    /// common conversions. It can be customized to converting even
    /// UDT's. Required.
    /// </summary>
    /// <value></value>
    public string TypeConversionFilePath {
      get { return typeConvPath; }
      set { typeConvPath = value; }
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
    /// This is a flag which decides whether to infer method signatures and names
    /// from the assembly. This allows us to not use the specific deployment attributes.
    /// However, we are limited to create procedures and UDF's. Optional.
    /// </summary>
    /// <value></value>
    public bool InferMethods {
      get { return inferMeth; }
      set { inferMeth = value; }
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

          scriptFile = Utility.AddToFileName(fullFilePath, "methods");
          string rfPath = Path.GetDirectoryName(fullFilePath);
          runFile = Path.Combine(rfPath, "test_methods.sql");
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

      if (typeConvPath == "") {
        val = false;
        valError.Append("* TypeConversionFilePath is missing.\n");
      }

      if (!toConnect && !toScript) {
        val = false;
        valError.Append("* ConnectionString and/or ScriptFilePath is missing.\n");
      }

      if (!toConnect && alterAsm) {
        val = false;
        valError.Append("* You need to run connected in order to deploy methods/aggregates in ALTER mode.\n");
      }

      if (inferMeth && alterAsm) {
        val = false;
        valError.Append("* Deploy methods in ALTER mode is not supported when inferring methods.\n");
      }

      if (!val) {
        throw new ApplicationException(valError.ToString());
      }
    }

    

    public override bool Execute() {
      SqlConnection conn2 = null;
      //Debugger.Launch();

      ArrayList al = null;

      try {
        //check that we have the necessary info
        Validate();

        //create the file
        if (toScript) {
          string deploymentString = "--Deployment script for methods in assembly: " + asmName;
          string testString = "--Testing statements script for methods in assembly: " + asmName;

          if (alterAsm) {
            deploymentString = "--Deployment script for new methods in altered assembly: " + asmName;
            testString = "--Testing statements script for new methods in altered assembly: " + asmName;
          }
          sw = Utility.OpenFile(scriptFile, !alterAsm);
          swRun = Utility.OpenFile(runFile, !alterAsm);
          Utility.WriteToFile(sw, deploymentString, false, false);
          Utility.WriteToFile(sw, "--Autogenerated at: " + DateTime.Now, false, true);
          Utility.WriteToFile(swRun, testString, false, false);
          Utility.WriteToFile(swRun, "--Autogenerated at: " + DateTime.Now, false, true);
        }

        if (toConnect) {
          //open the connection
          conn = new SqlConnection(connString);
          conn.Open();
          //start the tx
          tx = conn.BeginTransaction();
          if (!alterAsm) {
            //drop any relevant tables/columns, procs, views etc.
            DropAssembly d = new DropAssembly(sw, conn, tx, asmName, toDropTable, toScript, this);
            d.DropDependents();
          }
          else {
            conn2 = new SqlConnection(connString);
            conn2.Open();
            SqlCommand cmd = conn2.CreateCommand();
            cmd.CommandText = @"Select isnull(sam.assembly_method, sam.assembly_class) clrname,
                                so.name alias 
                                from sys.objects so with (nolock) 
                                join sys.assembly_modules sam with (nolock) 
                                on so.object_id = sam.object_id 
                                join sys.assemblies sa with (nolock) 
                                on sam.assembly_id = sa.assembly_id 
                                where sa.name = '" + asmName +"'";

            SqlDataReader dr = cmd.ExecuteReader();
            //if we have data load it into the arraylist
            if (dr != null && dr.HasRows) {
              alMeths = new ArrayList();
              while(dr.Read())
                alMeths.Add(dr.GetString(0) + ", " + dr.GetString(1));

              dr.Close();
              conn2.Close();

            }
           

          }
        }

        //instantiate the Utility class
        Utility u = new Utility(TypeConversionFilePath, alterAsm);
        if(alterAsm && alMeths != null)
          al = u.GetCreateString(asmPath, asmName, inferMeth, alMeths);
        else
          al = u.GetCreateString(asmPath, asmName, inferMeth);

        if(al.Count > 0 && toScript)
          Utility.WriteToFile(sw, "--About to drop and recreate CLR Methods and Aggregates", false, false);

        for (int i = 0; i < al.Count; i++) {
          string cmd = (string)al[i];

          Utility.LogMyComment(this, "About to execute:\n" + cmd + "\n");

          if (toScript) {
            Utility.WriteToFile(sw, cmd, true, true);
            if(cmd.Contains("CREATE"))
              Utility.WriteToFile(swRun, Utility.GetExecString(cmd), false, false);
          
          }

          if (toConnect)
            Utility.WriteToDb(cmd, conn, tx);
          
          Utility.LogMyComment(this, "Executed with success\n");

          
        }

        if (toConnect) {
          Utility.LogMyComment(this, "Comitting Transaction");
          tx.Commit();
        }

        Utility.LogMyComment(this, "Deployment Succeeded!\n");
        return true;
      }

      
      catch (Exception e) {
        Utility.LogMyComment(this, "Error(s) Occured");
        Utility.LogMyComment(this, "Creating Method(s)/Aggregates Failed");
        Utility.LogMyErrorFromException(this, e);
        if (toConnect) {
          if (tx != null) {
            Utility.LogMyComment(this, "Rolling Back Transaction\n");
            if (tx.Connection != null)
              tx.Rollback();
          }
        }


        return false;
      }
    
      finally {
        if (toConnect) {
          if (conn != null && conn.State != ConnectionState.Closed)
            conn.Close();
          if (conn2 != null && conn2.State != ConnectionState.Closed)
            conn2.Close();

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
    }

           
  }



}
