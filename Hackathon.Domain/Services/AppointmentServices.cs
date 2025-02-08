using Hackathon.Contract.Contracts;
using Hackathon.Core.DTO;
using Hackathon.Core.Models;
using Hackathon.Data.Interfaces;
using Hackathon.Domain.Interfaces;
using Hackathon.Domain.Validators;
using MassTransit;

namespace Hackathon.Domain.Services
{
    public class AppointmentServices : IAppointmentServices
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IUserRepository _userRepository;
        private readonly EmailMessageSettings _emailMessageSettings;
        private readonly IPublishEndpoint _publishEndpoint;

        public AppointmentServices(IAppointmentRepository appointmentRepository, IPublishEndpoint publishEndpoint, IUserRepository userRepository, EmailMessageSettings emailMessageSettings)
        {            
            _appointmentRepository = appointmentRepository;
            _publishEndpoint = publishEndpoint;
            _userRepository = userRepository;
            _emailMessageSettings = emailMessageSettings;
        }

        public async Task Create(Appointment appointment)
        {
            var appointmentValidator = new AppointmentValidator();
            var result = appointmentValidator.Validate(appointment);

            if (!result.IsValid)
                throw new Exception(result.Errors.FirstOrDefault().ErrorMessage);

            await _appointmentRepository.Create(appointment);
        }

        public async Task Delete(Guid id)
        {
            await _appointmentRepository.Delete(id);
        }

        public async Task<IEnumerable<Appointment>> GetAll()
        {
            var appointments = await _appointmentRepository.GetAll();
            return appointments;
        }

        public async Task<IEnumerable<Appointment>> GetAll(Guid? doctorId)
        {
            var appointments = await _appointmentRepository.GetAll();

            if(doctorId != null)
                return appointments.Where(a => a.DoctorId.Equals(doctorId));

            return appointments;
        }

        public async Task<Appointment> GetById(Guid id)
        {
            return await _appointmentRepository.GetById(id);
        }

        public async Task Update(Appointment appointment)
        {
            await _appointmentRepository.Update(appointment);
        }

        public async Task Cancel(Appointment appointment, Guid patientId)
        {
            await _appointmentRepository.Update(appointment);

            var patient = await _userRepository.GetById(patientId);
            var doctor = await _userRepository.GetById(appointment.DoctorId);

            if (patient == null)
            {
                Console.WriteLine("Paciente não encontrado, a notificação não será enviada!");
                return;
            }

            var notificationMsg = new EmailNotificationMessage()
            {
                RecipientEmail = patient.Email,
                RecipientName = patient.Name,
                Subject = "Health&Med - Cancelamento de Consulta",
                Body = $"<html><head><meta charset='UTF-8'><title>Notificação de Cancelamento de Consulta</title></head><body style='font-family: Arial, sans-serif; font-size: 16px; color: #333;'><p>Olá, {patient.Name}</strong>!</p><p>Sua consulta de: {appointment.StartAt.ToShortDateString()} com o Dr. {doctor.Name} foi <strong>cancelada!</strong></p><br><p>Atenciosamente,</p><p><em>Health&Med</em></p></body></html>"
            };

            await _publishEndpoint.Publish(notificationMsg);
        }

        public async Task Notify(Appointment appointment)
        {
            if (appointment == null)
            {
                Console.WriteLine("Dados da consulta nulo, a notificação não será criada");
                return;
            }

            var doctor = await _userRepository.GetById(appointment!.DoctorId);            

            if (doctor == null)
            {
                Console.WriteLine("Médico não encontrado, a notificação não será enviada");
                return;
            }

            var patient = await _userRepository.GetById(appointment.PatientId.Value);

            if (patient == null)
            {
                Console.WriteLine("Paciente não encontrado, a notificação não será enviada");
                return;
            }

            var notificationMsg = new EmailNotificationMessage()
            {
                RecipientEmail = doctor.Email,
                RecipientName = doctor.Name,
                Body = _emailMessageSettings.Body.Replace("{nome_do_médico}", doctor.Name).Replace("{nome_do_paciente}", patient.Name).Replace("{data}", appointment.StartAt.ToString("dddd, dd MMMM yyyy")).Replace("{horário_agendado}", appointment.StartAt.ToShortTimeString()),
                Subject = _emailMessageSettings.Subject,
            };

            await _publishEndpoint.Publish(notificationMsg);
        }
    }
}