using FluentValidation;
using Hackathon.Core.Models;

namespace Hackathon.Domain.Validators
{
    public class AppointmentValidator : AbstractValidator<Appointment>
    {
        public AppointmentValidator()
        {
            RuleFor(a => a.StartAt).NotEmpty().WithMessage("Data inicial de atendimento não pode ser nula");
            RuleFor(a => a.FinishAt).NotEmpty().WithMessage("Data final de atendimento não pode ser nula");
            RuleFor(a => a.FinishAt).GreaterThan(a => a.StartAt).WithMessage("A data final deve ser maior que a data inicial.");
            RuleFor(a => a.DoctorId).NotEmpty().WithMessage("O código do médico deve ser informado");
        }
    }
}