using System.Globalization;
using System.Windows.Data;

namespace Morin.Wpf.Converters
{
    public class VolumeToLevelsConverter : IValueConverter
    {
        public enum Volumes
        {
            Mute,
            Low,
            Med,
            High
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int volume = (int)value;

            if (volume == 0)
                return Volumes.Mute;
            else if (volume > 99)
                return Volumes.High;
            else if (volume > 49)
                return Volumes.Med;
            else
                return Volumes.Low;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { throw new NotImplementedException(); }
    }
}
