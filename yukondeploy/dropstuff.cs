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
  public class DropAssembly : Task {
    StreamWriter sw;
    SqlTransaction tx;
    SqlConnection conn;
    string asmName;
    string asmPath;
    string scriptFile;
    bool toDropTable;
    internal int castType = 0; //type to cast the UDT to when creating a temp column, values:0=varchar(max);1=varbinary(max)
    bool toScript;
    Task t;
    string connString;
    bool toConnect;
    internal bool isScriptingTool = false; //flag which says if we're coming from the script-tool and  not SQLCLRProject
    Assembly asm;

    /// <summary>
    /// The name of the assembly without file-extension. Required.
    /// </summary>
    public string AssemblyName {
      get { return asmName; }
      set { asmName = value; }
    }

    /// <summary>
    /// The full path to the assembly.Required.
    /// </summary>
    public string AssemblyPath {
      get { return asmPath; }
      set {
        asmPath = value;
        asm = Assembly.LoadFile(asmPath);

      }
    }

    /// <summary>
    /// The name including full path to a script file that is 
    /// to be created containing the T-SQL syntax for dropping the assembly.
    /// The name of the file is a generic name and is changed to name_assembly.ext by the task. 
    /// Optional if ConnectionString exists.
    /// [Optional]
    /// </summary>
    public string ScriptFilePath {
      
      get { return scriptFile; }
      set {
        if (value != string.Empty && value != null) {
          
          string fullFilePath = value;
          //if the beginning of the name is deploy, change to drop
          string fileName = Path.GetFileNameWithoutExtension(fullFilePath);
          if (fileName.Contains("deploy")) {
            fileName = fileName.Replace("deploy", "drop");
            
          }
          else //if user defined - add drop to the beginning
            fileName = fileName.Insert(0, "drop_");

          fullFilePath = Path.Combine(Path.GetDirectoryName(fullFilePath), fileName + Path.GetExtension(fullFilePath));

          scriptFile = Utility.AddToFileName(fullFilePath, "assembly");
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


    public DropAssembly() { }

    public DropAssembly(StreamWriter _sw, SqlConnection _conn, SqlTransaction _tx, string _asmName, bool toDrop, bool _toScript, Task _t):this(_sw, _conn, _tx,_asmName,toDrop,0, _toScript,_t, null){ }

    public DropAssembly(StreamWriter _sw, SqlConnection _conn, SqlTransaction _tx, string _asmName, bool toDrop, int _castTo, bool _toScript, Task _t, Assembly a) {
      asmName = _asmName;
      conn = _conn;
      toDropTable = toDrop;
      castType = _castTo;
      if(conn!=null)
        connString = conn.ConnectionString;
      t = _t;
      tx = _tx;
      sw = _sw;
      toScript = _toScript;
      if(a!=null)
        asm = a;
    }


    /// <summary>
    /// Validates the input. makes sure the necessary info exists.
    /// </summary>
    void Validate() {
      bool val = true;
      StringBuilder valError = new StringBuilder("Can not drop due to following:\n");

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

    public override bool Execute() {
      //Debugger.Launch();
      try {
        //check that we have the necessary info
        Validate();
        t = this;
        if (toConnect) {
          //open the connection
          conn = new SqlConnection(connString);
          conn.Open();

          //start the tx
          tx = conn.BeginTransaction();
        }

        //create the file
        if (toScript) {
          sw = Utility.OpenFile(scriptFile, true);
          Utility.WriteToFile(sw, "--Drop script for assembly: " + asmName, false, false);
          Utility.WriteToFile(sw, "--Autogenerated at: " + DateTime.Now, false, true);
        }

        DropAsm(toConnect);
       
        if (toConnect) {
          Utility.LogMyComment(this, "Comitting Transaction");
          tx.Commit();
        }

        Utility.LogMyComment(this, "Drop Succeeded!");
        return true;
      }
      catch (Exception e) {
        Utility.LogMyComment(this, "Error(s) Occured");
        Utility.LogMyComment(this, "Drop Failed");
        Utility.LogMyErrorFromException(this, e);
        if (toConnect) {
          if (tx != null) {
            Utility.LogMyComment(this, "Rolling Back Transaction");
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
        }
        if (toScript && sw != null) {
          sw.Flush();
          sw.Close();
        }
      }
    }
    
    
    internal void DropAsm(bool toConn) {
      //Debugger.Launch();
      toConnect = toConn;
      SqlCommand cmd = null;
      
      try {
        if (toConnect) {
          cmd = conn.CreateCommand();

          //make sure we drop dependent methods/functions/triggers
          DropDependents();
        }

        DropTypes(toConnect);

        //drop the assembly
        string dropCmd = string.Format("if exists(select * from sys.assemblies where name = '{0}')\nDROP ASSEMBLY [{0}];", asmName);
        string logComment = "About to drop assembly: '" + asmName + "'";

        Utility.LogMyComment(t, logComment);
        

        if (toScript) {
          Utility.WriteToFile(sw, "--Dropping the assembly: " + asmName, false, false);
          Utility.WriteToFile(sw, dropCmd, true, true);
        }

        if (toConnect) 
          Utility.WriteToDb(dropCmd, conn, tx);
        
      }
      catch (Exception e) {
        throw e;
      }
    }

    internal void DropTypes(bool toConnect) {

      if (toScript && !toConnect)
        CreateDropTypeScript();

      if(toConnect) 
        DropTypesConnected();

      
    }

    private void CreateDropTypeScript() {
      string udtName = "";
      string sysName = "";
      string stringName = "";
      Type udtAttr = typeof(SqlUserDefinedTypeAttribute);
      
      Utility.WriteToFile(sw, "--About to drop UDT's", false, false);
      foreach (Type t in asm.GetTypes()) {
        if(t.IsDefined(udtAttr, false)) {
          SqlUserDefinedTypeAttribute udt = (SqlUserDefinedTypeAttribute)t.GetCustomAttributes(udtAttr, false)[0];
          udtName = t.Name;
          if (udt.Name != string.Empty && udt.Name != null)
            udtName = udt.Name;

          sysName = Utility.GetSysName(udtName, out stringName);
          string udtDrop = "if exists(select * from sys.assembly_types where name = '{0}')\nDROP TYPE {1};";

          udtDrop = string.Format(udtDrop, stringName, sysName);
          Utility.WriteToFile(sw, udtDrop, true, true);
        }
      }

    }

    private void DropTypesConnected() {
      //Debugger.Launch();
      string sysName = "";
      string stringName;
      try {
        //drop methods/functions etc based on the type
        string cmdText = "select distinct sh.name sch, sp.[Object_id], so.name, so.type from sys.parameters sp with (nolock) join sys.objects so with (nolock) on so.[object_id] = sp.[object_id] join sys.assembly_types st with (nolock) on sp.system_type_id = st.system_type_id join sys.assemblies sa with (nolock) on st.assembly_id = sa.assembly_id join sys.schemas sh on so.schema_id = sh.schema_id where sa.name = '" + asmName + "'";
        
        DropMethods(cmdText);

        //drop tables columns based on the type
        cmdText = "select object_name(sc.[object_id]) tablename, so.type, sc.name colname, sc.[object_id] objid from sys.assembly_types sat with (nolock) join sys.assemblies sa with (nolock) on sat.assembly_id = sa.assembly_id join sys.columns sc with (nolock) on sat.system_type_id = sc.system_type_id join sys.objects so with (nolock) on sc.[object_id] = so.[object_id] where sa.name = '" + asmName + "'";

        string scriptText = "--Dropping Table Columns, Views and Functions dependent on UDT's";

        if (toDropTable)
          scriptText = "--Dropping Tables, Views and Functions dependent on UDT's";

        DropDependentTablesColumnsMethods(cmdText, scriptText, true);

        //retrieve a datareader with the types belonging to the assembly
        cmdText = "select sch.name sch, sat.name from sys.assembly_types sat with (nolock) join sys.assemblies sa with (nolock) on sat.assembly_id = sa.assembly_id join sys.schemas sch with (nolock) on sch.schema_id = sat.schema_id where sa.name = '" + asmName + "'";

        SqlDataReader dr = Utility.ExecuteReader(cmdText, connString);
        string typName;

        if (dr.HasRows) {
          if (toScript)
            Utility.WriteToFile(sw, "--Dropping UDT's.", false, false);
        }

        while (dr.Read()) {
          typName = dr[1].ToString();
          sysName = dr[0].ToString() + "." + typName;
          sysName = Utility.GetSysName(sysName, out stringName);

          string typeDrop = string.Format("if exists(select * from sys.assembly_types where name = '{0}')\nDROP TYPE {1};", stringName, sysName);
          string logComment = "About to drop TYPE: '" + sysName + "'";

          Utility.LogMyComment(t, logComment);
          if (toScript)
            Utility.WriteToFile(sw, typeDrop, true, true);
          
          Utility.WriteToDb(typeDrop, conn, tx);
          
        }

        dr.Close();
      }
      catch (Exception e) {
        throw e;
      }
    }

    //this method drops tables/columns, T-SQL functions, procs, triggers
    //dependent on CLR stuff
    internal void DropDependentTablesColumnsMethods(string commandText, string scriptText, bool typeDrop) {
      //Debugger.Launch();
      try {
        
        //this gives a reader with all tables/tvf's and views
        SqlDataReader dr = Utility.ExecuteReader(commandText, connString);

        if (dr.HasRows) {
          if(toScript)
            Utility.WriteToFile(sw, scriptText, false, false);
        }

        string typeName = "";

        while (dr.Read()) {
          string execDrop = "if exists(select * from sys.objects where name = '{0}' and type = '{1}')\nDROP {2} [{3}];";
          string dropComment = "About to drop {0}: '{1}'";
          string tblName = dr[0].ToString();
          string objType = dr[1].ToString();
          string colName = dr[2].ToString();
                    
          switch (objType.Trim()) {
            case "V":
              typeName = "VIEW";
              break;

            case "IF":
              typeName = "FUNCTION";
              break;

            case "TF":
              typeName = "FUNCTION";
              break;

            case "FN":
              typeName = "FUNCTION";
              break;

            case "P":
              typeName = "PROCEDURE";
              break;

            case "TR":
              typeName = "TRIGGER";
              break;

            case "U":
              typeName = "TABLE";
              break;
          }
          execDrop = string.Format(execDrop, tblName, objType, typeName, tblName);
          dropComment = string.Format(dropComment, typeName, tblName);
          string newColName = "";
          string newColType = "";
          //here we've come across a table with a column dependency
          //and in the settings we've said not to drop the table
          //but change the column
          if (!toDropTable && objType.Trim() == "U") {
            newColName = colName + "_old";
            newColType = "varchar(max)";
            if(castType == 1)
              newColType = "varbinary(max)";
            
            //execDrop = "if exists(select * from sys.columns where name = '{0}' and object_id = object_id('{1}'))\nALTER TABLE [{1}]\nDROP COLUMN [{2}]";
            execDrop = "if exists(select * from sys.columns where name = '{0}' and object_id = object_id('{1}'))\nBEGIN\nALTER TABLE [{1}]\nADD [{2}] {3};\nEND";
            //execDrop = string.Format(execDrop, colName, tblName, colName);
            execDrop = string.Format(execDrop, colName, tblName, newColName, newColType);
            //dropComment = "About to drop column: '{0}', from table: '{1}";
            dropComment = "About to add column: '{0} {2}', to table: '{1}";
            dropComment = string.Format(dropComment, newColName, tblName, newColType);
          }
          
          if (typeDrop || (typeName == "TABLE"))
          {
            Utility.LogMyComment(t, dropComment);

            if (toScript)
              Utility.WriteToFile(sw, execDrop, true, true);

            Utility.WriteToDb(execDrop, conn, tx);
            if (!toDropTable)
            {
              execDrop = "if exists(select * from sys.columns where name = '{0}' and object_id = object_id('{1}'))\nBEGIN\nUPDATE [{1}]\n{4};\nALTER TABLE [{1}]\nDROP COLUMN [{3}];\nEND";
              string setString = "SET [{0}] =[{1}].ToString()";
              if(castType == 1 || !typeDrop)
                setString = "SET [{0}] =cast([{1}] as varbinary(max))";
              setString = string.Format(setString, newColName, colName);
              execDrop = string.Format(execDrop, colName, tblName, newColName, colName, setString);
              dropComment = "About to update table '{0}' by setting '{1} = {2}', followed by dropping column: '{2}'";
              dropComment = string.Format(dropComment, tblName, newColName, colName);
              Utility.LogMyComment(t, dropComment);
              if (toScript)
              {
                Utility.WriteToFile(sw, execDrop,true, true);
                string execDrop2 = "----After  having re-deployed the type and converted the data back from {0}\n----uncomment the following statement and execute to drop the temporary column.\n--ALTER TABLE [{1}]\n--DROP COLUMN [{0}]\n";
                execDrop2 = string.Format(execDrop2, newColName, tblName);
                Utility.WriteToFile(sw, execDrop2, true, true);
              }
              Utility.WriteToDb(execDrop, conn, tx);
            }
          }
          
        }

        dr.Close();
      }
      catch (Exception e) {
        throw e;
      }
    }

    internal void DropDependents() {
      //we need first to drop tables, functions, triggers etc that are based on the CLR methods
      string cmdText = "select object_name(sd.[object_id]) tablename, so.type, sc.name, sd.[object_id] objid from sys.sql_dependencies sd with (nolock) join sys.objects so with (nolock) on sd.[object_id] = so.[object_id] join sys.assembly_modules sm with (nolock) on sd.referenced_major_id = sm.object_id join sys.assemblies sa with (nolock)  on sa.assembly_id = sm.assembly_id left join sys.columns sc with (nolock) on sd.column_id = sc.column_id  and sc.object_id = sd.object_id where sa.name = '" + asmName + "'";
      string scriptText = "--Dropping Table-Columns, Procedures, Functions, etc., dependent on CLR Methods";
      if (toDropTable)
        scriptText = "--Dropping Tables, Procedures, Functions, etc., dependent on CLR Methods";

      DropDependentTablesColumnsMethods(cmdText, scriptText, false);

      //now we can drop the methods
      cmdText = "Select sh.name sch, so.name name, so.type from sys.objects so with (nolock) join sys.assembly_modules sam with (nolock) on so.object_id = sam.object_id join sys.assemblies sa with (nolock) on sam.assembly_id = sa.assembly_id join sys.schemas sh on so.schema_id = sh.schema_id where sa.name = '" + asmName + "'";
      DropMethods(cmdText);
    }

    private void DropMethods(string commandText) {
      SqlCommand cmd = null;
      string stringName;
      string schName = "";

      try {
        cmd = conn.CreateCommand();

        //assign the tx to cmd - I don't need it on cmd2 as it is only reading
        SqlDataReader dr = Utility.ExecuteReader(commandText, connString);

        if (dr.HasRows) {
          if (toScript)
            Utility.WriteToFile(sw, "--Dropping CLR Procedures, Functions, Triggers etc.", false, false);
        }

        while (dr.Read()) {
          string name = dr["name"].ToString();
          schName = dr["sch"].ToString() + "." + name;
          schName = Utility.GetSysName(schName, out stringName);
          string procType = dr["type"].ToString();

          switch (procType.Trim()) {
            case "FS":
              procType = "FUNCTION";
              break;

            case "FN":
              procType = "FUNCTION";
              break;

            case "PC":
              procType = "PROCEDURE";
              break;

            case "P":
              procType = "PROCEDURE";
              break;

            case "FT":
              procType = "FUNCTION";
              break;

            case "IF":
              procType = "FUNCTION";
              break;

            case "TA":
              procType = "TRIGGER";
              break;

            case "AF":
              procType = "AGGREGATE";
              break;
          }

          string execDrop = string.Format("if exists(select * from sys.objects where name = '{0}')\nDROP {2} {1};",name, schName, procType);
          string logComment = "About to drop " + procType + ": '" + schName + "'";

          Utility.LogMyComment(t, logComment);

          if (toScript)
            Utility.WriteToFile(sw, execDrop, true, true);

          Utility.WriteToDb(execDrop, conn, tx);
        }

        dr.Close();
      }
      catch (Exception e) {
        throw e;
      }
    }
  }
}
