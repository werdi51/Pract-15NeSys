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
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            long stock = System.Convert.ToInt64(value);

            //if (stock <= 10)
            //{
            //    return Brushes.(#ffd129);
            //}

            if (stock <= 10)
            {
                var bc = new BrushConverter();
                return (Brush)bc.ConvertFrom("#ffd129");
            }

            return Brushes.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
