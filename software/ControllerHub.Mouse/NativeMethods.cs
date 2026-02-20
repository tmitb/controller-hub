using System.Runtime.InteropServices;

namespace ControllerHub.Mouse;

internal static class NativeMethods
{
    private const uint INPUT_MOUSE = 0;

    private const uint MOUSEEVENTF_MOVE     = 0x0001;
    private const uint MOUSEEVENTF_LEFTDOWN  = 0x0002;
    private const uint MOUSEEVENTF_LEFTUP    = 0x0004;
    private const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
    private const uint MOUSEEVENTF_RIGHTUP   = 0x0010;
    private const uint MOUSEEVENTF_MIDDLEDOWN = 0x0020;
    private const uint MOUSEEVENTF_MIDDLEUP  = 0x0040;
    private const uint MOUSEEVENTF_ABSOLUTE  = 0x8000;

    private const int SM_CXSCREEN = 0;
    private const int SM_CYSCREEN = 1;

    [StructLayout(LayoutKind.Sequential)]
    private struct MOUSEINPUT
    {
        public int   dx;
        public int   dy;
        public uint  mouseData;
        public uint  dwFlags;
        public uint  time;
        public IntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Explicit)]
    private struct INPUT
    {
        [FieldOffset(0)] public uint       type;
        [FieldOffset(4)] public MOUSEINPUT mi;
    }

    [DllImport("user32.dll", SetLastError = true)]
    private static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

    [DllImport("user32.dll")]
    private static extern int GetSystemMetrics(int nIndex);

    public static void MoveTo(int pixelX, int pixelY)
    {
        int screenW = GetSystemMetrics(SM_CXSCREEN);
        int screenH = GetSystemMetrics(SM_CYSCREEN);

        int nx = (int)((long)pixelX * 65535 / (screenW - 1));
        int ny = (int)((long)pixelY * 65535 / (screenH - 1));

        var input = new INPUT
        {
            type = INPUT_MOUSE,
            mi = new MOUSEINPUT
            {
                dx      = nx,
                dy      = ny,
                dwFlags = MOUSEEVENTF_MOVE | MOUSEEVENTF_ABSOLUTE,
            }
        };

        SendInput(1, [input], Marshal.SizeOf<INPUT>());
    }

    public static void ButtonEvent(string button, bool down)
    {
        uint flags = button.ToLowerInvariant() switch
        {
            "left"   => down ? MOUSEEVENTF_LEFTDOWN   : MOUSEEVENTF_LEFTUP,
            "right"  => down ? MOUSEEVENTF_RIGHTDOWN  : MOUSEEVENTF_RIGHTUP,
            "middle" => down ? MOUSEEVENTF_MIDDLEDOWN : MOUSEEVENTF_MIDDLEUP,
            _        => 0,
        };

        if (flags == 0) return;

        var input = new INPUT
        {
            type = INPUT_MOUSE,
            mi = new MOUSEINPUT { dwFlags = flags }
        };

        SendInput(1, [input], Marshal.SizeOf<INPUT>());
    }

    public static void Click(string button)
    {
        ButtonEvent(button, down: true);
        ButtonEvent(button, down: false);
    }
}
