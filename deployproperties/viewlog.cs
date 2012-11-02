using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace DM.Build.Yukon.DeployProperties {
  public partial class viewlog : Form {

    string msg = "";
    StringBuilder sb = new StringBuilder();

    public viewlog(StreamReader sr) {
      InitializeComponent();
      SetUpEvents();
      msg = sr.ReadToEnd();
      
    }

    public viewlog() {
      InitializeComponent();
      SetUpEvents();
      
    }

    public void AppendText(string txt) {
      sb.Append(txt);
    }





    void SetUpEvents() {
      btnOK.Click += new EventHandler(btnOK_Click);
      this.Shown += new EventHandler(viewlog_Shown);
    }

    void viewlog_Shown(object sender, EventArgs e) {
      this.rchView.Text = sb.ToString();
    }

    void btnOK_Click(object sender, EventArgs e) {
      this.Close();
      this.Dispose(); ;
    }
  }
}