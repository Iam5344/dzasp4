using AutoMapper;
using UserManager.DTOs;
using UserManager.Models;

namespace UserManager.Mappings;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, UserReadDto>();
    }
}
