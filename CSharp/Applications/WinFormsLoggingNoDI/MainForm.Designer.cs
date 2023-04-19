namespace WinFormsLoggingNoDI
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
            LogViewerControl = new LogViewer.WinForms.LogViewerControl();
            SuspendLayout();
            // 
            // LogViewerControl
            // 
            LogViewerControl.Dock = DockStyle.Fill;
            LogViewerControl.Location = new Point(0, 0);
            LogViewerControl.Name = "LogViewerControl";
            LogViewerControl.Size = new Size(878, 894);
            LogViewerControl.TabIndex = 0;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(878, 894);
            Controls.Add(LogViewerControl);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "C# WINFORMS MINIMAL | LogViewer Control Example - Dot Net 7.0";
            ResumeLayout(false);
        }

        #endregion

        private LogViewer.WinForms.LogViewerControl LogViewerControl;
    }
}