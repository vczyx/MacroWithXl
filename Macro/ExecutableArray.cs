using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InputMacro.Macro
{
  [Obsolete("Macro 클래스에서 지원되지 않습니다.",true)]
  public class ExecutableArray<T> : IExecutable where T : IExecutable
  {
    public IEnumerable<T> values { get; }

    public int interval { get; set; }

    public ExecutableArray(IEnumerable<T> values, int interval)
    {
      this.values = values;
    }

    public async void Execute()
    {
      foreach (var executable in values)
      {
        executable.Execute();
        if (interval > 0) await Task.Delay(interval);
      }
    }
  }

  public class ExecutableArray : IExecutable
  {
    public IEnumerable<IExecutable> values { get; }

    public ExecutableArray(IEnumerable<IExecutable> values)
    {
      this.values = values;
    }

    public void Execute()
    {
      foreach (var executable in values)
        executable.Execute();
    }
  }
}
