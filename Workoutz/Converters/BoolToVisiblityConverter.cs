using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Workoutz.Converters
{
    static class Converter
    {
        public static object ConvertValue<TSource, TTarget>(object value, Type targetType, Func<TSource, bool> comp, Func<TTarget> positive, Func<TTarget> negative)
        {
            if (targetType == typeof(TTarget) && value is TSource)
            {
                return comp((TSource)value) ? positive() : negative();
            }

            return null;
        }
    }

    [ValueConversion(typeof(bool), typeof(Visibility))]
    internal class BoolToVisiblityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Converter.ConvertValue<bool, Visibility>(value, targetType, t => (bool)t, () => Visibility.Visible, () => Visibility.Hidden);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Converter.ConvertValue<Visibility, bool>(value, targetType, t => ((Visibility)t) == Visibility.Visible, () => true, () => false);
        }
    }

    [ValueConversion(typeof(bool), typeof(Visibility))]
    internal class InvertedBoolToVisiblityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Converter.ConvertValue<bool, Visibility>(value, targetType, t => (bool)t, () => Visibility.Hidden, () => Visibility.Visible);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Converter.ConvertValue<Visibility, bool>(value, targetType, t => ((Visibility)t) == Visibility.Visible, () => false, () => true);
        }
    }
}
