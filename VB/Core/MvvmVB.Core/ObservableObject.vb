Imports System.ComponentModel
Imports System.Runtime.CompilerServices

Public MustInherit Class ObservableObject : Implements INotifyPropertyChanged

    Public Sub SetValue(Of TValue) _
        (
         ByRef field As TValue,
         newValue As TValue,
         <CallerMemberName> Optional propertyName As String = ""
         )

        If field IsNot Nothing Then
            If Not EqualityComparer(Of TValue).Default.Equals(field, Nothing) And
               field.Equals(newValue) Then
                Return
            End If
        End If

        field = newValue

        OnPropertyChanged(propertyName)

    End Sub

    Public Event PropertyChanged As PropertyChangedEventHandler _
        Implements INotifyPropertyChanged.PropertyChanged

    Public Sub OnPropertyChanged(propertyName As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
    End Sub

End Class