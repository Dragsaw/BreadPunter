using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BP.WebUI.Models
{
    public class RegisterUserModel : IValidatableObject
    {
        [Required(ErrorMessageResourceType=typeof(Resources.Resource),ErrorMessageResourceName="RequiredEmail")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessageResourceType=typeof(Resources.Resource),ErrorMessageResourceName="RequiredPassword")]
        [MinLength(5)]
        public string Password { get; set; }
        public string RepeatPassword { get; set; }
        public string Role { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> result = new List<ValidationResult>();
            if (Password != null && Password != RepeatPassword)
                result.Add(new ValidationResult(Resources.Resource.PasswordsDoNotMatch));
            else if (Password.Any(c => char.IsWhiteSpace(c)))
                result.Add(new ValidationResult(Resources.Resource.NoWhiteSpaces));
            return result;
        }
    }
}