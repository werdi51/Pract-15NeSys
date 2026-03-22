using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Pract_15.Validations
{
    public class RatingDataValidations : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string input = value?.ToString().Trim();

            if (string.IsNullOrEmpty(input))
                return new ValidationResult(false, "Рейтинг не может быть пустым");

            if (!double.TryParse(input, NumberStyles.Any, cultureInfo, out double rating))
                return new ValidationResult(false, "Введите корректное число ");

            if (rating < 0 || rating > 5)
                return new ValidationResult(false, "Рейтинг должен быть от 0 до 5");

            return ValidationResult.ValidResult;
        }
    }
}
