using System;
using System.Drawing;
using System.Windows.Forms;

namespace AlwaysOnTop
{
    public class TrayManager : IDisposable
    {
        private readonly NotifyIcon _notifyIcon;
        private readonly ContextMenuStrip _contextMenu;
        private readonly Action _onToggle;
        private readonly Action _onSettings;
        private readonly Action _onExit;

        public TrayManager(Action onToggle, Action onSettings, Action onExit)
        {
            _onToggle = onToggle;
            _onSettings = onSettings;
            _onExit = onExit;

            _contextMenu = new ContextMenuStrip();
            _contextMenu.Items.Add("Toggle Current Window", null, (s, e) => _onToggle());
            _contextMenu.Items.Add("Settings", null, (s, e) => _onSettings());
            _contextMenu.Items.Add("-");
            _contextMenu.Items.Add("Exit", null, (s, e) => _onExit());

            _notifyIcon = new NotifyIcon
            {
                Icon = GetAppIcon(),
                ContextMenuStrip = _contextMenu,
                Visible = true,
                Text = "Always On Top"
            };

            _notifyIcon.DoubleClick += (s, e) => _onSettings();
        }

        public void ShowNotification(string title, string text)
        {
            _notifyIcon.ShowBalloonTip(3000, title, text, ToolTipIcon.Info);
        }

        public void Dispose()
        {
            _notifyIcon.Dispose();
            _contextMenu.Dispose();
        }

        private Icon GetAppIcon()
        {
            try
            {
                if (System.IO.File.Exists("icon.png"))
                {
                    using (Bitmap bmp = new Bitmap("icon.png"))
                    {
                        IntPtr hIcon = bmp.GetHicon();
                        return Icon.FromHandle(hIcon);
                    }
                }
            }
            catch { }
            return SystemIcons.Application;
        }
    }
}
