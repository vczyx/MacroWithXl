using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InputMacro3.Macro
{
  public class ESendInput
  {
    [StructLayout(LayoutKind.Sequential)]
    public struct KeyboardInput
    {
      public ushort wVk;
      public ushort wScan;
      public uint dwFlags;
      public uint time;
      public IntPtr dwExtraInfo;
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public struct MouseInput
    {
      public int dx;
      public int dy;
      public uint mouseData;
      public uint dwFlags;
      public uint time;
      public IntPtr dwExtraInfo;
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public struct HardwareInput
    {
      public uint uMsg;
      public ushort wParamL;
      public ushort wParamH;
    }
    
    [StructLayout(LayoutKind.Explicit)]
    public struct InputUnion
    {
      [FieldOffset(0)] public MouseInput mi;
      [FieldOffset(0)] public KeyboardInput ki;
      [FieldOffset(0)] public HardwareInput hi;
    }  
    
    public struct Input
    {
      public int type;
      public InputUnion u;
    }
    
    [Flags]
    public enum InputType
    {
      Mouse = 0,
      Keyboard = 1,
      Hardware = 2
    }
    
    [Flags]
    public enum KeyEventF
    {
      KeyDown = 0x0000,
      ExtendedKey = 0x0001,
      KeyUp = 0x0002,
      Unicode = 0x0004,
      Scancode = 0x0008
    }
    
    [Flags]
    public enum MouseEventF
    {
      Absolute = 0x8000,
      HWheel = 0x01000,
      Move = 0x0001,
      MoveNoCoalesce = 0x2000,
      LeftDown = 0x0002,
      LeftUp = 0x0004,
      RightDown = 0x0008,
      RightUp = 0x0010,
      MiddleDown = 0x0020,
      MiddleUp = 0x0040,
      VirtualDesk = 0x4000,
      Wheel = 0x0800,
      XDown = 0x0080,
      XUp = 0x0100
    }
    
    [DllImport("user32.dll", SetLastError = true)]
    private static extern uint SendInput(uint nInputs, Input[] pInputs, int cbSize);
    
    [DllImport("user32.dll")]
    private static extern IntPtr GetMessageExtraInfo();
    
    public string identifier => "input";    
    
    public string description => "<keys>에 입력된 여러 키 값들을 차례대로 입력시킵니다.";

    public string arguments => "<keys>";

    public async Task Execute()
    {
      SendKeys.SendWait(value);
    }

    public string value { get; }

    public ESendInput(string value)
    {
      this.value = value;
    }
  }
}
