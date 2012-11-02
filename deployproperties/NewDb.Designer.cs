namespace DM.Build.Yukon.DeployProperties {
  partial class NewDb {
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
      this.textBox1 = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.txtDbName = new System.Windows.Forms.TextBox();
      this.btnCreate = new System.Windows.Forms.Button();
      this.btnCancel = new System.Windows.Forms.Button();
      this.label2 = new System.Windows.Forms.Label();
      this.txtSvr = new System.Windows.Forms.TextBox();
      this.SuspendLayout();
      // 
      // textBox1
      // 
      this.textBox1.BackColor = System.Drawing.SystemColors.Menu;
      this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.textBox1.Location = new System.Drawing.Point(13, 12);
      this.textBox1.Multiline = true;
      this.textBox1.Name = "textBox1";
      this.textBox1.Size = new System.Drawing.Size(267, 31);
      this.textBox1.TabIndex = 101;
      this.textBox1.Text = "Enter the name of the database you want to create, and then click on the \'Create " +
          "Db\' button.\r\n";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(12, 78);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(81, 13);
      this.label1.TabIndex = 102;
      this.label1.Text = "Database name:";
      // 
      // txtDbName
      // 
      this.txtDbName.Location = new System.Drawing.Point(99, 75);
      this.txtDbName.Name = "txtDbName";
      this.txtDbName.Size = new System.Drawing.Size(181, 20);
      this.txtDbName.TabIndex = 0;
      // 
      // btnCreate
      // 
      this.btnCreate.Location = new System.Drawing.Point(99, 110);
      this.btnCreate.Name = "btnCreate";
      this.btnCreate.Size = new System.Drawing.Size(75, 23);
      this.btnCreate.TabIndex = 1;
      this.btnCreate.Text = "Create Db";
      // 
      // btnCancel
      // 
      this.btnCancel.Location = new System.Drawing.Point(205, 110);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 2;
      this.btnCancel.Text = "Cancel";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(12, 52);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(37, 13);
      this.label2.TabIndex = 103;
      this.label2.Text = "Server:";
      // 
      // txtSvr
      // 
      this.txtSvr.Location = new System.Drawing.Point(99, 49);
      this.txtSvr.Name = "txtSvr";
      this.txtSvr.Size = new System.Drawing.Size(181, 20);
      this.txtSvr.TabIndex = 104;
      // 
      // NewDb
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(292, 146);
      this.Controls.Add(this.txtSvr);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.btnCreate);
      this.Controls.Add(this.txtDbName);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.textBox1);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "NewDb";
      this.Text = "New Database";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TextBox textBox1;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox txtDbName;
    private System.Windows.Forms.Button btnCreate;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox txtSvr;
  }
}