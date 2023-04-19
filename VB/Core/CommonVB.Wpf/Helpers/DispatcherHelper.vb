Imports System.Windows
Imports System.Windows.Threading

Public Module DispatcherHelper

    Public Sub Execute(action As Action)

        If Application.Current Is Nothing OrElse Application.Current.Dispatcher Is Nothing Then
            Return
        End If

        ' Marshall to Main Thread
        Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, action)

    End Sub

End Module