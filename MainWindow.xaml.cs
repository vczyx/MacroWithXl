using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using InputMacro.Macro;
using InputMacro3.Data;
using InputMacro3.EasingFunction;
using InputMacro3.Macro;
using InputMacro3.Utils;
using Application = System.Windows.Application;
using Excel = Microsoft.Office.Interop.Excel;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using MessageBox = System.Windows.MessageBox;

namespace InputMacro3
{

  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow
  {

    public List<File> macroDataList { get; set; } = new List<File>();

    public string currentMacroData { get; set; } = string.Empty;

    public int loadProgressValue { get; set; } = 0;

    public int loadProgressMax { get; set; } = 0;

    public static readonly double[] Heights =
    {
      300, 150, 115, 130, 500
    };

    public static readonly double[] Widths =
    {
      350, 350, 350, 350, 350
    };

    public double[] configHeights =
    {
      0, 0, 0, 0, 0
    };

    public double[] configWidths =
    {
      0, 0, 0, 0, 0
    };

    public int curPage { get; set; }

    public int curFloor { get; set; }

    public const double AnimationDuration = 300;

    public MainWindow()
    {
      InitializeComponent();
      _globalHook = new GlobalHook();
      _globalHook.AddKeyDownHandler(KeyDownHandler);
      _globalHook.Begin();

      _macro.BeginExecute += MacroOnBeginExecute;
      _macro.MacroStarted += MacroOnMacroStarted;
      _macro.MacroStopped += MacroOnMacroStopped;
      _macro.PropertyChanged += MacroOnPropertyChanged;
      _macro.RequestedCancellation += MacroOnRequestedCancellation;

      // new Data.Config().CopyObjectJson();
    }
    public void MovePage(int page, int floor = 0)
    {
      var marginAnim = new ThicknessAnimation
      (
        MoveablePanel.Margin,
        new Thickness
        (
          Math.Max(Widths[page], configWidths[page]) * -page,
          Math.Max(Heights[page], configHeights[page]) * -floor,
          MoveablePanel.Margin.Right,
          MoveablePanel.Margin.Bottom
        ),
        TimeSpan.FromMilliseconds(AnimationDuration)
      ) { EasingFunction = new EaseOutCubic() { EasingMode = EasingMode.EaseIn, } };

      var widthAnim = new DoubleAnimation(
        MainGrid.ActualWidth,
        Math.Max(Widths[page], configWidths[page]),
        TimeSpan.FromMilliseconds(AnimationDuration)
      ) { EasingFunction = new EaseOutCubic { EasingMode = EasingMode.EaseIn, } };

      var heightAnim = new DoubleAnimation(
        MainGrid.ActualHeight,
        Math.Max(Heights[page], configHeights[page]),
        TimeSpan.FromMilliseconds(AnimationDuration)
      ) { EasingFunction = new EaseOutCubic() { EasingMode = EasingMode.EaseIn, } };

      MainGrid.BeginAnimation(WidthProperty, widthAnim);
      MainGrid.BeginAnimation(HeightProperty, heightAnim);
      MoveablePanel.BeginAnimation(MarginProperty, marginAnim);
      curPage = page;
      curFloor = floor;
    }


    #region Page1

    private void FindMacroData(string folderPath)
    {
      macroDataList.Clear();
      var files = Directory.GetFiles(folderPath);
      foreach (var file in files)
      {
        var icon = System.Drawing.Icon.ExtractAssociatedIcon(file);
        macroDataList.Add(new File { name = Path.GetFileName(file), icon = icon.ToImageSource() });
      }
      LvPg1Explorer.ItemsSource = macroDataList;
    }

    private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
      FindMacroData(Path.Combine(Environment.CurrentDirectory, "Data"));
      LvPg1Explorer.Focus();
    }

    private void ListViewExplorer_OnMouseDoubleClick(object sender, MouseButtonEventArgs e) => SelectFile();
    private void ListViewExplorer_OnKeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Enter) SelectFile();
    }
    private void Pg1NextButtonOnClick(object sender, RoutedEventArgs e)
      => SelectFile();

    private void SelectFile()
    {
      if (LvPg1Explorer.SelectedItem is File data)
      {
        if (string.IsNullOrEmpty(currentMacroData) || currentMacroData != Path.Combine(Environment.CurrentDirectory, "Data", data.name))
          OpenMacro();
        else MovePage(1);
      }
      else MessageBox.Show("매크로를 선택하세요.", "InputMacro", MessageBoxButton.OK, MessageBoxImage.Warning);
    }
    private void Pg1CloseButtonOnClick(object sender, RoutedEventArgs e) => Application.Current.Shutdown();
    private void OpenMacro()
    {
      if (!(LvPg1Explorer.SelectedItem is File data)) return;
      currentMacroData = Path.Combine(Environment.CurrentDirectory, "Data", data.name);
      Topmost = true;
      Topmost = false;
      _isLoaded = true;
      configHeights[1] = 0;
      configWidths[1] = 0;
      configHeights[3] = 0;
      configWidths[3] = 0;
      WindowUtilities.Clear();
      UpdateLayout();
      ImgPg2View.Source = null;
      ImgPg4View.Source = null;
      MovePage(1);
      Load();
    }

    #endregion

    #region Page2

    private void Pg2PreviousButtonOnClick(object sender, RoutedEventArgs e)
      => MovePage(0);

    private void Pg2NextButtonOnClick(object sender, RoutedEventArgs e)
    {
      if (MessageBox.Show("파일을 저장하지 않고 불러오시겠습니까?", "InputMacro", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
      {
        MovePage(2);
        LoadData();
        Topmost = true;
        Topmost = false;
      }
    }

    private void Pg2ConfirmButtonOnClick(object sender, RoutedEventArgs e)
    {
      MovePage(0);
      CloseExcel();
      currentMacroData = string.Empty;
    }

    #endregion


    #region Main

    Excel.Application _excelApplication = null;
    Excel.Workbook _workbook = null;
    Excel.Worksheet _worksheet = null;

    private readonly GlobalHook _globalHook;
    private readonly Macro.Macro _macro = new Macro.Macro();
    private bool _isLoaded = false;
    private Config _config;
    private int _progressValue;

    private readonly List<(string exe, int x, int y)> _executions = new List<(string, int, int)>();
    private void KeyDownHandler(object sender, System.Windows.Forms.KeyEventArgs e)
    {
      if (e.KeyCode == Keys.F9)
      {
        Toggle();
      }
    }

    private void CloseExcel()
    {
      try
      {
        _excelApplication.Quit();

        Console.WriteLine("Closing Excel File...");
        ReleaseObject(_workbook);
        ReleaseObject(_worksheet);
        ReleaseObject(_excelApplication);
      }
      catch
      {
      }
    }
    static void ReleaseObject(object obj)
    {
      try
      {
        if (obj == null) return;
        Marshal.ReleaseComObject(obj);
        obj = null;
      }
      catch (Exception ex)
      {
        obj = null;

        throw ex;
      }
      finally
      {
        GC.Collect();
      }
    }
    private void MainWindow_OnClosing(object sender, CancelEventArgs e)
    {
      try
      {
        _workbook.AfterSave -= WorkbookOnAfterSave;
        _workbook.BeforeClose -= WorkbookOnBeforeClose;
        if (_config.closeAfterLoad)
        {
          _workbook.Close(false);
          CloseExcel();
        }

        Application.Current.Shutdown();
      }
      catch
      {
      }
    }

    private Config LoadConfig(Excel.Workbook workbook)
    {
      try
      {
        if (workbook.Sheets["config"] is Excel.Worksheet configSheet)
        {
          var configRange = configSheet.Cells[1, 1] as Excel.Range;
          return configRange?.Value2.ToString().ToObjectFromJson<Config>();
        }
        else return null;
      }
      catch
      {
        return null;
      }
    }

    private void Load()
    {
      var bw = new BackgroundWorker();
      bw.DoWork += (sender, args) => {
        try
        {
          CloseExcel();
          _isLoaded = false;
          Dispatcher.Invoke(() => {
            BtnPg2Next.IsEnabled = false;
            TbPg2WaitMessage.Text = "엑셀 파일을 실행하는 중";
            TbPg2Desc.Text = "";
            TbPg4Desc.Text = "";
          });
          Console.WriteLine("Clearing Data...");

          _macro.interval = 0;
          Dispatcher.Invoke(() => { DgvPg5DataView.Columns.Clear(); });
          _executions.Clear();
          _config = new Config();
          Console.WriteLine("Open Excel Files...");
          _excelApplication = new Excel.Application
          {
            Visible = true
          };
          _workbook = _excelApplication.Workbooks.Open(currentMacroData);
          _workbook.AfterSave += WorkbookOnAfterSave;
          _workbook.BeforeClose += WorkbookOnBeforeClose;
          _config = LoadConfig(_workbook);
          if (_config == null)
          {
            Dispatcher.Invoke(() => {
              _isLoaded = true;
              CloseExcel();
              TbPg2Message.Text = "config가 존재하지 않습니다. 다른 매크로 파일을 선택하세요.";
              MovePage(1, 1);
            });
            return;
          }

          if (_config.forceLoad)
          {
            Dispatcher.Invoke(() => {
              MovePage(2);
              LoadData();
              Topmost = true;
              Topmost = false;
            });
            return;
          }
          var viewPageConfig = _config.viewPages[0];
          configWidths[1] = viewPageConfig.width;
          configHeights[1] = viewPageConfig.height;
          Dispatcher.Invoke(() => {
            Topmost = false;
            BtnPg2Next.IsEnabled = true;
            if (!string.IsNullOrEmpty(viewPageConfig.imageRange) && viewPageConfig.height > 0)
              ImgPg2View.Source = GetBitmapFromExcelRange("view", viewPageConfig.imageRange);
            if (!string.IsNullOrEmpty(viewPageConfig.descriptions))
              configHeights[1] += (configHeights[1] == 0 ? Heights[1] : 0) + 20;
            MovePage(1);
            TbPg2Desc.Text = viewPageConfig.descriptions;
            TbPg2WaitMessage.Text = "파일이 저장되기를 기다리는 중";
          });
          Console.WriteLine("Waiting to be saved...");
        }
        catch (Exception ex)
        {
          MessageBox.Show($"ERROR\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
      };
      bw.RunWorkerAsync();
    }

    private void WorkbookOnBeforeClose(ref bool cancel)
    {
      cancel = false;
      if (!_isLoaded)
        Dispatcher.Invoke(() => {
          TbPg2Message.Text = "매크로 파일이 종료되었습니다. 매크로 파일을 다시 선택하세요.";
          MovePage(1, 1);
        });
    }

    private void WorkbookOnAfterSave(bool success)
    {
      Dispatcher.Invoke(() => {
        if (success)
        {
          MovePage(2);
          LoadData();
          Topmost = true;
          Topmost = false;
        }
      });
    }

    private void LoadLog(string text)
    {
      Dispatcher.Invoke(() => { TbPg3Status.Text = $"[{Path.GetFileName(currentMacroData)}] {text}"; });
    }

    private void LoadData()
    {
      var bw = new BackgroundWorker();
      bw.DoWork += (sender, args) => {
        LoadLog("초기화 중...");
        loadProgressValue = 0;
        Dispatcher.Invoke(() => {
          DgvPg5DataView.Columns.Clear();
          DgvPg5DataView.Rows.Clear();
          _executions.Clear();
          PbPg4Progress.Value = 0;
          TbPg4Progress.Text = "0%";
        });
        _macro.isStandBy = false;

        try
        {
          loadProgressMax = 3;
          loadProgressValue = 0;
          Dispatcher.Invoke(() => {
            PbPg3Loading.Maximum = loadProgressMax;
            PbPg3Loading.Value = loadProgressValue;
            TbPg3Loading.Text = $"{Math.Floor((double)loadProgressValue / loadProgressMax * 100)}%";
          });

          LoadLog("설정을 불러오는 중...");
          _config = LoadConfig(_workbook);

          for (var i = 1; i <= _config.macroCount; i++)
          {
            var curChk = _workbook.Worksheets.Cast<Excel.Worksheet>().Any(sheet => sheet.Name == $"macro{i}");
            if (!curChk)
              throw new Exception($"매크로 설정 시트 \"macro{i}\"을(를) 찾을 수 없습니다.");
          }

          for (var i = 1; i <= _config.macroCount; i++)
          {
            var x = (Excel.Worksheet)_workbook.Worksheets[$"macro{i}"];
            loadProgressMax += Convert.ToInt32(x.Range["B2"].Value) + 1;
          }

          var viewPageConfig = _config.viewPages[1];
          configHeights[3] = viewPageConfig.height;
          configWidths[3] = viewPageConfig.width;
          if (!string.IsNullOrEmpty(viewPageConfig.descriptions))
            configHeights[3] += (configHeights[3] == 0 ? Heights[3] : 0) + 20;
          loadProgressValue++;

          LoadLog("이미지를 불러오는 중...");

          if (viewPageConfig.height > 0)
            Dispatcher.Invoke(() => { ImgPg4View.Source = GetBitmapFromExcelRange("view", viewPageConfig.imageRange); });

          loadProgressValue++;

          var maxColumnCount = 0;
          var maxRowCount = 0;
          for (var currentMacroIndex = 1; currentMacroIndex <= _config.macroCount; currentMacroIndex++)
          {
            _worksheet = (Excel.Worksheet)_workbook.Worksheets[$"macro{currentMacroIndex}"];

            // 열 목록(C2:~2) 불러오기
            var currentColumnCount = 0;
            LoadLog("열 목록을 불러오는 중...");
            while (true)
            {
              var columnHeader = _worksheet.Cells[2, 3 + currentColumnCount] as Excel.Range;
              if (columnHeader?.Value == null) break;

              LoadLog($"열 {currentColumnCount}({columnHeader.Value2})을 불러왔습니다");
              currentColumnCount++;
              if (maxColumnCount < currentColumnCount)
                Dispatcher.Invoke(() => { DgvPg5DataView.Columns.Add($"a{currentMacroIndex}_{currentColumnCount}", ""); });
              maxColumnCount = Math.Max(maxColumnCount, currentColumnCount);
            }
            loadProgressValue++;

            // 행 (데이터) 불러오기
            LoadLog("데이터를 불러오는 중...");
            var currentRowCount = 0;
            while (true)
            {
              var hasRowData = false;
              var ls = new List<object>();
              for (int i = 0; i < currentColumnCount; i++)
              {
                var x = _worksheet.Cells[3 + currentRowCount, 3 + i] as Excel.Range;
                ls.Add(x?.Value);
                if (x?.Value != null && x.Value.ToString() != "")
                  hasRowData = true;
              }
              if (!hasRowData) break;
              Dispatcher.Invoke(() => { DgvPg5DataView.Rows.Add(); });

              for (var i = 0; i < ls.Count; i++)
              {
                if (ls[i] == null) continue;
                Dispatcher.Invoke(() => { DgvPg5DataView[i, maxRowCount + currentRowCount].Value = ls[i]; });
                _executions.Add((ls[i].ToString(), i, maxRowCount + currentRowCount));
                LoadLog($"열 {maxRowCount + currentRowCount}의 행 {i}을 불러왔습니다. ({ls[i]})");
              }
              currentRowCount++;
              loadProgressValue++;
              Dispatcher.Invoke(() => {
                PbPg3Loading.Maximum = loadProgressMax;
                PbPg3Loading.Value = loadProgressValue;
                TbPg3Loading.Text = $"{Math.Floor((double)loadProgressValue / loadProgressMax * 100)}%";
              });
            }
            maxRowCount += currentRowCount;
          }
          Dispatcher.Invoke(() => {
            TbPg4Desc.Text = viewPageConfig.descriptions;
            TbPg4Title.Text = Path.GetFileName(currentMacroData);
            MovePage(3);
          });
          _macro.Set(_executions.Select(x => x.exe), _config.macroInterval);
          _macro.isStandBy = true;
        }
        catch (Exception exc)
        {
          MessageBox.Show($"데이터를 불러오는데 오류가 발생했습니다.\n{exc.Message}", "InputMacro", MessageBoxButton.OK, MessageBoxImage.Error);
          Dispatcher.Invoke(() => { MovePage(0); });
        }
        finally
        {
          _isLoaded = true;
          if (_config.closeAfterLoad)
          {
            _workbook.Close(false);
            CloseExcel();
          }
        }
      };
      bw.RunWorkerAsync();
    }

    private BitmapSource GetBitmapFromExcelRange(string sheetName, string range)
    {
      var sheet = _workbook.Sheets[sheetName] as Excel.Worksheet;
      var r = sheet?.Range[range];
      var isCopyObj = r?.CopyPicture(Excel.XlPictureAppearance.xlScreen, Excel.XlCopyPictureFormat.xlBitmap);
      var isCopy = false;
      if (isCopyObj is bool)
        isCopy = bool.Parse(isCopyObj.ToString());
      if (isCopy)
      {
        var img = (Bitmap)System.Windows.Forms.Clipboard.GetImage();
        return WindowUtilities.BitmapToBitmapSourceSafe(img);
      }
      else return null;
    }

    private void Toggle()
    {
      _executions.Select(x => {
        Console.WriteLine(x.exe);
        return x;
      });
      if (!BtnPg4Execute.IsEnabled) return;

      if (_macro.isExecuting)
        _macro.Cancel();
      else

        // _macro.Start(_executions.Select(x => x.exe).ToArray<IExecutable>(), _config.macro.interval);
        _macro.Start();
    }

    private void MacroOnRequestedCancellation(object sender, EventArgs e)
    {
      Dispatcher.Invoke(() => { BtnPg4Execute.IsEnabled = false; });
    }
    private void MacroOnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (e.PropertyName == nameof(_macro.isExecuting))
      {
        Dispatcher.Invoke(() => { BtnPg4Execute.Content = _macro.isExecuting ? "(중지 (F9" : "(실행 (F9"; });
      }

      Dispatcher.Invoke(() => { BtnPg4Execute.IsEnabled = _macro.isExecuting || _macro.isStandBy; });
    }
    private void MacroOnMacroStopped(object sender, Macro.Macro.MacroStoppedEventArgs e)
    {
      // MessageBox.Show("매크로가 종료되었습니다.", "InputMacro", MessageBoxButton.OK, MessageBoxImage.Information);
      _macro.isStandBy = true;
    }

    private void MacroOnMacroStarted(object sender, Macro.Macro.MacroStartedEventArgs e)
    {
      Dispatcher.Invoke(() => {
        _progressValue = 0;
        PbPg4Progress.Value = _progressValue;
        PbPg4Progress.Maximum = _executions.Count;
        TbPg4Progress.Text = $"{Math.Floor((double)_progressValue / _executions.Count * 100)}%";
      });
    }

    private void MacroOnBeginExecute(object sender, Macro.Macro.ExecuteEventArgs e)
    {
      Dispatcher.Invoke(() => {
        DgvPg5DataView.ClearSelection();
        DgvPg5DataView[_executions[e.index].x, _executions[e.index].y].Selected = true;
        _progressValue = e.index + 1;
        PbPg4Progress.Value = _progressValue;
        TbPg4Progress.Text = $"{Math.Floor((double)_progressValue / _executions.Count * 100)}%";
      });
    }

    #endregion

    #region Page4

    private void Pg4GoHomeButtonOnClick(object sender, RoutedEventArgs e)
    {
      MovePage(0, 0);
      CloseExcel();
      currentMacroData = string.Empty;
    }
    private void Pg4ExecuteButtonOnClick(object sender, RoutedEventArgs e)
    {
      Toggle();
    }

    private void Pg4ViewMacroButtonOnClick(object sender, MouseButtonEventArgs e) => MovePage(4);

    #endregion

    #region Page5

    private void Pg5PreviousButtonOnClick(object sender, RoutedEventArgs e) => MovePage(3);

  #endregion
  }
}
