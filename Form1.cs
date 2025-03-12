using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace InputMacro
{
  public partial class Form1 : Form
  {

    Excel.Application excelApplication = null;
    Excel.Workbook workbook = null;
    Excel.Worksheet worksheet = null;

    private bool _started;
    private bool started {
      get => _started;
      set {
        _started = value;
        Invoke((MethodInvoker)delegate { btnExecute.Text = _started ? "중지 (F9)" : "실행 (F9)"; });
      }
    }

    private bool _standby = false;
    private bool standby {
      get => _standby;
      set {
        _standby = value;
        Invoke((MethodInvoker)delegate { btnExecute.Enabled = _standby; });
      }
    }
    private CancellationTokenSource cts = new CancellationTokenSource();
    private GlobalHook kHook;

    public Form1()
    {
      InitializeComponent();
    }
    private void Form1_Load(object sender, EventArgs e)
    {
      standby = false;
      var di = new DirectoryInfo(Environment.CurrentDirectory);
      txbDataPath.Items.AddRange(di.GetFiles("*.xls").Concat(di.GetFiles("*.xlsx")).Select(x => x.FullName).ToArray<object>());

      kHook = new GlobalHook();
      kHook.AddKeyDownHandler(KeyDownHandler);
      kHook.Begin();

      if (txbDataPath.Items.Count > 0)
        txbDataPath.Text = txbDataPath.Items[0].ToString();
    }
    private void KeyDownHandler(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.F9)
      {
        Toggle();
      }
    }

    private void LoadData(string path)
    {
      var t = new Task(() => {
        Invoke((MethodInvoker)delegate {
          _standby = false;
          nudSpeed.Value = 0;
          SetStatus("Preparing Data Load...", Color.Orange);
          SetStatus("Clearing Sheets...", Color.Orange);
          dgvDataView.Columns.Clear();
          try
          {
            SetStatus("Open Excel Files...", Color.Orange);
            excelApplication = new Excel.Application
            {
              Visible = false
            };
            workbook = excelApplication.Workbooks.Open(path);
            worksheet = (Excel.Worksheet)workbook.Worksheets[1];

            // var range = worksheet.Range["C3 : F102"];
            //
            // for (int row = 1; row <= range.Rows.Count; row++)
            // {
            //   var rv = range.Cells[row, 1] as Excel.Range;
            //   if (rv == null || rv.Value2 == null) continue;
            //   dgvDataView.Rows.Add();
            //   for (int column = 1; column <= range.Columns.Count; column++)
            //   {
            //     var value = range.Cells[row, column] as Excel.Range;
            //     dgvDataView[column - 1, row - 1].Value = value.Value2;
            //     SetStatus($"Load Cell: ({row}, {column}) {value.Value2}", Color.Orange);
            //   }
            // }
            SetStatus($"Load Interval...", Color.Orange);
            var interval = worksheet.Cells[2, 2] as Excel.Range;
            if (interval?.Value != null && int.TryParse(interval.Value.ToString(), out var itv))
              nudSpeed.Value = itv ;
            
            var r = 0;
            SetStatus($"Load Columns...", Color.Orange);
            while (true)
            {
              var columnHeader = worksheet.Cells[2, 3 + r] as Excel.Range;
              if (columnHeader?.Value == null) break;
              dgvDataView.Columns.Add($"a{r}", $"{columnHeader.Value2}");
              SetStatus($"Load Column: {columnHeader.Value}", Color.Orange);
              r++;
            }

            SetStatus($"Load Rows...", Color.Orange);
            var c = 0;
            while (true)
            {
              var any = false;
              var ls = new List<object>();
              for (int i = 0; i < r; i++)
              {
                var x = worksheet.Cells[3 + c, 3 + i] as Excel.Range;
                ls.Add(x?.Value);
                if (x?.Value != null && x.Value.ToString() != "")
                  any = true;

                // break;
              }
              if (!any) break;
              dgvDataView.Rows.Add();

              for (var i = 0; i < ls.Count; i++)
              {
                if (ls[i] == null) continue;
                dgvDataView[i, c].Value = ls[i];
                SetStatus($"Load Cell[{i}, {c}]: {ls[i]}", Color.Orange);
              }

              c++;
            }
          }
          catch (Exception exc)
          {
            MessageBox.Show(exc.Message);
          }
          finally
          {
            workbook.Close(false);
            excelApplication.Quit();
            ReleaseObject(workbook);
            ReleaseObject(worksheet);
            ReleaseObject(excelApplication);
            standby = true;
            SetStatus("StandBy", Color.Green);
          }
        });
      });
      t.Start();
    }

    private void SetStatus(string text) => SetStatus(text, Color.Black);
    private void SetStatus(string text, Color color)
    {
      Invoke((MethodInvoker)delegate {
        lbStatus.ForeColor = color;
        lbStatus.Text = text;
      });
    }

    static void ReleaseObject(object obj)
    {
      try
      {
        if (obj != null)
        {
          Marshal.ReleaseComObject(obj); // 액셀 객체 해제
          obj = null;
        }
      }
      catch (Exception ex)
      {
        obj = null;
        throw ex;
      }
      finally
      {
        GC.Collect(); // 가비지 수집
      }
    }

    private void Execute()
    {
      started = true;
      SetStatus("Starting Macro", Color.Blue);
      Task.Factory.StartNew(Exe, cts.Token);
    }

    private async void Exe()
    {
      try
      {
        for (var row = 0; row < dgvDataView.Rows.Count; row++)
        {
          for (var column = 0; column < dgvDataView.Columns.Count; column++)
          {
            var value = dgvDataView[column, row].Value;
            if (value == null || string.IsNullOrEmpty(value.ToString())) continue;
            SetStatus($"Send: {value}", Color.Blue);
            dgvDataView.ClearSelection();
            dgvDataView[column, row].Selected = true;
            SendKeys.SendWait(value.ToString());
            if(nudSpeed.Value > 0)
              await Task.Delay((int)nudSpeed.Value);
            cts.Token.ThrowIfCancellationRequested();
          }
        }
        started = false;
        SetStatus("Standby", Color.Green);
      }
      catch(OperationCanceledException exc)
      {
        started = false;
        SetStatus("StandBy", Color.Green);
      }
    }

    private void Toggle()
    {
      if (!standby) return;
      if (started)
      {
        SetStatus("Request Cancel...", Color.Green);
        cts.Cancel();
      }
      else
      {
        cts = new CancellationTokenSource();
        Execute();
      }
    }

    private void txbDataPath_SelectedIndexChanged_1(object sender, EventArgs e)
    {
      LoadData(txbDataPath.Text);
    }

    private void Form1_FormClosing(object sender, FormClosingEventArgs e)
    {
      try
      {
        workbook.Close(false);
        excelApplication.Quit();
        ReleaseObject(workbook);
        ReleaseObject(worksheet);
        ReleaseObject(excelApplication);
      }
      catch
      {
      }
    }
    private void btnExecute_Click(object sender, EventArgs e)
    {
      Toggle();
    }
  }
}
