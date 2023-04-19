Imports System.IO
Imports Microsoft.Extensions.Configuration

Public Class AppSettings(Of TOption)

#Region "Constructors"

    Public Sub New(configSection As IConfigurationSection, Optional key As String = Nothing)

        _configSection = configSection

        GetValue(key)

    End Sub


#End Region

#Region "Fields"

    Protected Shared _appSetting As AppSettings(Of TOption)
    Protected Shared _configSection As IConfigurationSection

#End Region

#Region "Properties"

    Public Property Value As TOption

#End Region

#Region "Methods"

    Public Shared Function Current(section As String, Optional key As String = Nothing) As TOption

        _appSetting = GetCurrentSettings(section, key)
        Return _appSetting.Value

    End Function

    Public Shared Function GetCurrentSettings(section As String, Optional key As String = Nothing) As AppSettings(Of TOption)

        Dim env As String = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")
        If String.IsNullOrWhiteSpace(env) Then
            env = "Production"
        End If

        Dim builder As IConfigurationBuilder = New ConfigurationBuilder() _
                .SetBasePath(Directory.GetCurrentDirectory()) _
                .AddJsonFile("appsettings.json", optional:=True, reloadOnChange:=True) _
                .AddJsonFile($"appsettings.{env}.json", optional:=True, reloadOnChange:=True) _
                .AddEnvironmentVariables()

        Dim configuration As IConfigurationRoot = builder.Build()

        If String.IsNullOrEmpty(section) Then
            section = "AppSettings" ' Default
        End If

        Dim settings As AppSettings(Of TOption) = New AppSettings(Of TOption)(configuration.GetSection(section), key)

        Return settings

    End Function

    Protected Overridable Sub GetValue(Optional key As String = Nothing)

        If key Is Nothing Then

            ' no key, so must be a class/strut object
            Value = Activator.CreateInstance(Of TOption)
            _configSection.Bind(Value)
            Return

        End If

        Dim optionType As Type = GetType(TOption)

        If (optionType Is GetType(String) OrElse
            optionType Is GetType(Integer) OrElse
            optionType Is GetType(Long) OrElse
            optionType Is GetType(Decimal) OrElse
            optionType Is GetType(Single) OrElse
            optionType Is GetType(Double)) _
           AndAlso _configSection IsNot Nothing Then

            ' we must be retrieving a value
            Value = _configSection.GetValue(Of TOption)(key)
            Return

        End If

        ' Could not find a supported type
        Throw New InvalidCastException($"Type {GetType(TOption).Name} is invalid")

    End Sub

#End Region

End Class