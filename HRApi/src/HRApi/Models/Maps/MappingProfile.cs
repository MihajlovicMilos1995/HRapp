using AutoMapper;
using HRApi.Models.UserViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRApi.Models.Maps
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegUser, UserDTO>();
            CreateMap<UserDTO, RegUser>();
            CreateMap<RegUser, BackEndUserDTO>();
            CreateMap<BackEndUserDTO, RegUser>();
        }
    }
}
