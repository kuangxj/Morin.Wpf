﻿using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Morin.Wpf.Converters
{
    public class TimeSpanFormatter : IValueConverter
    {
        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TimeSpan? p;

            switch (value)
            {
                case TimeSpan position:
                    p = position;
                    break;
                case Duration duration when duration.HasTimeSpan:
                    p = duration.TimeSpan;
                    break;
                default:
                    return string.Empty;
            }

            if (p.Value == TimeSpan.MinValue)
                return "N/A";

            return $"{(int)p.Value.TotalHours:00}:{p.Value.Minutes:00}:{p.Value.Seconds:00}.{p.Value.Milliseconds:000}";
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
}
