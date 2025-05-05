using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using InputMacro.Macro;

namespace InputMacro3.Macro
{
  public class ECursor : IExecutable
  {
    public string identifier => "cursor";    
    
    public string description => "커서 위치를 x:<x>, y:<y>로 이동시킵니다.";

    public string arguments => "<x>,<y>";

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern int SetCursorPos(int x, int y);

    public async Task Execute()
    {
      SetCursorPos(position.x, position.y);
    }

    public string value { get; }
    
    public (int x, int y) position { get; }

    public ECursor(string value)
    {
      this.value = value;
      if (!value.Contains(','))
        return;
      
      var split = value.Split(',').Select(x => Convert.ToInt32(x.Trim())).ToArray();

      position = (split[0], split[1]);
    }
  }
}
