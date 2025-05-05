using System.Threading.Tasks;

namespace InputMacro.Macro
{
  public interface IExecutable
  {
    string identifier { get; }
    string description { get; }
    string arguments { get; }

    Task Execute();
    string value { get; }
  }
}
