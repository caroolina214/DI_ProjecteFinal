using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace MilAventures.View.Converters
{
    public class LowStockForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int units && units < 10)
                return new SolidColorBrush(Color.FromRgb(242, 92, 5));
            return new SolidColorBrush(Color.FromRgb(31, 31, 31));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}