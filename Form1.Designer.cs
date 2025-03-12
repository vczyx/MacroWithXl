namespace InputMacro
{
  partial class Form1
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
      this.dgvDataView = new System.Windows.Forms.DataGridView();
      this.btnExecute = new System.Windows.Forms.Button();
      this.lbStatus = new System.Windows.Forms.Label();
      this.txbDataPath = new System.Windows.Forms.ComboBox();
      this.nudSpeed = new System.Windows.Forms.NumericUpDown();
      this.label1 = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.dgvDataView)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.nudSpeed)).BeginInit();
      this.SuspendLayout();

      // 
      // dgvDataView
      // 
      this.dgvDataView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
      this.dgvDataView.Location = new System.Drawing.Point(12, 39);
      this.dgvDataView.Name = "dgvDataView";
      this.dgvDataView.Size = new System.Drawing.Size(470, 291);
      this.dgvDataView.TabIndex = 0;

      // 
      // btnExecute
      // 
      this.btnExecute.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.btnExecute.Location = new System.Drawing.Point(12, 336);
      this.btnExecute.Name = "btnExecute";
      this.btnExecute.Size = new System.Drawing.Size(75, 23);
      this.btnExecute.TabIndex = 2;
      this.btnExecute.Text = "실행 (F9)";
      this.btnExecute.UseVisualStyleBackColor = true;
      this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);

      // 
      // lbStatus
      // 
      this.lbStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
      this.lbStatus.Location = new System.Drawing.Point(93, 336);
      this.lbStatus.Name = "lbStatus";
      this.lbStatus.Size = new System.Drawing.Size(389, 23);
      this.lbStatus.TabIndex = 3;
      this.lbStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

      // 
      // txbDataPath
      // 
      this.txbDataPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
      this.txbDataPath.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.txbDataPath.FormattingEnabled = true;
      this.txbDataPath.Location = new System.Drawing.Point(12, 12);
      this.txbDataPath.Name = "txbDataPath";
      this.txbDataPath.Size = new System.Drawing.Size(351, 20);
      this.txbDataPath.TabIndex = 4;
      this.txbDataPath.SelectedIndexChanged += new System.EventHandler(this.txbDataPath_SelectedIndexChanged_1);

      // 
      // nudSpeed
      // 
      this.nudSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.nudSpeed.Location = new System.Drawing.Point(418, 11);
      this.nudSpeed.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
      this.nudSpeed.Name = "nudSpeed";
      this.nudSpeed.Size = new System.Drawing.Size(64, 21);
      this.nudSpeed.TabIndex = 5;

      // 
      // label1
      // 
      this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.label1.Location = new System.Drawing.Point(369, 11);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(43, 21);
      this.label1.TabIndex = 6;
      this.label1.Text = "속도";
      this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

      // 
      // Form1
      // 
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.ClientSize = new System.Drawing.Size(494, 371);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.nudSpeed);
      this.Controls.Add(this.txbDataPath);
      this.Controls.Add(this.lbStatus);
      this.Controls.Add(this.btnExecute);
      this.Controls.Add(this.dgvDataView);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.Name = "Form1";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Input Macro 1.2";
      this.TopMost = true;
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
      this.Load += new System.EventHandler(this.Form1_Load);
      ((System.ComponentModel.ISupportInitialize)(this.dgvDataView)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.nudSpeed)).EndInit();
      this.ResumeLayout(false);
    }
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.NumericUpDown nudSpeed;
    private System.Windows.Forms.ComboBox txbDataPath;
    private System.Windows.Forms.Label lbStatus;
    private System.Windows.Forms.Button btnExecute;
    private System.Windows.Forms.DataGridView dgvDataView;

    #endregion
  }
}
