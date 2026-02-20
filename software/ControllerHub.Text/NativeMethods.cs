using System.Runtime.InteropServices;

namespace ControllerHub.Text;

internal static class NativeMethods
{
    private const uint KEYEVENTF_UNICODE = 0x0004;
    private const uint KEYEVENTF_KEYUP   = 0x0002;
    private const int  INPUT_KEYBOARD    = 1;

    [StructLayout(LayoutKind.Sequential)]
    private struct KEYBDINPUT
    {
        public ushort wVk;
        public ushort wScan;
        public uint   dwFlags;
        public uint   time;
        public nint   dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct INPUT
    {
        public int       type;
        public KEYBDINPUT ki;
    }

    [DllImport("user32.dll", SetLastError = true)]
    private static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

    public static void SendUnicodeChar(char ch)
    {
        var inputs = new INPUT[2];

        inputs[0].type       = INPUT_KEYBOARD;
        inputs[0].ki.wVk     = 0;
        inputs[0].ki.wScan   = (ushort)ch;
        inputs[0].ki.dwFlags = KEYEVENTF_UNICODE;

        inputs[1].type       = INPUT_KEYBOARD;
        inputs[1].ki.wVk     = 0;
        inputs[1].ki.wScan   = (ushort)ch;
        inputs[1].ki.dwFlags = KEYEVENTF_UNICODE | KEYEVENTF_KEYUP;

        SendInput(2, inputs, Marshal.SizeOf<INPUT>());
    }
}
