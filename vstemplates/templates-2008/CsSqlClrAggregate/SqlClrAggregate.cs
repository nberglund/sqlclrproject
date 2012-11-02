using System;
using System.Data.Sql;
using System.Data.SqlTypes;
using System.Runtime.InteropServices;
using Microsoft.SqlServer.Server;
using DM.Build.Yukon.Attributes;


[Serializable]
[StructLayout(LayoutKind.Sequential)]
[SqlUserDefinedAggregate(Format.Native)]
public class $safeitemrootname$ {
  public void Init() {
    
  }

  public void Accumulate(SqlString Value) {
    
  }

  public void Merge ($safeitemrootname$ Group) {
    
  }

  public SqlString Terminate () {
    
    return "";
  }
}
