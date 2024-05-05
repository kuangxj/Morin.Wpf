using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;

namespace Morin.Wpf.Converters
{
    public class EnumDescriptionConverter : IValueConverter
    {
        private static string GetEnumDescription(Enum enumObj)
        {
            FieldInfo fieldInfo = enumObj.GetType().GetField(enumObj.ToString());
            if (fieldInfo != null)
            {
                object[] attribArray = fieldInfo.GetCustomAttributes(false);

                if (attribArray.Length == 0)
                {
                    return enumObj.ToString();
                }
                else
                {
                    if (attribArray[0] is DescriptionAttribute attrib)
                    {
                        return attrib.Description;
                    }
                    return enumObj.ToString();
                }
            }
            return enumObj.ToString();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Enum v)
            {
                return GetEnumDescription(v);
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Empty;
        }
    }
}
