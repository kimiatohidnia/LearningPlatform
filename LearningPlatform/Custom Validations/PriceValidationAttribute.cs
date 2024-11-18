using LearningPlatform.Models;
using System.ComponentModel.DataAnnotations;

public class PriceValidationAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var course = (Course)validationContext.ObjectInstance;

        if (!course.IsFree && (course.Price == null || course.Price <= 0))
        {
            return new ValidationResult("Price is required and must be greater than 0 when the course is not free.");
        }

        return ValidationResult.Success;
    }
}
