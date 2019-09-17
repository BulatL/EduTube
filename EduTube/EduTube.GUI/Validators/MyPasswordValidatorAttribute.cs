using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EduTube.GUI.Validators
{
   public class MyPasswordValidatorAttribute : ValidationAttribute
   {
      protected override ValidationResult IsValid(object value, ValidationContext validationContext)
      {
         if (value.ToString().Length < 6)
         {
            return new ValidationResult("Passwords must be at least six characters");
         }

         if (!value.ToString().Any(char.IsLower))
         {
            return new ValidationResult("Passwords must contain at least 1 non uppercase character");
         }

         if (!value.ToString().Any(char.IsUpper))
         {
            return new ValidationResult("Passwords must contain at least 1 uppercase character");
         }

         return ValidationResult.Success;
      }
   }
}
