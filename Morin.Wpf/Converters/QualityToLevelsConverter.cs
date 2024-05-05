using System.Globalization;
using System.Windows.Data;

namespace Morin.Wpf.Converters;

public class QualityToLevelsConverter : IValueConverter
{
    public enum Qualities
    {
        None,
        Low,
        Med,
        High,
        _4k
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        int videoHeight = (int)value;

        if (videoHeight > 1080)
            return Qualities._4k;
        else if (videoHeight > 720)
            return Qualities.High;
        else if (videoHeight == 720)
            return Qualities.Med;
        else
            return Qualities.Low;
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { throw new NotImplementedException(); }
}
