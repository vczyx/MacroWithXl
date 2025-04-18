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

    [Serializable]
    public class Macro
    {
      public int interval { get; set; } = 0;

      public int maxCount { get; set; } = 0;
    }

    public Macro macro { get; set; } = new Macro();
    public ViewPage[] viewPages { get; set; } = new[] { new ViewPage(), new ViewPage() };

    public bool closeAfterLoad { get; set; } = true;
  }
}
