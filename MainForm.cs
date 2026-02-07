using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace AlwaysOnTop
{
    public partial class MainForm : Form
    {
        private TrayManager _trayManager;
        private HotkeyManager _hotkeyManager;
        private AppSettings _settings;
        private bool _isRecordingHotkey = false;



        public MainForm()
        {
            InitializeComponent();
            _settings = SettingsManager.Load();
            SetupUI();
            SetAppIcon();

            _trayManager = new TrayManager(ToggleTopMost, ShowSettings, ExitApp);
            _hotkeyManager = new HotkeyManager(this.Handle, 1);
            
            RegisterHotkey();
        }

        private void SetupUI()
        {
            this.BackColor = Color.FromArgb(25, 25, 25);
            this.ForeColor = Color.White;
            this.Font = new Font("Segoe UI", 10);
            this.Text = "";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            chkStartup.Checked = _settings.StartWithWindows;
            UpdateHotkeyLabel();
        }



        private void UpdateHotkeyLabel()
        {
            lblHotkey.Text = $"Current Hotkey: {GetHotkeyString()}";
        }

        private void SetAppIcon()
        {
            try
            {
                if (System.IO.File.Exists("icon.png"))
                {
                    using (Bitmap bmp = new Bitmap("icon.png"))
                    {
                        IntPtr hIcon = bmp.GetHicon();
                        this.Icon = Icon.FromHandle(hIcon);
                    }
                }
            }
            catch { }
        }

        private string GetHotkeyString()
        {
            string mods = "";
            if ((_settings.Modifiers & KeyModifiers.Control) != 0) mods += "Ctrl + ";
            if ((_settings.Modifiers & KeyModifiers.Alt) != 0) mods += "Alt + ";
            if ((_settings.Modifiers & KeyModifiers.Shift) != 0) mods += "Shift + ";
            if ((_settings.Modifiers & KeyModifiers.Windows) != 0) mods += "Win + ";
            return mods + _settings.Hotkey.ToString();
        }

        private void RegisterHotkey()
        {
            _hotkeyManager.Unregister();
            bool success = _hotkeyManager.Register(_settings.Hotkey, _settings.Modifiers);
            if (!success)
            {
                _trayManager.ShowNotification("Error", "Failed to register hotkey. It might be in use.");
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == HotkeyManager.WM_HOTKEY)
            {
                ToggleTopMost();
            }
            base.WndProc(ref m);
        }

        private void ToggleTopMost()
        {
            IntPtr hWnd = WindowManager.GetForegroundWindow();
            if (hWnd == this.Handle) return; // Don't pin the settings window itself

            bool success = WindowManager.ToggleAlwaysOnTop(hWnd);
            if (success)
            {
                bool isTop = WindowManager.IsWindowTopMost(hWnd);
                string title = WindowManager.GetWindowTitle(hWnd);
                _trayManager.ShowNotification(isTop ? "Window Pinned" : "Window Unpinned", title);
            }
        }

        private void ShowSettings()
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.BringToFront();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_isRecordingHotkey) CancelRecording();
            _settings.StartWithWindows = chkStartup.Checked;
            SettingsManager.Save(_settings);
            this.Hide();
        }

        private void btnChangeHotkey_Click(object sender, EventArgs e)
        {
            if (_isRecordingHotkey)
            {
                CancelRecording();
            }
            else
            {
                StartRecording();
            }
        }

        private void StartRecording()
        {
            _isRecordingHotkey = true;
            btnChangeHotkey.Text = "Recording...";
            btnChangeHotkey.BackColor = Color.DarkRed;
            lblHotkey.Text = "Press any key combination...";
            this.KeyPreview = true;
        }

        private void CancelRecording()
        {
            _isRecordingHotkey = false;
            btnChangeHotkey.Text = "Change Hotkey";
            btnChangeHotkey.BackColor = Color.FromArgb(60, 60, 60);
            UpdateHotkeyLabel();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (_isRecordingHotkey)
            {
                // Determine modifiers
                KeyModifiers mods = KeyModifiers.None;
                if (e.Control) mods |= KeyModifiers.Control;
                if (e.Alt) mods |= KeyModifiers.Alt;
                if (e.Shift) mods |= KeyModifiers.Shift;
                // Win key is harder to catch here, usually we stick to Ctrl/Alt/Shift

                // Extract the base key
                Keys key = e.KeyCode;

                // Stop recording if it's NOT just a modifier key
                if (key != Keys.ControlKey && key != Keys.ShiftKey && key != Keys.Menu && key != Keys.LWin && key != Keys.RWin)
                {
                    _settings.Hotkey = key;
                    _settings.Modifiers = mods;
                    RegisterHotkey();
                    CancelRecording();
                    e.Handled = true;
                }
            }
            base.OnKeyDown(e);
        }

        private void ExitApp()
        {
            _trayManager.Dispose();
            _hotkeyManager.Dispose();
            Application.Exit();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
            base.OnFormClosing(e);
        }
    }
}
