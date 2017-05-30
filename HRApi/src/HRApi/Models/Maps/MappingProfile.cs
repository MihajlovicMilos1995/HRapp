using AutoMapper;
using HRApi.Models.UserDTO;
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
            CreateMap<RegUser, UserViewModel>();
            CreateMap<UserViewModel, RegUser>();
            CreateMap<RegUser, BackEndUserViewModel>();
            CreateMap<BackEndUserViewModel, RegUser>();
        }
    }
}
