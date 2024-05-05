using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Morin.Wpf.Converters
{
    [ValueConversion(typeof(string), typeof(BitmapImage))]
    public class PathToImageSourceConverter : IValueConverter
    {
        #region Converter

        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is string v)
            {

                return new BitmapImage(new Uri(v, UriKind.Relative));

            }
            return new BitmapImage(new Uri("pack://application:,,,/Morin.Wpf;component/Resources/Images/Video_Default.jpg", UriKind.Relative));
        }

        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return default(BitmapImage); ;
        }
        #endregion
    }
}
