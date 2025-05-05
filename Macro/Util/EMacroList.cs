using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using InputMacro.Macro;

namespace InputMacro3.Macro.Util
{
  public class EMacroList : IExecutable
  {
    public string identifier => "util.mcrlist";   
    
    public string description => "(util) 모든 매크로 실행 명령 식별자에 대한 설명을 출력합니다.";

    public string arguments => "";

    public async Task Execute()
    {
      var dict = ExecutableExtensions.emptyInstanceExecutableDictionary;
      var str = string.Concat(dict.Values.Select(x => $"`${x.identifier}:{x.arguments}` : {x.description}\n"));
      await ExecutableExtensions.Execute($"$util.log:{str}");
    }

    public string value { get; }

    public EMacroList(string value)
    {
      this.value = value;
    }
  }
}
