using System;
using System.Reflection;
using System.Text;
using System.Data.Sql;
using Microsoft.SqlServer.Server;
using DM.Build.Yukon.Attributes.Service;

namespace DM.Build.Yukon.Attributes {

  
  public class DMSqlProcedure{
    string _name;
    
   
    public DMSqlProcedure(SqlProcedureAttribute sql) {
      if (sql.Name != string.Empty) {
        _name = sql.Name;
      }
        
    }

    public DMSqlProcedure() {
      
    }


    public Boolean Validate(MethodInfo m) {
      //as this is a procedure make sure the retval is either Void
      //or int
      bool success = false;

      if (m.ReturnType.ToString() == "System.Void" || m.ReturnType.ToString() == "System.Int32" || m.ReturnType.ToString() == "System.Data.SqlTypes.SqlInt32")
        success = true;
      else
        throw new ApplicationException("Return Value of a stored procedure can only be Integer or Void");
        

      return success;
    }

    //Method is called by the deployment task to create the execute statements for cataloging the stored procedure.
    public string[] GetCreateString(bool IsInfer, MethodInfo m, string AssemblyName, string parameters) {
      StringBuilder sb = null;
      string dropString = "";
      string createString = "";
      string stringName;
      string sysName = "";

      //if the name is explicetly set, use it. Otherwise use the method name.
      if (_name == string.Empty || _name == null)
        _name = m.Name;

      //if we infer methods from the task we still call into the attributes.
      //In this case we obviously need to derive the name from the method name.
      //we asppend the name with _sp as a method can be used both as proc as well as UDF
      if (IsInfer)
        _name = _name + "_sp";

      sysName = Utility.GetSysName(_name, out stringName);

      try {
        bool res = Validate(m);
        
        dropString = string.Format("if exists(select name from sys.objects where name = '{0}')\nDROP PROCEDURE {1}",stringName, sysName);

        sb = new StringBuilder();
               
        sb.Append(string.Format("CREATE PROCEDURE {0}", sysName));

        sb.Append(" " + parameters + "\n" + "AS \n");

        string externalName = string.Format("EXTERNAL NAME [{0}].[{1}].[{2}]", AssemblyName, m.DeclaringType.ToString(), m.Name);

        //append external name and name of proc
        sb.Append(externalName);
        createString = sb.ToString();
      }
      catch {
        throw;
      }
      return new string[] { dropString, createString };
    }
    
  }

  /// <summary>
  /// User Defined Function (UDF) attribute. The attribute is used for deployment tasks. This attribute indicates that the method the attribute is assigned to is to be created as an UDF.
  /// </summary>
  public class DMSqlFunction{
    string _name;
    string _tableParams;

    public DMSqlFunction() {
    }

    public DMSqlFunction(SqlFunctionAttribute sql) {
      if (sql.Name != string.Empty)
        _name = sql.Name;

      if (sql.TableDefinition != string.Empty)
        _tableParams = sql.TableDefinition;


    }
       
    public Boolean Validate(MethodInfo m) {
      bool success = true;

      //at the moment we only check for out/reparams 
      //this may change due to how void will be handled; i.e. TVF's and void
      foreach (ParameterInfo p in m.GetParameters()) {
        if (p.ParameterType.IsByRef) {
          success = false;
          throw new ApplicationException("UDF's do not allow out parameters");
        }
      }

      return success;
    }

    //Method is called by the deployment task to create the execute statements for cataloging the UDF.
    public string[] GetCreateString(bool IsInfer,MethodInfo m, string AssemblyName, string parameters, string retType) {
      StringBuilder sb = null;
      string dropString = "";
      string createString = "";
      string stringName;
      string sysName = "";

      //if the name is explicetly set, use it. Otherwise use the method name.
      if (_name == string.Empty||_name==null)
        _name = m.Name;

      
      //if we infer methods from the task we still call into the attributes.
      //In this case we obviously need to derive the name from the method name.
      //we append the name with _fn as a method can be used both as proc as well as UDF
      if (IsInfer)
        _name = _name + "_fn";

      sysName = Utility.GetSysName(_name, out stringName);


      try {
        bool res = Validate(m);
        bool onNull = false;

        dropString = string.Format("if exists(select name from sys.objects where name = '{0}')\nDROP FUNCTION {1}", stringName, sysName);
        
        sb = new StringBuilder();


        sb.Append(string.Format("CREATE FUNCTION {0}", sysName));

        sb.Append("(" + parameters + ")\n");

        if(retType=="TABLE")
          sb.Append("RETURNS " + retType + " (" + _tableParams + ")\n");
        else
          sb.Append("RETURNS " + retType + "\n");

        //check here for SqlMethod and returns null on null call
        SqlMethodAttribute[] attr = (SqlMethodAttribute[])m.GetCustomAttributes(typeof(SqlMethodAttribute), false);
        //loop and check for OnNullCall
        foreach (SqlMethodAttribute ma in attr) {
          if (!ma.OnNullCall)
            onNull = true;
        }

        if (onNull)
          sb.Append("WITH RETURNS NULL ON NULL INPUT\n");

        sb.Append("AS\n");

        string externalName = string.Format("EXTERNAL NAME [{0}].[{1}].[{2}]", AssemblyName, m.DeclaringType.ToString(), m.Name);

        //append external name and name of proc
        sb.Append(externalName);
        createString = sb.ToString();
      }
      catch{
        throw;
      }
      return new string[] { dropString, createString };
    }
    
  }

  public class DMSqlTrigger {
    string _tableName;
    string _name;
    string _triggerTypeAndAction;
    

    public DMSqlTrigger(SqlTriggerAttribute sql) {
      string stringName;
      _tableName = Utility.GetSysName(sql.Target, out stringName);
      _triggerTypeAndAction = sql.Event;
      if(sql.Name!=string.Empty)
        _name = sql.Name;

    }

    
    public Boolean Validate(MethodInfo m) {
      //check that we don't have any params as well
      //as return type other than void
      bool success = false;

      if (m.GetParameters().Length > 0) {
        throw new ApplicationException("A trigger does not allow parameters");
      }

      if (m.ReturnType.ToString()!="System.Void") {
        throw new ApplicationException("A trigger does not allow a return type other than Void");
      }

      return success;

    }

    public string[] GetCreateString(bool IsInfer, MethodInfo m, string AssemblyName) {
      StringBuilder sb = null;
      string createString = "";
      string dropString = "";
      string stringName;
      string sysName = "";

      //if the name is explicetly set, use it. Otherwise use the method name.
      if (_name == string.Empty || _name == null)
        _name = m.Name;

      sysName = Utility.GetSysName(_name, out stringName);
      
      try {
        bool res = Validate(m);

        dropString = string.Format("if exists(select name from sys.objects where name = '{0}')\nDROP TRIGGER {1}", stringName, sysName);

        sb = new StringBuilder();
                
        sb.Append(string.Format("CREATE TRIGGER {0}\n", sysName));

        

        sb.Append("ON " + _tableName + " " + _triggerTypeAndAction + "\n");
        sb.Append("AS\n");

        string externalName = string.Format("EXTERNAL NAME [{0}].[{1}].[{2}]", AssemblyName, m.DeclaringType.ToString(), m.Name);

        sb.Append(externalName);
        createString = sb.ToString();
      }
      catch {
        throw;
      }
      return new string[] { dropString, createString };
    }

    
  }

}
