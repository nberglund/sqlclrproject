using System;
using System.Reflection;
using System.Data.SqlTypes;
using System.Xml.XPath;
using System.Collections;
using System.Text;
using DM.Build.Yukon.Attributes;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System.IO;
using System.Diagnostics;
using Microsoft.SqlServer.Server;
using System.Collections.Generic;
using DM.Build.Yukon.Tasks;

//Select so.name alias, 
//  so.type, isnull(sam.assembly_method, sam.assembly_class) clrname
//from sys.objects so with (nolock) 
//  join sys.assembly_modules sam with (nolock) 
//  on so.object_id = sam.object_id 
//  join sys.assemblies sa with (nolock) 
//  on sam.assembly_id = sa.assembly_id 
//  where sa.name = 'testbld6';


namespace DM.Build.Yukon.Attributes.Service {
  public class Utility {
    XPathNavigator xpr = null;
    bool alterAsm = false;

    static string[] gacDlls2k5 = new string[] { "Microsoft.Visualbasic.dll", 
      "Mscorlib.dll", 
      "System.Data.dll", 
      "System.dll", 
      "System.Xml.dll", 
      "Microsoft.Visualc.dll", 
      "Custommarshallers.dll", 
      "System.Security.dll", 
      "System.Web.Services.dll", 
      "System.Data.SqlXml.dll", 
      "System.Transactions.dll", 
      "System.Data.OracleClient.dll",
      "System.Configuration.dll",
      "System.Deployment.dll"};
    static string[] gacDlls2k8 = new string[] { "Microsoft.Visualbasic.dll", 
      "Mscorlib.dll", 
      "System.Data.dll", 
      "System.dll", 
      "System.Xml.dll", 
      "Microsoft.Visualc.dll", 
      "Custommarshallers.dll", 
      "System.Security.dll", 
      "System.Web.Services.dll", 
      "System.Data.SqlXml.dll", 
      "System.Transactions.dll", 
      "System.Data.OracleClient.dll",
      "System.Core.dll",
      "System.Configuration.dll",
      "System.Deployment.dll",
      "System.Xml.Linq.dll"};


    public Utility(string resolvePath) :this(resolvePath, false) {}
      
    public Utility(string resolvePath, bool toAlter) {
      xpr = new XPathDocument(resolvePath).CreateNavigator();
      alterAsm = toAlter;
    }

    string ResolveTypeString(string paraMeterType, ICustomAttributeProvider ap, ref string prmName, ref object defVal) {
      bool res = false;
      int pos = paraMeterType.LastIndexOf(".");

      if (pos > 0)
        paraMeterType = paraMeterType.Substring(pos);

      //get the sql type
      XPathNodeIterator r = xpr.Select("/Types/Type[@ID='" + paraMeterType + "']");

      if (r.MoveNext()) {
        paraMeterType = r.Current.Value;

        //check for facet if ap is  ot null
        if (ap != null)
          res = GetFacet(ref paraMeterType, ap, ref prmName, ref defVal);

        if (!res) {
          //if the param is vachar or decimal we need to set size and/or precision/scale if
          //facet is not attributed
          if (paraMeterType == "nvarchar" || paraMeterType == "varbinary" || paraMeterType == "decimal") {
            paraMeterType = SetVarLengthAndPrecision(paraMeterType);
          }
        }
      }
      else {
        if (paraMeterType == "ISqlReader"||paraMeterType=="IEnumerable"||paraMeterType=="IEnumerator")
          paraMeterType = "TABLE";
      }

      return paraMeterType;
    }

    string ReflectParameters(MethodInfo m, out string retType) {
      //string builder to put the parameters into
      //Debugger.Launch();
      StringBuilder sb = new StringBuilder();
      string paraMeterType;
      string paraMetername = null;
      string sqlParam = "";
      string retVal = "";
      object defVal = null;
      bool firstRun = true;
      ICustomAttributeProvider ip = null;
      
      //loop through the parameters and get the names and types
      foreach (ParameterInfo p in m.GetParameters()) {
        bool IsRef = false;
        
        paraMeterType = GetParamType(p.ParameterType);

        paraMetername = p.Name;

            
        
        if (p.IsOut || p.ParameterType.IsByRef) {
          paraMeterType = paraMeterType.Substring(0, paraMeterType.LastIndexOf("&"));
          IsRef = true;
        }

        //convert the defined type to SQL type including length etc.
        sqlParam = ResolveTypeString(paraMeterType, p, ref paraMetername, ref defVal);
        paraMetername = "@" + paraMetername;

        if (IsRef)
          sqlParam = sqlParam + " out";

        if (!firstRun)
          sb.Append(", ");

        sb.Append(paraMetername);
        sb.Append(" ");
        sb.Append(sqlParam);
        if (defVal != null)
        {
          
          sb.Append(" = ");
          if (defVal.GetType() == typeof(string))
            sb.Append(string.Format("'{0}'", defVal.ToString()));
          else
            sb.Append(string.Format("{0}", defVal.ToString()));
          defVal = null;
        }
        firstRun = false;
        paraMetername = null;
      }

      //check the retval
      retVal = GetParamType(m.ReturnType);
      //retVal = m.ReturnType.Name;
      if (retVal != "Void") {
        ip = m.ReturnTypeCustomAttributes;
        retVal = ResolveTypeString(retVal, ip, ref paraMetername, ref defVal);
      }

      sqlParam = sb.ToString();
      retType = retVal;
      return sqlParam;
    }

    //method which checks if a parameter / return type is generic or not
    //and returns either the true type or the compile time type
    private static string GetParamType(Type p)
    {
        
      string paraMeterType;
      if (p.IsGenericType)
      {
        //as this is only used for params / return types
        //we only need to check the first element in the array
        paraMeterType = p.GetGenericArguments()[0].Name;
      }
      else
        paraMeterType = p.Name;

      return paraMeterType;
    }

    bool GetFacet(ref string sParam, ICustomAttributeProvider ap, ref string prmName, ref object defVal) {
      //Debugger.Launch();
      bool isFacet = false;
      bool SetFacet = false;
      Type attr = typeof(SqlFacetAttribute);
      Type attr2 = typeof(SqlParamFacetAttribute);
      SqlFacetAttribute fc = null;

      if(ap.IsDefined(attr2, false)) {
        SqlParamFacetAttribute prmfcarr = ((SqlParamFacetAttribute[])ap.GetCustomAttributes(attr2, false))[0];
        if (prmfcarr.Name != null)
        {
          prmName = prmfcarr.Name;
        }
        if (prmfcarr.DefaultValue != null)
          defVal = prmfcarr.DefaultValue;
        isFacet = true;
        fc = (SqlFacetAttribute)prmfcarr;
      }

      if (ap.IsDefined(attr, false) && !isFacet) {
        SqlFacetAttribute[] fcarr = (SqlFacetAttribute[])ap.GetCustomAttributes(attr, false);

        //I assume I can only have one sqlfacet per param
        fc = (SqlFacetAttribute)fcarr[0];
        isFacet = true;
      }

      if(isFacet) {

        switch (sParam) {
          case "nvarchar":
            if (fc.IsFixedLength) {
              sParam = "nchar";
              SetFacet = true;
            }

            if (fc.MaxSize > 0 && fc.MaxSize < 4001) {
              sParam = sParam + "(" + fc.MaxSize.ToString() + ")";
              SetFacet = true;
            }
            else if (fc.MaxSize == -1) {
              sParam = sParam + "(max)";
              SetFacet = true;
            }
            else {
              sParam = sParam + "(50)";
              SetFacet = true;
            }

            break;

          case "varbinary":
            if (fc.MaxSize==-1) {
              sParam = sParam + "(max)";
              SetFacet = true;
            }
            else if (fc.MaxSize > 0) {
              sParam = sParam + "(" + fc.MaxSize.ToString() + ")";
              SetFacet = true;
            }
            else {
              sParam = sParam + "(50)";
              SetFacet = true;
            }

            break;

          case "decimal":
            if (fc.Precision > 0 && fc.Scale > 0) {
              sParam = sParam + "(" + fc.Precision.ToString() + ", " + fc.Scale.ToString() + ")";
              SetFacet = true;
            }

            break;
        }
      }

      return SetFacet;
    }

    string SetVarLengthAndPrecision(string sParam) {
      switch (sParam) {
        case "nvarchar":
          sParam = sParam + "(50)";
          break;

        case "varbinary":
          sParam = sParam + "(4000)";
          break;

        case "decimal":
          sParam = sParam + "(18, 5)";
          break;
      }
      return sParam;
    }

    string GetAggregate(Type aggType, string aggName, string asmName) {
      //the aggregate has two methods which define how to create the agg
      //Accumulate and Terminate. Accumulate defines the in params
      //where Terminate defines the return type of the Agg
      MethodInfo aggTerminate = aggType.GetMethod("Terminate");
      MethodInfo aggAccumulate = aggType.GetMethod("Accumulate");
      string tempRetVal;
      //this gives us the parameters for the create statement
      string parameters = ReflectParameters(aggAccumulate, out tempRetVal);
      //this gives us the return value
      string tempParams = ReflectParameters(aggTerminate, out tempRetVal);
      string createString = "CREATE AGGREGATE {0}({1})\nRETURNS {2}\nEXTERNAL NAME [{3}].[{4}]";

      return string.Format(createString, aggName, parameters, tempRetVal, asmName, aggType.FullName);
      
    }

    public ArrayList GetCreateString(string assemblyPath, string AssemblyName, bool IsInfer) {

      return GetCreateString(assemblyPath, AssemblyName, IsInfer, null);
    }

    
    public ArrayList GetCreateString(string assemblyPath, string AssemblyName, bool IsInfer, ArrayList alMeths) {
      string parameters = "";
      string retType;
      string[] retString = null;
      string stringName;
      ArrayList al = new ArrayList();
      //Type attr = typeof(IDeployMethods);
      Assembly a = Assembly.LoadFile(assemblyPath);
      Type aggAttr = typeof(SqlUserDefinedAggregateAttribute);

      foreach (Type asmType in a.GetTypes()) {
        string aggName = "";
        //check for aggregates
        if (asmType.IsDefined(aggAttr, false)) {
          object[] attrs = asmType.GetCustomAttributes(aggAttr, false);
          SqlUserDefinedAggregateAttribute agg = (SqlUserDefinedAggregateAttribute)attrs[0];
          //string typeName = udt.Name;
          if (agg.Name != string.Empty && agg.Name != null)
            aggName = agg.Name;
          else
            aggName = asmType.Name;

          aggName = Utility.GetSysName(aggName, out stringName);

          if (alterAsm && CheckMethodExists(asmType.Name, aggName, alMeths))
            continue;
         

          string dropString = "if exists(select * from sys.objects where name = '{0}')\nDROP AGGREGATE {1}";
          dropString = string.Format(dropString, stringName, aggName);
          string createString = GetAggregate(asmType, aggName, AssemblyName);
          al.Add(dropString);
          al.Add(createString);
        }



        foreach (MethodInfo m in asmType.GetMethods(BindingFlags.Static | BindingFlags.Public)) {
          string realName = m.Name;
          string aliasName = m.Name;
          string sysName = "";
          bool addString = false;
          //get the parameters
          parameters = ReflectParameters(m, out retType);
          //get custom attributes for the method
          object[] attr = m.GetCustomAttributes(false);
          if (attr.Length > 0)
            for (int i = 0; i < attr.Length; i++) {
              addString = false;
              Type t = attr[i].GetType();
              if (t == typeof(SqlProcedureAttribute)) {
                SqlProcedureAttribute sql = (SqlProcedureAttribute)attr[i];
                if (sql.Name != string.Empty && sql.Name != null)
                  sysName = GetSysName(sql.Name, out aliasName);
                
                DMSqlProcedure id = new DMSqlProcedure(sql);
                retString = id.GetCreateString(false, m, AssemblyName, parameters);
                addString = true;
              }

              if (t == typeof(SqlFunctionAttribute)) {
                SqlFunctionAttribute sql = (SqlFunctionAttribute)attr[i];
                if (sql.Name != string.Empty && sql.Name != null)
                  sysName = GetSysName(sql.Name, out aliasName);
                DMSqlFunction id = new DMSqlFunction(sql);
                retString = id.GetCreateString(false, m, AssemblyName, parameters, retType);
                addString = true;
              }

              if (t == typeof(SqlTriggerAttribute)) {
                SqlTriggerAttribute sql = (SqlTriggerAttribute)attr[i];
                if (sql.Name != string.Empty && sql.Name != null)
                  sysName = GetSysName(sql.Name, out aliasName);
                DMSqlTrigger id = new DMSqlTrigger(sql);
                retString = id.GetCreateString(false, m, AssemblyName);
                addString = true;

              }
              if (alterAsm && CheckMethodExists(realName, aliasName, alMeths))
                break;

              if (addString) {
                foreach (String s in retString)
                  al.Add(s);
              }

            }
          else if (IsInfer) {
            //Debugger.Launch();
            if (retType == "Void") {
              DMSqlProcedure id = new DMSqlProcedure();
              retString = id.GetCreateString(true, m, AssemblyName, parameters);
              foreach (String s in retString)
                al.Add(s);
            }

            if (retType != "Void") {
              DMSqlFunction id = new DMSqlFunction();
              retString = id.GetCreateString(true, m, AssemblyName, parameters, retType);
              foreach (String s in retString)
                al.Add(s);
            }
          }
        }
      }

      return al;
    }

    bool CheckMethodExists(string realName, string aliasName, ArrayList alMeths) {
      string checkName = realName + ", " + aliasName;
      return alMeths.Contains(checkName);
    }

    internal static SqlDataReader ExecuteReader(string cmdText, string connectionString) {
      SqlConnection conn2 = null;
      SqlCommand cmd = null;

      conn2 = new SqlConnection(connectionString);

      //open the connection
      conn2.Open();
      cmd = conn2.CreateCommand();
      cmd.CommandText = cmdText;
      return cmd.ExecuteReader(CommandBehavior.CloseConnection);
    }

    //the method reads off the string sent in and
    //looks if there is a '.' in the string. In that case
    //it'll treat the part before the '.' as a schema name
    internal static string GetSysName(string _name, out string name) {
      string n = "";
      _name = _name.Replace("[", "");
      _name = _name.Replace("]", "");
      StringBuilder sb;
      StringBuilder sbName;
      string[] sar = _name.Split('.');
      if (sar.Length > 1) {
        sb = new StringBuilder();
        sbName = new StringBuilder();
        sb.Append("[" + sar[0] + "].[");
        if (sar.Length > 2) {
          for (int i = 1; i < sar.Length - 1; i++) {
            sb.Append(sar[i] + ".");
            sbName.Append(sar[i] + ".");
          }
        }
        sb.Append(sar[sar.Length - 1] + "]");
        sbName.Append(sar[sar.Length - 1]);
        n = sb.ToString();
        name = sbName.ToString();
      }

      else {
        n = "[" + _name + "]";
        name = _name;
      }

      return n;

    }

    internal static string GetExecString(string create) {
      string retString = "";
      StringBuilder sb = new StringBuilder();
      if (create.Contains("CREATE")) {
        string[] sar = create.Split('\n');
        string createString = sar[0].ToString();
        string[] exar = createString.Split(' ');
        sb.Append("-- ");
        if (createString.Contains("PROCEDURE")) {
          sb.Append("-- Test of proc " + createString.Replace("CREATE PROCEDURE ", ""));
          sb.Append("\n");
          sb.Append("-- -- Uncomment the statement below and\n");
          sb.Append("-- -- replace the eventual parameter names and types with real values.\n");

          retString = createString.Replace("CREATE PROCEDURE", "EXEC");
          sb.Append("-- ");
          sb.Append(retString);
        }

        else if (createString.Contains("FUNCTION") || createString.Contains("AGGREGATE")) {
          if (createString.Contains("FUNCTION"))
            sb.Append("-- Test of function " + createString.Replace("CREATE FUNCTION ", ""));
          else if (createString.Contains("AGGREGATE"))
            sb.Append("-- Test of aggregate " + createString.Replace("CREATE AGGREGATE ", ""));
          sb.Append("\n");
          sb.Append("-- -- Uncomment the statement below and\n");
          sb.Append("-- -- replace the eventual parameter names and types with real values.\n");

          if (sar[1].Contains("TABLE")) {
            retString = createString.Replace("CREATE FUNCTION", "SELECT * FROM");
            sb.Append("-- ");
            sb.Append(retString);
          }

          else {
            sb.Append("-- ");
            sb.Append("SELECT ");
            if (!exar[2].Contains("."))
              sb.Append("[dbo].");
            for (int i = 2; i < exar.Length; i++) {
              sb.Append(exar[i]);
              if (i < exar.Length - 1)
                sb.Append(" ");
            }
          }

        }

        else if (createString.Contains("TYPE")) {
          sb.Append("-- Test of UDT " + exar[2]);
          sb.Append("\n");
          sb.Append("-- -- Uncomment the statements below and\n");
          sb.Append("-- -- replace the '< >' block with real values.\n");
          sb.Append("-- DECLARE @x " + exar[2]);
          sb.Append(";\n");
          sb.Append("-- SET @x = ('<insert_values>')");

        }

        else if (createString.Contains("TRIGGER")) {
          sb.Append("-- Test of trigger " + exar[2]);
          sb.Append("\n");
          sb.Append("-- -- Uncomment the statement below and\n");
          sb.Append("-- -- replace the '< >' blocks with real columns/values.\n");
          string[] onar = sar[1].Split(' ');
          if (sar[1].ToUpper().Contains("INSERT")) {
            sb.Append("-- INSERT INTO " + onar[1] + "(<some_columns>) Values (<insert_some_values>)");

          }
          else if (sar[1].ToUpper().Contains("UPDATE")) {
            sb.Append("-- UPDATE " + onar[1] + "\n");
            sb.Append("-- SET <some_col> = <some_value>\n");
            sb.Append("-- WHERE <some_other_col> = <some_value>");

          }

          else if (sar[1].ToUpper().Contains("DELETE")) {
            sb.Append("-- DELETE " + onar[1] + "\n");
            sb.Append("-- WHERE <some_col> = <some_value>");

          }
          // this is no dml trigger
          else {
            sb.Append("-- It looks like this is a DDL trigger,\n");
            sb.Append("-- you have to enter the test code manually.");

          }




        }

        sb.Append(";\n");
        sb.Append("\n");
      }

      return sb.ToString();
    }

    internal static void WriteToFile(StreamWriter sw, string textToWrite, bool IncludeGo, bool DoNewLine) {
      string[] sar = textToWrite.Split(new char[] { '\n' });
      
      foreach (string s in sar)
        sw.WriteLine(s);

      if (IncludeGo)
        sw.WriteLine("GO");

      if (DoNewLine)
        sw.WriteLine();
    }
    
    internal static void LogMyComment(Task t, string msg) {
      
			//Console.WriteLine(msg);
      t.Log.LogMessage(MessageImportance.Normal, msg, null);
      //t.Log.LogMessage(
			//t.LogCommentFromText(BuildEventImportance.Normal, msg);
      //t.Log.LogCommentFromText(BuildEventImportance.Normal, msg);
    }

    internal static void LogMyErrorFromException(Task t, Exception msg) {
      //t.LogErrorFromException(msg);
      t.Log.LogErrorFromException(msg);
    }

    /// <summary>
    /// Adds a string to a filename.
    /// </summary>
    /// <param name="fullPath">Full name of file, including path.</param>
    /// <param name="addToName">The string to add to the name</param>
    /// <returns>The new full path.</returns>
    internal static string AddToFileName(string fullPath, string addToName) {
      string filePath = Path.GetDirectoryName(fullPath);
      string fileExt = Path.GetExtension(fullPath);
      string fileName = Path.GetFileNameWithoutExtension(fullPath) + "_" + addToName + fileExt;

      return Path.Combine(filePath, fileName);
    }

    internal static StreamWriter OpenFile(string scriptFile, bool createNew) {
      FileStream fs = null;

      //check if the directory exist
      string p = Path.GetDirectoryName(scriptFile);
      //if not create it
      if (!Directory.Exists(p))
        Directory.CreateDirectory(p);
           
      if (createNew) {
        if (File.Exists(scriptFile))
          File.Delete(scriptFile);

        fs = File.Create(scriptFile);
      }
      else {
        if (File.Exists(scriptFile)) {
          fs = new FileStream(scriptFile, FileMode.Append, FileAccess.Write);
          //fs = File.OpenWrite(scriptFile);
        }
        else
          fs = File.Create(scriptFile);
      }

      StreamWriter sw = new StreamWriter(fs);

      return sw;
    }

    internal static void WriteToDb(string cmdText, SqlConnection conn, SqlTransaction tx) {
      SqlCommand cmd = null;

      cmd = conn.CreateCommand();
      if(tx != null)
        cmd.Transaction = tx;
      cmd.CommandText = cmdText;
      cmd.ExecuteNonQuery();
      
    }

   
  }
}
