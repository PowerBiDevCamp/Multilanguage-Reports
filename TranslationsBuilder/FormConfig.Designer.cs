
namespace TranslationsBuilder {
  partial class FormConfig {
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
      this.lblServer = new System.Windows.Forms.Label();
      this.txtServer = new System.Windows.Forms.TextBox();
      this.lblDatabase = new System.Windows.Forms.Label();
      this.txtDatabase = new System.Windows.Forms.TextBox();
      this.lblTranslationsOutboxFolderPath = new System.Windows.Forms.Label();
      this.txtTranslationsOutboxFolderPath = new System.Windows.Forms.TextBox();
      this.lblTranslationsInboxFolderPath = new System.Windows.Forms.Label();
      this.txtTranslationsInboxFolderPath = new System.Windows.Forms.TextBox();
      this.btnSaveConfigurationChanges = new System.Windows.Forms.Button();
      this.lblAzureTranslatorServiceKey = new System.Windows.Forms.Label();
      this.txtAzureTranslatorServiceKey = new System.Windows.Forms.TextBox();
      this.lblAzureTranslatorServiceLocation = new System.Windows.Forms.Label();
      this.txtAzureTranslatorServiceLocation = new System.Windows.Forms.TextBox();
      this.btnCancel = new System.Windows.Forms.Button();
      this.btnSetOutboxPath = new System.Windows.Forms.Button();
      this.btnSetInboxPath = new System.Windows.Forms.Button();
      this.dialogSelectFolder = new System.Windows.Forms.FolderBrowserDialog();
      this.SuspendLayout();
      // 
      // lblServer
      // 
      this.lblServer.AutoSize = true;
      this.lblServer.Location = new System.Drawing.Point(10, 7);
      this.lblServer.Name = "lblServer";
      this.lblServer.Size = new System.Drawing.Size(39, 15);
      this.lblServer.TabIndex = 0;
      this.lblServer.Text = "Server";
      // 
      // txtServer
      // 
      this.txtServer.Location = new System.Drawing.Point(10, 24);
      this.txtServer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
      this.txtServer.Name = "txtServer";
      this.txtServer.Size = new System.Drawing.Size(578, 23);
      this.txtServer.TabIndex = 1;
      // 
      // lblDatabase
      // 
      this.lblDatabase.AutoSize = true;
      this.lblDatabase.Location = new System.Drawing.Point(10, 46);
      this.lblDatabase.Name = "lblDatabase";
      this.lblDatabase.Size = new System.Drawing.Size(46, 15);
      this.lblDatabase.TabIndex = 2;
      this.lblDatabase.Text = "Dataset";
      // 
      // txtDatabase
      // 
      this.txtDatabase.Location = new System.Drawing.Point(10, 64);
      this.txtDatabase.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
      this.txtDatabase.Name = "txtDatabase";
      this.txtDatabase.Size = new System.Drawing.Size(578, 23);
      this.txtDatabase.TabIndex = 3;
      // 
      // lblTranslationsOutboxFolderPath
      // 
      this.lblTranslationsOutboxFolderPath.AutoSize = true;
      this.lblTranslationsOutboxFolderPath.Location = new System.Drawing.Point(10, 87);
      this.lblTranslationsOutboxFolderPath.Name = "lblTranslationsOutboxFolderPath";
      this.lblTranslationsOutboxFolderPath.Size = new System.Drawing.Size(175, 15);
      this.lblTranslationsOutboxFolderPath.TabIndex = 4;
      this.lblTranslationsOutboxFolderPath.Text = "Translations Outbox Folder Path";
      // 
      // txtTranslationsOutboxFolderPath
      // 
      this.txtTranslationsOutboxFolderPath.Location = new System.Drawing.Point(10, 104);
      this.txtTranslationsOutboxFolderPath.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
      this.txtTranslationsOutboxFolderPath.Name = "txtTranslationsOutboxFolderPath";
      this.txtTranslationsOutboxFolderPath.Size = new System.Drawing.Size(578, 23);
      this.txtTranslationsOutboxFolderPath.TabIndex = 5;
      // 
      // lblTranslationsInboxFolderPath
      // 
      this.lblTranslationsInboxFolderPath.AutoSize = true;
      this.lblTranslationsInboxFolderPath.Location = new System.Drawing.Point(10, 129);
      this.lblTranslationsInboxFolderPath.Name = "lblTranslationsInboxFolderPath";
      this.lblTranslationsInboxFolderPath.Size = new System.Drawing.Size(165, 15);
      this.lblTranslationsInboxFolderPath.TabIndex = 6;
      this.lblTranslationsInboxFolderPath.Text = "Translations Inbox Folder Path";
      // 
      // txtTranslationsInboxFolderPath
      // 
      this.txtTranslationsInboxFolderPath.Location = new System.Drawing.Point(10, 148);
      this.txtTranslationsInboxFolderPath.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
      this.txtTranslationsInboxFolderPath.Name = "txtTranslationsInboxFolderPath";
      this.txtTranslationsInboxFolderPath.Size = new System.Drawing.Size(578, 23);
      this.txtTranslationsInboxFolderPath.TabIndex = 7;
      // 
      // btnSaveConfigurationChanges
      // 
      this.btnSaveConfigurationChanges.Location = new System.Drawing.Point(613, 16);
      this.btnSaveConfigurationChanges.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
      this.btnSaveConfigurationChanges.Name = "btnSaveConfigurationChanges";
      this.btnSaveConfigurationChanges.Size = new System.Drawing.Size(149, 28);
      this.btnSaveConfigurationChanges.TabIndex = 10;
      this.btnSaveConfigurationChanges.Text = "Save Changes";
      this.btnSaveConfigurationChanges.UseVisualStyleBackColor = true;
      this.btnSaveConfigurationChanges.Click += new System.EventHandler(this.btnSaveConfigurationChanges_Click);
      // 
      // lblAzureTranslatorServiceKey
      // 
      this.lblAzureTranslatorServiceKey.AutoSize = true;
      this.lblAzureTranslatorServiceKey.Location = new System.Drawing.Point(10, 172);
      this.lblAzureTranslatorServiceKey.Name = "lblAzureTranslatorServiceKey";
      this.lblAzureTranslatorServiceKey.Size = new System.Drawing.Size(153, 15);
      this.lblAzureTranslatorServiceKey.TabIndex = 9;
      this.lblAzureTranslatorServiceKey.Text = "Azure Translator Service Key";
      // 
      // txtAzureTranslatorServiceKey
      // 
      this.txtAzureTranslatorServiceKey.Location = new System.Drawing.Point(10, 189);
      this.txtAzureTranslatorServiceKey.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
      this.txtAzureTranslatorServiceKey.Name = "txtAzureTranslatorServiceKey";
      this.txtAzureTranslatorServiceKey.Size = new System.Drawing.Size(577, 23);
      this.txtAzureTranslatorServiceKey.TabIndex = 8;
      // 
      // lblAzureTranslatorServiceLocation
      // 
      this.lblAzureTranslatorServiceLocation.AutoSize = true;
      this.lblAzureTranslatorServiceLocation.Location = new System.Drawing.Point(11, 214);
      this.lblAzureTranslatorServiceLocation.Name = "lblAzureTranslatorServiceLocation";
      this.lblAzureTranslatorServiceLocation.Size = new System.Drawing.Size(252, 15);
      this.lblAzureTranslatorServiceLocation.TabIndex = 11;
      this.lblAzureTranslatorServiceLocation.Text = "Azure Translator Service Location (e.g. eastus2)";
      // 
      // txtAzureTranslatorServiceLocation
      // 
      this.txtAzureTranslatorServiceLocation.Location = new System.Drawing.Point(11, 232);
      this.txtAzureTranslatorServiceLocation.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
      this.txtAzureTranslatorServiceLocation.Name = "txtAzureTranslatorServiceLocation";
      this.txtAzureTranslatorServiceLocation.Size = new System.Drawing.Size(576, 23);
      this.txtAzureTranslatorServiceLocation.TabIndex = 9;
      // 
      // btnCancel
      // 
      this.btnCancel.Location = new System.Drawing.Point(613, 51);
      this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(149, 27);
      this.btnCancel.TabIndex = 13;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
      // 
      // btnSetOutboxPath
      // 
      this.btnSetOutboxPath.Location = new System.Drawing.Point(593, 100);
      this.btnSetOutboxPath.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
      this.btnSetOutboxPath.Name = "btnSetOutboxPath";
      this.btnSetOutboxPath.Size = new System.Drawing.Size(46, 27);
      this.btnSetOutboxPath.TabIndex = 14;
      this.btnSetOutboxPath.Text = "set";
      this.btnSetOutboxPath.UseVisualStyleBackColor = true;
      this.btnSetOutboxPath.Click += new System.EventHandler(this.SetOutboxPath);
      // 
      // btnSetInboxPath
      // 
      this.btnSetInboxPath.Location = new System.Drawing.Point(593, 144);
      this.btnSetInboxPath.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
      this.btnSetInboxPath.Name = "btnSetInboxPath";
      this.btnSetInboxPath.Size = new System.Drawing.Size(46, 27);
      this.btnSetInboxPath.TabIndex = 15;
      this.btnSetInboxPath.Text = "set";
      this.btnSetInboxPath.UseVisualStyleBackColor = true;
      this.btnSetInboxPath.Click += new System.EventHandler(this.SetInboxPath);
      // 
      // FormConfig
      // 
      this.AcceptButton = this.btnSaveConfigurationChanges;
      this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.SystemColors.ActiveCaption;
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size(788, 274);
      this.Controls.Add(this.btnSetInboxPath);
      this.Controls.Add(this.btnSetOutboxPath);
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.txtAzureTranslatorServiceLocation);
      this.Controls.Add(this.lblAzureTranslatorServiceLocation);
      this.Controls.Add(this.txtAzureTranslatorServiceKey);
      this.Controls.Add(this.lblAzureTranslatorServiceKey);
      this.Controls.Add(this.btnSaveConfigurationChanges);
      this.Controls.Add(this.txtTranslationsInboxFolderPath);
      this.Controls.Add(this.lblTranslationsInboxFolderPath);
      this.Controls.Add(this.txtTranslationsOutboxFolderPath);
      this.Controls.Add(this.lblTranslationsOutboxFolderPath);
      this.Controls.Add(this.txtDatabase);
      this.Controls.Add(this.lblDatabase);
      this.Controls.Add(this.txtServer);
      this.Controls.Add(this.lblServer);
      this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
      this.Name = "FormConfig";
      this.Text = "Configuration Options";
      this.Load += new System.EventHandler(this.FormConfig_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label lblServer;
    private System.Windows.Forms.TextBox txtServer;
    private System.Windows.Forms.Label lblDatabase;
    private System.Windows.Forms.TextBox txtDatabase;
    private System.Windows.Forms.Label lblTranslationsOutboxFolderPath;
    private System.Windows.Forms.TextBox txtTranslationsOutboxFolderPath;
    private System.Windows.Forms.Label lblTranslationsInboxFolderPath;
    private System.Windows.Forms.TextBox txtTranslationsInboxFolderPath;
    private System.Windows.Forms.Button btnSaveConfigurationChanges;
    private System.Windows.Forms.Label lblAzureTranslatorServiceKey;
    private System.Windows.Forms.TextBox txtAzureTranslatorServiceKey;
    private System.Windows.Forms.Label lblAzureTranslatorServiceLocation;
    private System.Windows.Forms.TextBox txtAzureTranslatorServiceLocation;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Button btnSetOutboxPath;
    private System.Windows.Forms.Button btnSetInboxPath;
    private System.Windows.Forms.FolderBrowserDialog dialogSelectFolder;
  }
}