using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace InputMacro3.EasingFunction
{
  public class EaseOutExpo : EasingFunctionBase
  {
    public EaseOutExpo()
      : base()
    {
    }

    protected override double EaseInCore(double normalizedTime)
    {
      return 1 - Math.Pow(2, -10 * normalizedTime);
    }

    protected override Freezable CreateInstanceCore()
    {
      return new EaseOutExpo();
    }
  }
}
