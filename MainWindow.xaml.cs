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
using InputMacro3.EasingFunction;
using InputMacro3.Macro;
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

    public static readonly int[] heights =
    {
      300, 150, 115, 120, 500
    };

    public int[] configHeights =
    {
      0, 0, 0, 0, 0
    };

    public ImageSource helpImage { get; set; }

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
    }
    public void MovePage(int page, int floor = 0)
    {
      var marginAnim = new ThicknessAnimation
      (
        MoveablePanel.Margin,
        new Thickness(MainGrid.ActualWidth * -page, MainGrid.ActualHeight * -floor, MoveablePanel.Margin.Right, MoveablePanel.Margin.Bottom),
        TimeSpan.FromMilliseconds(AnimationDuration)
      )
      {
        EasingFunction = new EaseOutCubic()
        {
          EasingMode = EasingMode.EaseIn,
        }
      };

      var heightAnim = new DoubleAnimation(
        MainGrid.ActualHeight,
        Math.Max(heights[page], configHeights[page]),
        TimeSpan.FromMilliseconds(AnimationDuration)
      )
      {
        EasingFunction = new EaseOutCubic()
        {
          EasingMode = EasingMode.EaseIn,
        }
      };

      BeginAnimation(HeightProperty, heightAnim);
      MainGrid.BeginAnimation(HeightProperty, heightAnim);
      MoveablePanel.BeginAnimation(MarginProperty, marginAnim);
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
      ListViewExplorer.ItemsSource = macroDataList;
    }

    private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
      FindMacroData(Path.Combine(Environment.CurrentDirectory, "Data"));
      ListViewExplorer.Focus();
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
      if (ListViewExplorer.SelectedItem is File data && currentMacroData != Path.Combine(Environment.CurrentDirectory, "Data", data.name))
      {
        if (string.IsNullOrEmpty(currentMacroData)) OpenMacro();
        else MovePage(1);
      }
      else MessageBox.Show("매크로를 선택하세요.", "InputMacro", MessageBoxButton.OK, MessageBoxImage.Warning);
    }
    private void Pg1CloseButtonOnClick(object sender, RoutedEventArgs e) => Application.Current.Shutdown();
    private void OpenMacro()
    {
      if (!(ListViewExplorer.SelectedItem is File data)) return;
      currentMacroData = Path.Combine(Environment.CurrentDirectory, "Data", data.name);
      Topmost = true;
      _isLoaded = true;
      configHeights[1] = 0;
      configHeights[3] = 0;
      WindowUtilities.Clear();
      UpdateLayout();
      ImgView2.Source = null;
      ImgView4.Source = null;
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

    private GlobalHook _globalHook;
    private readonly Macro.Macro _macro = new Macro.Macro();
    private bool _isLoaded = false;
    private Config _config;
    private int progressValue;

    private readonly List<(ESendKey exe, int x, int y)> _executions = new List<(ESendKey, int, int)>();
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
        Marshal.ReleaseComObject(obj); // 액셀 객체 해제
        obj = null;
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
    private void MainWindow_OnClosing(object sender, CancelEventArgs e)
    {
      try
      {
        _workbook.AfterSave -= WorkbookOnAfterSave;
        _workbook.BeforeClose -= WorkbookOnBeforeClose;
        _workbook.Close(false);
        CloseExcel();
        Application.Current.Shutdown();
      }
      catch
      {
      }
    }

    private void Load()
    {
      try
      {
        var bw = new BackgroundWorker();
        bw.DoWork += (sender, args) => {
          // Thread.Sleep(300);

          CloseExcel();
          _isLoaded = false;
          Dispatcher.Invoke(() => {
            btnWait.IsEnabled = false;
            TbWait.Text = "엑셀 파일을 실행하는 중";
          });
          Console.WriteLine("Clearing Data...");

          // nudSpeed.Value = 0;
          _macro.interval = 0;
          Dispatcher.Invoke(() => { dgvDataView.Columns.Clear(); });
          _executions.Clear();
          _config = new Config();
          Console.WriteLine("Open Excel Files...");
          _excelApplication = new Excel.Application
          {
            Visible = true
          };
          _workbook = _excelApplication.Workbooks.Open(currentMacroData);
          _worksheet = (Excel.Worksheet)_workbook.Worksheets["macro"];
          _workbook.AfterSave += WorkbookOnAfterSave;
          _workbook.BeforeClose += WorkbookOnBeforeClose;
          var configSheet = _workbook.Sheets["config"] as Excel.Worksheet;
          var configRange = configSheet.Range["E3:F4"];
          for (var i = 1; i <= configRange.Rows.Count; i++)
            _config.Apply((configRange[i, 1] as Excel.Range)?.Value.ToString(), (configRange[i, 2] as Excel.Range)?.Value);

          configHeights[1] = _config.heightStep2;
          Dispatcher.Invoke(() => {
            Topmost = false;
            btnWait.IsEnabled = true;
            if (_config.heightStep2 > 0)
              ImgView2.Source = GetBitmapFromExcelRange("view", _config.viewRangeStep2);
            MovePage(1);
            TbWait.Text = "파일이 저장되기를 기다리는 중";
          });
          Console.WriteLine("Waiting to be saved...");
        };
        bw.RunWorkerAsync();
      }
      catch (Exception ex)
      {
        Console.WriteLine("Error: {0}", ex.Message);
      }
    }

    private void WorkbookOnBeforeClose(ref bool cancel)
    {
      cancel = false;
      if (!_isLoaded)
        Dispatcher.Invoke(() => { MovePage(1, 1); });
    }

    private void WorkbookOnAfterSave(bool success)
    {
      Dispatcher.Invoke(() => {
        if (success)
        {
          MovePage(2);
          LoadData();
          Topmost = true;
        }
      });
    }
    private void LoadData()
    {
      var bw = new BackgroundWorker();
      bw.DoWork += (sender, args) => {
        // Thread.Sleep(300);

        Console.WriteLine("Preparing Data Load...");
        loadProgressValue = 0;
        _macro.isStandBy = false;

        try
        {
          loadProgressMax = 3;
          loadProgressValue = 0;
          Dispatcher.Invoke(() => {
            PbLoading.Maximum = loadProgressMax;
            PbLoading.Value = loadProgressValue;
            TbLoading.Text = $"{Math.Floor((double)loadProgressValue / loadProgressMax * 100)}%";
          });
          Console.WriteLine("Load Config...");
          var configSheet = _workbook.Sheets["config"] as Excel.Worksheet;
          var configRange = configSheet.Range["B3:C6"];
          for (var i = 1; i <= configRange.Rows.Count; i++)
            _config.Apply((configRange[i, 1] as Excel.Range)?.Value.ToString(), (configRange[i, 2] as Excel.Range)?.Value);
          loadProgressMax += _config.maxCount;

          configHeights[3] = _config.heightStep4;
          loadProgressValue++;

          Console.WriteLine("Load View...");

          if (_config.heightStep4 > 0)
            Dispatcher.Invoke(() => { ImgView4.Source = GetBitmapFromExcelRange("view", _config.viewRangeStep4); });

          loadProgressValue++;

          var r = 0;
          Console.WriteLine("Load Columns...");
          while (true)
          {
            var columnHeader = _worksheet.Cells[2, 3 + r] as Excel.Range;
            if (columnHeader?.Value == null) break;

            Dispatcher.Invoke(() => { dgvDataView.Columns.Add($"a{r}", $"{columnHeader.Value2}"); });

            Console.WriteLine("Load Column: {0}", columnHeader.Value);
            r++;
          }

          loadProgressValue++;
          Console.WriteLine("Load Rows...");
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
            Dispatcher.Invoke(() => { dgvDataView.Rows.Add(); });

            for (var i = 0; i < ls.Count; i++)
            {
              if (ls[i] == null) continue;
              Dispatcher.Invoke(() => { dgvDataView[i, c].Value = ls[i]; });
              _executions.Add((new ESendKey(ls[i].ToString()), i, c));
              Console.WriteLine("Load Cell[{0}, {1}]: {2}", i, c, ls[i]);
            }

            c++;
            loadProgressValue++;
            Dispatcher.Invoke(() => {
              PbLoading.Maximum = loadProgressMax;
              PbLoading.Value = loadProgressValue;
              TbLoading.Text = $"{Math.Floor((double)loadProgressValue / loadProgressMax * 100)}%";
            });
          }
          _isLoaded = true;
          _workbook.Close(false);
          Dispatcher.Invoke(() => { MovePage(3); });
        }
        catch (Exception exc)
        {
          Console.WriteLine("Data load error: {0}", exc.Message);
        }
        finally
        {
          CloseExcel();
          _macro.isStandBy = true;
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
      if (!btnExecute.IsEnabled) return;

      if (_macro.isExecuting)
        _macro.Cancel();
      else
        _macro.Start(_executions.Select(x => x.exe).ToArray<IExecutable>(), _config.interval);
    }

    private void MacroOnRequestedCancellation(object sender, EventArgs e)
    {
      Dispatcher.Invoke(() => { btnExecute.IsEnabled = false; });
    }
    private void MacroOnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (e.PropertyName == nameof(_macro.isExecuting))
      {
        Dispatcher.Invoke(() => { btnExecute.Content = _macro.isExecuting ? "(중지 (F9" : "(실행 (F9"; });
      }

      Dispatcher.Invoke(() => { btnExecute.IsEnabled = _macro.isExecuting || _macro.isStandBy; });
    }
    private void MacroOnMacroStopped(object sender, Macro.Macro.MacroStoppedEventArgs e)
    {
      MessageBox.Show("매크로가 종료되었습니다.", "InputMacro", MessageBoxButton.OK, MessageBoxImage.Information);
      _macro.isStandBy = true;
    }

    private void MacroOnMacroStarted(object sender, Macro.Macro.MacroStartedEventArgs e)
    {
      Dispatcher.Invoke(() => {
        progressValue = 0;
        PbProgress.Value = progressValue;
        PbProgress.Maximum = _executions.Count;
        TbProgress.Text = $"{Math.Floor((double)progressValue / _executions.Count * 100)}%";
      });
    }

    private void MacroOnBeginExecute(object sender, Macro.Macro.ExecuteEventArgs e)
    {
      Dispatcher.Invoke(() => {
        dgvDataView.ClearSelection();
        dgvDataView[_executions[e.index].x, _executions[e.index].y].Selected = true;
        progressValue = e.index + 1;
        PbProgress.Value = progressValue;
        TbProgress.Text = $"{Math.Floor((double)progressValue / _executions.Count * 100)}%";
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
