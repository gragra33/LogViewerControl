namespace WinFormsLoggingDI
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.HostPanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // HostPanel
            // 
            this.HostPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HostPanel.Location = new System.Drawing.Point(0, 0);
            this.HostPanel.Name = "HostPanel";
            this.HostPanel.Size = new System.Drawing.Size(878, 894);
            this.HostPanel.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(878, 894);
            this.Controls.Add(this.HostPanel);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "C# WINFORMS MVVM | LogViewer Control Example - Dot Net 7.0";
            this.ResumeLayout(false);

        }

        #endregion

        private Panel HostPanel;
    }
}