using AutoMapper;
using Hackathon.Contract.Contracts;
using Hackathon.Core.Models;

namespace Hackathon.Application.AutoMapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Appointment, EditAppointmentMessage>().ReverseMap();
        }        
    }
}