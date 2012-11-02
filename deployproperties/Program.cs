using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DM.Build.Yukon.DeployProperties {
  static class Program {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main(string[] args) {
      string projPath = "";
      Application.EnableVisualStyles();
      if (args.Length > 0) {
        projPath = args[0].ToString();
      }

      string execPath = Application.StartupPath;
      
      Application.Run(new PropertiesForm(projPath, execPath));
    }
  }
}