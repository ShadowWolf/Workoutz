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
                return time.TotalSeconds > 0 ? ToDisplayString(time) : null;
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (targetType == typeof(TimeSpan) && value is string && !string.IsNullOrEmpty(value.ToString()))
            {
                var parseValue = value.ToString();
                if (parseValue.StartsWith(":"))
                {
                    parseValue = string.Format("0{0}", parseValue);
                }

                TimeSpan timeSpan;
                if (TimeSpan.TryParse(string.Format("00:{0}", parseValue), out timeSpan))
                {
                    return timeSpan;
                }
            }

            return TimeSpan.Zero;
        }

        private string ToDisplayString(TimeSpan ts)
        {
            var parts = new List<string>(3);
            if (ts.Hours > 0)
            {
                parts.Add(ts.Hours.ToString());
            }

            if (ts.Minutes > 0)
            {
                parts.Add(ts.Minutes.ToString());
            }
            else
            {
                parts.Add("0");
            }

            if (ts.Seconds > 0)
            {
                parts.Add(ts.Seconds.ToString());
            }
            else
            {
                parts.Add("00");
            }

            return string.Join(":", parts);
        }
    }

    class SecondsTimeSpanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (targetType == typeof(string) && value is TimeSpan)
            {
                TimeSpan time = (TimeSpan)value;
                return time.TotalSeconds > 0 ? time.TotalSeconds.ToString() : null;
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
