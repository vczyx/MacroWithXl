using System.Windows.Forms;

namespace InputMacro.Macro
{
  public class ESendKey : IExecutable
  {
    public string value { get; set; }

    public ESendKey(string value)
    {
      this.value = value;
    }

    public void Execute()
    {
      SendKeys.SendWait(value);
    }

    public override string ToString() => $"SendKey: {value}";
  }
}
