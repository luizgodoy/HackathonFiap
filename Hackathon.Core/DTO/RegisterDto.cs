using Hackathon.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon.Core.DTO
{
    public class RegisterDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string CPF { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public Role Role { get; set; }

        [CRMValidator]
        public string CRM { get; set; } = string.Empty;


        public class CRMValidator : ValidationAttribute
        {
            protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
            {
                var model = (RegisterDto)validationContext.ObjectInstance;

                if (model.Role == Role.Doctor && string.IsNullOrEmpty((string?)value))
                {
                    return new ValidationResult("O CRM é obrigatório");
                }

                return ValidationResult.Success;
            }

        }
    }
}
