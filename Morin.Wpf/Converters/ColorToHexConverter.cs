using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Morin.Wpf.Converters
{
    public class ColorToHexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null) return null;

            string lowerHexString(int i) => i.ToString("X2").ToLower();
            Color color = (Color)value;
            var hex = lowerHexString(color.R) +
                      lowerHexString(color.G) +
                      lowerHexString(color.B);
            return hex;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return ColorConverter.ConvertFromString("#" + value.ToString());
            }
            catch (Exception) { }

            return Binding.DoNothing;
        }

    }
}
