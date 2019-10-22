using System;
using System.ComponentModel.DataAnnotations;

namespace EC.Models
{
    public class AgeValidation : ValidationAttribute
    {
       

            
            public AgeValidation() : base("Must be older than 18 years old.")
            {
            
            }

            public override bool IsValid(object value)
            {
                int year = ((DateTime)value).Year;
                if (DateTime.Now.Year - year < 18)
                    return false;
                return true;
            }

            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                var user = (User)validationContext.ObjectInstance;

                if (DateTime.Now.Year - user.Dob.Year < 18)
                {
                    return new ValidationResult(base.ErrorMessage);
                }

                return ValidationResult.Success;
            }
        
    }
}