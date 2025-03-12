using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using InputMacro.Macro;
using Excel = Microsoft.Office.Interop.Excel;

namespace InputMacro
{
  public partial class Form1 : Form
  {
    Excel.Application _excelApplication = null;
    Excel.Workbook _workbook = null;
    Excel.Worksheet _worksheet = null;

    private GlobalHook _globalHook;
    private readonly Macro.Macro _macro = new Macro.Macro();
    private bool _isLoaded = false;

    private readonly List<(ESendKey exe, int x, int y)> _executions = new List<(ESendKey, int, int)>();
    
    public Form1()
    {
      InitializeComponent();
      _macro.LogSubmitted += MacroOnLogSubmitted;
      _macro.BeginExecute += MacroOnBeginExecute;
      _macro.MacroStopped += MacroOnMacroStopped;
      _macro.PropertyChanged += MacroOnPropertyChanged;
      _macro.RequestedCancellation += MacroOnRequestedCancellation;
    }
    private void MacroOnRequestedCancellation(object sender, EventArgs e)
    {
      Invoke((MethodInvoker)delegate {
        btnExecute.Enabled = false;
      });
    }
    private void MacroOnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (e.PropertyName == nameof(_macro.isExecuting))
      {
        Invoke((MethodInvoker)delegate {
          btnExecute.Text = _macro.isExecuting ? "중지 (F9)" : "실행 (F9)";
        });
      }
      
      Invoke((MethodInvoker)delegate {
        btnExecute.Enabled = _macro.isExecuting || _macro.isStandBy;
      });
    }
    private void MacroOnMacroStopped(object sender, Macro.Macro.MacroStoppedEventArgs e)
    {
      _macro.isStandBy = true;
    }
    private void MacroOnBeginExecute(object sender, Macro.Macro.ExecuteEventArgs e)
    {
      dgvDataView.ClearSelection();
      dgvDataView[_executions[e.index].x, _executions[e.index].y].Selected = true;
    }

    private void Form1_Load(object sender, EventArgs e)
    {
      var di = new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, "Data"));
      var set = new HashSet<string>();
      foreach (var se in di.GetFiles("*.xls").Concat(di.GetFiles("*.xlsx")).Select(x => x.Name))
        set.Add(se);
      txbDataPath.Items.AddRange(set.ToArray<object>());

      _globalHook = new GlobalHook();
      _globalHook.AddKeyDownHandler(KeyDownHandler);
      _globalHook.Begin();

      Log($"Select a macro.", LogPriorities.Info);
    }
    private void MacroOnLogSubmitted(object sender, Macro.Macro.LogSubmittedEventArgs e)
    {
      switch (e.priority)
      {
        case LogPriorities.PopUpInfo:
        case LogPriorities.PopUpWarning:
        {
          MessageBox.Show(e.message,"ipmcr", MessageBoxButtons.OK, e.priority == LogPriorities.PopUpInfo ? MessageBoxIcon.Asterisk : MessageBoxIcon.Warning);
          break;
        }
        default:
        {
          var color = Color.Black;
          switch (e.priority)
          {
            case LogPriorities.Info:
              color = Color.Blue;
              break;
            case LogPriorities.Warning:
              color = Color.Orange;
              break;
            case LogPriorities.Error:
              color = Color.DarkRed;
              break;
            case LogPriorities.Normal:
              color = Color.Green;
              break;
          }
          SetStatus(e.message, color);
          break;
        }
      }
    }

    private void Log(string message, LogPriorities priority) => MacroOnLogSubmitted(this, new Macro.Macro.LogSubmittedEventArgs(message, priority));

    private void KeyDownHandler(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.F9)
      {
        Toggle();
      }
    }

    #region ########## Load Files ##########

    private void CloseExcel()
    {
      try
      {
        _excelApplication.Quit();
        Log($"Closing Excel File...", LogPriorities.Info);
        ReleaseObject(_workbook);
        ReleaseObject(_worksheet);
        ReleaseObject(_excelApplication);
      }
      catch
      {
      }
    }

    private void txbDataPath_SelectedIndexChanged_1(object sender, EventArgs e)
    {
      try
      {
        CloseExcel();
        _isLoaded = false;
        Log($"Clearing Data...", LogPriorities.Info);
        TopMost = false;
        nudSpeed.Value = 0;
        dgvDataView.Columns.Clear();
        _executions.Clear();
        Log($"Open Excel Files...", LogPriorities.Info);
        _excelApplication = new Excel.Application
        {
          Visible = true
        };
        _workbook = _excelApplication.Workbooks.Open(Path.Combine(Environment.CurrentDirectory, "Data", txbDataPath.Text));
        _worksheet = (Excel.Worksheet)_workbook.Worksheets["macro"];
        _workbook.AfterSave += WorkbookOnAfterSave;
        _workbook.BeforeClose += WorkbookOnBeforeClose;
        Log($"Waiting to be saved...", LogPriorities.Info);
      }
      catch (Exception ex)
      {
        Log($"Error\n{ex.Message}", LogPriorities.PopUpWarning);
      }
    }

    private void WorkbookOnBeforeClose(ref bool cancel)
    {
      cancel = false;
      if (!_isLoaded)
        Invoke((MethodInvoker)delegate {
          Log($"Excel has closed. Please select a macro again.", LogPriorities.Warning);
        });
    }

    private void WorkbookOnAfterSave(bool success)
    {
      Invoke((MethodInvoker)delegate {
        if (success)
        {
          LoadData();
          TopMost = true;
        }
      });
    }

    private void LoadData()
    {
      Task.Factory.StartNew(() => {
        Invoke((MethodInvoker)delegate {
          Log($"Preparing Data Load...", LogPriorities.Info);
          _macro.isStandBy = false;
          try
          {
            Log($"Load Interval...", LogPriorities.Info);
            var interval = _worksheet.Cells[2, 2] as Excel.Range;
            if (interval?.Value != null && int.TryParse(interval.Value.ToString(), out var itv))
              nudSpeed.Value = itv;

            var r = 0;
            Log($"Load Columns...", LogPriorities.Info);
            while (true)
            {
              var columnHeader = _worksheet.Cells[2, 3 + r] as Excel.Range;
              if (columnHeader?.Value == null) break;
              dgvDataView.Columns.Add($"a{r}", $"{columnHeader.Value2}");
              Log($"Load Column: {columnHeader.Value}", LogPriorities.Info);
              r++;
            }

            Log($"Load Rows...", LogPriorities.Info);
            var c = 0;
            while (true)
            {
              var any = false;
              var ls = new List<object>();
              for (int i = 0; i < r; i++)
              {
                var x = _worksheet.Cells[3 + c, 3 + i] as Excel.Range;
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
                _executions.Add((new ESendKey(ls[i].ToString()), i, c));
                Log($"Load Cell[{i}, {c}]: {ls[i]}", LogPriorities.Info);
              }

              c++;
            }
            _isLoaded = true;
            _workbook.Close(false);
          }
          catch (Exception exc)
          {
            Log($"Data load error: {exc.Message}", LogPriorities.PopUpWarning);
          }
          finally
          {
            CloseExcel();
            _macro.isStandBy = true;
          }
        });
      });
    }

    #endregion


    private void SetStatus(string text) => SetStatus(text, Color.Black);

    private void SetStatus(string text, Color color)
    {
      Invoke((MethodInvoker)delegate {
        if (rtbLog.Lines.Length >= 512)
          rtbLog.Clear();
        rtbLog.SelectionStart = rtbLog.Text.Length;
        rtbLog.SelectionColor = color;
        rtbLog.SelectedText = text + "\r\n";
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

    private void Toggle()
    {
      if (!btnExecute.Enabled) return;
      
      if (_macro.isExecuting)
      {
        _macro.Cancel();
      }
      else
      {
        _macro.Start(_executions.Select(x => x.exe).ToArray<IExecutable>(), (int)nudSpeed.Value);
      }
    }


    private void Form1_FormClosing(object sender, FormClosingEventArgs e)
    {
      try
      {
        _workbook.AfterSave -= WorkbookOnAfterSave;
        _workbook.BeforeClose -= WorkbookOnBeforeClose;
        _workbook.Close(false);
        CloseExcel();
      }
      catch
      {
      }
    }

    private void btnExecute_Click(object sender, EventArgs e)
    {
      Toggle();
    }
    private void nudSpeed_ValueChanged(object sender, EventArgs e)
    {
      _macro.interval = (int)nudSpeed.Value;
    }
  }
}
