namespace InputMacro.Macro
{
  public interface IExecutable
  {
    string identifier { get; }
    void Execute();
    string value { get; }
  }
}
