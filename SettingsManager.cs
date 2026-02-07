using System;
using System.IO;
using System.Text.Json;
using Microsoft.Win32;
using System.Windows.Forms;

namespace AlwaysOnTop
{
    public class AppSettings
    {
        public Keys Hotkey { get; set; } = Keys.Space;
        public KeyModifiers Modifiers { get; set; } = KeyModifiers.Control;
        public bool StartWithWindows { get; set; } = false;
    }

    public static class SettingsManager
    {
        private static readonly string SettingsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AlwaysOnTop", "settings.json");

        public static AppSettings Load()
        {
            try
            {
                if (File.Exists(SettingsPath))
                {
                    string json = File.ReadAllText(SettingsPath);
                    return JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
                }
            }
            catch { }
            return new AppSettings();
        }

        public static void Save(AppSettings settings)
        {
            try
            {
                string dir = Path.GetDirectoryName(SettingsPath)!;
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

                string json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(SettingsPath, json);

                SetStartup(settings.StartWithWindows);
            }
            catch { }
        }

        private static void SetStartup(bool start)
        {
            try
            {
                using (RegistryKey? key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true))
                {
                    if (key != null)
                    {
                        if (start)
                            key.SetValue("AlwaysOnTop", Application.ExecutablePath);
                        else
                            key.DeleteValue("AlwaysOnTop", false);
                    }
                }
            }
            catch { }
        }
    }
}
