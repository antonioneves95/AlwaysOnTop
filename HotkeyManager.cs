using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AlwaysOnTop
{
    public class HotkeyManager : IDisposable
    {
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public const int WM_HOTKEY = 0x0312;

        private readonly IntPtr _windowHandle;
        private readonly int _hotkeyId;

        public HotkeyManager(IntPtr windowHandle, int hotkeyId)
        {
            _windowHandle = windowHandle;
            _hotkeyId = hotkeyId;
        }

        public bool Register(Keys key, KeyModifiers modifiers)
        {
            return RegisterHotKey(_windowHandle, _hotkeyId, (uint)modifiers, (uint)key);
        }

        public void Unregister()
        {
            UnregisterHotKey(_windowHandle, _hotkeyId);
        }

        public void Dispose()
        {
            Unregister();
        }
    }

    [Flags]
    public enum KeyModifiers
    {
        None = 0,
        Alt = 1,
        Control = 2,
        Shift = 4,
        Windows = 8
    }
}
