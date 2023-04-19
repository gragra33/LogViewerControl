using Avalonia.Controls;
using Avalonia.Controls.Templates;
using AvaloniaLoggingDI.ViewModels;
using System;

namespace AvaloniaLoggingDI;

public class ViewLocator : IDataTemplate
{
    public IControl Build(object data)
    {
        string name = data.GetType().FullName!.Replace("ViewModel", "View");
        Type? type = Type.GetType(name);

        if (type != null)
        {
            return (Control)Activator.CreateInstance(type)!;
        }

        return new TextBlock { Text = "Not Found: " + name };
    }

    public bool Match(object data)
    {
        return data is ViewModelBase;
    }
}