using System.Globalization;
using System.Windows.Data;

namespace Morin.Wpf.Converters
{
    public class PlaylistItemsConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return $"Playlist ({values[0]}/{values[1]})";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
