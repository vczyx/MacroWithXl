using System;

namespace InputMacro3
{
  public class Config
  {
    public int interval { get; set; }
    public int maxCount { get; set; }
    public string viewRange { get; set; }

    public void Apply(string name, object value)
    {
      switch (name)
      {
        case nameof(interval):
          interval = Convert.ToInt32(value);
          break;
        case nameof(maxCount):
          maxCount = Convert.ToInt32(value);
          break;
        case nameof(viewRange):
          viewRange = (string)value;
          break;
      }
    }
  }
}
