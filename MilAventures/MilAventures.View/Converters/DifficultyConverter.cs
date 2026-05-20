using System;
using System.Globalization;
using System.Windows.Data;

namespace MilAventures.View.Converters
{
    public class DifficultyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is int diff)) return value?.ToString();
            switch (diff)
            {
                case 1: return "Fàcil";
                case 2: return "Principiant";
                case 3: return "Mitjà";
                case 4: return "Avançat";
                case 5: return "Expert";
                default: return value.ToString();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}