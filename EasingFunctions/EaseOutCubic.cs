using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace InputMacro3.EasingFunction
{
  public class EaseOutCubic : EasingFunctionBase
  {
    public EaseOutCubic()
      : base()
    {
    }

    protected override double EaseInCore(double normalizedTime)
    {
      return 1 - Math.Pow(1 - normalizedTime, 3);
    }

    protected override Freezable CreateInstanceCore()
    {
      return new EaseOutCubic();
    }
  }

}
