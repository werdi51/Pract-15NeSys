using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Pract_15.Validations
{
    public class StockDataValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var input = (value ?? "").ToString().Trim();

            if (string.IsNullOrEmpty(input))
            {
                return new ValidationResult(false, "колво не может быть пустым");
            }

            if (!double.TryParse(input, NumberStyles.Any, cultureInfo, out double price))
            {
                return new ValidationResult(false, "Введите корректное число");
            }

            if (price < 0)
            {
                return new ValidationResult(false, "колво не может быть отрицательной");
            }
            return ValidationResult.ValidResult;
        }
    }
}
