using System.Windows.Forms;
using InputMacro.Macro;

namespace InputMacro3.Macro
{
  public class ESendKey : IExecutable
  {
    public string identifier => "";

    public void Execute()
    {
      SendKeys.SendWait(value);
    }

    public string value { get; }

    public ESendKey(string value)
    {
      this.value = value;
    }
  }
}
