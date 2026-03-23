using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Pract_15.Validations
{
    public class ComboBoxValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
                return new ValidationResult(false, "Выберите значение из списка");

            //if (value == null || string.IsNullOrWhiteSpace(value.ToString()) || value.ToString() == "0")
            //    return new ValidationResult(false, "Выберите значение из списка");


            return ValidationResult.ValidResult;
        }
    
    }
}

