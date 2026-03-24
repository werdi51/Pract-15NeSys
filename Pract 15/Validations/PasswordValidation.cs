using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Pract_15.Validations
{
    public class PasswordValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var input = (value ?? "").ToString().Trim();

            if (string.IsNullOrEmpty(input))
            {
                return new ValidationResult(false, "пароль не может быть пустым");
            }

            foreach (char c in input)
            {
                if (char.IsDigit(c)) continue;

                return new ValidationResult(false, "числа");
            }

            return ValidationResult.ValidResult;
        }
    }
}
