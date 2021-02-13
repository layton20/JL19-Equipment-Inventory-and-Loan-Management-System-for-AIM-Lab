using System;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.CustomDataAnnotations
{
    public class FutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime Date = (DateTime)value;

            if (Date <= DateTime.Now.Date)
            {
                return new ValidationResult($"The {validationContext.DisplayName} field must be greater than {DateTime.Now.ToString("d")}.");
            }

            return ValidationResult.Success;
        }
    }
}