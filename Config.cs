using System;

namespace InputMacro3
{
  public class Config
  {
    public int interval { get; set; }
    public int maxCount { get; set; }
    public string viewRangeStep2 { get; set; }
    public string viewRangeStep4 { get; set; }
    public int heightStep2 { get; set; }
    public int heightStep4 { get; set; }

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
        case nameof(heightStep2):
          heightStep2 = Convert.ToInt32(value);
          break;
        case nameof(heightStep4):
          heightStep4 = Convert.ToInt32(value);
          break;
        case nameof(viewRangeStep2):
          viewRangeStep2 = (string)value;
          break;
        case nameof(viewRangeStep4):
          viewRangeStep4 = (string)value;
          break;
      }
    }
  }
}
