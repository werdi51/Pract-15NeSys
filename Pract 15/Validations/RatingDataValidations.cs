using System;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;

namespace Pract_15.Validations
{
    public class RatingDataValidations : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string input = value?.ToString().Trim();

            if (string.IsNullOrEmpty(input))
                return new ValidationResult(false, "Поле не может быть пустым");

            int separatorsCount = 0;
            foreach (char c in input)
            {
                if (char.IsDigit(c)) continue; 

                if (c == '.' || c == ',')
                {
                    separatorsCount++;
                    continue;
                }

                return new ValidationResult(false, "Разрешены только цифры и точка/запятая");
            }

            if (separatorsCount > 1)
                return new ValidationResult(false, "Слишком много точек или запятых");

            if (double.Parse(input) < 1 || double.Parse(input) > 5)
            {
                return new ValidationResult(false, "не меньше 1 и не больше 5");
            }

            return ValidationResult.ValidResult;
        }
    }
}