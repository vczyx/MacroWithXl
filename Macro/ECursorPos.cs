using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using InputMacro.Macro;

namespace InputMacro3.Macro
{
  public class ECursorPos : IExecutable
  {
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern int SetCursorPos(int x, int y);
    
    public string identifier => "cursor";

    public void Execute()
    {
      SetCursorPos(position.x, position.y);
      Console.WriteLine($"Cursor pos: {position.x}, {position.y}");
    }

    public string value { get; }
    
    public (int x, int y) position { get; }

    public ECursorPos(string value)
    {
      this.value = value;
      if (!value.Contains(','))
        return;
      
      var split = value.Split(',').Select(x => Convert.ToInt32(x.Trim())).ToArray();

      position = (split[0], split[1]);
    }
  }
}
