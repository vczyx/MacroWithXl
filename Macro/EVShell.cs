using System.Diagnostics;
using System.Threading.Tasks;
using InputMacro.Macro;

namespace InputMacro3.Macro
{
  public class EVShell : IExecutable
  {
    public string identifier => "vsh";
    
    public string description => "cmd를 실행시켜, <command> 명령을 실행합니다.";

    public string arguments => "<command>";

    public async Task Execute()
    {
      Process.Start(new ProcessStartInfo("cmd",  $"/c {value}"));
    }

    public string value { get; }

    public EVShell(string value)
    {
      this.value = value;
    }
  }
}
