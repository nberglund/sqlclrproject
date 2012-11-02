using System;
using System.Data.Sql;
using System.Data.SqlTypes;
using System.Runtime.InteropServices;
using Microsoft.SqlServer.Server;
using DM.Build.Yukon.Attributes;


[Serializable]
[StructLayout(LayoutKind.Sequential)]
[SqlUserDefinedType(Format.Native)]
public class $safeitemrootname$ : INullable {
  
  
  public override string ToString() {
    string s = "null";

    return s;
  }

  public bool IsNull {
    get {return true;}
  }

  public static $safeitemrootname$ Null {
    get {
      $safeitemrootname$ h = new $safeitemrootname$();

      return h;
    }
  }

  public static $safeitemrootname$ Parse(SqlString s) {
    if (s.IsNull || s.Value.ToLower() == "null")
      return Null;

    $safeitemrootname$ u = new $safeitemrootname$();

    return u;
  }

}

