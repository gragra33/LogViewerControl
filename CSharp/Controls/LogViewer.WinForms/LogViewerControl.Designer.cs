namespace LogViewer.WinForms
{
    partial class LogViewerControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.CanAutoScroll = new System.Windows.Forms.CheckBox();
            this.ListView = new System.Windows.Forms.ListView();
            this.colHdrTime = new System.Windows.Forms.ColumnHeader();
            this.colHdrLevel = new System.Windows.Forms.ColumnHeader();
            this.colHdrEventId = new System.Windows.Forms.ColumnHeader();
            this.colHdrState = new System.Windows.Forms.ColumnHeader();
            this.colHdrException = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            // 
            // CanAutoScroll
            // 
            this.CanAutoScroll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CanAutoScroll.AutoSize = true;
            this.CanAutoScroll.Checked = true;
            this.CanAutoScroll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CanAutoScroll.Location = new System.Drawing.Point(30, 410);
            this.CanAutoScroll.Name = "CanAutoScroll";
            this.CanAutoScroll.Size = new System.Drawing.Size(160, 29);
            this.CanAutoScroll.TabIndex = 3;
            this.CanAutoScroll.Text = "Auto Scroll Log";
            this.CanAutoScroll.UseVisualStyleBackColor = true;
            // 
            // ListView
            // 
            this.ListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ListView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colHdrTime,
            this.colHdrLevel,
            this.colHdrEventId,
            this.colHdrState,
            this.colHdrException});
            this.ListView.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.ListView.FullRowSelect = true;
            this.ListView.GridLines = true;
            this.ListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.ListView.Location = new System.Drawing.Point(2, 8);
            this.ListView.MultiSelect = false;
            this.ListView.Name = "ListView";
            this.ListView.ShowGroups = false;
            this.ListView.Size = new System.Drawing.Size(1239, 388);
            this.ListView.TabIndex = 2;
            this.ListView.UseCompatibleStateImageBehavior = false;
            this.ListView.View = System.Windows.Forms.View.Details;
            // 
            // colHdrTime
            // 
            this.colHdrTime.Text = "Time";
            this.colHdrTime.Width = 200;
            // 
            // colHdrLevel
            // 
            this.colHdrLevel.Text = "Level";
            this.colHdrLevel.Width = 120;
            // 
            // colHdrEventId
            // 
            this.colHdrEventId.Text = "Event Id";
            this.colHdrEventId.Width = 160;
            // 
            // colHdrState
            // 
            this.colHdrState.Text = "State";
            this.colHdrState.Width = 500;
            // 
            // colHdrException
            // 
            this.colHdrException.Text = "Exception";
            this.colHdrException.Width = 500;
            // 
            // LogViewerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.CanAutoScroll);
            this.Controls.Add(this.ListView);
            this.Name = "LogViewerControl";
            this.Size = new System.Drawing.Size(1242, 446);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CheckBox CanAutoScroll;
        private ListView ListView;
        private ColumnHeader colHdrTime;
        private ColumnHeader colHdrLevel;
        private ColumnHeader colHdrEventId;
        private ColumnHeader colHdrState;
        private ColumnHeader colHdrException;
    }
}
