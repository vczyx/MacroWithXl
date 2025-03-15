using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace InputMacro3
{
  public static class ClipboardHelper
  {
    // ClipboardHelper.GetImageSource()
    // public static ImageSource GetImageSource()
    // {
    //   if (!Clipboard.ContainsImage())
    //     return null;
    //
    //   // no try to get a Bitmap and then convert to BitmapSource
    //   using (var bmp = GetImage())
    //   {
    //     if (bmp == null)
    //       return null;
    //
    //     return WindowUtilities.BitmapToBitmapSource(bmp);
    //   }
    // }

// ClipboardHelper.GetImage()
    // public static Bitmap GetImage()
    // {
    //   try
    //   {
    //     var dataObject = Clipboard.GetDataObject();
    //
    //     var formats = dataObject.GetFormats(true);
    //     if (formats == null || formats.Length == 0)
    //       return null;
    //
    //     #if DEBUG // show all formats of the image pasted
    //     foreach (var f in formats)
    //       Debug.WriteLine(" - " + f.ToString());
    //     #endif
    //
    //     // Use this first as this gives you transparency!
    //     if (formats.Contains("PNG"))
    //     {
    //       using (MemoryStream ms = (MemoryStream)dataObject.GetData("PNG"))
    //       {
    //         ms.Position = 0;
    //         return (Bitmap)new Bitmap(ms);
    //       }
    //     }
    //     if (formats.Contains("System.Drawing.Bitmap"))
    //     {
    //       return (Bitmap)dataObject.GetData("System.Drawing.Bitmap");
    //     }
    //     if (formats.Contains(DataFormats.Bitmap))
    //     {
    //       return (Bitmap)dataObject.GetData(DataFormats.Bitmap);
    //     }
    //
    //     // just use GetImage() - 
    //     // retry multiple times to work around Windows timing
    //     BitmapSource src = null;
    //     for (int i = 0; i < 5; i++)
    //     {
    //       try
    //       {
    //         // This is notoriously unreliable so retry multiple time if it fails
    //         src = Clipboard.GetImage();
    //         break; // success
    //       }
    //       catch
    //       {
    //         Thread.Sleep(10); // retry
    //       }
    //     }
    //
    //     if (src == null)
    //     {
    //       try
    //       {
    //         Debug.WriteLine("Clipboard Fall through - use WinForms");
    //         return System.Windows.Forms.Clipboard.GetImage() as Bitmap;
    //       }
    //       catch
    //       {
    //         return null;
    //       }
    //     }
    //
    //     return WindowUtilities.BitmapSourceToBitmap(src);
    //   }
    //   catch
    //   {
    //     return null;
    //   }
    // }
  }
}
