using System;

namespace InputMacro3.Data
{
  [Serializable]
  public class Config
  {
    [Serializable]
    public class ViewPage
    {
      public double width { get; set; } = 0;

      public double height { get; set; } = 0;

      public string descriptions { get; set; } = "";

      public string imageRange { get; set; } = "";
    }
    
    public int macroInterval { get; set; } = 0;
    public int macroCount { get; set; } = 0;
    public ViewPage[] viewPages { get; set; } = new[] { new ViewPage(), new ViewPage() };

    public bool closeAfterLoad { get; set; } = true;
  }
}
