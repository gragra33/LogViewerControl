Public Module DispatcherHelper

    Public Sub Execute(action As Action)

        ' no cross-tread concerns
        If Application.OpenForms.Count = 0 Then
            action.Invoke()
            Return
        End If

        Try

            If Application.OpenForms(0).InvokeRequired Then
                ' Marshall to Main Thread
                Application.OpenForms(0).Invoke(action)
            Else
                ' no cross-tread concerns
                action.Invoke()
            End If

        Catch ex As Exception

            ' ignore as might be thrown on shutting down

        End Try

    End Sub

End Module