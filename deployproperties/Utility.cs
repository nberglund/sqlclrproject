using System;
using System.Text;
using System.Data;
using System.Xml;

namespace DM.Build.Yukon.DeployProperties 
{
  class Utility
  {

    internal static void VerifySqlProjFile(string sqlProjPath)
    {

      DataSet ds = new DataSet();
      ds.ReadXml(sqlProjPath);
      VerifySqlProjFile(ds, sqlProjPath);

    }

    internal static bool VerifySqlProjFile(DataSet ds, string sqlProjPath)
    {
      DataTable dt = null;
      bool hasChanges = false;
      string ver = "2.6.0.0";
      string asmName = "yukondeploy,version=2.6.0.0,culture=neutral,publickeytoken=837e5cc1726a2c56";

      dt = ds.Tables["UsingTask"];
      foreach (DataRow row in dt.Rows)
      {
        if (!row["AssemblyName"].ToString().Contains(ver))
        {
          row["AssemblyName"] = asmName;
          hasChanges = true;
        }

      }

      //create the table which will hold the projfile with
      DataTable dtFile = new DataTable();
      dtFile.Columns.Add(new DataColumn("TableName", typeof(string)));
      dtFile.Columns.Add(new DataColumn("ColumnName", typeof(string)));
      dtFile.Columns.Add(new DataColumn("TypeName", typeof(string)));
      dtFile.Columns.Add(new DataColumn("ObjectValue", typeof(object)));
      dtFile.Columns.Add(new DataColumn("MapType", typeof(MappingType)));

      //add values
      dtFile.Rows.Add(new object[] { "PropertyGroup", "Assemblyname", "System.String", "$(AssemblyName)", MappingType.Element });
      dtFile.Rows.Add(new object[] { "PropertyGroup", "Alterassembly", "System.Boolean", false, MappingType.Element });
      dtFile.Rows.Add(new object[] { "PropertyGroup", "Uncheckeddata", "System.Boolean", false, MappingType.Element });
      dtFile.Rows.Add(new object[] { "PropertyGroup", "Permissionset", "System.Int32", 0, MappingType.Element });
      dtFile.Rows.Add(new object[] { "PropertyGroup", "ConnectDatabase", "System.Boolean", true, MappingType.Element });
      dtFile.Rows.Add(new object[] { "PropertyGroup", "Connectionstring", "System.String", "server=localhost;database=[DB_NAME];Integrated Security='SSPI'", MappingType.Element });
      dtFile.Rows.Add(new object[] { "PropertyGroup", "Infermethods", "System.Boolean", false, MappingType.Element });
      dtFile.Rows.Add(new object[] { "PropertyGroup", "DropTable", "System.Boolean", true, MappingType.Element });
      dtFile.Rows.Add(new object[] { "PropertyGroup", "Uploadsource", "System.Boolean", false, MappingType.Element });
      dtFile.Rows.Add(new object[] { "PropertyGroup", "Sourceextension", "System.String", "cs", MappingType.Element });
      dtFile.Rows.Add(new object[] { "PropertyGroup", "Sourcepath", "System.String", "$(MSBuildProjectDirectory)", MappingType.Element });
      dtFile.Rows.Add(new object[] { "PropertyGroup", "Usedeployattributes", "System.Boolean", true, MappingType.Element });
      dtFile.Rows.Add(new object[] { "PropertyGroup", "Deploydbgsymbols", "System.Boolean", true, MappingType.Element });
      dtFile.Rows.Add(new object[] { "PropertyGroup", "Debugpath", "System.String", "$(TargetDir)$(TargetName).pdb", MappingType.Element });
      dtFile.Rows.Add(new object[] { "PropertyGroup", "Castudtcolto", "System.Int32", 0, MappingType.Element });
      dtFile.Rows.Add(new object[] { "PropertyGroup", "Serverversion", "System.Int32", 0, MappingType.Element });
      dtFile.Rows.Add(new object[] { "DeployAssembly", "UsingDMDeployAttributes", "System.String", "$(Usedeployattributes)", MappingType.Attribute });
      dtFile.Rows.Add(new object[] { "DeployAssembly", "DeployDebugSymbols", "System.String", "$(Deploydbgsymbols)", MappingType.Attribute });
      dtFile.Rows.Add(new object[] { "DeployAssembly", "TypeToCastUDTTo", "System.String", "$(Castudtcolto)", MappingType.Attribute });
      dtFile.Rows.Add(new object[] { "DeployAssembly", "SqlServerVersion", "System.String", "$(Serverversion)", MappingType.Attribute });
      dtFile.Rows.Add(new object[] { "DeployTypes", "TypeToCastUDTTo", "System.String", "$(Castudtcolto)", MappingType.Attribute });
      dtFile.Rows.Add(new object[] { "DropAssembly", "TypeToCastUDTTo", "System.String", "$(Castudtcolto)", MappingType.Attribute });


      bool ret = false;

      //read off the values and check against the proj-file
      foreach (DataRow r in dtFile.Rows)
      {
        ret = CheckDataSet2(ds, r["TableName"].ToString(), r["ColumnName"].ToString(), r["TypeName"].ToString(), (object)r["ObjectValue"], (MappingType)r["MapType"]);
        if (ret)
          hasChanges = true;
      }

      if (hasChanges)
      {
        ds.AcceptChanges();
        DataSet dsUpdate = null;
        dsUpdate = ds.Copy();
        XmlDataDocument doc = new XmlDataDocument(dsUpdate);
        doc.Save(sqlProjPath);
        ds.Clear();
        ds.Dispose();

      }
      return hasChanges;
    }

    static bool CheckDataSet2(DataSet ds, string tableName, string colName, string typeName, object colValue, MappingType mte)
    {
      bool hasChanges = false;
      DataTable dt = ds.Tables[tableName];
      if (!dt.Columns.Contains(colName))
      {
        Type t = Type.GetType(typeName);
        dt.Columns.Add(colName, t);
        dt.Columns[colName].ColumnMapping = mte;
        dt.Rows[0][colName] = colValue;
        hasChanges = true;
      }


      return hasChanges;

    }

  }
}
