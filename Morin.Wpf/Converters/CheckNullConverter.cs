using System.Collections;
using System.Globalization;
using System.Windows.Data;

namespace Morin.Wpf.Converters;

public class CheckNullConverter : IMultiValueConverter
{
    public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null) return true;
        if (value[0] == null || value[0] == System.Windows.DependencyProperty.UnsetValue) return true;
        if (value[1] == null || value[1] == System.Windows.DependencyProperty.UnsetValue) return true;

        return !((IDictionary)value[0]).Contains(value[1]);
    }
    public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture) { throw new NotImplementedException(); }
}
