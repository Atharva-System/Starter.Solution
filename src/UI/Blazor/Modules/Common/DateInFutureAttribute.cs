using System.ComponentModel.DataAnnotations;

namespace Starter.Blazor.Modules.Common;

public class DateInFutureAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is DateTime dateValue)
        {
            if (dateValue.Date < DateTime.Now.Date)
            {
                return new ValidationResult(ErrorMessage);
            }
        }

        return ValidationResult.Success;
    }
}
