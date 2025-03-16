using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace InputMacro3.Utils
{
  public static class WindowUtilities
  {
    public static List<BitmapSource> images = new List<BitmapSource>();

    public static void Clear()
    {
      for (var i = 0; i < images.Count; i++)
      {
        images[i] = null;
      }
      images.Clear();
    }
    
    public static BitmapSource BitmapToBitmapSource(Bitmap bmp)
    {
      var bitmapData = bmp.LockBits(
        new Rectangle(0, 0, bmp.Width, bmp.Height),
        ImageLockMode.ReadOnly, bmp.PixelFormat);

      var bitmapSource = BitmapSource.Create(
        bitmapData.Width, bitmapData.Height,
        bmp.HorizontalResolution, bmp.VerticalResolution,
        PixelFormats.Bgr24, null,
        bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride);

      bmp.UnlockBits(bitmapData);

      images.Add(bitmapSource);
      return bitmapSource;
    }
    public static System.Windows.Media.PixelFormat ConvertPixelFormat(System.Drawing.Imaging.PixelFormat systemDrawingFormat)
    {
      switch (systemDrawingFormat)
      {
        case PixelFormat.Format32bppArgb:
          return PixelFormats.Bgra32;
        case PixelFormat.Format32bppRgb:
          return PixelFormats.Bgr32;
        case PixelFormat.Format24bppRgb:
          return PixelFormats.Bgr24;
        case PixelFormat.Format16bppRgb565:
          return PixelFormats.Bgr565;
        case PixelFormat.Format16bppArgb1555:
          return PixelFormats.Bgr555;
        case PixelFormat.Format8bppIndexed:
          return PixelFormats.Gray8;
        case PixelFormat.Format1bppIndexed:
          return PixelFormats.BlackWhite;
        case PixelFormat.Format16bppGrayScale:
          return PixelFormats.Gray16;
        default:
          return PixelFormats.Bgr24;
      }
    }

    public static System.Drawing.Imaging.PixelFormat ConvertPixelFormat(System.Windows.Media.PixelFormat wpfFormat)
    {
      if (wpfFormat == PixelFormats.Bgra32)
        return PixelFormat.Format32bppArgb;
      if (wpfFormat == PixelFormats.Bgr32)
        return PixelFormat.Format32bppRgb;
      if (wpfFormat == PixelFormats.Bgr24)
        return PixelFormat.Format24bppRgb;
      if (wpfFormat == PixelFormats.Bgr565)
        return PixelFormat.Format16bppRgb565;
      if (wpfFormat == PixelFormats.Bgr555)
        return PixelFormat.Format16bppArgb1555;
      if (wpfFormat == PixelFormats.Gray8)
        return PixelFormat.Format8bppIndexed;
      if (wpfFormat == PixelFormats.Gray16)
        return PixelFormat.Format16bppGrayScale;
      if (wpfFormat == PixelFormats.BlackWhite)
        return PixelFormat.Format1bppIndexed;

      return PixelFormat.Format24bppRgb;
    }

    public static BitmapSource BitmapToBitmapSourceSafe(Bitmap bmp)
    {
      using (var ms = new MemoryStream())
      {
        bmp.Save(ms, ImageFormat.Png);
        ms.Position = 0;

        var bitmap = new BitmapImage();
        bitmap.BeginInit();
        bitmap.CacheOption = BitmapCacheOption.OnLoad; // Load image immediately
        bitmap.StreamSource = ms;
        bitmap.EndInit();
        bitmap.Freeze(); // Make the BitmapImage cross-thread accessible
        images.Add(bitmap);
        return bitmap;
      }
    }
  }
}
