using MahApps.Metro.IconPacks;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace MilAventures.View.Converters
{
    public class BoolToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isActive = value is bool b && b;
            return new PackIconMaterial
            {
                Kind = isActive ? PackIconMaterialKind.Check : PackIconMaterialKind.Close,
                Width = 16,
                Height = 16,
                Foreground = isActive
                    ? new SolidColorBrush(Color.FromRgb(102, 115, 2))
                    : new SolidColorBrush(Color.FromRgb(242, 92, 5))
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}