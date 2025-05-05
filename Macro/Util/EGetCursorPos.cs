using System.Threading.Tasks;
using System.Windows.Forms;
using InputMacro.Macro;

namespace InputMacro3.Macro.Util
{
  public class EGetCursorPos : IExecutable
  {
    public string identifier => "util.gcursor";    
    
    public string description => "(util) 현재 커서 위치를 출력합니다.";

    public string arguments => "";

    public async Task Execute()
    {
      var x = Cursor.Position.X;
      var y = Cursor.Position.Y;
      await ExecutableExtensions.Execute($"$util.log:({x}, {y})");
    }

    public string value { get; }

    public EGetCursorPos(string value)
    {
      this.value = value;
    }
  }
}
