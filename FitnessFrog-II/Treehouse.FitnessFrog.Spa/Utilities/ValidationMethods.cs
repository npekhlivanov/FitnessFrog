using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Treehouse.FitnessFrog.Spa.Utilities
{
    public class ValidationMethods
    {
        public static ValidationResult ValidateGreaterThanZero(decimal value, ValidationContext context)
        {
            bool isValid = value > Decimal.Zero;
            if (isValid)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(
                    errorMessage: string.Format("The field {0} must be greater than zero.", context.MemberName), 
                    memberNames: new List<string> { context.MemberName });
            }
        }

        public static ValidationResult ValidateIntGreaterThanZero(int value, ValidationContext context)
        {
            bool isValid = value > 0;
            if (isValid)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(
                    errorMessage: string.Format("The field {0} must be greater than zero.", context.MemberName),
                    memberNames: new List<string> { context.MemberName });
            }
        }
    }
}