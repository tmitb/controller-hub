using System;
using System.Runtime.InteropServices;

namespace ControllerHub.Keyboard;

internal static class NativeMethods
{
    private const uint INPUT_KEYBOARD = 1;
    private const uint KEYEVENTF_KEYUP = 0x0002;

    private const ushort VK_SHIFT   = 0x10;
    private const ushort VK_CONTROL = 0x11;
    private const ushort VK_MENU    = 0x12; // Alt

    [StructLayout(LayoutKind.Sequential)]
    private struct KEYBDINPUT
    {
        public ushort wVk;
        public ushort wScan;
        public uint   dwFlags;
        public uint   time;
        public IntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct INPUT
    {
        public uint type;
        public KEYBDINPUT ki;
    }

    [DllImport("user32.dll", SetLastError = true)]
    private static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

    public static void SendKeyCombo(ushort vk, bool ctrl, bool shift, bool alt)
    {
        // Build the press sequence: modifiers down, key down, key up, modifiers up.
        var inputs = new System.Collections.Generic.List<INPUT>();

        void AddKey(ushort code, bool keyUp)
        {
            inputs.Add(new INPUT
            {
                type = INPUT_KEYBOARD,
                ki = new KEYBDINPUT
                {
                    wVk     = code,
                    dwFlags = keyUp ? KEYEVENTF_KEYUP : 0,
                }
            });
        }

        // Press modifiers
        if (ctrl)  AddKey(VK_CONTROL, keyUp: false);
        if (shift) AddKey(VK_SHIFT,   keyUp: false);
        if (alt)   AddKey(VK_MENU,    keyUp: false);

        // Press + release target key
        AddKey(vk, keyUp: false);
        AddKey(vk, keyUp: true);

        // Release modifiers (reverse order)
        if (alt)   AddKey(VK_MENU,    keyUp: true);
        if (shift) AddKey(VK_SHIFT,   keyUp: true);
        if (ctrl)  AddKey(VK_CONTROL, keyUp: true);

        var arr = inputs.ToArray();
        SendInput((uint)arr.Length, arr, Marshal.SizeOf<INPUT>());
    }
}
