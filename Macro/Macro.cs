using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InputMacro.Macro
{
  public enum LogPriorities
  {
    PopUpInfo = -2,
    PopUpWarning = -1,
    Normal = 0,
    Info = 1,
    Warning = 2,
    Error = 3
  }

  public class Macro
  {
    #region ############ Properties ###########

    public bool isExecuting {
      get => _isExecuting;
      private set {
        _isExecuting = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(isExecuting)));
        Log(value ? "Started" : "Stopped", LogPriorities.Normal);
      }
    }

    public bool isStandBy {
      get => _isStandBy;
      set {
        _isStandBy = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(isStandBy)));
        if (value) Log("Standby", LogPriorities.Normal);
      }
    }

    public int interval { get; set; }

    public IEnumerable<IExecutable> executions { get; set; }

    #endregion

    #region ############ Private Properties ##########

    private bool _isExecuting = false;
    private bool _isStandBy = false;

    private CancellationTokenSource _cancellationTokenSrc = new CancellationTokenSource();

    #endregion

    #region ############ EVENTS ###########

    public delegate void LogSubmittedEventHandler(object sender, LogSubmittedEventArgs e);

    public delegate void MacroStartedEventHandler(object sender, MacroStartedEventArgs e);

    public delegate void MacroStoppedEventHandler(object sender, MacroStoppedEventArgs e);

    public delegate void ExecuteEventHandler(object sender, ExecuteEventArgs e);

    public class LogSubmittedEventArgs : EventArgs
    {
      public string message { get; }

      public LogPriorities priority { get; }

      public LogSubmittedEventArgs(string message, LogPriorities priority)
      {
        this.message = message;
        this.priority = priority;
      }
    }

    public class MacroStartedEventArgs : EventArgs
    {
      public IEnumerable<IExecutable> executions { get; }

      public MacroStartedEventArgs(IEnumerable<IExecutable> executions)
      {
        this.executions = executions;
      }
    }

    public class MacroStoppedEventArgs : EventArgs
    {
      public IEnumerable<IExecutable> executions { get; }

      public bool isCancelled { get; }

      public bool isError { get; }

      public MacroStoppedEventArgs(IEnumerable<IExecutable> executions, bool isCancelled, bool isError = false)
      {
        this.executions = executions;
        this.isCancelled = isCancelled;
        this.isError = isError;
      }
    }

    public class ExecuteEventArgs : EventArgs
    {
      public IExecutable execution { get; }

      public int index { get; }

      public ExecuteEventArgs(IExecutable execution, int index)
      {
        this.execution = execution;
        this.index = index;
      }
    }

    public event LogSubmittedEventHandler LogSubmitted;

    public event MacroStartedEventHandler MacroStarted;

    public event MacroStoppedEventHandler MacroStopped;

    public event PropertyChangedEventHandler PropertyChanged;

    public event ExecuteEventHandler BeginExecute;

    public event ExecuteEventHandler EndExecute;

    public event EventHandler RequestedCancellation;

    #endregion

    #region ############ Public Methods ##########

    public void Start(IEnumerable<IExecutable> executions, int interval)
    {
      this.executions = executions;
      this.interval = interval;
      CallStart();
    }

    public void Cancel()
    {
      CallStop();
      RequestedCancellation?.Invoke(this, EventArgs.Empty);
      Log("Sent cancellation request", LogPriorities.Info);
    }

    #endregion

    #region ############ Private Methods ##########

    private void CallStart()
    {
      if (!isStandBy)
      {
        Log("Not yet in Standby", LogPriorities.PopUpWarning);
        return;
      }
      if (executions == null) return;

      isStandBy = false;
      isExecuting = true;

      _cancellationTokenSrc = new CancellationTokenSource();
      Task.Factory.StartNew(Execute, _cancellationTokenSrc.Token);
    }

    private async void Execute()
    {
      MacroStarted?.Invoke(this, new MacroStartedEventArgs(executions));
      try
      {
        var i = 0;

        foreach (var execution in executions)
        {
          try
          {
            BeginExecute?.Invoke(this, new ExecuteEventArgs(execution, i));
            Log($"[{i:000}. Execute] {execution}", LogPriorities.Info);
            execution.Execute();
            EndExecute?.Invoke(this, new ExecuteEventArgs(execution, i));
          }
          catch (Exception ex)
          {
            Log($"[{i:000}. Error] {execution}\n  Message: {ex.Message}", LogPriorities.Error);
          }
          finally
          {
            if (interval > 0)
              await Task.Delay(interval);
            i++;
            _cancellationTokenSrc.Token.ThrowIfCancellationRequested();
          }
        }
        isExecuting = false;
        MacroStopped?.Invoke(this, new MacroStoppedEventArgs(executions, false));
      }
      catch (OperationCanceledException e)
      {
        isExecuting = false;
        Log("Cancelled", LogPriorities.Info);
        MacroStopped?.Invoke(this, new MacroStoppedEventArgs(executions, true));
      }
      catch (Exception ex)
      {
        isExecuting = false;
        Log($"An error occurred while running and it was stopped.  Message: \n{ex.Message}", LogPriorities.PopUpWarning);
        MacroStopped?.Invoke(this, new MacroStoppedEventArgs(executions, false, true));
      }
    }

    private void CallStop()
    {
      if (_cancellationTokenSrc.IsCancellationRequested) return;
      _cancellationTokenSrc.Cancel();
    }

    private void Log(string message, LogPriorities priority = LogPriorities.Normal)
    {
      LogSubmitted?.Invoke(this, new LogSubmittedEventArgs(message, priority));
    }

    #endregion
  }
}
