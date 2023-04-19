using Avalonia.Controls;
using Avalonia.Controls.Templates;
using AvaloniaLog4NetDI.ViewModels;
using System;

namespace AvaloniaLog4NetDI;

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