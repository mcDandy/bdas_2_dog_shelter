﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BDAS_2_dog_shelter.Validation
{
    public class NotEmptyStringRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            return new ValidationResult(!string.IsNullOrWhiteSpace(value as string), "Empty string");
        }
    }
    public class NumericStringRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            return new ValidationResult(value is string && ((string)value).Trim() != string.Empty && Int32.TryParse(value as string,out int i), "Empty string or not a number");
        }
    }
}