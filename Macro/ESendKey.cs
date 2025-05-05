using System.Threading.Tasks;
using System.Windows.Forms;
using InputMacro.Macro;

namespace InputMacro3.Macro
{
  public class ESendKey : IExecutable
  {
    public string identifier => "key";    
    
    public string description => "<keys>에 입력된 여러 키 값들을 차례대로 입력시킵니다.";

    public string arguments => "<keys>";

    public async Task Execute()
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
