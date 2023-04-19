<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MainForm
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
        Me.HostPanel = New System.Windows.Forms.Panel()
        Me.SuspendLayout()
        '
        'HostPanel
        '
        Me.HostPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.HostPanel.Location = New System.Drawing.Point(0, 0)
        Me.HostPanel.Name = "HostPanel"
        Me.HostPanel.Size = New System.Drawing.Size(878, 894)
        Me.HostPanel.TabIndex = 0
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(10.0!, 25.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(878, 894)
        Me.Controls.Add(Me.HostPanel)
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "VB WINFORMS MVVM | LOG4NET LogViewer Control Example - Dot Net 7.0"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents HostPanel As Panel
End Class
