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
                .MinimumLength(6).WithMessage("A senha deve ter no mínimo 6 caracteres");

            RuleFor(u => u.Role)
                .NotNull().WithMessage("A função do usuário deve ser informada")
                .IsInEnum().WithMessage("O usuário deve ser 'Doctor' ou 'Patient'");

            RuleFor(u => u.CRM)
                .NotEmpty().WithMessage("O CRM é obrigatório para médicos")
                .When(u => u.Role == Role.Doctor);
        }
    }
}