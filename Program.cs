using System;
using System.Windows.Forms;

namespace AlwaysOnTop
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            const string appName = "AlwaysOnTop_Mutex";
            using (System.Threading.Mutex mutex = new System.Threading.Mutex(true, appName, out bool createdNew))
            {
                if (!createdNew)
                {
                    // App is already running, exit quietly.
                    return;
                }

                ApplicationConfiguration.Initialize();
                Application.Run(new MainForm());
            }
        }
    }
}
