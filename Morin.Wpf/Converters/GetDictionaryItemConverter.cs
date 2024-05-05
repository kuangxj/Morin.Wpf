using System.Collections;
using System.Globalization;
using System.Windows.Data;

namespace Morin.Wpf.Converters
{
    public class GetDictionaryItemConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            if (value[0] == null || value[0] == System.Windows.DependencyProperty.UnsetValue) return null;
            if (value[1] == null || value[1] == System.Windows.DependencyProperty.UnsetValue) return null;

            return ((IDictionary)value[0])[value[1]];
        }
        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture) { throw new NotImplementedException(); }
    }
}
