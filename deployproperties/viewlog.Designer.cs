namespace DM.Build.Yukon.DeployProperties {
  partial class viewlog {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      this.panel1 = new System.Windows.Forms.Panel();
      this.btnOK = new System.Windows.Forms.Button();
      this.rchView = new System.Windows.Forms.RichTextBox();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      // 
      // panel1
      // 
      this.panel1.Controls.Add(this.rchView);
      this.panel1.Location = new System.Drawing.Point(12, 12);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(541, 420);
      this.panel1.TabIndex = 0;
      // 
      // btnOK
      // 
      this.btnOK.Location = new System.Drawing.Point(478, 453);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new System.Drawing.Size(75, 23);
      this.btnOK.TabIndex = 1;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      // 
      // rchView
      // 
      this.rchView.Dock = System.Windows.Forms.DockStyle.Fill;
      this.rchView.Location = new System.Drawing.Point(0, 0);
      this.rchView.Name = "rchView";
      this.rchView.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
      this.rchView.Size = new System.Drawing.Size(541, 420);
      this.rchView.TabIndex = 0;
      this.rchView.Text = "";
      // 
      // viewlog
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(565, 488);
      this.Controls.Add(this.btnOK);
      this.Controls.Add(this.panel1);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Name = "viewlog";
      this.Text = "Deployment Log Viewer";
      this.panel1.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.RichTextBox rchView;
    private System.Windows.Forms.Button btnOK;


  }
}