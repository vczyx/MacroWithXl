using System;
using System.Threading.Tasks;
using System.Windows;
using InputMacro.Macro;
using InputMacro3.Utils;

namespace InputMacro3.Macro.Clipboard
{
  public class ESet : IExecutable
  {
    public string identifier => "clipboard.set";    
    
    public string description => "<text>를 클립보드에 저장합니다.";

    public string arguments => "<text>";

    public async Task Execute()
    {
      await STATask.Run(() => System.Windows.Clipboard.SetText(value));
      Console.WriteLine(value);
    }

    public string value { get; }

    public ESet(string value)
    {
      this.value = value;
    }
  }
}
