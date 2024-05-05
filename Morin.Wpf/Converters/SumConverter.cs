using System.Globalization;
using System.Windows.Data;

namespace Morin.Wpf.Converters
{
    public class SumConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double sum = 0;

            if (values == null) return sum;

            foreach (object value in values)
            {
                if (value == System.Windows.DependencyProperty.UnsetValue) continue;
                sum += (double)value;
            }

            return sum;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
