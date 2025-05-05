using System.Threading.Tasks;
using System.Windows;
using InputMacro.Macro;

namespace InputMacro3.Macro.Util
{
  public class ELog : IExecutable
  {
    public string identifier => "util.log";
        
    public string description => "(util) <message>를 출력합니다.";

    public string arguments => "<message>";

    public async Task Execute()
    {
      MessageBox.Show(value, "Log");
    }

    public string value { get; }

    public ELog(string value)
    {
      this.value = value;
    }
  }
}
