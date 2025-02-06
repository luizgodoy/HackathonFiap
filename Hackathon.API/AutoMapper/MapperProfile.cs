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
            CreateMap<NewUserDto, User>()
                .ForMember(x => x.Email, opt => opt.MapFrom(x => x.Email))
                .ForMember(x => x.UserName, opt => opt.MapFrom(x => x.Email))
                .ForMember(x => x.NormalizedEmail, opt => opt.MapFrom(x => x.Email.ToUpper()))
                .ReverseMap();

            CreateMap<Specialty, SpecialtyDto>().ReverseMap();
        }
    }
}
