namespace AlwaysOnTop
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private CheckBox chkStartup;
        private Label lblHotkey;
        private Button btnSave;
        private Label lblTitle;
        private Button btnChangeHotkey;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.chkStartup = new CheckBox();
            this.lblHotkey = new Label();
            this.btnSave = new Button();
            this.lblTitle = new Label();
            this.btnChangeHotkey = new Button();
            this.SuspendLayout();
            
            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            this.lblTitle.Location = new Point(20, 20);
            this.lblTitle.Text = "Always On Top";

            // lblHotkey
            this.lblHotkey.AutoSize = true;
            this.lblHotkey.Location = new Point(20, 70);
            this.lblHotkey.Text = "Current Hotkey: Ctrl + Space";

            // btnChangeHotkey
            this.btnChangeHotkey.Location = new Point(20, 100);
            this.btnChangeHotkey.Size = new Size(120, 30);
            this.btnChangeHotkey.Text = "Change Hotkey";
            this.btnChangeHotkey.FlatStyle = FlatStyle.Flat;
            this.btnChangeHotkey.BackColor = Color.FromArgb(60, 60, 60);
            this.btnChangeHotkey.FlatAppearance.BorderSize = 0;
            this.btnChangeHotkey.Click += new EventHandler(this.btnChangeHotkey_Click);

            // chkStartup
            this.chkStartup.AutoSize = true;
            this.chkStartup.Location = new Point(20, 150);
            this.chkStartup.Text = "Start with Windows";
            this.chkStartup.FlatStyle = FlatStyle.Flat;

            // btnSave
            this.btnSave.Location = new Point(20, 200);
            this.btnSave.Size = new Size(120, 35);
            this.btnSave.Text = "Close To Tray";
            this.btnSave.FlatStyle = FlatStyle.Flat;
            this.btnSave.BackColor = Color.FromArgb(50, 50, 50);
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.Click += new EventHandler(this.btnSave_Click);

            // MainForm
            this.ClientSize = new Size(350, 260);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblHotkey);
            this.Controls.Add(this.btnChangeHotkey);
            this.Controls.Add(this.chkStartup);
            this.Controls.Add(this.btnSave);
            this.Name = "MainForm";
            this.Text = "";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
