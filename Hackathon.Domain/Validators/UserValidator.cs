using FluentValidation;
using Hackathon.Core.Models;

namespace Hackathon.Domain.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(u => u.Name)
                .NotEmpty().WithMessage("O nome do usuário é obrigatório")
                .MaximumLength(100).WithMessage("O nome deve ter no máximo 100 caracteres");

            RuleFor(u => u.CPF)
                .NotEmpty().WithMessage("O CPF é obrigatório")
                .Matches(@"^\d{11}$").WithMessage("O CPF deve conter 11 dígitos numéricos");

            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("O e-mail é obrigatório")
                .EmailAddress().WithMessage("O e-mail informado não é válido");

            RuleFor(u => u.Password)
                .NotEmpty().WithMessage("A senha é obrigatória")
                .Matches(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d@#$%^&+=!]{6,}$")
                .WithMessage("A senha deve conter pelo menos uma letra, um número e um caractere especial, com no mínimo 6 caracteres");

            RuleFor(u => u.Role)
                .IsInEnum().WithMessage("O valor da função do usuário é inválido");

            RuleFor(u => u.CRM)
                .NotEmpty().WithMessage("O CRM é obrigatório para médicos")
                .When(u => u.Role == Role.Doctor);

            RuleFor(u => u.CRM)
                .Empty().WithMessage("Pacientes não devem possuir CRM")
                .When(u => u.Role == Role.Patient);
        }
    }
}
