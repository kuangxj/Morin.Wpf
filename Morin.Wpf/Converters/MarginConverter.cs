using System.Globalization;
using System.Windows.Data;

namespace Morin.Wpf.Converters
{
    public class MarginConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new System.Windows.Thickness(0, System.Convert.ToDouble(value), 0, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
