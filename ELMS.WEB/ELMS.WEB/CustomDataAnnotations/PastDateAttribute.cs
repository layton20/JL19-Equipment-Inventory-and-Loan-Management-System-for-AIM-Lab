using System;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.CustomDataAnnotations
{
    public class PastDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime Date = (DateTime)value;

            if (Date > DateTime.Now)
            {
                return new ValidationResult($"The Date field must be less than or equal to {DateTime.Now.ToString("d")}.");
            }

            return ValidationResult.Success;
        }
    }
}
