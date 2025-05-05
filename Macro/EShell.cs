using System.Diagnostics;
using System.Threading.Tasks;
using InputMacro.Macro;

namespace InputMacro3.Macro
{
  public class EShell : IExecutable
  {
    public string identifier => "sh";
    
    public string description => "cmd를 백그라운드에서 실행시켜, <command> 명령을 실행합니다.";

    public string arguments => "<command>";
    public async Task Execute()
    {
      Process.Start(new ProcessStartInfo("cmd", $"/c {value}") { CreateNoWindow = true, WindowStyle = ProcessWindowStyle.Hidden});
    }

    public string value { get; }

    public EShell(string value)
    {
      this.value = value;
    }
  }
}
