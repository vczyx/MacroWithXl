using System;
using System.Threading.Tasks;
using InputMacro.Macro;

namespace InputMacro3.Macro
{
  public class EWait : IExecutable
  {
    public string identifier => "wait";

    public string description => "<milisec> 밀리초 동안 작업을 멈춥니다.";

    public string arguments => "<milisec>";

    public async Task Execute()
    {
      await Task.Delay(milisec);
    }

    public string value { get; }

    public int milisec { get; } = 0;

    public EWait(string value)
    {
      this.value = value;
      if (value.Length > 0)
        milisec = Convert.ToInt32(value);
    }
  }
}
