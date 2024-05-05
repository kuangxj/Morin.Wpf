﻿using System.Globalization;
using System.Windows.Data;

namespace Morin.Wpf.Converters
{
    public class ProgressConvert : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((TimeSpan)value).TotalSeconds;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return TimeSpan.FromSeconds((double)value);
        }
    }
}
