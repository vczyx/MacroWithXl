using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using InputMacro.Macro;

namespace InputMacro3.Macro
{
  public class ECursorBtn : IExecutable
  {
    private const uint LeftButtonDown = 0x00000002;
    private const uint LeftButtonUp = 0x00000004;
    private const uint MiddleButtonDown = 0x00000020;
    private const uint MiddleButtonUp = 0x00000040;
    private const uint RightButtonDown = 0x00000008;
    private const uint RightButtonUp = 0x00000010;
    
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, int dwExtraInfo);

    public enum ButtonType
    {
      LeftUp,
      LeftDown,
      Left,
      MiddleUp,
      MiddleDown,
      Middle,
      RightUp,
      RightDown,
      Right
    }

    public static readonly Dictionary<ButtonType, uint[]> MouseEventFlags = new Dictionary<ButtonType, uint[]>()
    {
      { ButtonType.Left , new [] {LeftButtonDown, LeftButtonUp}},
      { ButtonType.Middle , new [] {MiddleButtonDown, MiddleButtonUp}},
      { ButtonType.Right , new [] {RightButtonDown, RightButtonUp}},
      { ButtonType.LeftDown , new [] {LeftButtonDown}},
      { ButtonType.MiddleDown , new [] {MiddleButtonDown}},
      { ButtonType.RightDown , new [] {RightButtonDown}},
      { ButtonType.LeftUp , new [] {LeftButtonUp}},
      { ButtonType.MiddleUp , new [] {MiddleButtonUp}},
      { ButtonType.RightUp , new [] {RightButtonUp}},
      
    };

    public string identifier => "click";

    public void Execute()
    {
      var flags = MouseEventFlags[button];
      foreach (var flag in flags)
      {
        mouse_event(flag, 0, 0, 0, 0);
      }
    }

    public string value { get; }
    
    public ButtonType button { get; }

    public ECursorBtn(string value)
    {
      this.value = value;
      if (Enum.TryParse<ButtonType>(value, true, out var btn))
      {
        button = btn;
      }
    }
  }
}
