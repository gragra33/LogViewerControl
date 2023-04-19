<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MyMainForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.LogViewerControl = New LogViewerVB.WinForms.LogViewerControl()
        Me.SuspendLayout()
        '
        'LogViewerControl
        '
        Me.LogViewerControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LogViewerControl.Location = New System.Drawing.Point(0, 0)
        Me.LogViewerControl.Name = "LogViewerControl"
        Me.LogViewerControl.Size = New System.Drawing.Size(878, 894)
        Me.LogViewerControl.TabIndex = 0
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(10.0!, 25.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(878, 894)
        Me.Controls.Add(Me.LogViewerControl)
        Me.Name = "MainForm"
        StartPosition = FormStartPosition.CenterScreen
        Me.Text = "VB WINFORMS MINIMAL | LOG4NET LogViewer Control Example - Dot Net 7.0"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents LogViewerControl As LogViewerVB.WinForms.LogViewerControl

End Class
