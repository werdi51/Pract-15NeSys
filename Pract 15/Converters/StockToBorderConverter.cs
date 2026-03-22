using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Pract_15.Converters
{
    public class StockToBorderConverter : IValueConverter
    {
        private static readonly SolidColorBrush WarningBrush =
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffd129"));

        private static readonly SolidColorBrush DefaultBrush = Brushes.DarkGray;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double stock = 0;
            if (value is double d) stock = d;

            if (stock < 10)
            {
                return parameter?.ToString() == "Thickness"
                    ? new Thickness(2) 
                    : WarningBrush;    
            }

            return parameter?.ToString() == "Thickness"
                ? new Thickness(0, 0, 0, 1)
                : DefaultBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
