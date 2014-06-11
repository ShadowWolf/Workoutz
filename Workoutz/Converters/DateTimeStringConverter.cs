using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Workoutz.Converters
{
    class FormatTimeSpan : IFormatProvider
    {
        public object GetFormat(Type formatType)
        {
            return @"mm:ss";
        }
    }


    class MinuteSecondTimeSpanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (targetType == typeof(string) && value is TimeSpan)
            {
                TimeSpan time = (TimeSpan)value;
                return time.TotalSeconds > 0 ? time.ToString(@"mm\:ss") : null;
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (targetType == typeof(TimeSpan) && value is string && !string.IsNullOrEmpty(value.ToString()))
            {
                return TimeSpan.Parse(string.Format("00:{0}", value.ToString()));
            }

            return TimeSpan.Parse(string.Empty);
        }
    }

    class SecondsTimeSpanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (targetType == typeof(string) && value is TimeSpan)
            {
                TimeSpan time = (TimeSpan)value;
                return time.TotalSeconds > 0 ? ((TimeSpan)value).ToString(@"ss") : null;
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (targetType == typeof(TimeSpan) && value is string)
            {
                return TimeSpan.FromSeconds(double.Parse(value.ToString().Trim(':')));
            }

            return TimeSpan.Parse(string.Empty);
        }
    }
}
