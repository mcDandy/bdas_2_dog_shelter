using System.Text.RegularExpressions;
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
    public partial class EmailOrNull : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            return new ValidationResult(value is string && (EmailRegex().IsMatch(((string)value).Trim()) || ((string)value).Trim() is ""),"Not an number address or empty");
        }

        [GeneratedRegex("[a-zA-Z0-9.]+@[a-zA-Z0-9.]+[.][a-zA-Z0-9]+")]
        private static partial Regex EmailRegex();
    }
    public partial class TimeRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            return new ValidationResult(value is string && RegexClock().IsMatch(((string)value).Trim()), "Empty string or not a number");
        }

        [GeneratedRegex("([2]?[0-4]?)|([0-1][0-9]):[0-5]?[0-9](:[0-5]?[0-9])?")]
        public static partial Regex RegexClock();
    }
}
