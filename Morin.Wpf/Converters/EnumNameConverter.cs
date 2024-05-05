using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Data;

namespace Morin.Wpf.Converters
{
    public class EnumNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                Assembly assem = Assembly.GetExecutingAssembly();
                Type type = assem.GetType(parameter.ToString());
                return Enum.Parse(type, value.ToString());
            }
            return DependencyProperty.UnsetValue;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
