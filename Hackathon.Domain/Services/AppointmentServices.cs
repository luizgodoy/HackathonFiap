using Azure.Identity;
using Hackathon.Contract.Contracts;
using Hackathon.Core.DTO;
using Hackathon.Core.Models;
using Hackathon.Data.Interfaces;
using Hackathon.Domain.Interfaces;
using Hackathon.Domain.Validators;
using MassTransit;
using System.Net.Mail;

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

            var notificationMsg = new EmailNotificationMessage()
            {
                RecipientEmail = doctor.Email,
                RecipientName = doctor.Name,
                Body = _emailMessageSettings.Body,
                Subject = _emailMessageSettings.Subject,
            };

            await _publishEndpoint.Publish(notificationMsg);
        }
    }
}