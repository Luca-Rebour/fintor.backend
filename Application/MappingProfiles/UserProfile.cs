using Application.DTOs.Users;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.MappingProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDTO>();

            CreateMap<CreateUserDTO, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForPath(dest => dest.BaseCurrency.Code,
                    opt => opt.MapFrom(src => src.BaseCurrencyCode));

            CreateMap<User, CreateUserResponseDTO>();
        }
    }
}
