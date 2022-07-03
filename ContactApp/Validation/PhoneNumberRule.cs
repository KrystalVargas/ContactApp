using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace ContactApp.Validation
{
    class PhoneNumberRule : ValidationRule
    {
        public PhoneNumberRule() { }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (phoneNumber.IsMatch(value.ToString()))
                return new ValidationResult(false, "Only Numbers Allowed");
            else
                return ValidationResult.ValidResult;
        }

        Regex phoneNumber = new Regex("[^0-9]");
    }
}
