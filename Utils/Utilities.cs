﻿using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace InputMacro3.Utils
{
  public static class Utilities
  {
    public static ImageSource ToImageSource(this Icon icon)
    {
      ImageSource imageSource = Imaging.CreateBitmapSourceFromHIcon(
        icon.Handle,
        Int32Rect.Empty,
        BitmapSizeOptions.FromEmptyOptions());

      return imageSource;
    }
    
  }
}
