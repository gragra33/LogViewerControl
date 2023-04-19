<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LogViewerControl
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.CanAutoScroll = New System.Windows.Forms.CheckBox()
        Me.ListView = New System.Windows.Forms.ListView()
        Me.colHdrTime = New System.Windows.Forms.ColumnHeader()
        Me.colHdrLevel = New System.Windows.Forms.ColumnHeader()
        Me.colHdrEventId = New System.Windows.Forms.ColumnHeader()
        Me.colHdrState = New System.Windows.Forms.ColumnHeader()
        Me.colHdrException = New System.Windows.Forms.ColumnHeader()
        Me.SuspendLayout()
        '
        'CanAutoScroll
        '
        Me.CanAutoScroll.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CanAutoScroll.AutoSize = True
        Me.CanAutoScroll.Checked = True
        Me.CanAutoScroll.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CanAutoScroll.Location = New System.Drawing.Point(30, 410)
        Me.CanAutoScroll.Name = "CanAutoScroll"
        Me.CanAutoScroll.Size = New System.Drawing.Size(160, 29)
        Me.CanAutoScroll.TabIndex = 5
        Me.CanAutoScroll.Text = "Auto Scroll Log"
        Me.CanAutoScroll.UseVisualStyleBackColor = True
        '
        'ListView
        '
        Me.ListView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ListView.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colHdrTime, Me.colHdrLevel, Me.colHdrEventId, Me.colHdrState, Me.colHdrException})
        Me.ListView.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        Me.ListView.FullRowSelect = True
        Me.ListView.GridLines = True
        Me.ListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.ListView.Location = New System.Drawing.Point(2, 8)
        Me.ListView.MultiSelect = False
        Me.ListView.Name = "ListView"
        Me.ListView.ShowGroups = False
        Me.ListView.Size = New System.Drawing.Size(1239, 388)
        Me.ListView.TabIndex = 4
        Me.ListView.UseCompatibleStateImageBehavior = False
        Me.ListView.View = System.Windows.Forms.View.Details
        '
        'colHdrTime
        '
        Me.colHdrTime.Text = "Time"
        Me.colHdrTime.Width = 200
        '
        'colHdrLevel
        '
        Me.colHdrLevel.Text = "Level"
        Me.colHdrLevel.Width = 120
        '
        'colHdrEventId
        '
        Me.colHdrEventId.Text = "Event Id"
        Me.colHdrEventId.Width = 160
        '
        'colHdrState
        '
        Me.colHdrState.Text = "State"
        Me.colHdrState.Width = 500
        '
        'colHdrException
        '
        Me.colHdrException.Text = "Exception"
        Me.colHdrException.Width = 500
        '
        'LogViewerControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(10.0!, 25.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.CanAutoScroll)
        Me.Controls.Add(Me.ListView)
        Me.Name = "LogViewerControl"
        Me.Size = New System.Drawing.Size(1242, 446)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Private WithEvents CanAutoScroll As CheckBox
    Private WithEvents ListView As ListView
    Private WithEvents colHdrTime As ColumnHeader
    Private WithEvents colHdrLevel As ColumnHeader
    Private WithEvents colHdrEventId As ColumnHeader
    Private WithEvents colHdrState As ColumnHeader
    Private WithEvents colHdrException As ColumnHeader
End Class
