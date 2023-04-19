Imports Avalonia.Controls
Imports Avalonia.Controls.Templates

Public Class ViewLocator : Implements IDataTemplate

    Public Function Build(data As Object) As IControl Implements ITemplate(Of Object, IControl).Build

        Dim name As String = data.GetType().FullName.Replace("ViewModel", "View")
        Dim type As Type = Type.GetType(name)

        If type IsNot Nothing Then
            Return DirectCast(Activator.CreateInstance(type), Control)
        End If

        Return New TextBox With {.Text = "Not Found: " + name}

    End Function

    Public Function Match(data As Object) As Boolean Implements IDataTemplate.Match

        Return TypeOf data Is ViewModelBase

    End Function

End Class