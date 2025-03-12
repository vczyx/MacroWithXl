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
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
      this.dgvDataView = new System.Windows.Forms.DataGridView();
      this.btnExecute = new System.Windows.Forms.Button();
      this.lbStatus = new System.Windows.Forms.Label();
      this.txbDataPath = new System.Windows.Forms.ComboBox();
      this.nudSpeed = new System.Windows.Forms.NumericUpDown();
      this.label1 = new System.Windows.Forms.Label();
      this.tabControl1 = new System.Windows.Forms.TabControl();
      this.tabPage1 = new System.Windows.Forms.TabPage();
      this.label2 = new System.Windows.Forms.Label();
      this.tabPage3 = new System.Windows.Forms.TabPage();
      this.rtbLog = new System.Windows.Forms.RichTextBox();
      ((System.ComponentModel.ISupportInitialize)(this.dgvDataView)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.nudSpeed)).BeginInit();
      this.tabControl1.SuspendLayout();
      this.tabPage1.SuspendLayout();
      this.tabPage3.SuspendLayout();
      this.SuspendLayout();

      // 
      // dgvDataView
      // 
      this.dgvDataView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
      this.dgvDataView.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
      this.dgvDataView.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.dgvDataView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
      dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
      dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
      dataGridViewCellStyle1.Font = new System.Drawing.Font("맑은 고딕", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
      dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
      dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
      dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
      dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.dgvDataView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
      this.dgvDataView.ColumnHeadersHeight = 20;
      this.dgvDataView.ColumnHeadersVisible = false;
      dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
      dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
      dataGridViewCellStyle2.Font = new System.Drawing.Font("맑은 고딕", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
      dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
      dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
      dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
      dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
      this.dgvDataView.DefaultCellStyle = dataGridViewCellStyle2;
      this.dgvDataView.GridColor = System.Drawing.Color.Gray;
      this.dgvDataView.Location = new System.Drawing.Point(6, 33);
      this.dgvDataView.Name = "dgvDataView";
      this.dgvDataView.ReadOnly = true;
      this.dgvDataView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
      dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
      dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
      dataGridViewCellStyle3.Font = new System.Drawing.Font("맑은 고딕", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
      dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
      dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
      dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White;
      dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.dgvDataView.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
      this.dgvDataView.RowHeadersVisible = false;
      dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
      dataGridViewCellStyle4.BackColor = System.Drawing.Color.WhiteSmoke;
      dataGridViewCellStyle4.Font = new System.Drawing.Font("맑은 고딕", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
      dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
      dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
      dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.White;
      dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.dgvDataView.RowsDefaultCellStyle = dataGridViewCellStyle4;
      this.dgvDataView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
      this.dgvDataView.Size = new System.Drawing.Size(453, 199);
      this.dgvDataView.TabIndex = 0;

      // 
      // btnExecute
      // 
      this.btnExecute.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.btnExecute.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
      this.btnExecute.Enabled = false;
      this.btnExecute.FlatAppearance.BorderSize = 0;
      this.btnExecute.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnExecute.Font = new System.Drawing.Font("맑은 고딕", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
      this.btnExecute.Location = new System.Drawing.Point(12, 307);
      this.btnExecute.Name = "btnExecute";
      this.btnExecute.Size = new System.Drawing.Size(104, 52);
      this.btnExecute.TabIndex = 2;
      this.btnExecute.Text = "실행 (F9)";
      this.btnExecute.UseVisualStyleBackColor = false;
      this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);

      // 
      // lbStatus
      // 
      this.lbStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
      this.lbStatus.Location = new System.Drawing.Point(122, 307);
      this.lbStatus.Name = "lbStatus";
      this.lbStatus.Size = new System.Drawing.Size(360, 52);
      this.lbStatus.TabIndex = 3;
      this.lbStatus.TextAlign = System.Drawing.ContentAlignment.BottomRight;

      // 
      // txbDataPath
      // 
      this.txbDataPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
      this.txbDataPath.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
      this.txbDataPath.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.txbDataPath.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.txbDataPath.FormattingEnabled = true;
      this.txbDataPath.Location = new System.Drawing.Point(12, 12);
      this.txbDataPath.Name = "txbDataPath";
      this.txbDataPath.Size = new System.Drawing.Size(470, 23);
      this.txbDataPath.TabIndex = 4;
      this.txbDataPath.SelectedIndexChanged += new System.EventHandler(this.txbDataPath_SelectedIndexChanged_1);

      // 
      // nudSpeed
      // 
      this.nudSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.nudSpeed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.nudSpeed.Location = new System.Drawing.Point(62, 3);
      this.nudSpeed.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
      this.nudSpeed.Name = "nudSpeed";
      this.nudSpeed.Size = new System.Drawing.Size(64, 24);
      this.nudSpeed.TabIndex = 5;
      this.nudSpeed.ValueChanged += new System.EventHandler(this.nudSpeed_ValueChanged);

      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.label1.Location = new System.Drawing.Point(6, 5);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(50, 15);
      this.label1.TabIndex = 6;
      this.label1.Text = "간격 시간";
      this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

      // 
      // tabControl1
      // 
      this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
      this.tabControl1.Controls.Add(this.tabPage1);
      this.tabControl1.Controls.Add(this.tabPage3);
      this.tabControl1.Location = new System.Drawing.Point(12, 38);
      this.tabControl1.Name = "tabControl1";
      this.tabControl1.SelectedIndex = 0;
      this.tabControl1.Size = new System.Drawing.Size(470, 263);
      this.tabControl1.TabIndex = 7;

      // 
      // tabPage1
      // 
      this.tabPage1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
      this.tabPage1.Controls.Add(this.dgvDataView);
      this.tabPage1.Controls.Add(this.label2);
      this.tabPage1.Controls.Add(this.label1);
      this.tabPage1.Controls.Add(this.nudSpeed);
      this.tabPage1.Location = new System.Drawing.Point(4, 24);
      this.tabPage1.Name = "tabPage1";
      this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
      this.tabPage1.Size = new System.Drawing.Size(462, 235);
      this.tabPage1.TabIndex = 0;
      this.tabPage1.Text = "매크로";

      // 
      // label2
      // 
      this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.label2.AutoSize = true;
      this.label2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.label2.Location = new System.Drawing.Point(132, 5);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(22, 15);
      this.label2.TabIndex = 6;
      this.label2.Text = "ms";
      this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

      // 
      // tabPage3
      // 
      this.tabPage3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
      this.tabPage3.Controls.Add(this.rtbLog);
      this.tabPage3.Location = new System.Drawing.Point(4, 22);
      this.tabPage3.Name = "tabPage3";
      this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
      this.tabPage3.Size = new System.Drawing.Size(462, 237);
      this.tabPage3.TabIndex = 2;
      this.tabPage3.Text = "Log";

      // 
      // rtbLog
      // 
      this.rtbLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
      this.rtbLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.rtbLog.Cursor = System.Windows.Forms.Cursors.Default;
      this.rtbLog.Dock = System.Windows.Forms.DockStyle.Fill;
      this.rtbLog.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.rtbLog.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
      this.rtbLog.HideSelection = false;
      this.rtbLog.Location = new System.Drawing.Point(3, 3);
      this.rtbLog.Name = "rtbLog";
      this.rtbLog.ReadOnly = true;
      this.rtbLog.Size = new System.Drawing.Size(456, 231);
      this.rtbLog.TabIndex = 0;
      this.rtbLog.Text = "";

      // 
      // Form1
      // 
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.BackColor = System.Drawing.Color.White;
      this.ClientSize = new System.Drawing.Size(494, 371);
      this.Controls.Add(this.tabControl1);
      this.Controls.Add(this.txbDataPath);
      this.Controls.Add(this.lbStatus);
      this.Controls.Add(this.btnExecute);
      this.Font = new System.Drawing.Font("맑은 고딕", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.Name = "Form1";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Input Macro 2.0";
      this.TopMost = true;
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
      this.Load += new System.EventHandler(this.Form1_Load);
      ((System.ComponentModel.ISupportInitialize)(this.dgvDataView)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.nudSpeed)).EndInit();
      this.tabControl1.ResumeLayout(false);
      this.tabPage1.ResumeLayout(false);
      this.tabPage1.PerformLayout();
      this.tabPage3.ResumeLayout(false);
      this.ResumeLayout(false);
    }
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.RichTextBox rtbLog;
    private System.Windows.Forms.TabPage tabPage3;
    private System.Windows.Forms.TabControl tabControl1;
    private System.Windows.Forms.TabPage tabPage1;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.NumericUpDown nudSpeed;
    private System.Windows.Forms.ComboBox txbDataPath;
    private System.Windows.Forms.Label lbStatus;
    private System.Windows.Forms.Button btnExecute;
    private System.Windows.Forms.DataGridView dgvDataView;

    #endregion
  }
}
