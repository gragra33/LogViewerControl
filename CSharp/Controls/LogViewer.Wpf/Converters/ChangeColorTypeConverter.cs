using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using SysDrawColor = System.Drawing.Color;

namespace LogViewer.Wpf.Converters;

public class ChangeColorTypeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        SysDrawColor sysDrawColor = (SysDrawColor)value;
        return new SolidColorBrush(Color.FromArgb(
            sysDrawColor.A,
            sysDrawColor.R,
            sysDrawColor.G,
            sysDrawColor.B));
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}