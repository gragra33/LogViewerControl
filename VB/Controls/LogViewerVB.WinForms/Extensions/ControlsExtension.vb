Imports System.Reflection
Imports System.Runtime.CompilerServices

Public Module ControlsExtension

    <Extension>
    Public Sub AddControl(panel As Panel, control As Control)

        panel.Controls.Add(control)
        control.Dock = DockStyle.Fill
        control.BringToFront()

    End Sub

    ' ref: https://stackoverflow.com/a/42389596
    <Extension>
    Public Sub SetDoubleBuffered(control As Control, Optional doubleBuffered As Boolean = True)
        control.[GetType]().GetProperty("DoubleBuffered", BindingFlags.Instance Or BindingFlags.NonPublic)?.SetValue(control, doubleBuffered, Nothing)
    End Sub

End Module