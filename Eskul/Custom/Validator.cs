using System.ComponentModel.DataAnnotations;

namespace Eskul.Custom
{
    public class Validators:ValidationAttribute
    {
        protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
        {
            if (value!=null)
            {
             string Check=value.ToString(); 
                if (!string.IsNullOrEmpty(Check))
                {
                    return ValidationResult.Success;
                }
            }
            return new ValidationResult(ErrorMessage?? "Required");
        }
    }
}
