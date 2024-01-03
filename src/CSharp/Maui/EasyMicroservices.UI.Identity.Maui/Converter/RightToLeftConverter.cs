using EasyMicroservices.UI.Cores;
using System.Globalization;

namespace EasyMicroservices.UI.Identity.Maui.Converter;
public class RightToLeftConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (BaseViewModel.IsRightToLeft)
            return FlowDirection.RightToLeft;
        return FlowDirection.LeftToRight;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (bool)value ? 1 : 0;
    }
}