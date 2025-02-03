using AutoMapper;
using Hackathon.Contract.Contracts;
using Hackathon.Domain.Interfaces;
using MassTransit;

namespace Hackathon.Application.Consumers
{
    public class EditAppointmentConsumer : IConsumer<EditAppointmentMessage>
    {
        private readonly IMapper _mapper;
        private readonly IAppointmentServices _appointmentService;

        public EditAppointmentConsumer(IMapper mapper, IAppointmentServices appointmentService)
        {
            _mapper = mapper;
            _appointmentService = appointmentService;
        }

        public async Task Consume(ConsumeContext<EditAppointmentMessage> context)
        {
            // Verifica se já existe um agendamento nesse horário
            var appointment = await _appointmentService.GetById(context.Message.Id);

            if (appointment != null)
            {
                if (appointment?.PatientId == null)
                {
                    // Atualiza registro com o código do paciente
                    appointment!.Title = context.Message.Title;
                    appointment.Description = context.Message.Description;
                    appointment.PatientId = context.Message.PatientId;

                    await _appointmentService.Update(appointment);

                    // Envia notificação para o médico
                    await _appointmentService.Notify(appointment);
                }
            }
        }
    }
}