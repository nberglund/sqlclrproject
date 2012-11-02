using System;
using System.Reflection;
using Microsoft.SqlServer.Server;


namespace DM.Build.Yukon.Attributes {

  [AttributeUsage(AttributeTargets.Parameter)]
  public class SqlParamFacetAttribute : SqlFacetAttribute {

    private string _name;
    private object _val;

    public string Name {
      get { return _name; }
      set { _name = value; }
    }

    public object DefaultValue
    {
      get { return _val; }
      set { _val = value; }
    }


  }

}
