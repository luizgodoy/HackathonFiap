using AutoMapper;
using Hackathon.Contract.Contracts;
using Hackathon.Core.DTO;
using Hackathon.Core.Models;

namespace Hackathon.API.AutoMapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Appointment, AppointmentDto>().ReverseMap();
            CreateMap<EditAppointmentMessage, AppointmentDto>().ReverseMap();

            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
