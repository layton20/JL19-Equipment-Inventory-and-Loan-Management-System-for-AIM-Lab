using System;
using System.ComponentModel.DataAnnotations;

namespace ELMS.WEB.CustomDataAnnotations
{
    public class NonDefaultGuidAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Guid uid = (Guid)value;

            if (uid == Guid.Empty)
            {
                return new ValidationResult($"The {validationContext.DisplayName} field must be selected.");
            }

            return ValidationResult.Success;
        }
    }
}
